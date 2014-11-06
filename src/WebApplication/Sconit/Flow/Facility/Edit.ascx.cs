using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Procurement;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Entity;
using NHibernate.Expression;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Utility;
using com.Sconit.Entity.Customize;

public partial class MasterData_Facility_Edit : EditModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler EditEvent;
    public string ModuleType
    {
        get
        {
            return (string)ViewState["ModuleType"];
        }
        set
        {
            ViewState["ModuleType"] = value;
        }
    }

    public string FlowCode
    {
        get
        {
            return (string)ViewState["FlowCode"];
        }
        set
        {
            ViewState["FlowCode"] = value;
        }
    }

    public String Id
    {
        get
        {
            if (ViewState["Id"] == null)
            {
                return "0";
            }
            else
            {
                return (String)ViewState["Id"];
            }
        }
        set
        {
            ViewState["Id"] = value;
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    protected void FV_ProductLineFacility_DataBound(object sender, EventArgs e)
    {
        ProductLineFacility productLineFacility = (ProductLineFacility)((FormView)sender).DataItem;

        Controls_TextBox tbRouting = (Controls_TextBox)FV_ProductLineFacility.FindControl("tbRouting");
        if (productLineFacility.Routing != null)
        {
            tbRouting.Text = productLineFacility.Routing.Code;
        }
    }

    public void InitPageParameter(int id)
    {
        this.Id = Convert.ToString(id);
        this.ODS_ProductLineFacility.SelectParameters["id"].DefaultValue = this.Id.ToString();
        this.ODS_ProductLineFacility.DeleteParameters["id"].DefaultValue = this.Id.ToString();
        this.ODS_ProductLineFacility.DataBind();
    }

    protected void ODS_ProductLineFacility_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {

        ProductLineFacility productLineFacility = (ProductLineFacility)e.InputParameters[0];
        productLineFacility.Code = productLineFacility.Code.Trim();
        productLineFacility.ProductLine = FlowCode;
        Controls_TextBox tbRouting = (Controls_TextBox)FV_ProductLineFacility.FindControl("tbRouting");
        if (tbRouting != null && tbRouting.Text.Trim() != string.Empty)
        {
            productLineFacility.Routing = TheRoutingMgr.LoadRouting(tbRouting.Text.Trim());
        }
        if (productLineFacility.IsActive == null)
        {
            productLineFacility.IsActive = false;
        }
    }

    protected void ODS_ProductLineFacility_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (EditEvent != null)
        {
            EditEvent(((TextBox)(this.FV_ProductLineFacility.FindControl("tbCode"))).Text, e);

            ShowSuccessMessage("MasterData.Flow.Facility.UpdateProductLineFacility.Successfully", ((TextBox)(this.FV_ProductLineFacility.FindControl("tbCode"))).Text);
        }
    }

    protected void ODS_ProductLineFacility_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        btnBack_Click(this, e);
        ShowSuccessMessage("MasterData.Flow.Facility.DeleteProductLineFacility.Successfully", ((TextBox)(this.FV_ProductLineFacility.FindControl("tbCode"))).Text);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            this.Visible = false;
        }
    }

    protected void checkCodeExists(object source, ServerValidateEventArgs args)
    {

    }

    private void PageCleanup()
    {
        ((TextBox)this.FV_ProductLineFacility.FindControl("tbCode")).Text = string.Empty;
        ((CheckBox)(this.FV_ProductLineFacility.FindControl("cbIsActive"))).Checked = true;
    }
}

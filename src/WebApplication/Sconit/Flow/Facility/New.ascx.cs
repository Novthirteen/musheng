using System;
using System.Collections;
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
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Service.Ext.Procurement;
using com.Sconit.Entity;
using NHibernate.Expression;
using com.Sconit.Service.Ext.Criteria;
using System.Collections.Generic;
using com.Sconit.Utility;
using com.Sconit.Entity.Customize;

public partial class MasterData_Facility_New : ModuleBase
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

    private ProductLineFacility productLineFacility;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void InitPageParameter(string flowCode)
    {
        this.FlowCode = flowCode;
        PageCleanup();
    }

    protected void ODS_ProductLineFacility_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        productLineFacility = (ProductLineFacility)e.InputParameters[0];
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

    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.Visible = false;
    }

    protected void ODS_ProductLineFacility_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {

        if (EditEvent != null)
        {
            EditEvent(productLineFacility.Code, e);

            ShowSuccessMessage("MasterData.Flow.Facility.AddProductLineFacility.Successfully", productLineFacility.Code);

        }

    }

    protected void checkCodeExists(object source, ServerValidateEventArgs args)
    {
        String code = ((TextBox)(this.FV_ProductLineFacility.FindControl("tbCode"))).Text.Trim();
        if (string.IsNullOrEmpty(((TextBox)(this.FV_ProductLineFacility.FindControl("txtPointTime"))).Text.Trim()))
        {
            args.IsValid = false;
            this.ShowErrorMessage("贴片时间必须填入数字");
        }
        ProductLineFacility productLineFacility = TheProductLineFacilityMgr.GetPLFacility(code);
        
        if (productLineFacility != null)
        {
            args.IsValid = false;
            this.ShowErrorMessage("MasterData.Flow.Facility.Code.Exist", code);
        }
    }

    private void PageCleanup()
    {
        ((TextBox)this.FV_ProductLineFacility.FindControl("tbCode")).Text = string.Empty;
        ((CheckBox)(this.FV_ProductLineFacility.FindControl("cbIsActive"))).Checked = true;

    }

}

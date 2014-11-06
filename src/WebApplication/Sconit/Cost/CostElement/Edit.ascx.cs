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
using com.Sconit.Control;
using com.Sconit.Utility;
using com.Sconit.Entity.Cost;

public partial class Cost_CostElement_Edit : EditModuleBase
{
    public event EventHandler BackEvent;

    protected string Code
    {
        get
        {
            return (string)ViewState["Code"];
        }
        set
        {
            ViewState["Code"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }


    public void InitPageParameter(string code)
    {
        this.Code = code;
        this.ODS_CostElement.SelectParameters["code"].DefaultValue = this.Code;
        this.ODS_CostElement.DataBind();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }
    protected void FV_CostElement_DataBound(object sender, EventArgs e)
    {
        CostElement costElement = (CostElement)((FormView)sender).DataItem;

        com.Sconit.Control.CodeMstrDropDownList ddlCategory = (com.Sconit.Control.CodeMstrDropDownList)this.FV_CostElement.FindControl("ddlCategory");

        if (costElement.Category != null)
        {
            ddlCategory.Text = costElement.Category;
        }
    }
    protected void ODS_CostElement_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        CostElement costElement = (CostElement)e.InputParameters[0];
        costElement.Category = ((com.Sconit.Control.CodeMstrDropDownList)(this.FV_CostElement.FindControl("ddlCategory"))).SelectedValue;
    }
    protected void ODS_CostElement_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ShowSuccessMessage("Cost.CostElement.Update.Successfully");
    }

    protected void ODS_CostElement_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            TheCostElementMgr.DeleteCostElement(this.Code);
            ShowSuccessMessage("Cost.CostElement.Delete.Successfully");
            if (BackEvent != null)
            {
                BackEvent(this, e);
            }
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Cost.CostElement.Delete.Failed");
        }
    }

}

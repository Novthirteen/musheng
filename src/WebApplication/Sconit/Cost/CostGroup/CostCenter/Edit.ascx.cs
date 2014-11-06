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

public partial class Cost_CostCenter_Edit : EditModuleBase
{
    public event EventHandler BackEvent;

    public string CostGroupCode
    {
        get
        {
            return (string)ViewState["CostGroupCode"];
        }
        set
        {
            ViewState["CostGroupCode"] = value;
        }
    }

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
        this.ODS_CostCenter.SelectParameters["code"].DefaultValue = this.Code;
        this.ODS_CostCenter.DataBind();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void ODS_CostCenter_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        CostCenter costCenter = (CostCenter)e.InputParameters[0];
        costCenter.CostGroup = TheCostGroupMgr.LoadCostGroup(this.CostGroupCode);
    }
    protected void ODS_CostCenter_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ShowSuccessMessage("Cost.CostCenter.Update.Successfully");
    }

    protected void ODS_CostCenter_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            TheCostCenterMgr.DeleteCostCenter(this.Code);
            ShowSuccessMessage("Cost.CostCenter.Delete.Successfully");
            if (BackEvent != null)
            {
                BackEvent(this, e);
            }
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Cost.CostCenter.Delete.Failed");
        }
    }

}

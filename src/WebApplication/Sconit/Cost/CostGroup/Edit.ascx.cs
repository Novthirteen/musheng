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
using System.IO;
using com.Sconit.Utility;

public partial class Cost_CostGroup_Edit : EditModuleBase
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
        this.ODS_CostGroup.SelectParameters["code"].DefaultValue = this.Code;
        this.ODS_CostGroup.DataBind();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void ODS_CostGroup_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ShowSuccessMessage("Cost.CostGroup.Update.Successfully", this.Code);
    }


    protected void ODS_CostGroup_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            TheCostGroupMgr.DeleteCostGroup(this.Code);
            ShowSuccessMessage("Cost.CostGroup.Update.Successfully", this.Code);
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Cost.CostGroup.Update.Failed", this.Code);
        }
    }

   
}

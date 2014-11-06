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
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Web;
using com.Sconit.Utility;
using com.Sconit.Entity.Cost;

public partial class Cost_CostCenter_New : NewModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler CreateEvent;

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

    protected void Page_Load(object sender, EventArgs e)
    {
      
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    public void PageCleanup()
    {
        ((TextBox)(this.FV_CostCenter.FindControl("tbCode"))).Text = string.Empty;

        ((CheckBox)(this.FV_CostCenter.FindControl("tbIsActive"))).Checked = true;
        ((TextBox)(this.FV_CostCenter.FindControl("tbDescription"))).Text = string.Empty;
     
    }

    protected void ODS_CostCenter_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        CostCenter costCenter = (CostCenter)e.InputParameters[0];
        costCenter.CostGroup = TheCostGroupMgr.LoadCostGroup(this.CostGroupCode);
       
    }

   
    protected void ODS_CostCenter_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ShowSuccessMessage("Cost.CostCenter.Add.Successfully");
        if (CreateEvent != null)
        {
            string code = ((TextBox)(this.FV_CostCenter.FindControl("tbCode"))).Text;
            CreateEvent(code, e);
        }
    }

    protected void checkCostCenterExists(object source, ServerValidateEventArgs args)
    {
        string code = ((TextBox)(this.FV_CostCenter.FindControl("tbCode"))).Text;

        if (TheCostCenterMgr.LoadCostCenter(code) != null)
        {
            args.IsValid = false;
        }
    }

  
}

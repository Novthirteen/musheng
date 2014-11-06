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

public partial class Cost_CostGroup_New : NewModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler CreateEvent;

 

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
        ((TextBox)(this.FV_CostGroup.FindControl("tbCode"))).Text = string.Empty;
        ((CheckBox)(this.FV_CostGroup.FindControl("tbIsActive"))).Checked = true;
        ((TextBox)(this.FV_CostGroup.FindControl("tbDescription"))).Text = string.Empty;
    }

   

    protected void ODS_CostGroup_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        
            ShowSuccessMessage("Cost.CostGroup.Add.Successfully");
       
    }

    protected void CheckCostGroupExists(object source, ServerValidateEventArgs args)
    {
        string code = ((TextBox)(this.FV_CostGroup.FindControl("tbCode"))).Text;

        if (TheCostGroupMgr.LoadCostGroup(code) != null)
        {
            args.IsValid = false;
        }
    }

 
}

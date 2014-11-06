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

public partial class Cost_StandardCost_New : NewModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler CreateEvent;

    private StandardCost standardCost;
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
        ((Controls_TextBox)(this.FV_StandardCost.FindControl("tbItem"))).Text = string.Empty;
        ((Controls_TextBox)(this.FV_StandardCost.FindControl("tbCostElement"))).Text = string.Empty;
        ((Controls_TextBox)(this.FV_StandardCost.FindControl("tbCostGroup"))).Text = string.Empty;
        ((TextBox)(this.FV_StandardCost.FindControl("tbCost"))).Text = string.Empty;
    }

    protected void ODS_StandardCost_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        standardCost = (StandardCost)e.InputParameters[0];
        string item = ((Controls_TextBox)(this.FV_StandardCost.FindControl("tbItem"))).Text.Trim();
        if (item != string.Empty)
        {
            standardCost.Item = item;
        }
        string costElement = ((Controls_TextBox)(this.FV_StandardCost.FindControl("tbCostElement"))).Text.Trim();
        if (costElement != string.Empty)
        {
            standardCost.CostElement = TheCostElementMgr.LoadCostElement(costElement);
        }
        string costGroup = ((Controls_TextBox)(this.FV_StandardCost.FindControl("tbCostGroup"))).Text.Trim();
        if (costGroup != string.Empty)
        {
            standardCost.CostGroup = TheCostGroupMgr.LoadCostGroup(costGroup);
        }

        standardCost.CreateDate = DateTime.Now;
        standardCost.CreateUser = this.CurrentUser;
        standardCost.LastModifyDate = DateTime.Now;
        standardCost.LastModifyUser = this.CurrentUser;
    }


    protected void ODS_StandardCost_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ShowSuccessMessage("Cost.StandardCost.Add.Successfully");
        if (CreateEvent != null)
        {
            CreateEvent(standardCost.Id, e);
        }
    }
    protected void checkStandardCostExists(object source, ServerValidateEventArgs args)
    {
        Controls_TextBox tbItem = this.FV_StandardCost.FindControl("tbItem") as Controls_TextBox;
        Controls_TextBox tbCostElement = this.FV_StandardCost.FindControl("tbCostElement") as Controls_TextBox;
        Controls_TextBox tbCostGroup = this.FV_StandardCost.FindControl("tbCostGroup") as Controls_TextBox;

        if (tbItem.Text.Trim() == string.Empty || tbCostElement.Text.Trim() == string.Empty || tbCostGroup.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            return;
        }
        Item item = TheItemMgr.LoadItem(tbItem.Text.Trim());
        CostElement costElement = TheCostElementMgr.LoadCostElement(tbCostElement.Text.Trim());
        CostGroup costGroup = TheCostGroupMgr.LoadCostGroup(tbCostGroup.Text.Trim());

        if (TheStandardCostMgr.FindStandardCost(item, costElement, costGroup) != null)
        {
            args.IsValid = false;
            return;
        }
    }
}

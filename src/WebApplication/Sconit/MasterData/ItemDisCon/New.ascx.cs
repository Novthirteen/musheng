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
using com.Sconit.Control;

public partial class MasterData_ItemDisCon_New : NewModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler CreateEvent;
    public event EventHandler NewEvent;

    private ItemDiscontinue itemDiscontinue;
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

    protected void FV_ItemDisCon_OnDataBinding(object sender, EventArgs e)
    {

    }

    protected void checkItemExists(object source, ServerValidateEventArgs args)
    {
        string code = ((Controls_TextBox)(this.FV_ItemDisCon.FindControl("tdItem"))).Text;

        if (TheItemMgr.LoadItem(code) == null || TheItemMgr.LoadItem(code).Equals(""))
        {
            ShowErrorMessage("MasterData.ItemDisCon.CodeExist1", code);
            args.IsValid = false;
        }
    }

    protected void checkItemExists1(object source, ServerValidateEventArgs args)
    {
        string code = ((Controls_TextBox)(this.FV_ItemDisCon.FindControl("tdDiscontinueItem"))).Text;

        if (TheItemMgr.LoadItem(code) == null || TheItemMgr.LoadItem(code).Equals(""))
        {
            ShowErrorMessage("MasterData.ItemDisCon.CodeExist2", code);
            args.IsValid = false;
        }
    }

    public void PageCleanup()
    {
        ((TextBox)(this.FV_ItemDisCon.FindControl("tdBom"))).Text = string.Empty;
        ((Controls_TextBox)(this.FV_ItemDisCon.FindControl("tdItem"))).Text = string.Empty;
        ((Controls_TextBox)(this.FV_ItemDisCon.FindControl("tdDiscontinueItem"))).Text = string.Empty;
        ((TextBox)(this.FV_ItemDisCon.FindControl("tbUnitQty"))).Text = string.Empty;
        ((TextBox)(this.FV_ItemDisCon.FindControl("tbPriority"))).Text = string.Empty;
        ((TextBox)(this.FV_ItemDisCon.FindControl("tbStartDate"))).Text = string.Empty;
        ((TextBox)(this.FV_ItemDisCon.FindControl("tbEndDate"))).Text = string.Empty;
    }

    protected void ODS_ItemDisCon_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        itemDiscontinue = (ItemDiscontinue)e.InputParameters[0];
        string bom = ((TextBox)(this.FV_ItemDisCon.FindControl("tdBom"))).Text.Trim();
        itemDiscontinue.Bom = TheBomMgr.LoadBom(bom);
        string item = ((Controls_TextBox)(this.FV_ItemDisCon.FindControl("tdItem"))).Text.Trim();
        itemDiscontinue.Item = TheItemMgr.LoadItem(item);
        string discontinueItem = ((Controls_TextBox)(this.FV_ItemDisCon.FindControl("tdDiscontinueItem"))).Text.Trim();
        itemDiscontinue.DiscontinueItem = TheItemMgr.LoadItem(discontinueItem);

        itemDiscontinue.CreateDate = DateTime.Now;
        itemDiscontinue.CreateUser = this.CurrentUser.Code;
        itemDiscontinue.LastModifyDate = DateTime.Now;
        itemDiscontinue.LastModifyUser = this.CurrentUser.Code;

        string endDate = ((TextBox)(this.FV_ItemDisCon.FindControl("tbEndDate"))).Text;
        if (endDate != string.Empty)
        {
            itemDiscontinue.EndDate = DateTime.Parse(endDate);
        }
        else
        {
            itemDiscontinue.EndDate = null;
        }
    }

    protected void ODS_ItemDisCon_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (CreateEvent != null)
        {
            CreateEvent(itemDiscontinue.Id.ToString(), e);
            ShowSuccessMessage("MasterData.ItemDiscontinue.AddItemDiscontinue.Successfully", itemDiscontinue.Id.ToString());
        }
    }
}

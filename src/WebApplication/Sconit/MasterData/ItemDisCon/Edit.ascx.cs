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

public partial class MasterData_ItemDisCon_Edit : EditModuleBase
{
    public event EventHandler BackEvent;

    private ItemDiscontinue itemDiscontinue;
    protected string id
    {
        get
        {
            return (string)ViewState["Id"];
        }
        set
        {
            ViewState["Id"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {


    }


    protected void FV_ItemDisCon_DataBound(object sender, EventArgs e)
    {

        ItemDiscontinue itemDiscontinue = (ItemDiscontinue)(((FormView)(sender)).DataItem);
        if (itemDiscontinue != null)
        {
            ((TextBox)(this.FV_ItemDisCon.FindControl("tdBom"))).Text = (itemDiscontinue.Bom == null) ? string.Empty : itemDiscontinue.Bom.Code;
            ((Controls_TextBox)(this.FV_ItemDisCon.FindControl("tdItem"))).Text = (itemDiscontinue.Item) == null ? string.Empty : itemDiscontinue.Item.Code;
            ((Controls_TextBox)(this.FV_ItemDisCon.FindControl("tdDiscontinueItem"))).Text = (itemDiscontinue.DiscontinueItem == null) ? string.Empty : itemDiscontinue.DiscontinueItem.Code;
            
        }
    }
    public void InitPageParameter(string id)
    {
        this.id = id;
        this.ODS_ItemDisCon.SelectParameters["Id"].DefaultValue = this.id;
        itemDiscontinue = TheItemDiscontinueMgr.LoadItemDiscontinue(Int32.Parse(id));
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void ODS_ItemDisCon_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ShowSuccessMessage("MasterData.ItemDisCon.UpdateItemDisCon.Successfully", id);
       // btnBack_Click(this, e);
    }
    
    protected void checkItemExists(object source, ServerValidateEventArgs args)
    {
        string code = ((Controls_TextBox)(this.FV_ItemDisCon.FindControl("tdItem"))).Text;

        if (TheItemMgr.LoadItem(code) == null || TheItemMgr.LoadItem(code).Equals(""))
        {
            ShowErrorMessage("MasterData.ItemDisCon.CodeExist11", code);
            args.IsValid = false;
        }
    }

    protected void checkItemExists1(object source, ServerValidateEventArgs args)
    {
        string code = ((Controls_TextBox)(this.FV_ItemDisCon.FindControl("tdDiscontinueItem"))).Text;

        if (TheItemMgr.LoadItem(code) == null || TheItemMgr.LoadItem(code).Equals(""))
        {
            ShowErrorMessage("MasterData.ItemDisCon.CodeExist22", code);
            args.IsValid = false;
        }
    }

    protected void checkItemExists3(object source, ServerValidateEventArgs args)
    {
        string code = ((TextBox)(this.FV_ItemDisCon.FindControl("tdBom"))).Text;

        if (TheBomMgr.LoadBom(code) == null || TheBomMgr.LoadBom(code).Equals(""))
        {
            ShowErrorMessage("MasterData.ItemDisCon.CodeExist33", code);
            args.IsValid = false;
        }
    }

    protected void ODS_ItemDisCon_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        ItemDiscontinue ItemDisCon = (ItemDiscontinue)e.InputParameters[0];
        string bom = ((TextBox)(this.FV_ItemDisCon.FindControl("tdBom"))).Text.Trim();
        ItemDisCon.Bom = TheBomMgr.LoadBom(bom);
        string item = ((Controls_TextBox)(this.FV_ItemDisCon.FindControl("tdItem"))).Text.Trim();
        ItemDisCon.Item = TheItemMgr.LoadItem(item);
        string discontinueItem = ((Controls_TextBox)(this.FV_ItemDisCon.FindControl("tdDiscontinueItem"))).Text.Trim();
        ItemDisCon.DiscontinueItem = TheItemMgr.LoadItem(discontinueItem);
        ItemDisCon.LastModifyDate = DateTime.Now;
        ItemDisCon.LastModifyUser = this.CurrentUser.Code;

        string endDate = ((TextBox)(this.FV_ItemDisCon.FindControl("tbEndDate"))).Text;
        if (endDate != string.Empty)
        {
            ItemDisCon.EndDate = DateTime.Parse(endDate);
        }
        else
        {
            ItemDisCon.EndDate = null;
        }
    }

    protected void ODS_ItemDisCon_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception == null)
        {
            btnBack_Click(this, e);
            ShowSuccessMessage("MasterData.ItemDisCon.DeleteItemDisCon.Successfully", id);
        }
        else if (e.Exception.InnerException is Castle.Facilities.NHibernateIntegration.DataException)
        {
            ShowErrorMessage("MasterData.ItemDisCon.DeleteItemDisCon.Fail", id);
            e.ExceptionHandled = true;
        }
    }
}

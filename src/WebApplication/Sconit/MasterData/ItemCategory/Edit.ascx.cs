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

public partial class MasterData_ItemCategory_Edit : EditModuleBase
{
    public event EventHandler BackEvent;

    protected string ItemCategoryCode
    {
        get
        {
            return (string)ViewState["ItemCategoryCode"];
        }
        set
        {
            ViewState["ItemCategoryCode"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {


    }

    public void InitPageParameter(string code)
    {
        this.ItemCategoryCode = code;
        this.ODS_ItemCategory.SelectParameters["code"].DefaultValue = this.ItemCategoryCode;
        Controls_TextBox tbParent = (Controls_TextBox)(this.FV_ItemCategory.FindControl("tbParent"));
        ItemCategory itemCategory = TheItemCategoryMgr.LoadItemCategory(code);
        tbParent.Text = itemCategory.ParentCategory == null ? string.Empty : itemCategory.ParentCategory.Code;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void ODS_ItemCategory_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ShowSuccessMessage("MasterData.ItemCategory.UpdateItemCategory.Successfully", ItemCategoryCode);
        btnBack_Click(this, e);
    }

    protected void ODS_ItemCategory_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        ItemCategory itemCategory = (ItemCategory)e.InputParameters[0];
        string parentCategory = ((Controls_TextBox)(this.FV_ItemCategory.FindControl("tbParent"))).Text.Trim();
        if (itemCategory != null)
        {
            itemCategory.Code = itemCategory.Code.Trim();
            itemCategory.ParentCategory = TheItemCategoryMgr.LoadItemCategory(parentCategory);
        }
    }

    protected void ODS_ItemCategory_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception == null)
        {
            btnBack_Click(this, e);
            ShowSuccessMessage("MasterData.ItemCategory.DeleteItemCategory.Successfully", ItemCategoryCode);
        }
        else if (e.Exception.InnerException is Castle.Facilities.NHibernateIntegration.DataException)
        {
            ShowErrorMessage("MasterData.ItemCategory.DeleteItemCategory.Fail", ItemCategoryCode);
            e.ExceptionHandled = true;
        }
    }
}

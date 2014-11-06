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

public partial class MasterData_ItemCategory_New : NewModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler CreateEvent;
    public event EventHandler NewEvent;

    private ItemCategory itemCategory;
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

    protected void FV_ItemCategory_OnDataBinding(object sender, EventArgs e)
    {

    }

    public void PageCleanup()
    {
        ((TextBox)(this.FV_ItemCategory.FindControl("tbCode"))).Text = string.Empty;
        ((TextBox)(this.FV_ItemCategory.FindControl("tbDesc"))).Text = string.Empty;
        ((Controls_TextBox)(this.FV_ItemCategory.FindControl("tbParent"))).Text = string.Empty;
    }

    protected void ODS_ItemCategory_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        itemCategory = (ItemCategory)e.InputParameters[0];
        string parentCategory = ((Controls_TextBox)(this.FV_ItemCategory.FindControl("tbParent"))).Text.Trim();
        if (itemCategory != null)
        {
            itemCategory.Code = itemCategory.Code.Trim();
            itemCategory.ParentCategory = TheItemCategoryMgr.LoadItemCategory(parentCategory);
        }
    }

    protected void ODS_ItemCategory_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (CreateEvent != null)
        {
            CreateEvent(itemCategory.Code, e);
            ShowSuccessMessage("MasterData.ItemCategory.AddItemCategory.Successfully", itemCategory.Code);
        }
    }

    protected void checkItemCategory(object source, ServerValidateEventArgs args)
    {
        CustomValidator cv = (CustomValidator)source;

        switch (cv.ID)
        {
            case "cvInsert":
                if (TheItemCategoryMgr.LoadItemCategory(args.Value) != null)
                {
                    ShowErrorMessage("MasterData.ItemCategory.CodeExist", args.Value);
                    args.IsValid = false;
                }
                break;
            default:
                break;
        }
    }
}

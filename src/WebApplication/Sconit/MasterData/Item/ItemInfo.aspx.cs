using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;

public partial class MasterData_Item_ItemInfo : com.Sconit.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string itemCode = this.Request["ItemCode"];
        this.Permission = "Page_ItemInfo";
        if (itemCode == null)
        {
            return;
        }

        Item item = TheItemMgr.LoadItem(itemCode);
        this.FV_Item.DataSource = new List<Item> { item };
        this.FV_Item.DataBind();

        if (item.Type == BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_K)
        {
            IList<ItemKit> itemKitList = TheItemKitMgr.GetChildItemKit(item, true);
            if (itemKitList != null && itemKitList.Count > 0)
            {
                this.GV_List_ItemKit.DataSource = itemKitList;
                this.GV_List_ItemKit.DataBind();
                this.fldItemKit.Visible = true;
            }
        }

        IList<ItemReference> itemRefList = TheItemReferenceMgr.GetItemReference(itemCode);
        if (itemRefList != null && itemRefList.Count > 0)
        {
            this.GV_List_ItemRef.DataSource = itemRefList;
            this.GV_List_ItemRef.DataBind();
            this.fldItemRef.Visible = true;
        }

    }

    protected void FV_Item_DataBound(object sender, EventArgs e)
    {
        Item item = (Item)(ItemBase)(((FormView)(sender)).DataItem);
        if (item != null)
        {
            ((Image)(this.FV_Item.FindControl("imgItem"))).ImageUrl = (item.ImageUrl == null || item.ImageUrl.Trim() == string.Empty) ? null : item.ImageUrl;
            if (item.ImageUrl == null || item.ImageUrl.Trim() == string.Empty)
            {
                ((Image)(this.FV_Item.FindControl("imgItem"))).Visible = false;
            }
        }
    }
}

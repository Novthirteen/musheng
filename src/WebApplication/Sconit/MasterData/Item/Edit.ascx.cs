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
using com.Sconit.Entity;
using com.Sconit.Web;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Control;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using com.Sconit.Utility;
using DropDownList = System.Web.UI.WebControls.DropDownList;

public partial class MasterData_Item_Edit : EditModuleBase
{
    public event EventHandler BackEvent;

    protected string ItemCode
    {
        get
        {
            return (string)ViewState["ItemCode"];
        }
        set
        {
            ViewState["ItemCode"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void FV_Item_DataBound(object sender, EventArgs e)
    {
        //Item item = ItemMgr.LoadItem(ItemCode);

        Item item = (Item)(ItemBase)(((FormView)(sender)).DataItem);
        if (item != null)
        {
            ((TextBox)(this.FV_Item.FindControl("tbUom"))).Text = (item.Uom == null) ? string.Empty : item.Uom.Code;
            ((CodeMstrDropDownList)(this.FV_Item.FindControl("ddlType"))).SelectedValue = item.Type;
            //((Controls_TextBox)(this.FV_Item.FindControl("tbLocation"))).Text = (item.Location) == null ? string.Empty : item.Location.Code;
            //((Controls_TextBox)(this.FV_Item.FindControl("tbBom"))).Text = (item.Bom == null) ? string.Empty : item.Bom.Code;
            //((Controls_TextBox)(this.FV_Item.FindControl("tbRouting"))).Text = (item.Routing == null) ? string.Empty : item.Routing.Code;
            ((TextBox)(this.FV_Item.FindControl("tbMsl"))).Text = (item.Msl == null) ? string.Empty : item.Msl;
            ((TextBox)(this.FV_Item.FindControl("tbBin"))).Text = (item.Bin == null) ? string.Empty : item.Bin;
            ((Controls_TextBox)(this.FV_Item.FindControl("tbItemCategory"))).Text = (item.ItemCategory == null) ? string.Empty : item.ItemCategory.Code;
            ((System.Web.UI.WebControls.Image)(this.FV_Item.FindControl("imgUpload"))).ImageUrl = (item.ImageUrl == null || item.ImageUrl.Trim() == string.Empty) ? null : item.ImageUrl;

            ((Controls_TextBox)(this.FV_Item.FindControl("tbScrapBillAddress"))).Text = item.ScrapBillAddress == null ? string.Empty : item.ScrapBillAddress;

            ((DropDownList)(this.FV_Item.FindControl("ddlCategory1"))).SelectedValue = (item.Category1 == null) ? string.Empty : item.Category1.Code;
            ((DropDownList)(this.FV_Item.FindControl("ddlCategory2"))).SelectedValue = (item.Category2 == null) ? string.Empty : item.Category2.Code;
            ((DropDownList)(this.FV_Item.FindControl("ddlCategory3"))).SelectedValue = (item.Category3 == null) ? string.Empty : item.Category3.Code;
            ((DropDownList)(this.FV_Item.FindControl("ddlCategory4"))).SelectedValue = (item.Category4 == null) ? string.Empty : item.Category4.Code;
            ((DropDownList)(this.FV_Item.FindControl("ddlItemBrand"))).SelectedValue = (item.ItemBrand == null) ? string.Empty : item.ItemBrand.Code;

            Controls_TextBox tbDefaultSupplier = (Controls_TextBox)this.FV_Item.FindControl("tbDefaultSupplier");
            tbDefaultSupplier.ServiceParameter = "string:" + this.CurrentUser.Code;
            tbDefaultSupplier.DataBind();
            tbDefaultSupplier.Text = (item.DefaultSupplier == null) ? string.Empty : item.DefaultSupplier;

            if (item.ImageUrl == null || item.ImageUrl.Trim() == string.Empty)
            {
                ((CheckBox)(this.FV_Item.FindControl("cbDeleteImage"))).Visible = false;
                ((Literal)(this.FV_Item.FindControl("ltlDeleteImage"))).Visible = false;
            }

            ((Controls_TextBox)(this.FV_Item.FindControl("txtItemPack"))).Text = (item.ItemPack == null) ? string.Empty : item.ItemPack.Spec;
        }
    }

    public void InitPageParameter(string code)
    {
        this.ItemCode = code;
        this.ODS_Item.SelectParameters["code"].DefaultValue = this.ItemCode;
        this.ODS_Item.DataBind();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void ODS_Item_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ShowSuccessMessage("MasterData.Item.UpdateItem.Successfully", ItemCode);
    }

    protected void ODS_Item_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        Item item = (Item)e.InputParameters[0];

        string itemPack = ((Controls_TextBox)(this.FV_Item.FindControl("txtItemPack"))).Text.Trim();
        if (itemPack != string.Empty)
        {
            item.ItemPack = TheToolingMgr.GetItemPack(itemPack)[0];
        }

        item.Desc1 = item.Desc1.Trim();
        item.Desc2 = item.Desc2.Trim();
        item.Memo = item.Memo.Trim();

        item.Type = ((CodeMstrDropDownList)(this.FV_Item.FindControl("ddlType"))).SelectedValue;

        string uom = ((TextBox)(this.FV_Item.FindControl("tbUom"))).Text.Trim();
        item.Uom = TheUomMgr.LoadUom(uom);

        //string location = ((Controls_TextBox)(this.FV_Item.FindControl("tbLocation"))).Text.Trim();
        //item.Location = TheLocationMgr.LoadLocation(location);

        //string bom = ((Controls_TextBox)(this.FV_Item.FindControl("tbBom"))).Text.Trim();
        //item.Bom = TheBomMgr.LoadBom(bom);

        //string routing = ((Controls_TextBox)(this.FV_Item.FindControl("tbRouting"))).Text.Trim();
        //item.Routing = TheRoutingMgr.LoadRouting(routing);

        string msl = ((TextBox)(this.FV_Item.FindControl("tbMsl"))).Text.Trim();
        item.Msl = msl;

        string bin = ((TextBox)(this.FV_Item.FindControl("tbBin"))).Text.Trim();
        item.Bin = bin;

        string itemCategory = ((Controls_TextBox)(this.FV_Item.FindControl("tbItemCategory"))).Text.Trim();
        item.ItemCategory = TheItemCategoryMgr.LoadItemCategory(itemCategory);

        string scrapBillAddress = ((Controls_TextBox)(this.FV_Item.FindControl("tbScrapBillAddress"))).Text.Trim();
        if (scrapBillAddress != string.Empty)
        {
            item.ScrapBillAddress = scrapBillAddress;
        }

        string category1 = ((DropDownList)(this.FV_Item.FindControl("ddlCategory1"))).SelectedValue;
        if (category1 != string.Empty)
        {
            item.Category1 = TheItemTypeMgr.LoadItemType(category1);
        }

        string category2 = ((DropDownList)(this.FV_Item.FindControl("ddlCategory2"))).SelectedValue;
        if (category2 != string.Empty)
        {
            item.Category2 = TheItemTypeMgr.LoadItemType(category2);
        }

        string category3 = ((DropDownList)(this.FV_Item.FindControl("ddlCategory3"))).SelectedValue;
        if (category3 != string.Empty)
        {
            item.Category3 = TheItemTypeMgr.LoadItemType(category3);
        }

        string category4 = ((DropDownList)(this.FV_Item.FindControl("ddlCategory4"))).SelectedValue;
        if (category4 != string.Empty)
        {
            item.Category4 = TheItemTypeMgr.LoadItemType(category4);
        }

        string itemBrand = ((DropDownList)(this.FV_Item.FindControl("ddlItemBrand"))).SelectedValue;
        if (itemBrand != string.Empty)
        {
            item.ItemBrand = TheItemBrandMgr.LoadItemBrand(itemBrand);
        }

        string defaultSupplier = ((Controls_TextBox)(this.FV_Item.FindControl("tbDefaultSupplier"))).Text.Trim();
        if (defaultSupplier != string.Empty)
        {
            item.DefaultSupplier = defaultSupplier;
        }

        decimal uc = item.UnitCount;
        uc = System.Decimal.Round(uc, 8);
        if (uc == 0)
        {
            ShowErrorMessage("MasterData.Item.UC.Zero");
            e.Cancel = true;
        }

        string imageUrl;
        string imgUpload = ((System.Web.UI.WebControls.Image)(this.FV_Item.FindControl("imgUpload"))).ImageUrl;

        if (((CheckBox)(this.FV_Item.FindControl("cbDeleteImage"))).Checked == true)
        {
            imageUrl = null;
            if (File.Exists(Server.MapPath(imgUpload)))
            {
                File.Delete(Server.MapPath(imgUpload));
            }
        }
        else
        {
            imageUrl = UploadItemImage(item.Code);
            if (imageUrl == null)
            {
                imageUrl = imgUpload;
            }
        }

        item.ImageUrl = imageUrl;
        item.LastModifyDate = DateTime.Now;
        item.LastModifyUser = this.CurrentUser.Code;

        if (item.IsSortAndColor.HasValue && item.IsSortAndColor.Value)
        {
            if (item.SortLevel1From == null || item.SortLevel1From.Trim() == string.Empty)
            {
                item.SortLevel1From = BusinessConstants.SORT_COLOR_IGNORE_LABEL;
            }

            if (item.SortLevel1To == null || item.SortLevel1To.Trim() == string.Empty)
            {
                item.SortLevel1To = BusinessConstants.SORT_COLOR_IGNORE_LABEL;
            }

            if (item.ColorLevel1From == null || item.ColorLevel1From.Trim() == string.Empty)
            {
                item.ColorLevel1From = BusinessConstants.SORT_COLOR_IGNORE_LABEL;
            }

            if (item.ColorLevel1To == null || item.ColorLevel1To.Trim() == string.Empty)
            {
                item.ColorLevel1To = BusinessConstants.SORT_COLOR_IGNORE_LABEL;
            }

            if (item.SortLevel2From == null || item.SortLevel2From.Trim() == string.Empty)
            {
                item.SortLevel2From = BusinessConstants.SORT_COLOR_IGNORE_LABEL;
            }

            if (item.SortLevel2To == null || item.SortLevel2To.Trim() == string.Empty)
            {
                item.SortLevel2To = BusinessConstants.SORT_COLOR_IGNORE_LABEL;
            }

            if (item.ColorLevel2From == null || item.ColorLevel2From.Trim() == string.Empty)
            {
                item.ColorLevel2From = BusinessConstants.SORT_COLOR_IGNORE_LABEL;
            }

            if (item.ColorLevel2To == null || item.ColorLevel2To.Trim() == string.Empty)
            {
                item.ColorLevel2To = BusinessConstants.SORT_COLOR_IGNORE_LABEL;
            }
        }
    }
    protected void ODS_Item_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        //DeleteItem = (Item)e.InputParameters[0];
    }

    protected void ODS_Item_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception == null)
        {
            string imgUpload = ((System.Web.UI.WebControls.Image)(this.FV_Item.FindControl("imgUpload"))).ImageUrl;
            if (File.Exists(Server.MapPath(imgUpload)))
            {
                File.Delete(Server.MapPath(imgUpload));
            }
            btnBack_Click(this, e);
            ShowSuccessMessage("MasterData.Item.DeleteItem.Successfully", ItemCode);
        }
        else if (e.Exception.InnerException is Castle.Facilities.NHibernateIntegration.DataException)
        {
            ShowErrorMessage("MasterData.Item.DeleteItem.Fail", ItemCode);
            e.ExceptionHandled = true;
        }
    }

    private string UploadItemImage(string itemCode)
    {
        string mapPath = TheEntityPreferenceMgr.LoadEntityPreference("ItemImageDir").Value;//"~/Images/Item/";
        string filePath = Server.MapPath(mapPath);
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        System.Web.UI.WebControls.FileUpload fileUpload = (System.Web.UI.WebControls.FileUpload)(this.FV_Item.FindControl("fileUpload"));
        Literal lblUploadMessage = (Literal)(this.FV_Item.FindControl("lblUploadMessage"));

        if (fileUpload.HasFile)
        {
            if (fileUpload.FileName != "" && fileUpload.FileContent.Length != 0)
            {
                string fileExtension = Path.GetExtension(fileUpload.FileName);
                if (fileExtension.ToLower() == ".gif" || fileExtension.ToLower() == ".png" || fileExtension.ToLower() == ".jpg")
                {
                    string fileName = itemCode + ".jpg";
                    string fileFullPath = filePath + "\\" + fileName;

                    #region 调整图片大小
                    AdjustImageHelper.AdjustImage(150, fileFullPath, fileUpload.FileContent);
                    #endregion

                    if (File.Exists(fileFullPath))
                    {
                        ShowWarningMessage("MasterData.Item.AddImage.Replace", fileName);
                    }
                    else
                    {
                        ShowSuccessMessage("MasterData.Item.AddImage.Successfully", fileName);
                    }
                    return mapPath + fileName;
                }
                else
                {
                    ShowWarningMessage("MasterData.Item.AddImage.UnSupportFormat");
                    return null;
                }
            }
        }
        return null;
    }

    protected void checkInput(object source, ServerValidateEventArgs args)
    {
        #region 验证cbIsSortAndColor选中后的必输项
        //bool isSortAndColor = ((CheckBox)(this.FV_Item.FindControl("cbIsSortAndColor"))).Checked;

        //if (isSortAndColor)
        //{
        //    string itemBrand1 = ((DropDownList)(this.FV_Item.FindControl("ddlItemBrand"))).SelectedValue.Trim();
        //    if (itemBrand1 == string.Empty)
        //    {
        //        ShowErrorMessage("MasterData.Item.CheckedIsSortAndColor.ItemBrand.SortLevel.ColorLevel.NotEmpty");
        //        args.IsValid = false;
        //    }

        //    string sortLevel1From = ((TextBox)(this.FV_Item.FindControl("tbSortLevel1From"))).Text.Trim();
        //    if (sortLevel1From == string.Empty)
        //    {
        //        ShowErrorMessage("MasterData.Item.CheckedIsSortAndColor.ItemBrand.SortLevel.ColorLevel.NotEmpty");
        //        args.IsValid = false;
        //    }
        //    string tbSortLevel1To = ((TextBox)(this.FV_Item.FindControl("tbSortLevel1To"))).Text.Trim();
        //    if (tbSortLevel1To == string.Empty)
        //    {
        //        ShowErrorMessage("MasterData.Item.CheckedIsSortAndColor.ItemBrand.SortLevel.ColorLevel.NotEmpty");
        //        args.IsValid = false;
        //    }
        //    string sortLevel2From = ((TextBox)(this.FV_Item.FindControl("tbSortLevel2From"))).Text.Trim();
        //    if (sortLevel2From == string.Empty)
        //    {
        //        ShowErrorMessage("MasterData.Item.CheckedIsSortAndColor.ItemBrand.SortLevel.ColorLevel.NotEmpty");
        //        args.IsValid = false;
        //    }
        //    string sortLevel2To = ((TextBox)(this.FV_Item.FindControl("tbSortLevel2To"))).Text.Trim();
        //    if (sortLevel2To == string.Empty)
        //    {
        //        ShowErrorMessage("MasterData.Item.CheckedIsSortAndColor.ItemBrand.SortLevel.ColorLevel.NotEmpty");
        //        args.IsValid = false;
        //    }

        //    string colorLevel1From = ((TextBox)(this.FV_Item.FindControl("tbColorLevel1From"))).Text.Trim();
        //    if (colorLevel1From == string.Empty)
        //    {
        //        ShowErrorMessage("MasterData.Item.CheckedIsSortAndColor.ItemBrand.SortLevel.ColorLevel.NotEmpty");
        //        args.IsValid = false;
        //    }
        //    string colorLevel1To = ((TextBox)(this.FV_Item.FindControl("tbColorLevel1To"))).Text.Trim();
        //    if (colorLevel1To == string.Empty)
        //    {
        //        ShowErrorMessage("MasterData.Item.CheckedIsSortAndColor.ItemBrand.SortLevel.ColorLevel.NotEmpty");
        //        args.IsValid = false;
        //    }

        //    string colorLevel2From = ((TextBox)(this.FV_Item.FindControl("tbColorLevel2From"))).Text.Trim();
        //    if (colorLevel2From == string.Empty)
        //    {
        //        ShowErrorMessage("MasterData.Item.CheckedIsSortAndColor.ItemBrand.SortLevel.ColorLevel.NotEmpty");
        //        args.IsValid = false;
        //    }
        //    string colorLevel2To = ((TextBox)(this.FV_Item.FindControl("tbColorLevel2To"))).Text.Trim();
        //    if (colorLevel2To == string.Empty)
        //    {
        //        ShowErrorMessage("MasterData.Item.CheckedIsSortAndColor.ItemBrand.SortLevel.ColorLevel.NotEmpty");
        //        args.IsValid = false;
        //    }
        //}
        #endregion

        #region 验证ddlType为客供品时的必输项
        string type = ((CodeMstrDropDownList)(this.FV_Item.FindControl("ddlType"))).SelectedValue;

        if (type == BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_C)
        {
            string scrapPriceValue = ((TextBox)(this.FV_Item.FindControl("tbScrapPrice"))).Text.Trim();
            if (scrapPriceValue == string.Empty)
            {
                ShowErrorMessage("MasterData.Item.CheckedType.ScrapPrice.ScrapBillAddress.NotEmpty");
                args.IsValid = false;
            }
            string scrapBillAddressValue = ((Controls_TextBox)(this.FV_Item.FindControl("tbScrapBillAddress"))).Text.Trim();
            if (scrapBillAddressValue == string.Empty)
            {
                ShowErrorMessage("MasterData.Item.CheckedType.ScrapPrice.ScrapBillAddress.NotEmpty");
                args.IsValid = false;
            }
        }
        #endregion
    }
}

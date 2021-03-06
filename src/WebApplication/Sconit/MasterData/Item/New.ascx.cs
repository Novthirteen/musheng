﻿using System;
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
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using com.Sconit.Utility;

public partial class MasterData_Item_New : NewModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler CreateEvent;

    private Item item;

    protected void Page_Load(object sender, EventArgs e)
    {
        //((Controls_TextBox)(this.FV_Item.FindControl("tbLocation"))).ServiceParameter = "string:" + this.CurrentUser.Code + ",string:";
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
        ((TextBox)(this.FV_Item.FindControl("tbCode"))).Text = string.Empty;
        ((TextBox)(this.FV_Item.FindControl("tbCount"))).Text = string.Empty;
        ((TextBox)(this.FV_Item.FindControl("tbHuLotSize"))).Text = string.Empty;
        ((CheckBox)(this.FV_Item.FindControl("tbIsActive"))).Checked = true;
        ((TextBox)(this.FV_Item.FindControl("tbDesc1"))).Text = string.Empty;
        ((TextBox)(this.FV_Item.FindControl("tbDesc2"))).Text = string.Empty;
        ((Controls_TextBox)(this.FV_Item.FindControl("tbUom"))).Text = string.Empty;
        //((Controls_TextBox)(this.FV_Item.FindControl("tbLocation"))).Text = string.Empty;
        //((Controls_TextBox)(this.FV_Item.FindControl("tbBom"))).Text = string.Empty;
        //((Controls_TextBox)(this.FV_Item.FindControl("tbRouting"))).Text = string.Empty;
        ((TextBox)(this.FV_Item.FindControl("tbMsl"))).Text = string.Empty;
        ((TextBox)(this.FV_Item.FindControl("tbBin"))).Text = string.Empty;
        ((TextBox)(this.FV_Item.FindControl("tbMemo"))).Text = string.Empty;
        ((DropDownList)(this.FV_Item.FindControl("ddlItemBrand"))).SelectedIndex = 0;
        ((Controls_TextBox)(this.FV_Item.FindControl("tbScrapBillAddress"))).Text = string.Empty;
        ((DropDownList)(this.FV_Item.FindControl("ddlCategory1"))).SelectedIndex = 0;
        ((DropDownList)(this.FV_Item.FindControl("ddlCategory2"))).SelectedIndex = 0;
        ((DropDownList)(this.FV_Item.FindControl("ddlCategory3"))).SelectedIndex = 0;
        ((DropDownList)(this.FV_Item.FindControl("ddlCategory4"))).SelectedIndex = 0;
        ((DropDownList)(this.FV_Item.FindControl("ddlItemBrand"))).SelectedIndex = 0;
      
        ((CheckBox)(this.FV_Item.FindControl("cbIsSortAndColor"))).Checked = false;
        ((TextBox)(this.FV_Item.FindControl("tbSortLevel1From"))).Text = string.Empty;
        ((TextBox)(this.FV_Item.FindControl("tbSortLevel1To"))).Text = string.Empty;
        ((TextBox)(this.FV_Item.FindControl("tbSortLevel2From"))).Text = string.Empty;
        ((TextBox)(this.FV_Item.FindControl("tbSortLevel2To"))).Text = string.Empty;
        ((TextBox)(this.FV_Item.FindControl("tbColorLevel1From"))).Text = string.Empty;
        ((TextBox)(this.FV_Item.FindControl("tbColorLevel1To"))).Text = string.Empty;
        ((TextBox)(this.FV_Item.FindControl("tbColorLevel2From"))).Text = string.Empty;
        ((TextBox)(this.FV_Item.FindControl("tbColorLevel2To"))).Text = string.Empty;
     
        ((TextBox)(this.FV_Item.FindControl("tbScrapPercentage"))).Text = string.Empty;
        ((TextBox)(this.FV_Item.FindControl("tbPinNumber"))).Text = string.Empty;
        ((TextBox)(this.FV_Item.FindControl("tbGoodsReceiptLotSize"))).Text = string.Empty;
        ((TextBox)(this.FV_Item.FindControl("tbHistoryPrice"))).Text = string.Empty;
        ((TextBox)(this.FV_Item.FindControl("tbScrapPrice"))).Text = string.Empty;
        ((Controls_TextBox)(this.FV_Item.FindControl("tbScrapBillAddress"))).Text = string.Empty;
        ((TextBox)(this.FV_Item.FindControl("tbMRPLeadTime"))).Text = string.Empty;
        ((TextBox)(this.FV_Item.FindControl("tbMemo"))).Text = string.Empty;
        ((TextBox)(this.FV_Item.FindControl("tbSalesCost"))).Text = string.Empty;
        ((CheckBox)(this.FV_Item.FindControl("cbNeedInspect"))).Checked = false;
        ((CheckBox)(this.FV_Item.FindControl("cbIsRunMrp"))).Checked = false;
    }

    protected void ODS_Item_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        item = (Item)e.InputParameters[0];

        item.Desc1 = item.Desc1.Trim();
        item.Desc2 = item.Desc2.Trim();
        item.Memo = item.Memo.Trim();

        if (item.Code == null || item.Code.Trim() == string.Empty)
        {
            ShowErrorMessage("MasterData.Item.Code.Empty");
            e.Cancel = true;
            return;
        }
        else
        {
            item.Code = item.Code.Trim();
        }

        if (TheItemMgr.LoadItem(item.Code) != null)
        {
            e.Cancel = true;
            ShowErrorMessage("MasterData.Item.CodeExist", item.Code);
            return;
        }

        string uom = ((Controls_TextBox)(this.FV_Item.FindControl("tbUom"))).Text.Trim() == string.Empty ? "EA"
            : ((Controls_TextBox)(this.FV_Item.FindControl("tbUom"))).Text.Trim();
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

        string scrapBillAddress = ((Controls_TextBox)(this.FV_Item.FindControl("tbScrapBillAddress"))).Text.Trim();
        if (scrapBillAddress != string.Empty)
        {
            item.ScrapBillAddress = scrapBillAddress;
        }

        decimal uc = item.UnitCount;
        uc = System.Decimal.Round(uc, 8);
        if (uc == 0)
        {
            ShowErrorMessage("MasterData.Item.UC.Zero");
            e.Cancel = true;
        }

        item.ImageUrl = UploadItemImage(item.Code);
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

    protected void ODS_Item_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (CreateEvent != null)
        {
            CreateEvent(item.Code, e);
            ShowSuccessMessage("MasterData.Item.AddItem.Successfully", item.Code);
        }
    }

    protected void checkItemExists(object source, ServerValidateEventArgs args)
    {
        string code = ((TextBox)(this.FV_Item.FindControl("tbCode"))).Text;

        if (TheItemMgr.LoadItem(code) != null)
        {
            ShowErrorMessage("MasterData.Item.CodeExist", code);
            args.IsValid = false;
        }
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
        string type = ((DropDownList)(this.FV_Item.FindControl("ddlType"))).SelectedValue;

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
}

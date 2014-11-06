using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Exception;


public partial class Inventory_PrintHu_Item : ModuleBase
{
    public string ModuleType
    {
        get
        {
            return (string)ViewState["ModuleType"];
        }
        set
        {
            ViewState["ModuleType"] = value;
        }
    }

    public void InitPageParameter(Flow flow)
    {

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //tbManufactureDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
    }



    protected void tbItemHuId_TextChanged(object sender, EventArgs e)
    {
        string huId = this.tbItemHuId.Text.Trim();
        if (huId == string.Empty)
        {
        }
        else
        {
            Hu hu = this.TheHuMgr.LoadHu(huId);
            if (hu != null)
            {
                Item item = hu.Item;
                this.tbItemCode.Text = item.Code;

                this.tbManufactureDate.Text = hu.ManufactureDate.ToString("yyyy-MM-dd");
                this.tbSupplierLotNo.Text = hu.SupplierLotNo;
                this.tbItemBrand.Text = item.ItemBrand == null ? string.Empty : item.ItemBrand.Description;
                this.tbItemDescription.Text = item.Description;
                this.tbItemUom.Text = item.Uom.Code;

                if (this.rblPackageType.SelectedIndex == 0)
                {
                    this.tbItemUnitCount.Text = hu.LotSize.ToString("#.##");
                    this.tbHuLotSize.Text = item.HuLotSize.HasValue ? item.HuLotSize.Value.ToString() : string.Empty;
                }
                else
                {
                    this.tbItemUnitCount.Text = item.UnitCount.ToString("#.##");
                    this.tbHuLotSize.Text = hu.LotSize.ToString("#.##");
                }


                if (hu.ColorLevel1 != null && hu.ColorLevel1 != string.Empty)
                {
                    tbColorLevel1.Text = hu.ColorLevel1;
                }
                else
                {
                    tbColorLevel1.Text = string.Empty;
                }
                if (hu.SortLevel1 != null && hu.SortLevel1 != string.Empty)
                {
                    tbSortLevel1.Text = hu.SortLevel1;

                }
                else
                {
                    tbSortLevel1.Text = string.Empty;
                }


                if (hu.ColorLevel2 != null && hu.ColorLevel2 != string.Empty)
                {
                    tbColorLevel2.Text = hu.ColorLevel2;
                }
                else
                {
                    tbColorLevel2.Text = string.Empty;
                }
                if (hu.SortLevel2 != null && hu.SortLevel2 != string.Empty)
                {
                    tbSortLevel2.Text = hu.SortLevel2;

                }
                else
                {
                    tbSortLevel2.Text = string.Empty;
                }


                if (!item.IsSortAndColor.HasValue || !item.IsSortAndColor.Value)
                {
                    this.tbSortLevel1.ReadOnly = true;
                    this.tbSortLevel2.ReadOnly = true;
                    this.tbColorLevel1.ReadOnly = true;
                    this.tbColorLevel2.ReadOnly = true;

                    rfvColorLevel2.Enabled = false;
                    rfvColorLevel1.Enabled = false;
                    rfvSortLevel1.Enabled = false;
                    rfvSortLevel2.Enabled = false;

                    tbSortLevel2.Text = string.Empty;
                    tbColorLevel2.Text = string.Empty;
                }
            }
        }
    }
    protected void tbItem_TextChanged(object sender, EventArgs e)
    {
        string itemCode = this.tbItemCode.Text.Trim();
        Item item = TheItemMgr.LoadItem(itemCode);
        if (item != null)
        {
            this.tbItemBrand.Text = item.ItemBrand == null ? string.Empty : item.ItemBrand.Description;
            this.tbItemDescription.Text = item.Description;
            this.tbItemUnitCount.Text = item.UnitCount.ToString("F2");
            this.tbItemUom.Text = item.Uom.Code;
            this.tbHuLotSize.Text = item.HuLotSize.HasValue ? item.HuLotSize.Value.ToString() : string.Empty;
            this.tbManufactureDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            tbColorLevel1.Text = string.Empty;
            tbSortLevel1.Text = string.Empty;

            if (item.IsSortAndColor.HasValue && item.IsSortAndColor.Value)
            {
                this.tbSortLevel1.ReadOnly = false;
                this.tbSortLevel2.ReadOnly = false;
                this.tbColorLevel1.ReadOnly = false;
                this.tbColorLevel2.ReadOnly = false;
                rfvColorLevel2.Enabled = true;
                rfvColorLevel1.Enabled = true;
                rfvSortLevel1.Enabled = true;
                rfvSortLevel2.Enabled = true;

                //tbSortLevel2.Text = "*";
                //tbColorLevel2.Text = "*";
                if (item.SortLevel1From == BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                {
                    this.tbSortLevel1.Text = BusinessConstants.SORT_COLOR_IGNORE_LABEL;
                }
                else
                {
                    this.tbSortLevel1.Text = string.Empty;
                }

                if (item.ColorLevel1From == BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                {
                    this.tbColorLevel1.Text = BusinessConstants.SORT_COLOR_IGNORE_LABEL;
                }
                else
                {
                    this.tbColorLevel1.Text = string.Empty;
                }

                if (item.SortLevel2From == BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                {
                    this.tbSortLevel2.Text = BusinessConstants.SORT_COLOR_IGNORE_LABEL;
                }
                else
                {
                    this.tbSortLevel2.Text = string.Empty;
                }

                if (item.ColorLevel2From == BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                {
                    this.tbColorLevel2.Text = BusinessConstants.SORT_COLOR_IGNORE_LABEL;
                }
                else
                {
                    this.tbColorLevel2.Text = string.Empty;
                }

                this.lblSortLevel1Msg.Text = item.SortLevel1;
                this.lblColorLevel1Msg.Text = item.ColorLevel1;
                this.lblSortLevel2Msg.Text = item.SortLevel2;
                this.lblColorLevel2Msg.Text = item.ColorLevel2;
            }
            else
            {
                this.tbSortLevel1.ReadOnly = true;
                this.tbSortLevel2.ReadOnly = true;
                this.tbColorLevel1.ReadOnly = true;
                this.tbColorLevel2.ReadOnly = true;

                rfvColorLevel2.Enabled = false;
                rfvColorLevel1.Enabled = false;
                rfvSortLevel1.Enabled = false;
                rfvSortLevel2.Enabled = false;

                tbSortLevel1.Text = string.Empty;
                tbColorLevel1.Text = string.Empty;
                tbSortLevel2.Text = string.Empty;
                tbColorLevel2.Text = string.Empty;

                this.lblSortLevel1Msg.Text = string.Empty;
                this.lblColorLevel1Msg.Text = string.Empty;
                this.lblSortLevel2Msg.Text = string.Empty;
                this.lblColorLevel2Msg.Text = string.Empty;
            }
        }
    }

    protected void btnHuPrint_Click(object sender, EventArgs e)
    {
        try
        {
            String huTemplate = TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_DEFAULT_HU_TEMPLATE).Value;

            IList<Hu> huList = new List<Hu>();

            Hu hu = TheHuMgr.LoadHu(tbHuId.Text.Trim());

            if (hu != null)
            {
                if (huTemplate != null && huTemplate.Length > 0)
                {
                    huList.Add(hu);

                    IList<object> huDetailObj = new List<object>();

                    huDetailObj.Add(huList);
                    huDetailObj.Add(CurrentUser.Code);

                    string barCodeUrl = "";

                    barCodeUrl = TheReportMgr.WriteToFile(huTemplate, huDetailObj, huTemplate);

                    Page.ClientScript.RegisterStartupScript(GetType(), "method", " <script language='javascript' type='text/javascript'>PrintOrderByQty('" + barCodeUrl + "'," + this.tbHuCopies.Text.Trim() + "); </script>");

                    this.ShowSuccessMessage("Inventory.PrintHu.Successful");
                }
            }
            else
            {
                this.ShowErrorMessage("Common.Business.Hu.NotExist");
            }
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            #region  内/外包装
            string packageType = null;
            if (this.rblPackageType.SelectedValue == "0")
            {
                packageType = BusinessConstants.CODE_MASTER_PACKAGETYPE_INNER;
            }
            else if (this.rblPackageType.SelectedValue == "1")
            {
                packageType = BusinessConstants.CODE_MASTER_PACKAGETYPE_OUTER;
            }
            #endregion

            String huTemplate = TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_DEFAULT_HU_TEMPLATE).Value;

            IList<Hu> huList = null;
            string itemCode = this.tbItemCode.Text.Trim();
            Item item = TheItemMgr.LoadItem(itemCode);

            string lotNo = string.Empty;
            if (this.tbManufactureDate.Text.Trim() != string.Empty)
            {
                lotNo = LotNoHelper.GenerateLotNo(DateTime.Parse(this.tbManufactureDate.Text.Trim()));
            }

            if (this.ModuleType == BusinessConstants.CODE_MASTER_PARTY_TYPE_VALUE_SUPPLIER)
            {
                decimal? huLotSize = null;

                if (packageType == BusinessConstants.CODE_MASTER_PACKAGETYPE_INNER)
                {
                    if (tbItemUnitCount.Text.Trim() != string.Empty)
                    {
                        huLotSize = Convert.ToDecimal(tbItemUnitCount.Text.Trim());
                    }
                }
                else
                {
                    if (tbHuLotSize.Text.Trim() != string.Empty)
                    {
                        huLotSize = Convert.ToDecimal(tbHuLotSize.Text.Trim());
                    }
                }

                if (huLotSize == null)
                {
                    this.ShowErrorMessage("Inventory.PrintHu.Item.PackageQty.Empany");
                }

                string oldHuId = this.tbItemHuId.Text.Trim() == string.Empty ? null : this.tbItemHuId.Text.Trim();

                huList = TheHuMgr.CreateHu(item, rblQtyType.SelectedValue, decimal.Parse(this.tbPapers.Text.Trim()), lotNo, tbSupplierLotNo.Text.Trim(), huLotSize,
                    this.tbSortLevel1.Text.Trim(), this.tbColorLevel1.Text.Trim(), this.tbSortLevel2.Text.Trim(), this.tbColorLevel2.Text.Trim(), huTemplate, this.CurrentUser, null, packageType, oldHuId);

            }

            if (huTemplate != null && huTemplate.Length > 0)
            {

                IList<object> huDetailObj = new List<object>();

                huDetailObj.Add(huList);
                huDetailObj.Add(CurrentUser.Code);

                string barCodeUrl = "";
                if (true || packageType == BusinessConstants.CODE_MASTER_PACKAGETYPE_OUTER)
                {
                    //"BarCode.xls"
                    //targetFlowDetailList[0].Flow.HuTemplate
                    barCodeUrl = TheReportMgr.WriteToFile(huTemplate, huDetailObj, huTemplate);
                }
                else
                {
                    //"InsideBarCodeA4.xls" 
                    barCodeUrl = TheReportMgr.WriteToFile("Inside" + huTemplate, huDetailObj, "Inside" + huTemplate);
                }
                Page.ClientScript.RegisterStartupScript(GetType(), "method", " <script language='javascript' type='text/javascript'>PrintOrderByQty('" + barCodeUrl + "'," + this.tbCopies.Text.Trim() + "); </script>");

                this.ShowSuccessMessage("Inventory.PrintHu.Successful");


                if (!cbIsContinuousPrinting.Checked)
                {
                    PageClean();
                }
            }



        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }


    private void PageClean()
    {
        this.tbItemCode.Text = string.Empty;
        rblPackageType.SelectedIndex = 0;
        tbItemHuId.Text = string.Empty;
        tbItemUom.Text = string.Empty;
        tbItemDescription.Text = string.Empty;
        tbItemBrand.Text = string.Empty;
        tbItemUnitCount.Text = string.Empty;
        tbHuLotSize.Text = string.Empty;
        tbManufactureDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        tbSupplierLotNo.Text = string.Empty;
        rblQtyType.SelectedIndex = 0;
        tbPapers.Text = string.Empty;
        tbCopies.Text = "1";
        tbSortLevel1.Text = string.Empty;
        tbSortLevel2.Text = string.Empty;
        tbColorLevel1.Text = string.Empty;
        tbColorLevel2.Text = string.Empty;
        this.cbIsContinuousPrinting.Checked = false;
    }
}

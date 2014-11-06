using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Entity.Distribution;
using com.Sconit.Web;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using com.Sconit.Utility;
using com.Sconit.Entity.Exception;

public partial class Inventory_PrintHu_AsnList : ModuleBase
{
    public event EventHandler PrintEvent;

    private string CurrentIpNo
    {
        get
        {
            return (string)ViewState["CurrentIpNo"];
        }
        set
        {
            ViewState["CurrentIpNo"] = value;
        }
    }

    private IList<string> ItemList
    {
        get
        {
            return (IList<string>)ViewState["ItemList"];
        }
        set
        {
            ViewState["ItemList"] = value;
        }
    }

    public void InitPageParameter(string ipNo)
    {
        PageClean();
        this.CurrentIpNo = ipNo;
        ItemList = this.TheHqlMgr.FindAll<string>(@"select i.Code
            from InProcessLocationDetail as det
            join det.InProcessLocation as ip
            join det.OrderLocationTransaction as olt
            join olt.Item as i
            where ip.IpNo = ?", ipNo);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void tbItemHuId_TextChanged(object sender, EventArgs e)
    {
        string huId = this.tbItemHuId.Text.Trim();
        this.tbItemHuId.Text = string.Empty;
        if (huId == string.Empty)
        {
            PageClean();
        }
        else
        {
            string hql = string.Empty;
            IList<object> parm = new List<object>();
            foreach (string item in ItemList)
            {
                if (hql == string.Empty)
                {
                    hql = "from Item as i where i.Code in (?";
                }
                else
                {
                    hql += ",?";
                }
                parm.Add(item);
            }
            hql += ")";

            IList<Item> itemList = this.TheHqlMgr.FindAll<Item>(hql, parm.ToArray());
            if (itemList != null && itemList.Count > 0)
            {
                itemList = itemList.Where(i => (!string.IsNullOrEmpty(i.Desc1) && (i.Desc1.ToLower().Contains(huId.ToLower()) || huId.ToLower().Contains(i.Desc1.ToLower())))
                    || (!string.IsNullOrEmpty(i.Desc2) && (i.Desc2.ToLower().Contains(huId.ToLower()) || huId.ToLower().Contains(i.Desc2.ToLower())))).Distinct().ToList();
            }

            if (itemList == null || itemList.Count == 0)
            {
                this.ShowErrorMessage("InProcessLocation.Error.ItemNotFound", new string[] { huId });
                PageClean();
            }
            else
            {
                if (itemList.Count > 1)
                {
                    this.ShowErrorMessage("InProcessLocation.Error.FindManyItem", new string[] { huId });
                    PageClean();
                }
                else
                {
                    Item item = itemList[0];

                    this.tbItemCode.Text = item.Code;
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
        }
    }

    protected void tbItem_TextChanged(object sender, EventArgs e)
    {
        string itemCode = this.tbItemCode.Text.Trim();
        Item item = TheItemMgr.LoadItem(itemCode);
        if (item != null)
        {
            if (!ItemList.Contains(itemCode))
            {
                this.ShowErrorMessage("InProcessLocation.Error.ItemNotInASN", new string[] { itemCode, this.CurrentIpNo });
                PageClean();
            }
            else
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
        else
        {
            PageClean();
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

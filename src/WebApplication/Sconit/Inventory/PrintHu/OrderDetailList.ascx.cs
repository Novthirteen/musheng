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

public partial class Inventory_PrintHu_OrderDetailList : ModuleBase
{
    public event EventHandler PrintEvent;

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

    public void InitPageParameter(OrderHead orderHead)
    {
        this.GV_List.DataSource = orderHead.OrderDetails;

        #region 设置默认LotNo
        string lotNo = LotNoHelper.GenerateLotNo();
        foreach (OrderDetail orderDetail in orderHead.OrderDetails)
        {
            orderDetail.HuLotNo = lotNo;
        }
        #endregion

        this.GV_List.DataBind();
    }

    public void PrintCallBack()
    {
        IList<OrderDetail> orderDetailList = this.PopulateOrderDetailList();
        this.PrintEvent(orderDetailList, null);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            OrderDetail orderDetail = (OrderDetail)e.Row.DataItem;

            TextBox tbSortLevel1 = ((TextBox)e.Row.FindControl("tbSortLevel1"));
            TextBox tbColorLevel1 = (TextBox)e.Row.FindControl("tbColorLevel1");
            TextBox tbSortLevel2 = ((TextBox)e.Row.FindControl("tbSortLevel2"));
            TextBox tbColorLevel2 = (TextBox)e.Row.FindControl("tbColorLevel2");
            TextBox tbManufactureDate = (TextBox)e.Row.FindControl("tbManufactureDate");
            
            tbManufactureDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            
            if (orderDetail.Item.IsSortAndColor.HasValue && orderDetail.Item.IsSortAndColor.Value)
            {
                tbSortLevel1.Visible = true;
                tbColorLevel1.Visible = true;
                tbSortLevel2.Visible = true;
                tbColorLevel2.Visible = true;
            }
            else
            {
                tbSortLevel1.Visible = false;
                tbColorLevel1.Visible = false;
                tbSortLevel2.Visible = false;
                tbColorLevel2.Visible = false;
            }
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            IList<OrderDetail> orderDetailList = PopulateOrderDetailList();
            IList<OrderDetail> targetOrderDetailList = new List<OrderDetail>();

            if (orderDetailList != null && orderDetailList.Count > 0)
            {
                foreach (OrderDetail orderDetail in orderDetailList)
                {
                    if (orderDetail.OrderedQty > 0)
                    {
                        targetOrderDetailList.Add(orderDetail);
                    }
                }
            }

            if (targetOrderDetailList.Count == 0)
            {
                this.ShowErrorMessage("Inventory.Error.PrintHu.OrderDetail.Required");
                return;
            }

            IList<Hu> huList = null;

            #region  内/外包装
            string packageType = null;
            RadioButtonList rblPackageType = (RadioButtonList)this.Parent.FindControl("rblPackageType");
            if (rblPackageType.SelectedValue == "0")
            {
                packageType = BusinessConstants.CODE_MASTER_PACKAGETYPE_INNER;
            }
            else if (rblPackageType.SelectedValue == "1")
            {
                packageType = BusinessConstants.CODE_MASTER_PACKAGETYPE_OUTER;
            }
            #endregion
            if (this.ModuleType == BusinessConstants.CODE_MASTER_PARTY_TYPE_VALUE_SUPPLIER)
            {
                huList = TheHuMgr.CreateHu(targetOrderDetailList, this.CurrentUser, null, packageType);
            }
            else
            {
                EntityPreference entityPreference = this.TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_COMPANY_ID_MARK);
                huList = TheHuMgr.CreateHu(targetOrderDetailList, this.CurrentUser, entityPreference.Value, packageType);
            }

            String huTemplate = "";
            if (this.ModuleType == BusinessConstants.CODE_MASTER_PARTY_TYPE_VALUE_REGION)
            {
                huTemplate = TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_DEFAULT_HU_TEMPLATE).Value;
            }
            else if (targetOrderDetailList != null
                        && targetOrderDetailList.Count > 0
                        && targetOrderDetailList[0].OrderHead != null
                        && targetOrderDetailList[0].OrderHead.HuTemplate != null
                        && targetOrderDetailList[0].OrderHead.HuTemplate.Length > 0)
            {
                huTemplate = targetOrderDetailList[0].OrderHead.HuTemplate;
            }

            if (huTemplate != null && huTemplate.Length > 0)
            {
                IList<object> huDetailObj = new List<object>();
                huDetailObj.Add(huList);
                huDetailObj.Add(CurrentUser.Code);

                string barCodeUrl = "";
                if (true || packageType == BusinessConstants.CODE_MASTER_PACKAGETYPE_OUTER)
                {
                    barCodeUrl = TheReportMgr.WriteToFile(huTemplate, huDetailObj, huTemplate);
                }
                else
                {
                    barCodeUrl = TheReportMgr.WriteToFile("Inside" + huTemplate, huDetailObj, "Inside" + huTemplate);
                }
                Page.ClientScript.RegisterStartupScript(GetType(), "method", " <script language='javascript' type='text/javascript'>PrintOrderByQty('" + barCodeUrl + "'," + this.tbCopies.Text.Trim() + "); </script>");

                this.ShowSuccessMessage("Inventory.PrintHu.Successful");
            }
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    //返回订单明细
    private IList<OrderDetail> PopulateOrderDetailList()
    {
        if (this.GV_List.Rows != null && this.GV_List.Rows.Count > 0)
        {
            IList<OrderDetail> orderDetailList = new List<OrderDetail>();

            foreach (GridViewRow row in this.GV_List.Rows)
            {
                CheckBox checkBoxGroup = row.FindControl("CheckBoxGroup") as CheckBox;
                
                if (checkBoxGroup.Checked )
                {

                    TextBox tbSupplierLotNo = (TextBox)row.FindControl("tbSupplierLotNo");
                    if (tbSupplierLotNo.Text.Trim() == string.Empty)
                    {
                        this.ShowErrorMessage("Inventory.PrintHu.Item.SupplierLotNo.Required");
                        return null;
                    }

                    HiddenField hfId = (HiddenField)row.FindControl("hfId");
                    //TextBox tbLotNo = (TextBox)row.FindControl("tbLotNo");
                    TextBox tbManufactureDate = (TextBox)row.FindControl("tbManufactureDate");
                    TextBox tbOrderQty = (TextBox)row.FindControl("tbOrderQty");
                    
                    TextBox tbSortLevel1 = (TextBox)row.FindControl("tbSortLevel1");
                    TextBox tbColorLevel1 = (TextBox)row.FindControl("tbColorLevel1");
                    TextBox tbSortLevel2 = (TextBox)row.FindControl("tbSortLevel2");
                    TextBox tbColorLevel2 = (TextBox)row.FindControl("tbColorLevel2");

                    OrderDetail orderDetail = this.TheOrderDetailMgr.LoadOrderDetail(int.Parse(hfId.Value));
                    //orderDetail.HuLotNo = tbLotNo.Text.Trim() != string.Empty ? tbLotNo.Text.Trim() : null;
                    if (tbManufactureDate.Text.Trim() != string.Empty)
                    {
                        orderDetail.HuLotNo = LotNoHelper.GenerateLotNo(DateTime.Parse(tbManufactureDate.Text.Trim()));
                    }
                    orderDetail.HuSupplierLotNo = tbSupplierLotNo.Text != string.Empty ? tbSupplierLotNo.Text.Trim() : null;
                    orderDetail.OrderedQty = tbOrderQty.Text.Trim() != string.Empty ? decimal.Parse(tbOrderQty.Text.Trim()) : 0;
                    if (orderDetail.Item.IsSortAndColor.HasValue && orderDetail.Item.IsSortAndColor.Value)
                    {
                        if (orderDetail.Item.SortLevel1From != null && orderDetail.Item.SortLevel1From != string.Empty && orderDetail.Item.SortLevel1From != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                        {
                            orderDetail.HuSortLevel1 = tbSortLevel1.Text != string.Empty ? tbSortLevel1.Text.Trim() : null;
                            orderDetail.HuColorLevel1 = tbColorLevel1.Text != string.Empty ? tbColorLevel1.Text.Trim() : null;
                        }

                        if (orderDetail.Item.SortLevel2From != null && orderDetail.Item.SortLevel2From != string.Empty && orderDetail.Item.SortLevel2From != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                        {
                            orderDetail.HuSortLevel2 = tbSortLevel2.Text != string.Empty ? tbSortLevel2.Text.Trim() : null;
                            orderDetail.HuColorLevel2 = tbColorLevel2.Text != string.Empty ? tbColorLevel2.Text.Trim() : null;
                        }
                    }

                    orderDetailList.Add(orderDetail);
                }
            }

            return orderDetailList;
        }

        return null;
    }
}

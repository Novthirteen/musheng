using com.Sconit.Service.Ext.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;
using Castle.Services.Transaction;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity;

namespace com.Sconit.Service.Report.Impl
{
    [Transactional]
    public class RepRequisitionOrderMgr : RepTemplate1
    {
        public override string reportTemplateFolder { get; set; }

        public IOrderHeadMgrE orderHeadMgrE { get; set; }

        public RepRequisitionOrderMgr()
        {

            //明细部分的行数
            this.pageDetailRowCount = 36;
            //列数   1起始
            this.columnCount = 10;
            //报表头的行数  1起始
            this.headRowCount = 15;
            //报表尾的行数  1起始
            this.bottomRowCount = 1;

        }

        /**
         * 填充报表
         * 
         * Param list [0]OrderHead
         * Param list [0]IList<OrderDetail>           
         */
        [Transaction(TransactionMode.Requires)]
        protected override bool FillValuesImpl(String templateFileName, IList<object> list)
        {
            try
            {

                if (list == null || list.Count < 2) return false;

                OrderHead orderHead = (OrderHead)(list[0]);
                IList<OrderDetail> orderDetails = (IList<OrderDetail>)(list[1]);

                orderDetails = orderDetails.OrderBy(o => o.Sequence).ThenBy(o => o.Item.Code).ToList();

                if (orderHead == null
                    || orderDetails == null || orderDetails.Count == 0)
                {
                    return false;
                }


                //this.SetRowCellBarCode(0, 2, 8);
                this.barCodeFontName = this.GetBarcodeFontName(2, 8);
                this.CopyPage(orderDetails.Count);

                this.FillHead(orderHead);


                int pageIndex = 1;
                int rowIndex = 0;
                int rowTotal = 0;
                foreach (OrderDetail orderDetail in orderDetails)
                {

                    // No.	
                    this.SetRowCell(pageIndex, rowIndex, 0, "" + orderDetail.Sequence);

                    //零件号 Item Code
                    this.SetRowCell(pageIndex, rowIndex, 1, orderDetail.Item.Code);

                    //描述Description
                    this.SetRowCell(pageIndex, rowIndex, 2, orderDetail.Item.Description);

                    //单位Unit
                    this.SetRowCell(pageIndex, rowIndex, 3, orderDetail.Item.Uom.Code);

                    //单包装UC
                    this.SetRowCell(pageIndex, rowIndex, 4, orderDetail.UnitCount.ToString("0.########"));

                    //需求 Request	包装
                    int UCs = (int)Math.Ceiling(orderDetail.OrderedQty / orderDetail.UnitCount);
                    this.SetRowCell(pageIndex, rowIndex, 5, UCs.ToString());

                    //需求 Request	零件数
                    this.SetRowCell(pageIndex, rowIndex, 6, orderDetail.OrderedQty.ToString("0.########"));


                    if (orderDetail.UnitPriceAfterDiscount.HasValue)
                    {
                        //单价
                        this.SetRowCell(pageIndex, rowIndex, 7, orderDetail.UnitPriceAfterDiscount.Value.ToString("0.0000"));
                        //金额
                        this.SetRowCell(pageIndex, rowIndex, 8, (orderDetail.UnitPriceAfterDiscount * orderDetail.OrderedQty).Value.ToString("0.0000"));

                        //是否含税
                        if (orderDetail.IsIncludeTax)
                        {
                            this.SetRowCell(pageIndex, rowIndex, 9, "√");
                        }
                    }
                    /*
                    else if (orderDetail.UnitPrice.HasValue)
                    {
                        //单价
                        this.SetRowCell(pageIndex, rowIndex, 7, orderDetail.UnitPrice.Value.ToString("0.0000"));
                        //金额
                        this.SetRowCell(pageIndex, rowIndex, 8, (orderDetail.UnitPrice * orderDetail.OrderedQty).Value.ToString("0.0000"));

                        //是否含税
                        if (orderDetail.IsIncludeTax)
                        {
                            this.SetRowCell(pageIndex, rowIndex, 9, "√");
                        }
                    }
                    */
                    if (this.isPageBottom(rowIndex, rowTotal))//页的最后一行
                    {
                        pageIndex++;
                        rowIndex = 0;
                    }
                    else
                    {
                        rowIndex++;
                    }
                    rowTotal++;
                }

                this.sheet.DisplayGridlines = false;
                this.sheet.IsPrintGridlines = false;

                if (orderHead.IsPrinted == null || orderHead.IsPrinted == false)
                {
                    orderHead.IsPrinted = true;
                    orderHeadMgrE.UpdateOrderHead(orderHead);
                }

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        /*
         * 填充报表头
         * 
         * Param repack 报验单头对象
         */
        private void FillHead(OrderHead orderHead)
        {
            //订单号:
            string orderCode = Utility.BarcodeHelper.GetBarcodeStr(orderHead.OrderNo, this.barCodeFontName);
            if (orderHead.Type == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION)
            {
                this.SetRowCell(0, 6, "上海慕盛实业有限公司发货单");
            }
            this.SetRowCell(2, 7, orderCode);
            //Order No.:
            this.SetRowCell(3, 7, orderHead.OrderNo);

            if ("Normal".Equals(orderHead.Priority))
            {
                this.SetRowCell(4, 4, "");
            }
            else
            {
                this.SetRowCell(3, 4, "");
            }

            //发出时间 Issue Time:
            this.SetRowCell(4, 8, orderHead.StartDate.ToString());

            //供应商代码 Supplier Code:	
            this.SetRowCell(6, 2, orderHead.PartyFrom != null ? orderHead.PartyFrom.Code : String.Empty);

            //交货日期 Delivery Date:getDemandDeliverDate
            this.SetRowCell(6, 7, orderHead.WindowTime.ToLongDateString());


            //供应商名称 Supplier Name:		
            this.SetRowCell(7, 2, orderHead.PartyFrom != null ? orderHead.PartyFrom.Name : String.Empty);
            //窗口时间 Window Time:
            this.SetRowCell(7, 7, orderHead.WindowTime.ToLongTimeString());

            //供应商地址 Address:	
            this.SetRowCell(8, 2, orderHead.ShipFrom != null ? orderHead.ShipFrom.Address : String.Empty);
            //交货道口 Delivery Dock:
            this.SetRowCell(8, 7, orderHead.DockDescription);

            //供应商联系人 Contact:	
            this.SetRowCell(9, 2, orderHead.ShipFrom != null ? orderHead.ShipFrom.ContactPersonName : String.Empty);
            //物流协调员 Follow Up:
            this.SetRowCell(9, 7, orderHead.ShipTo != null ? orderHead.ShipTo.ContactPersonName : String.Empty);

            //供应商电话 Telephone:		
            this.SetRowCell(10, 2, orderHead.ShipFrom != null ? orderHead.ShipFrom.TelephoneNumber : String.Empty);
            //YFV电话 Telephone:
            this.SetRowCell(10, 7, orderHead.ShipTo != null ? orderHead.ShipTo.TelephoneNumber : String.Empty);

            //供应商传真 Fax:	
            this.SetRowCell(11, 2, orderHead.ShipFrom != null ? orderHead.ShipFrom.Fax : String.Empty);
            //YFV传真 Fax:
            this.SetRowCell(11, 7, orderHead.ShipTo != null ? orderHead.ShipTo.Fax : String.Empty);

            //系统号 SysCode:
            //this.SetRowCell(++rowNum, 3, "");
            //版本号 Version:
            //this.SetRowCell(rowNum, 8, "");
        }

        /**
           * 需要拷贝的数据与合并单元格操作
           * 
           * Param pageIndex 页号
           */
        public override void CopyPageValues(int pageIndex)
        {
            /*
            this.SetMergedRegion(pageIndex, 0, 7, 1, 11);
            this.SetMergedRegion(pageIndex, 2, 8, 2, 11);
            this.SetMergedRegion(pageIndex, 3, 8, 3, 11);
            this.SetMergedRegion(pageIndex, 4, 9, 4, 11);

            this.SetMergedRegion(pageIndex, 6, 0, 6, 2);
            this.SetMergedRegion(pageIndex, 7, 0, 7, 2);
            this.SetMergedRegion(pageIndex, 8, 0, 8, 2);
            this.SetMergedRegion(pageIndex, 9, 0, 9, 2);
            this.SetMergedRegion(pageIndex, 10, 0, 10, 2);
            this.SetMergedRegion(pageIndex, 11, 0, 11, 2);
            */

            //* 实际到货时间:
            this.CopyCell(pageIndex, 51, 1, "B52");
            //发单人签字:
            this.CopyCell(pageIndex, 51, 4, "E52");
            //供应商签字:
            this.CopyCell(pageIndex, 51, 7, "H52");
        }

        public override IList<object> GetDataList(string code)
        {
            IList<object> list = new List<object>();
            OrderHead orderHead = orderHeadMgrE.LoadOrderHead(code, true);
            if (orderHead != null)
            {
                list.Add(orderHead);
                list.Add(orderHead.OrderDetails);
            }
            return list;
        }
    }
}




#region Extend Class

namespace com.Sconit.Service.Ext.Report.Impl
{
    [Transactional]
    public partial class RepRequisitionOrderMgrE : com.Sconit.Service.Report.Impl.RepRequisitionOrderMgr, IReportBaseMgrE
    {


    }
}

#endregion

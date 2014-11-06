using com.Sconit.Service.Ext.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;
using Castle.Services.Transaction;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.MasterData;

namespace com.Sconit.Service.Report.Impl
{
    [Transactional]
    public class RepDeliveryOrderMgr : RepTemplate1
    {
        public override string reportTemplateFolder { get; set; }

        public IOrderHeadMgrE orderHeadMgrE { get; set; }

        public RepDeliveryOrderMgr()
        {

            //明细部分的行数
            this.pageDetailRowCount = 31;
            //列数   1起始
            this.columnCount = 12;
            //报表头的行数  1起始
            this.headRowCount = 15;
            //报表尾的行数  1起始
            this.bottomRowCount = 7;

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
                    this.SetRowCell(pageIndex, rowIndex, 0,"" + orderDetail.Sequence);

                    //零件号 Item Code
                    this.SetRowCell(pageIndex, rowIndex, 1, orderDetail.Item.Code);

                    //参考号 Ref No.
                    this.SetRowCell(pageIndex, rowIndex, 2, orderDetail.ReferenceItemCode);

                    //描述Description
                    this.SetRowCell(pageIndex, rowIndex, 3, orderDetail.Item.Description);

                    //单位Unit
                    this.SetRowCell(pageIndex, rowIndex, 4, orderDetail.Item.Uom.Code);

                    //单包装UC
                    this.SetRowCell(pageIndex, rowIndex, 5, orderDetail.UnitCount.ToString("0.########"));

                    //需求 Request	包装
                    int UCs = (int)Math.Ceiling(orderDetail.OrderedQty / orderDetail.UnitCount);
                    this.SetRowCell(pageIndex, rowIndex, 6, UCs.ToString());

                    //需求 Request	零件数
                    this.SetRowCell(pageIndex, rowIndex, 7, orderDetail.OrderedQty.ToString("0.########"));

                  
                    //实发 Shipped	包装
                    this.SetRowCell(pageIndex, rowIndex, 8, "");

                    //实发 Shipped	零件数
                    this.SetRowCell(pageIndex, rowIndex, 9, orderDetail.ShippedQty.HasValue ? orderDetail.ShippedQty.Value.ToString("0.########") : string.Empty);

                    //收货数
                    this.SetRowCell(pageIndex, rowIndex, 10, orderDetail.ReceivedQty.HasValue ? orderDetail.ReceivedQty.Value.ToString("0.########") : string.Empty);

                    //批号/备注
                    this.SetRowCell(pageIndex, rowIndex, 11, orderDetail.TextField1);

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
            this.SetRowCell(2, 8, orderCode);
            //Order No.:
            this.SetRowCell(3, 8, orderHead.OrderNo);

            if ("Normal".Equals(orderHead.Priority))
            {
                this.SetRowCell(4, 5, "");
            }
            else
            {
                this.SetRowCell(3, 5, "");
            }

            //发出时间 Issue Time:
            this.SetRowCell(4, 9, orderHead.StartDate.ToString());

            //客户代码 Customer Code:	
            this.SetRowCell(6, 3, orderHead.PartyTo != null ? orderHead.PartyTo.Code : String.Empty);

            //到货日期 Delivery Date:getDemandDeliverDate
            this.SetRowCell(6, 8, orderHead.WindowTime.ToLongDateString());


            //客户名称 Supplier Name:		
            this.SetRowCell(7, 3, orderHead.PartyTo != null ? orderHead.PartyTo.Name : String.Empty);
            //窗口时间 Window Time:
            this.SetRowCell(7, 8, orderHead.WindowTime.ToLongTimeString());

            //客户地址 Address:	
            this.SetRowCell(8, 3, orderHead.ShipTo != null ? orderHead.ShipTo.Address : String.Empty);
            //交货道口 Delivery Dock:
           // this.SetRowCell(8, 8, orderHead.DockDescription);

            //客户联系人 Contact:	
            this.SetRowCell(9, 3, orderHead.ShipTo != null ? orderHead.ShipTo.ContactPersonName : String.Empty);
            //物流协调员 Follow Up:
            this.SetRowCell(9, 8, orderHead.ShipFrom != null ? orderHead.ShipFrom.ContactPersonName : String.Empty);

            //客户电话 Telephone:		
            this.SetRowCell(10, 3, orderHead.ShipTo != null ? orderHead.ShipTo.TelephoneNumber : String.Empty);
            //YFV电话 Telephone:
            this.SetRowCell(10, 8, orderHead.ShipFrom != null ? orderHead.ShipFrom.TelephoneNumber : String.Empty);

            //客户传真 Fax:	
            this.SetRowCell(11, 3, orderHead.ShipTo != null ? orderHead.ShipTo.Fax : String.Empty);
            //YFV传真 Fax:
            this.SetRowCell(11, 8, orderHead.ShipFrom != null ? orderHead.ShipFrom.Fax : String.Empty);

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
            this.CopyCell(pageIndex, 35 , 1, "B36");
            //YFK发单人签字:
            this.CopyCell(pageIndex, 35 , 5, "F36");
            //供应商签字:
            this.CopyCell(pageIndex, 35 , 9, "J36");


            //Actual Delivery Time:
            this.CopyCell(pageIndex, 36 , 1, "B37");
            //YFK Order Issuer Signature:
            this.CopyCell(pageIndex, 36 , 5, "F37");
            //Supplier Signature:
            this.CopyCell(pageIndex, 36 , 9, "J37");

            //1. 供应商交货后，请务必在要货单上签字，以确认实收数量，检查并带走延锋百利得收货单。
            this.CopyCell(pageIndex, 38 , 0, "A39");

            //    After delivery, please confirm Received Qty and sign in OL, then get YFV Receipt Notes before departure.
            this.CopyCell(pageIndex, 39 , 0, "A40");

            //承运商 Shipper:
            this.CopyCell(pageIndex, 39 , 9, "J40");

            //2. 延锋百利得收货单是唯一被延锋百利得承认的采购收货对帐凭证，请供应商妥善保存。
            this.CopyCell(pageIndex, 40 , 0, "A41");
            //到车时间 Arrival Time:
            this.CopyCell(pageIndex, 40 , 9, "J41");


            //    YFK Receipt Notes are the only legal account check vouchers for Goods Receiving, please keep them properly.
            this.CopyCell(pageIndex, 41 , 0, "A42");
            //离开时间 Departure Time:
            this.CopyCell(pageIndex, 41 , 9, "J42");

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
    public partial class RepDeliveryOrderMgrE : com.Sconit.Service.Report.Impl.RepDeliveryOrderMgr, IReportBaseMgrE
    {
      
        
    }
}

#endregion

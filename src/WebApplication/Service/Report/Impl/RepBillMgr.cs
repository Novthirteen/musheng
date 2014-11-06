using com.Sconit.Service.Ext.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;
using Castle.Services.Transaction;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.MasterData;

namespace com.Sconit.Service.Report.Impl
{
    [Transactional]
    public class RepBillMgr : RepTemplate1
    {
        public override string reportTemplateFolder { get; set; }
        public IReceiptMgrE receiptMgrE { get; set; }
        public IBillMgrE billMgrE { get; set; }
        public IBillDetailMgrE billDetailMgrE { get; set; }

        public RepBillMgr()
        {
            //明细部分的行数
            this.pageDetailRowCount = 27;
            //列数   1起始
            this.columnCount = 11;
            //报表头的行数  1起始
            this.headRowCount = 9;
            //报表尾的行数  1起始
            this.bottomRowCount = 0;

        }

        /**
         * 填充报表
         * 
         * Param list [0]OrderHead
         * Param list [0]IList<OrderDetail>           
         */
        protected override bool FillValuesImpl(String templateFileName, IList<object> list)
        {
            try
            {
                if (list == null || list.Count < 2) return false;

                Bill bill = (Bill)(list[0]);
                IList<BillDetail> billDetails = (IList<BillDetail>)(list[1]);

                if (bill == null
                    || billDetails == null || billDetails.Count == 0)
                {
                    return false;
                }
                /*
                var billDets = from billDetail in billDetails
                               where billDetail.ActingBill != null
                               orderby billDetail.ActingBill.Item.Code, billDetail.ActingBill.EffectiveDate, billDetail.IpNo
                               select billDetail;
                */
                //billDetail.ActingBill.Item, billDetail.ActingBill.EffectiveDate, 

                //billDetails.OrderBy(bd => bd.ActingBill.Item).OrderBy(bd => bd.ActingBill.EffectiveDate).OrderBy(bd => bd.IpNo);

                int group = billDetails.GroupBy(bd => bd.ActingBill.Item.Code).Count();
                //分类汇总+尾部的2行（包含合计）
                this.CopyPage(billDetails.Count + group + 2);

                this.FillHead(bill);

                int pageIndex = 1;
                int rowIndex = 0;
                int rowTotal = 0;
                decimal totalPrice = 0;

                decimal billedQtySum = decimal.Zero;
                //decimal unitPriceSum = decimal.Zero;
                decimal amountSum = decimal.Zero;
                string itemCode = string.Empty;
                for (int i = 0; i < billDetails.Count(); i++)
                {
                    BillDetail billDetail = billDetails[i];

                    //零件号
                    this.SetRowCell(pageIndex, rowIndex, 0, billDetail.ActingBill.Item.Code);
                    //零件名称	
                    this.SetRowCell(pageIndex, rowIndex, 1, billDetail.ActingBill.Item.Description);
                    //入库数量	
                    this.SetRowCell(pageIndex, rowIndex, 2, billDetail.BilledQty.ToString("0.########"));
                    //采购单价	
                    this.SetRowCell(pageIndex, rowIndex, 3, billDetail.UnitPrice.ToString("0.00000000"));
                    //单位
                    this.SetRowCell(pageIndex, rowIndex, 4, billDetail.ActingBill.Uom.Code);
                    //发票单价	
                    this.SetRowCell(pageIndex, rowIndex, 5, billDetail.UnitPrice.ToString("0.00000000"));
                    //发票金额
                    this.SetRowCell(pageIndex, rowIndex, 6, billDetail.Amount.ToString("0.00"));

                    //采购单号	
                    this.SetRowCell(pageIndex, rowIndex, 7, billDetail.ActingBill.OrderNo);
                    //入库单号	
                    this.SetRowCell(pageIndex, rowIndex, 8, billDetail.ActingBill.ReceiptNo);
                    //送货单号	
                    this.SetRowCell(pageIndex, rowIndex, 9, billDetail.IpNo);
                    //入库日期
                    this.SetRowCell(pageIndex, rowIndex, 10, billDetail.ActingBill.EffectiveDate.ToString("yyyy-MM-dd"));

                    totalPrice += billDetail.UnitPrice * billDetail.BilledQty;

                    if (this.isPageBottom(rowIndex, rowTotal))//页的最后一行
                    {
                        rowIndex = 0;
                        pageIndex++;
                        //totalPrice = 0;
                    }
                    else
                    {
                        rowIndex++;
                    }
                    rowTotal++;

                    /*
                    if (this.isLastPage(rowTotal))
                    {
                        //合计发票金额：
                        this.SetRowCell(pageIndex - 1, this.pageDetailRowCount, 8, "合计发票金额：");
                        this.SetRowCell(pageIndex - 1, this.pageDetailRowCount, 9, totalPrice.ToString("0.########"));
                    }
                    */

                    billedQtySum += billDetail.BilledQty;
                    amountSum += billDetail.Amount;
                    /*
                    if (billedQtySum != decimal.Zero)
                    {
                        unitPriceSum = amountSum / billedQtySum;
                    }
                    */
                    if (itemCode == string.Empty)
                    {
                        itemCode = billDetail.ActingBill.Item.Code;
                    }
                    if (i == (billDetails.Count - 1) || itemCode != billDetails[i + 1].ActingBill.Item.Code)
                    {
                        this.SetMergedRegion(pageIndex, rowIndex + this.headRowCount, 0, rowIndex + this.headRowCount, 1);
                        this.SetRowCell(pageIndex, rowIndex, 0, itemCode + "合计");
                        this.SetRowCell(pageIndex, rowIndex, 2, billedQtySum.ToString("0.########"));
                        //this.SetRowCell(pageIndex, rowIndex, 5, unitPriceSum.ToString("0.00000000"));
                        this.SetRowCell(pageIndex, rowIndex, 6, amountSum.ToString("0.00"));

                        billedQtySum = decimal.Zero;
                        //unitPriceSum = decimal.Zero;
                        amountSum = decimal.Zero;
                        if (i != (billDetails.Count - 1))
                        {
                            itemCode = billDetails[i + 1].ActingBill.Item.Code;
                        }

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
                }

                rowIndex++;
                if (rowIndex >= this.pageDetailRowCount - 2)
                {
                    this.SetRowCell(pageIndex, rowIndex, 0, "合计");
                    decimal amount = billDetails.Sum(bd => bd.Amount);
                    this.SetMergedRegion(pageIndex, rowIndex + this.headRowCount, 0, rowIndex + this.headRowCount, 1);
                    this.SetRowCell(pageIndex, rowIndex, 1, amount.ToString("0.00"));

                    rowIndex++;
                    this.SetRowCell(pageIndex, rowIndex, 0, "采购员：");
                    this.SetRowCell(pageIndex, rowIndex, 3, "主管：");
                }
                else
                {
                    decimal amount = billDetails.Sum(bd => bd.Amount);
                    if (bill.Discount.HasValue && bill.Discount.Value != 0)
                    {
                        this.SetRowCell(pageIndex, this.pageDetailRowCount - 2, 0, "合计");
                        this.SetRowCell(pageIndex, this.pageDetailRowCount - 2, 1, amount.ToString("0.00"));

                        this.SetRowCell(pageIndex, this.pageDetailRowCount - 2, 2, "折扣");
                        this.SetMergedRegion(pageIndex, this.pageDetailRowCount - 2 + this.headRowCount, 3, this.pageDetailRowCount - 2 + this.headRowCount, 4);
                        
                        this.SetRowCell(pageIndex, this.pageDetailRowCount - 2, 3, bill.Discount.Value.ToString("0.00"));

                        this.SetRowCell(pageIndex, this.pageDetailRowCount - 2, 5, "折后合计");
                        this.SetMergedRegion(pageIndex, this.pageDetailRowCount - 2 + this.headRowCount, 6, this.pageDetailRowCount - 2 + this.headRowCount, this.columnCount - 1);
                        amount -= bill.Discount.Value;
                        this.SetRowCell(pageIndex, this.pageDetailRowCount - 2, 6, amount.ToString("0.00"));
                    }
                    else
                    {
                        this.SetRowCell(pageIndex, this.pageDetailRowCount - 2, 0, "合计");
                        this.SetMergedRegion(pageIndex, this.pageDetailRowCount - 2 + this.headRowCount, 1, this.pageDetailRowCount - 2 + this.headRowCount, this.columnCount - 1);
                        this.SetRowCell(pageIndex, this.pageDetailRowCount - 2, 1, amount.ToString("0.00"));
                    }
                    this.SetRowCell(pageIndex, this.pageDetailRowCount - 1, 0, "采购员：");
                    this.SetRowCell(pageIndex, this.pageDetailRowCount - 1, 3, "主管：");
                }

                this.sheet.DisplayGridlines = false;
                this.sheet.IsPrintGridlines = false;
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
        private void FillHead(Bill bill)
        {
            //供应商:
            this.SetRowCell(2, 1, bill.BillAddress.Party.Name);
            //结算单号:
            this.SetRowCell(2, 9, bill.BillNo);
            //结算时间：
            if (bill.StartDate.HasValue || bill.EndDate.HasValue)
                this.SetRowCell(3, 1, (bill.StartDate.HasValue ? bill.StartDate.Value.ToString("yyyy-MM-dd") : string.Empty) + "～" + (bill.EndDate.HasValue ? bill.EndDate.Value.ToString("yyyy-MM-dd") : string.Empty));

        }

        /**
           * 需要拷贝的数据与合并单元格操作
           * 
           * Param pageIndex 页号
           */
        public override void CopyPageValues(int pageIndex)
        {
            //采购员：
            //this.CopyCell(pageIndex, 35, 0, "A36");
            //主管：
            //this.CopyCell(pageIndex, 35, 3, "D36");
            //合计发票金额：
            //this.CopyCell(pageIndex, 35, 8, "I36");
        }

        public override IList<object> GetDataList(string code)
        {
            IList<object> list = new List<object>();
            Bill bill = billMgrE.LoadBill(code);
            if (bill != null)
            {
                list.Add(bill);
                IList<BillDetail> billDetails = this.billDetailMgrE.GetBillDetailOrderByItem(code);
                list.Add(billDetails);
            }
            return list;
        }
    }
}

#region Extend Class

namespace com.Sconit.Service.Ext.Report.Impl
{
    [Transactional]
    public partial class RepBillMgrE : com.Sconit.Service.Report.Impl.RepBillMgr, IReportBaseMgrE
    {

    }
}

#endregion
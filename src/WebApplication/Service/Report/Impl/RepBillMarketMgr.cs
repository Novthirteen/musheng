using com.Sconit.Service.Ext.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Distribution;
using Castle.Services.Transaction;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Distribution;

namespace com.Sconit.Service.Report.Impl
{
    [Transactional]
    public class RepBillMarketMgr : RepTemplate1
    {
        public override string reportTemplateFolder { get; set; }

        public IBillMgrE billMgrE { get; set; }
        public IBillDetailMgrE billDetailMgrE { get; set; }
        public IReceiptMgrE receiptMgrE { get; set; }
        public IInProcessLocationMgrE inprocessLocationMgrE { get; set; }


        public RepBillMarketMgr()
        {
            //明细部分的行数
            this.pageDetailRowCount = 31;
            //列数   1起始
            this.columnCount = 8;
            //报表头的行数  1起始
            this.headRowCount = 5;
            //报表尾的行数  1起始
            this.bottomRowCount = 0;

        }

        /**
         * 填充报表
         * 
         * Param list [0]bill
         * Param list [0]IList<BillDetail>           
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

                int group = billDetails.GroupBy(bd => bd.ActingBill.Item.Code).Count();
                //分类汇总+尾部的2行（包含合计）
                this.CopyPage(billDetails.Count + group + 2);

                this.FillHead(bill);

                int pageIndex = 1;
                int rowIndex = 0;
                int rowTotal = 0;
                decimal billedQtySum = decimal.Zero;
                //decimal unitPriceSum = decimal.Zero;
                decimal amountSum = decimal.Zero;
                string itemCode = string.Empty;
                for (int i = 0; i < billDetails.Count(); i++)
                {
                    BillDetail billDetail = billDetails[i];

                    //产品名称
                    this.SetRowCell(pageIndex, rowIndex, 0, billDetail.ActingBill.Item.Desc1);
                    //产品图号				
                    this.SetRowCell(pageIndex, rowIndex, 1, billDetail.ActingBill.Item.Desc2);
                    //零件号
                    this.SetRowCell(pageIndex, rowIndex, 2, billDetail.ActingBill.Item.Code);
                    //单位	
                    this.SetRowCell(pageIndex, rowIndex, 3, billDetail.ActingBill.Uom.Code);
                    //出库数量	
                    this.SetRowCell(pageIndex, rowIndex, 4, billDetail.BilledQty.ToString("0.########"));
                    //单价	
                    this.SetRowCell(pageIndex, rowIndex, 5, billDetail.UnitPrice.ToString("0.00000000"));
                    //合计	
                    this.SetRowCell(pageIndex, rowIndex, 6, billDetail.Amount.ToString("0.00"));
                    //出库日期(发货日期)
                    InProcessLocation ipLocation = inprocessLocationMgrE.LoadInProcessLocation(billDetail.IpNo);
                    this.SetRowCell(pageIndex, rowIndex, 7, ipLocation.CreateDate.ToString("yyyy-MM-dd"));

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
                        this.SetRowCell(pageIndex, rowIndex, 4, billedQtySum.ToString("0.########"));
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
                    this.SetRowCell(pageIndex, rowIndex, 1, "对账员：");
                    this.SetRowCell(pageIndex, rowIndex, 5, "主管：");
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

                    this.SetRowCell(pageIndex, this.pageDetailRowCount - 1, 1, "对账员：");
                    this.SetRowCell(pageIndex, this.pageDetailRowCount - 1, 5, "主管：");
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
            this.SetRowCell(3, 2, bill.BillAddress.Party.Name);
            this.SetRowCell(3, 6, bill.BillNo);
            //this.SetRowCell(3, 5, bill.CreateDate.ToString("yyyy-MM-dd"));
        }

        /**
           * 需要拷贝的数据与合并单元格操作
           * 
           * Param pageIndex 页号
           */
        public override void CopyPageValues(int pageIndex)
        {
            //对账员：
            //this.CopyCell(pageIndex, 37, 1, "B38");
            //主管：
            //this.CopyCell(pageIndex, 37, 7, "H38");

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
    public partial class RepBillMarketMgrE : com.Sconit.Service.Report.Impl.RepBillMarketMgr, IReportBaseMgrE
    {

    }
}

#endregion

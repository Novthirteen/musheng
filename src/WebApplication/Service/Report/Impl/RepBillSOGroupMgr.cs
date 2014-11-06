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
    public class RepBillSOGroupMgr : RepTemplate1
    {
        public override string reportTemplateFolder { get; set; }
        public IBillMgrE billMgrE { get; set; }
        public IReceiptMgrE receiptMgrE { get; set; }
        public IInProcessLocationMgrE inprocessLocationMgrE { get; set; }
        public IBillDetailMgrE billDetailMgrE { get; set; }

        log4net.ILog log = log4net.LogManager.GetLogger("RepBillSOGroupMgr");


        public RepBillSOGroupMgr()
        {
            //明细部分的行数
            this.pageDetailRowCount = 46;
            //列数   1起始
            this.columnCount = 7;
            //报表头的行数  1起始
            this.headRowCount = 4;
            //报表尾的行数  1起始
            this.bottomRowCount = 3;

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

                this.CopyPage(billDetails.Count);

                this.FillHead(bill);

                int pageIndex = 1;
                int rowIndex = 0;
                int rowTotal = 0;

                foreach (BillDetail billDetail in billDetails)
                {
                    //零件号
                    this.SetRowCell(pageIndex, rowIndex, 0, billDetail.Item.Code);
                    //零件名称	
                    this.SetRowCell(pageIndex, rowIndex, 1, billDetail.Item.Description);
                    //出库数量	
                    this.SetRowCell(pageIndex, rowIndex, 2, billDetail.BilledQty.ToString("0.########"));
                    //单位	
                    this.SetRowCell(pageIndex, rowIndex, 3, billDetail.Uom.Code);
                    //单价	
                    this.SetRowCell(pageIndex, rowIndex, 4, billDetail.UnitPrice.ToString("0.00000000"));
                    //合计	
                    this.SetRowCell(pageIndex, rowIndex, 5, billDetail.GroupAmount.ToString("0.00"));

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

                //送货金额：
                decimal groupAmount = billDetails.Sum(bd => bd.GroupAmount);
                this.SetMergedRegion(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount + this.headRowCount, 3, this.pageDetailRowCount + this.headRowCount, 4);
                this.SetRowCell(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount, 3, "送货金额:");
                this.SetRowCell(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount, 5, groupAmount.ToString("0.00"));


                this.SetMergedRegion(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount + this.headRowCount + 1, 3, this.pageDetailRowCount + this.headRowCount + 1, 4);
                this.SetRowCell(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount + 1, 3, "整单折扣:");

                this.SetMergedRegion(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount + this.headRowCount + 2, 3, this.pageDetailRowCount + this.headRowCount + 2, 4);
                this.SetRowCell(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount + 2, 3, "开票金额:");

                if (bill.Discount.HasValue && bill.Discount.Value != 0)
                {
                    decimal discount = bill.Discount.Value;
                    this.SetRowCell(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount + 1, 5, discount.ToString("0.00"));
                    this.SetRowCell(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount + 2, 5, (groupAmount - discount).ToString("0.00"));
                }

                this.sheet.DisplayGridlines = false;
                this.sheet.IsPrintGridlines = false;
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
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
            this.SetRowCell(2, 1, bill.BillAddress.Party.Name);
            this.SetRowCell(2, 4, bill.BillNo);
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
            this.CopyCell(pageIndex, 51, 0, "A52");
            //主管：
            this.CopyCell(pageIndex, 51, 1, "B52");

        }

        public override IList<object> GetDataList(string code)
        {
            IList<object> list = new List<object>();
            Bill bill = billMgrE.LoadBill(code, true);
            if (bill != null)
            {
                list.Add(bill);
                list.Add(billDetailMgrE.GroupBillDetail(bill.BillDetails));
            }
            return list;
        }
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.Report.Impl
{
    [Transactional]
    public partial class RepBillSOGroupMgrE : com.Sconit.Service.Report.Impl.RepBillSOGroupMgr, IReportBaseMgrE
    {

    }
}

#endregion

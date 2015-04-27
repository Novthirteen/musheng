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
using System.Text.RegularExpressions;

namespace com.Sconit.Service.Report.Impl
{
    [Transactional]
    public class RepBillPOMgr : RepTemplate1
    {
        public override string reportTemplateFolder { get; set; }
        public IReceiptMgrE receiptMgrE { get; set; }
        public IBillMgrE billMgrE { get; set; }
        public IBillDetailMgrE billDetailMgrE { get; set; }

        public RepBillPOMgr()
        {
            //明细部分的行数
            this.pageDetailRowCount = 22;
            //列数   1起始
            this.columnCount = 6;
            //报表头的行数  1起始
            this.headRowCount = 8;
            //报表尾的行数  1起始
            this.bottomRowCount = 19;
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

                this.CopyPage(billDetails.Count);
                bill.Amount = billDetails.Sum(d => d.GroupAmountRound) - Math.Round(bill.Discount.Value * (decimal)1.17,2);
                this.FillHead(bill);

                int pageIndex = 1;
                int rowIndex = 0;
                int rowTotal = 0;
                
                foreach (BillDetail billDetail in billDetails)
                {

                    //零件号
                    this.SetRowCell(pageIndex, rowIndex, 0, billDetail.Item.Code);
                    //客户零件号
                    //this.SetRowCell(pageIndex, rowIndex, 1, billDetail.ReferenceItemCode);//todo
                    //零件名称	
                    this.SetRowCell(pageIndex, rowIndex, 1, billDetail.Item.Description);
                    //入库数量	
                    this.SetRowCell(pageIndex, rowIndex, 2, billDetail.BilledQty.ToString("0.########"));
                    //单位	
                    this.SetRowCell(pageIndex, rowIndex, 3, billDetail.Uom.Code);
                    //发票单价
                    this.SetRowCell(pageIndex, rowIndex, 4, billDetail.UnitPrice.ToString("0.00000000"));
                    //合计
                    string cGroupAmount=billDetail.GroupAmount.ToString("c");
                    this.SetRowCell(pageIndex, rowIndex, 5, cGroupAmount.Substring(1));
                    if (this.isPageBottom(rowIndex, rowTotal+1))//页的最后一行
                    {

                        //送货金额：
                        decimal groupAmount = billDetails.Sum(bd => bd.GroupAmount);
                        this.SetRowCell(pageIndex, this.pageDetailRowCount, 5, groupAmount.ToString("c"));
                        if (bill.Discount.HasValue && bill.Discount.Value != 0)
                        {
                            decimal discount = bill.Discount.Value;
                            this.SetRowCell(pageIndex, this.pageDetailRowCount + 1, 5, discount.ToString("c"));
                            this.SetRowCell(pageIndex, this.pageDetailRowCount + 2, 5, (groupAmount - discount).ToString("c"));
                        }

                        pageIndex++;
                        rowIndex = 0;
                    }
                    else
                    {
                        rowIndex++;
                    }
                    rowTotal++;
                }
                
                //合计发票金额：
                /*
                
                this.SetMergedRegion(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount + this.headRowCount, 3, this.pageDetailRowCount + this.headRowCount, 4);
                this.SetRowCell(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount, 3, "合计发票金额：");
                this.SetRowCell(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount, 5, billDetails.Sum(bd => bd.GroupAmount).ToString("0.0000"));
                */

              

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
         */
        private void FillHead(Bill bill)
        {
           
            //供应商:
            this.SetRowCell(1, 1, bill.BillAddress.Party.Name);
            //结算单号:
            this.SetRowCell(1, 4, bill.BillNo);
            //结算时间：
            if (bill.StartDate.HasValue || bill.EndDate.HasValue)
            {
                this.SetRowCell(2, 1, (bill.StartDate.HasValue ? bill.StartDate.Value.ToString("yyyy-MM-dd") : string.Empty) + "～" + (bill.EndDate.HasValue ? bill.EndDate.Value.ToString("yyyy-MM-dd") : string.Empty));
            }

             //发票日期  40 B C D 
            this.SetRowCell(39, 1, bill.InvoiceDate.HasValue ? "             " + bill.InvoiceDate.Value.Year + " 年 " + bill.InvoiceDate.Value.Month + " 月 " + bill.InvoiceDate.Value.Day + " 日 " : "                年     月     日");

            //供应商:
            this.SetRowCell(41, 1, bill.BillAddress.Party.Name);
            //摘要:
            this.SetRowCell(43, 1, "货款#"+bill.ExternalBillNo);
            //金额:
            this.SetRowCell(43, 3, bill.Amount.ToString("c"));
            //备注:付款时间
            //this.SetRowCell(44, 5, bill.PaymentDate.HasValue ? bill.PaymentDate.Value.ToLongDateString() : "");
            //this.SetRowCell(44, 5, bill.BillAddress.Party.Aging.ToString() + "天");
            var endDay = bill.PaymentDate.Value.AddMonths(1).AddDays(-bill.PaymentDate.Value.Day);
            var fridayDate = System.DateTime.Now;
            var dayOfWeek = endDay.DayOfWeek;
            switch (dayOfWeek)
            {
                case DayOfWeek.Friday:
                    fridayDate = endDay;
                    break;
                case DayOfWeek.Thursday:
                    fridayDate = endDay.AddDays(-6);
                    break;
                case DayOfWeek.Wednesday:
                    fridayDate = endDay.AddDays(-5);
                    break;
                case DayOfWeek.Tuesday:
                    fridayDate = endDay.AddDays(-4);
                    break;
                case DayOfWeek.Monday:
                    fridayDate = endDay.AddDays(-3);
                    break;
                case DayOfWeek.Sunday:
                    fridayDate = endDay.AddDays(-2);
                    break;
                case DayOfWeek.Saturday:
                    fridayDate = endDay.AddDays(-1);
                    break;
                default:
                    fridayDate = endDay;
                    break;
            }
            this.SetRowCell(44, 5, fridayDate.ToShortDateString());

            //金额: (大小写)+ "         (小写)" + (bill.Amount * (decimal)1.17).ToString("0.00")
            string bigAmount = ConvertToChinese((double)bill.Amount );
            this.SetRowCell(46, 1, "（大写）" + bigAmount);

            //金额:
            this.SetRowCell(46, 5, bill.Amount.ToString("c")); 

        }

        /**
           * 需要拷贝的数据与合并单元格操作
           * 
           * Param pageIndex 页号
           */
        public override void CopyPageValues(int pageIndex)
        {


            this.SetMergedRegion(pageIndex, 35, 0, 36, 5);
            this.SetMergedRegion(pageIndex, 37, 0, 38, 5);
            this.SetMergedRegion(pageIndex, 39, 1, 39, 3);
            this.SetMergedRegion(pageIndex, 41, 1, 41, 3);
            this.SetMergedRegion(pageIndex, 42, 0, 45, 0);
            this.SetMergedRegion(pageIndex, 43, 1, 43, 2);
            this.SetMergedRegion(pageIndex, 44, 1, 44, 2);
            this.SetMergedRegion(pageIndex, 45, 1, 45, 2);
            this.SetMergedRegion(pageIndex, 46, 1, 46, 5);

            //采购员：
            this.CopyCell(pageIndex, 31, 0, "A32");
            //主管：
            this.CopyCell(pageIndex, 31, 1, "B32");
            this.CopyCell(pageIndex, 30, 4, "E31");
            this.CopyCell(pageIndex, 31, 4, "E32");
            this.CopyCell(pageIndex, 32, 4, "E33");

            //上 海 慕 盛 实 业 有 限 公 司
            this.CopyCell(pageIndex, 35, 0, "A36");
            this.CopyCell(pageIndex, 37, 0, "A38");
            this.CopyCell(pageIndex, 39, 0, "A40");
            this.CopyCell(pageIndex, 39, 1, "B40");
            this.CopyCell(pageIndex, 39, 5, "F40");
            //受款人
            this.CopyCell(pageIndex, 41, 0, "A42");
            this.CopyCell(pageIndex, 41, 1, "B42");
            this.CopyCell(pageIndex, 41, 4, "E42");
            this.CopyCell(pageIndex, 42, 0, "A43");
            //摘要:"货款#"+bill.ExternalBillNo
            this.CopyCell(pageIndex, 43, 1, "B44");
            //金额
            this.CopyCell(pageIndex, 43, 3, "D44");
            this.CopyCell(pageIndex, 42, 1, "B43");
            this.CopyCell(pageIndex, 42, 3, "D43");
            this.CopyCell(pageIndex, 42, 4, "E43");
            this.CopyCell(pageIndex, 43, 4, "E44");
            this.CopyCell(pageIndex, 44, 4, "E45");
            //备注：付款时间
            this.CopyCell(pageIndex, 44, 5, "F45");
            this.CopyCell(pageIndex, 45, 4, "E46");
            this.CopyCell(pageIndex, 45, 5, "F46");
            this.CopyCell(pageIndex, 46, 0, "A47");
            //金额:(大小写)
            this.CopyCell(pageIndex, 46, 1, "B47");
            this.CopyCell(pageIndex, 46, 4, "E47");
            this.CopyCell(pageIndex, 46, 5, "F47");
            this.CopyCell(pageIndex, 47, 0, "A48");
            this.CopyCell(pageIndex, 47, 1, "B48");
            this.CopyCell(pageIndex, 47, 3, "D48");
            this.CopyCell(pageIndex, 47, 5, "F48");
            


        }




        static string ConvertToChinese(double x)
        {

            string s = x.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            return Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟萬億兆京垓秭穰"[m.Value[0] - '-'].ToString());
        } 

    }
}

#region Extend Class

namespace com.Sconit.Service.Ext.Report.Impl
{
    [Transactional]
    public partial class RepBillPOMgrE : com.Sconit.Service.Report.Impl.RepBillPOMgr, IReportBaseMgrE
    {

    }
}

#endregion
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
    public class RepBillPOGroupMgr : RepTemplate1
    {
        public override string reportTemplateFolder { get; set; }
        public IReceiptMgrE receiptMgrE { get; set; }
        public IBillMgrE billMgrE { get; set; }
        public IBillDetailMgrE billDetailMgrE { get; set; }

        public RepBillPOGroupMgr()
        {
            //明细部分的行数
            this.pageDetailRowCount = 32;
            //列数   1起始
            this.columnCount = 6;
            //报表头的行数  1起始
            this.headRowCount = 11;
            //报表尾的行数  1起始
            this.bottomRowCount = 1;
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
                bill.Amount = billDetails.Sum(d => d.GroupAmount);
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
                
                //合计发票金额：
                /*
                
                this.SetMergedRegion(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount + this.headRowCount, 3, this.pageDetailRowCount + this.headRowCount, 4);
                this.SetRowCell(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount, 3, "合计发票金额：");
                this.SetRowCell(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount, 5, billDetails.Sum(bd => bd.GroupAmount).ToString("0.0000"));
                */

                ////送货金额：
                //decimal groupAmount = billDetails.Sum(bd => bd.GroupAmount);
                //this.SetMergedRegion(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount + this.headRowCount, 3, this.pageDetailRowCount + this.headRowCount, 4);
                //this.SetRowCell(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount, 3, "送货金额:");
                //this.SetRowCell(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount, 5, groupAmount.ToString("0.00"));


                //this.SetMergedRegion(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount + this.headRowCount + 1, 3, this.pageDetailRowCount + this.headRowCount + 1, 4);
                //this.SetRowCell(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount + 1, 3, "整单折扣:");

                //this.SetMergedRegion(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount + this.headRowCount + 2, 3, this.pageDetailRowCount + this.headRowCount + 2, 4);
                //this.SetRowCell(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount + 2, 3, "开票金额:");

                //if (bill.Discount.HasValue && bill.Discount.Value != 0)
                //{
                //    decimal discount = bill.Discount.Value;
                //    this.SetRowCell(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount + 1, 5, discount.ToString("0.00"));
                //    this.SetRowCell(rowIndex == 0 ? pageIndex - 1 : pageIndex, this.pageDetailRowCount + 2, 5, (groupAmount - discount).ToString("0.00"));
                //}

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
            this.SetRowCell(3, 1, bill.BillAddress.Party.Name);
            //摘要:
            this.SetRowCell(5, 1, "货款#"+bill.ExternalBillNo);
            //金额:
            this.SetRowCell(5, 2, bill.Amount.ToString("0.00"));
            //备注:付款时间
            this.SetRowCell(6, 4, bill.PaymentDate.HasValue ? bill.PaymentDate.Value.ToLongDateString() : "");

            //金额:(大小写)
            string bigAmount = ConvertToChinese((double)bill.Amount);
            this.SetRowCell(8, 1, "（大写）" + bigAmount + "         (小写)" + bill.Amount.ToString("0.00"));

        }

        /**
           * 需要拷贝的数据与合并单元格操作
           * 
           * Param pageIndex 页号
           */
        public override void CopyPageValues(int pageIndex)
        {
            //采购员：
            this.CopyCell(pageIndex, 43, 0, "A44");
            //主管：
            this.CopyCell(pageIndex, 43, 3, "D44");
        }




        static string ConvertToChinese(double x)
        {

            string s = x.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            return Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟萬億兆京垓秭穰"[m.Value[0] - '-'].ToString());
        } 


        ///// <summary>
        ///// 金额小写转中文大写。
        ///// 整数支持到万亿；小数部分支持到分(超过两位将进行Banker舍入法处理)
        ///// </summary>
        ///// <param name="Num">需要转换的双精度浮点数</param>
        ///// <returns>转换后的字符串</returns>
        //public static String NumGetStr(decimal Num)
        //{
        //      string[] Ls_ShZ = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖", "拾" };
        //      string[] Ls_DW_Zh = { "元", "拾", "佰", "仟", "万", "拾", "佰", "仟", "亿", "拾", "佰", "仟", "万" };
        //      string[] Num_DW = { "", "拾", "佰", "仟", "万", "拾", "佰", "仟", "亿", "拾", "佰", "仟", "万" };
        //      string[] Ls_DW_X = { "角", "分" };

        //    Boolean iXSh_bool = false;//是否含有小数，默认没有(0则视为没有)
        //    Boolean iZhSh_bool = true;//是否含有整数,默认有(0则视为没有)

        //    string NumStr;//整个数字字符串
        //    string NumStr_Zh;//整数部分
        //    string NumSr_X = "";//小数部分
        //    string NumStr_DQ;//当前的数字字符
        //    string NumStr_R = "";//返回的字符串

        //    Num = Math.Round(Num, 2);//四舍五入取两位


        //    //各种非正常情况处理
        //    if (Num < 0)
        //    {
        //        return "不转换欠条";
        //    }
        //    if (Num > 9999999999999)
        //    {
        //        return "很难想象谁会有这么多钱！";
        //    }
        //    if (Num == 0)
        //    {
        //        return Ls_ShZ[0];
        //    }

        //    //判断是否有整数
        //    if (Num < 1)
        //        iZhSh_bool = false;

        //    NumStr = Num.ToString();

        //    NumStr_Zh = NumStr;//默认只有整数部分
        //    if (NumStr_Zh.Contains("."))
        //    {//分开整数与小数处理
        //        NumStr_Zh = NumStr.Substring(0, NumStr.IndexOf("."));
        //        NumSr_X = NumStr.Substring((NumStr.IndexOf(".") + 1), (NumStr.Length - NumStr.IndexOf(".") - 1));
        //        iXSh_bool = true;
        //    }


        //    if (NumSr_X == "" || int.Parse(NumSr_X) <= 0)
        //    {//判断是否含有小数部分
        //        iXSh_bool = false;
        //    }

        //    if (iZhSh_bool)
        //    {//整数部分处理
        //        NumStr_Zh = Reversion_Str(NumStr_Zh);//反转字符串

        //        for (int a = 0; a < NumStr_Zh.Length; a++)
        //        {//整数部分转换
        //            NumStr_DQ = NumStr_Zh.Substring(a, 1);
        //            if (int.Parse(NumStr_DQ) != 0)
        //                NumStr_R = Ls_ShZ[int.Parse(NumStr_DQ)] + Ls_DW_Zh[a] + NumStr_R;
        //            else if (a == 0 || a == 4 || a == 8)
        //            {
        //                if (NumStr_Zh.Length > 8 && a == 4)
        //                    continue;
        //                NumStr_R = Ls_DW_Zh[a] + NumStr_R;
        //            }
        //            else if (int.Parse(NumStr_Zh.Substring(a - 1, 1)) != 0)
        //                NumStr_R = Ls_ShZ[int.Parse(NumStr_DQ)] + NumStr_R;

        //        }

        //        if (!iXSh_bool)
        //            return NumStr_R + "整";

        //        //NumStr_R += "零";
        //    }

        //    for (int b = 0; b < NumSr_X.Length; b++)
        //    {//小数部分转换
        //        NumStr_DQ = NumSr_X.Substring(b, 1);
        //        if (int.Parse(NumStr_DQ) != 0)
        //            NumStr_R += Ls_ShZ[int.Parse(NumStr_DQ)] + Ls_DW_X[b];
        //        else if (b != 1 && iZhSh_bool)
        //            NumStr_R += Ls_ShZ[int.Parse(NumStr_DQ)];
        //    }

        //    return NumStr_R;

        //}


    }
}

#region Extend Class

namespace com.Sconit.Service.Ext.Report.Impl
{
    [Transactional]
    public partial class RepBillPOGroupMgrE : com.Sconit.Service.Report.Impl.RepBillPOGroupMgr, IReportBaseMgrE
    {

    }
}

#endregion
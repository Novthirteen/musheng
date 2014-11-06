using com.Sconit.Service.Ext.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;
using Castle.Services.Transaction;
using NPOI.HSSF.UserModel;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity;

namespace com.Sconit.Service.Report.Impl
{
    /**
     * 
     * 原材料条码
     * 
     */
    [Transactional]
    public class RepBarCodeMgr : RepTemplate1
    {
        public override string reportTemplateFolder { get; set; }
        public IHuMgrE huMgrE { get; set; }
        public IItemReferenceMgrE itemReferenceMgrE { get; set; }
        int resultCountByPage = 0;
        public RepBarCodeMgr()
        {

            //明细部分的行数
            this.pageDetailRowCount = 9;
            //每页的记录数
            resultCountByPage = 1;

            //列数   1起始
            this.columnCount = 4;
            //报表尾的行数  1起始
            this.bottomRowCount = 0;

            this.headRowCount = 0;

        }


        /**
         * 需要拷贝的数据与合并单元格操作
         * 
         * Param pageIndex 页号
         */
        public override void CopyPageValues(int pageIndex)
        {
            this.SetMergedRegion(pageIndex, 0, 0, 0, 2);
            this.SetMergedRegion(pageIndex, 1, 1, 1, 2);
            this.SetMergedRegion(pageIndex, 2, 1, 2, 3);
            this.SetMergedRegion(pageIndex, 3, 1, 3, 3);
            this.SetMergedRegion(pageIndex, 5, 1, 5, 3);

            this.CopyCell(pageIndex, 2, 0, "A3");
            this.CopyCell(pageIndex, 3, 0, "A4");
            this.CopyCell(pageIndex, 4, 0, "A5");
            this.CopyCell(pageIndex, 4, 2, "C5");
            this.CopyCell(pageIndex, 5, 0, "A6");

            this.CopyCell(pageIndex, 6, 0, "A7");
            this.CopyCell(pageIndex, 6, 2, "C7");
            this.CopyCell(pageIndex, 7, 0, "A8");
            this.CopyCell(pageIndex, 7, 2, "C8");
            this.CopyCell(pageIndex, 8, 0, "A9");
            this.CopyCell(pageIndex, 8, 2, "C9");
        }
        /*
          * 计算页数
          * 
          * Param resultCount 记录明细数
          * 
          */
        protected override int PageCount(int resultCount)
        {
            return (int)Math.Ceiling(resultCount / (resultCountByPage * 1.0)); ;
        }
        /**
         * 填充报表
         * 
         * Param list [0]huDetailList
         */
        [Transaction(TransactionMode.Requires)]
        protected override bool FillValuesImpl(String templateFileName, IList<object> list)
        {
            try
            {
                IList<Hu> huList = null;
                if(list[0].GetType() == typeof(Hu))
                {
                    huList = new List<Hu>();
                    huList.Add((Hu)list[0]);
                }
                else if(list[0].GetType() == typeof(List<Hu>))
                {
                    huList = (IList<Hu>)list[0];
                }
                else
                {
                    return false;
                }

                string userName = "";
                if(list.Count == 2)
                {
                    userName = (string)list[1];
                }

                this.sheet.DisplayGridlines = false;
                this.sheet.IsPrintGridlines = false;

                //this.sheet.DisplayGuts = false;

                int count = 0;
                foreach(Hu hu in huList)
                {
                    if(hu.Item.Type.Equals("M") //成品
                            || hu.Item.Type.Equals("P")
                            || hu.Item.Type.Equals("C")) //原材料
                    {
                        count++;
                    }
                }

                if(count == 0) return false;

                this.barCodeFontName = this.GetBarcodeFontName(0, 0);

                //加页删页
                this.CopyPage(count);


                int pageIndex = 1;

                foreach(Hu hu in huList)
                {
                    if(hu.Item.Type.Equals("M") || hu.Item.Type.Equals("P") || hu.Item.Type.Equals("C")) //成品
                    {
                        hu.PrintCount += 1;
                        if(hu.PrintCount > 1)
                        {
                            this.SetRowCell(pageIndex, 0, 3, "MS " + hu.PrintCount);
                        }
                        else
                        {
                            this.SetRowCell(pageIndex, 0, 3, "MS");
                        }

                        //hu id内容
                        string barCode = Utility.BarcodeHelper.GetBarcodeStr(hu.HuId, this.barCodeFontName);
                        this.SetRowCell(pageIndex, 0, 0, barCode);

                        //MSL湿敏等级
                        this.SetRowCell(pageIndex, 1, 0, string.Format(" MSL:{0}", hu.Item.Msl));

                        //hu id内容
                        this.SetRowCell(pageIndex, 1, 1, hu.HuId);

                        //制造库格
                        this.SetRowCell(pageIndex, 1, 3, hu.Item.Bin);

                        // 物料号
                        this.SetRowCell(pageIndex, 2, 1, hu.Item.Code);

                        // 物料描述
                        this.SetRowCell(pageIndex, 3, 1, hu.Item.Description);
                        //单位
                        this.SetRowCell(pageIndex, 4, 1, hu.Uom.Code);
                        // 数量
                        this.SetRowCell(pageIndex, 4, 3, hu.Qty.ToString("0.########"));
                        // 供应商批号
                        this.SetRowCell(pageIndex, 5, 1, hu.SupplierLotNo == null ? string.Empty : hu.SupplierLotNo);
                        //品牌		
                        this.SetRowCell(pageIndex, 6, 1, hu.Item.ItemBrand == null ? string.Empty : hu.Item.ItemBrand.Description);
                        //批号
                        this.SetRowCell(pageIndex, 6, 3, hu.ManufactureDate.ToString("yyyy-MM-dd"));
                        //分光1		
                        this.SetRowCell(pageIndex, 7, 1, hu.SortLevel1);
                        //分色2
                        this.SetRowCell(pageIndex, 7, 3, hu.ColorLevel1);
                        //分光1	
                        this.SetRowCell(pageIndex, 8, 1, hu.SortLevel2);
                        //分色2
                        this.SetRowCell(pageIndex, 8, 3, hu.ColorLevel2);

                        //this.sheet.SetRowBreak(this.GetRowIndexAbsolute(pageIndex, ROW_COUNT - 1));
                        pageIndex++;
                    }
                }

                foreach(Hu hu in huList)
                {
                    if(hu.Item.Type.Equals("M") || hu.Item.Type.Equals("P") || hu.Item.Type.Equals("C")) //成品  //原材料
                    {
                        huMgrE.UpdateHu(hu);
                    }
                }
            }
            catch(Exception e)
            {
                return false;
            }

            return true;
        }
    }

}




#region Extend Class

namespace com.Sconit.Service.Ext.Report.Impl
{
    /**
     * 
     * 原材料条码
     * 
     */
    [Transactional]
    public partial class RepBarCodeMgrE : com.Sconit.Service.Report.Impl.RepBarCodeMgr, IReportBaseMgrE
    {



    }

}

#endregion

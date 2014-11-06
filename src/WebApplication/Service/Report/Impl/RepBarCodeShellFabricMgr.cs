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
    public class RepBarCodeShellFabricMgr : RepTemplate2
    {
        public override string reportTemplateFolder { get; set; }
        public IHuMgrE huMgrE { get; set; }
        public IItemReferenceMgrE itemReferenceMgrE { get; set; }
        public IEntityPreferenceMgrE entityPreferenceMgrE { get; set; }

        public RepBarCodeShellFabricMgr()
        {

            //明细部分的行数
            this.pageDetailRowCount = 11;
            //列数   1起始
            this.columnCount = 4;
            //报表头的行数  1起始
            this.leftColumnHeadCount = 1;
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
            this.SetMergedRegion(pageIndex, 1, 1, 1, 3);
            this.SetMergedRegion(pageIndex, 2, 1, 2, 3);
            this.SetMergedRegion(pageIndex, 3, 1, 3, 3);
            this.SetMergedRegion(pageIndex, 4, 1, 4, 3);
            this.SetMergedRegion(pageIndex, 5, 1, 5, 3);
            this.SetMergedRegion(pageIndex, 9, 1, 9, 3);

            //this.CopyCell(pageIndex, 0, 1, "C1");
            this.CopyCell(pageIndex, 6, 2, "C7");
            this.CopyCell(pageIndex, 7, 2, "C8");
            this.CopyCell(pageIndex, 8, 2, "C9");
            //this.CopyCell(pageIndex, 9, 1, "C10");
            this.CopyCell(pageIndex, 10, 2, "C11");
            this.CopyCell(pageIndex, 11, 2, "C12");

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
                if (list[0].GetType() == typeof(Hu))
                {
                    huList = new List<Hu>();
                    huList.Add((Hu)list[0]);
                }
                else if (list[0].GetType() == typeof(IList<Hu>))
                {
                    huList = (IList<Hu>)list[0];
                }
                else
                {
                    return false;
                }
                string userName = "";
                if (list.Count == 2)
                {
                    userName = (string)list[1];
                }

                this.sheet.DisplayGridlines = false;
                this.sheet.IsPrintGridlines = false;

                //this.sheet.DisplayGuts = false;

                int count = 0;
                foreach (Hu hu in huList)
                {
                    if (hu.Item.Type.Equals("M") //成品
                            || hu.Item.Type.Equals("P")) //原材料
                    {
                        count++;
                    }
                }

                if (count == 0) return false;

                this.barCodeFontName = this.GetBarcodeFontName(1, 1);

                //加页删页
                this.CopyPage(count);


                int pageIndex = 1;

                foreach (Hu hu in huList)
                {
                    if (hu.Item.Type.Equals("M") || hu.Item.Type.Equals("P")) //成品
                    {

                        if (hu.PrintCount > 1)
                        {
                            this.SetRowCell(pageIndex, 2, 3, hu.PrintCount);
                        }
                        hu.PrintCount += 1;

                        //文件号：XNG-QA-FR-51
                        this.SetRowCell(pageIndex, 0, 1, "文件号：XNG-QA-FR-51");
                        //hu id内容
                        string barCode = Utility.BarcodeHelper.GetBarcodeStr(hu.HuId, this.barCodeFontName);
                        this.SetRowCell(pageIndex, 1, 1, barCode);

                        //hu id内容
                        this.SetRowCell(pageIndex, 2, 1, hu.HuId);

                        // 物料号
                        this.SetRowCell(pageIndex, 3, 1, hu.Item.Code);

                        // 物料描述
                        this.SetRowCell(pageIndex, 4, 1, hu.Item.Description);

                        // 参考号
                        IList<ItemReference> itemReferenceList = itemReferenceMgrE.GetItemReference(hu.Item.Code);
                        if (itemReferenceList != null && itemReferenceList.Count > 0)
                        {
                            this.SetRowCell(pageIndex, 5, 1, itemReferenceList[0].ReferenceCode);
                        }

                        //单位
                        this.SetRowCell(pageIndex, 6, 1, hu.Uom.Code);
                        // 数量
                        this.SetRowCell(pageIndex, 6, 3, hu.Qty.ToString("0.########"));
                        // 生产日期
                        this.SetRowCell(pageIndex, 7, 1, hu.ManufactureDate.ToString("yyyy-MM-dd"));

                        //批号
                        this.SetRowCell(pageIndex, 7, 3, hu.LotNo);
                        // 生产线
                        this.SetRowCell(pageIndex, 8, 1, hu.HuId.Substring(0, (hu.HuId.Length - 9)) + "#");
                        //检验员
                        this.SetRowCell(pageIndex, 8, 3, hu.CreateUser.Code);
                        // 制造商
                        //this.SetRowCell(pageIndex, 9, 1, hu.ManufactureParty == null ? string.Empty : hu.ManufactureParty.Name);
                        string companyName = entityPreferenceMgrE.LoadEntityPreference("CompanyName").Value;
                        this.SetRowCell(pageIndex, 9, 1, companyName == null || companyName == string.Empty ? string.Empty : companyName);
                        // 打印日期
                        this.SetRowCell(pageIndex, 11, 1, DateTime.Now.ToString("yyyy-MM-dd"));
                        //打印人
                        this.SetRowCell(pageIndex, 11, 3, userName);

                        //this.sheet.SetRowBreak(this.GetRowIndexAbsolute(pageIndex, ROW_COUNT - 1));
                        pageIndex++;
                    }
                }

                foreach (Hu hu in huList)
                {
                    if (hu.Item.Type.Equals("M") || hu.Item.Type.Equals("P")) //成品  //原材料
                    {
                        huMgrE.UpdateHu(hu);
                    }
                }
            }
            catch (Exception e)
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
    public partial class RepBarCodeShellFabricMgrE : com.Sconit.Service.Report.Impl.RepBarCodeShellFabricMgr, IReportBaseMgrE
    {



    }

}

#endregion

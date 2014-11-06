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
    public class RepBomDetailMgr : ReportBaseMgr
    {
        public override string reportTemplateFolder { get; set; }

        public IBomMgrE bomMgrE { get; set; }


        public RepBomDetailMgr()
        {
        }

        /**
         * 填充报表
         * 
         * Param list [0]bill
         * Param list [0]IList<BomDetail>           
         */
        public override bool FillValues(String templateFileName, IList<object> list)
        {
            try
            {
                if (list == null || list.Count < 1) return false;


                IList<BomDetail> bomDetails = (IList<BomDetail>)(list[0]);


                if (bomDetails == null || bomDetails.Count == 0)
                {
                    return false;
                }
                this.init(templateFileName);
                int rowIndex = 1;

                foreach (BomDetail bomDetail in bomDetails)
                {

                    //No.	
                    this.SetRowCell(rowIndex, 0, rowIndex);
                    //工序	
                    this.SetRowCell(rowIndex, 1, bomDetail.Operation);
                    //参考	
                    this.SetRowCell(rowIndex, 2, bomDetail.Reference);
                    //父物料	
                    this.SetRowCell(rowIndex, 3, bomDetail.Bom.Code);
                    //子物料	
                    this.SetRowCell(rowIndex, 4, bomDetail.Item.Code);
                    //单位	
                    this.SetRowCell(rowIndex, 5, bomDetail.Uom.Code);
                    //类型	
                    this.SetRowCell(rowIndex, 6, bomDetail.StructureType);
                    //开始时间	
                    this.SetRowCell(rowIndex, 7, bomDetail.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    //结束时间	
                    if (bomDetail.EndDate.HasValue)
                        this.SetRowCell(rowIndex, 8, bomDetail.EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    //用量	
                    this.SetRowCell(rowIndex, 9, double.Parse(bomDetail.RateQty.ToString("0.########")));
                    //废品率	
                    if (bomDetail.ScrapPercentage.HasValue)
                        this.SetRowCell(rowIndex, 10, double.Parse(bomDetail.ScrapPercentage.Value.ToString("0.########")));
                    //回冲方式	
                    this.SetRowCell(rowIndex, 11, bomDetail.BackFlushMethod);
                    //发货扫描条码	
                    this.SetRowCell(rowIndex, 12, bomDetail.IsShipScanHu.ToString());
                    //打印	
                    this.SetRowCell(rowIndex, 13, bomDetail.NeedPrint.ToString());
                    //库位	
                    if (bomDetail.Location != null)
                        this.SetRowCell(rowIndex, 14, bomDetail.Location.Code);
                    //优先级
                    this.SetRowCell(rowIndex, 15, bomDetail.Priority);

                    rowIndex++;
                }

                //this.sheet.DisplayGridlines = false;
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
        private void FillHead(Bom bom)
        {

        }

        /**
           * 需要拷贝的数据与合并单元格操作
           * 
           * Param pageIndex 页号
           */
        public override void CopyPageValues(int pageIndex)
        {

        }

        public override IList<object> GetDataList(string code)
        {
            IList<object> list = new List<object>();

            return list;
        }
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.Report.Impl
{
    [Transactional]
    public partial class RepBomDetailMgrE : com.Sconit.Service.Report.Impl.RepBomDetailMgr, IReportBaseMgrE
    {

    }
}

#endregion

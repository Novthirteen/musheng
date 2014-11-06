using com.Sconit.Service.Ext.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;
using Castle.Services.Transaction;
using com.Sconit.Utility;
using com.Sconit.Entity;
using com.Sconit.Service.Ext.MasterData;

namespace com.Sconit.Service.Report.Impl
{

    /**
     * 
     * 不合格品处理单
     * 
     */
    [Transactional]
    public class RepBelowBradeMgr : RepTemplate1
    {
        public override string reportTemplateFolder { get; set; }
        public ICodeMasterMgrE codeMasterMgrE { get; set; }
        public IInspectOrderMgrE inspectOrderMgrE { get; set; }
        

        public RepBelowBradeMgr()
        {
            //明细部分的行数
            this.pageDetailRowCount = 7;
            //列数  1起始
            this.columnCount = 8;
            //报表头的行数  1起始
            this.headRowCount = 10;
            //报表尾的行数  1起始
            this.bottomRowCount = 20;
        }

        /**
         * 填充报表
         * 
         * Param list [0]InspectOrder
         * Param list [0]IList<InspectOrderDetail>           
         */
        [Transaction(TransactionMode.Requires)]
        protected override bool FillValuesImpl(String templateFileName, IList<object> list)
        {
            try
            {

                if (list == null || list.Count < 2) return false;

                InspectOrder inspectOrder = (InspectOrder)(list[0]);
                IList<InspectOrderDetail> inspectOrderDetails = (IList<InspectOrderDetail>)(list[1]);


                if (inspectOrder == null
                    || inspectOrderDetails == null || inspectOrderDetails.Count == 0)
                {
                    return false;
                }


                int count = 0;
                foreach (InspectOrderDetail inspectOrderDetail in inspectOrderDetails)
                {
                    if (inspectOrderDetail.RejectedQty > 0)
                    {
                        count++;
                    }
                }
                if (count == 0) return false;

                this.CopyPage(count);

                this.FillHead(inspectOrder);

                int pageIndex = 1;
                int rowIndex = 0;
                int rowTotal = 0;
                foreach (InspectOrderDetail inspectOrderDetail in inspectOrderDetails)
                {
                    if (inspectOrderDetail.RejectedQty > 0)
                    {
                        //零件名称     Part Name	
                        this.SetRowCell(pageIndex, rowIndex, 0, inspectOrderDetail.LocationLotDetail.Item.Description);
                        //"零件号Part No."
                        this.SetRowCell(pageIndex, rowIndex, 1, inspectOrderDetail.LocationLotDetail.Item.Code);
                        //工位号      Sta. No.
                        this.SetRowCell(pageIndex, rowIndex, 2, string.Empty);
                        //"数量     QTY."
                        this.SetRowCell(pageIndex, rowIndex, 3, inspectOrderDetail.RejectedQty.Value.ToString("0.########"));
                        //"缺陷                     Defect"		
                        this.SetRowCell(pageIndex, rowIndex, 4, string.Empty);
                        //处理方法             Disposition 
                        if (inspectOrderDetail.Disposition != null && inspectOrderDetail.Disposition != string.Empty)
                        {
                            CodeMaster codeMaster = codeMasterMgrE.GetCachedCodeMaster(BusinessConstants.CODE_MASTER_INSPECT_DISPOSITION, inspectOrderDetail.Disposition);
                            if (codeMaster != null && codeMaster.Description != null && codeMaster.Description.Length > 0)
                            {
                                this.SetRowCell(pageIndex, rowIndex, 5, codeMaster.Description); //inspectOrderDetail.Disposition
                            }
                        }
                        //起末库位	
                        this.SetRowCell(pageIndex, rowIndex, 6, inspectOrderDetail.LocationFrom.Code);
                        //总成零件号	
                        this.SetRowCell(pageIndex, rowIndex, 7, inspectOrderDetail.FinishGoods == null ? string.Empty : inspectOrderDetail.FinishGoods.Code);


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

                this.sheet.DisplayGridlines = false;
                this.sheet.IsPrintGridlines = false;

                if (inspectOrder.IsPrinted == null || inspectOrder.IsPrinted == false)
                {
                    inspectOrder.IsPrinted = true;
                    inspectOrderMgrE.UpdateInspectOrder(inspectOrder);
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
        private void FillHead(InspectOrder inspectOrder)
        {
            //序号
            this.SetRowCell(2, 6, inspectOrder.InspectNo);
            //部门/小组
            //this.SetRowCell(5, 1,  );
            //班次
            //this.SetRowCell(5, 3, inspectOrder );
            //填写人
            this.SetRowCell(5, 4, inspectOrder.CreateUser.Code);
            //日期
            this.SetRowCell(5, 6, inspectOrder.CreateDate.ToString("yyyy-MM-dd HH:mm"));
        }

        /**
           * 需要拷贝的数据与合并单元格操作
           * 
           * Param pageIndex 页号
           */
        public override void CopyPageValues(int pageIndex)
        {
            this.SetMergedRegion(pageIndex, 27, 5, 27, 6);

            this.CopyCell(pageIndex, 17, 0, "A18");
            this.CopyCell(pageIndex, 17, 1, "B18");
            this.CopyCell(pageIndex, 19, 0, "A20");
            this.CopyCell(pageIndex, 20, 0, "A21");
            this.CopyCell(pageIndex, 22, 0, "A23");
            this.CopyCell(pageIndex, 22, 3, "D23");
            this.CopyCell(pageIndex, 22, 5, "F23");


            this.CopyCell(pageIndex, 23, 0, "A24");
            this.CopyCell(pageIndex, 23, 3, "D24");
            this.CopyCell(pageIndex, 23, 5, "F24");

            this.CopyCell(pageIndex, 24, 0, "A25");
            this.CopyCell(pageIndex, 25, 0, "A26");

            this.CopyCell(pageIndex, 27, 5, "F28");

            this.CopyCell(pageIndex, 28, 0, "A29");
            this.CopyCell(pageIndex, 29, 0, "A30");
            this.CopyCell(pageIndex, 30, 0, "A31");
            this.CopyCell(pageIndex, 31, 0, "A32");
            this.CopyCell(pageIndex, 32, 0, "A33");
            this.CopyCell(pageIndex, 33, 0, "A34");

            this.CopyCell(pageIndex, 35, 0, "A36");
            this.CopyCell(pageIndex, 35, 2, "C36");
            this.CopyCell(pageIndex, 35, 4, "E36");
            this.CopyCell(pageIndex, 35, 6, "G36");

            this.CopyCell(pageIndex, 36, 0, "A37");
            this.CopyCell(pageIndex, 36, 2, "C37");
            this.CopyCell(pageIndex, 36, 4, "E37");
            this.CopyCell(pageIndex, 36, 6, "G37");
        }

        public override IList<object> GetDataList(string code)
        {
            IList<object> list = new List<object>();
            InspectOrder inspectOrder = inspectOrderMgrE.LoadInspectOrder(code, true);
            if (inspectOrder != null)
            {
                list.Add(inspectOrder);
                list.Add(inspectOrder.InspectOrderDetails);
            }
            return list;
        }
    }
}




#region Extend Class
namespace com.Sconit.Service.Ext.Report.Impl
{

    /**
     * 
     * 不合格品处理单
     * 
     */
    [Transactional]
    public partial class RepBelowBradeMgrE : com.Sconit.Service.Report.Impl.RepBelowBradeMgr, IReportBaseMgrE
    {
       
    }
}

#endregion

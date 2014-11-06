using com.Sconit.Service.Ext.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;
using Castle.Services.Transaction;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity;

namespace com.Sconit.Service.Report.Impl
{
    [Transactional]
    public class RepPickListMgr : RepTemplate1
    {
        public override string reportTemplateFolder { get; set; }
        public IPickListMgrE pickListMgrE { get; set; }
        public IFlowMgrE flowMgrE { get; set; }


        public RepPickListMgr()
        {
            //明细部分的行数
            this.pageDetailRowCount = 40;
            //列数   1起始
            this.columnCount = 9;
            //报表头的行数  1起始
            this.headRowCount = 7;
            //报表尾的行数  1起始
            this.bottomRowCount = 1;
        }

        /**
         * 填充报表
         * 
         * Param list [0]PickList
         *            
         */
        [Transaction(TransactionMode.Requires)]
        protected override bool FillValuesImpl(String templateFileName, IList<object> list)
        {
            try
            {
                PickList pickList = (PickList)list[0];
                IList<PickListDetail> pickListDetails = pickList.PickListDetails;

                if (pickList == null
                    || pickListDetails == null || pickListDetails.Count == 0)
                {
                    return false;
                }

                this.barCodeFontName = this.GetBarcodeFontName(2, 5);
                //this.SetRowCellBarCode(0, 2, 5);
                this.CopyPage(pickListDetails.Count);

                this.FillHead(pickList);


                int pageIndex = 1;
                int rowIndex = 0;
                int rowTotal = 0;

                foreach (PickListDetail pickListDetail in pickListDetails)
                {

                    //"订单号Order No."
                    this.SetRowCell(pageIndex, rowIndex, 0, pickListDetail.OrderLocationTransaction.OrderDetail.OrderHead.OrderNo);

                    //物料号
                    this.SetRowCell(pageIndex, rowIndex, 1, pickListDetail.Item.Code);

                    //描述Description
                    this.SetRowCell(pageIndex, rowIndex, 2, pickListDetail.Item.Description);

                    //"单位Unit"
                    this.SetRowCell(pageIndex, rowIndex, 3, pickListDetail.Item.Uom.Code);

                    //单包装UC
                    this.SetRowCell(pageIndex, rowIndex, 4, pickListDetail.UnitCount.ToString("0.########"));

                    //库格
                    this.SetRowCell(pageIndex, rowIndex, 5, pickListDetail.StorageBin != null ? pickListDetail.StorageBin.Code : pickListDetail.Location.Code);
                    //分光分色1
                    //this.SetRowCell(pageIndex, rowIndex, 6, pickListDetail.SortLevel1 + " " + pickListDetail.ColorLevel1);
                    //	分光分色2
                    //this.SetRowCell(pageIndex, rowIndex, 7, pickListDetail.SortLevel2 + " " + pickListDetail.ColorLevel2);

                    ///入库日期
                    if (pickListDetail.LotNo != null && pickListDetail.LotNo != string.Empty)
                        this.SetRowCell(pageIndex, rowIndex, 6, pickListDetail.LotNo);
                    //数量
                    this.SetRowCell(pageIndex, rowIndex, 7, pickListDetail.Qty.ToString("0.########"));

                    if (pickList.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_COMPLETE
                        || pickList.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)
                    {
                        if (pickListDetail.PickListResults != null && pickListDetail.PickListResults.Count > 0)
                        {
                            decimal pickListResultQty = pickListDetail.PickListResults.Sum(pr => pr.Qty);
                            this.SetRowCell(pageIndex, rowIndex, 8, pickListResultQty.ToString("0.########"));
                        }
                        else
                        {
                            this.SetRowCell(pageIndex, rowIndex, 8, "0");
                        }
                    }
                    else if (pickListDetail.LotNo == null || pickListDetail.LotNo == string.Empty)
                    {
                        this.SetRowCell(pageIndex, rowIndex, 8, pickListDetail.Memo);
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

                this.sheet.DisplayGridlines = false;
                this.sheet.IsPrintGridlines = false;

                if (pickList.IsPrinted == null || pickList.IsPrinted == false)
                {
                    pickList.IsPrinted = true;
                    pickListMgrE.UpdatePickList(pickList);
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
         * Param pickList 订单头对象
         */
        private void FillHead(PickList pickList)
        {
            //订单号:
            string orderCode = Utility.BarcodeHelper.GetBarcodeStr(pickList.PickListNo, this.barCodeFontName);
            this.SetRowCell(2, 5, orderCode);
            //目的库位
            this.SetRowCell(3, 2, pickList.Location == null ? string.Empty : (pickList.Location.Code + pickList.Location.Name));

            //Order No.:
            this.SetRowCell(3, 5, pickList.PickListNo);

            //订单时间
            this.SetRowCell(4, 5, pickList.CreateDate.ToString("yyyy-MM-dd HH:mm"));
        }

        /**
           * 需要拷贝的数据与合并单元格操作
           * 
           * Param pageIndex 页号
           */
        public override void CopyPageValues(int pageIndex)
        {
            //拣货时间:
            this.CopyCell(pageIndex, 47, 1, "B48");
        }

        public override IList<object> GetDataList(string code)
        {
            IList<object> list = new List<object>();
            PickList pickList = pickListMgrE.LoadPickList(code, true, true);
            if (pickList != null)
            {
                list.Add(pickList);
            }
            return list;
        }

    }
}




#region Extend Class







namespace com.Sconit.Service.Ext.Report.Impl
{
    [Transactional]
    public partial class RepPickListMgrE : com.Sconit.Service.Report.Impl.RepPickListMgr, IReportBaseMgrE
    {


    }
}

#endregion

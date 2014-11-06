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
using com.Sconit.Entity.Customize;
using com.Sconit.Service.Ext.Customize;


namespace com.Sconit.Service.Report.Impl
{

    ///
    ///作用：生产单
    ///作者：tiansu
    ///编写日期：2010-01-22
    ///
    [Transactional]
    public class RepProdutLineFeedSeqenceMgr : RepTemplate3
    {

        public override string reportTemplateFolder { get; set; }
        public IOrderHeadMgrE orderHeadMgr { get; set; }
        public IFlowMgrE flowMgr { get; set; }
        public IProdutLineFeedSeqenceMgrE produtLineFeedSeqenceMgr { get; set; }

        public RepProdutLineFeedSeqenceMgr()
        {
            
            //明细部分的行数 
            this.pageDetailRowCount = 50;
            //列数   1起始
            this.columnCount = 8;
            //报表头的行数  1起始
            this.headRowCount = 10;
            //报表尾的行数  1起始
            this.bottomRowCount = 0;

            this.fieldsCount = 2;
        }

        /*
         * 填充报表头
         * 
         * Param pageIndex 页号
         * Param orderHead 订单头对象
         * Param orderDetails 订单明细对象
         */
        protected void FillHead(OrderHead orderHead, IList<OrderDetail> orderDetails)
        {

            #region 报表头

            this.SetRowCell(0, 6, orderHead.Sequence.HasValue ? orderHead.Sequence.ToString() : string.Empty);

            //工单号码Order code
            string orderCode = Utility.BarcodeHelper.GetBarcodeStr(orderHead.OrderNo, this.barCodeFontName);
            this.SetRowCell(2, 6, orderCode);

            // "生产线：Prodline："
            Flow flow = this.flowMgr.LoadFlow(orderHead.Flow);
            this.SetRowCell(2, 1, flow.Description + "(" + flow.Code + ")");
            //"生产班组：Shift："
            this.SetRowCell(3, 1, orderHead.Shift == null ? string.Empty : orderHead.Shift.Code);
            //"设备Facility"
            this.SetRowCell(4, 1, orderHead.ProductLineFacility);
            


            // "生产单号：No. PO："
            this.SetRowCell(3, 6, orderHead.OrderNo);
            
            #endregion

            int rowIndex = 7;
            #region 产品信息  Product Information

            foreach (OrderDetail orderDetail in orderDetails)
            {

                if (rowIndex == 2)
                {
                    break;
                }
                //"成品物料号 FG Item Code"	
                this.SetRowCell(rowIndex, 0, orderDetail.Item.Code);
                //"描述Description"	
                this.SetRowCell(rowIndex, 1, orderDetail.Item.Description);
                //"单位Unit"	
                this.SetRowCell(rowIndex, 5, orderDetail.Uom.Code);
                //"包装UC"	
                this.SetRowCell(rowIndex, 6, orderDetail.UnitCount.ToString("0.########"));
                //"计划数Dmd Qty"	
                this.SetRowCell(rowIndex, 7, orderDetail.OrderedQty.ToString("0.########"));
                //"合格数Conf Qty"	
                //this.SetRowCell(pageIndex, rowIndex, 5, string.Empty);
                //"不合格数NC Qty"	
                //this.SetRowCell(pageIndex, rowIndex, 6, string.Empty);
                //"废品数Scrap Qty"	
                //this.SetRowCell(pageIndex, rowIndex, 7, string.Empty);
                //"收货人Receiver"	
                //this.SetRowCell(pageIndex, rowIndex, 8, string.Empty);
                // "收货日期Rct Date"
                //this.SetRowCell(pageIndex, rowIndex, 9, string.Empty);

                rowIndex++;


            }

            #endregion

        }

        /**
         * 需要拷贝的数据与合并单元格操作
         * 
         * Param pageIndex 页号
         */
        public override void CopyPageValues(int pageIndex)
        {
        }


        /**
         * 填充报表
         * 
         * Param list [0]订单头对象
         *            [1]订单明细对象
         *            [2]订单库位事物对象
         */
        [Transaction(TransactionMode.Requires)]
        protected override bool FillValuesImpl(String templateFileName, IList<object> list)
        {
            try
            {
                OrderHead orderHead = (OrderHead)(list[0]);
                IList<OrderDetail> orderDetails = (IList<OrderDetail>)(list[1]);
                IList<ProdutLineFeedSeqence> produtLineFeedSeqences = (IList<ProdutLineFeedSeqence>)(list[2]);


                var query = from o in produtLineFeedSeqences
                            orderby o.Sequence ascending
                            select o;
                produtLineFeedSeqences = query.ToList();

                 
                //this.SetRowCellBarCode(1, 2, 6);
                this.barCodeFontName = this.GetBarcodeFontName(2, 6);

                this.CopyPage(produtLineFeedSeqences.Count);


                this.FillHead(orderHead, orderDetails);

                #region 上料信息  Material Information

                int pageIndex = 1;
                int rowTotal = 0;
                int rowIndex = 0;
                int seq = 1;
                int colpos = 0;

                foreach (ProdutLineFeedSeqence produtLineFeedSeqence in produtLineFeedSeqences)
                {
                    //序号
                    this.SetRowCell(pageIndex, rowIndex, 0 + colpos, seq++);
                    //"物料号Item Code"	
                    this.SetRowCell(pageIndex, rowIndex, 1 + colpos, produtLineFeedSeqence.RawMaterial.Code);
                    // "描述Description"	
                    this.SetRowCell(pageIndex, rowIndex, 2 + colpos, produtLineFeedSeqence.RawMaterial.Description);
                    // "站位数 Qty"	
                    this.SetRowCell(pageIndex, rowIndex, 3 + colpos, produtLineFeedSeqence.Code);

                    if (colpos == 4 && this.isPageBottom(rowIndex, rowTotal))//页的最后一行
                    {
                        pageIndex++;
                        rowIndex = 0;
                        colpos = 0;
                    }
                    else
                    {
                        if (rowIndex != 0 && (rowIndex + 1) % (this.pageDetailRowCount / this.fieldsCount) == 0)
                        {
                            colpos = 4;
                            rowIndex = 0;
                        }
                        else
                        {
                            rowIndex++;
                        }
                    }
                    rowTotal++;
                }

                #endregion

                if (orderHead.IsPrinted == null || orderHead.IsPrinted == false)
                {
                    orderHead.IsPrinted = true;
                    orderHeadMgr.UpdateOrderHead(orderHead);
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public override IList<object> GetDataList(string code)
        {
            IList<object> list = new List<object>();
            OrderHead orderHead = orderHeadMgr.LoadOrderHead(code, true);
            if (orderHead != null)
            {
                list.Add(orderHead);
                list.Add(orderHead.OrderDetails);
                IList<ProdutLineFeedSeqence> produtLineFeedSeqences = produtLineFeedSeqenceMgr.GetActualProdutLineFeedSeqence(code);
                list.Add(produtLineFeedSeqences);
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
     * 原材料条码
     * 
     */
    [Transactional]
    public partial class RepProdutLineFeedSeqenceMgrE : com.Sconit.Service.Report.Impl.RepProdutLineFeedSeqenceMgr, IReportBaseMgrE
    {


    }

}

#endregion

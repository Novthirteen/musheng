using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using Castle.Services.Transaction;
using com.Sconit.Utility;
using com.Sconit.Service.MasterData;
using com.Sconit.Entity.View;
using com.Sconit.Service.Report;
using com.Sconit.Service.Ext.Business;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Report;


namespace com.Sconit.Service.Business.Impl
{
    public class TransferMgr : AbstractBusinessMgr
    {
        public ISetBaseMgrE setBaseMgrE { get; set; }
        public ISetDetailMgrE setDetailMgrE { get; set; }
        public IExecuteMgrE executeMgrE { get; set; }
        public IFlowMgrE flowMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }
        public ILocationLotDetailMgrE locationLotDetailMgrE { get; set; }
        public IItemMgrE itemMgrE { get; set; }
        public IOrderDetailMgrE orderDetailMgrE { get; set; }
        public IOrderMgrE orderMgrE { get; set; }
        public ILanguageMgrE languageMgrE { get; set; }
        public IReceiptDetailMgrE receiptDetailMgrE { get; set; }
        public IReportMgrE reportMgrE { get; set; }
        public IStorageBinMgrE storageBinMgrE { get; set; }
        public ILocationMgrE locationMgrE { get; set; }

        protected override void SetBaseInfo(Resolver resolver)
        {
            if (resolver.BarcodeHead == BusinessConstants.BARCODE_HEAD_BIN)
            {
                setBaseMgrE.FillResolverByBin(resolver);
            }
            //暂不支持不扫描物流路线移库到库位
            //else if (resolver.BarcodeHead == BusinessConstants.BARCODE_HEAD_LOCATION)
            //{
            //    setBaseMgrE.FillResolverByLocation(resolver);
            //}
            else if (resolver.BarcodeHead == BusinessConstants.BARCODE_HEAD_FLOW)
            {
                setBaseMgrE.FillResolverByFlow(resolver);//支持快速采购
                //if (resolver.OrderType != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_TRANSFER)
                //{
                //    throw new BusinessErrorException("Flow.Error.FlowTypeIsNotTransfer", resolver.OrderType);
                //}
            }
            else
            {
                throw new BusinessErrorException("Common.Business.Error.BarCodeInvalid");
            }
        }

        protected override void GetDetail(Resolver resolver)
        {
            ///setBaseMgrE.FillDetailByFlow(resolver);///
        }

        /// <summary>
        /// 只有发货扫描条码才支持不扫物流路线,扫描库格移库
        /// </summary>
        /// <param name="resolver"></param>
        protected override void SetDetail(Resolver resolver)
        {
            List<string> flowTypes = new List<string>();
            flowTypes.Add(BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_TRANSFER);
            flowTypes.Add(BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT);
            flowTypes.Add(BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_CUSTOMERGOODS);
            flowTypes.Add(BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_SUBCONCTRACTING);
            FlowView flowView = null;
            //扫描条码
            if (resolver.IsScanHu || (resolver.BinCode != null && resolver.BinCode != string.Empty))
            {
                Hu hu = huMgrE.CheckAndLoadHu(resolver.Input);
                if (this.locationMgrE.IsHuOcuppyByPickList(resolver.Input))
                {
                    throw new BusinessErrorException("Order.Error.PickUp.HuOcuppied", resolver.Input);
                }
                //直接根据FlowCode去匹配
                if (resolver.FlowCode != null && resolver.FlowCode.Trim() != string.Empty)
                {
                    flowView = flowMgrE.CheckAndLoadFlowView(resolver.FlowCode, null, null, null, hu, flowTypes);
                }
                //根据目的库格Bin和Hu匹配物流路线
                else
                {
                    if (resolver.BinCode == null || resolver.BinCode == string.Empty)
                    {
                        throw new BusinessErrorException("Common.Business.Error.ScanFlowOrStorageBinFirst");
                    }
                    else
                    {
                        flowView = flowMgrE.CheckAndLoadFlowView(null, resolver.UserCode, hu.Location, resolver.LocationToCode, hu, flowTypes);
                        setBaseMgrE.FillResolverByFlow(resolver, flowView.Flow);
                    }
                }
                setDetailMgrE.MatchHuByFlowView(resolver, flowView, hu);
            }
            //不扫描条码
            else
            {
                Item item = itemMgrE.CheckAndLoadItem(resolver.Input);
                //直接根据FlowCode去匹配
                if (resolver.FlowCode != null && resolver.FlowCode.Trim() != string.Empty)
                {
                    flowView = flowMgrE.CheckAndLoadFlowView(resolver.FlowCode, null, null, null, item, flowTypes);
                }
                else
                {
                    throw new BusinessErrorException("Common.Business.Error.ScanFlowFirst");
                }
                Transformer transformer = Utility.TransformerHelper.ConvertItemToTransformer(flowView, item);
                if (resolver.Transformers == null || resolver.Transformers.Count == 0)
                {
                    transformer.Sequence = 1;
                }
                else
                {
                    transformer.Sequence = resolver.Transformers.Select(t => t.Sequence).Max() + 1;
                }
                resolver.AddTransformer(transformer);
            }
        }

        protected override void ExecuteSubmit(Resolver resolver)
        {
            this.TransferOrder(resolver);
        }

        protected override void ExecuteCancel(Resolver resolver)
        {
            executeMgrE.CancelOperation(resolver);
        }


        /// <summary>
        /// 移库
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="transformerList"></param>
        /// <returns></returns>
        [Transaction(TransactionMode.Unspecified)]
        public void TransferOrder(Resolver resolver)
        {
            IList<OrderDetail> orderDetails = executeMgrE.ConvertResolverToOrderDetails(resolver);
            resolver.IOType = (resolver.IOType == null || resolver.IOType == string.Empty) ?
                resolver.IOType = BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML : resolver.IOType;
            //Receipt receipt = orderMgrE.QuickReceiveOrder(resolver.Code, orderDetails, resolver.UserCode);
            Receipt receipt = orderMgrE.QuickReceiveOrder2(resolver.Code, orderDetails, resolver.UserCode, resolver.IOType, DateTime.Now, DateTime.Now, false, null, null);
            #region Print
            if (resolver.NeedPrintReceipt && resolver.IsCSClient)
            {
                receipt.ReceiptDetails = receiptDetailMgrE.SummarizeReceiptDetails(receipt.ReceiptDetails);

                IList<object> list = new List<object>();
                list.Add(receipt);
                list.Add(receipt.ReceiptDetails);
                resolver.PrintUrl = reportMgrE.WriteToFile("ReceiptNotes.xls", list);
            }
            #endregion
            resolver.Result = languageMgrE.TranslateMessage("Common.Business.RecNoIs", resolver.UserCode, receipt.ReceiptNo);
            resolver.Code = receipt.ReceiptNo;
            resolver.Transformers = null;//TransformerHelper.ConvertReceiptToTransformer(receipt.ReceiptDetails);
            resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMER;
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override void ExecutePrint(Resolver resolver)
        {
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override void GetReceiptNotes(Resolver resolver)
        {
        }
    }
}



#region Extend Class

namespace com.Sconit.Service.Ext.Business.Impl
{
    public partial class TransferMgrE : com.Sconit.Service.Business.Impl.TransferMgr, IBusinessMgrE
    {
    }
}

#endregion
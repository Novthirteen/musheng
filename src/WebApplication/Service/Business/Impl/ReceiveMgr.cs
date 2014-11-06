using com.Sconit.Service.Ext.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.Distribution;
using com.Sconit.Service.Distribution;
using com.Sconit.Utility;
using Castle.Services.Transaction;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Service.Ext.Report;

namespace com.Sconit.Service.Business.Impl
{
    public class ReceiveMgr : AbstractBusinessMgr
    {
        public ILanguageMgrE languageMgrE { get; set; }
        public ISetBaseMgrE setBaseMgrE { get; set; }
        public ISetDetailMgrE setDetailMgrE { get; set; }
        public IExecuteMgrE executeMgrE { get; set; }
        public IOrderHeadMgrE orderHeadMgrE { get; set; }
        public IOrderMgrE orderMgrE { get; set; }
        public IInProcessLocationMgrE inProcessLocationMgrE { get; set; }
        public ILocationLotDetailMgrE locationLotDetailMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }
        public IOrderLocationTransactionMgrE orderLocationTransactionMgrE { get; set; }
        public IInspectOrderMgrE inspectOrderMgrE { get; set; }
        public IReportMgrE reportMgrE { get; set; }

        protected override void SetBaseInfo(Resolver resolver)
        {
            if (resolver.BarcodeHead == BusinessConstants.BARCODE_HEAD_BIN)
            {
                setBaseMgrE.FillResolverByBin(resolver);
            }
            else if (resolver.CodePrefix == BusinessConstants.CODE_PREFIX_ORDER)
            {
                resolver.Transformers = null;
                setBaseMgrE.FillResolverByOrder(resolver);

                #region 校验
                if (resolver.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)
                    throw new BusinessErrorException("Order.Error.StatusErrorWhenReceive", resolver.Status, resolver.Code);

                if (resolver.OrderType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
                {
                    throw new BusinessErrorException("Order.Error.OrderShipIsNotProduction", resolver.Code, resolver.OrderType);
                }
                #endregion
            }
            else if (resolver.CodePrefix == BusinessConstants.CODE_PREFIX_ASN)
            {
                resolver.Transformers = null;
                setBaseMgrE.FillResolverByASN(resolver);

                if (resolver.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)
                {
                    throw new BusinessErrorException("InProcessLocation.Error.StatusErrorWhenReceive", resolver.Status, resolver.Code);
                }
            }
            else
            {
                throw new TechnicalException("Error BarcodeHead:" + resolver.BarcodeHead + " and CodePrefix:" + resolver.CodePrefix);
            }
        }

        protected override void GetDetail(Resolver resolver)
        {
            InProcessLocation inProcessLocation = null;
            //订单收货
            if (resolver.CodePrefix == BusinessConstants.CODE_PREFIX_ORDER)
            {
                inProcessLocation = orderMgrE.ConvertOrderToInProcessLocation(resolver.Input);
            }
            //ASN收货
            else if (resolver.CodePrefix == BusinessConstants.CODE_PREFIX_ASN)
            {
                inProcessLocation = inProcessLocationMgrE.LoadInProcessLocation(resolver.Code, true);
            }
            if (inProcessLocation == null || inProcessLocation.InProcessLocationDetails == null || inProcessLocation.InProcessLocationDetails.Count == 0)
            {
                throw new BusinessErrorException("Common.Business.Error.NoDetailToReceive");
            }


            List<Transformer> newTransformerList = TransformerHelper.ConvertInProcessLocationDetailsToTransformers(inProcessLocation.InProcessLocationDetails);

            if (resolver.IsScanHu)
            {
                ClearReceivedQty(newTransformerList);
            }
            resolver.Transformers = resolver.Transformers == null ? new List<Transformer>() : resolver.Transformers;
            resolver.Transformers.AddRange(newTransformerList);
            resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMER;

            if (resolver.Transformers != null && resolver.Transformers.Count > 0)
            {
                foreach (Transformer transformer in resolver.Transformers)
                {
                    OrderLocationTransaction outOrderLocationTransaction = this.orderLocationTransactionMgrE.LoadOrderLocationTransaction(transformer.OrderLocTransId);
                    OrderLocationTransaction inOrderLocationTransaction = orderLocationTransactionMgrE.GetOrderLocationTransaction(outOrderLocationTransaction.OrderDetail.Id, BusinessConstants.IO_TYPE_IN)[0];

                    transformer.OrderLocTransId = inOrderLocationTransaction.Id;
                }
            }
        }

        protected override void SetDetail(Resolver resolver)
        {
            if (resolver.CodePrefix == string.Empty)
            {
                throw new BusinessErrorException("Common.Business.Error.ScanFlowFirst");
            }
            if (resolver.ModuleType == BusinessConstants.TRANSFORMER_MODULE_TYPE_PRODUCTIONRECEIVE)
            {
                LocationLotDetail locationLotDetail = locationLotDetailMgrE.CheckLoadHuLocationLotDetail(resolver.Input, resolver.UserCode);
                TransformerDetail newTransformerDetail = TransformerHelper.ConvertLocationLotDetailToTransformerDetail(locationLotDetail, false);
                resolver.AddTransformerDetail(newTransformerDetail);
            }
            else
            {
                setDetailMgrE.MatchReceive(resolver);
                if (resolver.BinCode != null && resolver.BinCode != string.Empty)
                {
                    resolver.Result = languageMgrE.TranslateMessage("Warehouse.CurrentBinCode", resolver.UserCode, resolver.BinCode);
                }
            }
        }

        protected override void ExecuteSubmit(Resolver resolver)
        {
            resolver = this.ReceiveOrder(resolver);
        }

        protected override void ExecuteCancel(Resolver resolver)
        {
            executeMgrE.CancelOperation(resolver);
        }

        /// <summary>
        /// 物流收货
        /// </summary>
        /// <param name="resolver"></param>
        /// <returns></returns>
        [Transaction(TransactionMode.Unspecified)]
        public Resolver ReceiveOrder(Resolver resolver)
        {
            ReceiptDetail recDet = new ReceiptDetail();
            IList<ReceiptDetail> receiptDetailList = orderMgrE.ConvertTransformerToReceiptDetail(resolver.Transformers);
            InProcessLocation ip = null;
            if (resolver.CodePrefix == BusinessConstants.CODE_PREFIX_ASN)
            {
                ip = inProcessLocationMgrE.LoadInProcessLocation(resolver.Code);
            }
            Receipt receipt = orderMgrE.ReceiveOrder(receiptDetailList, resolver.UserCode, ip, resolver.ExternalOrderNo);
            resolver.Transformers = TransformerHelper.ConvertReceiptToTransformer(receipt.ReceiptDetails);
            resolver.Code = receipt.ReceiptNo;
            resolver.BinCode = string.Empty;
            resolver.Result = languageMgrE.TranslateMessage("Common.Business.RecNoIs", resolver.UserCode, receipt.ReceiptNo);
            resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMER;

            //打印收货单
            if (resolver.NeedPrintReceipt)
            {
                executeMgrE.PrintReceipt(resolver, receipt);
            }
            //打印检验单
            if (resolver.NeedInspection)
            {
                IList<InspectOrder> inspectOrders = inspectOrderMgrE.GetInspectOrder(null, receipt.ReceiptNo);

                if (inspectOrders.Count == 1)
                {
                    //估计次接口尚未实现
                    //IList<object> list = new List<object>();
                    //list.Add(inspectOrders[0]);
                    //list.Add(inspectOrders[0].InspectOrderDetails);
                    //string printUrl = reportMgrE.WriteToFile("InspectOrder.xls", list);

                    string printUrl = reportMgrE.WriteToFile("InspectOrder.xls", inspectOrders[0].InspectNo);

                    if (resolver.PrintUrl == null || resolver.PrintUrl.Trim() == string.Empty)
                    {
                        resolver.PrintUrl = printUrl;
                    }
                    else
                    {
                        resolver.PrintUrl += "|" + printUrl;
                    }
                }
            }
            return resolver;
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override void ExecutePrint(Resolver resolver)
        {
            executeMgrE.PrintReceipt(resolver);
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override void GetReceiptNotes(Resolver resolver)
        {
            string[] orderTypes = new string[] { 
                BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT,
                BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_TRANSFER,
                BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION };

            resolver = executeMgrE.GetReceiptNotes(resolver, orderTypes);
        }

        private void ClearReceivedQty(List<Transformer> transformers)
        {
            if (transformers != null)
            {
                foreach (Transformer transformer in transformers)
                {
                    transformer.CurrentQty = 0M;
                    if (transformer.TransformerDetails != null)
                    {
                        foreach (TransformerDetail transformerDetail in transformer.TransformerDetails)
                        {
                            transformerDetail.CurrentQty = 0M;
                        }
                    }
                }
            }
        }
    }
}





#region Extend Class

namespace com.Sconit.Service.Ext.Business.Impl
{
    public partial class ReceiveMgrE : com.Sconit.Service.Business.Impl.ReceiveMgr, IBusinessMgrE
    {

    }
}

#endregion

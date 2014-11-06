using com.Sconit.Service.Ext.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using com.Sconit.Entity.Distribution;
using com.Sconit.Entity.Exception;
using com.Sconit.Utility;
using com.Sconit.Service.Distribution;
using com.Sconit.Service.Ext.Report;
using com.Sconit.Service.Ext.Distribution;

namespace com.Sconit.Service.Business.Impl
{
    public class ShipMgr : AbstractBusinessMgr
    {
        public ISetBaseMgrE setBaseMgrE { get; set; }
        public ISetDetailMgrE setDetailMgrE { get; set; }
        public IOrderMgrE orderMgrE { get; set; }
        public IOrderHeadMgrE orderHeadMgrE { get; set; }
        public IExecuteMgrE executeMgrE { get; set; }
        public ILanguageMgrE languageMgrE { get; set; }
        public IReportMgrE reportMgrE { get; set; }
        public IInProcessLocationMgrE inProcessLocationMgrE { get; set; }
        public IPickListResultMgrE pickListResultMgrE { get; set; }
        public IHuMgrE huMgrE { get; set; }
        public IFlowMgrE flowMgrE { get; set; }




        protected override void SetBaseInfo(Resolver resolver)
        {
            if (resolver.CodePrefix == BusinessConstants.CODE_PREFIX_ORDER)
            {
                bool isHasOrderNo = false;
                if (resolver.Transformers != null)
                {
                    foreach (Transformer transformer in resolver.Transformers)
                    {
                        if (transformer.OrderNo == resolver.Code)
                        {
                            isHasOrderNo = true;
                            break;
                        }
                    }
                }
                if (!isHasOrderNo)
                {
                    setBaseMgrE.FillResolverByOrder(resolver);

                    #region 校验
                    if (!resolver.IsShipByOrder && resolver.ModuleType == BusinessConstants.TRANSFORMER_MODULE_TYPE_SHIPORDER)
                    {
                        throw new BusinessErrorException("Order.Error.NotShipByOrder", resolver.Code);
                    }

                    if (resolver.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)
                    {
                        throw new BusinessErrorException("Common.Business.Error.StatusError", resolver.Code, resolver.Status);
                    }

                    if (resolver.OrderType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
                    {
                        throw new BusinessErrorException("Order.Error.OrderShipIsNotProduction", resolver.Code, resolver.OrderType);
                    }
                    #endregion
                }
            }
            //else if (resolver.CodePrefix == BusinessConstants.CODE_PREFIX_WORK_ORDER)
            //{
            //}
            else if (resolver.CodePrefix == BusinessConstants.CODE_PREFIX_PICKLIST)
            {
                resolver.Transformers = null;
                setBaseMgrE.FillResolverByPickList(resolver);

                if (resolver.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)
                {
                    throw new BusinessErrorException("Common.Business.Error.StatusError", resolver.Code, resolver.Status);
                }
            }
            else
            {
                throw new BusinessErrorException("Common.Business.Error.BarCodeInvalid");
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override void GetDetail(Resolver resolver)
        {
            InProcessLocation inProcessLocation = null;
            //订单发货
            if (resolver.CodePrefix == BusinessConstants.CODE_PREFIX_ORDER)
            {
                if (resolver.Transformers != null && resolver.Transformers.Count > 0)
                {
                    foreach (Transformer transformer in resolver.Transformers)
                    {
                        if (resolver.Input.Trim().ToUpper() == transformer.OrderNo.Trim().ToUpper())
                        {
                            throw new BusinessErrorException("Common.Business.Error.ReScan", resolver.Code);
                        }
                    }
                    //校验订单配置选项
                    this.CheckOrderConfigValid(resolver.Input, resolver.Transformers[0].OrderNo);
                }
                else
                {
                    resolver.Transformers = new List<Transformer>();
                }
                inProcessLocation = orderMgrE.ConvertOrderToInProcessLocation(resolver.Input);

                if (inProcessLocation == null || inProcessLocation.InProcessLocationDetails == null || inProcessLocation.InProcessLocationDetails.Count == 0)
                {
                    throw new BusinessErrorException("Common.Business.Error.NoDetailToShip");
                }
                //if (resolver.IsScanHu && resolver.CodePrefix != BusinessConstants.CODE_PREFIX_PICKLIST)
                //{
                //    OrderHelper.ClearShippedQty(inProcessLocation);//清空发货数
                //}
                List<Transformer> newTransformerList = TransformerHelper.ConvertInProcessLocationDetailsToTransformers(inProcessLocation.InProcessLocationDetails);
                resolver.Transformers = resolver.Transformers == null ? new List<Transformer>() : resolver.Transformers;
                resolver.Transformers.AddRange(newTransformerList);
            }
            //拣货单发货
            else if (resolver.CodePrefix == BusinessConstants.CODE_PREFIX_PICKLIST)
            {
                //inProcessLocation = orderMgrE.ConvertPickListToInProcessLocation(resolver.Input);
                IList<PickListResult> pickListResultList = pickListResultMgrE.GetPickListResult(resolver.Input);
                resolver.Transformers = Utility.TransformerHelper.ConvertPickListResultToTransformer(pickListResultList);
            }
            resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMER;
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override void SetDetail(Resolver resolver)
        {
            if (resolver.CodePrefix == string.Empty)
            {
                throw new BusinessErrorException("Common.Business.Error.ScanFlowFirst");
            }
            setDetailMgrE.MatchShip(resolver);
        }

        protected override void ExecuteSubmit(Resolver resolver)
        {
            resolver = this.ShipOrder(resolver);
        }

        protected override void ExecuteCancel(Resolver resolver)
        {
            if (resolver.CodePrefix == BusinessConstants.CODE_PREFIX_ORDER)
            {
                executeMgrE.CancelOperation(resolver);
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        private Resolver ShipOrder(Resolver resolver)
        {
            InProcessLocation inProcessLocation = null;
            if (resolver.CodePrefix == BusinessConstants.CODE_PREFIX_PICKLIST)
            {
                inProcessLocation = orderMgrE.ShipOrder(resolver.Code, resolver.UserCode);
            }
            else
            {
                IList<InProcessLocationDetail> inProcessLocationDetailList = orderMgrE.ConvertTransformerToInProcessLocationDetail(resolver.Transformers);
                if (inProcessLocationDetailList.Count > 0)
                {
                    inProcessLocation = orderMgrE.ShipOrder(inProcessLocationDetailList, resolver.UserCode);
                }
                else
                {
                    throw new BusinessErrorException("OrderDetail.Error.OrderDetailShipEmpty");
                }
            }
            #region 打印
            if (resolver.NeedPrintAsn)
            {
                resolver.PrintUrl = PrintASN(inProcessLocation);
            }
            #endregion

            resolver.Transformers = TransformerHelper.ConvertInProcessLocationDetailsToTransformers(inProcessLocation.InProcessLocationDetails);
            resolver.Result = languageMgrE.TranslateMessage("Common.Business.ASNIs", resolver.UserCode, inProcessLocation.IpNo);
            resolver.Code = inProcessLocation.IpNo;
            resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMER;
            return resolver;
        }

        /// <summary>
        /// 校验订单配置选项，是否允许同时发货
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="originalOrderNo"></param>
        private void CheckOrderConfigValid(string orderNo, string originalOrderNo)
        {
            OrderHead originalOrderHead = orderHeadMgrE.CheckAndLoadOrderHead(originalOrderNo);
            OrderHead orderHead = orderHeadMgrE.CheckAndLoadOrderHead(orderNo);
            Flow flow = this.flowMgrE.LoadFlow(orderHead.Flow);
            Flow originalFlow = this.flowMgrE.LoadFlow(originalOrderHead.Flow);
            #region 合并发货校验
            if (flow.Code != originalFlow.Code || flow.Type != originalFlow.Type)
                throw new BusinessErrorException("Order.Error.ShipOrder.OrderTypeNotEqual");

            if (orderHead.PartyFrom.Code != originalOrderHead.PartyFrom.Code)
                throw new BusinessErrorException("Order.Error.ShipOrder.PartyFromNotEqual");

            if (orderHead.PartyTo.Code != originalOrderHead.PartyTo.Code)
                throw new BusinessErrorException("Order.Error.ShipOrder.PartyToNotEqual");

            string shipFromCode = orderHead.ShipFrom != null ? orderHead.ShipFrom.Code : string.Empty;
            string originalShipFromCode = originalOrderHead.ShipFrom != null ? originalOrderHead.ShipFrom.Code : string.Empty;
            if (shipFromCode != originalShipFromCode)
                throw new BusinessErrorException("Order.Error.ShipOrder.ShipFromNotEqual");

            string shipToCode = orderHead.ShipTo != null ? orderHead.ShipTo.Code : string.Empty;
            string originalShipToCode = originalOrderHead.ShipTo != null ? originalOrderHead.ShipTo.Code : string.Empty;
            if (shipToCode != originalShipToCode)
                throw new BusinessErrorException("Order.Error.ShipOrder.ShipToNotEqual");

            string routingCode = orderHead.Routing != null ? orderHead.Routing.Code : string.Empty;
            string originalRoutingCode = originalOrderHead.Routing != null ? originalOrderHead.Routing.Code : string.Empty;
            if (routingCode != originalRoutingCode)
                throw new BusinessErrorException("Order.Error.ShipOrder.RoutingNotEqual");

            if (orderHead.IsShipScanHu != originalOrderHead.IsShipScanHu)
                throw new BusinessErrorException("Order.Error.ShipOrder.IsShipScanHuNotEqual");

            if (orderHead.IsReceiptScanHu != originalOrderHead.IsReceiptScanHu)
                throw new BusinessErrorException("Order.Error.ShipOrder.IsReceiptScanHuNotEqual");

            if (orderHead.IsAutoReceive != originalOrderHead.IsAutoReceive)
                throw new BusinessErrorException("Order.Error.ShipOrder.IsAutoReceiveNotEqual");

            if (orderHead.GoodsReceiptGapTo != originalOrderHead.GoodsReceiptGapTo)
                throw new BusinessErrorException("Order.Error.ShipOrder.GoodsReceiptGapToNotEqual");
            #endregion
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override void ExecutePrint(Resolver resolver)
        {
            InProcessLocation inProcessLocation = inProcessLocationMgrE.LoadInProcessLocation(resolver.Code, true);
            resolver.PrintUrl = PrintASN(inProcessLocation);
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override void GetReceiptNotes(Resolver resolver)
        {
            string[] orderTypes = new string[] { BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION,
                BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_TRANSFER };
            string[] row = resolver.Code.Split('|');
            int firstRow = int.Parse(row[0]);
            int maxRows = int.Parse(row[1]);
            IList<InProcessLocation> inProcessLocationList = inProcessLocationMgrE.GetInProcessLocation(resolver.UserCode, firstRow, maxRows, orderTypes);
            resolver.ReceiptNotes = ConvertInProcessLocationsToReceiptNotes(inProcessLocationList);
        }

        private List<ReceiptNote> ConvertInProcessLocationsToReceiptNotes(IList<InProcessLocation> inProcessLocations)
        {
            if (inProcessLocations == null)
            {
                return null;
            }
            int seq = 1;
            List<ReceiptNote> receiptNotes = new List<ReceiptNote>();
            foreach (InProcessLocation inProcessLocation in inProcessLocations)
            {
                ReceiptNote receiptNote = new ReceiptNote();
                receiptNote.CreateDate = inProcessLocation.CreateDate;
                receiptNote.CreateUser = inProcessLocation.CreateUser == null ? string.Empty : inProcessLocation.CreateUser.Name;
                receiptNote.Dock = inProcessLocation.DockDescription;
                receiptNote.IpNo = inProcessLocation.IpNo;
                receiptNote.PartyFrom = inProcessLocation.PartyFrom == null ? string.Empty : inProcessLocation.PartyFrom.Name;
                receiptNote.PartyTo = inProcessLocation.PartyTo == null ? string.Empty : inProcessLocation.PartyTo.Name;
                receiptNote.Sequence = seq;
                receiptNotes.Add(receiptNote);
                seq++;
            }
            return receiptNotes;
        }

        private string PrintASN(InProcessLocation inProcessLocation)
        {
            IList<object> list = new List<object>();
            list.Add(inProcessLocation);
            list.Add(inProcessLocation.InProcessLocationDetails);

            string printUrl = reportMgrE.WriteToFile(inProcessLocation.AsnTemplate, list);
            //this.PrintOrder(printUrl);
            //报表url
            return printUrl;
        }

    }
}



#region Extend Class





namespace com.Sconit.Service.Ext.Business.Impl
{
    public partial class ShipMgrE : com.Sconit.Service.Business.Impl.ShipMgr, IBusinessMgrE
    {
    }
}

#endregion

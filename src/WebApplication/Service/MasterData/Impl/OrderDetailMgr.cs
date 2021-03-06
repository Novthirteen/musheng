using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.Distribution;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using NHibernate.Expression;
using com.Sconit.Service.Ext.Procurement;
using System.Linq;
using com.Sconit.Service.Ext.Criteria.Impl;
using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class OrderDetailMgr : OrderDetailBaseMgr, IOrderDetailMgr
    {
        private string[] FlowDetail2OrderDetailCloneFields = new string[] 
            { 
                "Item",
                "ReferenceItemCode",
                "Uom",
                "UnitCount",
                "GoodsReceiptLotSize",
                "BatchSize",
                "Bom",
                "LocationFrom",
                "LocationTo",
                "BillAddress",
                "PriceList",
                "HuLotSize",
                "BillSettleTerm",
                "Customer",
                "PackageVolumn",
                "PackageType",
                "NeedInspection",
                "IdMark",
                "BarCodeType",
                "OddShipOption",
                "CustomerItemCode",
                "Remark",
                "InspectLocationFrom",
                "InspectLocationTo",
                "RejectLocationFrom",
                "RejectLocationTo",
                "NeedRejectInspection",
                "Routing",
                "ReturnRouting"
            };

        private string[] OrderDetailOfChildKitCloneFields = new string[] 
            { 
                "OrderHead", 
                "RequiredQty",
                "OrderedQty",
                "GoodsReceiptLotSize",
                "BatchSize",
                "LocationFrom",
                "LocationTo",
                "BillAddress",
                "PriceList",
                "HuLotSize",
                "BillSettleTerm",
                "Customer",
                "PackageVolumn",
                "PackageType",
                "NeedInspection",
                "IdMark",
                "BarCodeType",
                "OddShipOption",
                "CustomerItemCode",
                "Remark",
                "InspectLocationFrom",
                "InspectLocationTo",
                "RejectLocationFrom",
                "RejectLocationTo",
                "NeedRejectInspection",
                "Routing",
                "ReturnRouting"
            };

        private string[] ReferenceOrderDetailCloneFields = new string[] 
            { 
                "Item",
                "ReferenceItemCode",
                "Uom",
                "UnitCount",
                "GoodsReceiptLotSize",
                "BatchSize",
            };

        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IUomConversionMgrE uomConversionMgrE { get; set; }
        public IBomMgrE bomMgrE { get; set; }
        public IBomDetailMgrE bomDetailMgrE { get; set; }
        public IRoutingDetailMgrE routingDetailMgrE { get; set; }
        public IOrderOperationMgrE orderOperationMgrE { get; set; }
        public IOrderLocationTransactionMgrE orderLocationTransactionMgrE { get; set; }
        public IItemKitMgrE itemKitMgrE { get; set; }
        public IItemReferenceMgrE itemReferenceMgrE { get; set; }
        public IPriceListDetailMgrE priceListDetailMgrE { get; set; }
        public IEntityPreferenceMgrE entityPreferenceMgrE { get; set; }
        public IFlowMgrE flowMgrE { get; set; }
        public IOrderTracerMgrE orderTracerMgrE { get; set; }
        public IPartyMgrE partyMgr { get; set; }
        public IRegionMgrE regionMgr { get; set; }

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public override void DeleteOrderDetail(IList<OrderDetail> orderDetails)
        {
            if (orderDetails != null)
            {
                //foreach (OrderDetail od in orderDetails)
                //{
                //    if (od.OrderTracers != null)
                //    {
                //        orderTracerMgrE.DeleteOrderTracer(od.OrderTracers);
                //    }
                //}
                IList<int> oltIdList = orderDetails.Select(e => e.Id).ToList<int>();
                orderTracerMgrE.DeleteOrderTracerByOrderDetailId((IList<int>)oltIdList);
            }
            base.DeleteOrderDetail(orderDetails);
        }

        [Transaction(TransactionMode.Unspecified)]
        public OrderDetail LoadOrderDetail(int id, bool includeLocTrans)
        {
            OrderDetail orderDetail = this.LoadOrderDetail(id);

            if (orderDetail != null)
            {
                if (includeLocTrans && orderDetail.OrderLocationTransactions != null && orderDetail.OrderLocationTransactions.Count > 0)
                {
                }
            }

            return orderDetail;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderDetail> GenerateOrderDetail(OrderHead orderHead, FlowDetail flowDetail)
        {
            return GenerateOrderDetail(orderHead, flowDetail, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderDetail> GenerateOrderDetail(OrderHead orderHead, FlowDetail flowDetail, bool isReferencedFlow)
        {
            return GenerateOrderDetail(orderHead, flowDetail, false, false);
        }


        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderDetail> GenerateOrderDetail(OrderHead orderHead, FlowDetail flowDetail, bool isReferencedFlow, bool isStartKit)
        {
            EntityPreference entityPreference = this.entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_SEQ_INTERVAL);
            int seqInterval = int.Parse(entityPreference.Value);

            IList<OrderDetail> orderDetailList = new List<OrderDetail>();
            OrderDetail orderDetail = new OrderDetail();
            orderDetail.FlowDetail = flowDetail;
            orderDetail.OrderHead = orderHead;
            if (!isReferencedFlow)
            {
                CloneHelper.CopyProperty(flowDetail, orderDetail, FlowDetail2OrderDetailCloneFields);
            }
            else
            {
                CloneHelper.CopyProperty(flowDetail, orderDetail, ReferenceOrderDetailCloneFields);
            }

            #region 查找价格
            if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT || orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
            {
                if (orderDetail.DefaultPriceList != null)
                {
                    PriceListDetail priceListDetail = priceListDetailMgrE.GetLastestPriceListDetail(orderDetail.DefaultPriceList, orderDetail.Item, orderHead.StartTime, orderHead.Currency, orderDetail.Uom);
                    orderDetail.IsProvisionalEstimate = priceListDetail == null ? true : priceListDetail.IsProvisionalEstimate;
                    if (priceListDetail != null)
                    {
                        orderDetail.UnitPrice = priceListDetail.UnitPrice;
                        orderDetail.TaxCode = priceListDetail.TaxCode;
                        orderDetail.IsIncludeTax = priceListDetail.IsIncludeTax;
                    }
                }
            }
            #endregion

            #region 设置退货和次品库位
            //if (orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RTN
            //    || orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RWO)
            //{
            //    if (flowDetail.LocationFrom != null && flowDetail.LocationFrom.ActingLocation != null)
            //    {
            //        orderDetail.LocationFrom = flowDetail.LocationFrom.ActingLocation;
            //    }

            //    if (flowDetail.LocationTo != null && flowDetail.LocationTo.ActingLocation != null)
            //    {
            //        orderDetail.LocationTo = flowDetail.LocationTo.ActingLocation;
            //    }
            //}
            //if (orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RWO)
            //{
            //    orderDetail.LocationTo = this.locationMgrE.GetRejectLocation();
            //}
            #endregion

            if (isStartKit && orderDetail.Item.Type == BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_K)
            {
                #region 处理套件
                IList<ItemKit> itemKitList = this.itemKitMgrE.GetChildItemKit(orderDetail.Item.Code);

                if (itemKitList != null && itemKitList.Count > 0)
                {
                    int count = 0;
                    decimal convertRate = 1;
                    foreach (ItemKit itemKit in itemKitList)
                    {
                        count++;

                        if (itemKit.ParentItem.Uom.Code != orderDetail.Uom.Code)
                        {
                            convertRate = this.uomConversionMgrE.ConvertUomQty(orderDetail.Item, orderDetail.Uom, 1, itemKit.ParentItem.Uom);
                        }

                        OrderDetail orderDetailOfChildKit = new OrderDetail();
                        CloneHelper.CopyProperty(orderDetail, orderDetailOfChildKit, OrderDetailOfChildKitCloneFields);
                        orderDetailOfChildKit.Sequence = orderDetailOfChildKit.Sequence + count;
                        orderDetailOfChildKit.Item = itemKit.ChildItem;
                        orderDetailOfChildKit.ReferenceItemCode = this.itemReferenceMgrE.GetItemReferenceByItem(itemKit.ChildItem.Code, orderDetail.OrderHead.PartyFrom.Code, orderDetail.OrderHead.PartyTo.Code);
                        orderDetailOfChildKit.Uom = itemKit.ChildItem.Uom;
                        orderDetailOfChildKit.UnitCount = itemKit.ChildItem.UnitCount;
                        orderDetailOfChildKit.RequiredQty = orderDetailOfChildKit.RequiredQty * itemKit.Qty * convertRate;
                        orderDetailOfChildKit.OrderedQty = orderDetailOfChildKit.OrderedQty * itemKit.Qty * convertRate;
                        orderDetailOfChildKit.GoodsReceiptLotSize = orderDetailOfChildKit.GoodsReceiptLotSize * itemKit.Qty * convertRate;
                        orderDetailOfChildKit.BatchSize = orderDetailOfChildKit.BatchSize * itemKit.Qty * convertRate;
                        if (orderDetailOfChildKit.HuLotSize.HasValue)
                        {
                            orderDetailOfChildKit.HuLotSize = int.Parse((orderDetailOfChildKit.HuLotSize.Value * itemKit.Qty * convertRate).ToString("#"));
                        }

                        #region 计算价格
                        if (orderDetailOfChildKit.DefaultPriceList != null)
                        {
                            PriceListDetail priceListDetail = priceListDetailMgrE.GetLastestPriceListDetail(orderDetailOfChildKit.DefaultPriceList, orderDetailOfChildKit.Item, orderHead.StartTime, orderHead.Currency, orderDetailOfChildKit.Uom);
                            orderDetailOfChildKit.IsProvisionalEstimate = priceListDetail == null ? true : priceListDetail.IsProvisionalEstimate;
                            if (priceListDetail != null)
                            {
                                orderDetailOfChildKit.UnitPrice = priceListDetail.UnitPrice;
                                orderDetailOfChildKit.IsIncludeTax = priceListDetail.IsIncludeTax;
                                orderDetailOfChildKit.TaxCode = priceListDetail.TaxCode;
                            }
                        }
                        #endregion

                        //重新设置Sequence
                        int detailCount = orderHead.OrderDetails != null ? orderHead.OrderDetails.Count : 0;
                        orderDetail.Sequence = (detailCount + 1) * seqInterval;

                        orderHead.AddOrderDetail(orderDetailOfChildKit);
                        orderDetailList.Add(orderDetailOfChildKit);
                    }
                }
                else
                {
                    throw new BusinessErrorException("ItemKit.Error.NotFoundForParentItem", orderDetail.Item.Code);
                }
                #endregion
            }
            else
            {
                //重新设置Sequence
                int detailCount = orderHead.OrderDetails != null ? orderHead.OrderDetails.Count : 0;
                orderDetail.Sequence = (detailCount + 1) * seqInterval;

                #region 参考零件号
                if (orderDetail.ReferenceItemCode == null || orderDetail.ReferenceItemCode == string.Empty)
                {
                    string firstPartyCode = string.Empty;
                    string secondPartyCode = string.Empty;
                    if (orderHead.Type == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION)
                    {
                        firstPartyCode = orderHead.PartyTo.Code;
                    }
                    else
                    {
                        firstPartyCode = orderHead.PartyFrom.Code;
                    }
                    orderDetail.ReferenceItemCode = itemReferenceMgrE.GetItemReferenceByItem(orderDetail.Item.Code, firstPartyCode, secondPartyCode);
                }
                #endregion

                #region 设置默认库位
                //orderDetail.LocationFrom = orderDetail.LocationFrom == null ? orderHead.LocationFrom : orderDetail.LocationFrom;
                //orderDetail.LocationTo = orderDetail.LocationTo == null ? orderHead.LocationTo : orderDetail.LocationTo;
                #endregion

                //BOM赋值
                if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION && orderDetail.Bom == null)
                {
                    orderDetail.Bom = this.bomMgrE.LoadBom(this.bomMgrE.FindBomCode(orderDetail.Item.Code), true);
                }

                orderHead.AddOrderDetail(orderDetail);
                orderDetailList.Add(orderDetail);
            }

            return orderDetailList;
        }

        //创建OrderLocationTransaction、OrderOperation
        [Transaction(TransactionMode.Unspecified)]
        public void GenerateOrderDetailSubsidiary(OrderDetail orderDetail)
        {
            OrderHead orderHead = orderDetail.OrderHead;
            int maxOp = 0;    //记录最大工序号，给成品收货时用
            int minOp = 0;    //记录最小工序号，给返工成品用

            #region 把OrderDetail的收货单位和单位用量转换为BOM单位和单位用量
            //fgUom，fgUnityQty代表接收一个orderDetail.Uom单位(等于订单的收货单位)的FG，等于单位(fgUom)有多少(fgUnityQty)值
            Uom fgUom = orderDetail.Uom;
            decimal fgUnityQty = 1;     //运输物品和生产的成品UnitQty默认为1
            //如果和Bom上的单位不一致，转化为Bom上的单位，不然会导致物料回冲不正确。  
            if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
            {
                //#region 创建生产物料的OrderLocTrans
                //如果是生产订单，必须要有Bom
                FillBomForOrderDetail(orderDetail);
            }
            if (orderDetail.Bom != null && orderDetail.Uom.Code != orderDetail.Bom.Uom.Code)
            {
                fgUom = orderDetail.Bom.Uom;
                fgUnityQty = this.uomConversionMgrE.ConvertUomQty(orderDetail.Item, orderDetail.Uom, fgUnityQty, fgUom);
            }
            #endregion

            #region 创建OrderLocTrans

            bool isBackFlushIgnoreScrapRate = bool.Parse(this.entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_BACKFLUSH_IGNORE_SCRAPRATE).Value);

            if (orderDetail.Item.Type == BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_K)
            {
                //程序一般不会运行到这里，套件的拆分都在前台进行
                //用户在下订单的时候已经拆分了套件
                //2010-1-19 dingxin
                throw new BusinessErrorException("Order.Error.CreateOrder.ItemTypeK", orderDetail.Item.Code);
            }
            else
            {
                if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT
                    || orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_CUSTOMERGOODS
                    || orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_SUBCONCTRACTING)
                {
                    #region 采购，只需要记录入库事务RCT-PO
                    this.orderLocationTransactionMgrE.GenerateOrderLocationTransaction(orderDetail, orderDetail.Item, null,
                                fgUom, maxOp, BusinessConstants.IO_TYPE_OUT, null,
                                fgUnityQty, orderDetail.DefaultLocationFrom,
                                false, orderDetail.HuLotSize, true, null, orderDetail.ItemVersion,
                                null, null);

                    this.orderLocationTransactionMgrE.GenerateOrderLocationTransaction(orderDetail, orderDetail.Item, null,
                                fgUom, maxOp, BusinessConstants.IO_TYPE_IN, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_PO,
                                fgUnityQty, orderDetail.DefaultLocationTo,
                                false, orderDetail.HuLotSize, true, null, orderDetail.ItemVersion,
                                this.partyMgr.GetDefaultInspectLocation(orderHead.PartyTo.Code, orderDetail.DefaultInspectLocationTo),
                                this.partyMgr.GetDefaultRejectLocation(orderHead.PartyTo.Code, orderDetail.DefaultRejectLocationTo));
                    #endregion
                }
                else if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
                {
                    #region 销售，只需要记录出库事务ISS-SO
                    this.orderLocationTransactionMgrE.GenerateOrderLocationTransaction(orderDetail, orderDetail.Item, null,
                                fgUom, maxOp, BusinessConstants.IO_TYPE_OUT, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_SO,
                                fgUnityQty, orderDetail.DefaultLocationFrom,
                                false, orderDetail.HuLotSize, true, null, null,
                                this.partyMgr.GetDefaultInspectLocation(orderHead.PartyFrom.Code, orderDetail.DefaultInspectLocationFrom),
                                this.partyMgr.GetDefaultRejectLocation(orderHead.PartyFrom.Code, orderDetail.DefaultRejectLocationFrom));

                    this.orderLocationTransactionMgrE.GenerateOrderLocationTransaction(orderDetail, orderDetail.Item, null,
                                fgUom, maxOp, BusinessConstants.IO_TYPE_IN, null,
                                fgUnityQty, orderDetail.DefaultLocationTo,
                                false, orderDetail.HuLotSize, true, null, null, null, null);
                    #endregion
                }
                else if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_TRANSFER)
                {
                    #region 移库，需要记录出库事务ISS-TR和入库事务RCT-TR
                    this.orderLocationTransactionMgrE.GenerateOrderLocationTransaction(orderDetail, orderDetail.Item, null,
                                fgUom, maxOp, BusinessConstants.IO_TYPE_OUT, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_TR,
                                fgUnityQty, orderDetail.DefaultLocationFrom,
                                false, orderDetail.HuLotSize, true, null, null,
                                this.partyMgr.GetDefaultInspectLocation(orderHead.PartyFrom.Code, orderDetail.DefaultInspectLocationFrom),
                                this.partyMgr.GetDefaultRejectLocation(orderHead.PartyFrom.Code, orderDetail.DefaultRejectLocationFrom));

                    this.orderLocationTransactionMgrE.GenerateOrderLocationTransaction(orderDetail, orderDetail.Item, null,
                                fgUom, maxOp, BusinessConstants.IO_TYPE_IN, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_TR,
                                fgUnityQty, orderDetail.DefaultLocationTo,
                                false, orderDetail.HuLotSize, true, null, null,
                                this.partyMgr.GetDefaultInspectLocation(orderHead.PartyTo.Code, orderDetail.DefaultInspectLocationTo),
                                this.partyMgr.GetDefaultRejectLocation(orderHead.PartyTo.Code, orderDetail.DefaultRejectLocationTo));
                    #endregion
                }
                //else if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_INSPECTION)
                //{
                //    #region 检验，需要记录出库事务ISS-TR和入库事务RCT-TR
                //    this.orderLocationTransactionMgrE.GenerateOrderLocationTransaction(orderDetail, orderDetail.Item, null,
                //                fgUom, maxOp, BusinessConstants.IO_TYPE_OUT, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_TR,
                //                fgUnityQty, orderDetail.DefaultLocationFrom,
                //                false, orderDetail.HuLotSize, true, null, null, this.locationMgrE.GetRejectLocation());

                //    this.orderLocationTransactionMgrE.GenerateOrderLocationTransaction(orderDetail, orderDetail.Item, null,
                //                fgUom, maxOp, BusinessConstants.IO_TYPE_IN, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_TR,
                //                fgUnityQty, orderDetail.DefaultLocationTo,
                //                false, orderDetail.HuLotSize, true, null, null, this.locationMgrE.GetRejectLocation());
                //    #endregion
                //}
                else if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
                {
                    #region 创建生产物料的OrderLocTrans
                    //如果是生产订单，必须要有Bom
                    //FillBomForOrderDetail(orderDetail);

                    int materialNumber = 0;
                    IList<BomDetail> bomDetailList = this.bomDetailMgrE.GetFlatBomDetail(orderDetail.Bom.Code, orderHead.StartTime);
                    foreach (BomDetail bomDetail in bomDetailList)
                    {
                        #region 记录最大工序号
                        //最大工序号是从Bom上取还是从Routing上取？
                        if (maxOp < bomDetail.Operation)
                        {
                            //记录最大工序号
                            maxOp = bomDetail.Operation;
                        }
                        #endregion

                        #region 记录最小工序号
                        //最小工序号是从Bom上取还是从Routing上取？
                        if (minOp > bomDetail.Operation || minOp == 0)
                        {
                            //记录最大工序号
                            minOp = bomDetail.Operation;
                        }
                        #endregion

                        #region 查找物料的来源库位
                        //来源库位查找逻辑BomDetail-->RoutingDetail-->FlowDetail-->Flow
                        Location bomLocFrom = bomDetail.Location;

                        if (bomLocFrom == null)
                        {
                            //取RoutingDetail上的，在OrderHead上取
                            if (orderHead.OrderOperations != null && orderHead.OrderOperations.Count > 0)
                            {
                                OrderOperation orderOperation = orderHead.OrderOperations.Where(p => p.Operation == bomDetail.Operation && p.Reference == bomDetail.Reference).SingleOrDefault();

                                if (orderOperation != null)
                                {
                                    bomLocFrom = orderOperation.Location;
                                }
                            }

                        }
                        //if (orderHead.Routing != null)
                        //{
                        //    //在Routing上查找，并检验Routing上的工序和BOM上的是否匹配
                        //    RoutingDetail routingDetail = routingDetailMgrE.LoadRoutingDetail(orderHead.Routing, bomDetail.Operation, bomDetail.Reference);
                        //    if (routingDetail != null)
                        //    {
                        //        if (bomLocFrom == null)
                        //        {
                        //            bomLocFrom = routingDetail.Location;
                        //        }

                        //        //if (maxOp < routingDetail.Operation)
                        //        //{
                        //        //    //记录最大工序号
                        //        //    maxOp = routingDetail.Operation;
                        //        //}

                        //        //orderHead.AddOrderOperation(this.orderOperationMgrE.GenerateOrderOperation(orderHead, routingDetail));
                        //    }
                        //    //else
                        //    //{
                        //    //    //没有找到和BOM上相匹配的工序
                        //    //    throw new BusinessErrorException("Order.Error.OpNotMatch", bomDetail.Bom.Code, bomDetail.Item.Code, routing.Code, bomDetail.Operation.ToString(), bomDetail.Reference);
                        //    //}
                        //}

                        if (bomLocFrom == null)
                        {
                            //取默认库位FlowDetail-->Flow
                            bomLocFrom = orderDetail.DefaultLocationFrom;
                        }
                        //string bomLocFromType = bomLocFrom != null ? bomLocFrom.Type : null;
                        #endregion

                        #region 查找物料的目的库位
                        //目的库位，如果是生产类型，直接置为Null，其它情况FlowDetail-->Flow
                        Location bomLocTo = (orderHead.Type != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION) ?
                            orderDetail.DefaultLocationTo : null;
                        //string bomLocToType = bomLocTo != null ? bomLocTo.Type : null;
                        #endregion

                        #region 生产物料，只需要记录出库事务ISS-TR
                        this.orderLocationTransactionMgrE.GenerateOrderLocationTransaction(orderDetail, bomDetail.Item, bomDetail,
                                   bomDetail.Uom, bomDetail.Operation, BusinessConstants.IO_TYPE_OUT, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_WO,
                                   bomDetail.CalculatedQty * fgUnityQty,     //返工，原材料数量默认等于0，回冲物料
                                   (isBackFlushIgnoreScrapRate ? bomDetail.CalculatedQtyWithoutScrapRate : bomDetail.CalculatedQty) * fgUnityQty,   //回冲是否不考虑损耗率 
                                   bomLocFrom, bomDetail.IsShipScanHu, bomDetail.HuLotSize, bomDetail.NeedPrint, bomDetail.BackFlushMethod, null,
                                   this.partyMgr.GetDefaultInspectLocation(orderHead.PartyFrom.Code, orderDetail.DefaultInspectLocationFrom),
                                   this.partyMgr.GetDefaultRejectLocation(orderHead.PartyFrom.Code, orderDetail.DefaultRejectLocationFrom));
                        #endregion

                        if (bomDetail.BackFlushMethod == BusinessConstants.CODE_MASTER_BACKFLUSH_METHOD_VALUE_GOODS_RECEIVE
                            || bomDetail.BackFlushMethod == BusinessConstants.CODE_MASTER_BACKFLUSH_METHOD_VALUE_BATCH_FEED_GR)
                        {
                            materialNumber += Convert.ToInt32(Math.Round(bomDetail.CalculatedQty * fgUnityQty)); //累计原材料消耗数量
                        }
                    }
                    #endregion

                    #region 生产成品，只需要记录入库事务RCT-WO
                    OrderLocationTransaction fgOrderLocationTransaction = this.orderLocationTransactionMgrE.GenerateOrderLocationTransaction(orderDetail, orderDetail.Item, null,
                        fgUom, maxOp, BusinessConstants.IO_TYPE_IN, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_WO,
                                fgUnityQty, orderDetail.DefaultLocationTo,
                                false, orderDetail.HuLotSize, true, null, orderDetail.ItemVersion,
                                this.partyMgr.GetDefaultInspectLocation(orderHead.PartyTo.Code, orderDetail.DefaultInspectLocationTo),
                                this.partyMgr.GetDefaultRejectLocation(orderHead.PartyTo.Code, orderDetail.DefaultRejectLocationTo));

                    //原材料消耗数量
                    fgOrderLocationTransaction.MaterailNumber = materialNumber;


                    //返工，把自己添加到物料中
                    if (orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RWO)
                    {
                        //Location loc = orderDetail.DefaultLocationTo.ActingLocation != null ? orderDetail.DefaultLocationTo.ActingLocation : orderDetail.DefaultLocationTo;
                        //Location loc = this.locationMgrE.GetRejectLocation();
                        DetachedCriteria criteria = DetachedCriteria.For<Location>().Add(Expression.Eq("Code", this.regionMgr.GetDefaultRejectLocation(orderHead.PartyFrom.Code, orderDetail.DefaultRejectLocationFrom)));
                        IList<Location> locList = this.criteriaMgrE.FindAll<Location>(criteria);
                        Location loc = locList != null && locList.Count > 0 ? locList[0] : null;

                        //todo 处理返工的成品是否需要扫描Hu，现在不扫描
                        //返工对成品的投料记RCT-WO事务
                        this.orderLocationTransactionMgrE.GenerateOrderLocationTransaction(orderDetail, orderDetail.Item, null,
                          fgUom, minOp, BusinessConstants.IO_TYPE_OUT, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_WO,
                            fgUnityQty, loc,
                         false, orderDetail.HuLotSize, true, null, null,
                         this.partyMgr.GetDefaultInspectLocation(orderHead.PartyTo.Code, orderDetail.DefaultInspectLocationTo),
                         this.partyMgr.GetDefaultRejectLocation(orderHead.PartyTo.Code, orderDetail.DefaultRejectLocationTo));
                    }
                    #endregion
                }
            }
            #endregion

            #region 生产，给没有Op的OrderLocTrans赋值maxOp
            if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
            {
                if (maxOp == 0)
                {
                    EntityPreference entityPreference = this.entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_SEQ_INTERVAL);
                    int seqInterval = int.Parse(entityPreference.Value);
                    maxOp = seqInterval; //默认工序号
                }

                if (orderDetail.OrderLocationTransactions != null && orderDetail.OrderLocationTransactions.Count > 0)
                {
                    foreach (OrderLocationTransaction orderLocationTransaction in orderDetail.OrderLocationTransactions)
                    {
                        if (orderLocationTransaction.Operation == 0)
                        {
                            orderLocationTransaction.Operation = maxOp;
                        }
                    }
                }
            }
            #endregion
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderDetail> GetOrderDetail(string orderNo)
        {
            DetachedCriteria criteria = DetachedCriteria.For<OrderDetail>();
            criteria.Add(Expression.Eq("OrderHead.OrderNo", orderNo));
            return this.criteriaMgrE.FindAll<OrderDetail>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderDetail> GetOrderDetail(OrderHead orderHead)
        {
            return GetOrderDetail(orderHead.OrderNo);
        }

        [Transaction(TransactionMode.Unspecified)]
        public bool CheckOrderDet(OrderDetail orderDetail)
        {
            if (orderDetail.DefaultLocationFrom == null)
                return true;
            else
                return this.CheckOrderDet(orderDetail.Item.Code, orderDetail.DefaultLocationFrom.Code, orderDetail.OrderHead.CheckDetailOption);
        }

        [Transaction(TransactionMode.Unspecified)]
        public bool CheckOrderDet(string item, string loc, string checkOrderDetOption)
        {
            //不检查
            if (checkOrderDetOption == BusinessConstants.CODE_MASTER_CHECK_ORDER_DETAIL_OPTION_VALUE_NOT_CHECK)
                return true;

            //检查来源
            if (checkOrderDetOption == BusinessConstants.CODE_MASTER_CHECK_ORDER_DETAIL_OPTION_VALUE_CHECK_SOURCE)
            {
                DetachedCriteria criteria = DetachedCriteria.For(typeof(FlowDetail));
                criteria.Add(Expression.Eq("Item.Code", item));
                IList<FlowDetail> flowDetailList = criteriaMgrE.FindAll<FlowDetail>(criteria);

                if (flowDetailList != null && flowDetailList.Count > 0)
                {
                    foreach (FlowDetail flowDetail in flowDetailList)
                    {
                        if (flowDetail.DefaultLocationTo != null && flowDetail.DefaultLocationTo.Code == loc)
                        {
                            return true;
                        }

                        //ReferenceFlow
                        if (flowDetail.Flow.ReferenceFlow != null && flowDetail.Flow.ReferenceFlow.Trim() != string.Empty)
                        {
                            Flow refFlow = this.flowMgrE.LoadFlow(flowDetail.Flow.ReferenceFlow);
                            if (refFlow.LocationTo != null && refFlow.LocationTo.Code == loc)
                            {
                                return true;
                            }
                        }
                    }
                }

                throw new BusinessErrorException("OrderDetail.Error.CheckOrderDetOption.NoSource", item);
            }

            //检查库存
            if (checkOrderDetOption == BusinessConstants.CODE_MASTER_CHECK_ORDER_DETAIL_OPTION_VALUE_CHECK_INVENTORY)
            {
                decimal currentInv = GetCurrentInv(loc, item);
                if (currentInv > 0)
                    return true;
                else
                    throw new BusinessErrorException("OrderDetail.Error.CheckOrderDetOption.NoInventory", item, loc);
            }

            return false;
        }
        [Transaction(TransactionMode.Unspecified)]
        public OrderDetail TransferFlowDetail2OrderDetail(FlowDetail flowDetail)
        {
            OrderDetail orderDetail = new OrderDetail();
            CloneHelper.CopyProperty(flowDetail, orderDetail, FlowDetail2OrderDetailCloneFields);
            return orderDetail;
        }

        [Transaction(TransactionMode.Requires)]
        public void RecordOrderShipQty(int orderLocationTransactionId, InProcessLocationDetail inProcessLocationDetail, bool checkExcceed)
        {
            RecordOrderShipQty(this.orderLocationTransactionMgrE.LoadOrderLocationTransaction(orderLocationTransactionId), inProcessLocationDetail, checkExcceed);
        }

        [Transaction(TransactionMode.Requires)]
        public void RecordOrderShipQty(OrderLocationTransaction orderLocationTransaction, InProcessLocationDetail inProcessLocationDetail, bool checkExcceed)
        {
            //orderLocationTransaction = this.orderLocationTransactionMgrE.LoadOrderLocationTransaction(orderLocationTransaction.Id);
            EntityPreference entityPreference = this.entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_ALLOW_EXCEED_GI_GR);
            bool allowExceedGiGR = bool.Parse(entityPreference.Value); //企业属性，允许过量发货和收货

            OrderDetail orderDetail = orderLocationTransaction.OrderDetail;
            OrderHead orderHead = orderDetail.OrderHead;
            decimal shipQty = inProcessLocationDetail.Qty * orderLocationTransaction.UnitQty;

            #region 是否过量发货判断
            //生产物料发货不检查过量选项
            if (orderLocationTransaction.OrderDetail.OrderHead.Type != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION
                && orderLocationTransaction.OrderDetail.OrderHead.SubType != BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_ADJ)
            {
                //检查AccumulateQty(已发数)不能大于等于OrderedQty(订单数)
                //if (!(orderHead.AllowExceed && allowExceedGiGR))
                //{
                //    if ((orderLocationTransaction.OrderedQty > 0 && orderLocationTransaction.AccumulateQty.HasValue && orderLocationTransaction.AccumulateQty.Value >= orderLocationTransaction.OrderedQty && shipQty > 0)
                //            || (orderLocationTransaction.OrderedQty < 0 && orderLocationTransaction.AccumulateQty.HasValue && orderLocationTransaction.AccumulateQty.Value <= orderLocationTransaction.OrderedQty && shipQty < 0))
                //    {
                //        throw new BusinessErrorException("Order.Error.ShipExcceed", orderHead.OrderNo, orderLocationTransaction.Item.Code);
                //    }
                //}

                if (!(orderHead.AllowExceed && allowExceedGiGR) && checkExcceed)   //不允许过量发货
                {
                    //检查AccumulateQty(已发数) + shipQty(本次发货数)不能大于OrderedQty(订单数)
                    orderLocationTransaction.AccumulateQty = orderLocationTransaction.AccumulateQty.HasValue ? orderLocationTransaction.AccumulateQty.Value : 0;
                    if ((orderLocationTransaction.OrderedQty > 0 && (orderLocationTransaction.AccumulateQty + shipQty > orderLocationTransaction.OrderedQty))
                        || (orderLocationTransaction.OrderedQty < 0 && (orderLocationTransaction.AccumulateQty + shipQty < orderLocationTransaction.OrderedQty)))
                    {
                        throw new BusinessErrorException("Order.Error.ShipExcceed", orderHead.OrderNo, orderLocationTransaction.Item.Code);
                    }
                }
            }
            #endregion

            #region 记录发货量
            if (orderLocationTransaction.BackFlushMethod == null ||
                orderLocationTransaction.BackFlushMethod == BusinessConstants.CODE_MASTER_BACKFLUSH_METHOD_VALUE_GOODS_RECEIVE ||
                orderLocationTransaction.BackFlushMethod == BusinessConstants.CODE_MASTER_BACKFLUSH_METHOD_VALUE_BATCH_FEED_GR)
            {
                if (!orderLocationTransaction.AccumulateQty.HasValue)
                {
                    orderLocationTransaction.AccumulateQty = 0;
                }

                //记录LocationTransaction的累计发货量
                orderLocationTransaction.AccumulateQty += shipQty;
                this.orderLocationTransactionMgrE.UpdateOrderLocationTransaction(orderLocationTransaction);

                //记录OrderDetail的累计发货量
                if (orderLocationTransaction.Item.Code == orderLocationTransaction.OrderDetail.Item.Code)
                {
                    //如果OrderLocationTransaction和OrderDetail上的Item一致，需要更新OrderDetail上的ShippedQty
                    if (!orderDetail.ShippedQty.HasValue)
                    {
                        orderDetail.ShippedQty = 0;
                    }

                    //OrderLocationTransaction和OrderDetail上的Item可能单位不一致，直接除以UnitQty就可以转换了
                    orderDetail.ShippedQty += (shipQty / orderLocationTransaction.UnitQty);
                    this.UpdateOrderDetail(orderDetail);
                }
            }
            #endregion
        }
        #endregion Customized Methods

        #region private Methods
        private void FillBomForOrderDetail(OrderDetail orderDetail)
        {
            //Bom的选取顺序orderDetail.Bom(Copy from 路线明细) --> orderDetail.Item.Bom--> 用orderDetail.Item.Code作为BomCode
            if (orderDetail.Bom == null && orderDetail.Item.Bom != null)
            {
                orderDetail.Bom = orderDetail.Item.Bom;
            }

            if (orderDetail.Bom == null)
            {
                Bom bom = this.bomMgrE.LoadBom(orderDetail.Item.Code);
                if (bom != null)
                {
                    orderDetail.Bom = bom;
                }
            }

            if (orderDetail.Bom == null)
            {
                throw new BusinessErrorException("OrderDetail.Error.NoBom", orderDetail.Item.Code);
            }
        }


        private decimal GetCurrentInv(string location, string item)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(LocationDetail));
            if (location != null && location.Trim() != string.Empty)
                criteria.Add(Expression.Eq("Location.Code", location));
            if (item != null && item.Trim() != string.Empty)
                criteria.Add(Expression.Eq("Item.Code", item));

            criteria.SetProjection(Projections.Sum("Qty"));
            IList result = criteriaMgrE.FindAll(criteria);
            if (result[0] != null)
            {
                return (decimal)result[0];
            }
            else
            {
                return 0;
            }
        }
        #endregion
    }
}


#region Extend Class






namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class OrderDetailMgrE : com.Sconit.Service.MasterData.Impl.OrderDetailMgr, IOrderDetailMgrE
    {

    }
}
#endregion

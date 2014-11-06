using com.Sconit.Service.Ext.MasterData;


using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using com.Sconit.Utility;
using NHibernate.Transform;
using com.Sconit.Service.Ext.Procurement;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class OrderLocationTransactionMgr : OrderLocationTransactionBaseMgr, IOrderLocationTransactionMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IUomConversionMgrE uomConversionMgrE { get; set; }
        public IBomMgrE bomMgrE { get; set; }
        public IBomDetailMgrE bomDetailMgrE { get; set; }
        public IOrderOperationMgrE orderOperationMgrE { get; set; }
        public IPartyMgrE partyMgr { get; set; }

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderLocationTransaction> GetOrderLocationTransaction(string orderNo)
        {
            DetachedCriteria criteria = DetachedCriteria.For<OrderLocationTransaction>();
            criteria.CreateAlias("OrderDetail", "od");
            criteria.CreateAlias("od.OrderHead", "oh");

            criteria.Add(Expression.Eq("oh.OrderNo", orderNo));

            return criteriaMgrE.FindAll<OrderLocationTransaction>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderLocationTransaction> GetOrderLocationTransaction(string orderNo, string ioType)
        {
            DetachedCriteria criteria = DetachedCriteria.For<OrderLocationTransaction>();
            criteria.CreateAlias("OrderDetail", "od");
            criteria.CreateAlias("od.OrderHead", "oh");

            criteria.Add(Expression.Eq("oh.OrderNo", orderNo));
            criteria.Add(Expression.Eq("IOType", ioType));

            return criteriaMgrE.FindAll<OrderLocationTransaction>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderLocationTransaction> GetOrderLocationTransaction(IList<string> orderNoList, string ioType)
        {
            IList<OrderLocationTransaction> orderLocTransList = new List<OrderLocationTransaction>();
            if (orderNoList != null && orderNoList.Count > 0)
            {
                foreach (string orderNo in orderNoList)
                {
                    IListHelper.AddRange<OrderLocationTransaction>(orderLocTransList,
                        this.GetOrderLocationTransaction(orderNo, ioType));
                }
            }
            return orderLocTransList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderLocationTransaction> GetOrderLocationTransaction(OrderHead orderHead)
        {
            return GetOrderLocationTransaction(orderHead.OrderNo);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderLocationTransaction> GetOrderLocationTransaction(OrderHead orderHead, string ioType)
        {
            return GetOrderLocationTransaction(orderHead.OrderNo, ioType);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderLocationTransaction> GetOrderLocationTransaction(int orderDetailId)
        {
            return GetOrderLocationTransaction(orderDetailId, null, null);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderLocationTransaction> GetOrderLocationTransaction(int orderDetailId, string ioType)
        {
            return GetOrderLocationTransaction(orderDetailId, ioType, null);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderLocationTransaction> GetOrderLocationTransaction(int orderDetailId, string ioType, string backflushMethod)
        {
            DetachedCriteria criteria = DetachedCriteria.For<OrderLocationTransaction>();
            criteria.CreateAlias("OrderDetail", "od");
            criteria.Add(Expression.Eq("od.Id", orderDetailId));

            if (ioType != null)
            {
                criteria.Add(Expression.Eq("IOType", ioType));
            }

            if (backflushMethod != null)
            {
                criteria.Add(Expression.Eq("BackFlushMethod", backflushMethod));
            }

            return criteriaMgrE.FindAll<OrderLocationTransaction>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderLocationTransaction> GetOrderLocationTransaction(OrderDetail orderDetail)
        {
            return GetOrderLocationTransaction(orderDetail.Id, null, null);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderLocationTransaction> GetOrderLocationTransaction(OrderDetail orderDetail, string ioType)
        {
            return GetOrderLocationTransaction(orderDetail.Id, ioType, null);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderLocationTransaction> GetOrderLocationTransaction(OrderDetail orderDetail, string ioType, string backFlushMethod)
        {
            return GetOrderLocationTransaction(orderDetail.Id, ioType, backFlushMethod);
        }

        [Transaction(TransactionMode.Unspecified)]
        public OrderLocationTransaction GenerateOrderLocationTransaction(
            OrderDetail orderDetail, Item item, BomDetail bomDetail, Uom uom, int operation,
            string ioType, string transactionType, decimal unitQty, Location loc,
            bool isShipScanHu, int? huLotSize, bool needPrint, string backFlushMethod, string itemVersion,
            string inspectLocation, string rejectLocation)
        {
            return GenerateOrderLocationTransaction(
            orderDetail, item, bomDetail, uom, operation,
            ioType, transactionType, unitQty, unitQty, loc,
            isShipScanHu, huLotSize, needPrint, backFlushMethod, itemVersion,
            inspectLocation, rejectLocation);
        }

        [Transaction(TransactionMode.Unspecified)]
        public OrderLocationTransaction GenerateOrderLocationTransaction(
            OrderDetail orderDetail, Item item, BomDetail bomDetail, Uom uom, int operation,
            string ioType, string transactionType, decimal unitQty, decimal unitQty4Backflush, Location loc,
            bool isShipScanHu, int? huLotSize, bool needPrint, string backFlushMethod, string itemVersion,
            string inspectLocation, string rejectLocation)
        {
            OrderLocationTransaction orderLocationTransaction = new OrderLocationTransaction();
            orderLocationTransaction.OrderDetail = orderDetail;
            orderLocationTransaction.Item = item;
            orderLocationTransaction.RawItem = item;
            orderLocationTransaction.OrderedQty = unitQty * orderDetail.OrderedQty;   //根据unitQty计算实际的订单量

            orderLocationTransaction.Uom = uom;
            orderLocationTransaction.Operation = operation;
            orderLocationTransaction.IOType = ioType;
            orderLocationTransaction.TransactionType = transactionType;
            orderLocationTransaction.UnitQty = unitQty4Backflush;
            orderLocationTransaction.Location = loc;
            orderLocationTransaction.InspectLocation = inspectLocation;
            orderLocationTransaction.RejectLocation = rejectLocation;
            orderLocationTransaction.IsShipScanHu = isShipScanHu;  //仅生产有效
            orderLocationTransaction.BackFlushMethod = backFlushMethod;  //生产回冲物料方式
            orderLocationTransaction.ItemVersion = itemVersion;

            #region 分光分色等级
            orderLocationTransaction.SortLevel1From = item.SortLevel1From;
            orderLocationTransaction.SortLevel1To = item.SortLevel1To;
            orderLocationTransaction.ColorLevel1From = item.ColorLevel1From;
            orderLocationTransaction.ColorLevel1To = item.ColorLevel1To;
            orderLocationTransaction.SortLevel2From = item.SortLevel2From;
            orderLocationTransaction.SortLevel2To = item.SortLevel2To;
            orderLocationTransaction.ColorLevel2From = item.ColorLevel2From;
            orderLocationTransaction.ColorLevel2To = item.ColorLevel2To;
            #endregion

            if (huLotSize.HasValue)
            {
                orderLocationTransaction.HuLotSize = (int)(huLotSize.Value * unitQty);
            }
            orderLocationTransaction.NeedPrint = needPrint;
            orderLocationTransaction.IsAssemble = true;   //默认都安装
            if (bomDetail != null)
            {
                orderLocationTransaction.BomDetail = bomDetail;
                if (bomDetail.StructureType == BusinessConstants.CODE_MASTER_BOM_DETAIL_TYPE_VALUE_O)
                {
                    //如果是选装件，取bomDetail.Priority作为选装件的默认值，0代表默认不装，1代表默认安装
                    orderLocationTransaction.IsAssemble = (bomDetail.Priority != 0);
                }
            }

            if (uom.Code != item.Uom.Code)   //和库存单位不一致，需要进行转化
            {
                //单位转化，更改UnitQty和OrderedQty值
                orderLocationTransaction.Uom = item.Uom;
                orderLocationTransaction.UnitQty = this.uomConversionMgrE.ConvertUomQty(item, uom, unitQty4Backflush, item.Uom);
                orderLocationTransaction.OrderedQty = this.uomConversionMgrE.ConvertUomQty(item, uom, orderLocationTransaction.OrderedQty, item.Uom);

                if (orderLocationTransaction.HuLotSize.HasValue)
                {
                    orderLocationTransaction.HuLotSize = (int)(orderLocationTransaction.HuLotSize.Value * orderLocationTransaction.UnitQty);
                }
            }

            orderDetail.AddOrderLocationTransaction(orderLocationTransaction);
            return orderLocationTransaction;
        }

        [Transaction(TransactionMode.Requires)]
        public void AutoReplaceAbstractItem(OrderLocationTransaction orderLocationTransaction)
        {
            OrderDetail orderDetail = orderLocationTransaction.OrderDetail;
            OrderHead orderHead = orderDetail.OrderHead;
            //OrderOperation orderOperation = orderHead.GetOrderOperationByOperation(orderLocationTransaction.Operation.Value);

            //取得抽象件的子件
            BomDetail bomDetail = this.bomDetailMgrE.GetDefaultBomDetailForAbstractItem(
                orderLocationTransaction.Item, orderHead.Routing, orderHead.StartTime,
                orderLocationTransaction.OrderDetail.DefaultLocationFrom);

            if (bomDetail != null)
            {
                //删除抽象件
                this.DeleteOrderLocationTransaction(orderLocationTransaction);
                orderDetail.RemoveOrderLocationTransaction(orderLocationTransaction);
                if (orderLocationTransaction.Operation != 0)
                {
                    //删除对应的OrderOp
                    this.orderOperationMgrE.TryDeleteOrderOperation(orderHead, orderLocationTransaction.Operation);
                }

                //尝试分解子件
                string bomCode = this.bomMgrE.FindBomCode(bomDetail.Item);
                IList<BomDetail> bomDetailList = this.bomDetailMgrE.GetFlatBomDetail(bomCode, orderHead.StartTime);
                if (bomDetailList != null && bomDetailList.Count > 0)
                {
                    //子件有Bom
                    foreach (BomDetail subBomDetail in bomDetailList)
                    {
                        //循环插入子件的Bom
                        OrderLocationTransaction newOrderLocationTransaction = this.AddNewMaterial(orderDetail, subBomDetail, orderLocationTransaction.Location, orderLocationTransaction.OrderedQty);
                        if (newOrderLocationTransaction != null &&
                            newOrderLocationTransaction.Item.Type == BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_A)
                        {
                            //如果子件的Bom还包含抽象件，嵌套调用替换抽象零件方法
                            this.AutoReplaceAbstractItem(newOrderLocationTransaction);
                        }
                    }
                }
                else
                {
                    //子件没有Bom，用子件直接替换抽象件
                    this.AddNewMaterial(orderDetail, bomDetail, orderLocationTransaction.Location, orderLocationTransaction.OrderedQty);
                }
            }
            else
            {
                throw new BusinessErrorException("Bom.Error.NotFoundForItem", orderLocationTransaction.Item.Code);
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void ReplaceAbstractItem(OrderLocationTransaction orderLocationTransaction, BomDetail bomDetail)
        {
            OrderDetail orderDetail = orderLocationTransaction.OrderDetail;
            OrderHead orderHead = orderDetail.OrderHead;

            //删除抽象件
            this.DeleteOrderLocationTransaction(orderLocationTransaction);
            orderDetail.RemoveOrderLocationTransaction(orderLocationTransaction);
            if (orderLocationTransaction.Operation != 0)
            {
                //删除对应的OrderOp
                this.orderOperationMgrE.TryDeleteOrderOperation(orderHead, orderLocationTransaction.Operation);
            }

            //尝试分解子件
            string bomCode = this.bomMgrE.FindBomCode(bomDetail.Item);
            IList<BomDetail> bomDetailList = this.bomDetailMgrE.GetFlatBomDetail(bomCode, orderHead.StartTime);
            if (bomDetailList != null && bomDetailList.Count > 0)
            {
                //子件有Bom
                foreach (BomDetail subBomDetail in bomDetailList)
                {
                    //循环插入子件的Bom
                    OrderLocationTransaction newOrderLocationTransaction = this.AddNewMaterial(orderDetail, subBomDetail, orderLocationTransaction.Location, orderLocationTransaction.OrderedQty);
                    if (newOrderLocationTransaction != null &&
                        newOrderLocationTransaction.Item.Type == BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_A)
                    {
                        //如果子件的Bom还包含抽象件，嵌套调用替换抽象零件方法
                        this.AutoReplaceAbstractItem(newOrderLocationTransaction);
                    }
                }
            }
            else
            {
                //子件没有Bom，用子件直接替换抽象件
                this.AddNewMaterial(orderDetail, bomDetail, orderLocationTransaction.Location, orderLocationTransaction.OrderedQty);
            }

        }

        [Transaction(TransactionMode.Requires)]
        public OrderLocationTransaction AddNewMaterial(OrderDetail orderDetail, BomDetail bomDetail, Location orgLocation, decimal orgOrderedQty)
        {
            //如果是选装件并且默认不安装，不添加新物料
            if (bomDetail.StructureType != BusinessConstants.CODE_MASTER_BOM_DETAIL_TYPE_VALUE_O || bomDetail.Priority != 0)
            {
                foreach (OrderLocationTransaction orderLocationTransaction in orderDetail.OrderLocationTransactions)
                {
                    if (orderLocationTransaction.Item.Code == bomDetail.Item.Code
                        && orderLocationTransaction.Operation == bomDetail.Operation)
                    {
                        //合并相同物料
                        //decimal orderedQty = orderDetail.OrderedQty * orgOrderedQty * bomDetail.RateQty * (1 + bomDetail.ScrapPercentage);
                        decimal orderedQty = orgOrderedQty * bomDetail.RateQty * (1 + bomDetail.DefaultScrapPercentage);
                        if (orderLocationTransaction.Uom.Code != bomDetail.Uom.Code)
                        {
                            orderedQty = this.uomConversionMgrE.ConvertUomQty(orderLocationTransaction.Item.Code, bomDetail.Uom, orderedQty, orderLocationTransaction.Uom);
                        }
                        orderLocationTransaction.UnitQty += orgOrderedQty * bomDetail.RateQty * (1 + bomDetail.DefaultScrapPercentage);
                        orderLocationTransaction.OrderedQty += orderedQty;
                        this.UpdateOrderLocationTransaction(orderLocationTransaction);

                        return orderLocationTransaction;
                    }
                }

                Location bomLocFrom = bomDetail.Location != null ? bomDetail.Location : orgLocation;
                OrderLocationTransaction newOrderLocationTransaction =
                    this.GenerateOrderLocationTransaction(orderDetail, bomDetail.Item, bomDetail,
                                                        bomDetail.Uom, bomDetail.Operation, BusinessConstants.IO_TYPE_OUT, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_WO,
                                                        orgOrderedQty * bomDetail.RateQty * (1 + bomDetail.DefaultScrapPercentage), bomLocFrom,
                                                        bomDetail.IsShipScanHu, bomDetail.HuLotSize, bomDetail.NeedPrint, bomDetail.BackFlushMethod, null,
                                                        partyMgr.GetDefaultInspectLocation(orderDetail.OrderHead.PartyFrom.Code, orderDetail.DefaultInspectLocationFrom),
                                                        partyMgr.GetDefaultRejectLocation(orderDetail.OrderHead.PartyFrom.Code, orderDetail.DefaultRejectLocationFrom));

                this.CreateOrderLocationTransaction(newOrderLocationTransaction);
                orderDetail.AddOrderLocationTransaction(newOrderLocationTransaction);
                this.orderOperationMgrE.TryAddOrderOperation(orderDetail.OrderHead, bomDetail.Operation, bomDetail.Reference);

                return newOrderLocationTransaction;
            }

            return null;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderLocationTransaction> GetOpenOrderLocTransIn(string item, string location, string IOType, DateTime? winTime)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderLocationTransaction), "orderLocTrans");
            criteria.CreateAlias("OrderDetail", "od");
            criteria.CreateAlias("od.OrderHead", "oh");
            if (winTime.HasValue)
                criteria.Add(Expression.Le("oh.WindowTime", (DateTime)winTime));
            criteria.Add(Expression.Eq("orderLocTrans.Item.Code", item));
            criteria.Add(Expression.Eq("orderLocTrans.Location.Code", location));
            criteria.Add(Expression.Eq("orderLocTrans.IOType", IOType));
            criteria.Add(Expression.Or(Expression.Eq("oh.Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT),
                Expression.Eq("oh.Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)));
            IList<OrderLocationTransaction> orderLocTransList = criteriaMgrE.FindAll<OrderLocationTransaction>(criteria);

            return orderLocTransList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderLocationTransaction> GetOpenOrderLocTransOut(string item, string location, string IOType, DateTime? startTime)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderLocationTransaction), "orderLocTrans");
            criteria.CreateAlias("OrderDetail", "od");
            criteria.CreateAlias("od.OrderHead", "oh");
            if (startTime.HasValue)
                criteria.Add(Expression.Le("oh.StartTime", (DateTime)startTime));
            criteria.Add(Expression.Eq("orderLocTrans.Item.Code", item));
            criteria.Add(Expression.Eq("orderLocTrans.Location.Code", location));
            criteria.Add(Expression.Eq("orderLocTrans.IOType", IOType));
            criteria.Add(Expression.Or(Expression.Eq("oh.Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT),
                Expression.Eq("oh.Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)));
            IList<OrderLocationTransaction> orderLocTransList = criteriaMgrE.FindAll<OrderLocationTransaction>(criteria);

            return orderLocTransList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderLocationTransaction> GetPairOrderLocTrans(int orderDetId, string item, string IOType)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderLocationTransaction), "orderLocTrans");
            criteria.CreateAlias("OrderDetail", "od");
            criteria.Add(Expression.Eq("orderLocTrans.OrderDetail.Id", orderDetId));
            criteria.Add(Expression.Eq("orderLocTrans.Item.Code", item));
            criteria.Add(Expression.Eq("orderLocTrans.IOType", IOType));
            IList<OrderLocationTransaction> orderLocTransList = criteriaMgrE.FindAll<OrderLocationTransaction>(criteria);

            return orderLocTransList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderLocationTransaction> GetOpenOrderLocationTransaction(IList<string> itemList, IList<string> locList)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderLocationTransaction));
            criteria.CreateAlias("OrderDetail", "od");
            criteria.CreateAlias("od.OrderHead", "oh");
            OrderHelper.SetOpenOrderStatusCriteria(criteria, "oh.Status");
            if (itemList != null && itemList.Count > 0)
            {
                if (itemList.Count == 1)
                {
                    criteria.Add(Expression.Eq("Item.Code", itemList[0]));
                }
                else
                {
                    criteria.Add(Expression.InG<string>("Item.Code", itemList));
                }
            }
            if (locList != null && locList.Count > 0)
            {
                if (locList.Count == 1)
                {
                    criteria.Add(Expression.Eq("Location.Code", locList[0]));
                }
                else
                {
                    criteria.Add(Expression.InG<string>("Location.Code", locList));
                }
            }
            #region Projections
            ProjectionList projectionList = Projections.ProjectionList()
                .Add(Projections.Max("Id").As("Id"))
                .Add(Projections.Sum("OrderedQty").As("OrderedQty"))
                .Add(Projections.Sum("AccumulateQty").As("AccumulateQty"))
                .Add(Projections.GroupProperty("IOType").As("IOType"))
                .Add(Projections.GroupProperty("Item").As("Item"))
                .Add(Projections.GroupProperty("Location").As("Location"));

            criteria.SetProjection(projectionList);
            criteria.SetResultTransformer(Transformers.AliasToBean(typeof(OrderLocationTransaction)));
            #endregion

            return criteriaMgrE.FindAll<OrderLocationTransaction>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderLocationTransaction> GetOrderLocationTransaction(OrderDetail orderDetail, string ioType, Item item)
        {
            DetachedCriteria criteria = DetachedCriteria.For<OrderLocationTransaction>();
            criteria.Add(Expression.Eq("OrderDetail", orderDetail));
            criteria.Add(Expression.Eq("Item", item));
            criteria.Add(Expression.Eq("IOType", ioType));

            return criteriaMgrE.FindAll<OrderLocationTransaction>(criteria);
        }

        //[Transaction(TransactionMode.Unspecified)]
        //public IList<OrderLocationTransaction> GetTobeBackFlushOrderLocationTransaction(string flowCode)
        //{
        //    DetachedCriteria criteria = DetachedCriteria.For<OrderLocationTransaction>();
        //    criteria.CreateAlias("OrderDetail", "od");
        //    criteria.CreateAlias("od.OrderHead", "oh");
        //    criteria.CreateAlias("oh.Flow", "f");

        //    criteria.Add(Expression.Gt("PlannedBackFlushQty", 0));
        //    criteria.Add(Expression.Eq("f.Code", flowCode));
        //    criteria.Add(Expression.Eq("BackFlushMethod", BusinessConstants.CODE_MASTER_BACKFLUSH_METHOD_VALUE_BATCH_FEED));

        //    return criteriaMgrE.FindAll<OrderLocationTransaction>(criteria);
        //}
        #endregion Customized Methods
    }
}


#region Extend Class





namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class OrderLocationTransactionMgrE : com.Sconit.Service.MasterData.Impl.OrderLocationTransactionMgr, IOrderLocationTransactionMgrE
    {

    }
}
#endregion

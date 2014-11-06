using com.Sconit.Service.Ext.MasterData;


using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using NHibernate.Expression;
using System.Linq;
using com.Sconit.Entity;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class OrderOperationMgr : OrderOperationBaseMgr, IOrderOperationMgr
    {
        private string[] OrderOperationCloneFields = new string[] 
            { 
                "Operation",
                "Reference",
                "Activity",
                "WorkCenter",
                "SetupTime",
                "RunTime",
                "MoveTime",
                "MachineQty",
                "Yield",
                "Location"
            };

        public IRoutingDetailMgrE routingDetailMgrE { get; set; }
        public ICriteriaMgrE criteriaMgrE { get; set; }

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public OrderOperation GenerateOrderOperation(OrderHead orderHead, RoutingDetail routingDetail)
        {
            OrderOperation orderOp = new OrderOperation();
            CloneHelper.CopyProperty(routingDetail, orderOp, OrderOperationCloneFields);
            orderOp.OrderHead = orderHead;
            //todo UnitTime和WorkTime计算

            orderHead.AddOrderOperation(orderOp);
            return orderOp;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderOperation> GenerateOrderOperation(OrderHead orderHead)
        {
            IList<Routing> routingList = null;

            if (orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RWO
                || orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RUS)
            {
                routingList = (from orderDet in orderHead.OrderDetails
                               where orderDet.DefaultReturnRouting != null
                               select orderDet.DefaultReturnRouting).Distinct().ToList();
            }
            else
            {
                routingList = (from orderDet in orderHead.OrderDetails
                               where orderDet.DefaultRouting != null
                               select orderDet.DefaultRouting).Distinct().ToList();
            }

            if (routingList != null && routingList.Count == 1)
            {
                orderHead.Routing = routingList[0];
            }

            if (orderHead.Routing != null)
            {
                IList<OrderOperation> orderOperationList = new List<OrderOperation>();
                IList<RoutingDetail> routingDetailList = this.routingDetailMgrE.GetRoutingDetail(orderHead.Routing, orderHead.StartTime);
                foreach (RoutingDetail routingDetail in routingDetailList)
                {
                    orderOperationList.Add(GenerateOrderOperation(orderHead, routingDetail));
                }

                return orderOperationList;

            }

            return null;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderOperation> GetOrderOperation(string orderNo)
        {
            DetachedCriteria criteria = DetachedCriteria.For<OrderOperation>();
            criteria.Add(Expression.Eq("OrderHead.OrderNo", orderNo));

            return this.criteriaMgrE.FindAll<OrderOperation>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderOperation> GetOrderOperation(OrderHead orderHead)
        {
            return GetOrderOperation(orderHead.OrderNo);
        }

        [Transaction(TransactionMode.Requires)]
        public void TryAddOrderOperation(OrderHead orderHead, int operation, string reference)
        {
            bool hasOp = false;
            foreach (OrderOperation orderOperation in orderHead.OrderOperations)
            {
                if (orderOperation.Operation == operation)
                {
                    if (orderOperation.Reference == reference
                        || (orderOperation.Reference == null && reference == null))
                    {
                        hasOp = true;
                    }
                }
            }

            if (!hasOp)
            {
                //没有找到Op，新增

                RoutingDetail routingDetail = this.routingDetailMgrE.LoadRoutingDetail(orderHead.Routing, operation, reference);
                if (routingDetail != null)
                {
                    OrderOperation orderOperation = this.GenerateOrderOperation(orderHead, routingDetail);
                    this.CreateOrderOperation(orderOperation);
                }
                else
                {
                    throw new BusinessErrorException("Order.Error.RoutingDetail.Not.Found", orderHead.Routing.Code, operation.ToString(), reference);
                }
            }
        }

        [Transaction(TransactionMode.Requires)]
        public void TryDeleteOrderOperation(OrderHead orderHead, int operation)
        {
            IList<int> operationList = new List<int>();
            operationList.Add(operation);
            TryDeleteOrderOperation(orderHead, operationList);
        }

        [Transaction(TransactionMode.Requires)]
        public void TryDeleteOrderOperation(OrderHead orderHead, IList<int> operationList)
        {
            IList<int> usedOpList = new List<int>();
            foreach (OrderDetail orderDetail in orderHead.OrderDetails)
            {
                foreach (OrderLocationTransaction orderLocationTransaction in orderDetail.OrderLocationTransactions)
                {
                    if (operationList.Contains(orderLocationTransaction.Operation)
                        && !usedOpList.Contains(orderLocationTransaction.Operation))
                    {
                        //查找那些Op被其他明细使用，加入到usedOpList表中
                        usedOpList.Add(orderLocationTransaction.Operation);
                    }
                }
            }

            foreach (int op in operationList)
            {
                //没有使用的op将被删除
                if (!usedOpList.Contains(op))
                {
                    this.DeleteOrderOperation(orderHead.OrderNo, op);
                }
            }
        }

        #endregion Customized Methods
    }
}


#region Extend Class



namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class OrderOperationMgrE : com.Sconit.Service.MasterData.Impl.OrderOperationMgr, IOrderOperationMgrE
    {

    }
}
#endregion

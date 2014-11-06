using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using com.Sconit.Entity;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class OrderHeadMgr : OrderHeadBaseMgr, IOrderHeadMgr
    {
        public IOrderDetailMgrE orderDetailMgrE { get; set; }
        public ICriteriaMgrE criteriaMgrE { get; set; }
        //public IOrderOperationMgrE orderOperationMgrE { get; set; }
        

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public OrderHead CheckAndLoadOrderHead(string orderNo)
        {
            OrderHead orderHead = this.LoadOrderHead(orderNo);
            if (orderHead == null)
            {
                throw new BusinessErrorException("Order.Error.OrderNoNotExist", orderNo);
            }

            return orderHead;
        }

        [Transaction(TransactionMode.Unspecified)]
        public OrderHead CheckAndLoadOrderHead(string orderNo, bool includeDetail)
        {
            OrderHead orderHead = this.LoadOrderHead(orderNo, includeDetail);
            if (orderHead == null)
            {
                throw new BusinessErrorException("Order.Error.OrderNoNotExist", orderNo);
            }

            return orderHead;
        }

        [Transaction(TransactionMode.Unspecified)]
        public OrderHead LoadOrderHead(String orderNo, bool includeDetail)
        {
            return LoadOrderHead(orderNo, true, false, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public OrderHead LoadOrderHead(String orderNo, bool includeDetail, bool includeOperation)
        {
            return LoadOrderHead(orderNo, true, true, false);
        }

        //[Transaction(TransactionMode.Unspecified)]
        //public OrderHead LoadOrderHead(String orderNo, bool includeDetail, bool includeLocTrans)
        //{
        //    return LoadOrderHead(orderNo, true, false, true);
        //}

        public IList<OrderHead> GetOrderHead(int count, string status)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderHead));
            criteria.AddOrder(Order.Desc("CreateDate"));
            if (status != null && status != string.Empty)
            {
                criteria.Add(Expression.Eq("Status", status));
            }
            return criteriaMgrE.FindAll<OrderHead>(criteria, 0, count);
        }

        public IList<OrderHead> GetOrderHead(int count)
        {
            return GetOrderHead(count, null);
        }

        public IList<OrderHead> GetOrderHead(string status)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderHead));
            criteria.AddOrder(Order.Desc("CreateDate"));
            if (status != null && status != string.Empty)
            {
                criteria.Add(Expression.Eq("Status", status));
            }
            return criteriaMgrE.FindAll<OrderHead>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public OrderHead LoadOrderHead(String orderNo, bool includeDetail, bool includeOperation, bool includeLocTrans)
        {
            OrderHead orderHead = this.LoadOrderHead(orderNo);

            if (includeDetail && orderHead.OrderDetails != null && orderHead.OrderDetails.Count > 0)
            {
                foreach (OrderDetail orderDetail in orderHead.OrderDetails)
                {
                    if (includeLocTrans && orderDetail.OrderLocationTransactions != null && orderDetail.OrderLocationTransactions.Count > 0)
                    {
                    }
                }
            }

            if (includeOperation && orderHead.OrderOperations != null && orderHead.OrderOperations.Count > 0)
            {
            }

            return orderHead;
        }

        //创建OrderLocationTransaction、OrderOperation
        [Transaction(TransactionMode.Unspecified)]
        public void GenerateOrderHeadSubsidiary(OrderHead orderHead)
        {
            //#region 生产，设置Routing
            //if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION && orderHead.Routing != null)
            //{
            //    this.orderOperationMgrE.GenerateOrderOperation(orderHead);
            //}
            //#endregion

            if (orderHead.OrderDetails != null && orderHead.OrderDetails.Count > 0)
            {
                foreach (OrderDetail orderDetail in orderHead.OrderDetails)
                {
                    this.orderDetailMgrE.GenerateOrderDetailSubsidiary(orderDetail);
                }
            }
        }

        [Transaction(TransactionMode.Requires)]
        public IList<OrderHead> GetOrderHead(DateTime lastModifyDate, int firstRow, int maxRows, string status)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderHead));
            criteria.Add(Expression.Gt("LastModifyDate", lastModifyDate));
            criteria.AddOrder(Order.Asc("LastModifyDate"));
            if (status != null && status != string.Empty)
            {
                criteria.Add(Expression.Eq("Status", status));
            }
            IList<OrderHead> orderHeadList = criteriaMgrE.FindAll<OrderHead>(criteria, firstRow, maxRows);
            if (orderHeadList.Count > 0)
            {
                return orderHeadList;
            }
            return null;
        }

        #endregion Customized Methods
    }
}


#region Extend Class



namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class OrderHeadMgrE : com.Sconit.Service.MasterData.Impl.OrderHeadMgr, IOrderHeadMgrE
    {
        
    }
}
#endregion

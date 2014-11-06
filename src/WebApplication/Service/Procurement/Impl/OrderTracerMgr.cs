using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Procurement;
using com.Sconit.Entity.Procurement;
using NHibernate.Expression;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Procurement.Impl
{
    [Transactional]
    public class OrderTracerMgr : OrderTracerBaseMgr, IOrderTracerMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }

        #region Customized Methods

        [Transaction(TransactionMode.Requires)]
        public void DeleteOrderTracerByOrderDetailId(IList<int> olt)
        {
            if ((olt == null) || (olt.Count == 0))
            {
                return;
            }

            StringBuilder hql = new StringBuilder("From com.Sconit.Entity.Procurement.OrderTracer where OrderDetId in ( ");

            for (int i = 0; i < olt.Count; i++)
            {
                if (i != 0)
                    hql.Append(",");
                hql.Append(olt[i]);

            }
            hql.Append(" )");

            criteriaMgrE.DeleteWithHql(hql.ToString());

        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteOrderTracerByOrderDetail(IList<OrderDetail> otList)
        {
            if ((otList == null) || (otList.Count == 0))
            {
                return;
            }

            StringBuilder hql = new StringBuilder("From com.Sconit.Entity.Procurement.OrderTracer where OrderDetId in ( ");

            for (int i = 0; i < otList.Count; i++)
            {
                if (i != 0)
                    hql.Append(",");
                hql.Append(otList[i].Id.ToString());

            }
            hql.Append(" )");

            criteriaMgrE.DeleteWithHql(hql.ToString());

        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderTracer> GetOrderTracer(DateTime? startTime, DateTime? endTime, string TracerType)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderTracer));
            if (TracerType != null && TracerType.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("TracerType", TracerType));
            }
            if (startTime != null)
            {
                criteria.Add(Expression.Ge("ReqTime", startTime));
            }
            if (endTime != null)
            {
                criteria.Add(Expression.Le("ReqTime", endTime));

            }
            return criteriaMgrE.FindAll<OrderTracer>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderTracer> GetOrderTracer(List<int> ids, List<string> tracerTypes)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderTracer));
            if (ids != null)
            {
                criteria.Add(Expression.In("RefId", ids));
            }
            if (tracerTypes != null && tracerTypes.Count > 0)
            {
                criteria.Add(Expression.In("TracerType", tracerTypes));
            }
            criteria.CreateAlias("OrderDetail.OrderHead", "orderHead");
            criteria.Add(Expression.In("orderHead.Status", new List<string> { BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS,
                BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE, BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT}));
            return criteriaMgrE.FindAll<OrderTracer>(criteria);
        }

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.Procurement.Impl
{
    [Transactional]
    public partial class OrderTracerMgrE : com.Sconit.Service.Procurement.Impl.OrderTracerMgr, IOrderTracerMgrE
    {
    }
}

#endregion Extend Class
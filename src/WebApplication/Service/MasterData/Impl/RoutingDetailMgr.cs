using com.Sconit.Service.Ext.MasterData;


using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using NHibernate.Expression;
using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class RoutingDetailMgr : RoutingDetailBaseMgr, IRoutingDetailMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public override RoutingDetail LoadRoutingDetail(Routing routing, int operation, string reference)
        {
            return LoadRoutingDetail(routing.Code, operation, reference);
        }

        [Transaction(TransactionMode.Unspecified)]
        public override RoutingDetail LoadRoutingDetail(string routingCode, int operation, string reference)
        {
            RoutingDetail routingDetail = null;
            if (reference == null)
            {
                DetachedCriteria criteria = DetachedCriteria.For<RoutingDetail>();
                criteria.Add(Expression.Eq("Routing.Code",routingCode))
                    .Add(Expression.Eq("Operation", operation)).Add(Expression.IsNull("Reference"));
                IList<RoutingDetail> routingDetailList = criteriaMgrE.FindAll<RoutingDetail>(criteria);
                if (routingDetailList != null && routingDetailList.Count > 0)
                {
                    routingDetail = routingDetailList[0];
                }
            }
            else
            {
               routingDetail = base.LoadRoutingDetail(routingCode, operation, reference);
            }
            return routingDetail;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<RoutingDetail> GetRoutingDetail(string routingCode, DateTime effectiveDate)
        {
            DetachedCriteria criteria = DetachedCriteria.For<RoutingDetail>();
            criteria.Add(Expression.Eq("Routing.Code", routingCode));
            criteria.Add(Expression.Le("StartDate", effectiveDate));
            criteria.Add(Expression.Or(Expression.Ge("EndDate", effectiveDate), Expression.IsNull("EndDate")));

            return criteriaMgrE.FindAll<RoutingDetail>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<RoutingDetail> GetRoutingDetail(Routing routing, DateTime effectiveDate)
        {
            return GetRoutingDetail(routing.Code, effectiveDate);
        }

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class RoutingDetailMgrE : com.Sconit.Service.MasterData.Impl.RoutingDetailMgr, IRoutingDetailMgrE
    {
        
    }
}
#endregion

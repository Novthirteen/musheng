using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using NHibernate.Expression;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class OrderPlannedBackflushMgr : OrderPlannedBackflushBaseMgr, IOrderPlannedBackflushMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderPlannedBackflush> GetActiveOrderPlannedBackflush(string prodLineCode, string[] items)
        {
            DetachedCriteria criteria = DetachedCriteria.For<OrderPlannedBackflush>();
            criteria.CreateAlias("OrderLocationTransaction", "olt");
            criteria.CreateAlias("olt.OrderDetail", "od");
            criteria.CreateAlias("od.OrderHead", "oh");
            //criteria.CreateAlias("oh.Flow", "f");

            criteria.Add(Expression.Eq("IsActive", true));
            criteria.Add(Expression.Eq("oh.Flow", prodLineCode));
            if (items != null && items.Length > 0)
            {
                criteria.CreateAlias("olt.Item", "item");
                criteria.Add(Expression.In("item.Code", items));
            }

            return criteriaMgrE.FindAll<OrderPlannedBackflush>(criteria);
        }

        #endregion Customized Methods
    }
}


#region Extend Class


namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class OrderPlannedBackflushMgrE : com.Sconit.Service.MasterData.Impl.OrderPlannedBackflushMgr, IOrderPlannedBackflushMgrE
    {

    }
}
#endregion

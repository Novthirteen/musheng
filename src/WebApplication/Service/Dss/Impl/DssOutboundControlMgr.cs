using com.Sconit.Service.Ext.Dss;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Dss;
using com.Sconit.Entity.Dss;
using NHibernate.Expression;
using com.Sconit.Service.Ext.Criteria;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Dss.Impl
{
    [Transactional]
    public class DssOutboundControlMgr : DssOutboundControlBaseMgr, IDssOutboundControlMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public IList<DssOutboundControl> GetDssOutboundControl()
        {
            DetachedCriteria criteria = DetachedCriteria.For<DssOutboundControl>()
                .Add(Expression.Eq("IsActive", true));

            criteria.AddOrder(Order.Asc("Sequence"));

            return this.criteriaMgrE.FindAll<DssOutboundControl>(criteria);
        }
        #endregion Customized Methods
    }
}



#region Extend Class


namespace com.Sconit.Service.Ext.Dss.Impl
{
    [Transactional]
    public partial class DssOutboundControlMgrE : com.Sconit.Service.Dss.Impl.DssOutboundControlMgr, IDssOutboundControlMgrE
    {
        
    }
}
#endregion

using com.Sconit.Service.Ext.Dss;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Dss;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Entity.Dss;
using NHibernate.Expression;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Dss.Impl
{
    [Transactional]
    public class DssInboundControlMgr : DssInboundControlBaseMgr, IDssInboundControlMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public IList<DssInboundControl> GetDssInboundControl()
        {
            DetachedCriteria criteria = DetachedCriteria.For<DssInboundControl>();

            criteria.AddOrder(Order.Asc("Sequence"));

            return this.criteriaMgrE.FindAll<DssInboundControl>(criteria);
        }
        #endregion Customized Methods
    }
}



#region Extend Class


namespace com.Sconit.Service.Ext.Dss.Impl
{
    [Transactional]
    public partial class DssInboundControlMgrE : com.Sconit.Service.Dss.Impl.DssInboundControlMgr, IDssInboundControlMgrE
    {
         
    }
}
#endregion

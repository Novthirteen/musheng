using com.Sconit.Service.Ext.Dss;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Dss;
using NHibernate.Expression;
using com.Sconit.Entity.Dss;
using com.Sconit.Service.Ext.Criteria;

namespace com.Sconit.Service.Dss.Impl
{
    [Transactional]
    public class DssFtpControlMgr : DssFtpControlBaseMgr, IDssFtpControlMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public IList<DssFtpControl> GetDssFtpControl(string IOType)
        {
            DetachedCriteria criteria = DetachedCriteria.For<DssFtpControl>();

            criteria.Add(Expression.Eq("IOType", IOType));

            return this.criteriaMgrE.FindAll<DssFtpControl>(criteria);
        }
        #endregion Customized Methods
    }
}



#region Extend Class
namespace com.Sconit.Service.Ext.Dss.Impl
{
    [Transactional]
    public partial class DssFtpControlMgrE : com.Sconit.Service.Dss.Impl.DssFtpControlMgr, IDssFtpControlMgrE
    {
        
    }
}
#endregion

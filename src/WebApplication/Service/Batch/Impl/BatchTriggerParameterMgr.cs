using com.Sconit.Service.Ext.Batch;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Batch;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using com.Sconit.Entity.Batch;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Batch.Impl
{
    [Transactional]
    public class BatchTriggerParameterMgr : BatchTriggerParameterBaseMgr, IBatchTriggerParameterMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        

        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public IList<BatchTriggerParameter> GetBatchTriggerParameter(int jobId)
        {
            DetachedCriteria criteria = DetachedCriteria.For<BatchTriggerParameter>();

            criteria.Add(Expression.Eq("BatchTrigger.Id", jobId));

            return this.criteriaMgrE.FindAll<BatchTriggerParameter>(criteria);
        }

        #endregion Customized Methods
    }
}


#region À©Õ¹

namespace com.Sconit.Service.Ext.Batch.Impl
{
    [Transactional]
    public partial class BatchTriggerParameterMgrE : com.Sconit.Service.Batch.Impl.BatchTriggerParameterMgr, IBatchTriggerParameterMgrE
    {

    }
}
#endregion

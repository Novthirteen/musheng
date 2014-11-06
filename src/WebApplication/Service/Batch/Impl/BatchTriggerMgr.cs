using com.Sconit.Service.Ext.Batch;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Batch;
using com.Sconit.Entity.Batch;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using com.Sconit.Entity;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Batch.Impl
{
    [Transactional]
    public class BatchTriggerMgr : BatchTriggerBaseMgr, IBatchTriggerMgr
    {       
        public ICriteriaMgrE criteriaMgrE { get; set; }


        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public IList<BatchTrigger> GetTobeFiredTrigger()
        {
            DetachedCriteria criteria = DetachedCriteria.For<BatchTrigger>();

            criteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS));
            criteria.Add(Expression.Le("NextFireTime", DateTime.Now));

            return this.criteriaMgrE.FindAll<BatchTrigger>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<BatchTrigger> GetActiveTrigger()
        {
            DetachedCriteria criteria = DetachedCriteria.For<BatchTrigger>();

            criteria.Add(Expression.Not(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)));
            return this.criteriaMgrE.FindAll<BatchTrigger>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public BatchTrigger LoadLeanEngineTrigger()
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(BatchTrigger));
            criteria.Add(Expression.Eq("Name", "LeanEngineTrigger"));//todo,constants

            IList<BatchTrigger> result = criteriaMgrE.FindAll<BatchTrigger>(criteria);
            if (result != null && result.Count > 0)
            {
                return result[0];
            }
            return null;
        }

        #endregion Customized Methods
    }
}


#region À©Õ¹


namespace com.Sconit.Service.Ext.Batch.Impl
{
    [Transactional]
    public partial class BatchTriggerMgrE : com.Sconit.Service.Batch.Impl.BatchTriggerMgr, IBatchTriggerMgrE
    {

    }
}
#endregion

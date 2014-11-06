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

//TODO: Add other using statements here.

namespace com.Sconit.Service.Batch.Impl
{
    [Transactional]
    public class BatchJobParameterMgr : BatchJobParameterBaseMgr, IBatchJobParameterMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }


        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public IList<BatchJobParameter> GetBatchJobParameter(int jobId)
        {
            DetachedCriteria criteria = DetachedCriteria.For<BatchJobParameter>();

            criteria.Add(Expression.Eq("BatchJobDetail.Id", jobId));

            return this.criteriaMgrE.FindAll<BatchJobParameter>(criteria);
        }

        #endregion Customized Methods
    }
}


#region À©Õ¹


namespace com.Sconit.Service.Ext.Batch.Impl
{
    [Transactional]
    public partial class BatchJobParameterMgrE : com.Sconit.Service.Batch.Impl.BatchJobParameterMgr, IBatchJobParameterMgrE
    {

    }
}
#endregion

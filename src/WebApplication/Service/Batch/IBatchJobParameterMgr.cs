using System;
using com.Sconit.Entity.Batch;
using System.Collections.Generic;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Batch
{
    public interface IBatchJobParameterMgr : IBatchJobParameterBaseMgr
    {
        #region Customized Methods

        IList<BatchJobParameter> GetBatchJobParameter(int jobId);

        #endregion Customized Methods
    }
}

#region Extend Interface

namespace com.Sconit.Service.Ext.Batch
{
    public partial interface IBatchJobParameterMgrE : com.Sconit.Service.Batch.IBatchJobParameterMgr
    {
       
    }
}

#endregion

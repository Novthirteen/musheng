using com.Sconit.Service.Ext.Batch;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Batch;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Batch.Impl
{
    [Transactional]
    public class BatchRunLogMgr : BatchRunLogBaseMgr, IBatchRunLogMgr
    {
        

        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region À©Õ¹


namespace com.Sconit.Service.Ext.Batch.Impl
{
    [Transactional]
    public partial class BatchRunLogMgrE : com.Sconit.Service.Batch.Impl.BatchRunLogMgr, IBatchRunLogMgrE
    {

    }
}
#endregion

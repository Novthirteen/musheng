using System;
using System.Collections.Generic;
using com.Sconit.Entity.Batch;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Batch
{
    public interface IBatchTriggerMgr : IBatchTriggerBaseMgr
    {
        #region Customized Methods

        IList<BatchTrigger> GetTobeFiredTrigger();

        IList<BatchTrigger> GetActiveTrigger();

        BatchTrigger LoadLeanEngineTrigger();

        #endregion Customized Methods
    }
}

#region Extend Interface

namespace com.Sconit.Service.Ext.Batch
{
    public partial interface IBatchTriggerMgrE : com.Sconit.Service.Batch.IBatchTriggerMgr
    {
       
    }
}

#endregion

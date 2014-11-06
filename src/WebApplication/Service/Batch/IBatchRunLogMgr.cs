using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Batch
{
    public interface IBatchRunLogMgr : IBatchRunLogBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}

#region Extend Interface

namespace com.Sconit.Service.Ext.Batch
{
    public partial interface IBatchRunLogMgrE : com.Sconit.Service.Batch.IBatchRunLogMgr
    {
       
    }
}

#endregion

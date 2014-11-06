using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Batch
{
    public interface IBatchJobDetailMgr : IBatchJobDetailBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}

#region Extend Interface

namespace com.Sconit.Service.Ext.Batch
{
    public partial interface IBatchJobDetailMgrE : com.Sconit.Service.Batch.IBatchJobDetailMgr
    {
       
    }
}

#endregion


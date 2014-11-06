using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.View
{
    public interface ILeanEngineViewMgr : ILeanEngineViewBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.View
{
    public partial interface ILeanEngineViewMgrE : com.Sconit.Service.View.ILeanEngineViewMgr
    {
        
    }
}

#endregion Extend interface

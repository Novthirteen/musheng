using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IMenuMgr : IMenuBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IMenuMgrE : com.Sconit.Service.MasterData.IMenuMgr
    {
    }
}

#endregion Extend Interface
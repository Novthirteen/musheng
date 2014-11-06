using com.Sconit.Service.Ext.MasterData;
using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IRepackDetailMgr : IRepackDetailBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}





#region Extend Interface



namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IRepackDetailMgrE : com.Sconit.Service.MasterData.IRepackDetailMgr
    {
        
    }
}

#endregion

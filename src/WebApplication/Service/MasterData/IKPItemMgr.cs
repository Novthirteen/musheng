using com.Sconit.Service.Ext.MasterData;
using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IKPItemMgr : IKPItemBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}





#region Extend Interface



namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IKPItemMgrE : com.Sconit.Service.MasterData.IKPItemMgr
    {
        
    }
}

#endregion

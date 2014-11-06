using com.Sconit.Service.Ext.MasterData;
using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface ICompanyMgr : ICompanyBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}





#region Extend Interface



namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface ICompanyMgrE : com.Sconit.Service.MasterData.ICompanyMgr
    {
        
    }
}

#endregion

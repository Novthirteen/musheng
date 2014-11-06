using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IMenuCompanyMgr : IMenuCompanyBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IMenuCompanyMgrE : com.Sconit.Service.MasterData.IMenuCompanyMgr
    {
    }
}

#endregion Extend Interface
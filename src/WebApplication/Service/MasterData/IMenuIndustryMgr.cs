using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IMenuIndustryMgr : IMenuIndustryBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IMenuIndustryMgrE : com.Sconit.Service.MasterData.IMenuIndustryMgr
    {
    }
}

#endregion Extend Interface
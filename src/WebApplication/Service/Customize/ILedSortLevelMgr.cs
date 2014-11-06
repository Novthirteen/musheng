using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Customize
{
    public interface ILedSortLevelMgr : ILedSortLevelBaseMgr
    {
        #region Customized Methods

        bool CheckLedFeedSortLevel(string itemCode, string brand, string startLevel, string endLevel, string targetLevel);

        bool IsNearByLedSortLevel(string itemCode, string brand, string orgLevel, string targetLevel);


        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.Customize
{
    public partial interface ILedSortLevelMgrE : com.Sconit.Service.Customize.ILedSortLevelMgr
    {
    }
}

#endregion Extend Interface
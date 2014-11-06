using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Customize
{
    public interface ILedColorLevelMgr : ILedColorLevelBaseMgr
    {
        #region Customized Methods

        bool CheckLedFeedColorLevel(string itemCode, string brand, string startLevel, string endLevel, string targetLevel);

        bool IsNearByLedColorLevel(string itemCode, string brand, string orgLevel, string targetLevel);

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.Customize
{
    public partial interface ILedColorLevelMgrE : com.Sconit.Service.Customize.ILedColorLevelMgr
    {
    }
}

#endregion Extend Interface
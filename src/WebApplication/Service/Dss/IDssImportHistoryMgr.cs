using System;
using System.Collections.Generic;
using com.Sconit.Entity.Dss;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Dss
{
    public interface IDssImportHistoryMgr : IDssImportHistoryBaseMgr
    {
        #region Customized Methods

        IList<DssImportHistory> GetActiveDssImportHistory(int dssInboundCtrlId);

        void CreateDssImportHistory(IList<DssImportHistory> dssImportHistoryList);

        void UpdateDssImportHistory(IList<DssImportHistory> dssImportHistoryList);

        void UpdateDssImportHistory(IList<DssImportHistory> dssImportHistoryList, bool isActive);

        #endregion Customized Methods
    }
}



#region Extend Interface

namespace com.Sconit.Service.Ext.Dss
{
    public partial interface IDssImportHistoryMgrE : com.Sconit.Service.Dss.IDssImportHistoryMgr
    {
       
    }
}

#endregion

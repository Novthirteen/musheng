using System;
using System.Collections.Generic;
using com.Sconit.Entity.Dss;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Dss
{
    public interface IDssExportHistoryMgr : IDssExportHistoryBaseMgr
    {
        #region Customized Methods

        void CreateDssExportHistory(IList<DssExportHistory> dssExportHistoryList);

        #endregion Customized Methods
    }
}



#region Extend Interface





namespace com.Sconit.Service.Ext.Dss
{
    public partial interface IDssExportHistoryMgrE : com.Sconit.Service.Dss.IDssExportHistoryMgr
    {
        
    }
}

#endregion

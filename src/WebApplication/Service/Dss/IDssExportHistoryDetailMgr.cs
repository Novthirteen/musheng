using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Dss
{
    public interface IDssExportHistoryDetailMgr : IDssExportHistoryDetailBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}



#region Extend Interface



namespace com.Sconit.Service.Ext.Dss
{
    public partial interface IDssExportHistoryDetailMgrE : com.Sconit.Service.Dss.IDssExportHistoryDetailMgr
    {
        
    }
}

#endregion

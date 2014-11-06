using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface ISqlReportMgr : ISqlReportBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface ISqlReportMgrE : com.Sconit.Service.MasterData.ISqlReportMgr
    {
    }
}

#endregion Extend Interface
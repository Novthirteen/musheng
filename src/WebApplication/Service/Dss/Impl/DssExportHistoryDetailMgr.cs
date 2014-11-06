using com.Sconit.Service.Ext.Dss;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Dss;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Dss.Impl
{
    [Transactional]
    public class DssExportHistoryDetailMgr : DssExportHistoryDetailBaseMgr, IDssExportHistoryDetailMgr
    {
        

        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}



#region Extend Class

namespace com.Sconit.Service.Ext.Dss.Impl
{
    [Transactional]
    public partial class DssExportHistoryDetailMgrE : com.Sconit.Service.Dss.Impl.DssExportHistoryDetailMgr, IDssExportHistoryDetailMgrE
    {
    }
}
#endregion

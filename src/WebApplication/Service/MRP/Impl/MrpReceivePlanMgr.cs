using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MRP;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MRP.Impl
{
    [Transactional]
    public class MrpReceivePlanMgr : MrpReceivePlanBaseMgr, IMrpReceivePlanMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.MRP.Impl
{
    [Transactional]
    public partial class MrpReceivePlanMgrE : com.Sconit.Service.MRP.Impl.MrpReceivePlanMgr, IMrpReceivePlanMgrE
    {
    }
}

#endregion Extend Class
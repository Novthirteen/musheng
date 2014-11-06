using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.View;

//TODO: Add other using statements here.

namespace com.Sconit.Service.View.Impl
{
    [Transactional]
    public class InProcessLocationDetailTrackViewMgr : InProcessLocationDetailTrackViewBaseMgr, IInProcessLocationDetailTrackViewMgr
    {
        

        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}




#region Extend Interface

namespace com.Sconit.Service.Ext.View.Impl
{
    [Transactional]
    public partial class InProcessLocationDetailTrackViewMgrE : com.Sconit.Service.View.Impl.InProcessLocationDetailTrackViewMgr, IInProcessLocationDetailTrackViewMgrE
    {
        
    }
}
#endregion

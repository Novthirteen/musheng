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
    public class BillAgingViewMgr : BillAgingViewBaseMgr, IBillAgingViewMgr
    {
        

        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}



#region Extend Class

namespace com.Sconit.Service.Ext.View.Impl
{
    [Transactional]
    public partial class BillAgingViewMgrE : com.Sconit.Service.View.Impl.BillAgingViewMgr, IBillAgingViewMgrE
    {
        
    }
}
#endregion

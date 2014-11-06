using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost.Impl
{
    [Transactional]
    public class RawIOBMgr : RawIOBBaseMgr, IRawIOBMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.Cost.Impl
{
    [Transactional]
    public partial class RawIOBMgrE : com.Sconit.Service.Cost.Impl.RawIOBMgr, IRawIOBMgrE
    {
    }
}

#endregion Extend Class
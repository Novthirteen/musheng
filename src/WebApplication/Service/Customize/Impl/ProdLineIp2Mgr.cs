using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Customize;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Customize.Impl
{
    [Transactional]
    public class ProdLineIp2Mgr : ProdLineIp2BaseMgr, IProdLineIp2Mgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.Customize.Impl
{
    [Transactional]
    public partial class ProdLineIp2MgrE : com.Sconit.Service.Customize.Impl.ProdLineIp2Mgr, IProdLineIp2MgrE
    {
    }
}

#endregion Extend Class
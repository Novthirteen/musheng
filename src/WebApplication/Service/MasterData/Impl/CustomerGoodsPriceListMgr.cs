using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class CustomerGoodsPriceListMgr : CustomerGoodsPriceListBaseMgr, ICustomerGoodsPriceListMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class CustomerGoodsPriceListMgrE : com.Sconit.Service.MasterData.Impl.CustomerGoodsPriceListMgr, ICustomerGoodsPriceListMgrE
    {
    }
}

#endregion Extend Class
using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface ICustomerGoodsPriceListMgr : ICustomerGoodsPriceListBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface ICustomerGoodsPriceListMgrE : com.Sconit.Service.MasterData.ICustomerGoodsPriceListMgr
    {
    }
}

#endregion Extend Interface
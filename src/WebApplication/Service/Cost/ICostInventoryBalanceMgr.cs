using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface ICostInventoryBalanceMgr : ICostInventoryBalanceBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.Cost
{
    public partial interface ICostInventoryBalanceMgrE : com.Sconit.Service.Cost.ICostInventoryBalanceMgr
    {
    }
}

#endregion Extend Interface
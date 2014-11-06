using System;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface ICostAllocateTransactionMgr : ICostAllocateTransactionBaseMgr
    {
        #region Customized Methods

        void RecordCustomerGoodsDiff(Item customerGoods, decimal diffQty, string costCenterCode, User user);

        void RecordCustomerGoodsDiff(Item customerGoods, decimal diffQty, string costCenterCode, User user, DateTime effectiveDate);

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.Cost
{
    public partial interface ICostAllocateTransactionMgrE : com.Sconit.Service.Cost.ICostAllocateTransactionMgr
    {
    }
}

#endregion Extend Interface
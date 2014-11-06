using com.Sconit.Service.Ext.MasterData;
using System;
using System.Collections.Generic;
using com.Sconit.Entity.Cost;
using System.Collections;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface IInventoryBalanceMgr : IInventoryBalanceBaseMgr
    {
        #region Customized Methods

        IList<InventoryBalance> GetInventoryBalance(IList<string> itemList, IList<string> costgroupList, Int32 financeYear, Int32 financeMonth);

        void PostProcessGrossProfit(IList list, int financeYear, int financeMonth);

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.Cost
{
    public partial interface IInventoryBalanceMgrE : com.Sconit.Service.Cost.IInventoryBalanceMgr
    {
    }
}

#endregion Extend Interface
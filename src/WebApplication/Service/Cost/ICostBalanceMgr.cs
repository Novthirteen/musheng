using System;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Cost;
using System.Collections.Generic;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface ICostBalanceMgr : ICostBalanceBaseMgr
    {
        #region Customized Methods

        void ChangeCostBalance(string item, string costGroupCode, decimal qty, User user);

        void ChangeCostBalance(string item, string costGroupCode, string costElementCode, decimal amount, User user);

        void ChangeCostBalance(string item, string costGroupCode, string costElementCode, decimal amount, int financeYear, int financeMonth, User user);

        CostBalance GetCostBalance(string item, string costGroupCode, string costElementCode, int financeYear, int financeMonth);

        IList<CostBalance> GetCostBalance(string item, string costGroupCode, int financeYear, int financeMonth);

        IList<CostBalance> GetCostBalanceList(IList<string> itemList, IList<string> costgroupList, IList<string> itemcategoryList);

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.Cost
{
    public partial interface ICostBalanceMgrE : com.Sconit.Service.Cost.ICostBalanceMgr
    {
    }
}

#endregion Extend Interface
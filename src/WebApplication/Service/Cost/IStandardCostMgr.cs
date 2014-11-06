using System;
using com.Sconit.Entity.Cost;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface IStandardCostMgr : IStandardCostBaseMgr
    {
        #region Customized Methods

        StandardCost FindStandardCost(Item item, CostElement costElement, CostGroup costGroup);

        StandardCost FindStandardCost(string itemCode, string costElementCode, string costGroupCode);

        Decimal? SumStandardCost(Item item, CostElement costElement, CostGroup costGroup);

        Decimal? SumStandardCost(string itemCode, string costElementCode, string costGroupCode);       

        Decimal? SumStandardCost(Item item, CostGroup costGroup);

        Decimal? SumStandardCost(string itemCode, string costGroupCode);

        #endregion Customized Methods
    }
}
#region Extend Interface
namespace com.Sconit.Service.Ext.Cost
{
    public partial interface IStandardCostMgrE : com.Sconit.Service.Cost.IStandardCostMgr
    {

    }
}
#endregion
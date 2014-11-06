using System;
using System.Collections.Generic;
using com.Sconit.Entity.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface ICostDetailMgr : ICostDetailBaseMgr
    {
        #region Customized Methods

        decimal? CalculateItemUnitCost(string item, string costGroupCode, int financeYear, int financeMonth);

        decimal? CalculateItemUnitCost(string item, string costGroupCode, string costElement, int financeYear, int financeMonth);

        IList<CostDetail> GetCostDetail(string item, string costGroup, int financeYear, int financeMonth);

        #endregion Customized Methods
    }
}
#region Extend Interface
namespace com.Sconit.Service.Ext.Cost
{
    public partial interface ICostDetailMgrE : com.Sconit.Service.Cost.ICostDetailMgr
    {        
    }
}
#endregion
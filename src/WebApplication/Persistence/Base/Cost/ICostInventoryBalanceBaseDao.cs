using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Cost;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.Cost
{
    public interface ICostInventoryBalanceBaseDao
    {
        #region Method Created By CodeSmith

        void CreateCostInventoryBalance(CostInventoryBalance entity);

        CostInventoryBalance LoadCostInventoryBalance(Int32 id);
  
        IList<CostInventoryBalance> GetAllCostInventoryBalance();
  
        void UpdateCostInventoryBalance(CostInventoryBalance entity);
        
        void DeleteCostInventoryBalance(Int32 id);
    
        void DeleteCostInventoryBalance(CostInventoryBalance entity);
    
        void DeleteCostInventoryBalance(IList<Int32> pkList);
    
        void DeleteCostInventoryBalance(IList<CostInventoryBalance> entityList);    
        #endregion Method Created By CodeSmith
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Cost;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.Cost
{
    public interface ICostBalanceBaseDao
    {
        #region Method Created By CodeSmith

        void CreateCostBalance(CostBalance entity);

        CostBalance LoadCostBalance(Int32 id);
  
        IList<CostBalance> GetAllCostBalance();
  
        void UpdateCostBalance(CostBalance entity);
        
        void DeleteCostBalance(Int32 id);
    
        void DeleteCostBalance(CostBalance entity);
    
        void DeleteCostBalance(IList<Int32> pkList);
    
        void DeleteCostBalance(IList<CostBalance> entityList);    
        #endregion Method Created By CodeSmith
    }
}

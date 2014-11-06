using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Cost;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.Cost
{
    public interface ICostTransactionBaseDao
    {
        #region Method Created By CodeSmith

        void CreateCostTransaction(CostTransaction entity);

        CostTransaction LoadCostTransaction(Int32 id);
  
        IList<CostTransaction> GetAllCostTransaction();
  
        void UpdateCostTransaction(CostTransaction entity);
        
        void DeleteCostTransaction(Int32 id);
    
        void DeleteCostTransaction(CostTransaction entity);
    
        void DeleteCostTransaction(IList<Int32> pkList);
    
        void DeleteCostTransaction(IList<CostTransaction> entityList);    
        #endregion Method Created By CodeSmith
    }
}

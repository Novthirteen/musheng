using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Cost;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.Cost
{
    public interface ICostAllocateTransactionBaseDao
    {
        #region Method Created By CodeSmith

        void CreateCostAllocateTransaction(CostAllocateTransaction entity);

        CostAllocateTransaction LoadCostAllocateTransaction(Int32 id);
  
        IList<CostAllocateTransaction> GetAllCostAllocateTransaction();
  
        void UpdateCostAllocateTransaction(CostAllocateTransaction entity);
        
        void DeleteCostAllocateTransaction(Int32 id);
    
        void DeleteCostAllocateTransaction(CostAllocateTransaction entity);
    
        void DeleteCostAllocateTransaction(IList<Int32> pkList);
    
        void DeleteCostAllocateTransaction(IList<CostAllocateTransaction> entityList);    
        #endregion Method Created By CodeSmith
    }
}

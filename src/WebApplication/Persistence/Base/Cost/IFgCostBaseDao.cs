using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Cost;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.Cost
{
    public interface IFgCostBaseDao
    {
        #region Method Created By CodeSmith

        void CreateFgCost(FgCost entity);

        void BatchCreate(IList<FgCost> entities);

        FgCost LoadFgCost(Int32 id);
  
        IList<FgCost> GetAllFgCost();
  
        void UpdateFgCost(FgCost entity);
        
        void DeleteFgCost(Int32 id);
    
        void DeleteFgCost(FgCost entity);
    
        void DeleteFgCost(IList<Int32> pkList);
    
        void DeleteFgCost(IList<FgCost> entityList);    
        #endregion Method Created By CodeSmith
    }
}

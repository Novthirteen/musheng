using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Cost;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.Cost
{
    public interface ICostGroupBaseDao
    {
        #region Method Created By CodeSmith

        void CreateCostGroup(CostGroup entity);

        CostGroup LoadCostGroup(String code);
  
        IList<CostGroup> GetAllCostGroup();

        IList<CostGroup> GetAllCostGroup(bool includeInactive);
  
        void UpdateCostGroup(CostGroup entity);
        
        void DeleteCostGroup(String code);
    
        void DeleteCostGroup(CostGroup entity);
    
        void DeleteCostGroup(IList<String> pkList);
    
        void DeleteCostGroup(IList<CostGroup> entityList);    
        #endregion Method Created By CodeSmith
    }
}

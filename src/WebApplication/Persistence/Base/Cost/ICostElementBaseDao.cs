using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Cost;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.Cost
{
    public interface ICostElementBaseDao
    {
        #region Method Created By CodeSmith

        void CreateCostElement(CostElement entity);

        CostElement LoadCostElement(String code);
  
        IList<CostElement> GetAllCostElement();
  
        void UpdateCostElement(CostElement entity);
        
        void DeleteCostElement(String code);
    
        void DeleteCostElement(CostElement entity);
    
        void DeleteCostElement(IList<String> pkList);
    
        void DeleteCostElement(IList<CostElement> entityList);    
        #endregion Method Created By CodeSmith
    }
}

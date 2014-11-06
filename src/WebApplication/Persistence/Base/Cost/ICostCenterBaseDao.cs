using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Cost;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.Cost
{
    public interface ICostCenterBaseDao
    {
        #region Method Created By CodeSmith

        void CreateCostCenter(CostCenter entity);

        CostCenter LoadCostCenter(String code);
  
        IList<CostCenter> GetAllCostCenter();

        IList<CostCenter> GetAllCostCenter(bool includeInactive);
  
        void UpdateCostCenter(CostCenter entity);
        
        void DeleteCostCenter(String code);
    
        void DeleteCostCenter(CostCenter entity);
    
        void DeleteCostCenter(IList<String> pkList);
    
        void DeleteCostCenter(IList<CostCenter> entityList);    
        #endregion Method Created By CodeSmith
    }
}

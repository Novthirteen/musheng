using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface ICostAllocateMethodBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateCostAllocateMethod(CostAllocateMethod entity);

        CostAllocateMethod LoadCostAllocateMethod(Int32 id);

        IList<CostAllocateMethod> GetAllCostAllocateMethod();
    
        void UpdateCostAllocateMethod(CostAllocateMethod entity);

        void DeleteCostAllocateMethod(Int32 id);
    
        void DeleteCostAllocateMethod(CostAllocateMethod entity);
    
        void DeleteCostAllocateMethod(IList<Int32> pkList);
    
        void DeleteCostAllocateMethod(IList<CostAllocateMethod> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}

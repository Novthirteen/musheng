using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface IStandardCostBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateStandardCost(StandardCost entity);

        StandardCost LoadStandardCost(Int32 id);

        IList<StandardCost> GetAllStandardCost();
    
        void UpdateStandardCost(StandardCost entity);

        void DeleteStandardCost(Int32 id);
    
        void DeleteStandardCost(StandardCost entity);
    
        void DeleteStandardCost(IList<Int32> pkList);
    
        void DeleteStandardCost(IList<StandardCost> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}

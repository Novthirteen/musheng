using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface ICostDetailBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateCostDetail(CostDetail entity);

        CostDetail LoadCostDetail(Int32 id);

        IList<CostDetail> GetAllCostDetail();
    
        void UpdateCostDetail(CostDetail entity);

        void DeleteCostDetail(Int32 id);
    
        void DeleteCostDetail(CostDetail entity);
    
        void DeleteCostDetail(IList<Int32> pkList);
    
        void DeleteCostDetail(IList<CostDetail> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}

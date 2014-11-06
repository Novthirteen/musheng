using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface ISalesBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateSales(Sales entity);

        Sales LoadSales(Int32 id);

        IList<Sales> GetAllSales();
    
        void UpdateSales(Sales entity);

        void DeleteSales(Int32 id);
    
        void DeleteSales(Sales entity);
    
        void DeleteSales(IList<Int32> pkList);
    
        void DeleteSales(IList<Sales> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}

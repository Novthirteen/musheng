using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Cost;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.Cost
{
    public interface IPurchaseBaseDao
    {
        #region Method Created By CodeSmith

        void CreatePurchase(Purchase entity);

        Purchase LoadPurchase(Int32 id);
  
        IList<Purchase> GetAllPurchase();
  
        void UpdatePurchase(Purchase entity);
        
        void DeletePurchase(Int32 id);
    
        void DeletePurchase(Purchase entity);
    
        void DeletePurchase(IList<Int32> pkList);
    
        void DeletePurchase(IList<Purchase> entityList);    
        #endregion Method Created By CodeSmith
    }
}

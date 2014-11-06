using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.Cost;
//TODO: Add other using statements here.

namespace com.Sconit.Persistence.Cost
{
    public interface IInventoryBalanceBaseDao
    {
        #region Method Created By CodeSmith

        void CreateInventoryBalance(InventoryBalance entity);

        InventoryBalance LoadInventoryBalance(Int32 id);
  
        IList<InventoryBalance> GetAllInventoryBalance();
  
        void UpdateInventoryBalance(InventoryBalance entity);
        
        void DeleteInventoryBalance(Int32 id);
    
        void DeleteInventoryBalance(InventoryBalance entity);
    
        void DeleteInventoryBalance(IList<Int32> pkList);
    
        void DeleteInventoryBalance(IList<InventoryBalance> entityList);    
        #endregion Method Created By CodeSmith
    }
}

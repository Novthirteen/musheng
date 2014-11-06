using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.Cost;
using com.Sconit.Persistence.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost.Impl
{
    [Transactional]
    public class InventoryBalanceBaseMgr : SessionBase, IInventoryBalanceBaseMgr
    {
        public IInventoryBalanceDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateInventoryBalance(InventoryBalance entity)
        {
            entityDao.CreateInventoryBalance(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual InventoryBalance LoadInventoryBalance(Int32 id)
        {
            return entityDao.LoadInventoryBalance(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<InventoryBalance> GetAllInventoryBalance()
        {
            return entityDao.GetAllInventoryBalance();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateInventoryBalance(InventoryBalance entity)
        {
            entityDao.UpdateInventoryBalance(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteInventoryBalance(Int32 id)
        {
            entityDao.DeleteInventoryBalance(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteInventoryBalance(InventoryBalance entity)
        {
            entityDao.DeleteInventoryBalance(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteInventoryBalance(IList<Int32> pkList)
        {
            entityDao.DeleteInventoryBalance(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteInventoryBalance(IList<InventoryBalance> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteInventoryBalance(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

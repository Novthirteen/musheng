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
    public class PurchaseBaseMgr : SessionBase, IPurchaseBaseMgr
    {
        public IPurchaseDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreatePurchase(Purchase entity)
        {
            entityDao.CreatePurchase(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual Purchase LoadPurchase(Int32 id)
        {
            return entityDao.LoadPurchase(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<Purchase> GetAllPurchase()
        {
            return entityDao.GetAllPurchase();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdatePurchase(Purchase entity)
        {
            entityDao.UpdatePurchase(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeletePurchase(Int32 id)
        {
            entityDao.DeletePurchase(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeletePurchase(Purchase entity)
        {
            entityDao.DeletePurchase(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeletePurchase(IList<Int32> pkList)
        {
            entityDao.DeletePurchase(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeletePurchase(IList<Purchase> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeletePurchase(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

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
    public class CostAllocateTransactionBaseMgr : SessionBase, ICostAllocateTransactionBaseMgr
    {
        public ICostAllocateTransactionDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateCostAllocateTransaction(CostAllocateTransaction entity)
        {
            entityDao.CreateCostAllocateTransaction(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual CostAllocateTransaction LoadCostAllocateTransaction(Int32 id)
        {
            return entityDao.LoadCostAllocateTransaction(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<CostAllocateTransaction> GetAllCostAllocateTransaction()
        {
            return entityDao.GetAllCostAllocateTransaction();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateCostAllocateTransaction(CostAllocateTransaction entity)
        {
            entityDao.UpdateCostAllocateTransaction(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostAllocateTransaction(Int32 id)
        {
            entityDao.DeleteCostAllocateTransaction(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostAllocateTransaction(CostAllocateTransaction entity)
        {
            entityDao.DeleteCostAllocateTransaction(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostAllocateTransaction(IList<Int32> pkList)
        {
            entityDao.DeleteCostAllocateTransaction(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostAllocateTransaction(IList<CostAllocateTransaction> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteCostAllocateTransaction(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

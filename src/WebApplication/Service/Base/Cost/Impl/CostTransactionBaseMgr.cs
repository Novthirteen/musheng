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
    public class CostTransactionBaseMgr : SessionBase, ICostTransactionBaseMgr
    {
        public ICostTransactionDao entityDao { get; set; }

        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateCostTransaction(CostTransaction entity)
        {
            entityDao.CreateCostTransaction(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual CostTransaction LoadCostTransaction(Int32 id)
        {
            return entityDao.LoadCostTransaction(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<CostTransaction> GetAllCostTransaction()
        {
            return entityDao.GetAllCostTransaction();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateCostTransaction(CostTransaction entity)
        {
            entityDao.UpdateCostTransaction(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostTransaction(Int32 id)
        {
            entityDao.DeleteCostTransaction(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostTransaction(CostTransaction entity)
        {
            entityDao.DeleteCostTransaction(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostTransaction(IList<Int32> pkList)
        {
            entityDao.DeleteCostTransaction(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostTransaction(IList<CostTransaction> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteCostTransaction(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

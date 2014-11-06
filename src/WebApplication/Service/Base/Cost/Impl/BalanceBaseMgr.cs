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
    public class BalanceBaseMgr : SessionBase, IBalanceBaseMgr
    {
        public IBalanceDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateBalance(Balance entity)
        {
            entityDao.CreateBalance(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual Balance LoadBalance(Int32 id)
        {
            return entityDao.LoadBalance(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<Balance> GetAllBalance()
        {
            return entityDao.GetAllBalance();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateBalance(Balance entity)
        {
            entityDao.UpdateBalance(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteBalance(Int32 id)
        {
            entityDao.DeleteBalance(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteBalance(Balance entity)
        {
            entityDao.DeleteBalance(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteBalance(IList<Int32> pkList)
        {
            entityDao.DeleteBalance(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteBalance(IList<Balance> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteBalance(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

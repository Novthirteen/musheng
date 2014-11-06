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
    public class CostBalanceBaseMgr : SessionBase, ICostBalanceBaseMgr
    {
        public ICostBalanceDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateCostBalance(CostBalance entity)
        {
            entityDao.CreateCostBalance(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual CostBalance LoadCostBalance(Int32 id)
        {
            return entityDao.LoadCostBalance(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<CostBalance> GetAllCostBalance()
        {
            return entityDao.GetAllCostBalance();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateCostBalance(CostBalance entity)
        {
            entityDao.UpdateCostBalance(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostBalance(Int32 id)
        {
            entityDao.DeleteCostBalance(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostBalance(CostBalance entity)
        {
            entityDao.DeleteCostBalance(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostBalance(IList<Int32> pkList)
        {
            entityDao.DeleteCostBalance(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostBalance(IList<CostBalance> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteCostBalance(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

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
    public class CostInventoryBalanceBaseMgr : SessionBase, ICostInventoryBalanceBaseMgr
    {
        public ICostInventoryBalanceDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateCostInventoryBalance(CostInventoryBalance entity)
        {
            entityDao.CreateCostInventoryBalance(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual CostInventoryBalance LoadCostInventoryBalance(Int32 id)
        {
            return entityDao.LoadCostInventoryBalance(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<CostInventoryBalance> GetAllCostInventoryBalance()
        {
            return entityDao.GetAllCostInventoryBalance();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateCostInventoryBalance(CostInventoryBalance entity)
        {
            entityDao.UpdateCostInventoryBalance(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostInventoryBalance(Int32 id)
        {
            entityDao.DeleteCostInventoryBalance(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostInventoryBalance(CostInventoryBalance entity)
        {
            entityDao.DeleteCostInventoryBalance(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostInventoryBalance(IList<Int32> pkList)
        {
            entityDao.DeleteCostInventoryBalance(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostInventoryBalance(IList<CostInventoryBalance> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteCostInventoryBalance(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

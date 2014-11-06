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
    public class FgCostBaseMgr : SessionBase, IFgCostBaseMgr
    {
        public IFgCostDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateFgCost(FgCost entity)
        {
            entityDao.CreateFgCost(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void BatchCreate(IList<FgCost> entities)
        {
            entityDao.BatchCreate(entities);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual FgCost LoadFgCost(Int32 id)
        {
            return entityDao.LoadFgCost(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<FgCost> GetAllFgCost()
        {
            return entityDao.GetAllFgCost();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateFgCost(FgCost entity)
        {
            entityDao.UpdateFgCost(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteFgCost(Int32 id)
        {
            entityDao.DeleteFgCost(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteFgCost(FgCost entity)
        {
            entityDao.DeleteFgCost(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteFgCost(IList<Int32> pkList)
        {
            entityDao.DeleteFgCost(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteFgCost(IList<FgCost> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteFgCost(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

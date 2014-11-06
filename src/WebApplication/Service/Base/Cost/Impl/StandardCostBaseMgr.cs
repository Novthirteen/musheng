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
    public class StandardCostBaseMgr : SessionBase, IStandardCostBaseMgr
    {
        public IStandardCostDao entityDao { get; set; }

        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateStandardCost(StandardCost entity)
        {
            entityDao.CreateStandardCost(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual StandardCost LoadStandardCost(Int32 id)
        {
            return entityDao.LoadStandardCost(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<StandardCost> GetAllStandardCost()
        {
            return entityDao.GetAllStandardCost();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateStandardCost(StandardCost entity)
        {
            entityDao.UpdateStandardCost(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteStandardCost(Int32 id)
        {
            entityDao.DeleteStandardCost(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteStandardCost(StandardCost entity)
        {
            entityDao.DeleteStandardCost(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteStandardCost(IList<Int32> pkList)
        {
            entityDao.DeleteStandardCost(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteStandardCost(IList<StandardCost> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteStandardCost(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

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
    public class CostAllocateMethodBaseMgr : SessionBase, ICostAllocateMethodBaseMgr
    {
        public ICostAllocateMethodDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateCostAllocateMethod(CostAllocateMethod entity)
        {
            entityDao.CreateCostAllocateMethod(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual CostAllocateMethod LoadCostAllocateMethod(Int32 id)
        {
            return entityDao.LoadCostAllocateMethod(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<CostAllocateMethod> GetAllCostAllocateMethod()
        {
            return entityDao.GetAllCostAllocateMethod();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateCostAllocateMethod(CostAllocateMethod entity)
        {
            entityDao.UpdateCostAllocateMethod(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostAllocateMethod(Int32 id)
        {
            entityDao.DeleteCostAllocateMethod(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostAllocateMethod(CostAllocateMethod entity)
        {
            entityDao.DeleteCostAllocateMethod(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostAllocateMethod(IList<Int32> pkList)
        {
            entityDao.DeleteCostAllocateMethod(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostAllocateMethod(IList<CostAllocateMethod> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteCostAllocateMethod(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

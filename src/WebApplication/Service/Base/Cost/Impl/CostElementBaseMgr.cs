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
    public class CostElementBaseMgr : SessionBase, ICostElementBaseMgr
    {
        public ICostElementDao entityDao { get; set; }
       
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateCostElement(CostElement entity)
        {
            entityDao.CreateCostElement(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual CostElement LoadCostElement(String code)
        {
            return entityDao.LoadCostElement(code);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<CostElement> GetAllCostElement()
        {
            return entityDao.GetAllCostElement();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateCostElement(CostElement entity)
        {
            entityDao.UpdateCostElement(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostElement(String code)
        {
            entityDao.DeleteCostElement(code);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostElement(CostElement entity)
        {
            entityDao.DeleteCostElement(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostElement(IList<String> pkList)
        {
            entityDao.DeleteCostElement(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostElement(IList<CostElement> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteCostElement(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

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
    public class CostGroupBaseMgr : SessionBase, ICostGroupBaseMgr
    {
        public ICostGroupDao entityDao { get; set; }

        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateCostGroup(CostGroup entity)
        {
            entityDao.CreateCostGroup(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual CostGroup LoadCostGroup(String code)
        {
            return entityDao.LoadCostGroup(code);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<CostGroup> GetAllCostGroup()
        {
            return entityDao.GetAllCostGroup(false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<CostGroup> GetAllCostGroup(bool includeInactive)
        {
            return entityDao.GetAllCostGroup(includeInactive);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateCostGroup(CostGroup entity)
        {
            entityDao.UpdateCostGroup(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostGroup(String code)
        {
            entityDao.DeleteCostGroup(code);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostGroup(CostGroup entity)
        {
            entityDao.DeleteCostGroup(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostGroup(IList<String> pkList)
        {
            entityDao.DeleteCostGroup(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostGroup(IList<CostGroup> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteCostGroup(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

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
    public class CostCenterBaseMgr : SessionBase, ICostCenterBaseMgr
    {
        public ICostCenterDao entityDao {get; set;}

        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateCostCenter(CostCenter entity)
        {
            entityDao.CreateCostCenter(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual CostCenter LoadCostCenter(String code)
        {
            return entityDao.LoadCostCenter(code);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<CostCenter> GetAllCostCenter()
        {
            return entityDao.GetAllCostCenter(false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<CostCenter> GetAllCostCenter(bool includeInactive)
        {
            return entityDao.GetAllCostCenter(false);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateCostCenter(CostCenter entity)
        {
            entityDao.UpdateCostCenter(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostCenter(String code)
        {
            entityDao.DeleteCostCenter(code);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostCenter(CostCenter entity)
        {
            entityDao.DeleteCostCenter(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostCenter(IList<String> pkList)
        {
            entityDao.DeleteCostCenter(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostCenter(IList<CostCenter> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteCostCenter(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

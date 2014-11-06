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
    public class CostDetailBaseMgr : SessionBase, ICostDetailBaseMgr
    {
        public ICostDetailDao entityDao { get; set; }

        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateCostDetail(CostDetail entity)
        {
            entityDao.CreateCostDetail(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual CostDetail LoadCostDetail(Int32 id)
        {
            return entityDao.LoadCostDetail(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<CostDetail> GetAllCostDetail()
        {
            return entityDao.GetAllCostDetail();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateCostDetail(CostDetail entity)
        {
            entityDao.UpdateCostDetail(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostDetail(Int32 id)
        {
            entityDao.DeleteCostDetail(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostDetail(CostDetail entity)
        {
            entityDao.DeleteCostDetail(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostDetail(IList<Int32> pkList)
        {
            entityDao.DeleteCostDetail(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCostDetail(IList<CostDetail> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteCostDetail(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

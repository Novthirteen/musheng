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
    public class DiffBaseMgr : SessionBase, IDiffBaseMgr
    {
        public IDiffDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateDiff(Diff entity)
        {
            entityDao.CreateDiff(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual Diff LoadDiff(Int32 id)
        {
            return entityDao.LoadDiff(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<Diff> GetAllDiff()
        {
            return entityDao.GetAllDiff();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateDiff(Diff entity)
        {
            entityDao.UpdateDiff(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteDiff(Int32 id)
        {
            entityDao.DeleteDiff(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteDiff(Diff entity)
        {
            entityDao.DeleteDiff(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteDiff(IList<Int32> pkList)
        {
            entityDao.DeleteDiff(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteDiff(IList<Diff> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteDiff(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

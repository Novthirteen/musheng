using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.Customize;
using com.Sconit.Persistence.Customize;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Customize.Impl
{
    [Transactional]
    public class LedSortLevelBaseMgr : SessionBase, ILedSortLevelBaseMgr
    {
        public ILedSortLevelDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateLedSortLevel(LedSortLevel entity)
        {
            entityDao.CreateLedSortLevel(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual LedSortLevel LoadLedSortLevel(Int32 id)
        {
            return entityDao.LoadLedSortLevel(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<LedSortLevel> GetAllLedSortLevel()
        {
            return entityDao.GetAllLedSortLevel(false);
        }
    
        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<LedSortLevel> GetAllLedSortLevel(bool includeInactive)
        {
            return entityDao.GetAllLedSortLevel(includeInactive);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateLedSortLevel(LedSortLevel entity)
        {
            entityDao.UpdateLedSortLevel(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteLedSortLevel(Int32 id)
        {
            entityDao.DeleteLedSortLevel(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteLedSortLevel(LedSortLevel entity)
        {
            entityDao.DeleteLedSortLevel(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteLedSortLevel(IList<Int32> pkList)
        {
            entityDao.DeleteLedSortLevel(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteLedSortLevel(IList<LedSortLevel> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteLedSortLevel(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

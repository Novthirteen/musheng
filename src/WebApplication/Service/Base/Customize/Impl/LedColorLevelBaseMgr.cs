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
    public class LedColorLevelBaseMgr : SessionBase, ILedColorLevelBaseMgr
    {
        public ILedColorLevelDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateLedColorLevel(LedColorLevel entity)
        {
            entityDao.CreateLedColorLevel(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual LedColorLevel LoadLedColorLevel(Int32 id)
        {
            return entityDao.LoadLedColorLevel(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<LedColorLevel> GetAllLedColorLevel()
        {
            return entityDao.GetAllLedColorLevel(false);
        }
    
        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<LedColorLevel> GetAllLedColorLevel(bool includeInactive)
        {
            return entityDao.GetAllLedColorLevel(includeInactive);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateLedColorLevel(LedColorLevel entity)
        {
            entityDao.UpdateLedColorLevel(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteLedColorLevel(Int32 id)
        {
            entityDao.DeleteLedColorLevel(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteLedColorLevel(LedColorLevel entity)
        {
            entityDao.DeleteLedColorLevel(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteLedColorLevel(IList<Int32> pkList)
        {
            entityDao.DeleteLedColorLevel(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteLedColorLevel(IList<LedColorLevel> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteLedColorLevel(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

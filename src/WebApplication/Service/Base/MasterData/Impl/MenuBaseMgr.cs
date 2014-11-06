using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class MenuBaseMgr : SessionBase, IMenuBaseMgr
    {
        
        public IMenuDao entityDao { get; set; }
        

        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateMenu(Menu entity)
        {
            entityDao.CreateMenu(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual Menu LoadMenu(String id)
        {
            return entityDao.LoadMenu(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<Menu> GetAllMenu()
        {
            return entityDao.GetAllMenu(false);
        }
    
        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<Menu> GetAllMenu(bool includeInactive)
        {
            return entityDao.GetAllMenu(includeInactive);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateMenu(Menu entity)
        {
            entityDao.UpdateMenu(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteMenu(String id)
        {
            entityDao.DeleteMenu(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteMenu(Menu entity)
        {
            entityDao.DeleteMenu(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteMenu(IList<String> pkList)
        {
            entityDao.DeleteMenu(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteMenu(IList<Menu> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteMenu(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

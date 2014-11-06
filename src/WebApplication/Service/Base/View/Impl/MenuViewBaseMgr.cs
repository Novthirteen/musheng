using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.View;
using com.Sconit.Persistence.View;

//TODO: Add other using statements here.

namespace com.Sconit.Service.View.Impl
{
    [Transactional]
    public class MenuViewBaseMgr : SessionBase, IMenuViewBaseMgr
    {
        public IMenuViewDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateMenuView(MenuView entity)
        {
            entityDao.CreateMenuView(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual MenuView LoadMenuView(String Code)
        {
            return entityDao.LoadMenuView(Code);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<MenuView> GetAllMenuView()
        {
            return entityDao.GetAllMenuView(false);
        }
    
        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<MenuView> GetAllMenuView(bool includeInactive)
        {
            return entityDao.GetAllMenuView(includeInactive);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateMenuView(MenuView entity)
        {
            entityDao.UpdateMenuView(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteMenuView(String Code)
        {
            entityDao.DeleteMenuView(Code);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteMenuView(MenuView entity)
        {
            entityDao.DeleteMenuView(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteMenuView(IList<String> pkList)
        {
            entityDao.DeleteMenuView(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteMenuView(IList<MenuView> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteMenuView(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

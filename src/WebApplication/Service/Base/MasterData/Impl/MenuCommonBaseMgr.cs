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
    public class MenuCommonBaseMgr : SessionBase, IMenuCommonBaseMgr
    {
        public IMenuCommonDao entityDao { get; set; }

        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateMenuCommon(MenuCommon entity)
        {
            entityDao.CreateMenuCommon(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual MenuCommon LoadMenuCommon(Int32 id)
        {
            return entityDao.LoadMenuCommon(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<MenuCommon> GetAllMenuCommon()
        {
            return entityDao.GetAllMenuCommon(false);
        }
    
        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<MenuCommon> GetAllMenuCommon(bool includeInactive)
        {
            return entityDao.GetAllMenuCommon(includeInactive);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateMenuCommon(MenuCommon entity)
        {
            entityDao.UpdateMenuCommon(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteMenuCommon(Int32 id)
        {
            entityDao.DeleteMenuCommon(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteMenuCommon(MenuCommon entity)
        {
            entityDao.DeleteMenuCommon(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteMenuCommon(IList<Int32> pkList)
        {
            entityDao.DeleteMenuCommon(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteMenuCommon(IList<MenuCommon> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteMenuCommon(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

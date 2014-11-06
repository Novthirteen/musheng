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
    public class MenuCompanyBaseMgr : SessionBase, IMenuCompanyBaseMgr
    {
        
        public IMenuCompanyDao entityDao { get; set; }
       

        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateMenuCompany(MenuCompany entity)
        {
            entityDao.CreateMenuCompany(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual MenuCompany LoadMenuCompany(Int32 id)
        {
            return entityDao.LoadMenuCompany(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<MenuCompany> GetAllMenuCompany()
        {
            return entityDao.GetAllMenuCompany(false);
        }
    
        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<MenuCompany> GetAllMenuCompany(bool includeInactive)
        {
            return entityDao.GetAllMenuCompany(includeInactive);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateMenuCompany(MenuCompany entity)
        {
            entityDao.UpdateMenuCompany(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteMenuCompany(Int32 id)
        {
            entityDao.DeleteMenuCompany(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteMenuCompany(MenuCompany entity)
        {
            entityDao.DeleteMenuCompany(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteMenuCompany(IList<Int32> pkList)
        {
            entityDao.DeleteMenuCompany(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteMenuCompany(IList<MenuCompany> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteMenuCompany(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

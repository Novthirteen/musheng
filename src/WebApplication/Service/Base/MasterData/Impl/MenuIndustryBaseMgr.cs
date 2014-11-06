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
    public class MenuIndustryBaseMgr : SessionBase, IMenuIndustryBaseMgr
    {
        
        public IMenuIndustryDao entityDao { get; set; }

        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateMenuIndustry(MenuIndustry entity)
        {
            entityDao.CreateMenuIndustry(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual MenuIndustry LoadMenuIndustry(Int32 id)
        {
            return entityDao.LoadMenuIndustry(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<MenuIndustry> GetAllMenuIndustry()
        {
            return entityDao.GetAllMenuIndustry(false);
        }
    
        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<MenuIndustry> GetAllMenuIndustry(bool includeInactive)
        {
            return entityDao.GetAllMenuIndustry(includeInactive);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateMenuIndustry(MenuIndustry entity)
        {
            entityDao.UpdateMenuIndustry(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteMenuIndustry(Int32 id)
        {
            entityDao.DeleteMenuIndustry(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteMenuIndustry(MenuIndustry entity)
        {
            entityDao.DeleteMenuIndustry(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteMenuIndustry(IList<Int32> pkList)
        {
            entityDao.DeleteMenuIndustry(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteMenuIndustry(IList<MenuIndustry> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteMenuIndustry(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

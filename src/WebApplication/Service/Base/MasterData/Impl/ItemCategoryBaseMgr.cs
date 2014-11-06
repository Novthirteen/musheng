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
    public class ItemCategoryBaseMgr : SessionBase, IItemCategoryBaseMgr
    {
        public IItemCategoryDao entityDao {get; set;}
      
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateItemCategory(ItemCategory entity)
        {
            entityDao.CreateItemCategory(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual ItemCategory LoadItemCategory(String code)
        {
            return entityDao.LoadItemCategory(code);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<ItemCategory> GetAllItemCategory()
        {
            return entityDao.GetAllItemCategory();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateItemCategory(ItemCategory entity)
        {
            entityDao.UpdateItemCategory(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteItemCategory(String code)
        {
            entityDao.DeleteItemCategory(code);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteItemCategory(ItemCategory entity)
        {
            entityDao.DeleteItemCategory(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteItemCategory(IList<String> pkList)
        {
            entityDao.DeleteItemCategory(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteItemCategory(IList<ItemCategory> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteItemCategory(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

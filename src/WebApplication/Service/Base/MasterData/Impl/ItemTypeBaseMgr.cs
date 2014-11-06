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
    public class ItemTypeBaseMgr : SessionBase, IItemTypeBaseMgr
    {
        public IItemTypeDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateItemType(ItemType entity)
        {
            entityDao.CreateItemType(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual ItemType LoadItemType(String code)
        {
            return entityDao.LoadItemType(code);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<ItemType> GetAllItemType()
        {
            return entityDao.GetAllItemType();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateItemType(ItemType entity)
        {
            entityDao.UpdateItemType(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteItemType(String code)
        {
            entityDao.DeleteItemType(code);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteItemType(ItemType entity)
        {
            entityDao.DeleteItemType(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteItemType(IList<String> pkList)
        {
            entityDao.DeleteItemType(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteItemType(IList<ItemType> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteItemType(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

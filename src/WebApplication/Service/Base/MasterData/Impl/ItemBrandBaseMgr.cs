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
    public class ItemBrandBaseMgr : SessionBase, IItemBrandBaseMgr
    {
        public IItemBrandDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateItemBrand(ItemBrand entity)
        {
            entityDao.CreateItemBrand(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual ItemBrand LoadItemBrand(String code)
        {
            return entityDao.LoadItemBrand(code);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<ItemBrand> GetAllItemBrand()
        {
            return entityDao.GetAllItemBrand();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateItemBrand(ItemBrand entity)
        {
            entityDao.UpdateItemBrand(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteItemBrand(String code)
        {
            entityDao.DeleteItemBrand(code);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteItemBrand(ItemBrand entity)
        {
            entityDao.DeleteItemBrand(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteItemBrand(IList<String> pkList)
        {
            entityDao.DeleteItemBrand(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteItemBrand(IList<ItemBrand> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteItemBrand(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

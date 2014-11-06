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
    public class ItemDiscontinueBaseMgr : SessionBase, IItemDiscontinueBaseMgr
    {
        public IItemDiscontinueDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateItemDiscontinue(ItemDiscontinue entity)
        {
            entityDao.CreateItemDiscontinue(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual ItemDiscontinue LoadItemDiscontinue(Int32 id)
        {
            return entityDao.LoadItemDiscontinue(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<ItemDiscontinue> GetAllItemDiscontinue()
        {
            return entityDao.GetAllItemDiscontinue();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateItemDiscontinue(ItemDiscontinue entity)
        {
            entityDao.UpdateItemDiscontinue(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteItemDiscontinue(Int32 id)
        {
            entityDao.DeleteItemDiscontinue(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteItemDiscontinue(ItemDiscontinue entity)
        {
            entityDao.DeleteItemDiscontinue(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteItemDiscontinue(IList<Int32> pkList)
        {
            entityDao.DeleteItemDiscontinue(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteItemDiscontinue(IList<ItemDiscontinue> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteItemDiscontinue(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

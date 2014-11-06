using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.Cost;
using com.Sconit.Persistence.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost.Impl
{
    [Transactional]
    public class ConsumeBaseMgr : SessionBase, IConsumeBaseMgr
    {
        public IConsumeDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateConsume(Consume entity)
        {
            entityDao.CreateConsume(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual Consume LoadConsume(Int32 id)
        {
            return entityDao.LoadConsume(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<Consume> GetAllConsume()
        {
            return entityDao.GetAllConsume();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateConsume(Consume entity)
        {
            entityDao.UpdateConsume(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteConsume(Int32 id)
        {
            entityDao.DeleteConsume(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteConsume(Consume entity)
        {
            entityDao.DeleteConsume(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteConsume(IList<Int32> pkList)
        {
            entityDao.DeleteConsume(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteConsume(IList<Consume> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteConsume(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

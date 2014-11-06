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
    public class RawIOBBaseMgr : SessionBase, IRawIOBBaseMgr
    {
        public IRawIOBDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateRawIOB(RawIOB entity)
        {
            entityDao.CreateRawIOB(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual RawIOB LoadRawIOB(Int32 id)
        {
            return entityDao.LoadRawIOB(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<RawIOB> GetAllRawIOB()
        {
            return entityDao.GetAllRawIOB();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateRawIOB(RawIOB entity)
        {
            entityDao.UpdateRawIOB(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteRawIOB(Int32 id)
        {
            entityDao.DeleteRawIOB(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteRawIOB(RawIOB entity)
        {
            entityDao.DeleteRawIOB(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteRawIOB(IList<Int32> pkList)
        {
            entityDao.DeleteRawIOB(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteRawIOB(IList<RawIOB> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteRawIOB(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

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
    public class CbomBaseMgr : SessionBase, ICbomBaseMgr
    {
        public ICbomDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateCbom(Cbom entity)
        {
            entityDao.CreateCbom(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual Cbom LoadCbom(Int32 id)
        {
            return entityDao.LoadCbom(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<Cbom> GetAllCbom()
        {
            return entityDao.GetAllCbom();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateCbom(Cbom entity)
        {
            entityDao.UpdateCbom(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCbom(Int32 id)
        {
            entityDao.DeleteCbom(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCbom(Cbom entity)
        {
            entityDao.DeleteCbom(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCbom(IList<Int32> pkList)
        {
            entityDao.DeleteCbom(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCbom(IList<Cbom> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteCbom(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

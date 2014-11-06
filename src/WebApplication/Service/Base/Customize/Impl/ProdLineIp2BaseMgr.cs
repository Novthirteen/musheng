using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.Customize;
using com.Sconit.Persistence.Customize;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Customize.Impl
{
    [Transactional]
    public class ProdLineIp2BaseMgr : SessionBase, IProdLineIp2BaseMgr
    {
        public IProdLineIp2Dao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateProdLineIp2(ProdLineIp2 entity)
        {
            entityDao.CreateProdLineIp2(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual ProdLineIp2 LoadProdLineIp2(Int32 id)
        {
            return entityDao.LoadProdLineIp2(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<ProdLineIp2> GetAllProdLineIp2()
        {
            return entityDao.GetAllProdLineIp2();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateProdLineIp2(ProdLineIp2 entity)
        {
            entityDao.UpdateProdLineIp2(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteProdLineIp2(Int32 id)
        {
            entityDao.DeleteProdLineIp2(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteProdLineIp2(ProdLineIp2 entity)
        {
            entityDao.DeleteProdLineIp2(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteProdLineIp2(IList<Int32> pkList)
        {
            entityDao.DeleteProdLineIp2(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteProdLineIp2(IList<ProdLineIp2> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteProdLineIp2(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

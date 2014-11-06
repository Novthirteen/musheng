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
    public class ProdutLineFeedSeqenceBaseMgr : SessionBase, IProdutLineFeedSeqenceBaseMgr
    {
        public IProdutLineFeedSeqenceDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateProdutLineFeedSeqence(ProdutLineFeedSeqence entity)
        {
            entityDao.CreateProdutLineFeedSeqence(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual ProdutLineFeedSeqence LoadProdutLineFeedSeqence(Int32 id)
        {
            return entityDao.LoadProdutLineFeedSeqence(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<ProdutLineFeedSeqence> GetAllProdutLineFeedSeqence()
        {
            return entityDao.GetAllProdutLineFeedSeqence(false);
        }
    
        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<ProdutLineFeedSeqence> GetAllProdutLineFeedSeqence(bool includeInactive)
        {
            return entityDao.GetAllProdutLineFeedSeqence(includeInactive);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateProdutLineFeedSeqence(ProdutLineFeedSeqence entity)
        {
            entityDao.UpdateProdutLineFeedSeqence(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteProdutLineFeedSeqence(Int32 id)
        {
            entityDao.DeleteProdutLineFeedSeqence(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteProdutLineFeedSeqence(ProdutLineFeedSeqence entity)
        {
            entityDao.DeleteProdutLineFeedSeqence(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteProdutLineFeedSeqence(IList<Int32> pkList)
        {
            entityDao.DeleteProdutLineFeedSeqence(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteProdutLineFeedSeqence(IList<ProdutLineFeedSeqence> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteProdutLineFeedSeqence(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

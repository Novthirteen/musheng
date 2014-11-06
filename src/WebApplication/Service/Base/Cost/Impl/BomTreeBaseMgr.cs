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
    public class BomTreeBaseMgr : SessionBase, IBomTreeBaseMgr
    {
        public IBomTreeDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateBomTree(BomTree entity)
        {
            entityDao.CreateBomTree(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual BomTree LoadBomTree()
        {
            return entityDao.LoadBomTree();
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<BomTree> GetAllBomTree()
        {
            return entityDao.GetAllBomTree();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateBomTree(BomTree entity)
        {
            entityDao.UpdateBomTree(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteBomTree()
        {
            entityDao.DeleteBomTree();
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteBomTree(BomTree entity)
        {
            entityDao.DeleteBomTree(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteBomTree(IList<BomTree> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteBomTree(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

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
    public class SalesBaseMgr : SessionBase, ISalesBaseMgr
    {
        public ISalesDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateSales(Sales entity)
        {
            entityDao.CreateSales(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual Sales LoadSales(Int32 id)
        {
            return entityDao.LoadSales(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<Sales> GetAllSales()
        {
            return entityDao.GetAllSales();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateSales(Sales entity)
        {
            entityDao.UpdateSales(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteSales(Int32 id)
        {
            entityDao.DeleteSales(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteSales(Sales entity)
        {
            entityDao.DeleteSales(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteSales(IList<Int32> pkList)
        {
            entityDao.DeleteSales(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteSales(IList<Sales> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteSales(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

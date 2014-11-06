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
    public class TaxRateBaseMgr : SessionBase, ITaxRateBaseMgr
    {
        public ITaxRateDao entityDao { get; set; }
        
        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateTaxRate(TaxRate entity)
        {
            entityDao.CreateTaxRate(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual TaxRate LoadTaxRate(String code)
        {
            return entityDao.LoadTaxRate(code);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<TaxRate> GetAllTaxRate()
        {
            return entityDao.GetAllTaxRate();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateTaxRate(TaxRate entity)
        {
            entityDao.UpdateTaxRate(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteTaxRate(String code)
        {
            entityDao.DeleteTaxRate(code);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteTaxRate(TaxRate entity)
        {
            entityDao.DeleteTaxRate(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteTaxRate(IList<String> pkList)
        {
            entityDao.DeleteTaxRate(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteTaxRate(IList<TaxRate> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteTaxRate(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

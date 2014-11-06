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
    public class CurrencyExchangeBaseMgr : SessionBase, ICurrencyExchangeBaseMgr
    {
        public ICurrencyExchangeDao entityDao { get; set; }

        #region Method Created By CodeSmith

        [Transaction(TransactionMode.Requires)]
        public virtual void CreateCurrencyExchange(CurrencyExchange entity)
        {
            entityDao.CreateCurrencyExchange(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual CurrencyExchange LoadCurrencyExchange(Int32 id)
        {
            return entityDao.LoadCurrencyExchange(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public virtual IList<CurrencyExchange> GetAllCurrencyExchange()
        {
            return entityDao.GetAllCurrencyExchange();
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateCurrencyExchange(CurrencyExchange entity)
        {
            entityDao.UpdateCurrencyExchange(entity);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCurrencyExchange(Int32 id)
        {
            entityDao.DeleteCurrencyExchange(id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCurrencyExchange(CurrencyExchange entity)
        {
            entityDao.DeleteCurrencyExchange(entity);
        }
    
        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCurrencyExchange(IList<Int32> pkList)
        {
            entityDao.DeleteCurrencyExchange(pkList);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteCurrencyExchange(IList<CurrencyExchange> entityList)
        {
            if ((entityList == null) || (entityList.Count == 0))
            {
                return;
            }
            
            entityDao.DeleteCurrencyExchange(entityList);
        }   
        #endregion Method Created By CodeSmith
    }
}

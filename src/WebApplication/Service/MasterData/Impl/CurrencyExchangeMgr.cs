using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity;
using NHibernate.Expression;
using com.Sconit.Entity.Exception;
using com.Sconit.Service.Ext.Criteria;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class CurrencyExchangeMgr : CurrencyExchangeBaseMgr, ICurrencyExchangeMgr
    {
        public IEntityPreferenceMgrE entityPreferenceMgr { get; set; }
        public ICurrencyMgrE currencyMgr { get; set; }
        public ICriteriaMgrE criteriaMgr { get; set; }

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public decimal FindLastestExchangeRate(Currency currency, Currency baseCurrency, DateTime? effectiveDate)
        {
            return FindLastestExchangeRate(currency.Code, baseCurrency.Code, effectiveDate);
        }

        [Transaction(TransactionMode.Unspecified)]
        public decimal FindLastestExchangeRate(Currency currency, Currency baseCurrency)
        {
            return FindLastestExchangeRate(currency.Code, baseCurrency.Code, DateTime.Now);
        }
        [Transaction(TransactionMode.Unspecified)]
        public IList GetCurrencyExchange(string baseCurrency, string exchangeCurrency)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(CurrencyExchange));
            if (baseCurrency != null)
                criteria.Add(Expression.Eq("BaseCurrency", baseCurrency));
            if (exchangeCurrency != null)
                criteria.Add(Expression.Eq("ExchangeCurrency", exchangeCurrency));
            return criteriaMgr.FindAll(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public decimal FindLastestExchangeRate(Currency currency)
        {
            return FindLastestExchangeRate(currency.Code);
        }

        [Transaction(TransactionMode.Unspecified)]
        public decimal FindLastestExchangeRate(string currencyCode, string baseCurrencyCode, DateTime? effectiveDate)
        {
            DateTime effDate = effectiveDate.HasValue ? effectiveDate.Value : DateTime.Now;
            DetachedCriteria criteria = DetachedCriteria.For<CurrencyExchange>();
            criteria.Add(Expression.Eq("ExchangeCurrency", currencyCode));
            criteria.Add(Expression.Eq("BaseCurrency", baseCurrencyCode));
            criteria.Add(Expression.Ge("StartDate", effDate));
            criteria.Add(Expression.Lt("EndDate", effDate));
            criteria.AddOrder(Order.Desc("StartDate")); //����ʼ���ڵ���ȡ���µ�һ��

            IList<CurrencyExchange> list = this.criteriaMgr.FindAll<CurrencyExchange>(criteria, 1, 1);
            if (list != null && list.Count > 0)
            {
                CurrencyExchange currencyExchange = list[0];
                return currencyExchange.BaseQty / currencyExchange.ExchangeQty;
            }
            else
            {
                throw new BusinessErrorException("û���ҵ����ʣ���λ��{0}�����{1}����Ч����{2}", baseCurrencyCode, currencyCode, effDate.ToLongDateString());
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public decimal FindLastestExchangeRate(string currencyCode, string baseCurrencyCode)
        {
            return FindLastestExchangeRate(currencyCode, baseCurrencyCode, DateTime.Now);
        }

        [Transaction(TransactionMode.Unspecified)]
        public decimal FindLastestExchangeRate(string currencyCode)
        {
            #region ȡ��ҵ����
            EntityPreference baseCurrencyPreference = entityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_COST_BASE_CURRENCY);
            Currency baseCurrency = currencyMgr.CheckAndLoadCurrency(baseCurrencyPreference.Code);
            #endregion

            return FindLastestExchangeRate(currencyCode, baseCurrency.Code, DateTime.Now);
        }

        #endregion Customized Methods
    }
}

#region Extend Class


namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class CurrencyExchangeMgrE : com.Sconit.Service.MasterData.Impl.CurrencyExchangeMgr, ICurrencyExchangeMgrE
    {

    }
}
#endregion
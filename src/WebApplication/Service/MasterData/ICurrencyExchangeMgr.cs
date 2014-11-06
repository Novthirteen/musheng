using com.Sconit.Service.Ext.MasterData;
using System.Collections;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.MasterData;
using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface ICurrencyExchangeMgr : ICurrencyExchangeBaseMgr
    {
        #region Customized Methods

        decimal FindLastestExchangeRate(Currency currency, Currency baseCurrency, DateTime? effectiveDate);

        decimal FindLastestExchangeRate(Currency currency, Currency baseCurrency);

        decimal FindLastestExchangeRate(Currency currency);

        decimal FindLastestExchangeRate(string currencyCode, string baseCurrencyCode, DateTime? effectiveDate);

        decimal FindLastestExchangeRate(string currencyCode, string baseCurrencyCode);

        decimal FindLastestExchangeRate(string currencyCode);

        IList GetCurrencyExchange(string baseCurrency, string exchangeCurrency);


        #endregion Customized Methods
    }
}
#region Extend Interface
namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface ICurrencyExchangeMgrE : com.Sconit.Service.MasterData.ICurrencyExchangeMgr
    {

    }
}
#endregion
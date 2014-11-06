using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface ICurrencyExchangeBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateCurrencyExchange(CurrencyExchange entity);

        CurrencyExchange LoadCurrencyExchange(Int32 id);

        IList<CurrencyExchange> GetAllCurrencyExchange();
    
        void UpdateCurrencyExchange(CurrencyExchange entity);

        void DeleteCurrencyExchange(Int32 id);
    
        void DeleteCurrencyExchange(CurrencyExchange entity);
    
        void DeleteCurrencyExchange(IList<Int32> pkList);
    
        void DeleteCurrencyExchange(IList<CurrencyExchange> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}

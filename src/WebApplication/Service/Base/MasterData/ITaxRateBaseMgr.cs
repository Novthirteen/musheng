using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface ITaxRateBaseMgr
    {
        #region Method Created By CodeSmith

        void CreateTaxRate(TaxRate entity);

        TaxRate LoadTaxRate(String code);

        IList<TaxRate> GetAllTaxRate();
    
        void UpdateTaxRate(TaxRate entity);

        void DeleteTaxRate(String code);
    
        void DeleteTaxRate(TaxRate entity);
    
        void DeleteTaxRate(IList<String> pkList);
    
        void DeleteTaxRate(IList<TaxRate> entityList);    
    
        #endregion Method Created By CodeSmith
    }
}

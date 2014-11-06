using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Exception;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class TaxRateMgr : TaxRateBaseMgr, ITaxRateMgr
    {
        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public TaxRate CheckAndLoadTaxRate(string taxCode)
        {
            TaxRate taxRate = this.LoadTaxRate(taxCode);
            if (taxRate == null)
            {
                throw new BusinessErrorException("MasterData.TaxRate.Error.TaxCodeNotExist", taxCode);
            }

            return taxRate;
        }

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class TaxRateMgrE : com.Sconit.Service.MasterData.Impl.TaxRateMgr, ITaxRateMgrE
    {
    }
}

#endregion Extend Class
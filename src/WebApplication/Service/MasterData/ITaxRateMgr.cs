using System;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface ITaxRateMgr : ITaxRateBaseMgr
    {
        #region Customized Methods

        TaxRate CheckAndLoadTaxRate(string taxCode);

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface ITaxRateMgrE : com.Sconit.Service.MasterData.ITaxRateMgr
    {
    }
}

#endregion Extend Interface
using com.Sconit.Service.Ext.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IBillAddressMgr : IBillAddressBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.
        IList<BillAddress> GetBillAddress(bool isSupplier);

        #endregion Customized Methods
    }
}




#region Extend Interface



namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IBillAddressMgrE : com.Sconit.Service.MasterData.IBillAddressMgr
    {
        
    }
}

#endregion

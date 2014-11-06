using com.Sconit.Service.Ext.MasterData;
using System;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IReceiptInProcessLocationMgr : IReceiptInProcessLocationBaseMgr
    {
        #region Customized Methods

        IList<ReceiptInProcessLocation> GetReceiptInProcessLocation(string ipNo, string receiptNo);

        #endregion Customized Methods
    }
}





#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IReceiptInProcessLocationMgrE : com.Sconit.Service.MasterData.IReceiptInProcessLocationMgr
    {
        
    }
}

#endregion

using com.Sconit.Service.Ext.MasterData;
using System;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IHuOddMgr : IHuOddBaseMgr
    {
        #region Customized Methods

        HuOdd CreateHuOdd(ReceiptDetail receiptDetail, LocationLotDetail locationLotDetail, User user);

        IList<HuOdd> GetHuOdd(Item item, decimal unitCount, Uom uom, Location locFrom, Location locTo, string status);

        #endregion Customized Methods
    }
}





#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IHuOddMgrE : com.Sconit.Service.MasterData.IHuOddMgr
    {
        
    }
}

#endregion

using com.Sconit.Service.Ext.MasterData;
using System;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IPickListDetailMgr : IPickListDetailBaseMgr
    {
        #region Customized Methods

        PickListDetail LoadPickListDetail(int pickListDetailId, bool includePickListResult);

        IList<PickListDetail> GetPickListDetail(string locationCode, string itemCode, decimal? unitCount, string uomCode, string[] status);

        IList<PickListDetail> GetPickedPickListDetail(int orderLocationTransactionId);

        #endregion Customized Methods
    }
}



#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IPickListDetailMgrE : com.Sconit.Service.MasterData.IPickListDetailMgr
    {
        
    }
}

#endregion

#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IPickListDetailMgrE : com.Sconit.Service.MasterData.IPickListDetailMgr
    {
        
    }
}

#endregion

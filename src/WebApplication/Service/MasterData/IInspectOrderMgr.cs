using com.Sconit.Service.Ext.MasterData;
using System;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IInspectOrderMgr : IInspectOrderBaseMgr
    {
        #region Customized Methods

        InspectOrder CheckAndLoadInspectOrder(string inspectOrderNo);

        InspectOrder CheckAndLoadInspectOrder(string inspectOrderNo, bool includeDetail);

        InspectOrder CreateInspectOrder(IList<LocationLotDetail> locationLotDetailList, string inspectLocation, string rejectLocation, User user);

        InspectOrder CreateInspectOrder(IList<LocationLotDetail> locationLotDetailList, string inspectLocation, string rejectLocation, User user, string ipNo, string receiptNo, bool isSeperated);

        InspectOrder CreateInspectOrder(string locationCode, IDictionary<string, decimal> inspectItemList, User user);

        InspectOrder CreateInspectOrder(Location location, IDictionary<string, decimal> inspectItemList, User user);

        InspectOrder CreateInspectOrder(string locationCode, IDictionary<string, decimal> inspectItemList, string inspectLocation, string rejectLocation, User user);

        InspectOrder CreateInspectOrder(Location location, IDictionary<string, decimal> inspectItemList, string inspectLocation, string rejectLocation, User user);

        InspectOrder CreateFgInspectOrder(string locationCode, IDictionary<string, decimal> itemFgQtyDic, User user);

        InspectOrder CreateFgInspectOrder(string locationCode, IDictionary<string, decimal> itemFgQtyDic, string inspectLocation, string rejectLocation, User user);

        InspectOrder CreateInspectOrder(string locationCode, IList<InspectItem> inspectItemList, User user);

        InspectOrder CreateInspectOrder(string locationCode, IList<InspectItem> inspectItemList, string inspectLocation, string rejectLocation, User user);
        
        void ProcessInspectOrder(IList<InspectOrderDetail> inspectOrderDetailList, User user);

        void ProcessInspectOrder(IList<InspectOrderDetail> inspectOrderDetailList, string userCode);

        InspectOrder LoadInspectOrder(String inspectNo, bool includeDetail);
        
        InspectOrder LoadInspectOrder(String inspectNo, bool includeDetail, bool isSort);

        IList<InspectOrder> GetInspectOrder(string ipNo, string receiptNo);

        IList<InspectOrder> GetInspectOrder(string userCode, int firstRow, int maxRows);

        #endregion Customized Methods
    }
}



#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IInspectOrderMgrE : com.Sconit.Service.MasterData.IInspectOrderMgr
    {
        
    }
}

#endregion

#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IInspectOrderMgrE : com.Sconit.Service.MasterData.IInspectOrderMgr
    {
        
    }
}

#endregion

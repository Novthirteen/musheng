using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Procurement;
using System.Collections.Generic;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IPlannedBillMgr : IPlannedBillBaseMgr
    {
        #region Customized Methods

        IList<PlannedBill> GetUnSettledPlannedBill(OrderHead orderHead);

        IList<PlannedBill> GetUnSettledPlannedBill(string orderNo);

        //PlannedBill CreatePlannedBill(ReceiptDetail receiptDetail, User user);

        PlannedBill CreatePlannedBill(ReceiptDetail receiptDetail, User user);

        #endregion Customized Methods
    }
}



#region Extend Interface






namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IPlannedBillMgrE : com.Sconit.Service.MasterData.IPlannedBillMgr
    {
        
    }
}

#endregion

#region Extend Interface






namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IPlannedBillMgrE : com.Sconit.Service.MasterData.IPlannedBillMgr
    {
        
    }
}

#endregion

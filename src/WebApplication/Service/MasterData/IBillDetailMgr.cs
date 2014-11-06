using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IBillDetailMgr : IBillDetailBaseMgr
    {
        #region Customized Methods

        IList<BillDetail> GetBillDetail(string billNo);

        IList<BillDetail> GetBillDetailOrderByItem(string billNo);

        BillDetail TransferAtingBill2BillDetail(ActingBill actingBill);

        IList<BillDetail> GroupBillDetail(IList<BillDetail> billDetails);

        IList<BillDetail> GetBillDetailOrderByItem(string billNo, bool isGroup);

        #endregion Customized Methods
    }
}





#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IBillDetailMgrE : com.Sconit.Service.MasterData.IBillDetailMgr
    {
        
    }
}

#endregion

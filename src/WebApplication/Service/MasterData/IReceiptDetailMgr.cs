using com.Sconit.Service.Ext.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Procurement;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IReceiptDetailMgr : IReceiptDetailBaseMgr
    {
        #region Customized Methods

        IList<ReceiptDetail> CreateReceiptDetail(Receipt receipt, OrderLocationTransaction inOrderLocationTransaction, IList<Hu> huList);

        IList<ReceiptDetail> SummarizeReceiptDetails(IList<ReceiptDetail> receiptDetailList);

        #endregion Customized Methods
    }
}





#region Extend Interface






namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IReceiptDetailMgrE : com.Sconit.Service.MasterData.IReceiptDetailMgr
    {
        
    }
}

#endregion

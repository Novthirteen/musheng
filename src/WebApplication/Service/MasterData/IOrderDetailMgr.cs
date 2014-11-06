using com.Sconit.Service.Ext.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Distribution;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IOrderDetailMgr : IOrderDetailBaseMgr
    {
        #region Customized Methods

        OrderDetail LoadOrderDetail(int id, bool includeLocTrans);

        IList<OrderDetail> GenerateOrderDetail(OrderHead orderHead, FlowDetail flowDetail);

        IList<OrderDetail> GenerateOrderDetail(OrderHead orderHead, FlowDetail flowDetail, bool isReferencedFlow);

        IList<OrderDetail> GenerateOrderDetail(OrderHead orderHead, FlowDetail flowDetail, bool isReferencedFlow, bool isStartKit);

        void GenerateOrderDetailSubsidiary(OrderDetail orderDetail);

        IList<OrderDetail> GetOrderDetail(string orderNo);

        IList<OrderDetail> GetOrderDetail(OrderHead orderHead);

        bool CheckOrderDet(OrderDetail orderDetail);

        bool CheckOrderDet(string item, string loc, string checkOrderDetOption);

        OrderDetail TransferFlowDetail2OrderDetail(FlowDetail flowDetail);

        void RecordOrderShipQty(int orderLocationTransactionId, InProcessLocationDetail inProcessLocationDetail, bool checkExcceed);

        void RecordOrderShipQty(OrderLocationTransaction orderLocationTransaction, InProcessLocationDetail inProcessLocationDetail, bool checkExcceed);

        void DeleteOrderDetail(IList<OrderDetail> orderDetails);
        #endregion Customized Methods
    }
}





#region Extend Interface






namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IOrderDetailMgrE : com.Sconit.Service.MasterData.IOrderDetailMgr
    {
        
    }
}

#endregion

using System.Collections.Generic;
using com.Sconit.Entity.MasterData;
using System;

namespace com.Sconit.Service.Procurement
{
    public interface ILeanEngineMgr
    {
        #region 2G
        //void GenerateOrder();

        //IList<OrderHead> GenerateOrder(OrderHead orderTemplate);

        //IList<OrderHead> GenerateOrder(OrderHead orderTemplate, bool isUrgent);

        //void GetOrderDetailReqQty(OrderDetail orderDetail, bool isUrgent);

        //void UpdateAbnormalNextOrderTime();
        #endregion

        //3G
        void OrderGenerate();

        //OrderHead PreviewGenOrder(string flowCode);

        OrderHead PreviewGenOrder(string flowCode, string strategy, DateTime? windowTime, DateTime? nextWindowTime);

        void CreateOrder(OrderHead order, string userCode);

        //void ProcessCustomerPlans(List<CustomerPlan> customerPlans);

        //void ProcessMRPs(List<MRP> MRPs);
    }
}



#region Extend Interface



namespace com.Sconit.Service.Ext.Procurement
{
    public partial interface ILeanEngineMgrE : com.Sconit.Service.Procurement.ILeanEngineMgr
    {

    }
}

#endregion

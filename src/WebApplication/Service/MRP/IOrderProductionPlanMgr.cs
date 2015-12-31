using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MRP;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Customize;
using com.Sconit.Entity.Quote;

namespace com.Sconit.Service.MRP
{
    public interface IOrderProductionPlanMgr:IOrderProductionPlanBaseMgr
    {
        IList<OrderProductionPlan> GetOrderProductionPlan(string OrderPlanNo, string ProductionLineCode, string Flow, string CreateUser, string Item);
        void CreatOrderProductionPlan(OrderProductionPlan orderProductionPlan);
        IList<ItemPoint> GetItemPoint(string Item);
        void UpdateItemPoint(ItemPoint ItemPoint);
        void DeleteItemPoint(ItemPoint ItemPoint);
        void CreatItemPoint(ItemPoint ItemPoint);
        void UpdateOrderProductionPlan(OrderProductionPlan orderProductionPlan);
        void DeleteOrderProductionPlan(string OrderPlanNo);
        IList<ProductLineFacility> GetProductLineFacility(string Code);
        IList<OrderProductionPlan> GetOrderProductionPlanByID(int Id, string ProductionLineCode, string Flow);
        IList<OrderProductionPlan> GetUpOrderProductionPlanByID(int Id, string ProductionLineCode, string Flow);
        IList<OrderProductionPlan> GetOrderProductionPlanByRow(int Row, int Id, string ProductionLineCode, string Flow);
        IList<OrderProductionPlan> GetOrderProductionPlanByID(int Id);
        IList<OrderProductionPlan> GetOrderProductionPlanByBetween(int NewId, int OldId, string ProductionLineCode, string Flow);
        IList<OrderProductionPlan> GetOrderProductionPlans(string Flow, string ProductionLineCode, DateTime? PStartDate, DateTime? PEndDate, List<string> Status);
        IList<OrderProductionPlan> GetOrderProductionPlanByOrderNo(string OrderNo);

        #region 报价单部分
        IList<QuoteCustomerInfo> GetCustomer();
        IList<GPID> GetGPID(bool Status);

        IList<ItemPack> GetItemPack();
        IList<ProductInfo> GetProduct();
        #endregion
    }
}

#region Extend Interface

namespace com.Sconit.Service.Ext.MRP
{
    public partial interface IOrderProductionPlanMgrE : com.Sconit.Service.MRP.IOrderProductionPlanMgr
    {
    }
}

#endregion Extend Interface

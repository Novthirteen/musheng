using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MRP;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Customize;


namespace com.Sconit.Persistence.MRP
{
    public interface IOrderProductionPlanBaseDao
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

        /// <summary>
        /// 获取当前id之前的数据
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ProductionLineCode"></param>
        /// <param name="Flow"></param>
        /// <returns></returns>
        IList<OrderProductionPlan> GetUpOrderProductionPlanByID(int Id, string ProductionLineCode, string Flow);

        IList<OrderProductionPlan> GetOrderProductionPlanByRow(int Row,int Id, string ProductionLineCode, string Flow);

        IList<OrderProductionPlan> GetOrderProductionPlanByID(int Id);

        IList<OrderProductionPlan> GetOrderProductionPlanByBetween(int NewId, int OldId, string ProductionLineCode, string Flow);
    }
}

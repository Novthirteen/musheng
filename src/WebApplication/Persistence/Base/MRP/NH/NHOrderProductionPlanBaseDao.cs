using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MRP;
using Castle.Facilities.NHibernateIntegration;
using NHibernate;
using NHibernate.Type;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Customize;

namespace com.Sconit.Persistence.MRP.NH
{
    public class NHOrderProductionPlanBaseDao:NHDaoBase,IOrderProductionPlanBaseDao
    {
        public NHOrderProductionPlanBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        public virtual IList<OrderProductionPlan> GetOrderProductionPlan(string OrderPlanNo, string ProductionLineCode, string Flow, string CreateUser, string Item)
        {
            return FindAll<OrderProductionPlan>();
        }

        public virtual void CreatOrderProductionPlan(OrderProductionPlan orderProductionPlan)
        {
            Create(orderProductionPlan);
        }

        public virtual IList<ItemPoint> GetItemPoint(string Item)
        {
            return FindAll<ItemPoint>();
        }

        public virtual void UpdateItemPoint(ItemPoint ItemPoint)
        {
            Update(ItemPoint);
        }

        public virtual void DeleteItemPoint(ItemPoint ItemPoint)
        {
            Delete(ItemPoint);
        }

        public virtual void CreatItemPoint(ItemPoint ItemPoint)
        {
            Create(ItemPoint);
        }

        public virtual void UpdateOrderProductionPlan(OrderProductionPlan orderProductionPlan)
        {
            Update(orderProductionPlan);
        }

        public virtual void DeleteOrderProductionPlan(string OrderPlanNo)
        {
            Delete(OrderPlanNo);
        }

        public virtual IList<ProductLineFacility> GetProductLineFacility(string Code)
        {
            return FindAll<ProductLineFacility>();
        }

        public virtual IList<OrderProductionPlan> GetOrderProductionPlanByID(int Id, string ProductionLineCode, string Flow)
        {
            return FindAll<OrderProductionPlan>();
        }

        public virtual IList<OrderProductionPlan> GetUpOrderProductionPlanByID(int Id, string ProductionLineCode, string Flow)
        {
            return FindAll<OrderProductionPlan>();
        }

        public virtual IList<OrderProductionPlan> GetOrderProductionPlanByRow(int Row, int Id, string ProductionLineCode, string Flow)
        {
            return FindAll<OrderProductionPlan>();
        }

        public virtual IList<OrderProductionPlan> GetOrderProductionPlanByID(int Id)
        {
            return FindAll<OrderProductionPlan>();
        }

        public virtual IList<OrderProductionPlan> GetOrderProductionPlanByBetween(int NewId, int OldId, string ProductionLineCode, string Flow)
        {
            return FindAll<OrderProductionPlan>();
        }
    }
}

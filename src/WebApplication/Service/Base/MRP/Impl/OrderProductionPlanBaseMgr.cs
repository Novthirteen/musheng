using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MRP;
using com.Sconit.Persistence.MRP;
using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Customize;

namespace com.Sconit.Service.MRP.Impl
{
    [Transactional]
    public class OrderProductionPlanBaseMgr:SessionBase,IOrderProductionPlanBaseMgr
    {
        public IOrderProductionPlanDao entityDao { get; set; }

        [Transaction(TransactionMode.Requires)]
        public virtual IList<OrderProductionPlan> GetOrderProductionPlan(string OrderPlanNo, string ProductionLineCode, string Flow, string CreateUser, string Item)
        {
            return entityDao.GetOrderProductionPlan(OrderPlanNo, ProductionLineCode, Flow, CreateUser, Item);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void CreatOrderProductionPlan(OrderProductionPlan orderProductionPlan)
        {
            entityDao.CreatOrderProductionPlan(orderProductionPlan);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual IList<ItemPoint> GetItemPoint(string Item)
        {
            return entityDao.GetItemPoint(Item);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateItemPoint(ItemPoint ItemPoint)
        {
            entityDao.UpdateItemPoint(ItemPoint);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteItemPoint(ItemPoint ItemPoint)
        {
            entityDao.DeleteItemPoint(ItemPoint);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void CreatItemPoint(ItemPoint ItemPoint)
        {
            entityDao.CreatItemPoint(ItemPoint);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void UpdateOrderProductionPlan(OrderProductionPlan orderProductionPlan)
        {
            entityDao.UpdateOrderProductionPlan(orderProductionPlan);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual void DeleteOrderProductionPlan(string OrderPlanNo)
        {
            entityDao.DeleteOrderProductionPlan(OrderPlanNo);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual IList<ProductLineFacility> GetProductLineFacility(string Code)
        {
            return entityDao.GetProductLineFacility(Code);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual IList<OrderProductionPlan> GetOrderProductionPlanByID(int Id, string ProductionLineCode, string Flow)
        {
            return entityDao.GetOrderProductionPlanByID(Id, ProductionLineCode, Flow);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual IList<OrderProductionPlan> GetUpOrderProductionPlanByID(int Id, string ProductionLineCode, string Flow)
        {
            return entityDao.GetOrderProductionPlanByID(Id, ProductionLineCode, Flow);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual IList<OrderProductionPlan> GetOrderProductionPlanByRow(int Row,int Id, string ProductionLineCode, string Flow)
        {
            return entityDao.GetOrderProductionPlanByRow(Row, Id, ProductionLineCode, Flow);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual IList<OrderProductionPlan> GetOrderProductionPlanByID(int Id)
        {
            return entityDao.GetOrderProductionPlanByID(Id);
        }

        [Transaction(TransactionMode.Requires)]
        public virtual IList<OrderProductionPlan> GetOrderProductionPlanByBetween(int NewId, int OldId, string ProductionLineCode, string Flow)
        {
            return entityDao.GetOrderProductionPlanByBetween(NewId, OldId, ProductionLineCode, Flow);
        }
    }
}

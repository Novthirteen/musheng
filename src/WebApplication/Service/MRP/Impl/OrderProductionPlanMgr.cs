using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Entity.MRP;
using NHibernate.Expression;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MRP;
using com.Sconit.Entity.MasterData;
using NHibernate;
using NHibernate.Type;
using com.Sconit.Entity.Customize;

namespace com.Sconit.Service.MRP.Impl
{
    [Transactional]
    public class OrderProductionPlanMgr : OrderProductionPlanBaseMgr, IOrderProductionPlanMgr
    {
        public ICriteriaMgrE criteriaMgr { get; set; }
        //public IOrderProductionPlanMgrE OrderProductionPlanMgr { get; set; }

        [Transaction(TransactionMode.Requires)]
        public override IList<OrderProductionPlan> GetOrderProductionPlan(string OrderPlanNo, string ProductionLineCode, string Flow, string CreateUser, string Item)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderProductionPlan));
            if (OrderPlanNo != null && OrderPlanNo.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("OrderPlanNo", OrderPlanNo));
            }
            if (ProductionLineCode != null && ProductionLineCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("ProductionLineCode", ProductionLineCode));
            }
            if (Flow != null && Flow.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Flow", Flow));
            }
            if (CreateUser != null && CreateUser.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("CreateUser", CreateUser));
            }
            if (Item != null && Item.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Item", Item));
            }

            criteria.AddOrder(Order.Asc("StartTime"));
            return criteriaMgr.FindAll<OrderProductionPlan>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<OrderProductionPlan> GetOrderProductionPlans(string Flow, string ProductionLineCode, DateTime? PStartDate, DateTime? PEndDate, List<string> Status)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderProductionPlan));
            if (Flow != null && Flow.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Flow", Flow));
            }
            if (ProductionLineCode != null && ProductionLineCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("ProductionLineCode", ProductionLineCode));
            }
            if (PStartDate == null && PEndDate == null)
            {
                criteria.Add(Expression.Ge("StartTime", DateTime.Now.AddHours(-12)));
            }
            if (PStartDate != null)
            {
                criteria.Add(Expression.Ge("StartTime", PStartDate));
            }
            if (PEndDate != null)
            {
                criteria.Add(Expression.Le("StartTime", PEndDate));
            }
            if (Status != null && Status.Count > 0)
            {
                criteria.Add(Expression.In("Status", Status));
            }
            criteria.AddOrder(Order.Asc("StartTime"));
            return criteriaMgr.FindAll<OrderProductionPlan>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<OrderProductionPlan> GetOrderProductionPlanByOrderNo(string OrderNo)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderProductionPlan));
            criteria.Add(Expression.Eq("OrderNo", OrderNo));
            return criteriaMgr.FindAll<OrderProductionPlan>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public override void CreatOrderProductionPlan(OrderProductionPlan orderProductionPlan)
        {
            //DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderProductionPlan));
            Create(orderProductionPlan);
        }

        [Transaction(TransactionMode.Requires)]
        public override IList<ItemPoint> GetItemPoint(string Item)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(ItemPoint));
            if (Item != null && Item.Trim() != string.Empty)
            {
                criteria.Add(Expression.Like("Item", Item,MatchMode.Anywhere));
            }
            return criteriaMgr.FindAll<ItemPoint>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public override void UpdateItemPoint(ItemPoint ItemPoint)
        {
            criteriaMgr.Update(ItemPoint);
        }

        [Transaction(TransactionMode.Requires)]
        public override void DeleteItemPoint(ItemPoint ItemPoint)
        {
            criteriaMgr.Delete(ItemPoint);
        }

        [Transaction(TransactionMode.Requires)]
        public override void CreatItemPoint(ItemPoint ItemPoint)
        {
            Create(ItemPoint);
        }

        [Transaction(TransactionMode.Requires)]
        public override void UpdateOrderProductionPlan(OrderProductionPlan orderProductionPlan)
        {
            criteriaMgr.Update(orderProductionPlan);
        }

        [Transaction(TransactionMode.Requires)]
        public override void DeleteOrderProductionPlan(string OrderPlanNo)
        {
            criteriaMgr.DeleteWithHql(@"from OrderProductionPlan where OrderPlanNo = ?", new object[] { OrderPlanNo }, new IType[] { NHibernateUtil.String });
        }

        [Transaction(TransactionMode.Requires)]
        public override IList<ProductLineFacility> GetProductLineFacility(string Code)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(ProductLineFacility));
            criteria.Add(Expression.Eq("Code", Code));
            return criteriaMgr.FindAll<ProductLineFacility>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public override IList<OrderProductionPlan> GetOrderProductionPlanByID(int Id, string ProductionLineCode, string Flow)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderProductionPlan));
            criteria.Add(Expression.Gt("Id", Id));
            if (ProductionLineCode != null && ProductionLineCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("ProductionLineCode", ProductionLineCode));
            }
            if (Flow != null && Flow.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Flow", Flow));
            }
            return criteriaMgr.FindAll<OrderProductionPlan>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public override IList<OrderProductionPlan> GetUpOrderProductionPlanByID(int Id, string ProductionLineCode, string Flow)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderProductionPlan));
            criteria.Add(Expression.Lt("Id", Id));
            if (ProductionLineCode != null && ProductionLineCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("ProductionLineCode", ProductionLineCode));
            }
            if (Flow != null && Flow.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Flow", Flow));
            }
            return criteriaMgr.FindAll<OrderProductionPlan>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public override IList<OrderProductionPlan> GetOrderProductionPlanByRow(int Row, int Id, string ProductionLineCode, string Flow)
        {
            return null;
        }

        [Transaction(TransactionMode.Requires)]
        public override IList<OrderProductionPlan> GetOrderProductionPlanByID(int Id)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderProductionPlan));
            criteria.Add(Expression.Eq("Id", Id));
            return criteriaMgr.FindAll<OrderProductionPlan>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public override IList<OrderProductionPlan> GetOrderProductionPlanByBetween(int NewId, int OldId, string ProductionLineCode, string Flow)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderProductionPlan));
            if (NewId > OldId)
            {
                criteria.Add(Expression.Gt("Id", OldId));
                criteria.Add(Expression.Lt("Id", NewId));
            }
            else
            {
                criteria.Add(Expression.Gt("Id", NewId));
                criteria.Add(Expression.Lt("Id", OldId));
            }

            criteria.Add(Expression.Eq("ProductionLineCode", ProductionLineCode));
            criteria.Add(Expression.Eq("Flow", Flow));
            return criteriaMgr.FindAll<OrderProductionPlan>(criteria);
        }
    }
}

#region Extend Class

namespace com.Sconit.Service.Ext.MRP.Impl
{
    [Transactional]
    public partial class OrderProductionPlanMgrE : com.Sconit.Service.MRP.Impl.OrderProductionPlanMgr, IOrderProductionPlanMgrE
    {
    }
}

#endregion Extend Class

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.View;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using com.Sconit.Entity;
using com.Sconit.Entity.View;
using com.Sconit.Utility;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.View;

//TODO: Add other using statements here.

namespace com.Sconit.Service.View.Impl
{
    [Transactional]
    public class OrderDetailViewMgr : OrderDetailViewBaseMgr, IOrderDetailViewMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IOrderLocTransViewMgrE orderLocTransViewMgrE { get; set; }


        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderDetailView> GetProdIO(string flow, string region, string startDate, string endDate, string item, string userCode, int pageSize, int pageIndex)
        {
            IList<OrderDetailView> baseList = this.GetProdIOBaseList(flow, region, startDate, endDate, item, userCode);

            IList<OrderDetailView> list = new List<OrderDetailView>();
            if (baseList != null && baseList.Count > 0)
            {
                int startRow = GridViewHelper.GetStartRow(pageSize, pageIndex);
                int endRow = GridViewHelper.GetEndRow(pageSize, pageIndex, baseList.Count);
                for (int i = startRow; i <= endRow; i++)
                {
                    if (i < baseList.Count)
                    {
                        list.Add(baseList[i]);
                    }
                }
            }

            foreach (OrderDetailView orderDetailView in list)
            {
                orderDetailView.OutList = orderLocTransViewMgrE.GetProdIODataList(flow, region, startDate, endDate, orderDetailView.Item.Code, userCode, BusinessConstants.IO_TYPE_OUT, orderDetailView.Uom);
            }

            return list;
        }

        [Transaction(TransactionMode.Unspecified)]
        public int GetProdIOCount(string flow, string region, string startDate, string endDate, string item, string userCode)
        {
            IList<OrderDetailView> result = this.GetProdIOBaseList(flow, region, startDate, endDate, item, userCode);

            return result.Count;
        }

        #endregion Customized Methods

        #region Private Method
        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderDetailView> GetProdIOBaseList(string flow, string region, string startDate, string endDate, string item, string userCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderDetailView));
            criteria.Add(Expression.Eq("Type", BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION));
            //区域权限
            SecurityHelper.SetRegionSearchCriteria(criteria, "PartyTo.Code", userCode);
            //订单状态
            OrderHelper.SetActiveOrderStatusCriteria(criteria, "Status");

            //过滤掉全是0的
            criteria.Add(Expression.Or(Expression.Gt("ReceivedQty", decimal.Zero), Expression.Or(Expression.Gt("RejectedQty", decimal.Zero), Expression.Gt("ScrapQty", decimal.Zero))));

            if (flow != string.Empty)
            {
                criteria.Add(Expression.Eq("Flow", flow));
            }
            if (region != string.Empty)
            {
                criteria.Add(Expression.Eq("PartyTo.Code", region));
            }
            if (startDate != string.Empty)
            {
                criteria.Add(Expression.Ge("EffDate", DateTime.Parse(startDate)));
            }
            if (endDate != string.Empty)
            {
                criteria.Add(Expression.Lt("EffDate", DateTime.Parse(endDate).AddDays(1)));
            }
            if (item != string.Empty)
            {
                //  criteria.Add(Expression.Like("Item.Code", item, MatchMode.Anywhere));

                criteria.CreateAlias("Item", "i");
                criteria.Add(
             Expression.Like("i.Code", item, MatchMode.Anywhere) ||
             Expression.Like("i.Desc1", item, MatchMode.Anywhere) ||
             Expression.Like("i.Desc2", item, MatchMode.Anywhere)
             );
            }

            criteria.AddOrder(Order.Asc("Item.Code"));
            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.GroupProperty("Item"))
                .Add(Projections.GroupProperty("Uom"))
                .Add(Projections.Sum("ReceivedQty")));
            IList result = criteriaMgrE.FindAll(criteria);

            return this.ConvertToList(result);
        }

        public IList<OrderDetailView> ConvertToList(IList list)
        {
            IList<OrderDetailView> orderDetailViewList = new List<OrderDetailView>();
            if (list != null && list.Count > 0)
            {
                foreach (object obj in list)
                {
                    OrderDetailView orderDetailView = new OrderDetailView();
                    orderDetailView.Item = (Item)((object[])obj)[0];
                    orderDetailView.Uom = (string)((object[])obj)[1];
                    orderDetailView.ReceivedQty = (decimal)((object[])obj)[2];
                    orderDetailViewList.Add(orderDetailView);
                }
            }

            return orderDetailViewList;
        }

        #endregion
    }
}



#region Extend Class





namespace com.Sconit.Service.Ext.View.Impl
{
    [Transactional]
    public partial class OrderDetailViewMgrE : com.Sconit.Service.View.Impl.OrderDetailViewMgr, IOrderDetailViewMgrE
    {

    }
}
#endregion

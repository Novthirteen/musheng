using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.View;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Entity.View;
using NHibernate.Expression;
using com.Sconit.Utility;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.View.Impl
{
    [Transactional]
    public class OrderLocTransViewMgr : OrderLocTransViewBaseMgr, IOrderLocTransViewMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }

        #region Customized Methods

        //[Transaction(TransactionMode.Unspecified)]
        //public IList<OrderLocTransView> GetProdIO(string flow, string region, string startDate, string endDate, string item, string userCode, int pageSize, int pageIndex)
        //{
        //    IList baseList = this.GetProdIOBaseList(flow, region, startDate, endDate, item, userCode);

        //    IList<OrderLocTransView> list = new List<OrderLocTransView>();
        //    if (baseList != null && baseList.Count > 0)
        //    {
        //        int startRow = GridViewHelper.GetStartRow(pageSize, pageIndex);
        //        int endRow = GridViewHelper.GetEndRow(pageSize, pageIndex, baseList.Count);
        //        for (int i = startRow; i < endRow; i++)
        //        {
        //            OrderLocTransView orderLocTransView = new OrderLocTransView();
        //            orderLocTransView.ItemCode = (string)((object[])baseList[i])[0];
        //            orderLocTransView.Uom = (string)((object[])baseList[i])[1];
        //            orderLocTransView.RecQty = (decimal)((object[])baseList[i])[2];
        //            list.Add(orderLocTransView);
        //        }
        //    }

        //    foreach (OrderLocTransView orderLocTransView in list)
        //    {
        //        orderLocTransView.OutList = this.GetProdIODataList(flow, region, startDate, endDate, orderLocTransView.ItemCode, userCode, BusinessConstants.IO_TYPE_OUT);
        //        //orderLocTransView.InList = this.GetProdIODataList(flow, region, startDate, endDate, orderLocTransView.ItemCode, userCode, BusinessConstants.IO_TYPE_IN);
        //    }

        //    return list;
        //}

        //[Transaction(TransactionMode.Unspecified)]
        //public int GetProdIOCount(string flow, string region, string startDate, string endDate, string item, string userCode)
        //{
        //    IList result = this.GetProdIOBaseList(flow, region, startDate, endDate, item, userCode);

        //    return result.Count;
        //}

        [Transaction(TransactionMode.Unspecified)]
        public IList<OrderLocTransView> GetProdIODataList(string flow, string region, string startDate, string endDate, string item, string userCode, string ioType,string uomCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderLocTransView));
            criteria.Add(Expression.Eq("Type", BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION));
            //����Ȩ��
            SecurityHelper.SetRegionSearchCriteria(criteria, "PartyTo.Code", userCode);
            //����״̬
            OrderHelper.SetActiveOrderStatusCriteria(criteria, "Status");

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
                criteria.Add(Expression.Ge("StartTime", DateTime.Parse(startDate)));
            }
            if (endDate != string.Empty)
            {
                criteria.Add(Expression.Lt("StartTime", DateTime.Parse(endDate).AddDays(1)));
            }
            criteria.Add(Expression.Eq("ItemCode", item));
            criteria.Add(Expression.Eq("Uom", uomCode));
            criteria.Add(Expression.Eq("IOType", ioType));

            criteria.AddOrder(Order.Asc("Item.Code"));
            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.GroupProperty("Item"))
                .Add(Projections.Sum("AccumQty")));

            IList result = criteriaMgrE.FindAll(criteria);

            return this.ConvertToList(result);
        }

        #endregion Customized Methods

        #region Private Method
        //[Transaction(TransactionMode.Unspecified)]
        //public IList GetProdIOBaseList(string flow, string region, string startDate, string endDate, string item, string userCode)
        //{
        //    DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderDetailView));
        //    criteria.Add(Expression.Eq("Type", BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION));
        //    //����Ȩ��
        //    SecurityHelper.SetRegionSearchCriteria(criteria, "PartyTo.Code", userCode);
        //    //����״̬
        //    OrderHelper.SetActiveOrderStatusCriteria(criteria, "Status");

        //    if (flow != string.Empty)
        //    {
        //        criteria.Add(Expression.Eq("Flow", flow));
        //    }
        //    if (region != string.Empty)
        //    {
        //        criteria.Add(Expression.Eq("PartyTo.Code", region));
        //    }
        //    if (startDate != string.Empty)
        //    {
        //        criteria.Add(Expression.Ge("EffDate", DateTime.Parse(startDate)));
        //    }
        //    if (endDate != string.Empty)
        //    {
        //        criteria.Add(Expression.Lt("EffDate", DateTime.Parse(endDate).AddDays(1)));
        //    }
        //    if (item != string.Empty)
        //    {
        //        criteria.Add(Expression.Like("Item.Code", item, MatchMode.Anywhere));
        //    }

        //    criteria.AddOrder(Order.Asc("Item.Code"));
        //    criteria.SetProjection(Projections.ProjectionList()
        //        .Add(Projections.GroupProperty("Item.Code"))
        //        .Add(Projections.GroupProperty("Uom"))
        //        .Add(Projections.Sum("ReceivedQty")));
        //    IList result = criteriaMgrE.FindAll(criteria);

        //    return result;
        //}

        

        public IList<OrderLocTransView> ConvertToList(IList list)
        {
            IList<OrderLocTransView> orderLocTransViewList = new List<OrderLocTransView>();
            if (list != null && list.Count > 0)
            {
                foreach (object obj in list)
                {
                    OrderLocTransView orderLocTransView = new OrderLocTransView();
                    orderLocTransView.Item = (Item)((object[])obj)[0];
                    orderLocTransView.AccumQty = (decimal)((object[])obj)[1];
                    orderLocTransViewList.Add(orderLocTransView);
                }
            }

            return orderLocTransViewList;
        }
        #endregion
    }
}



#region Extend Class





namespace com.Sconit.Service.Ext.View.Impl
{
    [Transactional]
    public partial class OrderLocTransViewMgrE : com.Sconit.Service.View.Impl.OrderLocTransViewMgr, IOrderLocTransViewMgrE
    {
       
    }
}
#endregion

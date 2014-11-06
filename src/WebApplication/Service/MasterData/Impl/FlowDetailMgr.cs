using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using com.Sconit.Entity;
using com.Sconit.Entity.View;
using System.Linq;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class FlowDetailMgr : FlowDetailBaseMgr, IFlowDetailMgr
    {
        public IFlowDao flowDao { get; set; }
        public ICriteriaMgrE criteriaMgrE { get; set; }
        

        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public FlowDetail GetFlowDetailByItem(string flowCode, string itemCode, string locationFromCode, string locationToCode)
        {
            Flow flow = flowDao.LoadFlow(flowCode);
            FlowDetail flowDetail = null;
            DetachedCriteria criteria = DetachedCriteria.For(typeof(FlowDetail));
            criteria.CreateAlias("Flow", "fl");
            criteria.CreateAlias("fl.LocationFrom", "lf");
            criteria.CreateAlias("fl.LocationTo", "lt");
            criteria.Add(Expression.Eq("fl.Code", flowCode));
            criteria.Add(Expression.Eq("Item.Code", itemCode));

            if (locationFromCode != null && locationFromCode != string.Empty)
            {
                criteria.Add(
                    Expression.Or(
                        Expression.Eq("LocationFrom.Code", locationFromCode),
                        Expression.And(
                            Expression.IsNull("LocationFrom"),
                            Expression.Eq("lf.Code", locationFromCode))));
            }
            else
            {
                criteria.Add(Expression.And(
                                 Expression.IsNull("LocationFrom"),
                                 Expression.IsNull("lf.Code")));
            }

            if (locationToCode != null && locationToCode != string.Empty)
            {
                criteria.Add(
                    Expression.Or(
                        Expression.Eq("LocationTo.Code", locationToCode),
                        Expression.And(
                            Expression.IsNull("LocationTo"),
                            Expression.Eq("lt.Code", locationToCode))));
            }
            else
            {
                criteria.Add(Expression.And(
                                 Expression.IsNull("LocationTo"),
                                 Expression.IsNull("lt.Code")));
            }

            //todo 找到直接返回
            IList<FlowDetail> flowDetailList = criteriaMgrE.FindAll<FlowDetail>(criteria);
            if (flowDetailList.Count > 0)
            {
                flowDetail = (FlowDetail)flowDetailList[0];
            }
            else if (flowDetailList.Count == 0 && flow.ReferenceFlow != null && flow.ReferenceFlow.Trim() != string.Empty)
            {
                criteria = DetachedCriteria.For(typeof(FlowDetail));
                criteria.CreateAlias("Flow", "fl");
                criteria.CreateAlias("fl.LocationFrom", "lf");
                criteria.CreateAlias("fl.LocationTo", "lt");
                criteria.Add(Expression.Eq("fl.Code", flowCode));
                criteria.Add(Subqueries.In(itemCode,
                                             DetachedCriteria.For(typeof(FlowDetail))
                                            .CreateAlias("Item", "i")
                                            .CreateAlias("Flow", "f")
                                            .Add(Expression.Eq("f.Code", flow.ReferenceFlow))
                                            .SetProjection(Projections.ProjectionList().Add(Projections.Property("i.Code")))
                             ));

                if (locationFromCode != null && locationFromCode != string.Empty)
                {
                    criteria.Add(
                        Expression.Or(
                            Expression.Eq("LocationFrom.Code", locationFromCode),
                            Expression.And(
                                Expression.IsNull("LocationFrom"),
                                Expression.Eq("lf.Code", locationFromCode))));
                }
                else
                {
                    criteria.Add(Expression.And(
                                     Expression.IsNull("LocationFrom"),
                                     Expression.IsNull("lf.Code")));
                }

                if (locationToCode != null && locationToCode != string.Empty)
                {
                    criteria.Add(
                        Expression.Or(
                            Expression.Eq("LocationTo.Code", locationToCode),
                            Expression.And(
                                Expression.IsNull("LocationTo"),
                                Expression.Eq("lt.Code", locationToCode))));
                }
                else
                {
                    criteria.Add(Expression.And(
                                     Expression.IsNull("LocationTo"),
                                     Expression.IsNull("lt.Code")));
                }


                flowDetailList = criteriaMgrE.FindAll<FlowDetail>(criteria);
                if (flowDetailList.Count != 0)
                {
                    flowDetail = (FlowDetail)flowDetailList[0];
                }
            }
            return flowDetail;


        }

        [Transaction(TransactionMode.Unspecified)]
        public FlowDetail GetFlowDetailBySeq(string flowCode, int seq)
        {
            Flow flow = flowDao.LoadFlow(flowCode);
            FlowDetail flowDetail = null;
            DetachedCriteria criteria = DetachedCriteria.For(typeof(FlowDetail));
            criteria.Add(Expression.Eq("Sequence", seq));
            if (flow.ReferenceFlow != null && flow.ReferenceFlow.Trim() != string.Empty)
            {
                criteria.Add(Expression.Or(Expression.Eq("Flow.Code", flowCode), Expression.Eq("Flow.Code", flow.ReferenceFlow)));
            }
            else
            {
                criteria.Add(Expression.Eq("Flow.Code", flowCode));
            }
            List<FlowDetail> flowDetailList = (List<FlowDetail>)criteriaMgrE.FindAll<FlowDetail>(criteria);
            if (flowDetailList.Count > 0)
            {
                flowDetail = flowDetailList[0];
            }
            return flowDetail;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<FlowDetail> GetFlowDetail(string flowCode)
        {
            return GetFlowDetail(flowCode, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<FlowDetail> GetFlowDetail(string flowCode, bool includeRefDetail)
        {
            Flow flow = flowDao.LoadFlow(flowCode);
            if (flow == null)
            {
                return null;
            }
            return GetFlowDetail(flow, includeRefDetail);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<FlowDetail> GetFlowDetail(Flow flow)
        {
            return GetFlowDetail(flow, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<FlowDetail> GetFlowDetail(Flow flow, bool includeRefDetail)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(FlowDetail));

            if (includeRefDetail && flow.ReferenceFlow != null && flow.ReferenceFlow.Trim() != string.Empty)
            {
                criteria.Add(Expression.Or(Expression.Eq("Flow.Code", flow.Code), Expression.Eq("Flow.Code", flow.ReferenceFlow)));
                criteria.Add(Expression.Or(Expression.Le("StartDate", DateTime.Now), Expression.IsNull("StartDate")));
                criteria.Add(Expression.Or(Expression.Ge("EndDate", DateTime.Now), Expression.IsNull("EndDate")));
            }
            else
            {
                criteria.Add(Expression.Eq("Flow.Code", flow.Code));
                criteria.Add(Expression.Or(Expression.Le("StartDate", DateTime.Now), Expression.IsNull("StartDate")));
                criteria.Add(Expression.Or(Expression.Ge("EndDate", DateTime.Now), Expression.IsNull("EndDate")));
            }

            criteria.AddOrder(Order.Asc("Sequence"));

            return criteriaMgrE.FindAll<FlowDetail>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Item> GetAllFlowDetailItem(string flowCode)
        {
            IList<Item> itemList = new List<Item>();
            IList<FlowDetail> flowDetailList = this.GetFlowDetail(flowCode, true);
            if (flowDetailList != null && flowDetailList.Count > 0)
            {
                foreach (FlowDetail flowDetail in flowDetailList)
                {
                    itemList.Add(flowDetail.Item);
                }
            }
            return itemList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public override void UpdateFlowDetail(FlowDetail entity)
        {
            base.UpdateFlowDetail(entity);
            Flow flow = entity.Flow;
            flow.LastModifyDate = DateTime.Now;
            flowDao.UpdateFlow(flow);
        }

        [Transaction(TransactionMode.Unspecified)]
        public override void CreateFlowDetail(FlowDetail entity)
        {
            base.CreateFlowDetail(entity);
            Flow flow = entity.Flow;
            flow.LastModifyDate = DateTime.Now;
            flowDao.UpdateFlow(flow);
        }
        [Transaction(TransactionMode.Unspecified)]
        public override void DeleteFlowDetail(FlowDetail entity)
        {
            base.DeleteFlowDetail(entity);
        }

        [Transaction(TransactionMode.Unspecified)]
        public override void DeleteFlowDetail(int id)
        {
            Flow flow = this.LoadFlowDetail(id).Flow;
            flow.LastModifyDate = DateTime.Now;
            flowDao.UpdateFlow(flow);
            base.DeleteFlowDetail(id);
        }

        [Transaction(TransactionMode.Unspecified)]
        public FlowDetail LoadFlowDetail(string flowCode, string itemCode, int seq)
        {
            FlowDetail flowDetail = null;
            DetachedCriteria criteria = DetachedCriteria.For(typeof(FlowDetail));
            criteria.Add(Expression.Eq("Flow.Code", flowCode));
            criteria.Add(Expression.Eq("Item.Code", itemCode));
            IList<FlowDetail> flowDetailList = criteriaMgrE.FindAll<FlowDetail>(criteria);
            if (flowDetailList != null && flowDetailList.Count > 0)
            {
                if (flowDetailList.Count == 1)
                {
                    flowDetail = flowDetailList[0];
                }
                else
                {
                    foreach (FlowDetail fd in flowDetailList)
                    {
                        if (fd.Sequence == seq)
                        {
                            flowDetail = fd;
                            break;
                        }
                    }
                }
            }

            return flowDetail;
        }
        #endregion Customized Methods
    }
}


#region Extend Class




namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class FlowDetailMgrE : com.Sconit.Service.MasterData.Impl.FlowDetailMgr, IFlowDetailMgrE
    {
        
    }
}
#endregion

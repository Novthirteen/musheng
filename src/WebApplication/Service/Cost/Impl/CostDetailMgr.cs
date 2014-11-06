using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Cost;
using NHibernate.Expression;
using com.Sconit.Entity.Cost;
using com.Sconit.Service.Ext.Criteria;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost.Impl
{
    [Transactional]
    public class CostDetailMgr : CostDetailBaseMgr, ICostDetailMgr
    {
        public ICriteriaMgrE criteriaMgr { get; set; }

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public decimal? CalculateItemUnitCost(string item, string costGroupCode, int financeYear, int financeMonth)
        {
            DetachedCriteria criteria = DetachedCriteria.For<CostDetail>();

            criteria.Add(Expression.Eq("Item", item));
            criteria.Add(Expression.Eq("CostGroup.Code", costGroupCode));
            criteria.Add(Expression.Eq("FinanceYear", financeYear));
            criteria.Add(Expression.Eq("FinanceMonth", financeMonth));

            criteria.SetProjection(Projections.ProjectionList().Add(Projections.Sum("Cost")));

            IList list = this.criteriaMgr.FindAll(criteria);
            if (list != null && list.Count > 0 && list[0] != null) 
            {
                return (decimal)list[0];
            }
            else
            {
                return null;
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public decimal? CalculateItemUnitCost(string item, string costGroupCode, string costElement, int financeYear, int financeMonth)
        {
            DetachedCriteria criteria = DetachedCriteria.For<CostDetail>();

            criteria.Add(Expression.Eq("Item", item));
            criteria.Add(Expression.Eq("CostGroup.Code", costGroupCode));
            criteria.Add(Expression.Eq("CostElement.Code", costElement));
            criteria.Add(Expression.Eq("FinanceYear", financeYear));
            criteria.Add(Expression.Eq("FinanceMonth", financeMonth));

            criteria.SetProjection(Projections.ProjectionList().Add(Projections.Sum("Cost")));

            IList list = this.criteriaMgr.FindAll(criteria);
            if (list != null && list.Count > 0)
            {
                return (decimal)list[0];
            }
            else
            {
                return null;
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<CostDetail> GetCostDetail(string item, string costGroup, int financeYear, int financeMonth)
        {
            DetachedCriteria criteria = DetachedCriteria.For<CostDetail>();

            criteria.Add(Expression.Eq("Item", item));
            criteria.Add(Expression.Eq("CostGroup.Code", costGroup));
            criteria.Add(Expression.Eq("FinanceYear", financeYear));
            criteria.Add(Expression.Eq("FinanceMonth", financeMonth));

            return this.criteriaMgr.FindAll<CostDetail>(criteria);
        }
        #endregion Customized Methods
    }
}

#region Extend Class
namespace com.Sconit.Service.Ext.Cost.Impl
{
    [Transactional]
    public partial class CostDetailMgrE : com.Sconit.Service.Cost.Impl.CostDetailMgr, ICostDetailMgrE
    {

    }
}
#endregion
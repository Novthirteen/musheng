using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Cost;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Cost;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost.Impl
{
    [Transactional]
    public class StandardCostMgr : StandardCostBaseMgr, IStandardCostMgr
    {
        public ICriteriaMgrE criteriaMgr { get; set; }
        #region Customized Methods

        public StandardCost FindStandardCost(Item item, CostElement costElement, CostGroup costGroup)
        {
            return FindStandardCost(item.Code, costElement.Code, costGroup.Code);
        }

        public StandardCost FindStandardCost(string itemCode, string costElementCode, string costGroupCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For<StandardCost>();
            criteria.Add(Expression.Eq("Item", itemCode));
            criteria.Add(Expression.Eq("CostElement.Code", costElementCode));
            criteria.Add(Expression.Eq("CostGroup.Code", costGroupCode));

            IList<StandardCost> list = this.criteriaMgr.FindAll<StandardCost>(criteria);

            if (list != null && list.Count > 0)
            {
                return list[0];
            }

            return null;
        }

        public Decimal? SumStandardCost(Item item, CostElement costElement, CostGroup costGroup)
        {
            return SumStandardCost(item.Code, costElement.Code, costGroup.Code);
        }

        public Decimal? SumStandardCost(string itemCode, string costElementCode, string costGroupCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For<StandardCost>();
            criteria.SetProjection(Projections.ProjectionList().Add(Projections.Sum("Cost")));
            criteria.Add(Expression.Eq("Item", itemCode));
            criteria.Add(Expression.Eq("CostElement.Code", costElementCode));
            criteria.Add(Expression.Eq("CostGroup.Code", costGroupCode));

            IList list = this.criteriaMgr.FindAll(criteria);

            if (list != null && list.Count > 0 && list[0] != null)
            {
                return (decimal)list[0];
            }

            return null;
        }

        public Decimal? SumStandardCost(Item item, CostGroup costGroup)
        {
            return SumStandardCost(item.Code, costGroup.Code);
        }

        public Decimal? SumStandardCost(string itemCode, string costGroupCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For<StandardCost>();
            criteria.SetProjection(Projections.ProjectionList().Add(Projections.Sum("Cost")));
            criteria.Add(Expression.Eq("Item", itemCode));
            criteria.Add(Expression.Eq("CostGroup.Code", costGroupCode));

            IList list = this.criteriaMgr.FindAll(criteria);

            if (list != null && list.Count > 0 && list[0] != null)
            {
                return (decimal)list[0];
            }

            return null;
        }

        #endregion Customized Methods
    }
}

#region Extend Class
namespace com.Sconit.Service.Ext.Cost.Impl
{
    [Transactional]
    public partial class StandardCostMgrE : com.Sconit.Service.Cost.Impl.StandardCostMgr, IStandardCostMgrE
    {

    }
}
#endregion
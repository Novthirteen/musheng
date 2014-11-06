using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MRP;
using com.Sconit.Entity.MRP;
using NHibernate.Expression;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Entity;
using System.Linq;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MRP.Impl
{
    [Transactional]
    public class MrpShipPlanMgr : MrpShipPlanBaseMgr, IMrpShipPlanMgr
    {
        public ICriteriaMgrE criteriaMgr { get; set; }
        #region Customized Methods

        [Transaction(TransactionMode.Requires)]
        public IList<MrpShipPlan> GetMrpShipPlans(string flowCode, string locCode, string itemCode, DateTime effectiveDate, DateTime? winDate, DateTime? startDate)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(MrpShipPlan));
            if (itemCode != null && itemCode.Trim() != string.Empty)
            {
              //  criteria.Add(Expression.Like("Item", itemCode, MatchMode.Anywhere));
             //   criteria.CreateAlias("Item", "i");
                criteria.Add(
                   Expression.Like("Item", itemCode, MatchMode.Anywhere) ||
                   Expression.Like("ItemDescription", itemCode, MatchMode.Anywhere) 
                   );
            }
            if (flowCode != null && flowCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Flow", flowCode));
            }
            if (locCode != null && locCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("LocationTo", locCode));
            }
            criteria.Add(Expression.Eq("EffectiveDate", effectiveDate.Date));

            if (winDate.HasValue)
            {
                criteria.Add(Expression.Ge("WindowTime", winDate.Value.Date));
                criteria.Add(Expression.Lt("WindowTime", winDate.Value.Date.AddDays(1)));
            }
            if (startDate.HasValue)
            {
                criteria.Add(Expression.Ge("StartTime", startDate.Value.Date));
                criteria.Add(Expression.Lt("StartTime", startDate.Value.Date.AddDays(1)));
            }

            IList<MrpShipPlan> mrpShipPlan = criteriaMgr.FindAll<MrpShipPlan>(criteria);
            mrpShipPlan = mrpShipPlan.OrderBy(m => m.WindowTime).ThenBy(m => m.Item).ThenBy(m => m.Id).ToList();
            return mrpShipPlan;
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateMrpShipPlan(IList<MrpShipPlan> mrpShipPlans)
        {
            foreach (MrpShipPlan mrpShipPlan in mrpShipPlans)
            {
                UpdateMrpShipPlan(mrpShipPlan);
            }
        }

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.MRP.Impl
{
    [Transactional]
    public partial class MrpShipPlanMgrE : com.Sconit.Service.MRP.Impl.MrpShipPlanMgr, IMrpShipPlanMgrE
    {
    }
}

#endregion Extend Class
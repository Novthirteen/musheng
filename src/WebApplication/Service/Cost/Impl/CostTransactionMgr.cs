using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Cost;
using com.Sconit.Entity.Cost;
using com.Sconit.Entity.Exception;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using com.Sconit.Entity.MasterData;
using com.Sconit.Utility;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost.Impl
{   
    [Transactional]
    public class CostTransactionMgr : CostTransactionBaseMgr, ICostTransactionMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }

        public IList<CostTransaction> GetCostTransaction(IList<string> itemList, IList<string> costgroupList,IList<string> itemcategoryList,FinanceCalendar financeCalendar)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(CostTransaction));
            if (itemList != null && itemList.Count > 0)
            {
                if (itemList.Count == 1)
                {
                    criteria.Add(Expression.Eq("Item", itemList[0]));
                }
                else
                {
                    criteria.Add(Expression.InG<string>("Item", itemList));
                }
            }
            if (costgroupList != null && costgroupList.Count > 0)
            {
                if (costgroupList.Count == 1)
                {
                    criteria.Add(Expression.Eq("CostGroup.Code", costgroupList[0]));
                }
                else
                {
                    criteria.Add(Expression.InG<string>("CostGroup.Code", costgroupList));
                }
            }
            if (itemcategoryList != null && itemcategoryList.Count > 0)
            {
                if (itemcategoryList.Count == 1)
                {
                    criteria.Add(Expression.Eq("ItemCategory", itemcategoryList[0]));
                }
                else
                {
                    criteria.Add(Expression.InG<string>("ItemCategory", itemcategoryList));
                }
            }
            criteria.Add(Expression.Ge("EffectiveDate", financeCalendar.StartDate));
            criteria.Add(Expression.Lt("EffectiveDate", financeCalendar.EndDate));

            return criteriaMgrE.FindAll<CostTransaction>(criteria);
 
        }
    }
}

#region Extend Class
namespace com.Sconit.Service.Ext.Cost.Impl
{
    [Transactional]
    public partial class CostTransactionMgrE : com.Sconit.Service.Cost.Impl.CostTransactionMgr, ICostTransactionMgrE
    {

    }
}
#endregion
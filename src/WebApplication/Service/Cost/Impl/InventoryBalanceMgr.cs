using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Cost;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity;
using com.Sconit.Entity.Distribution;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using com.Sconit.Entity.Exception;
using NHibernate.Transform;
using com.Sconit.Service.Ext.Cost;
using com.Sconit.Entity.Cost;
using com.Sconit.Service.MasterData;
using com.Sconit.Utility;
using System.Linq;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost.Impl
{
    [Transactional]
    public class InventoryBalanceMgr : InventoryBalanceBaseMgr, IInventoryBalanceMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public ICostTransactionMgrE CostTransactionMgrE { get; set; }
        public IFinanceCalendarMgrE FinanceCalendarMgrE { get; set; }
        public ICostBalanceMgrE CostBalanceMgrE { get; set; }

        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public IList<InventoryBalance> GetInventoryBalance(IList<string> itemList, IList<string> costgroupList, Int32 financeYear, Int32 financeMonth)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(InventoryBalance));
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
                    criteria.Add(Expression.Eq("CostGroup", costgroupList[0]));
                }
                else
                {
                    criteria.Add(Expression.InG<string>("CostGroup", costgroupList));
                }
            }
            criteria.Add(Expression.Eq("FinanceYear", financeYear));
            criteria.Add(Expression.Eq("FinanceMonth", financeMonth));

            return criteriaMgrE.FindAll<InventoryBalance>(criteria);
        }
        
        [Transaction(TransactionMode.Unspecified)]
        public void PostProcessGrossProfit(IList list, int financeYear, int financeMonth)
        {
            //if (list == null)
            //    throw new BusinessErrorException("Common.Business.Warn.DetailEmpty");

            //IList<InventoryBalance> inventoryBalanceList = IListHelper.ConvertToList<InventoryBalance>(list);
            //FinanceCalendar currentfc = FinanceCalendarMgrE.GetFinanceCalendar(financeYear,financeMonth);
            
            //IList<string> itemList = inventoryBalanceList.Select(l => l.Item).Distinct().ToList<string>();
            //IList<string> costgroupList = inventoryBalanceList.Select(l => l.CostGroup.Code).Distinct().ToList<string>();
            //IList<string> itemcategoryList = inventoryBalanceList.Select(l => l.ItemCategory).Distinct().ToList<String>();
            //IList<CostTransaction> costTransList = CostTransactionMgrE.GetCostTransaction(itemList, costgroupList, itemcategoryList, currentfc);
            //IList<CostBalance> costBalanceList = CostBalanceMgrE.GetCostBalanceList(itemList, costgroupList, itemcategoryList);

            //var groupedCostTrans = from l in costTransList
            //                       group l by new { newItem = l.Item, newCostElement = l.CostElement, newCostGroup = l.CostGroup, newRefItem = l.ReferenceItem }
            //                           into g
            //                           select new
            //                           {
            //                               g.Key.newItem,
            //                               g.Key.newCostElement,
            //                               g.Key.newCostGroup,
            //                               g.Key.newRefItem,
            //                               sumStandardAmount = g.Sum(h => h.StandardAmount),
            //                               sumActualAmount = g.Sum(h => h.ActualAmount),
            //                               sumRefQty = g.Sum(h => h.ReferenceQty)
            //                           };

            //foreach (var ib in inventoryBalanceList)
            //{
            //    ib.StartInvBalance = (from l in costBalanceList
            //                          where l.Item == ib.Item && l.CostGroup.Code == ib.CostGroup.Code
            //                          && l.FinanceYear == ib.FinanceYear && l.FinanceMonth == ib.FinanceMonth
            //                          select l.Balance).Sum(); 
            //}
        }
        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.Cost.Impl
{
    [Transactional]
    public partial class InventoryBalanceMgrE : com.Sconit.Service.Cost.Impl.InventoryBalanceMgr, IInventoryBalanceMgrE
    {
    }
}

#endregion Extend Class
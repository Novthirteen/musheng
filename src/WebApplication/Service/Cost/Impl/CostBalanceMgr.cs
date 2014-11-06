using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Cost;
using NHibernate.Expression;
using com.Sconit.Entity.Cost;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.Cost;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost.Impl
{
    [Transactional]
    public class CostBalanceMgr : CostBalanceBaseMgr, ICostBalanceMgr
    {
        public ICostElementMgrE costElementMgr { get; set; }
        public ICostGroupMgrE costGroupMgr { get; set; }
        public ICriteriaMgrE criteriaMgr { get; set; }
        public IItemMgrE itemMgr { get; set; }
        public IFinanceCalendarMgrE financeCalendarMgr { get; set; }
        public ICostDetailMgrE costDetailMgr { get; set; }

        #region Customized Methods
        [Transaction(TransactionMode.Requires)]
        public void ChangeCostBalance(string item, string costGroupCode, decimal qty, User user)
        {
            FinanceCalendar financeCalendar = this.financeCalendarMgr.GetLastestOpenFinanceCalendar();
            int lastFinanceYear = financeCalendar.FinanceMonth != 1 ? financeCalendar.FinanceYear : financeCalendar.FinanceYear - 1;
            int lastFinanceMonth = financeCalendar.FinanceMonth != 1 ? financeCalendar.FinanceMonth - 1 : 12;

            IList<CostDetail> costDetailList = costDetailMgr.GetCostDetail(item, costGroupCode, lastFinanceYear, lastFinanceMonth);

            if (costDetailList != null && costDetailList.Count > 0)
            {
                foreach (CostDetail costDetail in costDetailList)
                {
                    ChangeCostBalance(item, costGroupCode, costDetail.CostElement.Code, costDetail.Cost * qty, financeCalendar.FinanceYear, financeCalendar.FinanceMonth, user);
                }
            }
        }


        [Transaction(TransactionMode.Requires)]
        public void ChangeCostBalance(string item, string costGroupCode, string costElementCode, decimal amount, User user)
        {
            FinanceCalendar financeCalendar = this.financeCalendarMgr.GetLastestOpenFinanceCalendar();
            ChangeCostBalance(item, costGroupCode, costElementCode, amount, financeCalendar.FinanceYear, financeCalendar.FinanceMonth, user);
        }

        [Transaction(TransactionMode.Requires)]
        public void ChangeCostBalance(string item, string costGroupCode, string costElementCode, decimal amount, int financeYear, int financeMonth, User user)
        {

            CostBalance itemCostBalance = GetCostBalance(item, costGroupCode, costElementCode, financeYear, financeMonth);
            if (itemCostBalance != null)
            {
                itemCostBalance.Balance += amount;

                this.UpdateCostBalance(itemCostBalance);
            }
            else
            {
                itemCostBalance = new CostBalance();
                itemCostBalance.Item = item;
                itemCostBalance.ItemCategory = itemMgr.CheckAndLoadItem(item).ItemCategory.Code;
                itemCostBalance.CostGroup = this.costGroupMgr.CheckAndLoadCostGroup(costGroupCode);
                itemCostBalance.CostElement = this.costElementMgr.CheckAndLoadCostElement(costElementCode);
                itemCostBalance.CreateDate = DateTime.Now;
                itemCostBalance.CreateUser = user.Code;
                itemCostBalance.Balance = amount;
                itemCostBalance.FinanceYear = financeYear;
                itemCostBalance.FinanceMonth = financeMonth;

                this.CreateCostBalance(itemCostBalance);
            }
        }

        [Transaction(TransactionMode.Requires)]
        public CostBalance GetCostBalance(string item, string costGroupCode, string costElementCode, int financeYear, int financeMonth)
        {
            DetachedCriteria criteria = DetachedCriteria.For<CostBalance>();
            criteria.Add(Expression.Eq("Item", item));
            criteria.Add(Expression.Eq("CostGroup.Code", costGroupCode));
            criteria.Add(Expression.Eq("CostElement.Code", costElementCode));
            criteria.Add(Expression.Eq("FinanceYear", financeYear));
            criteria.Add(Expression.Eq("FinanceMonth", financeMonth));

            IList<CostBalance> itemCostBalanceList = this.criteriaMgr.FindAll<CostBalance>(criteria);

            if (itemCostBalanceList != null && itemCostBalanceList.Count > 0)
            {
                return itemCostBalanceList[0];
            }
            else
            {
                return null;
            }
        }

        [Transaction(TransactionMode.Requires)]
        public IList<CostBalance> GetCostBalance(string item, string costGroupCode, int financeYear, int financeMonth)
        {
            DetachedCriteria criteria = DetachedCriteria.For<CostBalance>();
            criteria.Add(Expression.Eq("Item", item));
            criteria.Add(Expression.Eq("CostGroup.Code", costGroupCode));
            criteria.Add(Expression.Eq("FinanceYear", financeYear));
            criteria.Add(Expression.Eq("FinanceMonth", financeMonth));

            return this.criteriaMgr.FindAll<CostBalance>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<CostBalance> GetCostBalanceList(IList<string> itemList, IList<string> costgroupList, IList<string> itemcategoryList)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(CostBalance));
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

            return criteriaMgr.FindAll<CostBalance>(criteria);

        }
        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.Cost.Impl
{
    [Transactional]
    public partial class CostBalanceMgrE : com.Sconit.Service.Cost.Impl.CostBalanceMgr, ICostBalanceMgrE
    {
    }
}

#endregion Extend Class
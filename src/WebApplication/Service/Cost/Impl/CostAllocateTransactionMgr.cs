using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Cost;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Cost;
using com.Sconit.Service.Ext.Cost;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost.Impl
{
    [Transactional]
    public class CostAllocateTransactionMgr : CostAllocateTransactionBaseMgr, ICostAllocateTransactionMgr
    {

        public ICostCenterMgrE costCenterMgr { get; set; }
        public IEntityPreferenceMgrE entityPreferenceMgr { get; set; }
        public ICostElementMgrE costElementMgr { get; set; }
        public IFinanceCalendarMgrE financeCalendarMgr { get; set; }

        #region Customized Methods

        [Transaction(TransactionMode.Requires)]
        public void RecordCustomerGoodsDiff(Item customerGoods, decimal diffQty, string costCenterCode, User user)
        {
            RecordCustomerGoodsDiff(customerGoods, diffQty, costCenterCode, user, DateTime.Now);
        }

        [Transaction(TransactionMode.Requires)]
        public void RecordCustomerGoodsDiff(Item customerGoods, decimal diffQty, string costCenterCode, User user, DateTime effectiveDate)
        {
            if (!customerGoods.ScrapPrice.HasValue)
            {
                throw new BusinessErrorException("Cost.CostAllocateTransaction.Error.NoPriceForCustomerGoods", customerGoods.Code);
            }

            FinanceCalendar financeCalendar = this.financeCalendarMgr.GetLastestOpenFinanceCalendar();

            if (financeCalendar.StartDate > effectiveDate)
            {
                throw new BusinessErrorException("Cost.CostAllocateTransaction.Error.EffdateLTFCStartDate", effectiveDate.ToLongDateString(), financeCalendar.StartDate.ToLongDateString());
            }
            
            EntityPreference materialPerference = entityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_COST_ELEMENT_MATERIAL);
            CostElement materialCostElement = costElementMgr.LoadCostElement(materialPerference.Value);

            CostAllocateTransaction costAllocateTransaction = new CostAllocateTransaction();
            costAllocateTransaction.CostCenter = costCenterMgr.CheckAndLoadCostCenter(costCenterCode);
            costAllocateTransaction.CostElement = materialCostElement;
            costAllocateTransaction.DependCostElement = materialCostElement;
            costAllocateTransaction.AllocateBy = BusinessConstants.CODE_MASTER_COST_ALLOCATE_BY_QTY;
            costAllocateTransaction.Amount = customerGoods.ScrapPrice.Value * diffQty * -1;//因为差异为负数，所以要乘以-1
            costAllocateTransaction.EffectiveDate = effectiveDate;
            costAllocateTransaction.ReferenceItems = customerGoods.Code;
            costAllocateTransaction.CreateUser = user.Code;
            costAllocateTransaction.CreateDate = DateTime.Now;

            this.CreateCostAllocateTransaction(costAllocateTransaction);
        }

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.Cost.Impl
{
    [Transactional]
    public partial class CostAllocateTransactionMgrE : com.Sconit.Service.Cost.Impl.CostAllocateTransactionMgr, ICostAllocateTransactionMgrE
    {
    }
}

#endregion Extend Class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Cost;
using com.Sconit.Entity.Distribution;

namespace com.Sconit.Service.Cost
{
    public interface ICostMgr
    {
        #region Customized Methods

        void RecordProductionCostTransaction(Receipt receipt, User user);

        void RecordProductionBackFlushCostTransaction(OrderLocationTransaction orderLocationTransaction, Item item, decimal qty, User user);

        void RecordProductionSettingCostTransaction(OrderHead orderhead, User user);

        void RecordProcurementCostTransaction(PlannedBill plannedBill, User user);

        void RecordDiffProcurementCostTransaction(ActingBill actingBill, decimal diffAmount, User user);

        void RecordCostAllocationTransaction(CostAllocateTransaction costAllocateTransaction, User user);

        void RecordCostAllocationTransaction(CostAllocateTransaction costAllocateTransaction, User user, DateTime effectiveDate);

        void CloseFinanceMonth(User user);

        void AollcateCost(User user);

        #endregion Customized Methods


        IList<Balance> GetHisInv(DateTime effDate);
    }
}
#region Extend Interface
namespace com.Sconit.Service.Ext.Cost
{
    public partial interface ICostMgrE : com.Sconit.Service.Cost.ICostMgr
    {

    }
}
#endregion


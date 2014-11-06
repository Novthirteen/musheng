using System;
using System.Collections;
using System.Collections.Generic;
using com.Sconit.Entity.Cost;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface ICostTransactionMgr : ICostTransactionBaseMgr
    {
        #region Customized Methods

        IList<CostTransaction> GetCostTransaction(IList<string> itemList, IList<string> costgroupList, IList<string> itemcategoryList, FinanceCalendar financeCalendar);

        #endregion Customized Methods
    }
}
#region Extend Interface
namespace com.Sconit.Service.Ext.Cost
{
    public partial interface ICostTransactionMgrE : com.Sconit.Service.Cost.ICostTransactionMgr
    {

    }
}
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Cost;
using com.Sconit.Entity.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost.Impl
{
    [Transactional]
    public class FgCostMgr : FgCostBaseMgr, IFgCostMgr
    {
        #region Customized Methods

        [Transaction(TransactionMode.Requires)]
        public void CreateFgCost(IList<FgCost> fgCosts)
        {
            foreach (var fgCost in fgCosts)
            {
                base.CreateFgCost(fgCost);
            }
        }

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.Cost.Impl
{
    [Transactional]
    public partial class FgCostMgrE : com.Sconit.Service.Cost.Impl.FgCostMgr, IFgCostMgrE
    {
    }
}

#endregion Extend Class
using System;
using System.Collections.Generic;
using com.Sconit.Entity.Cost;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface IFgCostMgr : IFgCostBaseMgr
    {
        #region Customized Methods

        void CreateFgCost(IList<FgCost> fgCosts);

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.Cost
{
    public partial interface IFgCostMgrE : com.Sconit.Service.Cost.IFgCostMgr
    {
    }
}

#endregion Extend Interface
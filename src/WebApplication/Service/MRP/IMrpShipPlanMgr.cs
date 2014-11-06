using System;
using com.Sconit.Entity.MRP;
using System.Collections.Generic;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MRP
{
    public interface IMrpShipPlanMgr : IMrpShipPlanBaseMgr
    {
        #region Customized Methods

        IList<MrpShipPlan> GetMrpShipPlans(string flowCode, string locCode, string itemCode, DateTime effectiveDate, DateTime? winDate, DateTime? startDate);

        void UpdateMrpShipPlan(IList<MrpShipPlan> mrpShipPlans);

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.MRP
{
    public partial interface IMrpShipPlanMgrE : com.Sconit.Service.MRP.IMrpShipPlanMgr
    {
    }
}

#endregion Extend Interface
using System;
using com.Sconit.Entity.MRP;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MRP
{
    public interface IMrpShipPlanViewMgr : IMrpShipPlanViewBaseMgr
    {
        #region Customized Methods


        IList<MrpShipPlanView> GetMrpShipPlanViews(string flowCode, string locCode, string itemCode, DateTime effectiveDate, DateTime? winDate, DateTime? startDate);

        ScheduleView TransferMrpShipPlanViews2ScheduleView(IList<MrpShipPlanView> mrpShipPlanViews, IList<ExpectTransitInventoryView> expectTransitInventoryViews,
            IList<ItemDiscontinue> itemDiscontinueList, string locOrFlow, string winOrStartTime);

        ScheduleView TransferMrpShipPlanViews2ScheduleView2(IList<MrpShipPlanView> mrpShipPlanViews, IList<ExpectTransitInventoryView> expectTransitInventoryViews,
           IList<ItemDiscontinue> itemDiscontinueList);

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.MRP
{
    public partial interface IMrpShipPlanViewMgrE : com.Sconit.Service.MRP.IMrpShipPlanViewMgr
    {
    }
}

#endregion Extend Interface
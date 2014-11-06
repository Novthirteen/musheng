using com.Sconit.Service.Ext.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.MRP;

namespace com.Sconit.Service.MasterData
{
    public interface IImportMgr
    {
        IList<ShiftPlanSchedule> ReadPSModelFromXls(Stream inputStream, User user, string regionCode, string flowCode, DateTime date, string shiftCode);

        IList<CycleCountDetail> ReadCycleCountFromXls(Stream inputStream, User user, CycleCount cycleCount);

        IList<FlowPlan> ReadShipScheduleYFKFromXls(Stream inputStream, User user, string planType, string flowCode, string timePeriodType, DateTime date);

        IList<FlowPlan> ReadShipScheduleCSFromXls(Stream inputStream, User user, string planType, string flowCode, string timePeriodType, DateTime date);

        IList<FlowPlan> ReadScheduleFromXls(Stream inputStream, User user, string moduleType, string flowCode, string timePeriodType, DateTime date);

        CustomerSchedule ReadCustomerScheduleFromXls(Stream inputStream, User user, DateTime? startDate, DateTime? endDate, string flowCode, string refScheduleNo, bool isItemRef);

        IList<OrderLocationTransaction> ReadOrderLocationTransactionFromXls(Stream inputStream, string orderNo);
    }
}





#region Extend Interface


namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IImportMgrE : com.Sconit.Service.MasterData.IImportMgr
    {

    }
}

#endregion

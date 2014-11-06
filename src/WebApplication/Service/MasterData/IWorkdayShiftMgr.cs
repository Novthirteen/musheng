using com.Sconit.Service.Ext.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IWorkdayShiftMgr : IWorkdayShiftBaseMgr
    {
        #region Customized Methods

        IList<Shift> GetShiftsNotInWorkday(int Id);

        IList<Shift> GetShiftsByWorkdayId(int Id);

        WorkdayShift LoadWorkdayShift(int workdayId, string shiftCode);

        void CreateWorkdayShifts(Workday workday, IList<Shift> sList);

        IList<WorkdayShift> GetWorkdayShiftsByWorkdayId(int workdayId);

        #endregion Customized Methods
    }
}





#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IWorkdayShiftMgrE : com.Sconit.Service.MasterData.IWorkdayShiftMgr
    {
        
    }
}

#endregion

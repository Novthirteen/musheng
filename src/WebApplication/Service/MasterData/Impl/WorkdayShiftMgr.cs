using com.Sconit.Service.Ext.MasterData;


using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using NHibernate.Expression;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class WorkdayShiftMgr : WorkdayShiftBaseMgr, IWorkdayShiftMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IShiftMgrE ShiftMgrE { get; set; }
      

        #region Customized Methods

        public IList<Shift> GetShiftsNotInWorkday(int Id)
        {
            IList<Shift> allShifts = ShiftMgrE.GetAllShift();
            IList<Shift> workdayShifts = GetShiftsByWorkdayId(Id);
            List<Shift> otherShifts = new List<Shift>();
            if (allShifts != null && allShifts.Count > 0)
            {
                foreach (Shift s in allShifts)
                {
                    if (!workdayShifts.Contains(s))
                    {
                        otherShifts.Add(s);
                    }
                }
            }
            return otherShifts;
        }

        public IList<Shift> GetShiftsByWorkdayId(int Id)
        {
            List<Shift> sList = new List<Shift>();

            DetachedCriteria criteria = DetachedCriteria.For(typeof(WorkdayShift));
            criteria.Add(Expression.Eq("Workday.Id", Id));
            IList<WorkdayShift> wsList = criteriaMgrE.FindAll<WorkdayShift>(criteria);
            foreach (WorkdayShift ws in wsList)
            {
                sList.Add(ws.Shift);
            }
            return sList;
        }

        public WorkdayShift LoadWorkdayShift(int workdayId, string shiftCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(WorkdayShift));
            criteria.Add(Expression.Eq("Workday.Id", workdayId));
            criteria.Add(Expression.Eq("Shift.Code", shiftCode));
            IList<WorkdayShift> wsList = criteriaMgrE.FindAll<WorkdayShift>(criteria);
            if (wsList.Count == 0) return null;
            return wsList[0];
        }

        public void CreateWorkdayShifts(Workday workday, IList<Shift> sList)
        {
            foreach (Shift shift in sList)
            {
                WorkdayShift workdayShift = new WorkdayShift();
                workdayShift.Workday = workday;
                workdayShift.Shift = shift;
                entityDao.CreateWorkdayShift(workdayShift);
            }
        }

        public IList<WorkdayShift> GetWorkdayShiftsByWorkdayId(int workdayId)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(WorkdayShift));
            criteria.Add(Expression.Eq("Workday.Id", workdayId));
            IList workdayShifts = criteriaMgrE.FindAll(criteria);

            return IListHelper.ConvertToList<WorkdayShift>(workdayShifts);
        }

        #endregion Customized Methods
    }
}


#region Extend Class


namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class WorkdayShiftMgrE : com.Sconit.Service.MasterData.Impl.WorkdayShiftMgr, IWorkdayShiftMgrE
    {
        
    }
}
#endregion

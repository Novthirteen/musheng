using com.Sconit.Service.Ext.MasterData;


using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using NHibernate.Expression;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class WorkdayMgr : WorkdayBaseMgr, IWorkdayMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IWorkdayShiftMgrE WorkdayShiftMgrE { get; set; }
        

        #region Customized Methods

        public IList<Workday> GetWorkdayByDayofweekWizard(string dayofweek, string region, string workcenter)
        {
            IList<Workday> workdays = new List<Workday>();
            workdays = this.GetWorkdayByDayofweek(dayofweek, region, workcenter);
            if (workdays.Count > 0)
            {
                return workdays;
            }

            workcenter = null;
            workdays = this.GetWorkdayByDayofweek(dayofweek, region, workcenter);
            if (workdays.Count > 0)
            {
                return workdays;
            }

            region = null;
            workdays = this.GetWorkdayByDayofweek(dayofweek, region, workcenter);

            return workdays;
        }

        public IList<Workday> GetWorkdayByDayofweek(string dayofweek, string region, string workcenter)
        {
            IList<Workday> workdays = new List<Workday>();
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Workday));
            criteria.Add(Expression.Eq("DayOfWeek", dayofweek));
            if (workcenter == null || workcenter.Trim() == "")
            {
                criteria.Add(Expression.IsNull("WorkCenter.Code"));
            }
            else
            {
                criteria.Add(Expression.Eq("WorkCenter.Code", workcenter));
            }
            if (region == null || region.Trim() == "")
            {
                criteria.Add(Expression.IsNull("Region.Code"));
            }
            else
            {
                criteria.Add(Expression.Eq("Region.Code", region));
            }


            return criteriaMgrE.FindAll<Workday>(criteria);
        }

        //result:
        //1) no records, it's workday
        //2) 1 row, it's normal result, return by this type
        //3) more rows, it's abnormal result, only all rest will return rest
        public bool CheckWorkdayByDayofweek(string dayofweek, string region, string workcenter)
        {
            bool isWorkday = true;
            IList<Workday> workdays = this.GetWorkdayByDayofweekWizard(dayofweek, region, workcenter);
            if (workdays.Count > 0)
            {
                foreach (Workday workday in workdays)
                {
                    isWorkday = false;
                    if (workday.Type == BusinessConstants.CODE_MASTER_WORKCALENDAR_TYPE_VALUE_WORK)
                    {
                        isWorkday = true;
                        break;
                    }
                }
            }

            return isWorkday;
        }

        public void DeleteWorkday(int Id, bool deleteWorkdayShift)
        {
            if (deleteWorkdayShift)
            {
                IList<WorkdayShift> workdayShifts = WorkdayShiftMgrE.GetWorkdayShiftsByWorkdayId(Id);
                WorkdayShiftMgrE.DeleteWorkdayShift(workdayShifts);
            }

            this.DeleteWorkday(Id);
        }

        #endregion Customized Methods
    }
}


#region Extend Class


namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class WorkdayMgrE : com.Sconit.Service.MasterData.Impl.WorkdayMgr, IWorkdayMgrE
    {
        
    }
}
#endregion

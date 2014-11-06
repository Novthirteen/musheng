using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using com.Sconit.Entity.Exception;
using System.Linq;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class FinanceCalendarMgr : FinanceCalendarBaseMgr, IFinanceCalendarMgr
    {
        public ICriteriaMgrE criteriaMgr { get; set; }

        #region Customized Methods

        public FinanceCalendar GetLastestOpenFinanceCalendar()
        {
            DetachedCriteria criteria = DetachedCriteria.For<FinanceCalendar>();
            criteria.Add(Expression.Eq("IsClosed", false));
            criteria.AddOrder(Order.Asc("StartDate"));
            IList<FinanceCalendar> financeCalendarList = this.criteriaMgr.FindAll<FinanceCalendar>(criteria, 0, 1);
            if (financeCalendarList != null && financeCalendarList.Count > 0)
            {
                return financeCalendarList[0];
            }
            else
            {
                throw new BusinessErrorException("FinanceCalendar.Error.OpenFinanceCalendarNotFind");
            }
        }

        public FinanceCalendar GetFinanceCalendar(Int32 year, Int32 month)
        {
            DetachedCriteria criteria = DetachedCriteria.For<FinanceCalendar>();
            criteria.Add(Expression.Eq("FinanceYear", year));
            criteria.Add(Expression.Eq("FinanceMonth", month));
            IList<FinanceCalendar> financeCalendarList = this.criteriaMgr.FindAll<FinanceCalendar>(criteria);
            if (financeCalendarList != null && financeCalendarList.Count > 0)
            {
                return financeCalendarList[0];
            }
            else
            {
                return null;
            }
        }

        public FinanceCalendar GetFinanceCalendar(Int32 year, Int32 month, int interval)//ͨ����ǰ����½��мӼ��õ���ȥ��δ���Ļ����
        {
            DetachedCriteria criteria = DetachedCriteria.For<FinanceCalendar>();

            criteria.AddOrder(Order.Asc("FinanceYear"));
            criteria.AddOrder(Order.Asc("FinanceMonth"));
            IList<FinanceCalendar> financeCalendarList = this.criteriaMgr.FindAll<FinanceCalendar>(criteria);

            FinanceCalendar fc = this.GetFinanceCalendar(year, month);
            int seq = 0;
            if (fc != null)
                seq = financeCalendarList.IndexOf(this.GetFinanceCalendar(year, month));
            if (seq + interval >= 0 && seq + interval < financeCalendarList.Count)
            {
                return financeCalendarList[seq + interval];
            }
            else
            {
                return null;
            }
        }

        #endregion Customized Methods
    }
}

#region Extend Class
namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class FinanceCalendarMgrE : com.Sconit.Service.MasterData.Impl.FinanceCalendarMgr, IFinanceCalendarMgrE
    {

    }
}
#endregion
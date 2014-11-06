using System;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IFinanceCalendarMgr : IFinanceCalendarBaseMgr
    {
        #region Customized Methods

        FinanceCalendar GetLastestOpenFinanceCalendar();

        FinanceCalendar GetFinanceCalendar(Int32 year, Int32 month);

        FinanceCalendar GetFinanceCalendar(Int32 year, Int32 month, int interval);

        #endregion Customized Methods
    }
}

#region Extend Interface
namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IFinanceCalendarMgrE : com.Sconit.Service.MasterData.IFinanceCalendarMgr
    {

    }
}
#endregion
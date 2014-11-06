using com.Sconit.Service.Ext.MasterData;
using System.Collections;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IWorkdayMgr : IWorkdayBaseMgr
    {
        #region Customized Methods

        IList<Workday> GetWorkdayByDayofweekWizard(string dayofweek, string region, string workcenter);

        IList<Workday> GetWorkdayByDayofweek(string dayofweek, string region, string workcenter);

        bool CheckWorkdayByDayofweek(string dayofweek, string region, string workcenter);

        void DeleteWorkday(int Id, bool deleteWorkdayShift);

        #endregion Customized Methods
    }
}





#region Extend Interface






namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IWorkdayMgrE : com.Sconit.Service.MasterData.IWorkdayMgr
    {
        
    }
}

#endregion

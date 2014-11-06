using com.Sconit.Service.Ext.MasterData;
using System.Collections;
using System;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IShiftMgr : IShiftBaseMgr
    {
        #region Customized Methods

        IList GetShiftWizard(string region, string workcenter);

        IList GetShift(string region, string workcenter);

        DateTime GetShiftStartTime(DateTime date, string code);

        DateTime GetShiftStartTime(DateTime date, Shift shift);

        DateTime GetShiftEndTime(DateTime date, string code);

        DateTime GetShiftEndTime(DateTime date, Shift shift);

        Shift LoadShift(string code, DateTime date);

        IList<Shift> GetRegionShift(string region);

        Shift GetDefaultShift();

        #endregion Customized Methods
    }
}





#region Extend Interface






namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IShiftMgrE : com.Sconit.Service.MasterData.IShiftMgr
    {
        
    }
}

#endregion

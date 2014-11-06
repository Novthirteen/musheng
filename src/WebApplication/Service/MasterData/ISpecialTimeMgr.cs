using com.Sconit.Service.Ext.MasterData;
using System;
using System.Collections;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface ISpecialTimeMgr : ISpecialTimeBaseMgr
    {
        #region Customized Methods

        IList GetReferSpecialTimeWizard(DateTime starttime, DateTime endtime, string region, string workcenter);

        IList GetSpecialTime(DateTime starttime, DateTime endtime, string region, string workcenter);

        #endregion Customized Methods
    }
}





#region Extend Interface




namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface ISpecialTimeMgrE : com.Sconit.Service.MasterData.ISpecialTimeMgr
    {
        
    }
}

#endregion

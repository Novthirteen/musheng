using com.Sconit.Service.Ext.MasterData;
using System;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IShiftDetailMgr : IShiftDetailBaseMgr
    {
        #region Customized Methods

        IList<ShiftDetail> GetShiftDetail(string shiftCode, DateTime date);

        #endregion Customized Methods
    }
}





#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IShiftDetailMgrE : com.Sconit.Service.MasterData.IShiftDetailMgr
    {
        
    }
}

#endregion

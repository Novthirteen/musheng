using com.Sconit.Service.Ext.MasterData;
using System;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IOrderPlannedBackflushMgr : IOrderPlannedBackflushBaseMgr
    {
        #region Customized Methods

        IList<OrderPlannedBackflush> GetActiveOrderPlannedBackflush(string prodLineCode, string[] items);

        #endregion Customized Methods
    }
}




#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IOrderPlannedBackflushMgrE : com.Sconit.Service.MasterData.IOrderPlannedBackflushMgr
    {
        
    }
}

#endregion

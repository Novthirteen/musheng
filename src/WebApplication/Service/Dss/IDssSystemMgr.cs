using System;
using com.Sconit.Entity.Dss;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Dss
{
    public interface IDssSystemMgr : IDssSystemBaseMgr
    {
        #region Customized Methods

       

        #endregion Customized Methods
    }
}



#region Extend Interface




namespace com.Sconit.Service.Ext.Dss
{
    public partial interface IDssSystemMgrE : com.Sconit.Service.Dss.IDssSystemMgr
    {
        
    }
}

#endregion

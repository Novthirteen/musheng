using System;
using com.Sconit.Entity.Dss;
using System.Collections.Generic;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Dss
{
    public interface IDssInboundControlMgr : IDssInboundControlBaseMgr
    {
        #region Customized Methods

        IList<DssInboundControl> GetDssInboundControl();

        #endregion Customized Methods
    }
}



#region Extend Interface





namespace com.Sconit.Service.Ext.Dss
{
    public partial interface IDssInboundControlMgrE : com.Sconit.Service.Dss.IDssInboundControlMgr
    {
        
    }
}

#endregion

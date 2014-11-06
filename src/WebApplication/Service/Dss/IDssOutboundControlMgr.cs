using System;
using com.Sconit.Entity.Dss;
using System.Collections.Generic;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Dss
{
    public interface IDssOutboundControlMgr : IDssOutboundControlBaseMgr
    {
        #region Customized Methods

        IList<DssOutboundControl> GetDssOutboundControl();

        #endregion Customized Methods
    }
}



#region Extend Interface





namespace com.Sconit.Service.Ext.Dss
{
    public partial interface IDssOutboundControlMgrE : com.Sconit.Service.Dss.IDssOutboundControlMgr
    {
        
    }
}

#endregion

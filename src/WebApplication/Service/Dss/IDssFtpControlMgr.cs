using System;
using com.Sconit.Entity.Dss;
using System.Collections.Generic;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Dss
{
    public interface IDssFtpControlMgr : IDssFtpControlBaseMgr
    {
        #region Customized Methods

        IList<DssFtpControl> GetDssFtpControl(string IOType);

        #endregion Customized Methods
    }
}



#region Extend Interface





namespace com.Sconit.Service.Ext.Dss
{
    public partial interface IDssFtpControlMgrE : com.Sconit.Service.Dss.IDssFtpControlMgr
    {
        
    }
}

#endregion

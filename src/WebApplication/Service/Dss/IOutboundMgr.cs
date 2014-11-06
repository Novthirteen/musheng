using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.Dss;

namespace com.Sconit.Service.Dss
{
    public interface IOutboundMgr
    {
        void ProcessOutbound(DssOutboundControl dssOutboundControl);
    }
}



#region Extend Interface





namespace com.Sconit.Service.Ext.Dss
{
    public partial interface IOutboundMgrE : com.Sconit.Service.Dss.IOutboundMgr
    {
       
    }
}

#endregion

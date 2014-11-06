using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.Dss;

namespace com.Sconit.Service.Dss
{
    public interface IInboundMgr
    {
        void ProcessInboundFile(DssInboundControl dssInboundControl, string[] files);
    }
}



#region Extend Interface





namespace com.Sconit.Service.Ext.Dss
{
    public partial interface IInboundMgrE : com.Sconit.Service.Dss.IInboundMgr
    {
        
    }
}

#endregion

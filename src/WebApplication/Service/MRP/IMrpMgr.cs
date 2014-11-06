using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;

namespace com.Sconit.Service.MRP
{
    public interface IMrpMgr
    {
        void RunMrp(User user);

        void RunMrp(DateTime effectiveDate, User user);
    }
}

#region Extend Interface

namespace com.Sconit.Service.Ext.MRP
{
    public partial interface IMrpMgrE : com.Sconit.Service.MRP.IMrpMgr
    {
    }
}

#endregion Extend Interface

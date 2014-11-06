using com.Sconit.Service.Ext.Dss;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.Dss;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using com.Sconit.Entity.Dss;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Dss.Impl
{
    [Transactional]
    public class DssSystemMgr : DssSystemBaseMgr, IDssSystemMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        
		

        #region Customized Methods
        
        #endregion Customized Methods
    }
}



#region Extend Class


namespace com.Sconit.Service.Ext.Dss.Impl
{
    [Transactional]
    public partial class DssSystemMgrE : com.Sconit.Service.Dss.Impl.DssSystemMgr, IDssSystemMgrE
    {
        
    }
}
#endregion

using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class FlowPlanMgr : FlowPlanBaseMgr, IFlowPlanMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IFlowDetailMgrE flowDetailMgrE { get; set; }



        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Class
namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class FlowPlanMgrE : com.Sconit.Service.MasterData.Impl.FlowPlanMgr, IFlowPlanMgrE
    {
        
    }
}
#endregion

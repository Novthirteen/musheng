using System;
using System.Collections.Generic;
using com.Sconit.Entity.View;

//TODO: Add other using statements here.

namespace com.Sconit.Service.View
{
    public interface IFlowViewMgr : IFlowViewBaseMgr
    {
        #region Customized Methods

        IList<FlowView> GetFlowView(string flowCode);

        #endregion Customized Methods
    }
}



#region Extend Interface



namespace com.Sconit.Service.Ext.View
{
    public partial interface IFlowViewMgrE : com.Sconit.Service.View.IFlowViewMgr
    {
        
    }
}

#endregion

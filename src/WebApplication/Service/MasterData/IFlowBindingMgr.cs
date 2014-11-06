using com.Sconit.Service.Ext.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IFlowBindingMgr : IFlowBindingBaseMgr
    {
        #region Customized Methods

        FlowBinding LoadFlowBinding(string flowCode, string slaveFlowCode);

        IList<FlowBinding> GetFlowBinding(string flowCode);

        IList<FlowBinding> GetFlowBinding(string flowCode, params string[] bindingTypes);

        IList<FlowDetail> GetBindedFlowDetail(int orderDetailId, string slaveFlowCode);

        IList<FlowDetail> GetBindedFlowDetail(OrderDetail orderDetail, string slaveFlowCode);

        #endregion Customized Methods
    }
}





#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IFlowBindingMgrE : com.Sconit.Service.MasterData.IFlowBindingMgr
    {
        
    }
}

#endregion

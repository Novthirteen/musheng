using com.Sconit.Service.Ext.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IOrderBindingMgr : IOrderBindingBaseMgr
    {
        #region Customized Methods

        OrderBinding CreateOrderBinding(OrderHead order, Flow bindedFlow, string bindingType);

        IList<OrderBinding> GetOrderBinding(OrderHead order, params string[] bindingTypes);

        IList<OrderBinding> GetOrderBinding(string orderNo, params string[] bindingTypes);

        #endregion Customized Methods
    }
}



#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IOrderBindingMgrE : com.Sconit.Service.MasterData.IOrderBindingMgr
    {
        
    }
}

#endregion

#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IOrderBindingMgrE : com.Sconit.Service.MasterData.IOrderBindingMgr
    {
        
    }
}

#endregion

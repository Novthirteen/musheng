using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MRP
{
    public interface IExpectTransitInventoryMgr : IExpectTransitInventoryBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.MRP
{
    public partial interface IExpectTransitInventoryMgrE : com.Sconit.Service.MRP.IExpectTransitInventoryMgr
    {
    }
}

#endregion Extend Interface
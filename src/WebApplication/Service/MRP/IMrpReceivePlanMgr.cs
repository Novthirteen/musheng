using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MRP
{
    public interface IMrpReceivePlanMgr : IMrpReceivePlanBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.MRP
{
    public partial interface IMrpReceivePlanMgrE : com.Sconit.Service.MRP.IMrpReceivePlanMgr
    {
    }
}

#endregion Extend Interface
using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.View
{
    public interface IPlannedBillViewMgr : IPlannedBillViewBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}



#region Extend Interface



namespace com.Sconit.Service.Ext.View
{
    public partial interface IPlannedBillViewMgrE : com.Sconit.Service.View.IPlannedBillViewMgr
    {
        
    }
}

#endregion

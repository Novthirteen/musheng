using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Cost
{
    public interface ICostAllocateMethodMgr : ICostAllocateMethodBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Interface

namespace com.Sconit.Service.Ext.Cost
{
    public partial interface ICostAllocateMethodMgrE : com.Sconit.Service.Cost.ICostAllocateMethodMgr
    {
    }
}

#endregion Extend Interface
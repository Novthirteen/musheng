
//TODO: Add other using statements here.

namespace com.Sconit.Service.Distribution
{
    public interface ICustomerRollingPlanMgr : ICustomerRollingPlanBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}



#region Extend Interface



namespace com.Sconit.Service.Ext.Distribution
{
    public partial interface ICustomerRollingPlanMgrE : com.Sconit.Service.Distribution.ICustomerRollingPlanMgr
    {
       
    }
}

#endregion

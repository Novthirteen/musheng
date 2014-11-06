using Castle.Services.Transaction;
using com.Sconit.Persistence.Distribution;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Distribution.Impl
{
    [Transactional]
    public class CustomerRollingPlanMgr : CustomerRollingPlanBaseMgr, ICustomerRollingPlanMgr
    {
        

        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}



#region Extend Class





namespace com.Sconit.Service.Ext.Distribution.Impl
{
    [Transactional]
    public partial class CustomerRollingPlanMgrE : com.Sconit.Service.Distribution.Impl.CustomerRollingPlanMgr, ICustomerRollingPlanMgrE
    {
        
    }
}
#endregion

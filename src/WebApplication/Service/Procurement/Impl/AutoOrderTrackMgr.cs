using com.Sconit.Service.Ext.Procurement;

using Castle.Services.Transaction;
using com.Sconit.Persistence.Procurement;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Procurement.Impl
{
    [Transactional]
    public class AutoOrderTrackMgr : AutoOrderTrackBaseMgr, IAutoOrderTrackMgr
    {
        

        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Class



namespace com.Sconit.Service.Ext.Procurement.Impl
{
    [Transactional]
    public partial class AutoOrderTrackMgrE : com.Sconit.Service.Procurement.Impl.AutoOrderTrackMgr, IAutoOrderTrackMgrE
    {
       
    }
}
#endregion

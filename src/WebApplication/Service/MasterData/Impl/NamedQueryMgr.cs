using com.Sconit.Service.Ext.MasterData;


using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class NamedQueryMgr : NamedQueryBaseMgr, INamedQueryMgr
    {
        

        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}


#region Extend Class





namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class NamedQueryMgrE : com.Sconit.Service.MasterData.Impl.NamedQueryMgr, INamedQueryMgrE
    {
       
    }
}
#endregion

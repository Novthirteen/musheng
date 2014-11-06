using com.Sconit.Service.Ext.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface INamedQueryMgr : INamedQueryBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}





#region Extend Interface



namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface INamedQueryMgrE : com.Sconit.Service.MasterData.INamedQueryMgr
    {
        
    }
}

#endregion

using com.Sconit.Service.Ext.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IRoleMgr : IRoleBaseMgr
    {
        #region Customized Methods


        #endregion Customized Methods
    }
}





#region Extend Interface



namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IRoleMgrE : com.Sconit.Service.MasterData.IRoleMgr
    {
        
    }
}

#endregion

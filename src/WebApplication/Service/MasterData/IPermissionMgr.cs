using com.Sconit.Service.Ext.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IPermissionMgr : IPermissionBaseMgr
    {
        #region Customized Methods

        Permission GetPermission(string code);

        IList<Permission> GetALlPermissionsByCategory(string categoryCode);

        IList<Permission> GetALlPermissionsByCategory(string categoryCode, User user);

        void DeletePermission(string code);

        #endregion Customized Methods
    }
}



#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IPermissionMgrE : com.Sconit.Service.MasterData.IPermissionMgr
    {
        
    }
}

#endregion

#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IPermissionMgrE : com.Sconit.Service.MasterData.IPermissionMgr
    {
        
    }
}

#endregion

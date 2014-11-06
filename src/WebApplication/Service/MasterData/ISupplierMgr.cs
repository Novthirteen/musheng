using com.Sconit.Service.Ext.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;
using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface ISupplierMgr : ISupplierBaseMgr
    {
        #region Customized Methods

        IList<Supplier> GetSupplier(string userCode, bool includeInactive);

        IList<Supplier> GetSupplier(string userCode);

        void CreateSupplier(Supplier entity, User currentUser);

        #endregion Customized Methods
    }
}





#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface ISupplierMgrE : com.Sconit.Service.MasterData.ISupplierMgr
    {
        
    }
}

#endregion

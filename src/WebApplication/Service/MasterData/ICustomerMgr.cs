using com.Sconit.Service.Ext.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;
using System;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface ICustomerMgr : ICustomerBaseMgr
    {
        #region Customized Methods

        IList<Customer> GetCustomer(string userCode, bool includeInactive);

        IList<Customer> GetCustomer(string userCode);

        void CreateCustomer(Customer entity, User currentUser);

        #endregion Customized Methods
    }
}





#region Extend Interface


namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface ICustomerMgrE : com.Sconit.Service.MasterData.ICustomerMgr
    {
        
    }
}

#endregion

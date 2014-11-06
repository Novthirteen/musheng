using com.Sconit.Service.Ext.MasterData;
using System;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IClientOrderDetailMgr : IClientOrderDetailBaseMgr
    {
        #region Customized Methods

        IList<ClientOrderDetail> GetAllClientOrderDetail(string OrderHeadId);

        #endregion Customized Methods
    }
}





#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IClientOrderDetailMgrE : com.Sconit.Service.MasterData.IClientOrderDetailMgr
    {
        
    }
}

#endregion

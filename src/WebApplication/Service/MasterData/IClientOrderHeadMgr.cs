using com.Sconit.Service.Ext.MasterData;
using System;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData
{
    public interface IClientOrderHeadMgr : IClientOrderHeadBaseMgr
    {
        #region Customized Methods

        //TODO: Add other methods here.
        void CreateClientOrderHead(ClientOrderHead clientOrderHead, IList<ClientOrderDetail> clientOrderDetailList, IList<ClientWorkingHours> clientWorkingHoursList);

        #endregion Customized Methods
    }
}





#region Extend Interface





namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface IClientOrderHeadMgrE : com.Sconit.Service.MasterData.IClientOrderHeadMgr
    {
        
    }
}

#endregion

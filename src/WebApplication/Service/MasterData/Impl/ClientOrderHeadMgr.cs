using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class ClientOrderHeadMgr : ClientOrderHeadBaseMgr, IClientOrderHeadMgr
    {
        public IClientOrderDetailMgrE clientOrderDetailMgrE { get; set; }
        public IClientWorkingHoursMgrE clientWorkingHoursMgrE { get; set; }

        

        #region Customized Methods

        //TODO: Add other methods here.
        [Transaction(TransactionMode.Unspecified)]
        public void CreateClientOrderHead(ClientOrderHead clientOrderHead, IList<ClientOrderDetail> clientOrderDetailList, IList<ClientWorkingHours> clientWorkingHoursList)
        {
            CreateClientOrderHead(clientOrderHead);
            foreach (ClientOrderDetail clientOrderDetail in clientOrderDetailList)
            {
                clientOrderDetailMgrE.CreateClientOrderDetail(clientOrderDetail);
            }
            if (clientWorkingHoursList!=null && clientWorkingHoursList.Count>0)
            {
                foreach (ClientWorkingHours clientWorkingHours in clientWorkingHoursList)
                {
                    clientWorkingHoursMgrE.CreateClientWorkingHours(clientWorkingHours);
                }
            }
        }

        #endregion Customized Methods
    }
}


#region Extend Class
namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class ClientOrderHeadMgrE : com.Sconit.Service.MasterData.Impl.ClientOrderHeadMgr, IClientOrderHeadMgrE
    {
       
    }
}
#endregion

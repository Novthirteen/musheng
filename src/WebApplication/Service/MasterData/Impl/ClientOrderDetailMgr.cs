using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using NHibernate.Expression;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class ClientOrderDetailMgr : ClientOrderDetailBaseMgr, IClientOrderDetailMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        

        #region Customized Methods

        public IList<ClientOrderDetail> GetAllClientOrderDetail(string OrderHeadId) 
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(ClientOrderDetail));
            criteria.Add(Expression.Eq("ClientOrderHead.Id", OrderHeadId));
            IList<ClientOrderDetail> clientOrderDetailList = criteriaMgrE.FindAll<ClientOrderDetail>(criteria);
            if (clientOrderDetailList.Count > 0)
            {
                return clientOrderDetailList;
            }
            return null;
        }

        #endregion Customized Methods
    }
}


#region Extend Class


namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class ClientOrderDetailMgrE : com.Sconit.Service.MasterData.Impl.ClientOrderDetailMgr, IClientOrderDetailMgrE
    {
       
    }
}
#endregion

using com.Sconit.Service.Ext.MasterData;


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Entity.MasterData;
using NHibernate.Expression;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class ClientWorkingHoursMgr : ClientWorkingHoursBaseMgr, IClientWorkingHoursMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        

        #region Customized Methods

        public IList<ClientWorkingHours> GetAllClientWorkingHours(string OrderHeadId)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(ClientWorkingHours));
            criteria.Add(Expression.Eq("ClientOrderHead.Id", OrderHeadId));
            IList<ClientWorkingHours> clientWorkingHoursList = criteriaMgrE.FindAll<ClientWorkingHours>(criteria);
            if (clientWorkingHoursList.Count > 0)
            {
                return clientWorkingHoursList;
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
    public partial class ClientWorkingHoursMgrE : com.Sconit.Service.MasterData.Impl.ClientWorkingHoursMgr, IClientWorkingHoursMgrE
    {
       
    }
}
#endregion

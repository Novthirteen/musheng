using com.Sconit.Service.Ext.MasterData;


using Castle.Services.Transaction;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;
using NHibernate;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using NHibernate.Expression;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class RoutingMgr : RoutingBaseMgr, IRoutingMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
       

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public Routing CheckAndLoadRouting(string routingCode)
        {
            Routing routing = this.LoadRouting(routingCode);
            if (routing == null)
            {
                throw new BusinessErrorException("Routing.Error.RoutingCodeNotExist", routingCode);
            }

            return routing;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Routing> GetRouting(string type)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Routing));
            if (type == BusinessConstants.CODE_MASTER_ROUTING_TYPE_VALUE_PRODUCTION)
            {
                string[] productionType = new string[]{BusinessConstants.CODE_MASTER_ROUTING_TYPE_VALUE_SELFSCHEDULE,BusinessConstants.CODE_MASTER_ROUTING_TYPE_VALUE_SINGLELABOUR,BusinessConstants.CODE_MASTER_ROUTING_TYPE_VALUE_STREAMLINE};
                criteria.Add(Expression.In("Type", productionType));
            }
            else
            {
                criteria.Add(Expression.Eq("Type", type));
            }
                return criteriaMgrE.FindAll<Routing>(criteria);
        }

        #endregion Customized Methods
    }
}


#region Extend Class



namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class RoutingMgrE : com.Sconit.Service.MasterData.Impl.RoutingMgr, IRoutingMgrE
    {
       
    }
}
#endregion

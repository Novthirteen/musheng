using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Facilities.NHibernateIntegration;

namespace com.Sconit.Persistence.MRP.NH
{
    public class NHOrderProductionPlanDao:NHOrderProductionPlanBaseDao,IOrderProductionPlanDao
    {
        public NHOrderProductionPlanDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }
    }
}

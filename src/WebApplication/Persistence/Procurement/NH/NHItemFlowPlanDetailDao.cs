using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Facilities.NHibernateIntegration;

//TODO: Add other using statmens here.

namespace com.Sconit.Persistence.Procurement.NH
{
    public class NHItemFlowPlanDetailDao : NHItemFlowPlanDetailBaseDao, IItemFlowPlanDetailDao
    {
        public NHItemFlowPlanDetailDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}
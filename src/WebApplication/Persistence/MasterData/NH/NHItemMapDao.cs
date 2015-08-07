using Castle.Facilities.NHibernateIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Sconit.Persistence.MasterData.NH
{
    public class NHItemMapDao : NHItemMapBaseDao, IItemMapDao
    {
         public NHItemMapDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }
    }
}

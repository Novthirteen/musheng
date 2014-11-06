using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Facilities.NHibernateIntegration;

//TODO: Add other using statmens here.

namespace com.Sconit.Persistence.Customize.NH
{
    public class NHProdLineIp2Dao : NHProdLineIp2BaseDao, IProdLineIp2Dao
    {
        public NHProdLineIp2Dao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        #region Customized Methods

        //TODO: Add other methods here.

        #endregion Customized Methods
    }
}

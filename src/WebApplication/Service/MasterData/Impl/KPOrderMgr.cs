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
    public class KPOrderMgr : KPOrderBaseMgr, IKPOrderMgr
    {
        

        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public KPOrder LoadKPOrder(decimal orderId, bool includeDetail)
        {
            KPOrder kpOrder = this.LoadKPOrder(orderId);

            if (includeDetail && kpOrder.KPItems != null && kpOrder.KPItems.Count > 0)
            {
               
            }

            return kpOrder;
        }
        #endregion Customized Methods
    }
}


#region Extend Class
namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class KPOrderMgrE : com.Sconit.Service.MasterData.Impl.KPOrderMgr, IKPOrderMgrE
    {
        
    }
}
#endregion

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
    public class ReceiptInProcessLocationMgr : ReceiptInProcessLocationBaseMgr, IReceiptInProcessLocationMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        

        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public IList<ReceiptInProcessLocation> GetReceiptInProcessLocation(string ipNo, string receiptNo)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(ReceiptInProcessLocation));
            if (ipNo != null && ipNo.Trim() != string.Empty)
                criteria.Add(Expression.Eq("InProcessLocation.IpNo", ipNo));
            if (receiptNo != null && receiptNo.Trim() != string.Empty)
                criteria.Add(Expression.Eq("Receipt.ReceiptNo", receiptNo));

            return criteriaMgrE.FindAll<ReceiptInProcessLocation>(criteria);
        }

        #endregion Customized Methods
    }
}


#region Extend Class


namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class ReceiptInProcessLocationMgrE : com.Sconit.Service.MasterData.Impl.ReceiptInProcessLocationMgr, IReceiptInProcessLocationMgrE
    {
        
    }
}
#endregion

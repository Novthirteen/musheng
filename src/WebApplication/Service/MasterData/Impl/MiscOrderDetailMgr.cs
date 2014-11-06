using com.Sconit.Service.Ext.MasterData;


using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using NHibernate.Expression;
//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class MiscOrderDetailMgr : MiscOrderDetailBaseMgr, IMiscOrderDetailMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        

        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public IList<MiscOrderDetail> GetMiscOrderDetail(string orderNo)
        {
            DetachedCriteria criteria = DetachedCriteria.For<MiscOrderDetail>();
            criteria.Add(Expression.Eq("MiscOrder.OrderNo", orderNo));
            return this.criteriaMgrE.FindAll<MiscOrderDetail>(criteria);
        }

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class MiscOrderDetailMgrE : com.Sconit.Service.MasterData.Impl.MiscOrderDetailMgr, IMiscOrderDetailMgrE
    {
        
    }
}
#endregion

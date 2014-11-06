using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Castle.Services.Transaction;
using com.Sconit.Persistence.View;
using com.Sconit.Entity.View;
using NHibernate.Expression;
using com.Sconit.Service.Ext.Criteria;

//TODO: Add other using statements here.

namespace com.Sconit.Service.View.Impl
{
    [Transactional]
    public class FlowViewMgr : FlowViewBaseMgr, IFlowViewMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        #region Customized Methods

        public IList<FlowView> GetFlowView(string flowCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(FlowView));
            if (flowCode != null && flowCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Flow.Code", flowCode));
            }
            return criteriaMgrE.FindAll<FlowView>(criteria);
        }

        #endregion Customized Methods
    }
}



#region Extend Class

namespace com.Sconit.Service.Ext.View.Impl
{
    [Transactional]
    public partial class FlowViewMgrE : com.Sconit.Service.View.Impl.FlowViewMgr, IFlowViewMgrE
    {

    }
}
#endregion

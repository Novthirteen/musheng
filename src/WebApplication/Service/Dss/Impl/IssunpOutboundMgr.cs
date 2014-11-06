using com.Sconit.Service.Ext.Dss;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using
 Castle.Services.Transaction;
using System.Collections;
using com.Sconit.Entity;
using NHibernate.Expression;
using com.Sconit.Entity.Dss;

namespace com.Sconit.Service.Dss.Impl
{
    public class IssunpOutboundMgr : UnpOutboundBaseMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public ICommonOutboundMgrE commonOutboundMgrE { get; set; }
		public IssunpOutboundMgr(INumberControlMgrE numberControlMgrE,
           IDssExportHistoryMgrE dssExportHistoryMgrE,
           ICriteriaMgrE criteriaMgrE,
           IDssOutboundControlMgrE dssOutboundControlMgrE,
           IDssObjectMappingMgrE dssObjectMappingMgrE,
            ICommonOutboundMgrE commonOutboundMgrE,
            IMiscOrderMgrE miscOrderMgrE)
            : base(numberControlMgrE, dssExportHistoryMgrE, criteriaMgrE, dssOutboundControlMgrE, dssObjectMappingMgrE, commonOutboundMgrE, miscOrderMgrE)
        {
            
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override IList<DssExportHistory> ExtractOutboundData(DssOutboundControl dssOutboundControl)
        {
            IList result = commonOutboundMgrE.ExtractOutboundDataFromLocationTransaction(dssOutboundControl,
                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_UNP, MatchMode.Exact);

            IList<DssExportHistory> list = this.ConvertList(result, dssOutboundControl);
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    item.Qty = -item.Qty;//修正数量
                }
            }
            return list;
        }
    }
}





#region Extend Interface


namespace com.Sconit.Service.Ext.Dss.Impl
{
    public partial class IssunpOutboundMgrE : com.Sconit.Service.Dss.Impl.IssunpOutboundMgr, IOutboundMgrE
    {
        public IssunpOutboundMgrE(INumberControlMgrE numberControlMgrE,
           IDssExportHistoryMgrE dssExportHistoryMgrE,
           ICriteriaMgrE criteriaMgrE,
           IDssOutboundControlMgrE dssOutboundControlMgrE,
           IDssObjectMappingMgrE dssObjectMappingMgrE,
            ICommonOutboundMgrE commonOutboundMgrE,
            IMiscOrderMgrE miscOrderMgrE)
            : base(numberControlMgrE, dssExportHistoryMgrE, criteriaMgrE, dssOutboundControlMgrE, dssObjectMappingMgrE, commonOutboundMgrE, miscOrderMgrE)
        {
            
        }
    }
}

#endregion

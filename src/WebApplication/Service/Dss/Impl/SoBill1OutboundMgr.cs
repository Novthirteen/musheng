using com.Sconit.Service.Ext.Dss;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using System.Collections;
using com.Sconit.Entity.MasterData;
using com.Sconit.Utility;
using com.Sconit.Entity.Dss;
using com.Sconit.Entity.Exception;

namespace com.Sconit.Service.Dss.Impl
{
    /// <summary>
    /// QAD:7.9.15
    /// </summary>
    public class SoBill1OutboundMgr : SoBillOutboundBaseMgr
    {


        public SoBill1OutboundMgr(INumberControlMgrE numberControlMgrE,
            IDssExportHistoryMgrE dssExportHistoryMgrE,
            ICriteriaMgrE criteriaMgrE,
            IDssOutboundControlMgrE dssOutboundControlMgrE,
            IDssObjectMappingMgrE dssObjectMappingMgrE,
            ICommonOutboundMgrE commonOutboundMgrE)
            : base(numberControlMgrE, dssExportHistoryMgrE, criteriaMgrE, dssOutboundControlMgrE, dssObjectMappingMgrE, commonOutboundMgrE)
        {
        }

        protected override object Serialize(object obj)
        {
            DssExportHistory dssExportHistory = (DssExportHistory)obj;
            DateTime effDate = dssExportHistory.EffectiveDate.HasValue ? dssExportHistory.EffectiveDate.Value : DateTime.Today;

            string[] line1 = new string[] 
            { 
                DssHelper.GetEventValue(dssExportHistory.EventCode),
                dssExportHistory.PartyTo,//客户
                DssHelper.FormatDate(effDate,dssExportHistory.DssOutboundControl.ExternalSystem.Code),//生效日期
                dssExportHistory.PartyFrom,//QAD:Site
                dssExportHistory.Item//零件号
            };

            string[] line2 = new string[]
            {
                DssHelper.GetEventValue(dssExportHistory.EventCode),
                dssExportHistory.Item,//零件号
                dssExportHistory.Qty.ToString("0.########"),//数量
                dssExportHistory.PartyFrom,//QAD:Site
                dssExportHistory.Location//客户库位
            };

            string[][] data = new string[][] { line1, line2 };

            return new object[] { effDate, data };
        }
    }
}




#region Extend Class





namespace com.Sconit.Service.Ext.Dss.Impl
{
    /// <summary>
    /// QAD:7.9.15
    /// </summary>
    public partial class SoBill1OutboundMgrE : com.Sconit.Service.Dss.Impl.SoBill1OutboundMgr
    {
        public SoBill1OutboundMgrE(INumberControlMgrE numberControlMgrE,
            IDssExportHistoryMgrE dssExportHistoryMgrE,
            ICriteriaMgrE criteriaMgrE,
            IDssOutboundControlMgrE dssOutboundControlMgrE,
            IDssObjectMappingMgrE dssObjectMappingMgrE,
            ICommonOutboundMgrE commonOutboundMgrE)
            : base(numberControlMgrE, dssExportHistoryMgrE, criteriaMgrE, dssOutboundControlMgrE, dssObjectMappingMgrE, commonOutboundMgrE)
        {
        }
    }
}

#endregion

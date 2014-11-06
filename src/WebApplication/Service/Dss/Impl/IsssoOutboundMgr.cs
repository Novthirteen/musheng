using com.Sconit.Service.Ext.Dss;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Entity.Dss;
using Castle.Services.Transaction;
using System.Collections;
using NHibernate.Expression;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Utility;
using com.Sconit.Entity.View;
using com.Sconit.Entity.Exception;
using com.Sconit.Service.Ext.Criteria;

namespace com.Sconit.Service.Dss.Impl
{
    public class IsssoOutboundMgr : AbstractOutboundMgr
    {
        
		public ICriteriaMgrE criteriaMgr { get; set; }
		public IDssObjectMappingMgr dssObjectMappingMgr { get; set; }
        public ICommonOutboundMgrE commonOutboundMgrE { get; set; }

        
		public IsssoOutboundMgr(INumberControlMgrE numberControlMgrE,
           IDssExportHistoryMgrE dssExportHistoryMgrE,
           ICriteriaMgrE criteriaMgrE,
           IDssOutboundControlMgrE dssOutboundControlMgrE,
           IDssObjectMappingMgrE dssObjectMappingMgrE )
            : base(numberControlMgrE, dssExportHistoryMgrE, criteriaMgrE, dssOutboundControlMgrE, dssObjectMappingMgrE)
        {
            
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override IList<DssExportHistory> ExtractOutboundData(DssOutboundControl dssOutboundControl)
        {
            IList result = commonOutboundMgrE.ExtractOutboundDataFromLocationTransaction(dssOutboundControl,
                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_SO, MatchMode.Start);

            return this.ConvertList(result, dssOutboundControl);
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override object GetOutboundData(DssExportHistory dssExportHistory)
        {
            if (dssExportHistory.ReferenceLocation == null || dssExportHistory.ReferenceLocation.Trim() == string.Empty)
            {
                throw new BusinessErrorException("目的库位为空");
            }
            if (dssExportHistory.Location == null || dssExportHistory.Location.Trim() == string.Empty)
            {
                throw new BusinessErrorException("来源库位为空");
            }

            return (object)dssExportHistory;
        }

        protected override object Serialize(object obj)
        {
            DssExportHistory dssExportHistory = (DssExportHistory)obj;
            DateTime effDate = dssExportHistory.EffectiveDate.HasValue ? dssExportHistory.EffectiveDate.Value : DateTime.Today;

            log.Debug("Now write data:" + dssExportHistory.Item + "," + dssExportHistory.ReferenceLocation + "," + dssExportHistory.Location + "," + dssExportHistory.Qty);

            string[] line1 = new string[] 
            { 
                DssHelper.GetEventValue(dssExportHistory.EventCode),
                dssExportHistory.Item,//零件号
                dssExportHistory.Qty.ToString("0.########"),//数量
                DssHelper.FormatDate(effDate,dssExportHistory.DssOutboundControl.ExternalSystem.Code),//生效日期
                dssExportHistory.KeyCode,//收货单号
                dssExportHistory.PartyFrom,//QAD:Site
                dssExportHistory.Location,//来源库位
                dssExportHistory.PartyFrom,//QAD:Site,销售对应QAD移库,所以取来源区域
                dssExportHistory.ReferenceLocation//目的库位
            };

            string[][] data = new string[][] { line1 };

            return new object[] { effDate, data };
        }

        #region Private Method
        public IList<DssExportHistory> ConvertList(IList list, DssOutboundControl dssOutboundControl)
        {
            IList<DssExportHistory> result = new List<DssExportHistory>();
            if (list != null && list.Count > 0)
            {
                foreach (object obj in list)
                {
                    DssExportHistory dssExportHistory = commonOutboundMgrE.ConvertLocationTransactionToDssExportHistory(obj, dssOutboundControl);

                    dssExportHistory.Qty = -dssExportHistory.Qty;//修正数量

                    dssExportHistory.KeyCode = dssExportHistory.OrderNo;//订单号
                    dssExportHistory.ReferenceLocation = dssOutboundControl.UndefinedString1;//客户库位

                    if (dssExportHistory.Location != null && dssExportHistory.ReferenceLocation != null &&
                        dssExportHistory.Location.Trim().ToUpper() == dssExportHistory.ReferenceLocation.Trim().ToUpper())
                    {
                        continue;
                    }
                    result.Add(dssExportHistory);
                }
            }

            return result;
        }
        #endregion
    }
}





#region Extend Interface



namespace com.Sconit.Service.Ext.Dss.Impl
{
    public partial class IsssoOutboundMgrE : com.Sconit.Service.Dss.Impl.IsssoOutboundMgr, IOutboundMgrE
    {
        public IsssoOutboundMgrE(INumberControlMgrE numberControlMgrE,
           IDssExportHistoryMgrE dssExportHistoryMgrE,
           ICriteriaMgrE criteriaMgrE,
           IDssOutboundControlMgrE dssOutboundControlMgrE,
           IDssObjectMappingMgrE dssObjectMappingMgrE,
            ICommonOutboundMgrE commonOutboundMgrE)
            : base(numberControlMgrE, dssExportHistoryMgrE, criteriaMgrE, dssOutboundControlMgrE, dssObjectMappingMgrE)
        {
            
        }
    }
}

#endregion

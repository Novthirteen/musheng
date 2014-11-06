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
using com.Sconit.Service.Ext.Criteria;

namespace com.Sconit.Service.Dss.Impl
{
    /// <summary>
    /// QAD:5.13.1
    /// </summary>
    public class PoBillTransactionOutboundMgr : AbstractOutboundMgr
    {

		public INumberControlMgrE numberControlMgrE { get; set; }
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IOrderHeadMgrE orderHeadMgrE { get; set; }
        public ICommonOutboundMgrE commonOutboundMgrE { get; set; }
        
        
		public PoBillTransactionOutboundMgr(INumberControlMgrE numberControlMgrE,
            IDssExportHistoryMgrE dssExportHistoryMgrE,
            ICriteriaMgrE criteriaMgrE,
            IDssOutboundControlMgrE dssOutboundControlMgrE,
            IDssObjectMappingMgrE dssObjectMappingMgrE)
            : base(numberControlMgrE, dssExportHistoryMgrE, criteriaMgrE, dssOutboundControlMgrE, dssObjectMappingMgrE)
        {
            
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override IList<DssExportHistory> ExtractOutboundData(DssOutboundControl dssOutboundControl)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(BillTransaction))
                .Add(Expression.Gt("Id", dssOutboundControl.Mark))
                .Add(Expression.Eq("TransactionType", BusinessConstants.BILL_TRANS_TYPE_PO));

            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.Max("Id"))
                .Add(Projections.GroupProperty("OrderNo"))
                .Add(Projections.GroupProperty("ReceiptNo"))
                .Add(Projections.GroupProperty("Item"))
                .Add(Projections.Sum("Qty"))
                .Add(Projections.GroupProperty("EffectiveDate"))
                .Add(Projections.GroupProperty("Party")));

            IList result = criteriaMgrE.FindAll(criteria);
            return this.ConvertList(result, dssOutboundControl);
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override object GetOutboundData(DssExportHistory dssExportHistory)
        {
            return (object)dssExportHistory;
        }

        protected override object Serialize(object obj)
        {
            DssExportHistory dssExportHistory = (DssExportHistory)obj;
            DateTime effDate = dssExportHistory.EffectiveDate.HasValue ? dssExportHistory.EffectiveDate.Value : DateTime.Today;

            string[] line1 = new string[] 
            { 
                DssHelper.GetEventValue(dssExportHistory.EventCode),
                dssExportHistory.PartyFrom,//日程单号
                dssExportHistory.PartyFrom,//供应商
                dssExportHistory.PartyTo,//QAD:Site
                DssHelper.FormatDate(effDate,dssExportHistory.DssOutboundControl.ExternalSystem.Code),//生效日期
                dssExportHistory.Item,//零件号
                dssExportHistory.KeyCode
            };

            string[] line2 = new string[]
            {
                DssHelper.GetEventValue(dssExportHistory.EventCode),
                dssExportHistory.Item,//零件号
                dssExportHistory.Qty.ToString("0.########"),//数量
                dssExportHistory.PartyTo,//QAD:Site
                dssExportHistory.Location,//目的库位
                string.Empty
            };

            string[][] data = new string[][] { line1, line2 };

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
                    DssExportHistory dssExportHistory = new DssExportHistory();

                    dssExportHistory.DssOutboundControl = dssOutboundControl;
                    dssExportHistory.EventCode = BusinessConstants.DSS_EVENT_CODE_CREATE;
                    dssExportHistory.IsActive = true;
                    dssExportHistory.CreateDate = DateTime.Now;

                    dssExportHistory.OriginalId = (int)((object[])obj)[0];
                    dssExportHistory.OrderNo = (string)((object[])obj)[1];
                    dssExportHistory.ReceiptNo = (string)((object[])obj)[2];
                    dssExportHistory.Item = (string)((object[])obj)[3];
                    dssExportHistory.Qty = (decimal)((object[])obj)[4];
                    dssExportHistory.EffectiveDate = (DateTime)((object[])obj)[5];
                    dssExportHistory.PartyFrom = (string)((object[])obj)[6];
                    this.GetLoctransInfo(dssExportHistory);

                    dssExportHistory.KeyCode = this.GetKeyCode(dssExportHistory, dssExportHistory.ReceiptNo);

                    result.Add(dssExportHistory);
                }
            }

            return result;
        }

        [Transaction(TransactionMode.Unspecified)]
        private string GetKeyCode(DssExportHistory dssExportHistory, string receiptNo)
        {
            if (dssExportHistory.DssOutboundControl.ExternalSystem.Code == BusinessConstants.DSS_SYSTEM_CODE_QAD)
            {
                //string prefix = receiptNo.Substring(0, 1);
                //if (dssExportHistory.DssOutboundControl.ExternalSystem.Prefix1 != null && dssExportHistory.DssOutboundControl.ExternalSystem.Prefix1.Trim() != string.Empty)
                //    prefix = dssExportHistory.DssOutboundControl.ExternalSystem.Prefix1;

                //string keyCode = prefix + dssExportHistory.ReceiptNo.Remove(0, receiptNo.Length - 5);
                //return numberControlMgrE.GenerateNumber(keyCode, 2);

                return this.GetIpNo(receiptNo);
            }
            else
            {
                return receiptNo;
            }
        }

        /// <summary>
        /// Location,Region
        /// </summary>
        /// <param name="dssExportHistory"></param>
        [Transaction(TransactionMode.Unspecified)]
        private void GetLoctransInfo(DssExportHistory dssExportHistory)
        {
            OrderLocationTransaction orderLocationTransaction =
                commonOutboundMgrE.GetOrderLocationTransactionInfo(dssExportHistory.OrderNo, dssExportHistory.Item, BusinessConstants.IO_TYPE_IN);

            if (orderLocationTransaction != null)
            {
                dssExportHistory.Location = orderLocationTransaction.Location != null ? orderLocationTransaction.Location.Code : null;//收货库位
                dssExportHistory.PartyTo = orderLocationTransaction.OrderDetail.OrderHead.PartyTo.Code;//收货区域
            }
        }

        //山寨方法,过几天就删
        private string GetIpNo(string receiptNo)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(ReceiptInProcessLocation));
            criteria.Add(Expression.Eq("Receipt.ReceiptNo", receiptNo));

            criteria.SetProjection(Projections.Distinct(Projections.Property("InProcessLocation.IpNo")));
            IList result = criteriaMgrE.FindAll(criteria);
            if (result != null && result.Count > 0)
            {
                return (string)result[0];
            }
            else
            {
                return receiptNo;
            }
        }
        #endregion
    }
}




#region Extend Class




namespace com.Sconit.Service.Ext.Dss.Impl
{
    /// <summary>
    /// QAD:5.13.1
    /// </summary>
    public partial class PoBillTransactionOutboundMgrE : com.Sconit.Service.Dss.Impl.PoBillTransactionOutboundMgr, IOutboundMgrE
    {
        public PoBillTransactionOutboundMgrE(INumberControlMgrE numberControlMgrE,
            IDssExportHistoryMgrE dssExportHistoryMgrE,
            ICriteriaMgrE criteriaMgrE,
            IDssOutboundControlMgrE dssOutboundControlMgrE,
            IDssObjectMappingMgrE dssObjectMappingMgrE)
            : base(numberControlMgrE, dssExportHistoryMgrE, criteriaMgrE, dssOutboundControlMgrE, dssObjectMappingMgrE)
        {
            
        }
    }
}

#endregion

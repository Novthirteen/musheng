﻿using com.Sconit.Service.Ext.Dss;

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
using com.Sconit.Service.Ext.Criteria;

namespace com.Sconit.Service.Dss.Impl
{
    public abstract class SoBillOutboundBaseMgr : AbstractOutboundMgr
    {
        public virtual ICriteriaMgrE criteriaMgrE { get; set; }
        public virtual ICommonOutboundMgrE commonOutboundMgrE { get; set; }
		public SoBillOutboundBaseMgr(INumberControlMgrE numberControlMgrE,
            IDssExportHistoryMgrE dssExportHistoryMgrE,
            ICriteriaMgrE criteriaMgrE,
            IDssOutboundControlMgrE dssOutboundControlMgrE,
            IDssObjectMappingMgrE dssObjectMappingMgrE,
            ICommonOutboundMgrE commonOutboundMgrE)
            : base(numberControlMgrE, dssExportHistoryMgrE, criteriaMgrE, dssOutboundControlMgrE, dssObjectMappingMgrE)
        {
            this.criteriaMgrE = criteriaMgrE;
            this.commonOutboundMgrE = commonOutboundMgrE;
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override IList<DssExportHistory> ExtractOutboundData(DssOutboundControl dssOutboundControl)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(BillDetail))
                .CreateAlias("Bill", "b")
                .Add(Expression.Gt("Id", dssOutboundControl.Mark))
                .Add(Expression.Eq("b.TransactionType", BusinessConstants.BILL_TRANS_TYPE_SO));
                //.Add(Expression.In("b.Status", new string[] { BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE, BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT }));

            IList<BillDetail> result = criteriaMgrE.FindAll<BillDetail>(criteria);
            return this.ConvertList(result, dssOutboundControl);
        }

        [Transaction(TransactionMode.Unspecified)]
        protected override object GetOutboundData(DssExportHistory dssExportHistory)
        {
            if (!commonOutboundMgrE.CheckBillStatusValid(dssExportHistory.DefinedString1))
            {
                throw new BusinessErrorException("状态不合法");
            }

            return (object)dssExportHistory;
        }

        //protected override object Serialize(object obj)
        //{
        //    throw new NotImplementedException();
        //}

        #region Private Method
        public IList<DssExportHistory> ConvertList(IList<BillDetail> list, DssOutboundControl dssOutboundControl)
        {
            IList<DssExportHistory> result = new List<DssExportHistory>();
            if (list != null && list.Count > 0)
            {
                foreach (BillDetail billDetail in list)
                {
                    DssExportHistory dssExportHistory = new DssExportHistory();

                    dssExportHistory.DssOutboundControl = dssOutboundControl;
                    dssExportHistory.EventCode = BusinessConstants.DSS_EVENT_CODE_CREATE;
                    dssExportHistory.IsActive = true;
                    dssExportHistory.CreateDate = DateTime.Now;

                    dssExportHistory.OriginalId = billDetail.Id;
                    dssExportHistory.OrderNo = billDetail.ActingBill.OrderNo;
                    dssExportHistory.ReceiptNo = billDetail.ActingBill.ReceiptNo;
                    dssExportHistory.Item = billDetail.ActingBill.Item.Code;
                    dssExportHistory.Location = dssExportHistory.DssOutboundControl.UndefinedString1;//客户库位
                    dssExportHistory.Qty = billDetail.BilledQty;
                    dssExportHistory.EffectiveDate = billDetail.Bill.CreateDate.Date;
                    dssExportHistory.PartyTo = billDetail.Bill.BillAddress.Party.Code;//客户

                    dssExportHistory.DefinedString1 = billDetail.Bill.BillNo;//开票通知单号

                    dssExportHistory.KeyCode = DssHelper.GetBillKeyCode(dssExportHistory, billDetail.Bill.BillNo);

                    this.GetLoctransInfo(dssExportHistory);
                    result.Add(dssExportHistory);
                }
            }

            return result;
        }

        /// <summary>
        /// Location,Region
        /// </summary>
        /// <param name="dssExportHistory"></param>
        [Transaction(TransactionMode.Unspecified)]
        private void GetLoctransInfo(DssExportHistory dssExportHistory)
        {
            OrderLocationTransaction orderLocationTransaction =
                commonOutboundMgrE.GetOrderLocationTransactionInfo(dssExportHistory.OrderNo, dssExportHistory.Item, BusinessConstants.IO_TYPE_OUT);

            if (orderLocationTransaction != null)
            {
                //dssExportHistory.Location = orderLocationTransaction.Location != null ? orderLocationTransaction.Location.Code : null;//发出库位
                dssExportHistory.PartyFrom = orderLocationTransaction.OrderDetail.OrderHead.PartyFrom.Code;//发出区域
            }
        }
        #endregion
    }
}




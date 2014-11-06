using com.Sconit.Service.Ext.Dss;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Services.Transaction;
using NHibernate.Expression;
using System.Collections;
using com.Sconit.Entity;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Dss;

namespace com.Sconit.Service.Dss.Impl
{
    public class CommonOutboundMgr : ICommonOutboundMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }

       

        [Transaction(TransactionMode.Unspecified)]
        public bool CheckBillStatusValid(string billNo)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Bill))
                .Add(Expression.Eq("BillNo", billNo));

            criteria.SetProjection(Projections.Distinct(Projections.Property("Status")));

            IList result = criteriaMgrE.FindAll(criteria);
            if (result != null && result.Count > 0)
            {
                string status = (string)result[0];
                if (status == BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT || status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Location,Region
        /// </summary>
        /// <param name="dssExportHistory"></param>
        [Transaction(TransactionMode.Unspecified)]
        public OrderLocationTransaction GetOrderLocationTransactionInfo(string orderNo, string itemCode, string ioType)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderLocationTransaction));
            criteria.CreateAlias("OrderDetail", "od");
            criteria.Add(Expression.Eq("od.OrderHead.OrderNo", orderNo));
            criteria.Add(Expression.Eq("Item.Code", itemCode));
            criteria.Add(Expression.Eq("IOType", ioType));

            IList<OrderLocationTransaction> result = criteriaMgrE.FindAll<OrderLocationTransaction>(criteria);
            if (result != null && result.Count > 0)
            {
                return result[0];
            }
            else
            {
                return null;
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList ExtractOutboundDataFromLocationTransaction(DssOutboundControl dssOutboundControl, string transType, MatchMode matchMode)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(LocationTransaction));
            criteria.Add(Expression.Gt("Id", dssOutboundControl.Mark));
            criteria.Add(Expression.Like("TransactionType", transType, matchMode));

            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.Max("Id"))
                .Add(Projections.GroupProperty("OrderNo"))
                .Add(Projections.GroupProperty("ReceiptNo"))
                .Add(Projections.GroupProperty("Item"))
                .Add(Projections.GroupProperty("Location"))
                .Add(Projections.Sum("Qty"))
                .Add(Projections.GroupProperty("EffectiveDate"))
                .Add(Projections.GroupProperty("PartyFrom"))
                .Add(Projections.GroupProperty("PartyTo")));

            return criteriaMgrE.FindAll(criteria);
        }

        public DssExportHistory ConvertLocationTransactionToDssExportHistory(object obj, DssOutboundControl dssOutboundControl)
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
            dssExportHistory.Location = (string)((object[])obj)[4];
            dssExportHistory.Qty = (decimal)((object[])obj)[5];
            dssExportHistory.EffectiveDate = (DateTime)((object[])obj)[6];
            dssExportHistory.PartyFrom = (string)((object[])obj)[7];
            dssExportHistory.PartyTo = (string)((object[])obj)[8];

            return dssExportHistory;
        }

    }
}




#region Extend Class


namespace com.Sconit.Service.Ext.Dss.Impl
{
    public partial class CommonOutboundMgrE : com.Sconit.Service.Dss.Impl.CommonOutboundMgr, ICommonOutboundMgrE
    {
       

    }
}

#endregion

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
using NHibernate.Transform;


namespace com.Sconit.Service.Dss.Impl
{
    public class RcttrOutboundMgr : AbstractOutboundMgr
    {

		public ICriteriaMgrE criteriaMgr { get; set; }
        public IDssObjectMappingMgrE dssObjectMappingMgr { get; set; }
        public ICommonOutboundMgrE commonOutboundMgrE { get; set; }
        public ILocationTransactionMgrE locationTransactionMgrE { get; set; }

        public RcttrOutboundMgr(INumberControlMgrE numberControlMgrE,
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
            IList<DssExportHistory> trList = this.GetRctTrLocationTransaction(dssOutboundControl.Mark);
            this.ProcessRctTr(trList, dssOutboundControl);

            IList<LocationTransaction> inpTr = this.GetInpLocationTransaction(dssOutboundControl.Mark);
            IList<DssExportHistory> inpList = this.ProcessRctInp(inpTr);

            IList<DssExportHistory> result = trList.Concat(inpList).ToList();
            log.Debug("Get records: RCT-TR:" + trList.Count + ",RCT-INP:" + inpList.Count + ",concat result:" + result.Count);

            result = this.GroupData(result, dssOutboundControl);
            log.Debug("Group data,now records: " + result.Count);

            return result;
        }

        protected override object GetOutboundData(DssExportHistory dssExportHistory)
        {
            if (dssExportHistory.ReferenceLocation == null || dssExportHistory.ReferenceLocation.Trim() == string.Empty)
            {
                dssExportHistory.Comments = "来源库位为空";
                throw new BusinessErrorException("来源库位为空");
            }
            if (dssExportHistory.Location == null || dssExportHistory.Location.Trim() == string.Empty)
            {
                throw new BusinessErrorException("目的库位为空");
            }

            return (object)dssExportHistory;
        }

        protected override object Serialize(object obj)
        {
            DssExportHistory dssExportHistory = (DssExportHistory)obj;
            DateTime effDate = dssExportHistory.EffectiveDate.HasValue ? dssExportHistory.EffectiveDate.Value : DateTime.Today;

            log.Debug("Now write data:" + dssExportHistory.Item + "," + dssExportHistory.ReferenceLocation + "," + dssExportHistory.Location + "," + dssExportHistory.Qty.ToString("0.########"));

            string[] line1 = new string[] 
            { 
                DssHelper.GetEventValue(dssExportHistory.EventCode),
                dssExportHistory.Item,//零件号
                dssExportHistory.Qty.ToString("0.########"),//数量
                DssHelper.FormatDate(effDate,dssExportHistory.DssOutboundControl.ExternalSystem.Code),//生效日期
                dssExportHistory.KeyCode,//Loctrans ID
                dssExportHistory.PartyFrom,//QAD:Site,来源
                dssExportHistory.ReferenceLocation,//来源库位
                dssExportHistory.PartyTo,//QAD:Site,目的
                dssExportHistory.Location//目的库位
            };

            string[][] data = new string[][] { line1 };

            return new object[] { effDate, data };
        }

        #region Private Method
        private void ProcessRctTr(IList<DssExportHistory> list, DssOutboundControl dssOutboundControl)
        {
            if (list != null && list.Count > 0)
            {
                List<int> orderDetIdList = list.Where(l => l.OrderDetailId > 0).Select(l => l.OrderDetailId).Distinct().ToList();
                IList<LocationTransaction> issList = this.GetIssTr(orderDetIdList);

                if (issList != null && issList.Count > 0)
                {
                    foreach (var dssExportHistory in list)
                    {
                        var issTrans = issList.Where(i => i.OrderDetailId == dssExportHistory.OrderDetailId).SingleOrDefault();

                        if (issTrans != null)
                        {
                            dssExportHistory.ReferenceLocation = issTrans.Location;
                            dssExportHistory.PartyFrom = issTrans.PartyFrom;
                        }
                    }
                }
            }
        }

        public IList<DssExportHistory> ProcessRctInp(IList<LocationTransaction> inpTr)
        {
            IList<DssExportHistory> dssExportHistoryList = new List<DssExportHistory>();
            if (inpTr != null && inpTr.Count > 0)
            {
                var issList = inpTr
                    .Where(i => StringHelper.Eq(i.TransactionType, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_INP))
                    .ToList();
                var rctList = inpTr
                    .Where(i => StringHelper.Eq(i.TransactionType, BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_INP))
                    .ToList();

                foreach (var rctinp in rctList)
                {
                    DssExportHistory dssExportHistory = new DssExportHistory();
                    dssExportHistory.OriginalId = rctinp.Id;
                    dssExportHistory.OrderNo = rctinp.OrderNo;
                    dssExportHistory.PartyTo = rctinp.PartyTo;
                    dssExportHistory.Location = rctinp.Location;
                    dssExportHistory.EffectiveDate = rctinp.EffectiveDate;
                    dssExportHistory.Item = rctinp.Item;
                    dssExportHistory.Qty = rctinp.Qty;

                    #region 获取来源库位
                    //todo
                    var query = issList.Where(i => StringHelper.Eq(i.OrderNo, rctinp.OrderNo)
                        && StringHelper.Eq(i.Item, rctinp.Item)
                        && i.Id < rctinp.Id)
                        .OrderByDescending(i => i.Id);
                    if (query.Count() > 0)
                    {
                        var issinp = query.First();
                        dssExportHistory.PartyFrom = issinp.PartyFrom;
                        dssExportHistory.ReferenceLocation = issinp.Location;
                    }
                    #endregion

                    dssExportHistoryList.Add(dssExportHistory);
                }
            }

            return dssExportHistoryList;
        }

        public IList<DssExportHistory> GetRctTrLocationTransaction(int mark)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(LocationTransaction));
            criteria.Add(Expression.Gt("Id", mark));
            criteria.Add(Expression.Eq("TransactionType", BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_TR));
            criteria.Add(Expression.IsNotNull("OrderNo"));//todo,过滤投料

            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.Max("Id").As("OriginalId"))
                .Add(Projections.GroupProperty("OrderNo").As("OrderNo"))
                .Add(Projections.GroupProperty("OrderDetailId").As("OrderDetailId"))
                //.Add(Projections.GroupProperty("ReceiptNo"))
                .Add(Projections.GroupProperty("Item").As("Item"))
                .Add(Projections.GroupProperty("Location").As("Location"))
                .Add(Projections.Sum("Qty").As("Qty"))
                .Add(Projections.GroupProperty("EffectiveDate").As("EffectiveDate"))
                //.Add(Projections.GroupProperty("PartyFrom").As("PartyFrom"))
                .Add(Projections.GroupProperty("PartyTo").As("PartyTo")));

            criteria.SetResultTransformer(Transformers.AliasToBean(typeof(DssExportHistory)));
            return criteriaMgrE.FindAll<DssExportHistory>(criteria);
        }

        public IList<LocationTransaction> GetInpLocationTransaction(int mark)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(LocationTransaction));
            criteria.Add(Expression.Gt("Id", mark));
            criteria.Add(Expression.In("TransactionType", new string[]{ 
                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_ISS_INP,
                BusinessConstants.CODE_MASTER_LOCATION_TRANSACTION_TYPE_VALUE_RCT_INP}));

            return criteriaMgrE.FindAll<LocationTransaction>(criteria);
        }

        public IList<LocationTransaction> GetIssTr(List<int> orderDetIdList)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(OrderLocationTransaction));
            criteria.CreateAlias("OrderDetail", "od");
            criteria.CreateAlias("od.OrderHead", "oh");
            CriteriaHelper.SetInCriteria<int>(criteria, "od.Id", orderDetIdList);
            criteria.Add(Expression.Eq("IOType", BusinessConstants.IO_TYPE_OUT));

            criteria.SetProjection(Projections.Distinct(Projections.ProjectionList()
                .Add(Projections.Property("od.Id").As("OrderDetailId"))
                .Add(Projections.Property("Location.Code").As("Location"))
                .Add(Projections.Property("oh.PartyFrom.Code").As("PartyFrom"))));

            criteria.SetResultTransformer(Transformers.AliasToBean(typeof(LocationTransaction)));
            return criteriaMgrE.FindAll<LocationTransaction>(criteria);
        }

        public IList<DssExportHistory> GroupData(IList<DssExportHistory> list, DssOutboundControl dssOutboundControl)
        {
            var query = from l in list
                        group l by new { l.Item, l.PartyFrom, l.ReferenceLocation, l.PartyTo, l.Location, l.EffectiveDate } into g
                        select new DssExportHistory
                        {
                            Item = g.Key.Item,
                            PartyFrom = g.Key.PartyFrom,
                            ReferenceLocation = g.Key.ReferenceLocation,
                            PartyTo = g.Key.PartyTo,
                            Location = g.Key.Location,
                            Qty = g.Sum(d => d.Qty),
                            EffectiveDate = g.Key.EffectiveDate,
                            OriginalId = g.Max(d => d.OriginalId),
                            KeyCode = g.Max(d => d.OriginalId).ToString(),
                            DssOutboundControl = dssOutboundControl,
                            EventCode = BusinessConstants.DSS_EVENT_CODE_CREATE,
                            IsActive = true,
                            CreateDate = DateTime.Now
                        };

            return query.Where(q => q.Qty != 0).ToList();
        }

        #endregion
    }
}




#region Extend Class







namespace com.Sconit.Service.Ext.Dss.Impl
{
    public partial class RcttrOutboundMgrE : com.Sconit.Service.Dss.Impl.RcttrOutboundMgr, IOutboundMgrE
    {
        public RcttrOutboundMgrE(INumberControlMgrE numberControlMgrE,
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
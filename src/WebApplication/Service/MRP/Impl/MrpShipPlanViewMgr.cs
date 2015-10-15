using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Services.Transaction;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.MRP;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using NHibernate.Expression;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MRP.Impl
{
    [Transactional]
    public class MrpShipPlanViewMgr : MrpShipPlanViewBaseMgr, IMrpShipPlanViewMgr
    {
        public ICriteriaMgrE criteriaMgr { get; set; }
        public IFlowDetailMgrE flowDetailMgr { get; set; }
        #region Customized Methods

        [Transaction(TransactionMode.Requires)]
        public IList<MrpShipPlanView> GetMrpShipPlanViews(string flowCode, string locCode, string itemCode, DateTime effectiveDate, DateTime? winDate, DateTime? startDate)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(MrpShipPlanView));
            if (itemCode != null && itemCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Like("Item", itemCode, MatchMode.Anywhere)
                    || Expression.Like("ItemDescription", itemCode, MatchMode.Anywhere));
            }
            if (flowCode != null && flowCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Flow", flowCode));
            }
            if (locCode != null && locCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Location", locCode));
            }
            criteria.Add(Expression.Eq("EffectiveDate", effectiveDate));
            if (winDate.HasValue)
            {
                criteria.Add(Expression.Eq("WindowTime", winDate.Value.Date));
            }
            if (startDate.HasValue)
            {
                criteria.Add(Expression.Eq("StartTime", startDate.Value.Date));
            }
            return criteriaMgr.FindAll<MrpShipPlanView>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public ScheduleView TransferMrpShipPlanViews2ScheduleView(IList<MrpShipPlanView> mrpShipPlanViews,
            IList<ExpectTransitInventoryView> expectTransitInventoryViews,
            IList<ItemDiscontinue> itemDiscontinueList,
            string locOrFlow, string winOrStartTime)
        {
            if (mrpShipPlanViews == null || mrpShipPlanViews.Count == 0)
            {
                return null;
            }
            #region Í·
            List<ScheduleHead> scheduleHeads = new List<ScheduleHead>();

            if (locOrFlow == "Flow")
            {
                if (winOrStartTime == "WindowTime")
                {
                    scheduleHeads = (from det in mrpShipPlanViews
                                     group det by new { det.Flow, det.FlowType, det.WindowTime } into result
                                     select new ScheduleHead
                                     {
                                         Flow = result.Key.Flow,
                                         Type = result.Key.FlowType,
                                         DateTo = result.Key.WindowTime,
                                         Location = result.First().Location
                                     }).ToList();
                }
                else
                {
                    scheduleHeads = (from det in mrpShipPlanViews
                                     group det by new { det.Flow, det.FlowType, det.StartTime } into result
                                     select new ScheduleHead
                                     {
                                         Flow = result.Key.Flow,
                                         Type = result.Key.FlowType,
                                         DateFrom = result.Key.StartTime,
                                         Location = result.First().Location
                                     }).ToList();
                }
            }
            else if (locOrFlow == "Location")
            {
                if (winOrStartTime == "WindowTime")
                {
                    scheduleHeads = (from det in mrpShipPlanViews
                                     group det by new { det.Location, det.WindowTime } into result
                                     select new ScheduleHead
                                     {
                                         Location = result.Key.Location,
                                         Type = "Location",
                                         DateTo = result.Key.WindowTime,
                                     }).ToList();
                }
                else
                {
                    scheduleHeads = (from det in mrpShipPlanViews
                                     group det by new { det.Location, det.StartTime } into result
                                     select new ScheduleHead
                                     {
                                         Location = result.Key.Location,
                                         Type = "Location",
                                         DateFrom = result.Key.StartTime,
                                     }).ToList();
                }
            }
            else
            {
                throw new TechnicalException(locOrFlow);
            }

            if (winOrStartTime == "WindowTime")
            {
                scheduleHeads = scheduleHeads.OrderBy(c => c.DateTo).Take(41).ToList();
            }
            else
            {
                scheduleHeads = scheduleHeads.OrderBy(c => c.DateFrom).Take(41).ToList();
            }
            #endregion

            #region Ã÷Ï¸
            //var flowDetails = flowDetailMgr.GetFlowDetail(mrpShipPlanViews.First().Flow, true)
            //    .GroupBy(p => p.Item, (k, g) => new { k.Code, g.First().Sequence })
            //    .ToDictionary(d => d.Code, d => d.Sequence);

            List<ScheduleBody> scheduleBodys =
                (from det in mrpShipPlanViews
                 group det by new { det.Item, det.ItemDescription, det.ItemReference, det.Uom, det.UnitCount } into result
                 select new ScheduleBody
                 {
                     Item = result.Key.Item,
                     ItemDescription = result.Key.ItemDescription,
                     ItemReference = result.Key.ItemReference,
                     Uom = result.Key.Uom,
                     UnitCount = result.Key.UnitCount,
                     Location = result.First().Location,
                     //Seq = flowDetails.ValueOrDefault(result.Key.Item)
                 }).OrderBy(p => p.Item).ToList();
            #endregion

            #region ¸³Öµ
            foreach (ScheduleBody scheduleBody in scheduleBodys)
            {
                int i = 0;
                DateTime? lastDate = null;
                ScheduleHead lastScheduleHead = null;

                DateTime dtS = DateTime.MinValue;

                var itemDisconList = itemDiscontinueList == null ? null :
                        from discon in itemDiscontinueList
                        where discon.Item.Code == scheduleBody.Item
                        select discon;

                foreach (ScheduleHead scheduleHead in scheduleHeads)
                {

                    string qty = "Qty" + i.ToString();
                    string actQty = "ActQty" + i.ToString();
                    string disconActQty = "DisconActQty" + i.ToString();

                    if (locOrFlow == "Location")
                    {
                        if (winOrStartTime == "WindowTime")
                        {
                            var p = from plan in mrpShipPlanViews
                                    where plan.Location == scheduleHead.Location
                                    && plan.Item == scheduleBody.Item
                                    && plan.WindowTime == scheduleHead.DateTo
                                    select plan;

                            if (p != null && p.Count() >= 0)
                            {
                                PropertyInfo[] scheduleBodyPropertyInfo = typeof(ScheduleBody).GetProperties();
                                foreach (PropertyInfo pi in scheduleBodyPropertyInfo)
                                {
                                    if (pi.Name != null && StringHelper.Eq(pi.Name.ToLower(), qty))
                                    {
                                        pi.SetValue(scheduleBody, p.Sum(pp => pp.Qty), null);
                                        break;
                                    }
                                }
                            }

                            var q = from inv in expectTransitInventoryViews
                                    where inv.Location == scheduleHead.Location
                                    && inv.Item == scheduleBody.Item
                                    && inv.WindowTime <= scheduleHead.DateTo
                                    && (!lastDate.HasValue || inv.WindowTime > lastDate.Value)
                                    select inv;

                            if (q != null && q.Count() >= 0)
                            {
                                PropertyInfo[] scheduleBodyPropertyInfo = typeof(ScheduleBody).GetProperties();
                                foreach (PropertyInfo pi in scheduleBodyPropertyInfo)
                                {
                                    if (pi.Name != null && StringHelper.Eq(pi.Name.ToLower(), actQty))
                                    {
                                        pi.SetValue(scheduleBody, q.Sum(qq => qq.TransitQty), null);
                                        break;
                                    }
                                }
                            }

                            if (itemDisconList != null && itemDisconList.Count() > 0)
                            {
                                var r = from discon in itemDisconList
                                        join inv in expectTransitInventoryViews
                                        on discon.DiscontinueItem.Code equals inv.Item
                                        where inv.Location == scheduleHead.Location
                                        && inv.WindowTime <= scheduleHead.DateTo
                                        && (!lastDate.HasValue || inv.WindowTime > lastDate.Value)
                                        && discon.StartDate <= inv.StartTime
                                        && (!discon.EndDate.HasValue || discon.EndDate.Value >= inv.WindowTime)
                                        select inv;

                                if (r != null && r.Count() >= 0)
                                {
                                    PropertyInfo[] scheduleBodyPropertyInfo = typeof(ScheduleBody).GetProperties();
                                    foreach (PropertyInfo pi in scheduleBodyPropertyInfo)
                                    {
                                        if (pi.Name != null && StringHelper.Eq(pi.Name.ToLower(), disconActQty))
                                        {
                                            pi.SetValue(scheduleBody, r.Sum(rr => rr.TransitQty), null);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else if (winOrStartTime == "StartTime")
                        {
                            var p = from plan in mrpShipPlanViews
                                    where plan.Location == scheduleHead.Location
                                    && plan.Item == scheduleBody.Item
                                    && plan.StartTime == scheduleHead.DateFrom
                                    select plan;

                            if (p != null && p.Count() >= 0)
                            {
                                PropertyInfo[] scheduleBodyPropertyInfo = typeof(ScheduleBody).GetProperties();
                                foreach (PropertyInfo pi in scheduleBodyPropertyInfo)
                                {
                                    if (pi.Name != null && StringHelper.Eq(pi.Name.ToLower(), qty))
                                    {
                                        pi.SetValue(scheduleBody, p.Sum(pp => pp.Qty), null);
                                        break;
                                    }
                                }
                            }

                            var q = from inv in expectTransitInventoryViews
                                    where inv.Location == scheduleHead.Location
                                    && inv.Item == scheduleBody.Item
                                    && inv.StartTime <= scheduleHead.DateFrom
                                    && (!lastDate.HasValue || inv.StartTime > lastDate.Value)
                                    select inv;

                            if (q != null && q.Count() >= 0)
                            {
                                PropertyInfo[] scheduleBodyPropertyInfo = typeof(ScheduleBody).GetProperties();
                                foreach (PropertyInfo pi in scheduleBodyPropertyInfo)
                                {
                                    if (pi.Name != null && StringHelper.Eq(pi.Name.ToLower(), actQty))
                                    {
                                        pi.SetValue(scheduleBody, q.Sum(qq => qq.TransitQty), null);
                                        break;
                                    }
                                }
                            }

                            if (itemDisconList != null && itemDisconList.Count() > 0)
                            {
                                var r = from discon in itemDisconList
                                        join inv in expectTransitInventoryViews
                                        on discon.DiscontinueItem.Code equals inv.Item
                                        where inv.Location == scheduleHead.Location
                                        && inv.StartTime <= scheduleHead.DateFrom
                                        && (!lastDate.HasValue || inv.StartTime > lastDate.Value)
                                        && discon.StartDate <= inv.StartTime
                                        && (!discon.EndDate.HasValue || discon.EndDate.Value >= inv.WindowTime)
                                        select inv;

                                if (r != null && r.Count() >= 0)
                                {
                                    PropertyInfo[] scheduleBodyPropertyInfo = typeof(ScheduleBody).GetProperties();
                                    foreach (PropertyInfo pi in scheduleBodyPropertyInfo)
                                    {
                                        if (pi.Name != null && StringHelper.Eq(pi.Name.ToLower(), disconActQty))
                                        {
                                            pi.SetValue(scheduleBody, r.Sum(rr => rr.TransitQty), null);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (locOrFlow == "Flow")
                    {
                        if (winOrStartTime == "WindowTime")
                        {
                            var p = from plan in mrpShipPlanViews
                                    where plan.Flow == scheduleHead.Flow
                                    && plan.Item == scheduleBody.Item
                                    && plan.WindowTime == scheduleHead.DateTo
                                    select plan;

                            if (p != null && p.Count() >= 0)
                            {
                                PropertyInfo[] scheduleBodyPropertyInfo = typeof(ScheduleBody).GetProperties();
                                foreach (PropertyInfo pi in scheduleBodyPropertyInfo)
                                {
                                    if (pi.Name != null && StringHelper.Eq(pi.Name.ToLower(), qty))
                                    {
                                        pi.SetValue(scheduleBody, p.Sum(pp => pp.Qty), null);
                                        break;
                                    }
                                }
                            }

                            
                            //if(scheduleHead.)
                            var q = from inv in expectTransitInventoryViews
                                    where inv.Flow == scheduleHead.Flow
                                    && inv.Item == scheduleBody.Item
                                    && inv.WindowTime <= scheduleHead.DateTo
                                    && inv.WindowTime > dtS
                                    //&& (!lastDate.HasValue || inv.WindowTime > lastDate.Value)
                                    select inv;
                            

                            if (q != null && q.Count() >= 0)
                            {
                                PropertyInfo[] scheduleBodyPropertyInfo = typeof(ScheduleBody).GetProperties();
                                foreach (PropertyInfo pi in scheduleBodyPropertyInfo)
                                {
                                    if (pi.Name != null && StringHelper.Eq(pi.Name.ToLower(), actQty))
                                    {
                                        pi.SetValue(scheduleBody, q.Sum(qq => qq.TransitQty), null);
                                        dtS = scheduleHead.DateTo;
                                        break;
                                    }
                                }
                            }

                            if (itemDisconList != null && itemDisconList.Count() > 0)
                            {
                                var r = from discon in itemDisconList
                                        join inv in expectTransitInventoryViews
                                        on discon.DiscontinueItem.Code equals inv.Item
                                        where inv.Flow == scheduleHead.Flow
                                        && inv.WindowTime <= scheduleHead.DateTo
                                        && (!lastDate.HasValue || inv.WindowTime > lastDate.Value)
                                        && discon.StartDate <= inv.StartTime
                                        && (!discon.EndDate.HasValue || discon.EndDate.Value >= inv.WindowTime)
                                        select inv;

                                if (r != null && r.Count() >= 0)
                                {
                                    PropertyInfo[] scheduleBodyPropertyInfo = typeof(ScheduleBody).GetProperties();
                                    foreach (PropertyInfo pi in scheduleBodyPropertyInfo)
                                    {
                                        if (pi.Name != null && StringHelper.Eq(pi.Name.ToLower(), disconActQty))
                                        {
                                            pi.SetValue(scheduleBody, r.Sum(rr => rr.TransitQty), null);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else if (winOrStartTime == "StartTime")
                        {
                            var p = from plan in mrpShipPlanViews
                                    where plan.Flow == scheduleHead.Flow
                                    && plan.Item == scheduleBody.Item
                                    && plan.StartTime == scheduleHead.DateFrom
                                    select plan;

                            if (p != null && p.Count() >= 0)
                            {
                                PropertyInfo[] scheduleBodyPropertyInfo = typeof(ScheduleBody).GetProperties();
                                foreach (PropertyInfo pi in scheduleBodyPropertyInfo)
                                {
                                    if (pi.Name != null && StringHelper.Eq(pi.Name.ToLower(), qty))
                                    {
                                        pi.SetValue(scheduleBody, p.Sum(pp => pp.Qty), null);
                                        break;
                                    }
                                }
                            }

                            var q = from inv in expectTransitInventoryViews
                                    where inv.Flow == scheduleHead.Flow
                                    && inv.Item == scheduleBody.Item
                                    && inv.StartTime <= scheduleHead.DateFrom
                                    && (!lastDate.HasValue || inv.StartTime > lastDate.Value)
                                    select inv;

                            if (q != null && q.Count() >= 0)
                            {
                                PropertyInfo[] scheduleBodyPropertyInfo = typeof(ScheduleBody).GetProperties();
                                foreach (PropertyInfo pi in scheduleBodyPropertyInfo)
                                {
                                    if (pi.Name != null && StringHelper.Eq(pi.Name.ToLower(), actQty))
                                    {
                                        pi.SetValue(scheduleBody, q.Sum(qq => qq.TransitQty), null);
                                        break;
                                    }
                                }
                            }

                            if (itemDisconList != null && itemDisconList.Count() > 0)
                            {
                                var r = from discon in itemDisconList
                                        join inv in expectTransitInventoryViews
                                        on discon.DiscontinueItem.Code equals inv.Item
                                        where inv.Flow == scheduleHead.Flow
                                        && inv.StartTime <= scheduleHead.DateFrom
                                        && (!lastDate.HasValue || inv.StartTime > lastDate.Value)
                                        && discon.StartDate <= inv.StartTime
                                        && (!discon.EndDate.HasValue || discon.EndDate.Value >= inv.WindowTime)
                                        select inv;

                                if (r != null && r.Count() >= 0)
                                {
                                    PropertyInfo[] scheduleBodyPropertyInfo = typeof(ScheduleBody).GetProperties();
                                    foreach (PropertyInfo pi in scheduleBodyPropertyInfo)
                                    {
                                        if (pi.Name != null && StringHelper.Eq(pi.Name.ToLower(), disconActQty))
                                        {
                                            pi.SetValue(scheduleBody, r.Sum(rr => rr.TransitQty), null);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    i++;
                    if (winOrStartTime == "WindowTime")
                    {
                        lastDate = scheduleHead.DateTo;
                    }
                    else if (winOrStartTime == "StartTime")
                    {
                        lastDate = scheduleHead.DateFrom;
                    }
                    else
                    {
                        throw new TechnicalException(winOrStartTime);
                    }

                    if (lastScheduleHead != null)
                    {
                        scheduleHead.LastDateTo = lastScheduleHead.DateTo;
                        scheduleHead.LastDateFrom = lastScheduleHead.DateFrom;
                    }
                    lastScheduleHead = scheduleHead;
                }
            }
            #endregion

            ScheduleView scheduleView = new ScheduleView();
            scheduleView.ScheduleHeads = scheduleHeads;
            scheduleView.ScheduleBodys = scheduleBodys;
            return scheduleView;
        }

        #endregion Customized Methods
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.MRP.Impl
{
    [Transactional]
    public partial class MrpShipPlanViewMgrE : com.Sconit.Service.MRP.Impl.MrpShipPlanViewMgr, IMrpShipPlanViewMgrE
    {
    }
}

#endregion Extend Class
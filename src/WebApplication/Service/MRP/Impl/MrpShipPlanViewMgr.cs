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

        public ScheduleView TransferMrpShipPlanViews2ScheduleView(IList<MrpShipPlanView> mrpShipPlanViews,
            IList<ExpectTransitInventoryView> expectTransitInventoryViews,
            IList<ItemDiscontinue> itemDiscontinueList,
            string locOrFlow, string winOrStartTime)
        {
            if (mrpShipPlanViews == null || mrpShipPlanViews.Count == 0)
            {
                return null;
            }
            #region 头
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

            #region 明细
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

            #region 赋值
            foreach (ScheduleBody scheduleBody in scheduleBodys)
            {
                int i = 0;
                DateTime? lastDate = null;
                ScheduleHead lastScheduleHead = null;

                DateTime dtS = DateTime.MinValue;

                var itemDisconList = itemDiscontinueList == null ? null :
                        from discon in itemDiscontinueList
                        where discon.Item.Code == scheduleBody.Item && discon.StartDate <= DateTime.Now && discon.EndDate >= DateTime.Now
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


        public ScheduleView TransferMrpShipPlanViews2ScheduleView2(IList<MrpShipPlanView> mrpShipPlanViews,
           IList<ExpectTransitInventoryView> expectTransitInventoryViews,
           IList<ItemDiscontinue> itemDiscontinueList)
        {
            if (mrpShipPlanViews == null || mrpShipPlanViews.Count == 0)
            {
                return null;
            }
            #region 头
            List<ScheduleHead> scheduleHeads = new List<ScheduleHead>();

            scheduleHeads = (from det in mrpShipPlanViews
                             group det by new { det.Flow, det.FlowType, det.WindowTime } into result
                             select new ScheduleHead
                             {
                                 Flow = result.Key.Flow,
                                 Type = result.Key.FlowType,
                                 DateTo = result.Key.WindowTime,
                                 Location = result.First().Location
                             }).OrderBy(c => c.DateTo).Take(41).ToList();
            #endregion

            #region 明细
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

            #region 赋值
            foreach (ScheduleBody scheduleBody in scheduleBodys)
            {
                int i = 0;
                DateTime? lastDate = null;
                ScheduleHead lastScheduleHead = null;

                DateTime dtS = DateTime.MinValue;

                var itemDisconList = itemDiscontinueList == null ? null :
                        from discon in itemDiscontinueList
                        where discon.Item.Code == scheduleBody.Item && discon.StartDate <= DateTime.Now && discon.EndDate >= DateTime.Now
                        select discon;

                foreach (ScheduleHead scheduleHead in scheduleHeads)
                {

                    string qty = "Qty" + i.ToString();
                    string ReqQty = "ReqQty" + i.ToString();
                    string actQty = "ActQty" + i.ToString();
                    string disconActQty = "DisconActQty" + i.ToString();


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
                            if (pi.Name != null && StringHelper.Eq(pi.Name.ToLower(), ReqQty))
                            {
                                pi.SetValue(scheduleBody, p.Sum(pp => pp.Qty), null);
                                break;
                            }
                        }
                    }

                    //if(scheduleHead.)
                    var q = from inv in expectTransitInventoryViews
                            where inv.Location == scheduleHead.Location
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

                    i++;

                    lastDate = scheduleHead.DateTo;

                    if (lastScheduleHead != null)
                    {
                        scheduleHead.LastDateTo = lastScheduleHead.DateTo;
                        scheduleHead.LastDateFrom = lastScheduleHead.DateFrom;
                    }
                    lastScheduleHead = scheduleHead;
                }

                scheduleBody.Qty0 = scheduleBody.ReqQty0;
                scheduleBody.Qty1 = scheduleBody.ReqQty1;
                scheduleBody.Qty2 = scheduleBody.ReqQty2;
                scheduleBody.Qty3 = scheduleBody.ReqQty3;
                scheduleBody.Qty4 = scheduleBody.ReqQty4;
                scheduleBody.Qty5 = scheduleBody.ReqQty5;
                scheduleBody.Qty6 = scheduleBody.ReqQty6;
                scheduleBody.Qty7 = scheduleBody.ReqQty7;
                scheduleBody.Qty8 = scheduleBody.ReqQty8;
                scheduleBody.Qty9 = scheduleBody.ReqQty9;
                scheduleBody.Qty10 = scheduleBody.ReqQty10;
                scheduleBody.Qty11 = scheduleBody.ReqQty11;
                scheduleBody.Qty12 = scheduleBody.ReqQty12;
                scheduleBody.Qty13 = scheduleBody.ReqQty13;
                scheduleBody.Qty14 = scheduleBody.ReqQty14;
                scheduleBody.Qty15 = scheduleBody.ReqQty15;
                scheduleBody.Qty16 = scheduleBody.ReqQty16;
                scheduleBody.Qty17 = scheduleBody.ReqQty17;
                scheduleBody.Qty18 = scheduleBody.ReqQty18;
                scheduleBody.Qty19 = scheduleBody.ReqQty19;
                scheduleBody.Qty20 = scheduleBody.ReqQty20;
                scheduleBody.Qty21 = scheduleBody.ReqQty21;
                scheduleBody.Qty22 = scheduleBody.ReqQty22;
                scheduleBody.Qty23 = scheduleBody.ReqQty23;
                scheduleBody.Qty24 = scheduleBody.ReqQty24;
                scheduleBody.Qty25 = scheduleBody.ReqQty25;
                scheduleBody.Qty26 = scheduleBody.ReqQty26;
                scheduleBody.Qty27 = scheduleBody.ReqQty27;
                scheduleBody.Qty28 = scheduleBody.ReqQty28;
                scheduleBody.Qty29 = scheduleBody.ReqQty29;
                scheduleBody.Qty30 = scheduleBody.ReqQty30;
                scheduleBody.Qty31 = scheduleBody.ReqQty31;
                scheduleBody.Qty32 = scheduleBody.ReqQty32;
                scheduleBody.Qty33 = scheduleBody.ReqQty33;
                scheduleBody.Qty34 = scheduleBody.ReqQty34;
                scheduleBody.Qty35 = scheduleBody.ReqQty35;
                scheduleBody.Qty36 = scheduleBody.ReqQty36;
                scheduleBody.Qty37 = scheduleBody.ReqQty37;
                scheduleBody.Qty38 = scheduleBody.ReqQty38;
                scheduleBody.Qty39 = scheduleBody.ReqQty39;
                scheduleBody.Qty40 = scheduleBody.ReqQty40;
            }
            #endregion

            foreach (ExpectTransitInventoryView expectTransitInventoryView in expectTransitInventoryViews)
            {
                if (expectTransitInventoryView.TransitQty.HasValue
                    && expectTransitInventoryView.TransitQty.Value > 0)
                {
                    decimal transitQty = expectTransitInventoryView.TransitQty.Value;

                    int i = 0;
                    foreach (ScheduleHead scheduleHead in scheduleHeads)
                    {
                        if (transitQty <= 0)
                        {
                            break;
                        }

                        if (scheduleHead.DateTo >= expectTransitInventoryView.WindowTime
                            && scheduleHead.Location == expectTransitInventoryView.Location)
                        {
                            ScheduleBody scheduleBody = scheduleBodys.Where(b => b.Item == expectTransitInventoryView.Item).FirstOrDefault();
                            if (scheduleBody != null)
                            {
                                if (i == 0)
                                {
                                    if (transitQty >= scheduleBody.Qty0)
                                    {
                                        transitQty -= scheduleBody.Qty0;
                                        scheduleBody.Qty0 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty0 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 1)
                                {
                                    if (transitQty >= scheduleBody.Qty1)
                                    {
                                        transitQty -= scheduleBody.Qty1;
                                        scheduleBody.Qty1 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty1 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 2)
                                {
                                    if (transitQty >= scheduleBody.Qty2)
                                    {
                                        transitQty -= scheduleBody.Qty2;
                                        scheduleBody.Qty2 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty2 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 3)
                                {
                                    if (transitQty >= scheduleBody.Qty3)
                                    {
                                        transitQty -= scheduleBody.Qty3;
                                        scheduleBody.Qty3 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty3 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 4)
                                {
                                    if (transitQty >= scheduleBody.Qty4)
                                    {
                                        transitQty -= scheduleBody.Qty4;
                                        scheduleBody.Qty4 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty4 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 5)
                                {
                                    if (transitQty >= scheduleBody.Qty5)
                                    {
                                        transitQty -= scheduleBody.Qty5;
                                        scheduleBody.Qty5 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty5 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 6)
                                {
                                    if (transitQty >= scheduleBody.Qty6)
                                    {
                                        transitQty -= scheduleBody.Qty6;
                                        scheduleBody.Qty6 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty6 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 7)
                                {
                                    if (transitQty >= scheduleBody.Qty7)
                                    {
                                        transitQty -= scheduleBody.Qty7;
                                        scheduleBody.Qty7 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty7 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 8)
                                {
                                    if (transitQty >= scheduleBody.Qty8)
                                    {
                                        transitQty -= scheduleBody.Qty8;
                                        scheduleBody.Qty8 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty8 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 9)
                                {
                                    if (transitQty >= scheduleBody.Qty9)
                                    {
                                        transitQty -= scheduleBody.Qty9;
                                        scheduleBody.Qty9 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty9 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 10)
                                {
                                    if (transitQty >= scheduleBody.Qty10)
                                    {
                                        transitQty -= scheduleBody.Qty10;
                                        scheduleBody.Qty10 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty10 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 11)
                                {
                                    if (transitQty >= scheduleBody.Qty11)
                                    {
                                        transitQty -= scheduleBody.Qty11;
                                        scheduleBody.Qty11 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty11 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 12)
                                {
                                    if (transitQty >= scheduleBody.Qty12)
                                    {
                                        transitQty -= scheduleBody.Qty12;
                                        scheduleBody.Qty12 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty12 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 13)
                                {
                                    if (transitQty >= scheduleBody.Qty13)
                                    {
                                        transitQty -= scheduleBody.Qty13;
                                        scheduleBody.Qty13 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty13 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 14)
                                {
                                    if (transitQty >= scheduleBody.Qty14)
                                    {
                                        transitQty -= scheduleBody.Qty14;
                                        scheduleBody.Qty14 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty14 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 15)
                                {
                                    if (transitQty >= scheduleBody.Qty15)
                                    {
                                        transitQty -= scheduleBody.Qty15;
                                        scheduleBody.Qty15 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty15 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 16)
                                {
                                    if (transitQty >= scheduleBody.Qty16)
                                    {
                                        transitQty -= scheduleBody.Qty16;
                                        scheduleBody.Qty16 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty16 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 17)
                                {
                                    if (transitQty >= scheduleBody.Qty17)
                                    {
                                        transitQty -= scheduleBody.Qty17;
                                        scheduleBody.Qty17 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty17 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 18)
                                {
                                    if (transitQty >= scheduleBody.Qty18)
                                    {
                                        transitQty -= scheduleBody.Qty18;
                                        scheduleBody.Qty18 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty18 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 19)
                                {
                                    if (transitQty >= scheduleBody.Qty19)
                                    {
                                        transitQty -= scheduleBody.Qty19;
                                        scheduleBody.Qty19 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty19 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 20)
                                {
                                    if (transitQty >= scheduleBody.Qty20)
                                    {
                                        transitQty -= scheduleBody.Qty20;
                                        scheduleBody.Qty20 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty20 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 21)
                                {
                                    if (transitQty >= scheduleBody.Qty21)
                                    {
                                        transitQty -= scheduleBody.Qty21;
                                        scheduleBody.Qty21 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty21 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 22)
                                {
                                    if (transitQty >= scheduleBody.Qty22)
                                    {
                                        transitQty -= scheduleBody.Qty22;
                                        scheduleBody.Qty22 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty22 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 23)
                                {
                                    if (transitQty >= scheduleBody.Qty23)
                                    {
                                        transitQty -= scheduleBody.Qty23;
                                        scheduleBody.Qty23 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty23 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 24)
                                {
                                    if (transitQty >= scheduleBody.Qty24)
                                    {
                                        transitQty -= scheduleBody.Qty24;
                                        scheduleBody.Qty24 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty24 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 25)
                                {
                                    if (transitQty >= scheduleBody.Qty25)
                                    {
                                        transitQty -= scheduleBody.Qty25;
                                        scheduleBody.Qty25 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty25 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 26)
                                {
                                    if (transitQty >= scheduleBody.Qty26)
                                    {
                                        transitQty -= scheduleBody.Qty26;
                                        scheduleBody.Qty26 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty26 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 27)
                                {
                                    if (transitQty >= scheduleBody.Qty27)
                                    {
                                        transitQty -= scheduleBody.Qty27;
                                        scheduleBody.Qty27 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty27 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 28)
                                {
                                    if (transitQty >= scheduleBody.Qty28)
                                    {
                                        transitQty -= scheduleBody.Qty28;
                                        scheduleBody.Qty28 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty28 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 29)
                                {
                                    if (transitQty >= scheduleBody.Qty29)
                                    {
                                        transitQty -= scheduleBody.Qty29;
                                        scheduleBody.Qty29 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty29 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 30)
                                {
                                    if (transitQty >= scheduleBody.Qty30)
                                    {
                                        transitQty -= scheduleBody.Qty30;
                                        scheduleBody.Qty30 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty30 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 31)
                                {
                                    if (transitQty >= scheduleBody.Qty31)
                                    {
                                        transitQty -= scheduleBody.Qty31;
                                        scheduleBody.Qty31 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty31 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 32)
                                {
                                    if (transitQty >= scheduleBody.Qty32)
                                    {
                                        transitQty -= scheduleBody.Qty32;
                                        scheduleBody.Qty32 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty32 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 33)
                                {
                                    if (transitQty >= scheduleBody.Qty33)
                                    {
                                        transitQty -= scheduleBody.Qty33;
                                        scheduleBody.Qty33 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty33 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 34)
                                {
                                    if (transitQty >= scheduleBody.Qty34)
                                    {
                                        transitQty -= scheduleBody.Qty34;
                                        scheduleBody.Qty34 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty34 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 35)
                                {
                                    if (transitQty >= scheduleBody.Qty35)
                                    {
                                        transitQty -= scheduleBody.Qty35;
                                        scheduleBody.Qty35 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty35 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 36)
                                {
                                    if (transitQty >= scheduleBody.Qty36)
                                    {
                                        transitQty -= scheduleBody.Qty36;
                                        scheduleBody.Qty36 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty36 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 37)
                                {
                                    if (transitQty >= scheduleBody.Qty37)
                                    {
                                        transitQty -= scheduleBody.Qty37;
                                        scheduleBody.Qty37 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty37 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 38)
                                {
                                    if (transitQty >= scheduleBody.Qty38)
                                    {
                                        transitQty -= scheduleBody.Qty38;
                                        scheduleBody.Qty38 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty38 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 39)
                                {
                                    if (transitQty >= scheduleBody.Qty39)
                                    {
                                        transitQty -= scheduleBody.Qty39;
                                        scheduleBody.Qty39 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty39 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                                else if (i == 40)
                                {
                                    if (transitQty >= scheduleBody.Qty40)
                                    {
                                        transitQty -= scheduleBody.Qty40;
                                        scheduleBody.Qty40 = 0;
                                    }
                                    else
                                    {
                                        scheduleBody.Qty40 -= transitQty;
                                        transitQty = 0;
                                    }
                                }
                            }
                        }

                        i++;
                    }
                }
            }

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
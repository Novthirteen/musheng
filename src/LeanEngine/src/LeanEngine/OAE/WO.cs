﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeanEngine.Entity;
using LeanEngine.Utility;

namespace LeanEngine.OAE
{
    /// <summary>
    /// Just In Time
    /// </summary>
    public class WO : OAEBase
    {
        public WO(List<Plans> Plans, List<InvBalance> InvBalances, List<DemandChain> DemandChains)
            : base(Plans, InvBalances, DemandChains)
        {
        }

        protected override decimal GetReqQty(ItemFlow itemFlow)
        {
            string item = itemFlow.Item;
            DateTime? orderTime = itemFlow.Flow.OrderTime;
            DateTime? winTime = itemFlow.Flow.WindowTime;
            DateTime? nextWinTime = itemFlow.Flow.NextWindowTime;

            #region Demand
            OrderTracer demand = this.GetDemand_OrderTracer(itemFlow);
            itemFlow.AddOrderTracer(demand);
            #endregion

            foreach (var loc in itemFlow.DemandSources)
            {
                #region Demand
                //var demands = this.GetOrderIss(loc, item, winTime, nextWinTime, Enumerators.TracerType.Demand);
                //itemFlow.AddOrderTracer(demands);
                #endregion

                #region OnhandInv
                OrderTracer onhandInv = this.GetOnhandInv_OrderTracer(loc, item);
                itemFlow.AddOrderTracer(onhandInv);
                #endregion

                #region InspectInv
                OrderTracer inspectInv = this.GetInspectInv_OrderTracer(loc, item);
                itemFlow.AddOrderTracer(inspectInv);
                #endregion

                #region OrderRct
                var orderRcts = this.GetOrderRct(loc, item, null, null);
                itemFlow.AddOrderTracer(orderRcts);
                #endregion

                #region OrderIss
                DateTime? startTime = null;
                if (true)//todo,config
                {
                    startTime = orderTime;
                }
                var orderIsss = this.GetOrderIss(loc, item, null, null);
                itemFlow.AddOrderTracer(orderIsss);
                #endregion
            }

            decimal reqQty = this.GetReqQty(itemFlow.OrderTracers);

            return reqQty;

            //double relativeLeadTime = demandChain.RelativeLeadTime;
            //decimal relativeQtyPer = demandChain.RelativeQtyPer;

            //string loc = demandChain.Loc;
            //string item = demandChain.Item;
            //string flowCode = demandChain.FlowCode;

            //DateTime? winTime = itemFlow.Flow.WindowTime.HasValue ?
            //    itemFlow.Flow.WindowTime.Value : DateTime.Now.AddHours(itemFlow.Flow.FlowStrategy.LeadTime);
            //winTime = this.GetRelativeTime(itemFlow.Flow.WindowTime, demandChain);
            //DateTime? nextWinTime = this.GetRelativeTime(itemFlow.Flow.NextWindowTime, demandChain);

            //List<Plans> demandList = (from p in Plans
            //                          where Utilities.StringEq(loc, p.Loc)
            //                          && (flowCode == null || Utilities.StringEq(flowCode, p.FlowCode))
            //                          && Utilities.StringEq(item, p.Item)
            //                          && (!winTime.HasValue || p.ReqTime >= winTime.Value)//greater equal
            //                          && (!nextWinTime.HasValue || p.ReqTime < nextWinTime.Value)//less than
            //                          && p.IRType == Enumerators.IRType.ISS
            //                          select p).ToList();

            //decimal orderQty = demandList.Where(d => d.PlanType == Enumerators.PlanType.Orders).Sum(d => d.Qty);
            //decimal planQty = demandList.Where(d => d.PlanType == Enumerators.PlanType.Plans).Sum(d => d.Qty);
            //decimal demandQty = Math.Max(orderQty, planQty);

            //decimal safeInv = Math.Max(demandChain.SafeInv, itemFlow.SafeInv);//todo
            //decimal availableInv = this.GetAvailableInvQty(loc, item, winTime);

            //decimal reqQty = this.GetJITReqQty(availableInv, safeInv, demandQty);
            //if (reqQty > 0 && demandChain.IsTrace)
            //{
            //    itemFlow.DemandList.AddRange(demandList);
            //}

            //return reqQty;
        }

        protected virtual List<OrderTracer> GetOrderIss(string loc, string item, Enumerators.TracerType tracerType, Flow flow)
        {
            if (Plans == null || Plans.Count == 0)
                return null;

            var query = from p in Plans
                        where Utilities.StringEq(p.Item, item) && Utilities.StringEq(p.Loc, loc) &&                     
                        p.IRType == Enumerators.IRType.ISS &&
                        p.PlanType == Enumerators.PlanType.Orders
                        select new OrderTracer
                        {
                            TracerType = tracerType,
                            Code = p.OrderNo,
                            ReqTime = p.ReqTime,
                            Item = p.Item,
                            OrderedQty = p.OrderedQty,
                            FinishedQty = p.FinishedQty,
                            Qty = p.Qty,
                            RefId = p.ID
                        };

            return query.ToList();
        }

        //private decimal GetJITReqQty(decimal availableInv, decimal safeInv, decimal demandQty)
        //{
        //    return demandQty + safeInv - availableInv;
        //}
    }
}

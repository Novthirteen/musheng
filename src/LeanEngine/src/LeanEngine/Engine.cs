// **************************************************************************************
// This is a solution for SCM(Supply Chain Management). It contains
// SCM(Supply Chain Model)/OAE(Order Automation Engine)/MPS(Master Production Schedule)/
// MRP(Material Requirements Planning)/APS(Advanced Planning and Scheduling) components.
// Author: Deng Xuyao.  Date: 2010-07.
// **************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeanEngine.Entity;
using LeanEngine.Utility;
using LeanEngine.OAE;
using System.Reflection;
using LeanEngine.SCM;

namespace LeanEngine
{
    /// <summary>
    /// Lean Engine
    /// </summary>
    public class Engine : IEngine, ISupplyChainMgr
    {
        private KB kb;
        private JIT jit;
        private ODP odp;
        private TRD trd;
        private MRP mrp;
        private WO wo;
        private FaLiao faLiao;
        private ISupplyChainMgr TheSupplyChainMgr = new SupplyChainMgr();

        public List<SupplyChain> BuildSupplyChain(string itemCode, List<ItemFlow> ItemFlows)
        {
            return TheSupplyChainMgr.BuildSupplyChain(itemCode, ItemFlows);
        }

        public IOAE GetProcessor(Enumerators.Strategy strategy)
        {
            Dictionary<Enumerators.Strategy, IOAE> dic = new Dictionary<Enumerators.Strategy, IOAE>();
            dic.Add(Enumerators.Strategy.KB, kb);
            dic.Add(Enumerators.Strategy.JIT, jit);
            dic.Add(Enumerators.Strategy.ODP, odp);
            dic.Add(Enumerators.Strategy.TRD, trd);
            dic.Add(Enumerators.Strategy.MRP, mrp);
            dic.Add(Enumerators.Strategy.WO, wo);
            dic.Add(Enumerators.Strategy.FaLiao, faLiao);

            if (dic.ContainsKey(strategy))
            {
                return dic[strategy];
            }
            return null;
        }

        public void TellMeDemands(EngineContainer container)
        {
            if (container == null || container.ItemFlows == null || container.ItemFlows.Count == 0)
                return;

            #region Initialize
            kb = new KB(container.Plans, container.InvBalances, container.DemandChains);
            jit = new JIT(container.Plans, container.InvBalances, container.DemandChains);
            odp = new ODP(container.Plans, container.InvBalances, container.DemandChains);
            trd = new TRD(container.Plans, container.InvBalances, container.DemandChains);
            mrp = new MRP(container.Plans, container.InvBalances, container.DemandChains);
            wo = new WO(container.Plans, container.InvBalances, container.DemandChains);
            faLiao = new FaLiao(container.Plans, container.InvBalances, container.DemandChains);
            #endregion

            #region Container param
            List<ItemFlow> ItemFlows = container.ItemFlows;
            List<Plans> Plans = container.Plans;
            List<InvBalance> InvBalances = container.InvBalances;
            List<DemandChain> DemandChains = container.DemandChains;
            #endregion

            #region Process time
            List<Flow> flows = ItemFlows.Select(s => s.Flow).Distinct().ToList<Flow>();
            this.ProcessTime(flows);
            #endregion

            #region Process ReqQty
            foreach (var itemFlow in ItemFlows)
            {
                this.DataValidCheck(itemFlow);
                this.SetFlowProperty(itemFlow, flows);

                this.ProcessReqQty(itemFlow);
            }
            #endregion
        }

        public List<Orders> DemandToOrders(List<ItemFlow> itemFlows)
        {
            #region 非WO策略
            #region ProcessOrderQty
            var query = from i in itemFlows
                        where i.ReqQty > 0 && i.Flow.FlowStrategy.Strategy != Enumerators.Strategy.WO
                        group i by new { i.LocTo, i.Item } into g
                        select new { g.Key, list = g.ToList(), Count = g.Count() };

            if (query != null && query.Count() > 0)
            {
                foreach (var g in query)
                {
                    if (g.Count == 1)
                    {
                        ItemFlow itemFlow = itemFlows.Single(i => i.Equals(g.list[0]));
                        this.ProcessOrderQty(itemFlow);
                    }
                    else
                    {
                        //to be testing
                        ItemFlow winBidItemFlow = this.BidItemFlow(g.list);
                        ItemFlow itemFlow = itemFlows.FirstOrDefault(i => i.Equals(winBidItemFlow));
                        
                        this.ProcessOrderQty(itemFlow);
                    }
                }
            }
            #endregion

            List<Orders> result = new List<Orders>();
            #region Emergency Orders
            List<Orders> emOrders = this.BuildOrders(itemFlows, true);
            if (emOrders != null)
                result = result.Concat(emOrders).ToList();
            #endregion

            #region Normal Orders
            List<Orders> nmlOrders = this.BuildOrders(itemFlows, false);
            if (nmlOrders != null)
                result = result.Concat(nmlOrders).ToList();
            #endregion

            #region Filter
            result = result.Where(r => (r.IsEmergency && r.ItemFlows.Count > 0)
                || (!r.IsEmergency && (r.Flow.IsUpdateWindowTime || r.ItemFlows.Count > 0))).ToList();
            #endregion
            #endregion

            #region WO策略
            var queryWO = from i in itemFlows
                          where i.ReqQty > 0 && i.Flow.FlowStrategy.Strategy == Enumerators.Strategy.WO
                          select i;

            if (queryWO != null && queryWO.Count() > 0)
            {
                foreach (var woItemFlow in queryWO)
                {
                    List<OrderTracer> orderTracerList = new List<OrderTracer>();
                    var orderIss = from ot in woItemFlow.OrderTracers
                                   where ot.TracerType == Enumerators.TracerType.OrderIss && ot.Qty != 0
                                   select ot;

                    var onhandInv = from ot in woItemFlow.OrderTracers
                                    where ot.TracerType == Enumerators.TracerType.OnhandInv && ot.Qty != 0
                                    select ot;

                    decimal onhandInvQty = decimal.Zero;
                    if (onhandInv != null && onhandInv.Count() > 0)
                    {
                        onhandInvQty = onhandInv.ToList().Sum(inv => inv.Qty);
                        orderTracerList = orderTracerList.Concat(onhandInv.ToList()).ToList();
                    }

                    var inspectInv = from ot in woItemFlow.OrderTracers
                                     where ot.TracerType == Enumerators.TracerType.InspectInv && ot.Qty != 0
                                     select ot;

                    decimal inspectInvQty = decimal.Zero;
                    if (inspectInv != null && inspectInv.Count() > 0)
                    {
                        inspectInvQty = inspectInv.ToList().Sum(inv => inv.Qty);
                        orderTracerList = orderTracerList.Concat(inspectInv.ToList()).ToList();
                    }

                    var orderRct = from ot in woItemFlow.OrderTracers
                                   where ot.TracerType == Enumerators.TracerType.OrderRct && ot.Qty != 0
                                   select ot;

                    decimal orderRctQty = decimal.Zero;
                    if (orderRct != null && orderRct.Count() > 0)
                    {
                        orderRctQty = orderRct.ToList().Sum(rct => rct.Qty);
                        orderTracerList = orderTracerList.Concat(orderRct.ToList()).ToList();
                    }

                    if (orderIss != null && orderIss.Count() > 0)
                    {
                        foreach (OrderTracer ot in orderIss)
                        {
                            decimal qty = ot.Qty;

                            #region 扣减库存
                            if (qty > 0)
                            {
                                if (qty >= onhandInvQty)
                                {
                                    qty -= onhandInvQty;
                                    onhandInvQty = 0;
                                }
                                else
                                {
                                    onhandInvQty -= qty;
                                    qty = 0;
                                }
                            }
                            #endregion

                            #region 扣减待验库存
                            if (qty > 0)
                            {
                                if (qty >= inspectInvQty)
                                {
                                    qty -= inspectInvQty;
                                    inspectInvQty = 0;
                                }
                                else
                                {
                                    inspectInvQty -= qty;
                                    qty = 0;
                                }
                            }
                            #endregion

                            #region 扣减待收
                            if (qty > 0)
                            {
                                if (qty >= orderRctQty)
                                {
                                    qty -= orderRctQty;
                                    orderRctQty = 0;
                                }
                                else
                                {
                                    orderRctQty -= qty;
                                    qty = 0;
                                }
                            }
                            #endregion

                            if (qty > 0)
                            {
                                #region 生成订单
                                Orders orders = new Orders();

                                orders.Flow = woItemFlow.Flow;

                                double emLeadTime = orders.Flow.FlowStrategy.EmLeadTime;
                                double leadTime = orders.Flow.FlowStrategy.LeadTime;
                                if (ot.ReqTime.AddHours(-emLeadTime) <= DateTime.Now)
                                {
                                    orders.IsEmergency = true;
                                    orders.StartTime = DateTime.Now;
                                    orders.WindowTime = orders.StartTime.AddHours(emLeadTime);
                                }
                                else
                                {
                                    orders.WindowTime = ot.ReqTime;
                                    orders.StartTime = ot.ReqTime.AddHours(-leadTime);
                                    orders.IsEmergency = false;
                                }

                                List<ItemFlow> itemFlowList = new List<ItemFlow>();

                                ItemFlow itemFlow = new ItemFlow();

                                CopyProperty(woItemFlow, itemFlow);
                                orderTracerList.Add(ot);
                                itemFlow.ReqQty = qty;
                                itemFlow.OrderQty = itemFlow.ReqQty;
                                itemFlow.OrderTracers = orderTracerList;
                                itemFlowList.Add(itemFlow);
                                orders.ItemFlows = itemFlowList;

                                result.Add(orders);

                                orderTracerList = new List<OrderTracer>();
                                #endregion
                            }
                            else
                            {
                                orderTracerList.Add(ot);
                            }
                        }
                    }
                }
            }
            #endregion

            return result;
        }

        #region Data check
        private void DataValidCheck(ItemFlow itemFlow)
        {
            if (itemFlow.Flow == null)
            {
                throw new BusinessException("Flow is key infomation, it can't be empty!");
            }
        }
        private void DataValidCheck(Flow flow)
        {
            if (flow.FlowStrategy == null)
            {
                throw new BusinessException("FlowStrategy is key infomation, just tell me the strategy!");
            }
        }
        #endregion

        #region Setting properties
        private void SetFlowProperty(ItemFlow itemFlow, List<Flow> flows)
        {
            Flow flow = flows.Single(f => f.Equals(itemFlow.Flow));
            itemFlow.Flow.WindowTime = flow.WindowTime;
            itemFlow.Flow.NextOrderTime = flow.NextOrderTime;
            itemFlow.Flow.NextWindowTime = flow.NextWindowTime;
            itemFlow.Flow.IsUpdateWindowTime = flow.IsUpdateWindowTime;
        }
        #endregion

        #region Goto Processor
        private void ProcessTime(List<Flow> flows)
        {
            if (flows != null && flows.Count > 0)
            {
                foreach (var flow in flows)
                {
                    this.DataValidCheck(flow);
                    IOAE processor = this.GetProcessor(flow.FlowStrategy.Strategy);
                    if (processor != null)
                        processor.ProcessTime(flow);
                }
            }
        }

        private void ProcessReqQty(ItemFlow itemFlow)
        {
            IOAE processor = this.GetProcessor(itemFlow.Flow.FlowStrategy.Strategy);
            if (processor != null)
                processor.ProcessReqQty(itemFlow);
        }

        private void ProcessOrderQty(ItemFlow itemFlow)
        {
            if (itemFlow == null || itemFlow.ReqQty <= 0)
                return;

            decimal orderQty = itemFlow.ReqQty;
            decimal minLotSize = itemFlow.MinLotSize;//Min qty to order
            decimal UC = itemFlow.UC;//Unit container
            Enumerators.RoundUp roundUp = itemFlow.RoundUp;//Round up option
            //decimal orderLotSize = itemFlow.OrderLotSize;//Order lot size, one to many

            //Min lot size to order
            if (minLotSize > 0 && orderQty < minLotSize)
            {
                orderQty = minLotSize;
            }

            //round up
            if (UC > 0)
            {
                if (roundUp == Enumerators.RoundUp.Ceiling)
                {
                    orderQty = Math.Ceiling(orderQty / UC) * UC;
                }
                else if (roundUp == Enumerators.RoundUp.Floor)
                {
                    orderQty = Math.Floor(orderQty / UC) * UC;
                }
            }
            itemFlow.OrderQty = orderQty;

            if (itemFlow.OrderQty != itemFlow.ReqQty)
            {
                OrderTracer orderTracer = new OrderTracer();
                orderTracer.TracerType = Enumerators.TracerType.Adj;
                orderTracer.Item = itemFlow.Item;
                orderTracer.Qty = itemFlow.OrderQty - itemFlow.ReqQty;
                orderTracer.Code = roundUp == Enumerators.RoundUp.Ceiling ? "RoundUp-Ceiling" : "RoundUp-Floor";
                orderTracer.ReqTime = DateTime.Now;
                orderTracer.OrderedQty = itemFlow.ReqQty;
                itemFlow.OrderTracers.Add(orderTracer);
            }
        }

        private List<decimal> SplitOrderByLotSize(decimal orderQty, decimal orderLotSize)
        {
            if (orderQty <= 0 || orderLotSize <= 0)
                return null;

            List<decimal> orderQtyList = new List<decimal>();
            if (orderLotSize > 0)
            {
                int count = (int)Math.Floor(orderQty / orderLotSize);
                for (int i = 0; i < count; i++)
                {
                    orderQtyList.Add(orderLotSize);
                }

                decimal oddQty = orderQty % orderLotSize;
                if (oddQty > 0)
                {
                    orderQtyList.Add(oddQty);
                }
            }

            return orderQtyList;
        }

        private ItemFlow BidItemFlow(List<ItemFlow> itemFlows)
        {
            if (itemFlows == null || itemFlows.Count == 0)
                return null;

            var q1 = itemFlows.Where(i => i.Flow.OrderTime <= DateTime.Now && i.ReqQty > 0).ToList();
            if (q1.Count == 0)
                return null;

            if (q1.Count == 1)
                return q1[0];

            //todo
            return q1[0];
        }
        #endregion

        #region Build Orders
        private List<Orders> BuildOrders(List<ItemFlow> itemFlows, bool isEmergency)
        {
            if (itemFlows == null || itemFlows.Count == 0)
                return null;

            var query =
                from i in itemFlows
                where i.IsEmergency == isEmergency || (!isEmergency && i.Flow.IsUpdateWindowTime)
                group i by i.Flow into g
                select new Orders
                {
                    Flow = g.Key,
                    IsEmergency = isEmergency,
                    ItemFlows = g.Where(i => i.OrderQty > 0 && i.IsEmergency == isEmergency).ToList(),
                    WindowTime = this.GetWindowTime(g.Key, isEmergency),
                    StartTime = this.GetStartTime(g.Key, isEmergency)
                };

            return query.ToList();
        }
        private DateTime GetWindowTime(Flow flow, bool isEmergency)
        {
            if (isEmergency)
                return DateTime.Now.AddHours(flow.FlowStrategy.EmLeadTime);
            else
                return flow.WindowTime.HasValue ? flow.WindowTime.Value : DateTime.Now.AddHours(flow.FlowStrategy.LeadTime);
        }
        private DateTime GetStartTime(Flow flow, bool isEmergency)
        {
            if (isEmergency)
            {
                return DateTime.Now;
            }
            else
            {
                DateTime windowTime = this.GetWindowTime(flow, isEmergency);
                return windowTime.AddHours(-flow.FlowStrategy.LeadTime);
            }
        }

        private static void CopyProperty(object sourceObj, object targetObj)
        {
            PropertyInfo[] sourcePropertyInfoAry = sourceObj.GetType().GetProperties();
            PropertyInfo[] targetPropertyInfoAry = targetObj.GetType().GetProperties();

            foreach (PropertyInfo sourcePropertyInfo in sourcePropertyInfoAry)
            {
                foreach (PropertyInfo targetPropertyInfo in targetPropertyInfoAry)
                {
                    if (sourcePropertyInfo.Name == targetPropertyInfo.Name)
                    {
                        if (targetPropertyInfo.CanWrite && sourcePropertyInfo.CanRead)
                        {
                            targetPropertyInfo.SetValue(targetObj, sourcePropertyInfo.GetValue(sourceObj, null), null);
                        }
                    }
                }
            }
        }

        #endregion
    }
}

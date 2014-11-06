using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeanEngine.Entity;
using LeanEngine.Utility;

namespace LeanEngine.OAE
{
    public class MRP : OAEBase
    {
        public MRP(List<Plans> Plans, List<InvBalance> InvBalances, List<DemandChain> DemandChains)
            : base(Plans, InvBalances, DemandChains)
        {
        }

        public override void ProcessReqQty(ItemFlow itemFlow)
        {
            //ReqQty? It's too early for this
            DateTime? orderTime = itemFlow.Flow.OrderTime;
            if (orderTime.HasValue && orderTime.Value > DateTime.Now)
                return;

            base.ProcessReqQty(itemFlow);
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
                var demands = this.GetOrderMrp(loc, item, winTime, nextWinTime, Enumerators.TracerType.MRP);
                itemFlow.AddOrderTracer(demands);
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
                var orderRcts = this.GetOrderRct(loc, item, null, winTime);
                itemFlow.AddOrderTracer(orderRcts);
                #endregion

                #region OrderIss
                DateTime? startTime = null;
                if (true)//todo,config
                {
                    startTime = orderTime;
                }
                var orderIsss = this.GetOrderIss(loc, item, startTime, winTime);
                itemFlow.AddOrderTracer(orderIsss);
                #endregion
            }
            decimal reqQty = this.GetReqQty(itemFlow.OrderTracers);
            return reqQty;
        }

    }
}

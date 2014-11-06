using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeanEngine.Utility
{
    /// <summary>
    /// Define Enum types
    /// </summary>
    public static class Enumerators
    {
        /// <summary>
        /// Procurement: purchase
        /// Distribution: sales
        /// </summary>
        public enum FlowType
        {
            Procurement,
            Production,
            Distribution,
            Transfer
        }

        /// <summary>
        /// Issue or receipt?
        /// </summary>
        public enum IRType
        {
            ISS,
            RCT,
            MRP
        }

        /// <summary>
        /// Orders is confirmed demand and will be executed, but Plans is not
        /// </summary>
        public enum PlanType
        {
            Plans,
            Orders
        }

        /// <summary>
        /// Flow strategy:KB/JIT
        /// </summary>
        public enum Strategy
        {
            /// <summary>
            /// Manual
            /// </summary>
            Manual,
            /// <summary>
            /// Kanban
            /// </summary>
            KB,
            /// <summary>
            /// Order Point Method
            /// </summary>
            ODP,
            /// <summary>
            /// Just In Time
            /// </summary>
            JIT,
            /// <summary>
            /// Material Requirement Planning
            /// </summary>
            MRP,
            /// <summary>
            /// Trade
            /// </summary>
            TRD,
            /// <summary>
            /// WO
            /// </summary>
            WO,
            /// <summary>
            /// FaLiao
            /// </summary>
            FaLiao
        }

        /// <summary>
        /// Time unit:hours/day/week/month
        /// </summary>
        public enum TimeUnit
        {
            /// <summary>
            /// Default:hours
            /// </summary>
            Default,
            Day,
            Week,
            Month,
            Quarter,
            Year
        }

        /// <summary>
        /// Round up option
        /// </summary>
        public enum RoundUp
        {
            None,
            Ceiling,
            Floor
        }

        /// <summary>
        /// Inventory type
        /// </summary>
        public enum InvType
        {
            Normal,
            Inspect
        }

        /// <summary>
        /// Order Tracer Type
        /// </summary>
        public enum TracerType
        {
            Demand,//+  需求量,KA时此值是最大库存; JIT时此值是某一段时间(下一次窗口时间到下下次窗口时间的订单待发)的库存
            OnhandInv,//-   当前库存
            InspectInv,//-  检验库存
            OrderRct,//-    订单代收
            OrderIss,//+    订单待发
            PlanRct,//- 计划代收
            PlanIss,//+ 计划待发
            Adj,
			MRP
        }
    }
}

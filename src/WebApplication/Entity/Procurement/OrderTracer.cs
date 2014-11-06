using System;

//TODO: Add other using statements here

namespace com.Sconit.Entity.Procurement
{
    [Serializable]
    public class OrderTracer : OrderTracerBase
    {
        #region Non O/R Mapping Properties

        public decimal? CurrentQty { get; set; }

        #endregion
    }
}
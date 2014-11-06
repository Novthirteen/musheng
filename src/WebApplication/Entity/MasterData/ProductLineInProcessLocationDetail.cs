using System;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class ProductLineInProcessLocationDetail : ProductLineInProcessLocationDetailBase
    {
        #region Non O/R Mapping Properties
        public decimal CurrentBackflushQty { get; set; }
        public decimal RemainQty
        {
            get
            {
                return this.Qty - this.BackflushQty;
            }
        }
        #endregion
    }
}
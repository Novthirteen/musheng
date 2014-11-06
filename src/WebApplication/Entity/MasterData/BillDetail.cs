using System;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class BillDetail : BillDetailBase
    {
        #region Non O/R Mapping Properties

        public decimal DiscountRate
        {
            get
            {
                return ((this.Discount.HasValue ? this.Discount.Value : 0) / (((this.BilledQty * this.UnitPrice) == decimal.Zero) ? decimal.One : (this.BilledQty * this.UnitPrice))) * 100;
            }
        }
        public DateTime EffectiveDate
        {
            get
            {
                return this.ActingBill.EffectiveDate;
            }
        }

        //public decimal Amount
        //{
        //    get
        //    {
        //        return (this.BilledQty * this.UnitPrice - (this.Discount.HasValue ? this.Discount.Value : 0));
        //    }
        //}

        private Item _item;
        public Item Item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
            }
        }
        private Uom _uom;
        public Uom Uom
        {
            get
            {
                return _uom;
            }
            set
            {
                _uom = value;
            }
        }
        private decimal _billQty;
        public decimal BillQty
        {
            get
            {
                return _billQty;
            }
            set
            {
                _billQty = value;
            }
        }
        public decimal GroupAmount
        {
            get;
            set;
        }
        #endregion
    }
}
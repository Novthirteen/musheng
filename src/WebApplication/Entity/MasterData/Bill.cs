using System;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class Bill : BillBase
    {
        #region Non O/R Mapping Properties
        public void AddBillDetail(BillDetail billDetail)
        {
            if (this.BillDetails == null)
            {
                this.BillDetails = new List<BillDetail>();
            }
            this.BillDetails.Add(billDetail);
        }

        public void RemoveBillDetail(BillDetail billDetail)
        {
            if (this.BillDetails != null)
            {
                this.BillDetails.Remove(billDetail);
            }
        }

        public decimal TotalBillDetailAmount
        {
            get
            {
                decimal billDetailAmount = 0;
                if (this.BillDetails != null)
                {
                    foreach (BillDetail billDetail in this.BillDetails)
                    {
                        billDetailAmount += billDetail.Amount - (billDetail.Discount.HasValue ? billDetail.Discount.Value : 0);
                    }
                }
                return billDetailAmount;
            }
        }

        public decimal TotalBillAmount
        {
            get
            {
                return this.TotalBillDetailAmount - (this.Discount.HasValue ? this.Discount.Value : 0);
            }
        }

        public decimal TotalBillDiscountRate
        {
            get
            {
                if (TotalBillDetailAmount == 0)
                {
                    return 0;
                }
                return (this.Discount.HasValue ? this.Discount.Value : 0) / (TotalBillDetailAmount == decimal.Zero ? decimal.One : TotalBillDetailAmount) * 100;
            }
        }

        public decimal Amount { get; set; }
        #endregion
    }
}
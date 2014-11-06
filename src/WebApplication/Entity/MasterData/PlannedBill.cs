using System;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class PlannedBill : PlannedBillBase
    {
        #region Non O/R Mapping Properties

        private decimal _currentActingQty;
        public decimal CurrentActingQty
        {
            get
            {
                return this._currentActingQty;
            }
            set
            {
                this._currentActingQty = value;
            }
        }

        private decimal _currentBillAmount;
        public decimal CurrentBillAmount
        {
            get
            {
                return this._currentBillAmount;
            }
            set
            {
                this._currentBillAmount = value;
            }
        }
        #endregion
    }
}
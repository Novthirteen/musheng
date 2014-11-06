using System;

//TODO: Add other using statements here

namespace com.Sconit.Entity.Cost
{
    [Serializable]
    public class FgCost : FgCostBase
    {
        #region Non O/R Mapping Properties

        //TODO: Add Non O/R Mapping Properties here. 
        public double Amount
        {
            get { return this.OutQty * this.Cost; }
        }

        public double TotalAmount
        {
            get { return this.OutQty * TotalCost; }
        }

        public double TotalCost
        {
            get { return this.Cost + this.Allocation; }
        }
        #endregion
    }
}
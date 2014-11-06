using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.Cost
{
    [Serializable]
    public abstract class CbomBase : EntityBase
    {
        #region O/R Mapping Properties
		
		public Int32 Id { get; set; }
		public string Bom { get; set; }
		public string Item { get; set; }
		public string Uom { get; set; }
		public Int32 BomLevel { get; set; }
		public string ItemCategory { get; set; }
        public Double AccumQty { get; set; }
        public Double InQty { get; set; }
        public Double ScraptQty { get; set; }
		public Double Cost { get; set; }
        public Double Allocation { get; set; }
        public string FinanceCalendar { get; set; }
        public bool IsProvEst { get; set; }
		public DateTime CreateDate { get; set; }
		public string CreateUser { get; set; }
        public string FGCategory { get; set; }
        
        #endregion

		public override int GetHashCode()
        {
			if (Id != 0)
            {
                return Id.GetHashCode();
            }
            else
            {
                return base.GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            CbomBase another = obj as CbomBase;

            if (another == null)
            {
                return false;
            }
            else
            {
            	return (this.Id == another.Id);
            }
        } 
    }
	
}

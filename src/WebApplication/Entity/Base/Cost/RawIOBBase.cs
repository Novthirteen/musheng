using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.Cost
{
    [Serializable]
    public abstract class RawIOBBase : EntityBase
    {
        #region O/R Mapping Properties

        public Int32 Id { get; set; }
        public string Item { get; set; }
        public string Uom { get; set; }
        public Double StartQty { get; set; }
        public Double StartAmount { get; set; }
        public Double StartCost { get; set; }
        public Double InQty { get; set; }
        public Double InAmount { get; set; }
        public Double InCost { get; set; }
        public Double DiffAmount { get; set; }
        public Double DiffCost { get; set; }
        public Double EndQty { get; set; }
        public Double EndAmount { get; set; }
        public Double EndCost { get; set; }
        public string FinanceCalendar { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public DateTime LastModifyTime { get; set; }
        public string LastModifyUser { get; set; }

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
            RawIOBBase another = obj as RawIOBBase;

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

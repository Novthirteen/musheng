using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.Cost
{
    [Serializable]
    public abstract class SalesBase : EntityBase
    {
        #region O/R Mapping Properties

        public Int32 Id { get; set; }
        public string Item { get; set; }
        public string Uom { get; set; }
        public string Customer { get; set; }
        public Double Qty { get; set; }
        public Double Balance { get; set; }
        public string FinanceCalendar { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }

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
            SalesBase another = obj as SalesBase;

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

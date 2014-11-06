using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.Cost
{
    [Serializable]
    public abstract class BomTreeBase : EntityBase
    {
        #region O/R Mapping Properties

        public Int32 Id { get; set; }
        public string Bom { get; set; }
        public string BomDesc { get; set; }
        public string Item { get; set; }
        public string ItemDesc { get; set; }
        public Decimal RateQty { get; set; }
        public string Uom { get; set; }
        public Int32 BomLevel { get; set; }
        public string ItemCategoryCode { get; set; }
        public string ItemCategoryDesc { get; set; }
        public Decimal AccumQty { get; set; }
        public DateTime CreateTime { get; set; }
        public string FG { get; set; }
        public string FGDesc { get; set; }
        public string FGUom { get; set; }
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
            BomTreeBase another = obj as BomTreeBase;

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

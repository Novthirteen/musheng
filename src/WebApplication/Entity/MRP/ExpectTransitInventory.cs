using System;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MRP
{
    [Serializable]
    public class ExpectTransitInventory : ExpectTransitInventoryBase
    {
        #region Non O/R Mapping Properties

        public DateTime OrgStartTime { get; set; }
        public DateTime OrgWindowTime { get; set; }

        #endregion
    }
}
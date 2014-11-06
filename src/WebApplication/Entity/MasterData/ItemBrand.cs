using System;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class ItemBrand : ItemBrandBase
    {
        #region Non O/R Mapping Properties

        public string FullDescription
        {
            get
            {
                return Abbreviation + " [" + Description + "]";
            }
        }

        #endregion
    }
}
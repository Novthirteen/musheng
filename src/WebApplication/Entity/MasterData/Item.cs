using System;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class Item : ItemBase
    {
        #region Non O/R Mapping Properties

        public string Description
        {
            get
            {
                //return Desc1;
                return ((Desc1 != null ? Desc1 : string.Empty) + ((Desc2 != null && Desc2 != string.Empty) ? "[" + Desc2 + "]" : string.Empty));
            }

        }

        public string Description1
        {
            get
            {
                //return Desc1;
                return ((Desc1 != null ? Desc1 : string.Empty) + ((Desc2 != null && Desc2 != string.Empty) ? Desc2 : string.Empty));
            }

        }

        public string SortLevel1
        {
            get
            {
                string _SortLevel1 = null;
                if (this.IsSortAndColor.HasValue && this.IsSortAndColor.Value)
                {
                    _SortLevel1 = ((this.SortLevel1From != null ? this.SortLevel1From : string.Empty) + "-"
                        + ((this.SortLevel1To != null) ? this.SortLevel1To : string.Empty));
                }
                return _SortLevel1;
            }
        }

        public string SortLevel2
        {
            get
            {
                string _SortLevel2 = null;
                if (this.IsSortAndColor.HasValue && this.IsSortAndColor.Value)
                {
                    _SortLevel2 = ((this.SortLevel2From != null ? this.SortLevel2From : string.Empty) + "-"
                        + ((this.SortLevel2To != null) ? this.SortLevel2To : string.Empty));
                }
                return _SortLevel2;
            }
        }


        public string ColorLevel1
        {
            get
            {
                string _ColorLevel1 = null;
                if (this.IsSortAndColor.HasValue && this.IsSortAndColor.Value)
                {
                    _ColorLevel1 = ((this.ColorLevel1From != null ? this.ColorLevel1From : string.Empty) + "-"
                        + ((this.ColorLevel1To != null) ? this.ColorLevel1To : string.Empty));
                }
                return _ColorLevel1;
            }
        }

        public string ColorLevel2
        {
            get
            {
                string _ColorLevel2 = null;
                if (this.IsSortAndColor.HasValue && this.IsSortAndColor.Value)
                {
                    _ColorLevel2 = ((this.ColorLevel2From != null ? this.ColorLevel2From : string.Empty) + "-"
                            + ((this.ColorLevel2To != null) ? this.ColorLevel2To : string.Empty));
                }
                return _ColorLevel2;
            }
        }

        public bool IsBlank { get; set; }

        public string DefaultBomCode
        {
            get
            {
                if (this.Bom != null)
                {
                    return Bom.Code;
                }
                else
                {
                    return Code;
                }
            }

        }
        #endregion
    }
}
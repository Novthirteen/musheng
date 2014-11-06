using System;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class BomDetail : BomDetailBase
    {
        #region Non O/R Mapping Properties

        //选装件分组标记，默认取父件的BomCode
        private string _optionalItemGroup;
        public string OptionalItemGroup
        {
            set
            {
                this._optionalItemGroup = value;
            }
            get
            {
                return this._optionalItemGroup;
            }
        }

        private decimal _calculatedQty = 1;
        public decimal CalculatedQty
        {
            set
            {
                this._calculatedQty = value;
            }
            get
            {
                return this._calculatedQty;
            }
        }

        public decimal CalculatedQtyWithoutScrapRate { get; set; }

        //默认flow上设置的库位
        private Location _defaultLocation;
        public Location DefaultLocation
        {
            set
            {
                this._defaultLocation = value;
            }
            get
            {
                return this._defaultLocation;
            }
        }
        #endregion

        public decimal DefaultScrapPercentage
        {
            get
            {
                if (this.ScrapPercentage.HasValue && this.ScrapPercentage.Value > 0)
                {
                    return this.ScrapPercentage.Value;
                }
                else if (this.Item.ScrapPercentage.HasValue)
                {
                    return this.Item.ScrapPercentage.Value;
                }

                return 0;
            }
        }

        private string _scrapPctString;
        public string ScrapPctString
        {
            get
            {
                return ScrapPercentage.HasValue ? ScrapPercentage.Value.ToString() : string.Empty;
            }

            set
            {
                _scrapPctString = value;
            }

        }

        //public string DefaultSortLevel1From
        //{
        //    get
        //    {
        //        if (this.SortLevel1From != null && this.SortLevel1From.Trim() != string.Empty)
        //        {
        //            return this.SortLevel1From;
        //        }
        //        else
        //        {
        //            return this.Item.SortLevel1From;
        //        }
        //    }
        //}

        //public string DefaultSortLevel1To
        //{
        //    get
        //    {
        //        if (this.SortLevel1To != null && this.SortLevel1To.Trim() != string.Empty)
        //        {
        //            return this.SortLevel1To;
        //        }
        //        else
        //        {
        //            return this.Item.SortLevel1To;
        //        }
        //    }
        //}

        //public string DefaultColorLevel1From
        //{
        //    get
        //    {
        //        if (this.ColorLevel1From != null && this.ColorLevel1From.Trim() != string.Empty)
        //        {
        //            return this.ColorLevel1From;
        //        }
        //        else
        //        {
        //            return this.Item.ColorLevel1From;
        //        }
        //    }
        //}

        //public string DefaultColorLevel1To
        //{
        //    get
        //    {
        //        if (this.ColorLevel1To != null && this.ColorLevel1To.Trim() != string.Empty)
        //        {
        //            return this.ColorLevel1To;
        //        }
        //        else
        //        {
        //            return this.Item.ColorLevel1To;
        //        }
        //    }
        //}

        //public string DefaultSortLevel2From
        //{
        //    get
        //    {
        //        if (this.SortLevel2From != null && this.SortLevel2From.Trim() != string.Empty)
        //        {
        //            return this.SortLevel2From;
        //        }
        //        else
        //        {
        //            return this.Item.SortLevel2From;
        //        }
        //    }
        //}

        //public string DefaultSortLevel2To
        //{
        //    get
        //    {
        //        if (this.SortLevel2To != null && this.SortLevel2To.Trim() != string.Empty)
        //        {
        //            return this.SortLevel2To;
        //        }
        //        else
        //        {
        //            return this.Item.SortLevel2To;
        //        }
        //    }
        //}

        //public string DefaultColorLevel2From
        //{
        //    get
        //    {
        //        if (this.ColorLevel2From != null && this.ColorLevel2From.Trim() != string.Empty)
        //        {
        //            return this.ColorLevel2From;
        //        }
        //        else
        //        {
        //            return this.Item.ColorLevel2From;
        //        }
        //    }
        //}

        //public string DefaultColorLevel2To
        //{
        //    get
        //    {
        //        if (this.ColorLevel2To != null && this.ColorLevel2To.Trim() != string.Empty)
        //        {
        //            return this.ColorLevel2To;
        //        }
        //        else
        //        {
        //            return this.Item.ColorLevel2To;
        //        }
        //    }
        //}
        public int BomLevel { get; set; }

        public decimal AccumQty { get; set; }
    }
}
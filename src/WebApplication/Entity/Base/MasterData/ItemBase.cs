using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class ItemBase : EntityBase
    {
        #region O/R Mapping Properties

        private string _code;
        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
            }
        }
        private Int32? _huLotSize;
        public Int32? HuLotSize
        {
            get
            {
                return _huLotSize;
            }
            set
            {
                _huLotSize = value;
            }
        }
        private string _type;
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
        private string _desc1;
        public string Desc1
        {
            get
            {
                return _desc1;
            }
            set
            {
                _desc1 = value;
            }
        }
        private string _desc2;
        public string Desc2
        {
            get
            {
                return _desc2;
            }
            set
            {
                _desc2 = value;
            }
        }
        private com.Sconit.Entity.MasterData.Uom _uom;
        public com.Sconit.Entity.MasterData.Uom Uom
        {
            get
            {
                return _uom;
            }
            set
            {
                _uom = value;
            }
        }
        private Decimal _unitCount;
        public Decimal UnitCount
        {
            get
            {
                return _unitCount;
            }
            set
            {
                _unitCount = value;
            }
        }
        private com.Sconit.Entity.MasterData.Location _location;
        public com.Sconit.Entity.MasterData.Location Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }
        private string _imageUrl;
        public string ImageUrl
        {
            get
            {
                return _imageUrl;
            }
            set
            {
                _imageUrl = value;
            }
        }
        private com.Sconit.Entity.MasterData.Bom _bom;
        public com.Sconit.Entity.MasterData.Bom Bom
        {
            get
            {
                return _bom;
            }
            set
            {
                _bom = value;
            }
        }
        private com.Sconit.Entity.MasterData.Routing _routing;
        public com.Sconit.Entity.MasterData.Routing Routing
        {
            get
            {
                return _routing;
            }
            set
            {
                _routing = value;
            }
        }
        private Boolean _isActive;
        public Boolean IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
            }
        }
        private string _memo;
        public string Memo
        {
            get
            {
                return _memo;
            }
            set
            {
                _memo = value;
            }
        }
        private DateTime _lastModifyDate;
        public DateTime LastModifyDate
        {
            get
            {
                return _lastModifyDate;
            }
            set
            {
                _lastModifyDate = value;
            }
        }
        //private com.Sconit.Entity.MasterData.User _lastModifyUser;
        //[System.Xml.Serialization.XmlIgnoreAttribute()]
        //public com.Sconit.Entity.MasterData.User LastModifyUser
        //{
        //    get
        //    {
        //        return _lastModifyUser;
        //    }
        //    set
        //    {
        //        _lastModifyUser = value;
        //    }
        //}
        public string LastModifyUser { get; set; }

        public ItemCategory ItemCategory { get; set; }
        private Decimal? _scrapPercentage;
        public Decimal? ScrapPercentage
        {
            get
            {
                return _scrapPercentage;
            }
            set
            {
                _scrapPercentage = value;
            }
        }

        public ItemBrand ItemBrand { get; set; }

        private Decimal? _goodsReceiptLotSize;
        public Decimal? GoodsReceiptLotSize
        {
            get
            {
                return _goodsReceiptLotSize;
            }
            set
            {
                _goodsReceiptLotSize = value;
            }
        }

        private Decimal? _mrpLeadTime;
        public Decimal? MrpLeadTime
        {
            get
            {
                return _mrpLeadTime;
            }
            set
            {
                _mrpLeadTime = value;
            }
        }
        private Int32 _id;
        public Int32 Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public string DefaultFlow { get; set; }
        public string DefaultSupplier { get; set; }
        #endregion

        #region O/R Mapping Retention Properties
        private com.Sconit.Entity.MasterData.ItemType _category1;
        public com.Sconit.Entity.MasterData.ItemType Category1
        {
            get
            {
                return _category1;
            }
            set
            {
                _category1 = value;
            }
        }
        private com.Sconit.Entity.MasterData.ItemType _category2;
        public com.Sconit.Entity.MasterData.ItemType Category2
        {
            get
            {
                return _category2;
            }
            set
            {
                _category2 = value;
            }
        }
        private com.Sconit.Entity.MasterData.ItemType _category3;
        public com.Sconit.Entity.MasterData.ItemType Category3
        {
            get
            {
                return _category3;
            }
            set
            {
                _category3 = value;
            }
        }
        private com.Sconit.Entity.MasterData.ItemType _category4;
        public com.Sconit.Entity.MasterData.ItemType Category4
        {
            get
            {
                return _category4;
            }
            set
            {
                _category4 = value;
            }
        }
        private com.Sconit.Entity.MasterData.ItemType _category5;
        public com.Sconit.Entity.MasterData.ItemType Category5
        {
            get
            {
                return _category5;
            }
            set
            {
                _category5 = value;
            }
        }

        public string ScrapBillAddress { get; set; }
        public decimal? SalesCost { get; set; }
        public decimal? PinNumber { get; set; }
        public decimal? HistoryPrice { get; set; }
        public decimal? ScrapPrice { get; set; }
        public Boolean? NeedInspect { get; set; }
        public Boolean? IsRunMrp { get; set; }
        public Boolean? IsSortAndColor { get; set; }
        //分光1开始
        private string _sortLevel1From;
        public string SortLevel1From
        {
            get
            {
                return _sortLevel1From;
            }
            set
            {
                _sortLevel1From = value;
            }
        }
        //分光1结束
        private string _sortLevel1To;
        public string SortLevel1To
        {
            get
            {
                return _sortLevel1To;
            }
            set
            {
                _sortLevel1To = value;
            }
        }
        //分色1开始
        private string _colorLevel1From;
        public string ColorLevel1From
        {
            get
            {
                return _colorLevel1From;
            }
            set
            {
                _colorLevel1From = value;
            }
        }
        //分色1结束
        private string _colorLevel1To;
        public string ColorLevel1To
        {
            get
            {
                return _colorLevel1To;
            }
            set
            {
                _colorLevel1To = value;
            }
        }
        //分光2开始
        private string _sortLevel2From;
        public string SortLevel2From
        {
            get
            {
                return _sortLevel2From;
            }
            set
            {
                _sortLevel2From = value;
            }
        }
        //分光2结束
        private string _sortLevel2To;
        public string SortLevel2To
        {
            get
            {
                return _sortLevel2To;
            }
            set
            {
                _sortLevel2To = value;
            }
        }
        //分色2开始
        private string _colorLevel2From;
        public string ColorLevel2From
        {
            get
            {
                return _colorLevel2From;
            }
            set
            {
                _colorLevel2From = value;
            }
        }
        //分色2结束
        private string _colorLevel2To;
        public string ColorLevel2To
        {
            get
            {
                return _colorLevel2To;
            }
            set
            {
                _colorLevel2To = value;
            }
        }
        public string Msl { get; set; }
        public string Bin { get; set; }
        #endregion

        public override int GetHashCode()
        {
            if (Code != null)
            {
                return Code.GetHashCode();
            }
            else
            {
                return base.GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            ItemBase another = obj as ItemBase;

            if (another == null)
            {
                return false;
            }
            else
            {
                return string.Equals(this.Code, another.Code, StringComparison.OrdinalIgnoreCase);
            }
        }
    }

}

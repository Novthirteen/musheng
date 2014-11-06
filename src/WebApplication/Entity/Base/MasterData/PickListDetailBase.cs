using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class PickListDetailBase : EntityBase
    {
        #region O/R Mapping Properties
		
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
		private com.Sconit.Entity.MasterData.PickList _pickList;
		public com.Sconit.Entity.MasterData.PickList PickList
		{
			get
			{
				return _pickList;
			}
			set
			{
				_pickList = value;
			}
		}
        private com.Sconit.Entity.MasterData.OrderLocationTransaction _orderLocationTransaction;
        public com.Sconit.Entity.MasterData.OrderLocationTransaction OrderLocationTransaction
		{
			get
			{
                return _orderLocationTransaction;
			}
			set
			{
                _orderLocationTransaction = value;
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
		private com.Sconit.Entity.MasterData.StorageArea _storageArea;
		public com.Sconit.Entity.MasterData.StorageArea StorageArea
		{
			get
			{
				return _storageArea;
			}
			set
			{
				_storageArea = value;
			}
		}
		private com.Sconit.Entity.MasterData.StorageBin _storageBin;
		public com.Sconit.Entity.MasterData.StorageBin StorageBin
		{
			get
			{
				return _storageBin;
			}
			set
			{
				_storageBin = value;
			}
		}
		private com.Sconit.Entity.MasterData.Item _item;
		public com.Sconit.Entity.MasterData.Item Item
		{
			get
			{
				return _item;
			}
			set
			{
				_item = value;
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
		private string _huId;
		public string HuId
		{
			get
			{
				return _huId;
			}
			set
			{
				_huId = value;
			}
		}
		private string _lotNo;
		public string LotNo
		{
			get
			{
				return _lotNo;
			}
			set
			{
				_lotNo = value;
			}
		}
		private Decimal _qty;
		public Decimal Qty
		{
			get
			{
				return _qty;
			}
			set
			{
				_qty = value;
			}
		}

        private IList<PickListResult> _pickListResults;
        public IList<PickListResult> PickListResults
        {
            get
            {
                return _pickListResults;
            }
            set
            {
                _pickListResults = value;
            }
        }
        public String Memo { get; set; }
        #endregion

        #region O/R Mapping Retention Properties

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

        private Decimal? _numField1;
        public Decimal? NumField1
        {
            get
            {
                return _numField1;
            }
            set
            {
                _numField1 = value;
            }
        }
        private Decimal? _numField2;
        public Decimal? NumField2
        {
            get
            {
                return _numField2;
            }
            set
            {
                _numField2 = value;
            }
        }
        private Decimal? _numField3;
        public Decimal? NumField3
        {
            get
            {
                return _numField3;
            }
            set
            {
                _numField3 = value;
            }
        }
        private Decimal? _numField4;
        public Decimal? NumField4
        {
            get
            {
                return _numField4;
            }
            set
            {
                _numField4 = value;
            }
        }
        //private DateTime _manufactureDate;
        //public DateTime ManufactureDate
        //{
        //    get
        //    {
        //        return _manufactureDate;
        //    }
        //    set
        //    {
        //        _manufactureDate = value;
        //    }
        //}
        private DateTime? _dateField1;
        public DateTime? DateField1
        {
            get
            {
                return _dateField1;
            }
            set
            {
                _dateField1 = value;
            }
        }
        private DateTime? _dateField2;
        public DateTime? DateField2
        {
            get
            {
                return _dateField2;
            }
            set
            {
                _dateField2 = value;
            }
        }

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
            PickListDetailBase another = obj as PickListDetailBase;

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

using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class FlowDetailBase : EntityBase
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

		private com.Sconit.Entity.MasterData.Flow _flow;
        [System.Xml.Serialization.XmlIgnoreAttribute()]
		public com.Sconit.Entity.MasterData.Flow Flow
		{
			get
			{
				return _flow;
			}
			set
			{
				_flow = value;
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
        private com.Sconit.Entity.MasterData.Customer _customer;
        public com.Sconit.Entity.MasterData.Customer Customer
        {
            get
            {
                return _customer;
            }
            set
            {
                _customer = value;
            }
        }
        private string _referenceItemCode;
        public string ReferenceItemCode
        {
            get
            {
                return _referenceItemCode;
            }
            set
            {
                _referenceItemCode = value;
            }
        }
        private string _timeUnit;
        public string TimeUnit
        {
            get
            {
                return _timeUnit;
            }
            set
            {
                _timeUnit = value;
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
		private com.Sconit.Entity.MasterData.Bom _bom;
        [System.Xml.Serialization.XmlIgnoreAttribute()]
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
		private Int32 _sequence;
		public Int32 Sequence
		{
			get
			{
				return _sequence;
			}
			set
			{
				_sequence = value;
			}
		}
		private com.Sconit.Entity.MasterData.Location _locationFrom;
		public com.Sconit.Entity.MasterData.Location LocationFrom
		{
			get
			{
				return _locationFrom;
			}
			set
			{
				_locationFrom = value;
			}
		}
		private com.Sconit.Entity.MasterData.Location _locationTo;
		public com.Sconit.Entity.MasterData.Location LocationTo
		{
			get
			{
				return _locationTo;
			}
			set
			{
				_locationTo = value;
			}
		}
        private com.Sconit.Entity.MasterData.BillAddress _billAddress;
        public com.Sconit.Entity.MasterData.BillAddress BillAddress
		{
			get
			{
                return _billAddress;
			}
			set
			{
                _billAddress = value;
			}
		}
		private Boolean _isAutoCreate;
		public Boolean IsAutoCreate
		{
			get
			{
				return _isAutoCreate;
			}
			set
			{
				_isAutoCreate = value;
			}
		}
		private Decimal? _safeStock;
		public Decimal? SafeStock
		{
			get
			{
				return _safeStock;
			}
			set
			{
				_safeStock = value;
			}
		}
		private Decimal? _maxStock;
		public Decimal? MaxStock
		{
			get
			{
				return _maxStock;
			}
			set
			{
				_maxStock = value;
			}
		}
		private Decimal? _minLotSize;
		public Decimal? MinLotSize
		{
			get
			{
				return _minLotSize;
			}
			set
			{
				_minLotSize = value;
			}
		}
		private Decimal? _orderLotSize;
		public Decimal? OrderLotSize
		{
			get
			{
				return _orderLotSize;
			}
			set
			{
				_orderLotSize = value;
			}
		}
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
        private Decimal? _batchSize;
        public Decimal? BatchSize
        {
            get
            {
                return _batchSize;
            }
            set
            {
                _batchSize = value;
            }
        }
		private string _roundUpOption;
		public string RoundUpOption
		{
			get
			{
				return _roundUpOption;
			}
			set
			{
				_roundUpOption = value;
			}
		}
        private string _billSettleTerm;
        public string BillSettleTerm
        {
            get
            {
                return _billSettleTerm;
            }
            set
            {
                _billSettleTerm = value;
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
		private Decimal? _packageVolumn;
		public Decimal? PackageVolumn
		{
			get
			{
				return _packageVolumn;
			}
			set
			{
				_packageVolumn = value;
			}
		}
		private string _packageType;
		public string PackageType
		{
			get
			{
				return _packageType;
			}
			set
			{
				_packageType = value;
			}
		}
		private string _projectDescription;
		public string ProjectDescription
		{
			get
			{
				return _projectDescription;
			}
			set
			{
				_projectDescription = value;
			}
		}
		private string _remark;
		public string Remark
		{
			get
			{
				return _remark;
			}
			set
			{
				_remark = value;
			}
		}
        private com.Sconit.Entity.MasterData.PriceList _priceList;
        public com.Sconit.Entity.MasterData.PriceList PriceList
        {
            get
            {
                return _priceList;
            }
            set
            {
                _priceList = value;
            }
        }
        public Boolean NeedInspection { get; set; }
        public String IdMark { get; set; }
        public String BarCodeType { get; set; }
        public String CustomerItemCode { get; set; }
        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
            }
        }
        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;
            }
        }
        public String OddShipOption { get; set; }
        public String ExtraDmdSource { get; set; }
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
        private com.Sconit.Entity.MasterData.Routing _returnRouting;
        public com.Sconit.Entity.MasterData.Routing ReturnRouting
        {
            get
            {
                return _returnRouting;
            }
            set
            {
                _returnRouting = value;
            }
        }
        public Int32 MRPWeight { get; set; }
        #endregion

        #region O/R Mapping Retention Properties

        private string _textField1;
        public string TextField1     //ÅúºÅ
        {
            get
            {
                return _textField1;
            }
            set
            {
                _textField1 = value;
            }
        }
        private string _textField2;
        public string TextField2
        {
            get
            {
                return _textField2;
            }
            set
            {
                _textField2 = value;
            }
        }
        private string _textField3;
        public string TextField3
        {
            get
            {
                return _textField3;
            }
            set
            {
                _textField3 = value;
            }
        }
        private string _textField4;
        public string TextField4
        {
            get
            {
                return _textField4;
            }
            set
            {
                _textField4 = value;
            }
        }
        private string _textField5;
        public string TextField5
        {
            get
            {
                return _textField5;
            }
            set
            {
                _textField5 = value;
            }
        }
        private string _textField6;
        public string TextField6
        {
            get
            {
                return _textField6;
            }
            set
            {
                _textField6 = value;
            }
        }
        private string _textField7;
        public string TextField7
        {
            get
            {
                return _textField7;
            }
            set
            {
                _textField7 = value;
            }
        }
        private string _textField8;
        public string TextField8
        {
            get
            {
                return _textField8;
            }
            set
            {
                _textField8 = value;
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
        private Decimal? _numField5;
        public Decimal? NumField5
        {
            get
            {
                return _numField5;
            }
            set
            {
                _numField5 = value;
            }
        }
        private Decimal? _numField6;
        public Decimal? NumField6
        {
            get
            {
                return _numField6;
            }
            set
            {
                _numField6 = value;
            }
        }
        private Decimal? _numField7;
        public Decimal? NumField7
        {
            get
            {
                return _numField7;
            }
            set
            {
                _numField7 = value;
            }
        }
        private Decimal? _numField8;
        public Decimal? NumField8
        {
            get
            {
                return _numField8;
            }
            set
            {
                _numField8 = value;
            }
        }
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
        private DateTime? _dateField3;
        public DateTime? DateField3
        {
            get
            {
                return _dateField3;
            }
            set
            {
                _dateField3 = value;
            }
        }
        private DateTime? _dateField4;
        public DateTime? DateField4
        {
            get
            {
                return _dateField4;
            }
            set
            {
                _dateField4 = value;
            }
        }
        public String InspectLocationFrom { get; set; }
        public String InspectLocationTo { get; set; }
        public String RejectLocationFrom { get; set; }
        public String RejectLocationTo { get; set; }
        public String StorageBin { get; set; }
        public Boolean NeedRejectInspection { get; set; }
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
            FlowDetailBase another = obj as FlowDetailBase;

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

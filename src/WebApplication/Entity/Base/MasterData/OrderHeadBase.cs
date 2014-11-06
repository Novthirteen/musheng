using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class OrderHeadBase : EntityBase
    {
        #region O/R Mapping Properties
		
		private string _orderNo;
        [System.Xml.Serialization.SoapElement()]
		public string OrderNo
		{
			get
			{
				return _orderNo;
			}
			set
			{
				_orderNo = value;
			}
		}
		private string _referenceOrderNo;
		public string ReferenceOrderNo
		{
			get
			{
				return _referenceOrderNo;
			}
			set
			{
				_referenceOrderNo = value;
			}
		}
		private string _externalOrderNo;
		public string ExternalOrderNo
		{
			get
			{
				return _externalOrderNo;
			}
			set
			{
				_externalOrderNo = value;
			}
		}
		private Int32? _sequence;
		public Int32? Sequence
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
		private DateTime _startTime;
		public DateTime StartTime
		{
			get
			{
				return _startTime;
			}
			set
			{
				_startTime = value;
			}
		}
		private DateTime _windowTime;
		public DateTime WindowTime
		{
			get
			{
				return _windowTime;
			}
			set
			{
				_windowTime = value;
			}
		}
		private string _status;
		public string Status
		{
			get
			{
				return _status;
			}
			set
			{
				_status = value;
			}
		}
		private string _priority;
		public string Priority
		{
			get
			{
				return _priority;
			}
			set
			{
				_priority = value;
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
		private string _subType;
		public string SubType
		{
			get
			{
				return _subType;
			}
			set
			{
				_subType = value;
			}
		}
		private com.Sconit.Entity.MasterData.Party _partyFrom;
		public com.Sconit.Entity.MasterData.Party PartyFrom
		{
			get
			{
				return _partyFrom;
			}
			set
			{
				_partyFrom = value;
			}
		}
		private com.Sconit.Entity.MasterData.Party _partyTo;
		public com.Sconit.Entity.MasterData.Party PartyTo
		{
			get
			{
				return _partyTo;
			}
			set
			{
				_partyTo = value;
			}
		}
		private com.Sconit.Entity.MasterData.ShipAddress _shipFrom;
		public com.Sconit.Entity.MasterData.ShipAddress ShipFrom
		{
			get
			{
				return _shipFrom;
			}
			set
			{
				_shipFrom = value;
			}
		}
		private com.Sconit.Entity.MasterData.ShipAddress _shipTo;
		public com.Sconit.Entity.MasterData.ShipAddress ShipTo
		{
			get
			{
				return _shipTo;
			}
			set
			{
				_shipTo = value;
			}
		}
		private string _dockDescription;
		public string DockDescription
		{
			get
			{
				return _dockDescription;
			}
			set
			{
				_dockDescription = value;
			}
		}
		private Boolean _isAutoRelease;
		public Boolean IsAutoRelease
		{
			get
			{
				return _isAutoRelease;
			}
			set
			{
				_isAutoRelease = value;
			}
		}
		private Boolean _isAutoStart;
		public Boolean IsAutoStart
		{
			get
			{
				return _isAutoStart;
			}
			set
			{
				_isAutoStart = value;
			}
		}
        private Boolean _isAutoShip;
        public Boolean IsAutoShip
        {
            get
            {
                return _isAutoShip;
            }
            set
            {
                _isAutoShip = value;
            }
        }
        private Boolean _isAutoReceive;
        public Boolean IsAutoReceive
        {
            get
            {
                return _isAutoReceive;
            }
            set
            {
                _isAutoReceive = value;
            }
        }
		private Boolean _isAutoBill;
		public Boolean IsAutoBill
		{
			get
			{
				return _isAutoBill;
			}
			set
			{
				_isAutoBill = value;
			}
		}
        private Boolean _isShowPrice;
        public Boolean IsShowPrice
        {
            get
            {
                return _isShowPrice;
            }
            set
            {
                _isShowPrice = value;
            }
        }
		private Decimal? _startLatency;
		public Decimal? StartLatency
		{
			get
			{
				return _startLatency;
			}
			set
			{
				_startLatency = value;
			}
		}
		private Decimal? _completeLatency;
		public Decimal? CompleteLatency
		{
			get
			{
				return _completeLatency;
			}
			set
			{
				_completeLatency = value;
			}
		}
		private Boolean _needPrintOrder;
		public Boolean NeedPrintOrder
		{
			get
			{
				return _needPrintOrder;
			}
			set
			{
				_needPrintOrder = value;
			}
		}
		private Boolean _needPrintAsn;
		public Boolean NeedPrintAsn
		{
			get
			{
				return _needPrintAsn;
			}
			set
			{
				_needPrintAsn = value;
			}
		}
		private Boolean _needPrintReceipt;
		public Boolean NeedPrintReceipt
		{
			get
			{
				return _needPrintReceipt;
			}
			set
			{
				_needPrintReceipt = value;
			}
		}
		private string _goodsReceiptGapTo;
		public string GoodsReceiptGapTo
		{
			get
			{
				return _goodsReceiptGapTo;
			}
			set
			{
				_goodsReceiptGapTo = value;
			}
		}
		private Boolean _allowExceed;
		public Boolean AllowExceed
		{
			get
			{
				return _allowExceed;
			}
			set
			{
				_allowExceed = value;
			}
		}
        private Boolean _allowCreateDetail;
        public Boolean AllowCreateDetail
        {
            get
            {
                return _allowCreateDetail;
            }
            set
            {
                _allowCreateDetail = value;
            }
        }
		private Boolean _isPrinted;
		public Boolean IsPrinted
		{
			get
			{
				return _isPrinted;
			}
			set
			{
				_isPrinted = value;
			}
		}
		private string _orderTemplate;
		public string OrderTemplate
		{
			get
			{
                return _orderTemplate;
			}
			set
			{
                _orderTemplate = value;
			}
		}
        private string _asnTemplate;
        public string AsnTemplate
        {
            get
            {
                return _asnTemplate;
            }
            set
            {
                _asnTemplate = value;
            }
        }
        private string _receiptTemplate;
        public string ReceiptTemplate
        {
            get
            {
                return _receiptTemplate;
            }
            set
            {
                _receiptTemplate = value;
            }
        }
        private string _huTemplate;
        public string HuTemplate
        {
            get
            {
                return _huTemplate;
            }
            set
            {
                _huTemplate = value;
            }
        }
		private DateTime _createDate;
		public DateTime CreateDate
		{
			get
			{
				return _createDate;
			}
			set
			{
				_createDate = value;
			}
		}
		private com.Sconit.Entity.MasterData.User _createUser;
		public com.Sconit.Entity.MasterData.User CreateUser
		{
			get
			{
				return _createUser;
			}
			set
			{
				_createUser = value;
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
		private com.Sconit.Entity.MasterData.User _lastModifyUser;
		public com.Sconit.Entity.MasterData.User LastModifyUser
		{
			get
			{
				return _lastModifyUser;
			}
			set
			{
				_lastModifyUser = value;
			}
		}
		private DateTime? _releaseDate;
		public DateTime? ReleaseDate
		{
			get
			{
				return _releaseDate;
			}
			set
			{
				_releaseDate = value;
			}
		}
		private com.Sconit.Entity.MasterData.User _releaseUser;
		public com.Sconit.Entity.MasterData.User ReleaseUser
		{
			get
			{
				return _releaseUser;
			}
			set
			{
				_releaseUser = value;
			}
		}
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
		private com.Sconit.Entity.MasterData.User _startUser;
		public com.Sconit.Entity.MasterData.User StartUser
		{
			get
			{
				return _startUser;
			}
			set
			{
				_startUser = value;
			}
		}
		private DateTime? _completeDate;
		public DateTime? CompleteDate
		{
			get
			{
				return _completeDate;
			}
			set
			{
				_completeDate = value;
			}
		}
		private com.Sconit.Entity.MasterData.User _completeUser;
		public com.Sconit.Entity.MasterData.User CompleteUser
		{
			get
			{
				return _completeUser;
			}
			set
			{
				_completeUser = value;
			}
		}
        private String _checkDetailOption;
        public String CheckDetailOption
        {
            get
            {
                return _checkDetailOption;
            }
            set
            {
                _checkDetailOption = value;
            }
        }
		private DateTime? _closeDate;
		public DateTime? CloseDate
		{
			get
			{
				return _closeDate;
			}
			set
			{
				_closeDate = value;
			}
		}
		private com.Sconit.Entity.MasterData.User _closeUser;
		public com.Sconit.Entity.MasterData.User CloseUser
		{
			get
			{
				return _closeUser;
			}
			set
			{
				_closeUser = value;
			}
		}
		private DateTime? _cancelDate;
		public DateTime? CancelDate
		{
			get
			{
				return _cancelDate;
			}
			set
			{
				_cancelDate = value;
			}
		}
		private com.Sconit.Entity.MasterData.User _cancelUser;
		public com.Sconit.Entity.MasterData.User CancelUser
		{
			get
			{
				return _cancelUser;
			}
			set
			{
				_cancelUser = value;
			}
		}
		private string _cancelReason;
		public string CancelReason
		{
			get
			{
				return _cancelReason;
			}
			set
			{
				_cancelReason = value;
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
        private Decimal? _discount;
        public Decimal? Discount
        {
            get
            {
                return _discount;
            }
            set
            {
                _discount = value;
            }
        }
        private Currency _currency;
        public Currency Currency
        {
            get
            {
                return _currency;
            }
            set
            {
                _currency = value;
            }
        }
        private IList<OrderDetail> _orderDetails;
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public IList<OrderDetail> OrderDetails
        {
            get
            {
                return _orderDetails;
            }
            set
            {
                _orderDetails = value;
            }
        }
        private IList<OrderOperation> _orderOperations;
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public IList<OrderOperation> OrderOperations
        {
            get
            {
                return _orderOperations;
            }
            set
            {
                _orderOperations = value;
            }
        }
        private IList<OrderBinding> _orderBindings;
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public IList<OrderBinding> OrderBindings
        {
            get
            {
                return _orderBindings;
            }
            set
            {
                _orderBindings = value;
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
        private com.Sconit.Entity.MasterData.Routing _routing;
        [System.Xml.Serialization.XmlIgnoreAttribute()]
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
        private string _flow;
        public string Flow
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
        private com.Sconit.Entity.MasterData.Supplier _carrier;
        public com.Sconit.Entity.MasterData.Supplier Carrier
        {
            get
            {
                return _carrier;
            }
            set
            {
                _carrier = value;
            }
        }
        private com.Sconit.Entity.MasterData.BillAddress _carrierBillAddress;
        public com.Sconit.Entity.MasterData.BillAddress CarrierBillAddress
        {
            get
            {
                return _carrierBillAddress;
            }
            set
            {
                _carrierBillAddress = value;
            }
        }

        public Boolean FulfillUnitCount { get; set; }
        private Boolean _isShipScanHu;
        public Boolean IsShipScanHu
        {
            get
            {
                return _isShipScanHu;
            }
            set
            {
                _isShipScanHu = value;
            }
        }

        private Boolean _isReceiptScanHu;
        public Boolean IsReceiptScanHu
        {
            get
            {
                return _isReceiptScanHu;
            }
            set
            {
                _isReceiptScanHu = value;
            }
        }
        private Boolean _autoPrintHu;
        public Boolean AutoPrintHu
        {
            get
            {
                return _autoPrintHu;
            }
            set
            {
                _autoPrintHu = value;
            }
        }
        private Boolean _isOddCreateHu;
        public Boolean IsOddCreateHu
        {
            get
            {
                return _isOddCreateHu;
            }
            set
            {
                _isOddCreateHu = value;
            }
        }
        private string _createHuOption;
        public string CreateHuOption
        {
            get
            {
                return _createHuOption;
            }
            set
            {
                _createHuOption = value;
            }
        }
        private Boolean _isAutoCreatePickList;
        public Boolean IsAutoCreatePickList
        {
            get
            {
                return _isAutoCreatePickList;
            }
            set
            {
                _isAutoCreatePickList = value;
            }
        }
        public Boolean NeedInspection { get; set; }
        public com.Sconit.Entity.MasterData.Shift Shift { get; set; }
        public Boolean IsGoodsReceiveFIFO { get; set; }
        public Int32 MaxOnlineQty { get; set; }
        public Boolean IsNewItem {get;set;}
        public Boolean AllowRepeatlyExceed { get; set; }
        public Boolean IsPickFromBin { get; set; }
        public Boolean IsShipByOrder { get; set; }
        private bool _isAsnUniqueReceipt;
        public bool IsAsnUniqueReceipt
        {
            get
            {
                return _isAsnUniqueReceipt;
            }
            set
            {
                _isAsnUniqueReceipt = value;
            }
        }
        private bool _isSubcontract;
        public bool IsSubcontract
        {
            get
            {
                return _isSubcontract;
            }
            set
            {
                _isSubcontract = value;
            }
        }
        public String InspectLocationFrom { get; set; }
        public String InspectLocationTo { get; set; }
        public String RejectLocationFrom { get; set; }
        public String RejectLocationTo { get; set; }
        public Boolean NeedRejectInspection { get; set; }
        public String ProductLineFacility { get; set; }
        public Boolean NeedSortAndColor { get; set; }
        public Boolean HasSortAndColor { get; set; }
        public Boolean CheckRM { get; set; }
        public bool IsPickListCreated { get; set; }
        public DateTime? CheckRMDate { get; set; }
        public String CheckRMUser { get; set; }
        #endregion

        #region O/R Mapping Retention Properties

        private string _textField1;
        public string TextField1
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

        #endregion

        public override int GetHashCode()
        {
			if (OrderNo != null)
            {
                return OrderNo.GetHashCode();
            }
            else
            {
                return base.GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            OrderHeadBase another = obj as OrderHeadBase;

            if (another == null)
            {
                return false;
            }
            else
            {
            	return (this.OrderNo == another.OrderNo);
            }
        } 
    }
	
}

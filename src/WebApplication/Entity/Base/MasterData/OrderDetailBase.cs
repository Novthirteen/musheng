using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class OrderDetailBase : EntityBase
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
        private com.Sconit.Entity.MasterData.OrderHead _orderHead;
        public com.Sconit.Entity.MasterData.OrderHead OrderHead
        {
            get
            {
                return _orderHead;
            }
            set
            {
                _orderHead = value;
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

        public String CustomerItemCode { get; set; }

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
        private Decimal _requiredQty;
        public Decimal RequiredQty
        {
            get
            {
                return _requiredQty;
            }
            set
            {
                _requiredQty = value;
            }
        }
        private Decimal _orderedQty;
        public Decimal OrderedQty
        {
            get
            {
                return _orderedQty;
            }
            set
            {
                _orderedQty = value;
            }
        }
        private Decimal? _shippedQty;
        public Decimal? ShippedQty
        {
            get
            {
                return _shippedQty;
            }
            set
            {
                _shippedQty = value;
            }
        }
        private Decimal? _receivedQty;
        public Decimal? ReceivedQty
        {
            get
            {
                return _receivedQty;
            }
            set
            {
                _receivedQty = value;
            }
        }
        private Decimal? _rejectedQty;
        public Decimal? RejectedQty
        {
            get
            {
                return _rejectedQty;
            }
            set
            {
                _rejectedQty = value;
            }
        }
        private Decimal? _scrapQty;
        public Decimal? ScrapQty
        {
            get
            {
                return _scrapQty;
            }
            set
            {
                _scrapQty = value;
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
        private IList<OrderLocationTransaction> _orderLocationTransactions;
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public IList<OrderLocationTransaction> OrderLocationTransactions
        {
            get
            {
                return _orderLocationTransactions;
            }
            set
            {
                _orderLocationTransactions = value;
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
        private Decimal? _unitPrice;
        public Decimal? UnitPrice
        {
            get
            {
                return _unitPrice;
            }
            set
            {
                _unitPrice = value;
            }
        }
        private Decimal? _unitPriceAfterDiscount;
        public Decimal? UnitPriceAfterDiscount
        {
            get
            {
                return _unitPriceAfterDiscount;
            }
            set
            {
                _unitPriceAfterDiscount = value;
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
        private Decimal? _headDiscount;
        public Decimal? HeadDiscount
        {
            get
            {
                return _headDiscount;
            }
            set
            {
                _headDiscount = value;
            }
        }
        private Boolean _isProvisionalEstimate;
        public Boolean IsProvisionalEstimate
        {
            get
            {
                return _isProvisionalEstimate;
            }
            set
            {
                _isProvisionalEstimate = value;
            }
        }
        private Boolean _isIncludeTax;
        public Boolean IsIncludeTax
        {
            get
            {
                return _isIncludeTax;
            }
            set
            {
                _isIncludeTax = value;
            }
        }
        private string _taxCode;
        public string TaxCode
        {
            get
            {
                return _taxCode;
            }
            set
            {
                _taxCode = value;
            }
        }
        public com.Sconit.Entity.MasterData.Customer Customer { get; set; }
        public Boolean NeedInspection { get; set; }
        public String IdMark { get; set; }
        public String BarCodeType { get; set; }
        public String ItemVersion { get; set; }
        public String OddShipOption { get; set; }
        public String Remark { get; set; }
        public String InspectLocationFrom { get; set; }
        public String InspectLocationTo { get; set; }
        public String RejectLocationFrom { get; set; }
        public String RejectLocationTo { get; set; }
        public String StorageBin { get; set; }
        public Boolean NeedRejectInspection { get; set; }
        public Routing Routing { get; set; }
        public Routing ReturnRouting { get; set; }
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
            OrderDetailBase another = obj as OrderDetailBase;

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

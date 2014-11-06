using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class ReceiptDetailBase : EntityBase
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
		private com.Sconit.Entity.MasterData.Receipt _receipt;
		public com.Sconit.Entity.MasterData.Receipt Receipt
		{
			get
			{
				return _receipt;
			}
			set
			{
				_receipt = value;
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
        //private com.Sconit.Entity.MasterData.Uom _uom;
        //public com.Sconit.Entity.MasterData.Uom Uom
        //{
        //    get
        //    {
        //        return _uom;
        //    }
        //    set
        //    {
        //        _uom = value;
        //    }
        //}
        private String _huId;
        public String HuId
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
        private String _lotNo;
        public String LotNo
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
		private Decimal _receivedQty;
		public Decimal ReceivedQty
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
        private Decimal _shippedQty;
        public Decimal ShippedQty
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
		private Decimal _rejectedQty;
		public Decimal RejectedQty
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
		private Decimal _scrapQty;
		public Decimal ScrapQty
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
        private Boolean _isConsignment;
        public Boolean IsConsignment
        {
            get
            {
                return _isConsignment;
            }
            set
            {
                _isConsignment = value;
            }
        }
        private Int32? _plannedBill;
        public Int32? PlannedBill
        {
            get
            {
                return _plannedBill;
            }
            set
            {
                _plannedBill = value;
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
        private Int32 _inProcessLocationDetail;
        public Int32 InProcessLocationDetail
        {
            get
            {
                return _inProcessLocationDetail;
            }
            set
            {
                _inProcessLocationDetail = value;
            }
        }
        #endregion

        #region O/R Mapping Retention Properties

        private string _textField1;
        public string TextField1   //疵点数
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
        public string TextField2   //让码数
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
        public string TextField3   //批号
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
            ReceiptDetailBase another = obj as ReceiptDetailBase;

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

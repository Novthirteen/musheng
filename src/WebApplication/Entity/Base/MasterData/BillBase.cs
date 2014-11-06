using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class BillBase : EntityBase
    {
        #region O/R Mapping Properties
		
		private string _billNo;
		public string BillNo
		{
			get
			{
				return _billNo;
			}
			set
			{
				_billNo = value;
			}
		}
		private string _externalBillNo;
		public string ExternalBillNo
		{
			get
			{
				return _externalBillNo;
			}
			set
			{
				_externalBillNo = value;
			}
		}
        private string _refBillNo;
        public string ReferenceBillNo
        {
            get
            {
                return _refBillNo;
            }
            set
            {
                _refBillNo = value;
            }
        }
        private string _billType;
        public string BillType
        {
            get
            {
                return _billType;
            }
            set
            {
                _billType = value;
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
		private string _transactionType;
		public string TransactionType
		{
			get
			{
				return _transactionType;
			}
			set
			{
				_transactionType = value;
			}
		}
		private com.Sconit.Entity.MasterData.BillAddress _billAddr;
		public com.Sconit.Entity.MasterData.BillAddress BillAddress
		{
			get
			{
				return _billAddr;
			}
			set
			{
				_billAddr = value;
			}
		}
		private com.Sconit.Entity.MasterData.Currency _currency;
		public com.Sconit.Entity.MasterData.Currency Currency
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
        //private Decimal _totalAmount;
        //public Decimal TotalAmount
        //{
        //    get
        //    {
        //        return _totalAmount;
        //    }
        //    set
        //    {
        //        _totalAmount = value;
        //    }
        //}
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
        private Decimal? _paymentAmount;
        public Decimal? PaymentAmount
        {
            get
            {
                return _paymentAmount;
            }
            set
            {
                _paymentAmount = value;
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
        private IList<BillDetail> _billDetails;
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public IList<BillDetail> BillDetails
        {
            get
            {
                return _billDetails;
            }
            set
            {
                _billDetails = value;
            }
        }

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

        private DateTime? _invoiceDate;
        public DateTime? InvoiceDate
        {
            get
            {
                return _invoiceDate == null ? this.CreateDate.AddDays(Convert.ToDouble(this.BillAddress.Party.Aging)) : _invoiceDate;
            }
            set
            {
                _invoiceDate = value;
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
			if (BillNo != null)
            {
                return BillNo.GetHashCode();
            }
            else
            {
                return base.GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            BillBase another = obj as BillBase;

            if (another == null)
            {
                return false;
            }
            else
            {
            	return (this.BillNo == another.BillNo);
            }
        } 
    }
	
}

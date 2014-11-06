using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class BillDetailBase : EntityBase
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
		private com.Sconit.Entity.MasterData.Bill _bill;
		public com.Sconit.Entity.MasterData.Bill Bill
		{
			get
			{
				return _bill;
			}
			set
			{
				_bill = value;
			}
		}
        private com.Sconit.Entity.MasterData.ActingBill _actingBill;
        public com.Sconit.Entity.MasterData.ActingBill ActingBill
		{
			get
			{
                return _actingBill;
			}
			set
			{
                _actingBill = value;
			}
		}
		private Decimal _billedQty;
		public Decimal BilledQty
		{
			get
			{
				return _billedQty;
			}
			set
			{
				_billedQty = value;
			}
		}
		private Decimal _unitPrice;
		public Decimal UnitPrice
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
        private Decimal _listPrice;
        public Decimal ListPrice
        {
            get
            {
                return _listPrice;
            }
            set
            {
                _listPrice = value;
            }
        }
        private Decimal _headDiscount;
        public Decimal HeadDiscount
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
        private Decimal _orderAmount;
        public Decimal Amount
		{
			get
			{
                return _orderAmount;
			}
			set
			{
                _orderAmount = value;
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

        public String LocationFrom { get; set; }
        public String IpNo { get; set; }
        public String ReferenceItemCode { get; set; }
        public String FlowCode { get; set; }
        public String CostCenter { get; set; }
        public String CostGroup { get; set; }
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
            BillDetailBase another = obj as BillDetailBase;

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

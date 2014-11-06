using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.Cost
{
    [Serializable]
    public abstract class CostTransactionBase : EntityBase
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
		private String _item;
        public String Item
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
        private String _itemCategory;
        public String ItemCategory
        {
            get
            {
                return _itemCategory;
            }
            set
            {
                _itemCategory = value;
            }
        }
		private string _orderNo;
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
        private string _receiptNo;
        public string ReceiptNo
        {
            get
            {
                return _receiptNo;
            }
            set
            {
                _receiptNo = value;
            }
        }
        private com.Sconit.Entity.Cost.CostGroup _costGroup;
        public com.Sconit.Entity.Cost.CostGroup CostGroup
        {
            get
            {
                return _costGroup;
            }
            set
            {
                _costGroup = value;
            }
        }
		private com.Sconit.Entity.Cost.CostCenter _costCenter;
		public com.Sconit.Entity.Cost.CostCenter CostCenter
		{
			get
			{
				return _costCenter;
			}
			set
			{
				_costCenter = value;
			}
		}
		private com.Sconit.Entity.Cost.CostElement _costElement;
		public com.Sconit.Entity.Cost.CostElement CostElement
		{
			get
			{
				return _costElement;
			}
			set
			{
				_costElement = value;
			}
		}
		private string _currency;
		public string Currency
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
		private string _baseCurrency;
		public string BaseCurrency
		{
			get
			{
				return _baseCurrency;
			}
			set
			{
				_baseCurrency = value;
			}
		}
		private Decimal _exchangeRate;
		public Decimal ExchangeRate
		{
			get
			{
				return _exchangeRate;
			}
			set
			{
				_exchangeRate = value;
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
        private Decimal _standardAmount;
		public Decimal StandardAmount
		{
			get
			{
                return _standardAmount;
			}
			set
			{
                _standardAmount = value;
			}
		}
        private Decimal _actualAmount;
		public Decimal ActualAmount
		{
			get
			{
                return _actualAmount;
			}
			set
			{
                _actualAmount = value;
			}
		}
        private string _referenceItem;
		public string ReferenceItem
		{
			get
			{
                return _referenceItem;
			}
			set
			{
                _referenceItem = value;
			}
		}
        private Decimal? _referenceQty;
		public Decimal? ReferenceQty
		{
			get
			{
                return _referenceQty;
			}
			set
			{
                _referenceQty = value;
			}
		}
       
        private DateTime _effectiveDate;
		public DateTime EffectiveDate
		{
			get
			{
                return _effectiveDate;
			}
			set
			{
                _effectiveDate = value;
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
        private string _createUser;
        public string CreateUser
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

        public int CostAllocateTransaction { get; set; }
        public String ReferenceItemType { get; set; }
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
            CostTransactionBase another = obj as CostTransactionBase;

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

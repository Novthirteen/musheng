using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class PartyBase : EntityBase
    {
        #region O/R Mapping Properties
		
		private string _code;
        public virtual string Code
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
		private string _name;
        public virtual string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}
        private string _paymentTerm;
        public virtual string PaymentTerm
        {
            get
            {
                return _paymentTerm;
            }
            set
            {
                _paymentTerm = value;
            }
        }
        private string _tradeTerm;
        public virtual string TradeTerm
        {
            get
            {
                return _tradeTerm;
            }
            set
            {
                _tradeTerm = value;
            }
        }
        private string _country;
        public virtual string Country
        {
            get
            {
                return _country;
            }
            set
            {
                _country = value;
            }
        }
       
		private Party _parentParty;
        public virtual Party ParentParty
		{
			get
			{
				return _parentParty;
			}
			set
			{
				_parentParty = value;
			}
		}
		private Boolean _isActive;
        public virtual Boolean IsActive
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

        public Int32 Aging { get; set; }
        #endregion

        #region O/R Mapping Retention Properties

        private string _textField1;
        public virtual string TextField1
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
        public virtual string TextField2
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
        public virtual string TextField3
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
        public virtual string TextField4
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
        public virtual Decimal? NumField1
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
        public virtual Decimal? NumField2
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
        public virtual Decimal? NumField3
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
        public virtual Decimal? NumField4
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
        public virtual DateTime? DateField1
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
        public virtual DateTime? DateField2
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
            PartyBase another = obj as PartyBase;

            if (another == null)
            {
                return false;
            }
            else
            {
            	return (this.Code == another.Code);
            }
        } 
    }
	
}

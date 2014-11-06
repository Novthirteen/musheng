using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class LocationBase : EntityBase
    {
        #region O/R Mapping Properties
		private Boolean _allowNegativeInventory;
		public Boolean AllowNegativeInventory
		{
			get
			{
				return _allowNegativeInventory;
			}
			set
			{
				_allowNegativeInventory = value;
			}
		}
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
		private string _name;
		public string Name
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
		private com.Sconit.Entity.MasterData.Region _region;
		public com.Sconit.Entity.MasterData.Region Region
		{
			get
			{
				return _region;
			}
			set
			{
				_region = value;
			}
		}
		
		private Decimal? _volume;
        public Decimal? Volume
		{
			get
			{
				return _volume;
			}
			set
			{
				_volume = value;
			}
		}

        private Boolean _enableAdvWM;
        public Boolean EnableAdvWM
        {
            get
            {
                return _enableAdvWM;
            }
            set
            {
                _enableAdvWM = value;
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

        private Boolean _isSettleConsignment;
        public Boolean IsSettleConsignment
        {
            get
            {
                return _isSettleConsignment;
            }
            set
            {
                _isSettleConsignment = value;
            }
        }
        private Boolean _isMRP;
        public Boolean IsMRP
        {
            get
            {
                return _isMRP;
            }
            set
            {
                _isMRP = value;
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
            LocationBase another = obj as LocationBase;

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

using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class InspectOrderBase : EntityBase
    {
        #region O/R Mapping Properties
		
		private string _inspectNo;
		public string InspectNo
		{
			get
			{
				return _inspectNo;
			}
			set
			{
				_inspectNo = value;
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
        private Boolean _isDetailHasHu;
        public Boolean IsDetailHasHu
        {
            get
            {
                return _isDetailHasHu;
            }
            set
            {
                _isDetailHasHu = value;
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
        public Boolean IsPrinted { get; set; }

        public string IpNo { get; set; }

        public string ReceiptNo { get; set; }

        public Boolean IsSeperated { get; set; }

        public string InspectLocation { get; set; }

        public string RejectLocation { get; set; }

        private IList<InspectOrderDetail> _inspectOrderDetails;
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public IList<InspectOrderDetail> InspectOrderDetails
        {
            get
            {
                return _inspectOrderDetails;
            }
            set
            {
                _inspectOrderDetails = value;
            }
        }

        private DateTime _estimateInspectDate;
        public DateTime EstimateInspectDate
        {
            get
            {
                return _estimateInspectDate;
            }
            set
            {
                _estimateInspectDate = value;
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
			if (InspectNo != null)
            {
                return InspectNo.GetHashCode();
            }
            else
            {
                return base.GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            InspectOrderBase another = obj as InspectOrderBase;

            if (another == null)
            {
                return false;
            }
            else
            {
            	return (this.InspectNo == another.InspectNo);
            }
        } 
    }
	
}

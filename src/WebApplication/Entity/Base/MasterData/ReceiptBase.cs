using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class ReceiptBase : EntityBase
    {
        #region O/R Mapping Properties
		
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
        private string _orderType;
        public string OrderType
        {
            get
            {
                return _orderType;
            }
            set
            {
                _orderType = value;
            }
        }
        private com.Sconit.Entity.MasterData.Flow _flow;
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
        private string _externalReceiptNo;
        public string ExternalReceiptNo
        {
            get
            {
                return _externalReceiptNo;
            }
            set
            {
                _externalReceiptNo = value;
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
        private User _createUser;
		public User CreateUser
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
        private IList<com.Sconit.Entity.MasterData.ReceiptDetail> _receiptDetails;
        public IList<com.Sconit.Entity.MasterData.ReceiptDetail> ReceiptDetails
        {
            get
            {
                return _receiptDetails;
            }
            set
            {
                _receiptDetails = value;
            }
        }
        private IList<com.Sconit.Entity.Distribution.InProcessLocation> _inProcessLocations;
        public IList<com.Sconit.Entity.Distribution.InProcessLocation> InProcessLocations
        {
            get
            {
                return _inProcessLocations;
            }
            set
            {
                _inProcessLocations = value;
            }
        }
        public String ReceiptTemplate { get; set; }
        public String ReferenceIpNo { get; set; }
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
			if (ReceiptNo != null)
            {
                return ReceiptNo.GetHashCode();
            }
            else
            {
                return base.GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            ReceiptBase another = obj as ReceiptBase;

            if (another == null)
            {
                return false;
            }
            else
            {
            	return (this.ReceiptNo == another.ReceiptNo);
            }
        } 
    }
	
}

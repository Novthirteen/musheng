using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class InspectOrderDetailBase : EntityBase
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
		private com.Sconit.Entity.MasterData.InspectOrder _inspectOrder;
		public com.Sconit.Entity.MasterData.InspectOrder InspectOrder
		{
			get
			{
				return _inspectOrder;
			}
			set
			{
				_inspectOrder = value;
			}
		}
		private com.Sconit.Entity.MasterData.LocationLotDetail _locationLotDetail;
		public com.Sconit.Entity.MasterData.LocationLotDetail LocationLotDetail
		{
			get
			{
				return _locationLotDetail;
			}
			set
			{
				_locationLotDetail = value;
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
		private Decimal _inspectQty;
		public Decimal InspectQty
		{
			get
			{
				return _inspectQty;
			}
			set
			{
				_inspectQty = value;
			}
		}
		private Decimal? _qualifiedQty;
		public Decimal? QualifiedQty
		{
			get
			{
				return _qualifiedQty;
			}
			set
			{
				_qualifiedQty = value;
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

        private string _disposition;
        public string Disposition
        {
            get
            {
                return _disposition;
            }
            set
            {
                _disposition = value;
            }
        }
        private Item _finishGoods;
        public Item FinishGoods
        {
            get
            {
                return _finishGoods;
            }
            set
            {
                _finishGoods = value;
            }
        }
        #endregion

        #region O/R Mapping Retention Properties

        private string _lotNo;
        public string LotNo
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
            InspectOrderDetailBase another = obj as InspectOrderDetailBase;

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

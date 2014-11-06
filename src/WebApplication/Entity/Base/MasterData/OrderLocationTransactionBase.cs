using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class OrderLocationTransactionBase : EntityBase
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
		private com.Sconit.Entity.MasterData.OrderDetail _orderDetail;
		public com.Sconit.Entity.MasterData.OrderDetail OrderDetail
		{
			get
			{
				return _orderDetail;
			}
			set
			{
				_orderDetail = value;
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
        private com.Sconit.Entity.MasterData.ItemDiscontinue _itemDiscontinue;
        public com.Sconit.Entity.MasterData.ItemDiscontinue ItemDiscontinue
        {
            get
            {
                return _itemDiscontinue;
            }
            set
            {
                _itemDiscontinue = value;
            }
        }

        private com.Sconit.Entity.MasterData.Item _rawItem;
        public com.Sconit.Entity.MasterData.Item RawItem
        {
            get
            {
                return _rawItem;
            }
            set
            {
                _rawItem = value;
            }
        }
		private string _itemDescription;
		public string ItemDescription
		{
			get
			{
				return _itemDescription;
			}
			set
			{
				_itemDescription = value;
			}
		}
        private BomDetail _bomDetail;
        public BomDetail BomDetail
        {
            get
            {
                return _bomDetail;
            }
            set
            {
                _bomDetail = value;
            }
        }
        private Boolean _isAssemble;
        public Boolean IsAssemble
        {
            get
            {
                return _isAssemble;
            }
            set
            {
                _isAssemble = value;
            }
        }
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
		private Int32 _operation;
        public Int32 Operation
		{
			get
			{
				return _operation;
			}
			set
			{
				_operation = value;
			}
		}
		private string _iOType;
		public string IOType
		{
			get
			{
				return _iOType;
			}
			set
			{
				_iOType = value;
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
		private Decimal _unitQty;
		public Decimal UnitQty
		{
			get
			{
				return _unitQty;
			}
			set
			{
				_unitQty = value;
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
		private Decimal? _accumulateQty;
		public Decimal? AccumulateQty
		{
			get
			{
				return _accumulateQty;
			}
			set
			{
				_accumulateQty = value;
			}
		}
		private Decimal? _accumulateRejectQty;
		public Decimal? AccumulateRejectQty
		{
			get
			{
				return _accumulateRejectQty;
			}
			set
			{
				_accumulateRejectQty = value;
			}
		}
        private Decimal? _accumulateScrapQty;
        public Decimal? AccumulateScrapQty
        {
            get
            {
                return _accumulateScrapQty;
            }
            set
            {
                _accumulateScrapQty = value;
            }
        }
		private com.Sconit.Entity.MasterData.Location _location;
		public com.Sconit.Entity.MasterData.Location Location
		{
			get
			{
				return _location;
			}
			set
			{
				_location = value;
			}
		}
		private String _rejectLocation;
        public String RejectLocation
		{
			get
			{
				return _rejectLocation;
			}
			set
			{
				_rejectLocation = value;
			}
		}
        private String _inpectLocation;
        public String InspectLocation
        {
            get
            {
                return _inpectLocation;
            }
            set
            {
                _inpectLocation = value;
            }
        }
        private Boolean _IsShipScanHu;
        public Boolean IsShipScanHu
        {
            get
            {
                return _IsShipScanHu;
            }
            set
            {
                _IsShipScanHu = value;
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
		private Boolean _needPrint;
		public Boolean NeedPrint
		{
			get
			{
				return _needPrint;
			}
			set
			{
				_needPrint = value;
			}
		}
        private String _backFlushMethod;
        public String BackFlushMethod
        {
            get
            {
                return _backFlushMethod;
            }
            set
            {
                _backFlushMethod = value;
            }
        }
        private String _itemVersion;
        public String ItemVersion
        {
            get
            {
                return _itemVersion;
            }
            set
            {
                _itemVersion = value;
            }
        }
        //private Decimal? _plannedBackFlushQty;
        //public Decimal? PlannedBackFlushQty
        //{
        //    get
        //    {
        //        return _plannedBackFlushQty;
        //    }
        //    set
        //    {
        //        _plannedBackFlushQty = value;
        //    }
        //}
        //分光1开始
        private string _sortLevel1From;
        public string SortLevel1From
        {
            get
            {
                return _sortLevel1From;
            }
            set
            {
                _sortLevel1From = value;
            }
        }
        //分光1结束
        private string _sortLevel1To;
        public string SortLevel1To
        {
            get
            {
                return _sortLevel1To;
            }
            set
            {
                _sortLevel1To = value;
            }
        }
        //分色1开始
        private string _colorLevel1From;
        public string ColorLevel1From
        {
            get
            {
                return _colorLevel1From;
            }
            set
            {
                _colorLevel1From = value;
            }
        }
        //分色1结束
        private string _colorLevel1To;
        public string ColorLevel1To
        {
            get
            {
                return _colorLevel1To;
            }
            set
            {
                _colorLevel1To = value;
            }
        }
        //分光2开始
        private string _sortLevel2From;
        public string SortLevel2From
        {
            get
            {
                return _sortLevel2From;
            }
            set
            {
                _sortLevel2From = value;
            }
        }
        //分光2结束
        private string _sortLevel2To;
        public string SortLevel2To
        {
            get
            {
                return _sortLevel2To;
            }
            set
            {
                _sortLevel2To = value;
            }
        }
        //分色2开始
        private string _colorLevel2From;
        public string ColorLevel2From
        {
            get
            {
                return _colorLevel2From;
            }
            set
            {
                _colorLevel2From = value;
            }
        }
        //分色2结束
        private string _colorLevel2To;
        public string ColorLevel2To
        {
            get
            {
                return _colorLevel2To;
            }
            set
            {
                _colorLevel2To = value;
            }
        }
        //原材料数量，计算工时用
        public int? MaterailNumber { get; set; }
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
            OrderLocationTransactionBase another = obj as OrderLocationTransactionBase;

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

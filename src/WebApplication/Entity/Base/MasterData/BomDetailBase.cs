using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class BomDetailBase : EntityBase
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
		private com.Sconit.Entity.MasterData.Bom _bom;
		public com.Sconit.Entity.MasterData.Bom Bom
		{
			get
			{
				return _bom;
			}
			set
			{
				_bom = value;
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
		private string _reference;
		public string Reference
		{
			get
			{
				return _reference;
			}
			set
			{
				_reference = value;
			}
		}
		private string _structureType;
		public string StructureType
		{
			get
			{
				return _structureType;
			}
			set
			{
				_structureType = value;
			}
		}
		private DateTime _startDate;
		public DateTime StartDate
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
		private Decimal _rateQty;
		public Decimal RateQty
		{
			get
			{
				return _rateQty;
			}
			set
			{
				_rateQty = value;
			}
		}
		private Decimal? _scrapPercentage;
		public Decimal? ScrapPercentage
		{
			get
			{
				return _scrapPercentage;
			}
			set
			{
				_scrapPercentage = value;
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
        private Int32 _priority;
        public Int32 Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
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
        private string _backFlushMethod;
        public string BackFlushMethod
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
        #endregion

        ////分光1开始
        //private string _sortLevel1From;
        //public string SortLevel1From
        //{
        //    get
        //    {
        //        return _sortLevel1From;
        //    }
        //    set
        //    {
        //        _sortLevel1From = value;
        //    }
        //}
        ////分光1结束
        //private string _sortLevel1To;
        //public string SortLevel1To
        //{
        //    get
        //    {
        //        return _sortLevel1To;
        //    }
        //    set
        //    {
        //        _sortLevel1To = value;
        //    }
        //}
        ////分色1开始
        //private string _colorLevel1From;
        //public string ColorLevel1From
        //{
        //    get
        //    {
        //        return _colorLevel1From;
        //    }
        //    set
        //    {
        //        _colorLevel1From = value;
        //    }
        //}
        ////分色1结束
        //private string _colorLevel1To;
        //public string ColorLevel1To
        //{
        //    get
        //    {
        //        return _colorLevel1To;
        //    }
        //    set
        //    {
        //        _colorLevel1To = value;
        //    }
        //}
        ////分光2开始
        //private string _sortLevel2From;
        //public string SortLevel2From
        //{
        //    get
        //    {
        //        return _sortLevel2From;
        //    }
        //    set
        //    {
        //        _sortLevel2From = value;
        //    }
        //}
        ////分光2结束
        //private string _sortLevel2To;
        //public string SortLevel2To
        //{
        //    get
        //    {
        //        return _sortLevel2To;
        //    }
        //    set
        //    {
        //        _sortLevel2To = value;
        //    }
        //}
        ////分色2开始
        //private string _colorLevel2From;
        //public string ColorLevel2From
        //{
        //    get
        //    {
        //        return _colorLevel2From;
        //    }
        //    set
        //    {
        //        _colorLevel2From = value;
        //    }
        //}
        ////分色2结束
        //private string _colorLevel2To;
        //public string ColorLevel2To
        //{
        //    get
        //    {
        //        return _colorLevel2To;
        //    }
        //    set
        //    {
        //        _colorLevel2To = value;
        //    }
        //}
        //位号
        private string _positionNo;
        public string PositionNo
        {
            get
            {
                return _positionNo;
            }
            set
            {
                _positionNo = value;
            }
        }

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
            BomDetailBase another = obj as BomDetailBase;

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

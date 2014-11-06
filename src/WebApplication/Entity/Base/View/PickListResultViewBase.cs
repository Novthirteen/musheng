using System;
using System.Collections;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here

namespace com.Sconit.Entity.View
{
    [Serializable]
    public abstract class PickListResultViewBase : EntityBase
    {
        #region O/R Mapping Properties
		
		private Int32? _id;
		public Int32? Id
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
		private PickList _pickList;
        public PickList PickList
		{
			get
			{
                return _pickList;
			}
			set
			{
                _pickList = value;
			}
		}
		private string _uom;
		public string Uom
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
		private Decimal _unitCount;
        public Decimal UnitCount
		{
			get
			{
                return _unitCount;
			}
			set
			{
                _unitCount = value;
			}
		}
		private string _location;
		public string Location
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
		private Decimal? _actqty;
		public Decimal? actqty
		{
			get
			{
				return _actqty;
			}
			set
			{
				_actqty = value;
			}
		}
		private Decimal? _planqty;
		public Decimal? planqty
		{
			get
			{
				return _planqty;
			}
			set
			{
				_planqty = value;
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
            PickListResultViewBase another = obj as PickListResultViewBase;

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

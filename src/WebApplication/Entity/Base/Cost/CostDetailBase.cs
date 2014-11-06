using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.Cost
{
    [Serializable]
    public abstract class CostDetailBase : EntityBase
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
		private Decimal _cost;
		public Decimal Cost
		{
			get
			{
                return _cost;
			}
			set
			{
                _cost = value;
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
        public int FinanceYear { get; set; }
        public int FinanceMonth { get; set; }
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
            CostDetailBase another = obj as CostDetailBase;

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

using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.Cost
{
    [Serializable]
    public abstract class CostAllocateTransactionBase : EntityBase
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
		private com.Sconit.Entity.Cost.ExpenseElement _expenseElement;
		public com.Sconit.Entity.Cost.ExpenseElement ExpenseElement
		{
			get
			{
				return _expenseElement;
			}
			set
			{
				_expenseElement = value;
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
		private com.Sconit.Entity.Cost.CostElement _dependCostElement;
		public com.Sconit.Entity.Cost.CostElement DependCostElement
		{
			get
			{
				return _dependCostElement;
			}
			set
			{
				_dependCostElement = value;
			}
		}
		private string _allocateBy;
		public string AllocateBy
		{
			get
			{
				return _allocateBy;
			}
			set
			{
				_allocateBy = value;
			}
		}
		private Decimal _amount;
		public Decimal Amount
		{
			get
			{
				return _amount;
			}
			set
			{
				_amount = value;
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
        private string _items;
        public string Items
		{
			get
			{
                return _items;
			}
			set
			{
                _items = value;
			}
		}
        private string _orders;
        public string Orders
        {
            get
            {
                return _orders;
            }
            set
            {
                _orders = value;
            }
        }
        private string _itemCategorys;
        public string ItemCategorys
        {
            get
            {
                return _itemCategorys;
            }
            set
            {
                _itemCategorys = value;
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
        private string _referenceItems;
        public string ReferenceItems
        {
            get
            {
                return _referenceItems;
            }
            set
            {
                _referenceItems = value;
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
            CostAllocateTransactionBase another = obj as CostAllocateTransactionBase;

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

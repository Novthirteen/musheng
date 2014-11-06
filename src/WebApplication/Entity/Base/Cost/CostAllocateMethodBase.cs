using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.Cost
{
    [Serializable]
    public abstract class CostAllocateMethodBase : EntityBase
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
            CostAllocateMethodBase another = obj as CostAllocateMethodBase;

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

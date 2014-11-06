using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.Cost
{
    [Serializable]
    public abstract class CostCenterBase : EntityBase
    {
        #region O/R Mapping Properties
		
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
		private string _description;
		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				_description = value;
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
            CostCenterBase another = obj as CostCenterBase;

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

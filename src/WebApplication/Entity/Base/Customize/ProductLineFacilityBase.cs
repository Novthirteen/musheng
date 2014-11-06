using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.Customize
{
    [Serializable]
    public abstract class ProductLineFacilityBase : EntityBase
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
		private string _productLine;
		public string ProductLine
		{
			get
			{
				return _productLine;
			}
			set
			{
				_productLine = value;
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

        private com.Sconit.Entity.MasterData.Routing _routing;
        public com.Sconit.Entity.MasterData.Routing Routing
        {
            get
            {
                return _routing;
            }
            set
            {
                _routing = value;
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
            ProductLineFacilityBase another = obj as ProductLineFacilityBase;

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

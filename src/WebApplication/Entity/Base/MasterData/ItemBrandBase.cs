using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class ItemBrandBase : EntityBase
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
        private com.Sconit.Entity.MasterData.ItemBrand _parentBrand;
        public com.Sconit.Entity.MasterData.ItemBrand ParentBrand
		{
			get
			{
                return _parentBrand;
			}
			set
			{
                _parentBrand = value;
			}
		}
        public string Abbreviation { get; set; }
        public string ManufactureParty { get; set; }
        public string Origin { get; set; }
        public string ManufactureAddress { get; set; }
        public Boolean IsActive { get; set; }
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
            ItemBrandBase another = obj as ItemBrandBase;

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

using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.Customize
{
    [Serializable]
    public abstract class ProdutLineFeedSeqenceBase : EntityBase
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
        private String _code;
        public String Code
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
        //private com.Sconit.Entity.MasterData.Flow _productLine;
        //public com.Sconit.Entity.MasterData.Flow ProductLine
        //{
        //    get
        //    {
        //        return _productLine;
        //    }
        //    set
        //    {
        //        _productLine = value;
        //    }
        //}
        public string ProductLineFacility { get; set; }
		private com.Sconit.Entity.MasterData.Item _finishGood;
		public com.Sconit.Entity.MasterData.Item FinishGood
		{
			get
			{
				return _finishGood;
			}
			set
			{
				_finishGood = value;
			}
		}
		private com.Sconit.Entity.MasterData.Item _rawMaterial;
		public com.Sconit.Entity.MasterData.Item RawMaterial
		{
			get
			{
				return _rawMaterial;
			}
			set
			{
				_rawMaterial = value;
			}
		}
		private Int32 _sequence;
		public Int32 Sequence
		{
			get
			{
                return _sequence;
			}
			set
			{
                _sequence = value;
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
		private string _lastModifyUser;
		public string LastModifyUser
		{
			get
			{
				return _lastModifyUser;
			}
			set
			{
				_lastModifyUser = value;
			}
		}
		private DateTime _lastModifyDate;
		public DateTime LastModifyDate
		{
			get
			{
				return _lastModifyDate;
			}
			set
			{
				_lastModifyDate = value;
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
            ProdutLineFeedSeqenceBase another = obj as ProdutLineFeedSeqenceBase;

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

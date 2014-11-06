using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class MenuBase : EntityBase
    {
        #region O/R Mapping Properties
		
		private string _id;
		public string Id
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
		private Int32 _version;
		public Int32 Version
		{
			get
			{
				return _version;
			}
			set
			{
				_version = value;
			}
		}
		private string _title;
		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				_title = value;
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
		private string _desc;
		public string Desc
		{
			get
			{
				return _desc;
			}
			set
			{
				_desc = value;
			}
		}
		private string _pageUrl;
		public string PageUrl
		{
			get
			{
				return _pageUrl;
			}
			set
			{
				_pageUrl = value;
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
		private string _imageUrl;
		public string ImageUrl
		{
			get
			{
				return _imageUrl;
			}
			set
			{
				_imageUrl = value;
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
		private com.Sconit.Entity.MasterData.User _createUser;
		public com.Sconit.Entity.MasterData.User CreateUser
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
		private com.Sconit.Entity.MasterData.User _lastModifyUser;
		public com.Sconit.Entity.MasterData.User LastModifyUser
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
		private string _remark;
		public string Remark
		{
			get
			{
				return _remark;
			}
			set
			{
				_remark = value;
			}
		}
        
        #endregion

		public override int GetHashCode()
        {
			if (Id != null)
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
            MenuBase another = obj as MenuBase;

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

using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.View
{
    [Serializable]
    public abstract class MenuViewBase : EntityBase
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
		private string _desc;
        public string Description
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
		private Int32 _menuRelationId;
		public Int32 MenuRelationId
		{
			get
			{
				return _menuRelationId;
			}
			set
			{
				_menuRelationId = value;
			}
		}
		private string _type;
		public string Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}
		private string _industryOrCompanyCode;
		public string IndustryOrCompanyCode
		{
			get
			{
				return _industryOrCompanyCode;
			}
			set
			{
				_industryOrCompanyCode = value;
			}
		}
		private string _parentCode;
		public string ParentCode
		{
			get
			{
				return _parentCode;
			}
			set
			{
				_parentCode = value;
			}
		}
		private Int32? _parenVersion;
		public Int32? ParenVersion
		{
			get
			{
				return _parenVersion;
			}
			set
			{
				_parenVersion = value;
			}
		}
		private Int32 _level;
		public Int32 Level
		{
			get
			{
				return _level;
			}
			set
			{
				_level = value;
			}
		}
		private Int32 _seq;
		public Int32 Seq
		{
			get
			{
				return _seq;
			}
			set
			{
				_seq = value;
			}
		}
		private Boolean _menuRelationIsActive;
		public Boolean MenuRelationIsActive
		{
			get
			{
				return _menuRelationIsActive;
			}
			set
			{
				_menuRelationIsActive = value;
			}
		}
		private DateTime? _createDate;
		public DateTime? CreateDate
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
		private DateTime? _lastModifyDate;
		public DateTime? LastModifyDate
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
            MenuViewBase another = obj as MenuViewBase;

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

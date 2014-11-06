using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class MenuCompanyBase : EntityBase
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
		private com.Sconit.Entity.MasterData.Company _companyCode;
		public com.Sconit.Entity.MasterData.Company CompanyCode
		{
			get
			{
				return _companyCode;
			}
			set
			{
				_companyCode = value;
			}
		}
		private com.Sconit.Entity.MasterData.Menu _menuId;
		public com.Sconit.Entity.MasterData.Menu MenuId
		{
			get
			{
				return _menuId;
			}
			set
			{
				_menuId = value;
			}
		}
		private com.Sconit.Entity.MasterData.Menu _parentMenuId;
		public com.Sconit.Entity.MasterData.Menu ParentMenuId
		{
			get
			{
				return _parentMenuId;
			}
			set
			{
				_parentMenuId = value;
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
            MenuCompanyBase another = obj as MenuCompanyBase;

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

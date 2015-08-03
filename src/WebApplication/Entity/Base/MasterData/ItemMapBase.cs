using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class ItemMapBase : EntityBase
    {
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

        private String _mapItem;
        public String MapItem
        {
            get
            {
                return _mapItem;
            }
            set
            {
                _mapItem = value;
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
            }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;
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
    }
}

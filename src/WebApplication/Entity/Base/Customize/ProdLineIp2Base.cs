using System;
using System.Collections;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;

//TODO: Add other using statements here

namespace com.Sconit.Entity.Customize
{
    [Serializable]
    public abstract class ProdLineIp2Base : EntityBase
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
		private string _prodLine;
		public string ProdLine
		{
			get
			{
				return _prodLine;
			}
			set
			{
				_prodLine = value;
			}
		}
		private string _prodLineFact;
		public string ProdLineFact
		{
			get
			{
				return _prodLineFact;
			}
			set
			{
				_prodLineFact = value;
			}
		}
		private string _orderNo;
		public string OrderNo
		{
			get
			{
				return _orderNo;
			}
			set
			{
				_orderNo = value;
			}
		}
		private string _item;
		public string Item
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
        private Hu _hu;
        public Hu Hu
		{
			get
			{
                return _hu;
			}
			set
			{
                _hu = value;
			}
		}
		private string _lotNo;
		public string LotNo
		{
			get
			{
				return _lotNo;
			}
			set
			{
				_lotNo = value;
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
        public string Position { get; set; }
        public string Type { get; set; }
        public string FG { get; set; }
        public string ItemDescription { get; set; }
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
            ProdLineIp2Base another = obj as ProdLineIp2Base;

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

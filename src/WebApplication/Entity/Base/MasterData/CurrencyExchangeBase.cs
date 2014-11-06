using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class CurrencyExchangeBase : EntityBase
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
		private string _baseCurrency;
		public string BaseCurrency
		{
			get
			{
				return _baseCurrency;
			}
			set
			{
				_baseCurrency = value;
			}
		}
		private string _exchangeCurrency;
		public string ExchangeCurrency
		{
			get
			{
				return _exchangeCurrency;
			}
			set
			{
				_exchangeCurrency = value;
			}
		}
		private Decimal _baseQty;
		public Decimal BaseQty
		{
			get
			{
				return _baseQty;
			}
			set
			{
				_baseQty = value;
			}
		}
		private Decimal _exchangeQty;
		public Decimal ExchangeQty
		{
			get
			{
				return _exchangeQty;
			}
			set
			{
				_exchangeQty = value;
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
            CurrencyExchangeBase another = obj as CurrencyExchangeBase;

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

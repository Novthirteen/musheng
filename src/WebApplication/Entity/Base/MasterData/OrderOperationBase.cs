using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class OrderOperationBase : EntityBase
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
		private com.Sconit.Entity.MasterData.OrderHead _orderHead;
		public com.Sconit.Entity.MasterData.OrderHead OrderHead
		{
			get
			{
				return _orderHead;
			}
			set
			{
				_orderHead = value;
			}
		}
		private Int32 _operation;
		public Int32 Operation
		{
			get
			{
				return _operation;
			}
			set
			{
				_operation = value;
			}
		}
        private string _reference;
        public string Reference
        {
            get
            {
                return _reference;
            }
            set
            {
                _reference = value;
            }
        }
		private com.Sconit.Entity.MasterData.WorkCenter _workCenter;
		public com.Sconit.Entity.MasterData.WorkCenter WorkCenter
		{
			get
			{
				return _workCenter;
			}
			set
			{
				_workCenter = value;
			}
		}
        private Decimal? _setupTime;
        public Decimal? SetupTime
        {
            get
            {
                return _setupTime;
            }
            set
            {
                _setupTime = value;
            }
        }
        private Decimal? _runTime;
        public Decimal? RunTime
        {
            get
            {
                return _runTime;
            }
            set
            {
                _runTime = value;
            }
        }
        private Decimal? _moveTime;
        public Decimal? MoveTime
        {
            get
            {
                return _moveTime;
            }
            set
            {
                _moveTime = value;
            }
        }
        private Decimal? _machineQty;
        public Decimal? MachineQty
        {
            get
            {
                return _machineQty;
            }
            set
            {
                _machineQty = value;
            }
        }
        private Decimal? _yield;
        public Decimal? Yield
        {
            get
            {
                return _yield;
            }
            set
            {
                _yield = value;
            }
        }
        public com.Sconit.Entity.MasterData.Location Location { get; set; }
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
            OrderOperationBase another = obj as OrderOperationBase;

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

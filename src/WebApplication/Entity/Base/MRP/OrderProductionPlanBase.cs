using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Sconit.Entity.MRP
{
    [Serializable]
    public abstract class OrderProductionPlanBase : EntityBase
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

        private string _orderPlanNo;
        public string OrderPlanNo
        {
            get
            {
                return _orderPlanNo;
            }
            set
            {
                _orderPlanNo = value;
            }
        }

        private string _flow;
        public string Flow
        {
            get
            {
                return _flow;
            }
            set
            {
                _flow = value;
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

        private string _productionLineCode;
        public string ProductionLineCode
        {
            get
            {
                return _productionLineCode;
            }
            set
            {
                _productionLineCode = value;
            }
        }

        private int? _orderQty;
        public int? OrderQty
        {
            get
            {
                return _orderQty;
            }
            set
            {
                _orderQty = value;
            }
        }

        private DateTime? _planInTime;
        public DateTime? PlanInTime
        {
            get
            {
                return _planInTime;
            }
            set
            {
                _planInTime = value;
            }
        }

        private DateTime? _startTime;
        public DateTime? StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                _startTime = value;
            }
        }

        private DateTime? _planEndTime;
        public DateTime? PlanEndTime
        {
            get
            {
                return _planEndTime;
            }
            set
            {
                _planEndTime = value;
            }
        }

        private DateTime? _actualEndTime;
        public DateTime? ActualEndTime
        {
            get
            {
                return _actualEndTime;
            }
            set
            {
                _actualEndTime = value;
            }
        }

        private DateTime? _endTime;
        public DateTime? EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                _endTime = value;
            }
        }

        private Decimal? _planOrderHours;
        public Decimal? PlanOrderHours
        {
            get
            {
                return _planOrderHours;
            }
            set
            {
                _planOrderHours = value;
            }
        }

        /// <summary>
        /// 加工点数
        /// </summary>
        private Decimal? _orderNum;
        public Decimal? OrderNum
        {
            get
            {
                return _orderNum;
            }
            set
            {
                _orderNum = value;
            }
        }

        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
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

        private DateTime? _windowTime;
        public DateTime? WindowTime
        {
            get
            {
                return _windowTime;
            }
            set
            {
                _windowTime = value;
            }
        }

        public override int GetHashCode()
        {
            if (OrderPlanNo != null)
            {
                return OrderPlanNo.GetHashCode();
            }
            else
            {
                return base.GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            OrderProductionPlan another = obj as OrderProductionPlan;

            if (another == null)
            {
                return false;
            }
            else
            {
                return (this.OrderPlanNo == another.OrderPlanNo);
            }
        }
    }
}

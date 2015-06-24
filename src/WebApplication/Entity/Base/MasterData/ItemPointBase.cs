using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public abstract class ItemPointBase : EntityBase
    {
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
        private string _fact;
        public string Fact
        {
            get
            {
                return _fact;
            }
            set
            {
                _fact = value;
            }
        }
        private string _model;
        public string Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
            }
        }
        private int? _point;
        public int? Point
        {
            get
            {
                return _point;
            }
            set
            {
                _point = value;
            }
        }
        private int? _transferTime;
        public int? TransferTime
        {
            get
            {
                return _transferTime;
            }
            set
            {
                _transferTime = value;
            }
        }
        private decimal? _equipmentTime;
        public decimal? EquipmentTime
        {
            get
            {
                return _equipmentTime;
            }
            set
            {
                _equipmentTime = value;
            }
        }
        private int? _pCBNumber;
        public int? PCBNumber
        {
            get
            {
                return _pCBNumber;
            }
            set
            {
                _pCBNumber = value;
            }
        }
    }
}

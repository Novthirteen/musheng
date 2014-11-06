using System;

//TODO: Add other using statements here

namespace com.Sconit.Entity.Cost
{
    [Serializable]
    public class InventoryBalance : InventoryBalanceBase
    {
        #region Non O/R Mapping Properties

        ////���ڽ��
        //private decimal _startInvQty;
        //public decimal StartInvQty
        //{
        //    get
        //    {
        //        return this._startInvQty;
        //    }

        //    set
        //    {
        //        this._startInvQty = value;
        //    }
        //}
        //���ڽ����
        private decimal _startInvBalance;
        public decimal StartInvBalance
        {
            get
            {
                return this._startInvBalance;
            }

            set
            {
                this._startInvBalance = value;
            }
        }

        //ֱ���˹��ϼ�
        private decimal _totalLabor;
        public decimal TotalLabor
        {
            get
            {
                return this._totalLabor;
            }

            set
            {
                this._totalLabor = value;
            }
        }

        //ֱ�Ӳ��Ϻϼ�
        private decimal _totalMaterial;
        public decimal TotalMaterial
        {
            get
            {
                return this._totalMaterial;
            }

            set
            {
                this._totalMaterial = value;
            }
        }
        //������úϼ�
        private decimal _totalMgtExpense;
        public decimal TotalMgtExpense
        {
            get
            {
                return this._totalMgtExpense;
            }

            set
            {
                this._totalMgtExpense = value;
            }
        }
        private decimal _totalMfcExpense;
        //������úϼ�
        public decimal TotalMfcExpense
        {
            get
            {
                return this._totalMfcExpense;
            }

            set
            {
                this._totalMfcExpense = value;
            }
        }

        #endregion
    }
}
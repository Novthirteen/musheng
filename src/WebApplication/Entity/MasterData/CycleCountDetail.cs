using System;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class CycleCountDetail : CycleCountDetailBase
    {
        #region Non O/R Mapping Properties

        //�¼ӿ���
        private Boolean _isBlankDetail = false;
        public Boolean IsBlankDetail
        {
            get
            {
                return _isBlankDetail;
            }
            set
            {
                _isBlankDetail = value;
            }
        }

        //��װ��
        private int _ucQty;
        public int UCQty
        {
            get
            {
                if (Qty != 0 && Item != null && Item.UnitCount > 0)
                    return (int)(Qty / Item.UnitCount);
                else
                    return _ucQty;
            }
            set
            {
                _ucQty = value;
            }
        }
        //��ͷ��
        private decimal _oddQty;
        public decimal OddQty
        {
            get
            {
                if (Qty != 0 && Item != null)
                {
                    if (Item.UnitCount > 0)
                        return (decimal)(Qty % Item.UnitCount);
                    else if (Item.UnitCount == 0)
                        return (decimal)Qty;
                    else
                        return _oddQty;
                }
                else
                    return _oddQty;
            }
            set
            {
                _oddQty = value;
            }
        }

        #endregion
    }
}
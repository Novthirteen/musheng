using System;
using com.Sconit.Entity.Distribution;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class LocationDetail : LocationDetailBase
    {
        #region Non O/R Mapping Properties

        //����;
        private decimal _inTransitQty;
        public decimal InTransitQty
        {
            get
            {
                return this._inTransitQty;
            }

            set
            {
                this._inTransitQty = value;
            }
        }

        //����;
        private decimal _inTransitQtyOut;
        public decimal InTransitQtyOut
        {
            get
            {
                return this._inTransitQtyOut;
            }

            set
            {
                this._inTransitQtyOut = value;
            }
        }

        //����
        private decimal _qtyToBeIn;
        public decimal QtyToBeIn
        {
            get
            {
                return this._qtyToBeIn;
            }

            set
            {
                this._qtyToBeIn = value;
            }
        }

        //����
        private decimal _qtyToBeOut;
        public decimal QtyToBeOut
        {
            get
            {
                return this._qtyToBeOut;
            }

            set
            {
                this._qtyToBeOut = value;
            }
        }

        private decimal _invQty;
        public decimal InvQty
        {
            get
            {
                return this._invQty;
            }

            set
            {
                this._invQty = value;
            }
        }

        //Ԥ�ƿ��ÿ��
        private decimal _pAB;
        public decimal PAB
        {
            get
            {
                return this._pAB;
            }

            set
            {
                this._pAB = value;
            }
        }

        //�ڳ����
        private decimal _startInvQty;
        public decimal StartInvQty
        {
            get
            {
                return this._startInvQty;
            }

            set
            {
                this._startInvQty = value;
            }
        }

        //�ܼ����
        private decimal _totalInQty;
        public decimal TotalInQty
        {
            get
            {
                return this._totalInQty;
            }

            set
            {
                this._totalInQty = value;
            }
        }

        //�ܼƳ���
        private decimal _totalOutQty;
        public decimal TotalOutQty
        {
            get
            {
                return this._totalOutQty;
            }

            set
            {
                this._totalOutQty = value;
            }
        }

        //�ɹ����
        private decimal _RCTPO;
        public decimal RCTPO
        {
            get
            {
                return this._RCTPO;
            }

            set
            {
                this._RCTPO = value;
            }
        }

        //�ƿ����
        private decimal _RCTTR;
        public decimal RCTTR
        {
            get
            {
                return this._RCTTR;
            }

            set
            {
                this._RCTTR = value;
            }
        }

        //�ƿ����(��ͨ)
        private decimal _RCTTRNML;
        public decimal RCTTRNML
        {
            get
            {
                return this._RCTTRNML;
            }

            set
            {
                this._RCTTRNML = value;
            }
        }

        //�ƿ����(ί��)
        private decimal _RCTTRSUB;
        public decimal RCTTRSUB
        {
            get
            {
                return this._RCTTRSUB;
            }

            set
            {
                this._RCTTRSUB = value;
            }
        }

        //�ƿ����(���)
        private decimal _RCTTRREM;
        public decimal RCTTRREM
        {
            get
            {
                return this._RCTTRREM;
            }

            set
            {
                this._RCTTRREM = value;
            }
        }

        //�������
        private decimal _RCTWO;
        public decimal RCTWO
        {
            get
            {
                return this._RCTWO;
            }

            set
            {
                this._RCTWO = value;
            }
        }

        //�������(����)
        private decimal _RCTWOHOM;
        public decimal RCTWOHOM
        {
            get
            {
                return this._RCTWOHOM;
            }

            set
            {
                this._RCTWOHOM = value;
            }
        }

        //�������(ί��)
        private decimal _RCTWOSUB;
        public decimal RCTWOSUB
        {
            get
            {
                return this._RCTWOSUB;
            }

            set
            {
                this._RCTWOSUB = value;
            }
        }

        //�ƻ������
        private decimal _RCTUNP;
        public decimal RCTUNP
        {
            get
            {
                return this._RCTUNP;
            }

            set
            {
                this._RCTUNP = value;
            }
        }

        //�������
        private decimal _RCTINP;
        public decimal RCTINP
        {
            get
            {
                return this._RCTINP;
            }

            set
            {
                this._RCTINP = value;
            }
        }

        //���۳���
        private decimal _ISSSO;
        public decimal ISSSO
        {
            get
            {
                return this._ISSSO;
            }

            set
            {
                this._ISSSO = value;
            }
        }

        //�ƿ����
        private decimal _ISSTR;
        public decimal ISSTR
        {
            get
            {
                return this._ISSTR;
            }

            set
            {
                this._ISSTR = value;
            }
        }

        //�ƿ����(��ͨ)
        private decimal _ISSTRNML;
        public decimal ISSTRNML
        {
            get
            {
                return this._ISSTRNML;
            }

            set
            {
                this._ISSTRNML = value;
            }
        }

        //�ƿ����(ί��)
        private decimal _ISSTRSUB;
        public decimal ISSTRSUB
        {
            get
            {
                return this._ISSTRSUB;
            }

            set
            {
                this._ISSTRSUB = value;
            }
        }

        //�ƿ����(���)
        private decimal _ISSTRREM;
        public decimal ISSTRREM
        {
            get
            {
                return this._ISSTRREM;
            }

            set
            {
                this._ISSTRREM = value;
            }
        }

        //��������
        private decimal _ISSWO;
        public decimal ISSWO
        {
            get
            {
                return this._ISSWO;
            }

            set
            {
                this._ISSWO = value;
            }
        }

        //�ƻ������
        private decimal _ISSUNP;
        public decimal ISSUNP
        {
            get
            {
                return this._ISSUNP;
            }

            set
            {
                this._ISSUNP = value;
            }
        }

        //�������
        private decimal _ISSINP;
        public decimal ISSINP
        {
            get
            {
                return this._ISSINP;
            }

            set
            {
                this._ISSINP = value;
            }
        }

        //�̵����
        private decimal _CYCCNT;
        public decimal CYCCNT
        {
            get
            {
                return this._CYCCNT;
            }

            set
            {
                this._CYCCNT = value;
            }
        }

        //FlowDetail
        private FlowDetail _flowDetail;
        public FlowDetail FlowDetail
        {
            get
            {
                return this._flowDetail;
            }

            set
            {
                this._flowDetail = value;
            }
        }

        public Region Region { get; set; }

        //��ת��
        public decimal? InvTurnRate
        {
            get
            {
                decimal avgInvQty = (this.StartInvQty + this.InvQty) / 2;
                if (avgInvQty != 0)
                {
                    return (-this.TotalOutQty) / avgInvQty;
                }
                else
                {
                    return null;
                }
            }
        }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// �Ѽ���
        /// </summary>
        public decimal PickedQty { get; set; }

        /// <summary>
        /// δͳ��
        /// </summary>
        public decimal NoStatsQty { get; set; }

        #endregion
    }
}
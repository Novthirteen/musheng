using System;
using com.Sconit.Entity.Production;
using System.Collections.Generic;
using com.Sconit.Entity.Distribution;

//TODO: Add other using statements here

namespace com.Sconit.Entity.MasterData
{
    [Serializable]
    public class ReceiptDetail : ReceiptDetailBase
    {
        #region Non O/R Mapping Properties

        //�ѷ���س������
        private IList<MaterialFlushBack> _materialFlushBack;
        public IList<MaterialFlushBack> MaterialFlushBack
        {
            get
            {
                return this._materialFlushBack;
            }

            set
            {
                this._materialFlushBack = value;
            }
        }

        public void AddMaterialFlushBack(MaterialFlushBack materialFlushBack)
        {
            if (this._materialFlushBack == null)
            {
                this._materialFlushBack = new List<MaterialFlushBack>();
            }

            this._materialFlushBack.Add(materialFlushBack);
        }

        //public decimal PlannedAmount { get; set; }

        //��¼�ջ����Ӧ�ķ�����
        //public InProcessLocationDetail InProcessLocationDetail { get; set; }

        private IList<ReceiptDetail> _huReceiptDetails;
        public IList<ReceiptDetail> HuReceiptDetails
        {
            get
            {
                return _huReceiptDetails;
            }
            set
            {
                this._huReceiptDetails = value;
            }
        }
        public void AddHuReceiptDetails(ReceiptDetail receiptDetail)
        {
            if (this.HuReceiptDetails == null)
            {
                this.HuReceiptDetails = new List<ReceiptDetail>();
            }

            this.HuReceiptDetails.Add(receiptDetail);
        }

        public string PutAwayBinCode { get; set; }

        public InProcessLocationDetail ReceivedInProcessLocationDetail { get; set; }

        public string HuSupplierLotNo { get; set; }

        public string HuSortLevel1 { get; set; }

        public string HuColorLevel1 { get; set; }

        public string HuSortLevel2 { get; set; }

        public string HuColorLevel2 { get; set; }
        #endregion
    }
}
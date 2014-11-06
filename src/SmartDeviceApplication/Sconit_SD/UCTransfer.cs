using Sconit_SD.SconitWS;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Sconit_SD
{
    public partial class UCTransfer : UCBase
    {
        private string moduleType;
        public UCTransfer(User user, string moduleType)
            : base(user, moduleType)
        {
            InitializeComponent();
            this.btnOrder.Text = "移库";
            this.moduleType = moduleType;
            if (moduleType == BusinessConstants.TRANSFORMER_MODULE_TYPE_ORDERQUICK)
            {
                this.btnOrder.Text = "快收";
            }
            this.resolver.ModuleType = BusinessConstants.TRANSFORMER_MODULE_TYPE_TRANSFER;
        }

        protected override void gvListDataBind()
        {
            base.gvListDataBind();

            if (this.resolver != null && this.resolver.IsScanHu)
            {
                List<Transformer> transformerList = new List<Transformer>();
                foreach (Transformer transformer in this.resolver.Transformers)
                {
                    if (transformer.CurrentQty != 0)
                    {
                        transformerList.Add(transformer);
                    }
                }
                this.dgList.DataSource = transformerList;
                ts.MappingName = transformerList.GetType().Name;
            }
        }

        protected override void BarCodeScan()
        {
            base.BarCodeScan();
            if (this.resolver.OrderType != null)
            {
                if (this.moduleType == BusinessConstants.TRANSFORMER_MODULE_TYPE_TRANSFER
                    && this.resolver.OrderType != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_TRANSFER)
                {
                    this.InitialAll();
                    this.btnHidden.Focus();
                    Utility.ShowMessageBox("不是移库路线类型不能操作!");
                }
                else if (this.moduleType == BusinessConstants.TRANSFORMER_MODULE_TYPE_ORDERQUICK
                    && this.resolver.OrderType != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT
                    && this.resolver.OrderType != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_SUBCONCTRACTING
                    && this.resolver.OrderType != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_CUSTOMERGOODS)
                {
                    this.InitialAll();
                    this.btnHidden.Focus();
                    Utility.ShowMessageBox("不是采购,客供,委外路线类型不能操作!");
                }
            }
        }
    }
}

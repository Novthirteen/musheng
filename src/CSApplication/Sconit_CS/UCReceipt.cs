using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sconit_CS.SconitWS;
using System.ServiceModel;

namespace Sconit_CS
{
    public partial class UCReceipt : UCBase
    {
        public UCReceipt(User user, string moduleType)
            : base(user, moduleType)
        {
            InitializeComponent();
            this.btnConfirm.Text = "收货";
            this.gvList.Columns["Qty"].HeaderText = "订单数";
            this.gvList.Columns["CurrentQty"].HeaderText = "实收数";
            this.gvList.Columns["LocationFromCode"].Visible = false;
        }

        protected override void gvHuList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int index = this.gvHuList.CurrentCell.RowIndex;
            this.gvHuList.CurrentCell = this.gvHuList.Rows[index].Cells["HuQty"];
            this.gvHuList.BeginEdit(true);
        }

        protected override void BarCodeScan()
        {
            base.BarCodeScan();
            if (resolver.Code == BusinessConstants.CODE_PREFIX_PICKLIST)
            {
                this.btnConfirm.Focus();
                this.tbBarCode.Text = resolver.Code;
            }
        }

        protected override void OrderConfirm()
        {
            try
            {
                if (!Utility.IsHasTransformer(this.resolver))
                {
                    this.DataBind();
                    this.lblMessage.Text = "明细不能为空";
                    this.tbBarCode.Focus();
                    return;
                }
                Utility.RemoveLot(resolver);
                this.resolver.Input = BusinessConstants.BARCODE_SPECIAL_MARK + BusinessConstants.BARCODE_HEAD_OK;
                this.resolver = TheClientMgr.ScanBarcode(this.resolver);
                this.lblMessage.Text = this.resolver.Result;

                if ((this.resolver.NeedPrintReceipt || this.resolver.NeedInspection)
                    && this.resolver.PrintUrl != null && this.resolver.PrintUrl != string.Empty)
                {
                    string[] printUrlArray = this.resolver.PrintUrl.Split('|');
                    foreach (string printUrl in printUrlArray)
                    {
                        Utility.PrintOrder(printUrl, this);
                    }
                }

                InitialAll();
            }
            catch (FaultException ex)
            {
                this.DataBind();
                this.lblMessage.Text = Utility.FormatExMessage(ex.Message);
                MessageBox.Show(this, Utility.FormatExMessage(ex.Message));
                this.tbBarCode.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "程序内部错误,请与管理员联系", MessageBoxButtons.OK, MessageBoxIcon.Error);
                InitialAll();
            }
        }
    }
}
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
    public partial class UCUnitization : UCBase
    {
        public UCUnitization(User user, string moduleType)
            : base(user, moduleType)
        {
            InitializeComponent();
            this.btnConfirm.Text = "单元化";
            this.gvList.Columns["LocationFromCode"].Visible = false;
            this.gvList.Columns["LocationToCode"].Visible = false;
            this.gvList.Columns["Cartons"].Visible = false;
            this.gvList.Columns["OrderNo"].Visible = false;
            this.gvList.Columns["LotNumber"].Visible = false;
            this.gvList.Columns["UomCode"].Visible = true;
            this.gvList.Columns["s1"].Visible = true;
            this.gvList.Columns["s2"].Visible = true;
            this.gvList.Columns["s3"].Visible = true;
            this.gvList.Columns["Qty"].HeaderText = "库存数";
            this.gvList.Columns["CurrentQty"].HeaderText = "合格数";
            this.lblResult.Text = "请先扫描库位,再扫描物流路线!";
        }

        protected override void gvList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.gvList.BeginEdit(true);
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
                this.resolver.Input = BusinessConstants.BARCODE_SPECIAL_MARK + BusinessConstants.BARCODE_HEAD_OK;
                this.resolver = TheClientMgr.ScanBarcode(this.resolver);
                this.lblMessage.Text = this.resolver.Result;

                if (this.resolver.PrintUrl != null && this.resolver.PrintUrl != string.Empty)
                {
                    Utility.PrintOrder(this.resolver.PrintUrl, this);
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

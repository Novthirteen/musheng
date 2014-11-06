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
    public partial class UCMaterialChange : UserControl
    {
        private Resolver resolver;
        private ClientMgrWSSoapClient TheClientMgr;
        private User user;
        private string moduleType;

        public UCMaterialChange(User user, string moduleType)
        {
            InitializeComponent();
            this.dgInList.AutoGenerateColumns = false;
            this.dgOutList.AutoGenerateColumns = false;
            TheClientMgr = new ClientMgrWSSoapClient();
            this.user = user;
            this.moduleType = moduleType;
            this.InitialAll();
        }

        private void InitialAll()
        {
            this.tbBarCode.Text = string.Empty;
            this.tbBarCode.Focus();
            this.lblMessage.Text = string.Empty;
            this.resolver = new Resolver();
            this.resolver.UserCode = this.user.Code;
            this.resolver.ModuleType = this.moduleType;
            this.resolver.IOType = BusinessConstants.IO_TYPE_IN;
            this.resolver.Transformers = new Transformer[2];
            this.resolver.Transformers[0] = new Transformer();
            this.resolver.Transformers[1] = new Transformer();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Enter))
            {
                this.MaterialChange();
                return true;
            }
            else if (keyData == Keys.Escape)
            {
                try
                {
                    if (this.resolver != null)
                    {
                        this.InitialAll();
                        return true;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                    return true;
                }
            }
            else if (keyData == (Keys.Control | Keys.P) && this.resolver.Code != null)
            {
                return true;
            }

            if (this.tbBarCode.Focused)
            {
                if (keyData == Keys.Enter)
                {
                    this.BarCodeScan();
                    return true;
                }
            }
            else if (this.btnDevanning.Focused)
            {
                if (keyData == Keys.Enter)
                {
                    if (this.tbBarCode.Text.Trim() != string.Empty)
                    {
                        this.BarCodeScan();
                    }
                    else
                    {
                        this.MaterialChange();
                    }
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void BarCodeScan()
        {
            try
            {
                this.tbBarCode.Text = this.tbBarCode.Text.Trim();
                if (this.tbBarCode.Text != string.Empty)
                {
                    if (Utility.IsHasTransformer(this.resolver))
                    {
                        if (this.resolver.Transformers[0].TransformerDetails == null
                            || this.resolver.Transformers[0].TransformerDetails.Length == 0)
                        {
                            this.resolver.IOType = BusinessConstants.IO_TYPE_IN;
                        }
                        else if (this.resolver.Transformers[0].TransformerDetails != null
                            && this.resolver.Transformers[0].TransformerDetails.Length == 1)
                        {
                            this.resolver.IOType = BusinessConstants.IO_TYPE_OUT;
                        }
                        else
                        {
                            MessageBox.Show(this, "已经有换料前和换料后条码，请提交", "已经有换料前和换料后条码!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.btnDevanning.Focus();
                        }
                    }

                    this.resolver.Input = this.tbBarCode.Text;
                    this.resolver = TheClientMgr.ScanBarcode(this.resolver);
                    this.BindTransformer();
                }
            }
            catch (FaultException ex)
            {
                MessageBox.Show(this, Utility.FormatExMessage(ex.Message), "获取扫条码信息失败!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.tbBarCode.Text = string.Empty;
                this.tbBarCode.Focus();
                this.lblMessage.Text = Utility.FormatExMessage(ex.Message);
                this.lblMessage.ForeColor = Color.Red;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "程序内部错误, 请与管理员联系", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.InitialAll();
            }
        }

        private void MaterialChange()
        {
            try
            {
                this.resolver.Input = BusinessConstants.BARCODE_SPECIAL_MARK + BusinessConstants.BARCODE_HEAD_OK;
                this.resolver = TheClientMgr.ScanBarcode(this.resolver);
                this.InitialAll();
                this.BindTransformer();
                this.lblMessage.Text = "换料成功!";
            }
            catch (FaultException ex)
            {
                MessageBox.Show(this, Utility.FormatExMessage(ex.Message), "错误:换料失败!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.tbBarCode.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "程序内部错误,请与管理员联系", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.InitialAll();
            }
        }

        private void BindTransformer()
        {
            if (this.resolver != null && this.resolver.Transformers != null && this.resolver.Transformers.Length == 2)
            {
                this.dgInList.DataSource = this.resolver.Transformers[0].TransformerDetails;
                this.dgOutList.DataSource = this.resolver.Transformers[1].TransformerDetails;
            }
            if (this.resolver.Transformers[0].TransformerDetails == null || this.resolver.Transformers[0].TransformerDetails.Length == 0)
            {
                this.lblBarCode.Text = "换料前条码";
            }
            else
            {
                this.lblBarCode.Text = "换料后条码";
            }
            this.tbBarCode.Text = string.Empty;
        }

        private void btnDevanning_Click(object sender, EventArgs e)
        {
            this.MaterialChange();
        }
        private void tbBarCode_TextChanged(object sender, EventArgs e)
        {
            if (this.tbBarCode.Text != string.Empty)
            {
                this.lblMessage.Text = string.Empty;
            }
        }
    }
}

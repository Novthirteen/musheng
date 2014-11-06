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
    public partial class UCLoadMaterial : UserControl
    {
        private Resolver resolver;
        private ClientMgrWSSoapClient TheClientMgr;
        private string moduleType;
        private User user;

        public UCLoadMaterial(User user, string moduleType)
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
            this.resolver.ModuleType = this.moduleType;
            this.resolver.UserCode = this.user.Code;
            this.resolver.Transformers = new Transformer[2];
            this.resolver.Transformers[0] = new Transformer();
            this.resolver.Transformers[1] = new Transformer();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Enter))
            {
                this.LoadMaterial();
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

            if (this.tbBarCode.Focused)
            {
                if (keyData == Keys.Enter)
                {
                    this.BarCodeScan();
                    return true;
                }
            }
            else if (this.btnLoadMaterial.Focused)
            {
                if (keyData == Keys.Enter)
                {
                    if (this.tbBarCode.Text.Trim() != string.Empty)
                    {
                        this.BarCodeScan();
                    }
                    else
                    {
                        this.LoadMaterial();
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
                if (this.tbBarCode.Text.Trim() == string.Empty)
                {
                    return;
                }
                this.tbBarCode.Text = this.tbBarCode.Text.Trim();
                this.resolver.Input = this.tbBarCode.Text;
                this.resolver = TheClientMgr.ScanBarcode(this.resolver);
                this.BindTransformer();
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

        private void LoadMaterial()
        {
            try
            {
                this.resolver.Input = BusinessConstants.BARCODE_SPECIAL_MARK + BusinessConstants.BARCODE_HEAD_OK;
                this.resolver = TheClientMgr.ScanBarcode(this.resolver);
                this.InitialAll();
                this.BindTransformer();
                this.lblMessage.Text = "检验上料成功!";
            }
            catch (FaultException ex)
            {
                MessageBox.Show(this, Utility.FormatExMessage(ex.Message), "错误:检验上料失败!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            this.tbBarCode.Text = string.Empty;
            this.lblMessage.Text = resolver.Result;
        }

        private void btnLoadMaterial_Click(object sender, EventArgs e)
        {
            this.LoadMaterial();
        }

        private void tbBarCode_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

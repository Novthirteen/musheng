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
    public partial class UCLoadMaterialPrint : UserControl
    {

        private User user;
        private string moduleType;
        private string orderNo;
        private ClientMgrWSSoapClient TheClientMgr;
        protected Resolver resolver;

        public UCLoadMaterialPrint(User user, string moduleType)
        {
            InitializeComponent();
            this.lblMessage.Text = string.Empty;
            this.user = user;
            this.moduleType = moduleType;
            this.resolver = new Resolver();
            resolver.ModuleType = this.moduleType;
            this.resolver.UserCode = this.user.Code;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            CodeScan();
        }

        private void CodeScan()
        {
            try
            {
                if (this.tbWO.Text.Trim().Length > 5)
                {
                    this.resolver.Input = tbWO.Text.Trim();
                    this.resolver = TheClientMgr.ScanBarcode(resolver);
                    if (this.resolver.PrintUrl != null && this.resolver.PrintUrl != string.Empty)
                    {
                        Utility.PrintOrder(this.resolver.PrintUrl, this);
                    }
                }
                else
                {
                    this.tbWO.Text = string.Empty;
                    this.lblMessage.Text = "请输入正确的工单号!";
                    this.lblMessage.ForeColor = Color.Red;
                    this.tbWO.Focus();
                }
            }
            catch (FaultException ex)
            {
                MessageBox.Show(this, Utility.FormatExMessage(ex.Message));
                this.tbWO.Text = string.Empty;
                this.tbWO.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "程序内部错误,请与管理员联系", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.tbWO.Text = string.Empty;
                this.tbWO.Focus();
            }
        }

        private void UCLoadMaterialPrint_Load(object sender, EventArgs e)
        {
            this.TheClientMgr = new ClientMgrWSSoapClient();

            this.tbWO.Text = string.Empty;
            this.tbWO.Focus();
        }

        private void tbWO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (this.tbWO.Text.Trim() != "" && this.tbWO.Text.Length > 4)
                {
                    this.CodeScan();
                }
                else
                {
                    this.tbWO.Text = string.Empty;
                    this.lblMessage.Text = "请输入正确的工单号!";
                    this.lblMessage.ForeColor = Color.Red;
                    this.tbWO.Focus();
                }
            }
        }
    }
}

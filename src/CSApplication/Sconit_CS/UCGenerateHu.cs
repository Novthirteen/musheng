using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sconit_CS.SconitWS;

namespace Sconit_CS
{
    public partial class UCGenerateHu : UserControl
    {
        private User user;
        protected ClientMgrWSSoapClient TheClientMgr;
        protected Resolver resolver;

        public UCGenerateHu(User user, string moduleTyp)
        {
            InitializeComponent();
            this.TheClientMgr = new ClientMgrWSSoapClient();
            this.user = user;
            this.resolver = new Resolver();
            this.resolver.UserCode = user.Code;
            this.resolver.ModuleType = BusinessConstants.TRANSFORMER_MODULE_TYPE_GENERATEHU;
            this.InitialAll();
        }

        private void InitialAll()
        {
           
        }

        private void btnGenerateHu_Click(object sender, EventArgs e)
        {
            this.lblMessage.Visible = false;
            if(Validate())
            {
                IList<string> values = new List<string>();
                values.Add(this.tbItem.Text.Trim() == string.Empty ? "   " : this.tbItem.Text.Trim());
                values.Add(this.tbQty.Text.Trim() == string.Empty ? "   " : this.tbQty.Text.Trim());
                values.Add(this.tbLot.Text.Trim() == string.Empty ? "   " : this.tbLot.Text.Trim());
                values.Add(this.tbSupplier.Text.Trim() == string.Empty ? "   " : this.tbSupplier.Text.Trim());
                values.Add(this.tbCAT.Text.Trim() == string.Empty ? "   " : this.tbCAT.Text.Trim());
                values.Add(this.tbHUE.Text.Trim() == string.Empty ? "   " : this.tbHUE.Text.Trim());

                this.resolver.Input = BusinessConstants.BARCODE_SPECIAL_MARK + BusinessConstants.BARCODE_HEAD_OK;
                this.resolver.s = values.ToArray<string>();
                this.resolver.AutoPrintHu = true;
                this.resolver.PrintUrl = string.Empty;
                try
                {
                    this.resolver = TheClientMgr.ScanBarcode(this.resolver);
                    if (this.resolver.AutoPrintHu && this.resolver.PrintUrl != null && this.resolver.PrintUrl != string.Empty)
                    {
                        Utility.PrintOrder(this.resolver.PrintUrl, this);
                    }
                    this.lblMessage.Text = this.resolver.Result;
                }
                catch (Exception ee)
                {
                    this.lblMessage.Text = Utility.FormatExMessage(ee.Message);
                    this.tbItem.Focus();
                }

                this.lblMessage.Visible = true;
            }
        }

        private bool Validate()
        {
            this.lblMessage.Visible = false;
            if (this.tbItem.Text.Trim().Length == 0)
            {
                this.lblMessage.Text = "物料号不能为空";
                this.lblMessage.Visible = true;
                return false;
            }
            if (this.tbQty.Text.Trim().Length == 0 )
            {
                this.lblMessage.Text = "数量不能为空";
                this.lblMessage.Visible = true;
                return false;
            }else if(!Utility.IsDecimal(this.tbQty.Text.Trim()))
            {
                this.lblMessage.Text = "数量必须为数字";
                this.lblMessage.Visible = true;
                return false;
            }
            
            return true;
        }

        private void tbItem_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void UCGenerateHu_KeyUp(object sender, KeyEventArgs e)
        {
            string key = e.KeyData.ToString();
            if (key.ToUpper() == "TAB" || key.ToUpper() == "RETURN")
            {

                this.tbItem.Text = key.Substring(0, key.Length-8);
                this.tbLot.Text = key.Substring(key.Length-8,4);
                
                return;
            }
        }

        
    }
}

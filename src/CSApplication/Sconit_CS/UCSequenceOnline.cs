using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.ServiceModel;
using Sconit_CS.SconitWS;

namespace Sconit_CS
{
    public partial class UCSequenceOnline : UserControl
    {
        private Resolver resolver;
        private ClientMgrWSSoapClient TheClientMgr = new ClientMgrWSSoapClient();
        public UCSequenceOnline(User user,string moduleType)
        {
            InitializeComponent();
            this.resolver = new Resolver();
            this.resolver.UserCode = user.Code;
            this.resolver.ModuleType = BusinessConstants.TRANSFORMER_MODULE_TYPE_ONLINE;
            this.lblmessage.Text = string.Empty;
        }

        private void UCSequenceOnline_Load(object sender, EventArgs e)
        { }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Enter))
            {
                ExecuteOnline();
                return true;
            }
            
            if (keyData == Keys.Enter)
            {

                this.lblmessage.Text = string.Empty;
                this.resolver.Input = this.tbFlow.Text.Trim();
                this.resolver = TheClientMgr.ScanBarcode(resolver);
                this.gvSubmitWODataBind();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        private void gvSubmitWODataBind()
        {
            if (this.resolver.Transformers == null)
            {
                this.resolver.Transformers = new Transformer[] { };
            }
            this.gvSubmitWO.DataSource = resolver.Transformers;
            this.gvSubmitWO.ClearSelection();
        }

        private void ExecuteOnline()
        {
            string woNo = this.gvSubmitWO.SelectedRows[0].Cells["WONo1"].ToString();
        }
    }
}

using System.Windows.Forms;
using Sconit_SD.SconitWS;
using System.Collections.Generic;
using System;

namespace Sconit_SD
{
    public partial class UCOnline : UCBase
    {
        private List<Transformer> transformerList;

        public UCOnline(User user, string moduleType)
            : base(user, moduleType)
        {
            InitializeComponent();
            transformerList = new List<Transformer>();
            this.gvLayout(moduleType);
        }

        protected override void BarCodeScan()
        {
            if (this.tbBarCode.Text == string.Empty)
            {
                return;
            }
            base.BarCodeScan();
            this.lblMessage.Visible = true;
            this.lblResult.Text = string.Empty;
            this.lblMessage.Text = this.resolver.Result;

            if (this.resolver.OrderNo != null && this.resolver.OrderNo != string.Empty)
            {
                Transformer transformer = new Transformer();
                transformer.OrderNo = this.resolver.Code;          //当前上线的Picklist
                transformer.ItemDescription = DateTime.Now.ToString("MM-dd HH:mm:ss");//替代上线时间(当前上线的Picklist的时间)
                //倒序
                List<Transformer> transformers = new List<Transformer>();
                transformers.Add(transformer);
                transformers.AddRange(this.transformerList);
                this.transformerList = transformers;
                this.gvListDataBind();

                string message = "工单:" + transformer.OrderNo + "上线成功!";
                if (resolver.ModuleType == BusinessConstants.TRANSFORMER_MODULE_TYPE_PICKLISTONLINE)
                {
                    message = "拣货单:" + transformer.OrderNo + "上线成功!";
                }
                this.btnHidden.Focus();
                MessageBox.Show(message);
            }
        }

        protected override void gvListDataBind()
        {
            this.dgList.DataSource = this.transformerList;
            this.gvLayout(this.resolver.ModuleType);
        }


        protected override void tbBarCode_TextChanged(object sender, EventArgs e)
        {
            //base.tbBarCode_TextChanged(sender, e);
        }

        private void gvLayout(string ModuleType)
        {
            ts = new DataGridTableStyle();
            ts.MappingName = this.transformerList.GetType().Name;

            columnItemCode.MappingName = "OrderNo";
            if (ModuleType == BusinessConstants.TRANSFORMER_MODULE_TYPE_ONLINE)
            {
                columnItemCode.HeaderText = "生产单号";
            }
            else if (ModuleType == BusinessConstants.TRANSFORMER_MODULE_TYPE_PICKLISTONLINE)
            {
                columnItemCode.HeaderText = "拣货单号";
            }
            columnItemCode.Width = 100;
            ts.GridColumnStyles.Add(columnItemCode);

            columnItemDescription.MappingName = "ItemDescription";
            columnItemDescription.HeaderText = "上线时间";
            columnItemDescription.Width = 130;
            ts.GridColumnStyles.Add(columnItemDescription);

            this.dgList.TableStyles.Clear();
            this.dgList.TableStyles.Add(ts);

            this.ResumeLayout();
        }
    }
}

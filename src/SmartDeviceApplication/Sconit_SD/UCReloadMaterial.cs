using Sconit_SD.SconitWS;
using System.Web.Services.Protocols;
using System;
using System.Windows.Forms;
using System.Drawing;

namespace Sconit_SD
{
    public partial class UCReloadMaterial : UCBase
    {
        private Label lblPosition1;
        private Label lblHuId1;
        private Label lblItemCode1;
        private Label lblSortCorlorLevel11;
        private Label lblSortCorlorLevel12;
        private Label lblItemDescription1;
        private Label lblCurrentQty1;

        private Label lblPosition2;
        private Label lblHuId2;
        private Label lblItemCode2;
        private Label lblSortCorlorLevel21;
        private Label lblSortCorlorLevel22;
        private Label lblItemDescription2;
        private Label lblCurrentQty2;


        private TabControl tabMaterial1;
        private TabPage tabPageMaterial1;
        private TabPage tabPageMaterial2;

        public UCReloadMaterial(User user, string moduleType)
            : base(user, moduleType)
        {
            InitializeComponent();
            this.btnOrder.Text = "换料";
            this.lblMessage.Visible = true;
            this.lblMessage.Text = "请扫描换料前条码";

            #region Layout1
            this.lblPosition1 = new Label();
            this.lblPosition1.ForeColor = System.Drawing.Color.Black;
            this.lblPosition1.Location = new System.Drawing.Point(0, 5);
            this.lblPosition1.Name = "lblPosition1";
            this.lblPosition1.Size = new System.Drawing.Size(320, 20);

            this.lblHuId1 = new Label();
            this.lblHuId1.ForeColor = System.Drawing.Color.Black;
            this.lblHuId1.Location = new System.Drawing.Point(0, 35);
            this.lblHuId1.Name = "lblHuId1";
            this.lblHuId1.Size = new System.Drawing.Size(320, 20);

            this.lblItemCode1 = new Label();
            this.lblItemCode1.ForeColor = System.Drawing.Color.Black;
            this.lblItemCode1.Location = new System.Drawing.Point(0, 65);
            this.lblItemCode1.Name = "lblItemCode1";
            this.lblItemCode1.Size = new System.Drawing.Size(320, 20);
            this.lblItemCode1.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));


            this.lblItemDescription1 = new Label();
            this.lblItemDescription1.ForeColor = System.Drawing.Color.Black;
            this.lblItemDescription1.Location = new System.Drawing.Point(0, 95);
            this.lblItemDescription1.Name = "lblItemDescription1";
            this.lblItemDescription1.Size = new System.Drawing.Size(320, 20);
            this.lblItemDescription1.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));

            this.lblCurrentQty1 = new Label();
            this.lblCurrentQty1.ForeColor = System.Drawing.Color.Black;
            this.lblCurrentQty1.Location = new System.Drawing.Point(0, 125);
            this.lblCurrentQty1.Name = "lblCurrentQty1";
            this.lblCurrentQty1.Size = new System.Drawing.Size(320, 20);

            this.lblSortCorlorLevel11 = new Label();
            this.lblSortCorlorLevel11.ForeColor = System.Drawing.Color.Black;
            this.lblSortCorlorLevel11.Location = new System.Drawing.Point(0, 155);
            this.lblSortCorlorLevel11.Name = "lblSortCorlorLevel11";
            this.lblSortCorlorLevel11.Size = new System.Drawing.Size(320, 20);
            this.lblSortCorlorLevel11.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));


            this.lblSortCorlorLevel12 = new Label();
            this.lblSortCorlorLevel12.ForeColor = System.Drawing.Color.Black;
            this.lblSortCorlorLevel12.Location = new System.Drawing.Point(0, 185);
            this.lblSortCorlorLevel12.Name = "lblSortCorlorLevel12";
            this.lblSortCorlorLevel12.Size = new System.Drawing.Size(320, 20);
            this.lblSortCorlorLevel12.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));

            ///tab
            this.tabPageMaterial1 = new TabPage();
            this.tabPageMaterial1.SuspendLayout();
            this.tabPageMaterial1.Text = "换料前条码";
            this.tabPageMaterial1.ResumeLayout(false);
            this.tabPageMaterial1.Controls.Add(this.lblPosition1);
            this.tabPageMaterial1.Controls.Add(this.lblHuId1);
            this.tabPageMaterial1.Controls.Add(this.lblItemCode1);
            this.tabPageMaterial1.Controls.Add(this.lblItemDescription1);
            this.tabPageMaterial1.Controls.Add(this.lblCurrentQty1);
            this.tabPageMaterial1.Controls.Add(this.lblSortCorlorLevel11);
            this.tabPageMaterial1.Controls.Add(this.lblSortCorlorLevel12);

            #endregion

            #region Layout2
            ///Layout2
            this.lblPosition2 = new Label();
            this.lblPosition2.ForeColor = System.Drawing.Color.Black;
            this.lblPosition2.Location = new System.Drawing.Point(0, 5);
            this.lblPosition2.Name = "lblPosition2";
            this.lblPosition2.Size = new System.Drawing.Size(320, 20);

            this.lblHuId2 = new Label();
            this.lblHuId2.ForeColor = System.Drawing.Color.Black;
            this.lblHuId2.Location = new System.Drawing.Point(0, 35);
            this.lblHuId2.Name = "lblHuId2";
            this.lblHuId2.Size = new System.Drawing.Size(320, 20);

            this.lblItemCode2 = new Label();
            this.lblItemCode2.ForeColor = System.Drawing.Color.Black;
            this.lblItemCode2.Location = new System.Drawing.Point(0, 65);
            this.lblItemCode2.Name = "lblItemCode2";
            this.lblItemCode2.Size = new System.Drawing.Size(320, 20);
            this.lblItemCode2.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));

            this.lblItemDescription2 = new Label();
            this.lblItemDescription2.ForeColor = System.Drawing.Color.Black;
            this.lblItemDescription2.Location = new System.Drawing.Point(0, 95);
            this.lblItemDescription2.Name = "lblItemDescription2";
            this.lblItemDescription2.Size = new System.Drawing.Size(320, 20);
            this.lblItemDescription2.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));

            this.lblCurrentQty2 = new Label();
            this.lblCurrentQty2.ForeColor = System.Drawing.Color.Black;
            this.lblCurrentQty2.Location = new System.Drawing.Point(0, 125);
            this.lblCurrentQty2.Name = "lblCurrentQty2";
            this.lblCurrentQty2.Size = new System.Drawing.Size(320, 20);

            this.lblSortCorlorLevel21 = new Label();
            this.lblSortCorlorLevel21.ForeColor = System.Drawing.Color.Black;
            this.lblSortCorlorLevel21.Location = new System.Drawing.Point(0, 155);
            this.lblSortCorlorLevel21.Name = "lblSortCorlorLevel2";
            this.lblSortCorlorLevel21.Size = new System.Drawing.Size(320, 20);
            this.lblSortCorlorLevel21.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));

            this.lblSortCorlorLevel22 = new Label();
            this.lblSortCorlorLevel22.ForeColor = System.Drawing.Color.Black;
            this.lblSortCorlorLevel22.Location = new System.Drawing.Point(0, 185);
            this.lblSortCorlorLevel22.Name = "lblSortCorlorLevel22";
            this.lblSortCorlorLevel22.Size = new System.Drawing.Size(320, 20);
            this.lblSortCorlorLevel22.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));

            ///tab
            this.tabPageMaterial2 = new TabPage();
            this.tabPageMaterial2.SuspendLayout();
            //this.tabPageMaterial2.Size = new Size(300, 100);
            this.tabPageMaterial2.Text = "换料后条码";
            this.tabPageMaterial2.ResumeLayout(false);

            this.tabPageMaterial2.Controls.Add(this.lblPosition2);
            this.tabPageMaterial2.Controls.Add(this.lblHuId2);
            this.tabPageMaterial2.Controls.Add(this.lblItemCode2);
            this.tabPageMaterial2.Controls.Add(this.lblItemDescription2);
            this.tabPageMaterial2.Controls.Add(this.lblCurrentQty2);
            this.tabPageMaterial2.Controls.Add(this.lblSortCorlorLevel21);
            this.tabPageMaterial2.Controls.Add(this.lblSortCorlorLevel22);

            #endregion

            this.tabMaterial1 = new TabControl();
            this.tabMaterial1.SuspendLayout();
            this.tabMaterial1.Name = "tabMaterial1";
            this.tabMaterial1.SelectedIndex = 0;
            this.tabMaterial1.Location = new Point(3, 60);
            this.tabMaterial1.Size = new Size(300, 250);
            this.tabMaterial1.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));
            this.tabMaterial1.Controls.Add(this.tabPageMaterial1);
            this.tabMaterial1.Controls.Add(this.tabPageMaterial2);
            this.Controls.Add(this.tabMaterial1);
            this.tabMaterial1.ResumeLayout(false);

            #region 测试
            if (false)
            {
                this.lblPosition1.Text = "位号:12";
                //this.Controls.Add(this.lblPosition1);
                this.lblHuId1.Text = "条码:HU189776767978";
                //this.Controls.Add(this.lblHuId1);
                this.lblItemCode1.Text = "物料:123456789012345678901234567890";
                //this.Controls.Add(this.lblItemCode1);
                this.lblItemDescription1.Text = "描述:123456789012345678901234567890ABCD 描述要长~~~";
                //this.Controls.Add(this.lblItemDescription1);
                this.lblCurrentQty1.Text = "数量:10000";
                //this.Controls.Add(this.lblCurrentQty1);
                this.lblSortCorlorLevel11.Text = "分光1:12-12 分色1:45-56";
                //this.Controls.Add(this.lblSortCorlorLevel1);
                this.lblSortCorlorLevel12.Text = "分光2:12-12 分色2:32-43";
                //this.Controls.Add(this.lblSortCorlorLevel1);


                this.lblPosition2.Text = "位号:22";
                //this.Controls.Add(this.lblPosition2);
                this.lblHuId2.Text = "条码:HU298980998789789";
                //this.Controls.Add(this.lblHuId2);
                this.lblItemCode2.Text = "物料:2123456789012345678901234567890";
                //this.Controls.Add(this.lblItemCode2);
                this.lblItemDescription2.Text = "描述:2123456789012345678901234567890ABCD 描述要长~~~";
                //this.Controls.Add(this.lblItemDescription2);
                this.lblCurrentQty2.Text = "数量:20000";
                //this.Controls.Add(this.lblCurrentQty2);
                this.lblSortCorlorLevel21.Text = "分光1:22-12 分色1:45-56";
                //this.Controls.Add(this.lblSortCorlorLevel2);
                this.lblSortCorlorLevel22.Text = "分光2:22-12 分色2:32-43";
                //this.Controls.Add(this.lblSortCorlorLevel2);

                this.tbBarCode.Text = "TE0B4130003";
            }
            #endregion
        }

        public override void InitialAll()
        {
            base.InitialAll();
            this.dgList.Visible = false;
            this.resolver.IOType = BusinessConstants.IO_TYPE_IN;
            this.resolver.Transformers = new Transformer[2];
            this.resolver.Transformers[0] = new Transformer();
            this.resolver.Transformers[1] = new Transformer();
            this.resolver.AllowExceed = (this.resolver.ModuleType == BusinessConstants.TRANSFORMER_MODULE_TYPE_RELOADMATERIAL);
            this.tbBarCode.Focus();
            this.resolver.ModuleType = BusinessConstants.TRANSFORMER_MODULE_TYPE_RELOADMATERIAL;

            this.lblHuId1.Text = string.Empty;
            this.lblItemCode1.Text = string.Empty;
            this.lblItemDescription1.Text = string.Empty;
            this.lblCurrentQty1.Text = string.Empty;
            this.lblSortCorlorLevel11.Text = string.Empty;
            this.lblSortCorlorLevel12.Text = string.Empty;

            this.lblHuId2.Text = string.Empty;
            this.lblItemCode2.Text = string.Empty;
            this.lblItemDescription2.Text = string.Empty;
            this.lblCurrentQty2.Text = string.Empty;
            this.lblSortCorlorLevel21.Text = string.Empty;
            this.lblSortCorlorLevel22.Text = string.Empty;

            this.tabMaterial1.SelectedIndex = 0;
        }
        protected override void BarCodeScan()
        {
            this.DoReloadMaterial();
        }

        private void DoReloadMaterial()
        {
            try
            {
                this.dgList.Visible = false;
                this.resolver.Input = this.tbBarCode.Text.Trim();
                if (this.resolver.Input != string.Empty)
                {
                    this.resolver = TheSmartDeviceMgr.ScanBarcode(this.resolver);

                    this.gvHuListDataBind();
                }
            }
            catch (SoapException ex)
            {
                this.btnHidden.Focus();
                this.lblMessage.Text = Utility.FormatExMessage(ex.Message);
                this.lblMessage.Visible = true;
                Utility.ShowMessageBox(ex.Message);
            }
            catch (Exception ex)
            {
                this.InitialAll();
                this.lblMessage.Text = Utility.FormatExMessage(ex.Message);
                this.lblMessage.Visible = true;
                Utility.ShowMessageBox(ex.Message);
                this.tbBarCode.Focus();
                this.tbBarCode.Text = string.Empty;
            }
        }

        protected override void gvHuListDataBind()
        {
            if (this.resolver.Transformers != null
                       && this.resolver.Transformers.Length == 2
                       && this.resolver.Transformers[0].TransformerDetails != null
                       && this.resolver.Transformers[0].TransformerDetails.Length > 0)
            {
                TransformerDetail t1 = this.resolver.Transformers[0].TransformerDetails[0];

                //this.lblPosition1.Text = "位号:" + t1.Position;
                this.lblHuId1.Text = "条码:" + t1.HuId.ToString();
                this.lblItemCode1.Text = "物料:" + t1.ItemCode;
                this.lblItemDescription1.Text = "描述:" + t1.ItemDescription;
                this.lblCurrentQty1.Text = "数量:" + t1.CurrentQty.ToString("0.######");
                if (t1.ColorLevel1 != null && t1.ColorLevel1 != null)
                {
                    this.lblSortCorlorLevel11.Text = "分光1:" + t1.SortLevel1 + " 分色1:" + t1.ColorLevel1;
                }
                else
                {
                    this.lblSortCorlorLevel11.Text = string.Empty;
                }
                if (t1.ColorLevel2 != null && t1.ColorLevel2 != null)
                {
                    this.lblSortCorlorLevel12.Text = "分光2:" + t1.SortLevel2 + " 分色2:" + t1.ColorLevel2;
                }
                else
                {
                    this.lblSortCorlorLevel12.Text = string.Empty;
                }
                this.resolver.IOType = BusinessConstants.IO_TYPE_OUT;
                this.lblMessage.Text = "请扫描换料后条码";
                this.tbBarCode.Focus();
            }

            if (this.resolver.Transformers != null
                && this.resolver.Transformers.Length == 2
                && this.resolver.Transformers[1].TransformerDetails != null
                && this.resolver.Transformers[1].TransformerDetails.Length > 0)
            {
                TransformerDetail t2 = this.resolver.Transformers[1].TransformerDetails[0];

                //this.lblPosition2.Text = "位号:" + t2.Position;
                this.lblHuId2.Text = "条码:" + t2.HuId.ToString();
                this.lblItemCode2.Text = "物料:" + t2.ItemCode;
                this.lblItemDescription2.Text = "描述:" + t2.ItemDescription;
                this.lblCurrentQty2.Text = "数量:" + t2.CurrentQty.ToString("0.######");
                if (t2.ColorLevel1 != null && t2.ColorLevel1 != null)
                {
                    this.lblSortCorlorLevel21.Text = "分光1:" + t2.SortLevel1 + " 分色1:" + t2.ColorLevel1;
                }
                else
                {
                    this.lblSortCorlorLevel21.Text = string.Empty;
                }
                if (t2.ColorLevel2 != null && t2.ColorLevel2 != null)
                {
                    this.lblSortCorlorLevel22.Text = "分光2:" + t2.SortLevel2 + " 分色2:" + t2.ColorLevel2;
                }
                else
                {
                    this.lblSortCorlorLevel22.Text = string.Empty;
                }
                this.resolver.IOType = null;
                this.tabMaterial1.SelectedIndex = 1;
                this.lblMessage.Text = "请确认换料";
                this.btnOrder.Focus();
            }
            this.tbBarCode.Text = string.Empty;
        }

        protected override void gvListDataBind()
        {
            this.gvHuListDataBind();
        }

        protected override void tbBarCode_TextChanged(object sender, EventArgs e)
        {
            this.lblMessage.Visible = true;
        }

        protected override void tbBarCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (((e.KeyData & Keys.KeyCode) == Keys.Escape))
            {
                this.InitialAll();
            }
            base.tbBarCode_KeyUp(sender, e);
        }


    }
}

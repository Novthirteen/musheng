using Sconit_SD.SconitWS;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Sconit_SD
{
    public partial class UCLoadMaterial : Sconit_SD.UCBase
    {
        private Label lblPosition;
        private Label lblItemCode;
        private Label lblSortCorlorLevel1;
        private Label lblSortCorlorLevel2;
        private Label lblItemDescription;

        public UCLoadMaterial(User user, string moduleType)
            : base(user, moduleType)
        {
            InitializeComponent();
            this.btnOrder.Text = "上料";

            #region Layout
            columnSequence.Width = 50;
            columnSequence.NullText = string.Empty;

            columnSortLevel1.Width = 35;
            columnSortLevel1.NullText = string.Empty;
            columnColorLevel1.Width = 35;
            columnColorLevel1.NullText = string.Empty;
            columnSortLevel2.Width = 35;
            columnSortLevel2.NullText = string.Empty;
            columnColorLevel2.Width = 35;
            columnColorLevel2.NullText = string.Empty;

            columnHuId.Width = 100;
            ///Layout
            this.lblPosition = new Label();
            this.lblPosition.ForeColor = System.Drawing.Color.Black;
            this.lblPosition.Location = new System.Drawing.Point(6, 60);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(320, 18);
            this.lblPosition.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));
            this.Controls.Add(this.lblPosition);

            this.lblItemCode = new Label();
            this.lblItemCode.ForeColor = System.Drawing.Color.Black;
            this.lblItemCode.Location = new System.Drawing.Point(6, 80);
            this.lblItemCode.Name = "lblItemCode";
            this.lblItemCode.Size = new System.Drawing.Size(320, 18);
            this.lblItemCode.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));
            this.Controls.Add(this.lblItemCode);

            this.lblItemDescription = new Label();
            this.lblItemDescription.ForeColor = System.Drawing.Color.Black;
            this.lblItemDescription.Location = new System.Drawing.Point(6, 100);
            this.lblItemDescription.Name = "lblItemDescription";
            this.lblItemDescription.Size = new System.Drawing.Size(320, 18);
            this.lblItemDescription.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));
            this.Controls.Add(this.lblItemDescription);


            this.lblSortCorlorLevel1 = new Label();
            this.lblSortCorlorLevel1.ForeColor = System.Drawing.Color.Black;
            this.lblSortCorlorLevel1.Location = new System.Drawing.Point(6, 120);
            this.lblSortCorlorLevel1.Name = "lblSortCorlorLevel1";
            this.lblSortCorlorLevel1.Size = new System.Drawing.Size(300, 18);
            this.lblSortCorlorLevel1.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));
            this.Controls.Add(this.lblSortCorlorLevel1);

            this.lblSortCorlorLevel2 = new Label();
            this.lblSortCorlorLevel2.ForeColor = System.Drawing.Color.Black;
            this.lblSortCorlorLevel2.Location = new System.Drawing.Point(6, 140);
            this.lblSortCorlorLevel2.Name = "lblSortCorlorLevel2";
            this.lblSortCorlorLevel2.Size = new System.Drawing.Size(320, 18);
            this.lblSortCorlorLevel2.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));
            this.Controls.Add(this.lblSortCorlorLevel2);

            this.dgList.Location = new System.Drawing.Point(0, 160);
            this.dgList.Size = new System.Drawing.Size(380, 370);

            #endregion

            #region 测试
            if (false)
            {
                this.lblPosition.Text = "位号:12";
                this.lblSortCorlorLevel1.Text = "分光1:12-12 分色1:45-56";
                this.lblItemCode.Text = "物料:123456789012345678901234567890";
                this.lblItemDescription.Text = "描述:1234567890123456ABCD 描述要长~~~";
                this.lblSortCorlorLevel2.Text = "分光2:12-12 分色2:22-43";
                this.lblMessage.Visible = true;
                this.lblMessage.Text = "测试一下";
                this.tbBarCode.Text = "ORD000026661";
            }
            #endregion
        }

        public override void InitialAll()
        {
            base.InitialAll();
            this.resolver.IOType = BusinessConstants.IO_TYPE_IN;
            this.resolver.Transformers = new Transformer[2];
            this.resolver.Transformers[0] = new Transformer();
            this.resolver.Transformers[1] = new Transformer();
            this.tbBarCode.Focus();
            this.gvHuListDataBind();
        }

        protected override void gvListDataBind()
        {
            this.gvHuListDataBind();
        }

        protected override void gvHuListDataBind()
        {
            base.gvHuListDataBind();
            TransformerDetail[] transformerArray = new TransformerDetail[] { };
            if (isHasOutDetail)
            {
                transformerArray = this.resolver.Transformers[1].TransformerDetails;
            }
            ts.MappingName = transformerArray.GetType().Name;
            this.dgList.DataSource = transformerArray;

            if (this.resolver.Transformers != null
                && this.resolver.Transformers.Length > 0
                && this.resolver.Transformers[0].TransformerDetails != null
                && this.resolver.Transformers[0].TransformerDetails.Length > 0)
            {
                TransformerDetail td = this.resolver.Transformers[0].TransformerDetails[0];
                this.lblPosition.Text = "位号:" + td.Position;
                this.lblItemCode.Text = "物料:" + td.ItemCode;
                if (td.ColorLevel1 != null && td.ColorLevel1 != null)
                {
                    this.lblSortCorlorLevel1.Text = "分光1:" + td.SortLevel1 + " 分色1:" + td.ColorLevel1;
                }
                else
                {
                    this.lblSortCorlorLevel1.Text = string.Empty;
                }
                if (td.ColorLevel2 != null && td.ColorLevel2 != null)
                {
                    this.lblSortCorlorLevel2.Text = "分光2:" + td.SortLevel2 + " 分色2:" + td.ColorLevel2;
                }
                else
                {
                    this.lblSortCorlorLevel2.Text = string.Empty;
                }
                this.lblItemDescription.Text = "描述:" + td.ItemDescription;
            }
        }


        protected override void tbBarCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (((e.KeyData & Keys.KeyCode) == Keys.Escape))
            {
                this.InitialAll();
            }
            base.tbBarCode_KeyUp(sender, e);
        }

        private bool isHasOutDetail
        {
            get
            {
                if (this.resolver.Transformers != null
                 && this.resolver.Transformers.Length == 2
                 && this.resolver.Transformers[1] != null
                 && this.resolver.Transformers[1].TransformerDetails != null
                 && this.resolver.Transformers[1].TransformerDetails.Length > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}

using System.Collections.Generic;
using System.ComponentModel;
using Sconit_CS.SconitWS;

namespace Sconit_CS
{
    public partial class UCReturnMaterial : UCBase
    {
        public UCReturnMaterial(User user, string moduleType)
            : base(user, moduleType)
        {
            InitializeComponent();
            this.btnConfirm.Text = "退料";

            this.gvHuList.Columns["HuSortLevel1"].Visible = true;
            this.gvHuList.Columns["HuSortLevel2"].Visible = true;
            this.gvHuList.Columns["HuColorLevel1"].Visible = true;
            this.gvHuList.Columns["HuColorLevel2"].Visible = true;
            this.gvHuList.Columns["HuCurrentQty"].Visible = true;
        }

        protected override void DataBind()
        {
            this.resolver.Command = BusinessConstants.CS_BIND_VALUE_TRANSFORMERDETAIL;
            base.DataBind();
        }
    }
}

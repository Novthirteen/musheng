using Sconit_SD.SconitWS;
using System.Collections.Generic;

namespace Sconit_SD
{
    public partial class UCReturnMaterial : UCBase
    {
        public UCReturnMaterial(User user, string moduleType)
            : base(user, moduleType)
        {
            InitializeComponent();
            this.btnOrder.Text = "退料";
            this.resolver.IsScanHu = true;

            //columnSequence.Width = 20;
            //columnSequence.NullText = string.Empty;

            columnSortLevel1.Width = 35;
            columnSortLevel1.NullText = string.Empty;

            columnColorLevel1.Width = 35;
            columnColorLevel1.NullText = string.Empty;

            columnSortLevel2.Width = 35;
            columnSortLevel2.NullText = string.Empty;

            columnColorLevel2.Width = 35;
            columnColorLevel2.NullText = string.Empty;

            columnHuId.Width = 100;
        }

        protected override void gvListDataBind()
        {
            this.gvHuListDataBind();
        }
    }
}

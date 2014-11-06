using Sconit_SD.SconitWS;

namespace Sconit_SD
{
    public partial class UCStockTaking : UCBase
    {
        public UCStockTaking(User user, string moduleType)
            : base(user, moduleType)
        {
            InitializeComponent();
            this.btnOrder.Text = "盘点";
        }     
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Entity.Distribution;
using com.Sconit.Entity.Exception;

public partial class Inventory_PrintHu_Asn : ModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void tbAsn_TextChanged(Object sender, EventArgs e)
    {
        try
        {
            if (this.tbAsn.Text.Trim() != string.Empty)
            {
                IList<string> ipNoList = TheHqlMgr.FindAll<string>("select ip.IpNo from InProcessLocation as ip where ip.IpNo = ?", this.tbAsn.Text.Trim());

                if (ipNoList == null || ipNoList.Count == 0 || string.IsNullOrEmpty(ipNoList[0]))
                {
                    this.ShowErrorMessage("InProcessLocation.Error.IpNoExists", new string[] { this.tbAsn.Text.Trim() });
                    this.ucList.Visible = false;
                }
                else
                {
                    this.ucList.InitPageParameter(ipNoList[0]);
                    this.ucList.Visible = true;
                }
            }
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }
}

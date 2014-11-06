using System;
using com.Sconit.Web;
using System.ServiceProcess;

public partial class ManageSconit_LeanEngine_Main : MainModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void ucTabNavigator_lbSingleClick(object sender, EventArgs e)
    {
        this.ucSingle.Visible = true;
        this.ucMulti.Visible = false;
    }

    protected void ucTabNavigator_lbMultiClick(object sender, EventArgs e)
    {
        this.ucSingle.Visible = false;
        this.ucMulti.Visible = true;
    }
}

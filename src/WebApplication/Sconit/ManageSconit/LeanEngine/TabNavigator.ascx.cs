using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class ManageSconit_LeanEngine_TabNavigator : System.Web.UI.UserControl
{
    public event EventHandler lbSingleClick;
    public event EventHandler lbMultiClick;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void lbSingle_Click(object sender, EventArgs e)
    {
        if (lbSingleClick != null)
        {
            lbSingleClick(this, e);
        }

        this.tab_single.Attributes["class"] = "ajax__tab_active";
        this.tab_multi.Attributes["class"] = "ajax__tab_inactive";
    }

    protected void lbMulti_Click(object sender, EventArgs e)
    {
        if (lbMultiClick != null)
        {
            lbMultiClick(this, e);
        }

        this.tab_single.Attributes["class"] = "ajax__tab_inactive";
        this.tab_multi.Attributes["class"] = "ajax__tab_active";
    }

}

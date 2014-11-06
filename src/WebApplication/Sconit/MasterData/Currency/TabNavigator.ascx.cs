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

public partial class MasterData_Currency_TabNavigator : System.Web.UI.UserControl
{
    public event EventHandler lbCurrencyClickEvent;
    public event EventHandler lbCurrencyExClickEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void lbCurrency_Click(object sender, EventArgs e)
    {
        if (lbCurrencyClickEvent != null)
        {
            lbCurrencyClickEvent(this, e);
        }

        this.tab_Currency.Attributes["class"] = "ajax__tab_active";
        this.tab_CurrencyEx.Attributes["class"] = "ajax__tab_inactive";
    }

    protected void lbCurrencyEx_Click(object sender, EventArgs e)
    {
        if (lbCurrencyExClickEvent != null)
        {
            lbCurrencyExClickEvent(this, e);

            this.tab_Currency.Attributes["class"] = "ajax__tab_inactive";
            this.tab_CurrencyEx.Attributes["class"] = "ajax__tab_active";
        }
    }
}

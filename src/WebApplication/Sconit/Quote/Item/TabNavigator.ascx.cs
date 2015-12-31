using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Quote_Item_TabNavigator : System.Web.UI.UserControl
{
    public event EventHandler lbBomClickEvent;
    public event EventHandler lbPriceClickEvent;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void lbBom_Click(object sender, EventArgs e)
    {
        if (lbBomClickEvent != null)
        {
            lbBomClickEvent(this, e);
        }

        this.tab_bom.Attributes["class"] = "ajax__tab_active";
        this.tab_price.Attributes["class"] = "ajax__tab_inactive";
    }

    protected void lbPrice_Click(object sender, EventArgs e)
    {
        if (lbPriceClickEvent != null)
        {
            lbPriceClickEvent(this, e);
        }

        this.tab_bom.Attributes["class"] = "ajax__tab_inactive";
        this.tab_price.Attributes["class"] = "ajax__tab_active";
    }
}
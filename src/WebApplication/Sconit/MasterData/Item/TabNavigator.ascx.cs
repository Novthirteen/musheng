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

public partial class MasterData_Item_TabNavigator : System.Web.UI.UserControl
{

    public event EventHandler lbItemClickEvent;
    public event EventHandler lbItemKitClickEvent;
    public event EventHandler lbItemRefClickEvent;

    public event EventHandler lbItemQuoteClickEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void lbItem_Click(object sender, EventArgs e)
    {
        if (lbItemClickEvent != null)
        {
            lbItemClickEvent(this, e);
        }

        this.tab_Item.Attributes["class"] = "ajax__tab_active";
        this.tab_ItemKit.Attributes["class"] = "ajax__tab_inactive";
        this.tab_ItemRef.Attributes["class"] = "ajax__tab_inactive";
        this.tab_ItemQuote.Attributes["class"] = "ajax__tab_inactive";
    }

    protected void lbItemKit_Click(object sender, EventArgs e)
    {
        if (lbItemKitClickEvent != null)
        {
            lbItemKitClickEvent(this, e);

            this.tab_Item.Attributes["class"] = "ajax__tab_inactive";
            this.tab_ItemKit.Attributes["class"] = "ajax__tab_active";
            this.tab_ItemRef.Attributes["class"] = "ajax__tab_inactive";
            this.tab_ItemQuote.Attributes["class"] = "ajax__tab_inactive";
        }
    }

    protected void lbItemRef_Click(object sender, EventArgs e)
    {
        if (lbItemRefClickEvent != null)
        {
            lbItemRefClickEvent(this, e);

            this.tab_Item.Attributes["class"] = "ajax__tab_inactive";
            this.tab_ItemKit.Attributes["class"] = "ajax__tab_inactive";
            this.tab_ItemRef.Attributes["class"] = "ajax__tab_active";
            this.tab_ItemQuote.Attributes["class"] = "ajax__tab_inactive";
        }
    }

    public void ShowTabKit(bool isShow)
    {
        this.tab_ItemKit.Visible = isShow;
    }

    public void UpdateView()
    {
        lbItem_Click(this, null);
    }

    protected void lbItemQuote_Click(object sender, EventArgs e)
    {
        lbItemQuoteClickEvent(this, e);

        this.tab_Item.Attributes["class"] = "ajax__tab_inactive";
        this.tab_ItemKit.Attributes["class"] = "ajax__tab_inactive";
        this.tab_ItemRef.Attributes["class"] = "ajax__tab_inactive";
        this.tab_ItemQuote.Attributes["class"] = "ajax__tab_active";
    }

}

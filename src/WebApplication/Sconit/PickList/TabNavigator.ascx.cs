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

public partial class Picklist_TabNavigator : System.Web.UI.UserControl
{
    public event EventHandler lbPicklistClickEvent;
    public event EventHandler lbPicklistBatchClickEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void lbPicklist_Click(object sender, EventArgs e)
    {
        if (lbPicklistClickEvent != null)
        {
            lbPicklistClickEvent(this, e);
        }

        this.tab_Picklist.Attributes["class"] = "ajax__tab_active";
        this.tab_Picklist_Batch.Attributes["class"] = "ajax__tab_inactive";
    }

    protected void lbPicklist_Batch_Click(object sender, EventArgs e)
    {
        if (lbPicklistBatchClickEvent != null)
        {
            lbPicklistBatchClickEvent(this, e);
        }

        this.tab_Picklist.Attributes["class"] = "ajax__tab_inactive";
        this.tab_Picklist_Batch.Attributes["class"] = "ajax__tab_active";
    }
}

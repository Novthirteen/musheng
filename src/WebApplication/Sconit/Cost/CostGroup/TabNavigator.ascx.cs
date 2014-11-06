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

public partial class Cost_CostGroup_TabNavigator : System.Web.UI.UserControl
{

    public event EventHandler lbCostGroupClickEvent;
    public event EventHandler lbCostCenterClickEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void lbCostGroup_Click(object sender, EventArgs e)
    {
        if (lbCostGroupClickEvent != null)
        {
            lbCostGroupClickEvent(this, e);
        }

        this.tab_CostGroup.Attributes["class"] = "ajax__tab_active";
        this.tab_CostCenter.Attributes["class"] = "ajax__tab_inactive";
    }

    protected void lbCostCenter_Click(object sender, EventArgs e)
    {
        if (lbCostCenterClickEvent != null)
        {
            lbCostCenterClickEvent(this, e);

            this.tab_CostGroup.Attributes["class"] = "ajax__tab_inactive";
            this.tab_CostCenter.Attributes["class"] = "ajax__tab_active";
        }
    }

    public void UpdateView()
    {
        lbCostGroup_Click(this, null);
    }

}

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
using com.Sconit.Web;
using NHibernate.Expression;
using com.Sconit.Entity;

public partial class MasterData_Currency_Main : MainModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucTabNavigator.lbCurrencyExClickEvent += new System.EventHandler(this.TabCurrencyExClick_Render);
        this.ucTabNavigator.lbCurrencyClickEvent += new System.EventHandler(this.TabCurrencyClick_Render);
        if (!IsPostBack)
        {
            if (this.Action == BusinessConstants.PAGE_LIST_ACTION)
            {
                this.ucCurrency.QuickSearch();
            }
        }
    }

    private void TabCurrencyExClick_Render(object sender, EventArgs e)
    {
        this.ucCurrencyEx.Visible = true;
        this.ucCurrency.Visible = false;
    }

    private void TabCurrencyClick_Render(object sender, EventArgs e)
    {
        this.ucCurrencyEx.Visible = false;
        this.ucCurrency.Visible = true;
    }
    
}

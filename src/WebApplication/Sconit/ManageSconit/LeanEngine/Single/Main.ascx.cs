using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using NHibernate.Expression;

public partial class ManageSconit_LeanEngine_Single_Main : MainModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void ucSearch_btnSearchClick(object sender, EventArgs e)
    {
        this.ucList.SetSearchCriteria((DetachedCriteria)((object[])sender)[0], (DetachedCriteria)((object[])sender)[1]);
        this.ucList.Visible = true;
        this.ucList.UpdateView();
    }

    protected void ucList_lbViewClick(object sender, EventArgs e)
    {
        this.ucSearch.Visible = false;
        this.ucList.Visible = false;
        this.ucOrder.Visible = true;
        this.ucOrder.InitPageParameter((string)sender);
    }

    protected void ucOrder_btnBackClick(object sender, EventArgs e)
    {
        this.ucOrder.Visible = false;
        this.ucSearch.Visible = true;
        this.ucList.Visible = true;
    }
}

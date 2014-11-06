using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Entity.Procurement;
using com.Sconit.Web;
using NHibernate.Expression;
using com.Sconit.Entity.MasterData;

public partial class ManageSconit_LeanEngine_Single_OrderTracer : ModuleBase
{
    public event EventHandler btnBackClick;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.Visible = false;
        if (btnBackClick != null)
        {
            btnBackClick(this, null);
        }
    }

    public void InitPageParameter(IList<OrderTracer> orderTracer)
    {
        this.GV_List.DataSource = orderTracer;
        this.GV_List.DataBind();
    }
}

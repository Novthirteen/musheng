using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Exception;

public partial class Finance_Bill_List : ListModuleBase
{
    public EventHandler ViewEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
        }
    }

    public override void UpdateView()
    {
        this.GV_List.Execute();
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
       
    }

  
    protected void lbtnView_Click(object sender, EventArgs e)
    {
        if (ViewEvent != null)
        {
            decimal orderId = decimal.Parse(((LinkButton)sender).CommandArgument);
            ViewEvent(orderId, null);
        }
    }

   
}

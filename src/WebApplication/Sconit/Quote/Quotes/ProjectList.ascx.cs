using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;

public partial class Quote_Quotes_ProjectList : ListModuleBase
{
    public EventHandler EditEvent;
    public EventHandler ViewEvent;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void lbtnView_Click(object sender, EventArgs e)
    {
        string id = ((LinkButton)sender).CommandArgument;
        ViewEvent(id, e);
    }
    public override void UpdateView()
    {
        this.GV_List.Execute();
    }

    protected void lbtnEdit_Click(object sender, EventArgs e)
    {
        string id = ((LinkButton)sender).CommandArgument;
        EditEvent(id, e);
    }
}
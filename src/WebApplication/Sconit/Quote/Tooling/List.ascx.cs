using com.Sconit.Entity.Quote;
using com.Sconit.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Quote_Tooling_List : ListModuleBase
{
    public EventHandler EditEvent;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public override void UpdateView()
    {
        this.GV_List.Execute();
    }

    protected void lbtnView_Click(object sender, EventArgs e)
    {
        string tlNo = ((LinkButton)sender).CommandArgument;
        EditEvent(tlNo, e);
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        string tlNo = ((LinkButton)sender).CommandArgument;
        IList<Tooling> tlList = TheToolingMgr.GetToolingByTLNo(tlNo);
        if (tlList.Count > 0)
        {
            Tooling tl = tlList[0];
            TheToolingMgr.DeleteTooling(tl);
            ShowSuccessMessage("Quote.Tooling.DeleteSuccess", tl.TL_No);
        }
    }
}
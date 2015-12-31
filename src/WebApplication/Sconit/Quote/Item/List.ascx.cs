using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;

public partial class Quote_Item_List : ListModuleBase
{
    public EventHandler EditEvent;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public override void UpdateView()
    {
        this.GV_List.Execute();
    }

    public void lbtnView_Click(object sender, EventArgs e)
    {
        string id = ((LinkButton)sender).CommandArgument;
        EditEvent(id, e);
    }

    public void lbtnDelete_Click(object sender, EventArgs e)
    {
        string id = ((LinkButton)sender).CommandArgument;
        try
        {
            TheToolingMgr.DeleteQuoteItemById(id);
            ShowSuccessMessage("Common.Business.Result.Delete.Successfully");
            UpdateView();
        }
        catch
        {
            ShowErrorMessage("Common.Business.Result.Delete.Failed");
        }
    }
}
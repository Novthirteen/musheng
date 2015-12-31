using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;

public partial class Quote_Template_List : ListModuleBase
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
        string id = ((LinkButton)sender).CommandArgument;
        EditEvent(id, e);
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        string id = ((LinkButton)sender).CommandArgument;
        try
        {
            TheToolingMgr.DeleteCostCategory(TheToolingMgr.GetCostCategoryById(id)[0]);
            ShowSuccessMessage("Common.Business.Result.Delete.Successfully");
        }
        catch(Exception ex)
        {
            ShowErrorMessage("Common.Business.Result.Delete.Failed");
        }
    }
}
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
using com.Sconit.Service.Ext.MasterData;
using System.IO;

public partial class Cost_CostGroup_List : ListModuleBase
{
    public EventHandler EditEvent;

  

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public override void UpdateView()
    {
        this.GV_List.Execute();
    }

    protected void lbtnEdit_Click(object sender, EventArgs e)
    {
        if (EditEvent != null)
        {
            string code = ((LinkButton)sender).CommandArgument;
            EditEvent(code, e);
        }
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        string code = ((LinkButton)sender).CommandArgument;
        try
        {
           
           
            ShowSuccessMessage("Cost.CostGroup.Delete.Successfully", code);
            UpdateView();
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Cost.CostGroup.Delete.Fail", code);
        }

    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
    }
}

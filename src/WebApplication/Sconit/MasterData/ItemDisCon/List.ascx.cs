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

public partial class MasterData_ItemDisCon_List : ListModuleBase
{
    public EventHandler EditEvent;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public override void UpdateView()
    {
        this.GV_List.Execute();
    }
    
    public void Export()
    {
        this.IsExport = true;
        GV_List.Columns[GV_List.Columns.Count-1].Visible = false;
        this.ExportXLS(GV_List);
    }

    protected void lbtnEdit_Click(object sender, EventArgs e)
    {
        if (EditEvent != null)
        {
            string id  = ((LinkButton)sender).CommandArgument;
            EditEvent(id, e);
        }
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        string id = ((LinkButton)sender).CommandArgument;
        try
        {
            TheItemDiscontinueMgr.DeleteItemDiscontinue(Int32.Parse(id));
            ShowSuccessMessage("MasterData.ItemDiscontinue.ItemDiscontinue.Successfully", id);
            UpdateView();
        }
        catch (Castle.Facilities.NHibernateIntegration.DataException ex)
        {
            ShowErrorMessage("MasterData.ItemDiscontinue.DeleteItemCategory.Fail", id);
        }

    }
}

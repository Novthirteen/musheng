using com.Sconit.Web;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterData_ItemMap_List : ListModuleBase
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
            string item = ((LinkButton)sender).CommandArgument;
            EditEvent(item, e);
        }
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        string id = ((LinkButton)sender).CommandArgument;
        try
        {
            string hql = "delete from ItemMap where Item = @Item";
            TheSqlHelperMgr.Delete(hql, new System.Data.SqlClient.SqlParameter[] { new SqlParameter("@Item", id) });
            ShowSuccessMessage("MasterData.ItemMap.DeleteItemMap.Successfully", id);
            UpdateView();
        }
        catch (Castle.Facilities.NHibernateIntegration.DataException ex)
        {
            ShowErrorMessage("MasterData.ItemMap.DeleteItemMap.Fail", id);
        }

    }
}
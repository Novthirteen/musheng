using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Customize;

public partial class MasterData_ItemPoint_Search : ListModuleBase
{
    public event EventHandler NewEvent;
    public EventHandler EditEvent;
    string Item = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.GV_ItemPoint.DataBind();
        GV_ItemPoint.Visible = true;
        if (GV_ItemPoint.Rows.Count == 0)
        {
            fldGV.Visible = true;
            lblResult.Visible = true;
        }
        else
        {
            fldGV.Visible = true;
            lblResult.Visible = false;
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        if (NewEvent != null)
        {
            NewEvent(sender, e);
        }
    }

    protected void ODS_GV_ItemPoint_OnUpdating(object source, ObjectDataSourceMethodEventArgs e)
    {
        ItemPoint ItemPoint = (ItemPoint)e.InputParameters[0];
        IList<ProductLineFacility> productLineFacility = TheOrderProductionPlanMgr.GetProductLineFacility(ItemPoint.Fact);
        if (productLineFacility.Count > 0)
        {
            ItemPoint.EquipmentTime = ItemPoint.Point * productLineFacility[0].PointTime;
        }
    }

    protected void ODS_GV_ItemPoint_OnDeleting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        ItemPoint ItemPoint = (ItemPoint)e.InputParameters[0];
        Item = ItemPoint.Item;
    }

    protected void ODS_GV_ItemPoint_OnDeleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            ShowSuccessMessage("MasterData.ItemPoint.DeleteItemPoint.Successfully", Item);
        }
        catch (Castle.Facilities.NHibernateIntegration.DataException ex)
        {
            ShowErrorMessage("MasterData.ItemPoint.DeleteItemPoint.Fail", Item);
        }
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
        string Item = ((LinkButton)sender).CommandArgument;
        ItemPoint ItemPoint = new ItemPoint();
        ItemPoint.Item = Item;
        try
        {
            TheOrderProductionPlanMgr.DeleteItemPoint(ItemPoint);
            ShowSuccessMessage("MasterData.ItemPoint.DeleteItemPoint.Successfully", Item);
            UpdateView();
        }
        catch
        {
            ShowErrorMessage("MasterData.ItemPoint.DeleteItemPoint.Fail", Item);
        }
    }

    public override void UpdateView()
    {
        this.GV_ItemPoint.DataBind();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.MRP;

public partial class MRP_Schedule_ProductionPlan_List : ListModuleBase
{
    public event EventHandler BackEvent;
    static IList<ItemPoint> ItemPoint;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void tbItemCode_TextChange(Object sender, EventArgs e)
    {
        tbFlow.Text = "";
        txProductLineFacility.Text = "";
        ItemPoint = TheOrderProductionPlanMgr.GetItemPoint(this.tbItemCode.Text.Trim());
        if (ItemPoint.Count > 0)
        {
            //if (ItemPoint[0].Flow != tbFlow.Text && ItemPoint[0].Fact != txProductLineFacility.Text)
            //{
            //    tbItemCode.Text = "";
            //    ShowWarningMessage("MasterData.ItemPoint.Mismatch");
            //}
            tbFlow.Text = ItemPoint[0].Flow;
            txProductLineFacility.Text = ItemPoint[0].Fact;
        }
        else
        {
            ShowWarningMessage("MasterData.ItemPoint.NoFound");
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void CheckStartTime(object source, ServerValidateEventArgs args)
    { }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        if (this.tbFlow.Text == "" || tbItemCode.Text == "" || tbOrderQty.Text == "" || tbPlanInTime.Text == "" || txProductLineFacility.Text == "" || txtStartTime.Text == "")
        {
            ShowWarningMessage("MasterData.ItemPoint.NotIncomplete");
            return;
        }
        IList<OrderProductionPlan> OrderProductionPlan = TheOrderProductionPlanMgr.GetOrderProductionPlan(null, txProductLineFacility.Text, tbFlow.Text, null, null);
        if (OrderProductionPlan.Count < 1)
        {
            OrderProductionPlan orderProductionPlan = new OrderProductionPlan();
            orderProductionPlan.OrderPlanNo = TheNumberControlMgr.GenerateNumber("ORDP");
            orderProductionPlan.Flow = this.tbFlow.Text.Trim();
            orderProductionPlan.Item = this.tbItemCode.Text.Trim();
            orderProductionPlan.OrderQty = Int32.Parse(this.tbOrderQty.Text.Trim());
            orderProductionPlan.PlanInTime = DateTime.Parse(this.tbPlanInTime.Text);
            orderProductionPlan.StartTime = DateTime.Parse(txtStartTime.Text);
            orderProductionPlan.PlanOrderHours = decimal.Parse(this.tbOrderQty.Text.Trim()) * ItemPoint[0].EquipmentTime / 3600;
            orderProductionPlan.PlanEndTime = ((DateTime)orderProductionPlan.StartTime).AddDays((double)(orderProductionPlan.PlanOrderHours * 60 / 1440));
            orderProductionPlan.WindowTime = DateTime.Now;
            orderProductionPlan.EndTime = orderProductionPlan.PlanEndTime;
            orderProductionPlan.Status = "S";
            orderProductionPlan.CreateUser = this.CurrentUser.Code;
            orderProductionPlan.OrderNum = ItemPoint[0].Point * decimal.Parse(this.tbOrderQty.Text.Trim());
            orderProductionPlan.ProductionLineCode = this.txProductLineFacility.Text.Trim();
            TheOrderProductionPlanMgr.CreatOrderProductionPlan(orderProductionPlan);
            ShowSuccessMessage("MasterData.ItemPoint.CreatSuccess");
        }
        else
        {
            ShowWarningMessage("MasterData.OrderPlan.NoFirst");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Entity;
using com.Sconit.Entity.MRP;
using com.Sconit.Web;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;

public partial class MRP_Schedule_ProductionPlan_New : ListModuleBase
{
    static IList<ItemPoint> ItemPoint;
    public event EventHandler BackEvent;
    static int id;
    static string ONo;

    protected void Page_Load(object sender, EventArgs e)
    {
        //this.tbFlow.ServiceParameter = "string:" + this.CurrentUser.Code + ",bool:false,bool:false,bool:false,bool:true,bool:false,bool:false,string:" + BusinessConstants.PARTY_AUTHRIZE_OPTION_BOTH;
        IList<OrderProductionPlan> opp = TheOrderProductionPlanMgr.GetOrderProductionPlan(null, null, null, null, null);
        if (opp.Count > 0)
        {
            if (opp[opp.Count - 1].EndTime == null)
            {
                txtStartTime.Enabled = true;
            }
            else
            {
                txtStartTime.Enabled = false;
            }
        }
        else
        {
            txtStartTime.Enabled = true;
        }
    }

    public void InitPageParameter(string Id)
    {
        string[] arg = Id.Split(',');
        id = int.Parse(arg[1]);
        ONo = arg[0];
        OrderProductionPlan opp = TheOrderProductionPlanMgr.GetOrderProductionPlan(ONo, null, null, null, null)[0];
        tbFlow.Text = opp.Flow;
        txProductLineFacility.Text = opp.ProductionLineCode;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        if (this.tbFlow.Text == "" || tbItemCode.Text == "" || tbOrderQty.Text == "" || tbPlanInTime.Text == "" || txProductLineFacility.Text == "")
        {
            ShowWarningMessage("MasterData.ItemPoint.NotIncomplete");
            return;
        }

        int row = id;
        IList<OrderProductionPlan> oppList = TheOrderProductionPlanMgr.GetOrderProductionPlan(null, txProductLineFacility.Text, tbFlow.Text, null, null);

        OrderProductionPlan opp = TheOrderProductionPlanMgr.GetOrderProductionPlan(ONo, null, null, null, null)[0];

        OrderProductionPlan orderProductionPlan = new OrderProductionPlan();
        orderProductionPlan.OrderPlanNo = TheNumberControlMgr.GenerateNumber("ORDP");
        orderProductionPlan.Flow = this.tbFlow.Text.Trim();
        orderProductionPlan.Item = this.tbItemCode.Text.Trim();
        orderProductionPlan.OrderQty = Int32.Parse(this.tbOrderQty.Text.Trim());
        orderProductionPlan.PlanInTime = DateTime.Parse(this.tbPlanInTime.Text);
        orderProductionPlan.StartTime = ((DateTime)(opp.EndTime)).AddDays((double)ItemPoint[0].TransferTime / 1440);
        orderProductionPlan.PlanOrderHours = decimal.Parse(this.tbOrderQty.Text.Trim()) * ItemPoint[0].EquipmentTime / 3600;
        orderProductionPlan.PlanEndTime = ((DateTime)orderProductionPlan.StartTime).AddDays((double)(orderProductionPlan.PlanOrderHours * 60 / 1440));
        orderProductionPlan.WindowTime = DateTime.Now;
        orderProductionPlan.EndTime = orderProductionPlan.PlanEndTime;
        orderProductionPlan.Status = "S";
        orderProductionPlan.CreateUser = this.CurrentUser.Code;
        orderProductionPlan.OrderNum = ItemPoint[0].Point * decimal.Parse(this.tbOrderQty.Text.Trim());
        orderProductionPlan.ProductionLineCode = this.txProductLineFacility.Text.Trim();
        TheOrderProductionPlanMgr.CreatOrderProductionPlan(orderProductionPlan);

        //OrderProductionPlan oppNew = TheOrderProductionPlanMgr.GetOrderProductionPlan(orderProductionPlan.OrderPlanNo, null, null, null, null)[0];
        
        for (int i = row + 1; i < oppList.Count; i++)
        {
            ItemPoint itemPointD = TheOrderProductionPlanMgr.GetItemPoint(oppList[i].Item)[0];
            if (i == row + 1)
            {
                oppList[i].StartTime = ((DateTime)orderProductionPlan.EndTime).AddDays((double)itemPointD.TransferTime / 1440);
                oppList[i].PlanEndTime = ((DateTime)oppList[i].StartTime).AddDays((double)oppList[i].PlanOrderHours * 60 / 1400);
                oppList[i].EndTime = oppList[i].PlanEndTime;
            }
            else
            {
                oppList[i].StartTime = ((DateTime)oppList[i - 1].EndTime).AddDays((double)itemPointD.TransferTime / 1440);
                oppList[i].PlanEndTime = ((DateTime)oppList[i].StartTime).AddDays((double)oppList[i].PlanOrderHours * 60 / 1400);
                oppList[i].EndTime = oppList[i].PlanEndTime;
            }
            TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[i]);
        }

        //IList<OrderProductionPlan> oppOne = TheOrderProductionPlanMgr.GetOrderProductionPlanByID(id, txProductLineFacility.Text, tbFlow.Text);
        //IList<OrderProductionPlan> oppTwo = TheOrderProductionPlanMgr.GetOrderProductionPlanByID(id, txProductLineFacility.Text, tbFlow.Text);
        //for (int j = 0; j < oppOne.Count; j++)
        //{
        //    if (j == 0)
        //    {
        //        Exchange(oppOne[j], oppTwo[oppOne.Count - 1]);
        //    }
        //    else
        //    {
        //        Exchange(oppOne[j], oppTwo[j - 1]);
        //    }
        //}

        ShowSuccessMessage("MasterData.OrderPlan.CreatSuccess", orderProductionPlan.OrderPlanNo);
    }

    protected void tbFlow_TextChanged(Object sender, EventArgs e)
    { }

    protected void CheckStartTime(object source, ServerValidateEventArgs args)
    { }

    protected void tbItemCode_TextChange(Object sender, EventArgs e)
    {
        //tbFlow.Text = "";
        //txProductLineFacility.Text = "";
        ItemPoint = TheOrderProductionPlanMgr.GetItemPoint(this.tbItemCode.Text.Trim());
        if (ItemPoint.Count > 0)
        {
            if (ItemPoint[0].Flow != tbFlow.Text && ItemPoint[0].Fact != txProductLineFacility.Text)
            {
                tbItemCode.Text = "";
                ShowWarningMessage("MasterData.ItemPoint.Mismatch");
            }
            //tbFlow.Text = ItemPoint[0].Flow;
            //txProductLineFacility.Text = ItemPoint[0].Fact;
        }
        else
        {
            ShowWarningMessage("MasterData.ItemPoint.NoFound");
        }
    }

    void Exchange(OrderProductionPlan oppOne, OrderProductionPlan oppTwo)
    {
        oppOne.OrderPlanNo = oppTwo.OrderPlanNo;
        oppOne.Flow = oppTwo.Flow;
        oppOne.Item = oppTwo.Item;
        oppOne.ProductionLineCode = oppTwo.ProductionLineCode;
        oppOne.OrderQty = oppTwo.OrderQty;
        oppOne.PlanInTime = oppTwo.PlanInTime;
        oppOne.StartTime = oppTwo.StartTime;
        oppOne.PlanEndTime = oppTwo.PlanEndTime;
        oppOne.ActualEndTime = oppTwo.ActualEndTime;
        oppOne.EndTime = oppTwo.EndTime;
        oppOne.PlanOrderHours = oppTwo.PlanOrderHours;
        oppOne.OrderNum = oppTwo.OrderNum;
        oppOne.Status = oppTwo.Status;
        oppOne.CreateUser = oppTwo.CreateUser;
        oppOne.WindowTime = oppTwo.WindowTime;
        TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppOne);
    }
}
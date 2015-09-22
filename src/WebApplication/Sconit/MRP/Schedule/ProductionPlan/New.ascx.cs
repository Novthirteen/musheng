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
        
    }

    public void InitPageParameter(string orderPlanNo)
    {
        //string[] arg = Id.Split(',');
        //id = int.Parse(arg[1]);
        //ONo = arg[0];
        //OrderProductionPlan opp = TheOrderProductionPlanMgr.GetOrderProductionPlan(ONo, null, null, null, null)[0];
        //tbFlow.Text = opp.Flow;
        //txProductLineFacility.Text = opp.ProductionLineCode;

        ONo = orderPlanNo;
        if(orderPlanNo == "")
        {
            //
            txtStartTime.Enabled = true;
        }
        else
        {
            OrderProductionPlan opp = TheOrderProductionPlanMgr.GetOrderProductionPlan(ONo, null, null, null, null)[0];
            tbFlow.Text = opp.Flow;
            txProductLineFacility.Text = opp.ProductionLineCode;
            txtStartTime.Enabled = false;
        }
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

        //int row = id;
        if (ONo != "")
        {
            IList<OrderProductionPlan> oppList = TheOrderProductionPlanMgr.GetOrderProductionPlan(null, txProductLineFacility.Text, tbFlow.Text, null, null);

            OrderProductionPlan opp = TheOrderProductionPlanMgr.GetOrderProductionPlan(ONo, null, null, null, null)[0];

            OrderProductionPlan orderProductionPlan = new OrderProductionPlan();
            orderProductionPlan.OrderPlanNo = TheNumberControlMgr.GenerateNumber("ORDP");
            orderProductionPlan.Flow = this.tbFlow.Text.Trim();
            orderProductionPlan.Item = this.tbItemCode.Text.Trim();
            orderProductionPlan.OrderQty = Int32.Parse(this.tbOrderQty.Text.Trim());
            orderProductionPlan.PlanInTime = DateTime.Parse(this.tbPlanInTime.Text);
            orderProductionPlan.StartTime = ((DateTime)(opp.PlanEndTime)).AddDays((double)ItemPoint[0].TransferTime / 1440);
            orderProductionPlan.PlanOrderHours = decimal.Parse(this.tbOrderQty.Text.Trim()) * ItemPoint[0].EquipmentTime / 3600;
            orderProductionPlan.PlanEndTime = ((DateTime)orderProductionPlan.StartTime).AddDays((double)(orderProductionPlan.PlanOrderHours * 60 / 1440));
            orderProductionPlan.WindowTime = DateTime.Now;
            orderProductionPlan.EndTime = DateTime.Parse(this.tbPlanInTime.Text);
            orderProductionPlan.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE;
            orderProductionPlan.CreateUser = this.CurrentUser.Code;
            orderProductionPlan.OrderNum = ItemPoint[0].Point * decimal.Parse(this.tbOrderQty.Text.Trim());
            orderProductionPlan.ProductionLineCode = this.txProductLineFacility.Text.Trim();
            TheOrderProductionPlanMgr.CreatOrderProductionPlan(orderProductionPlan);
            if (ONo != oppList[oppList.Count - 1].OrderPlanNo)
            {
                int num = 0;
                foreach (OrderProductionPlan oppNum in oppList)
                {
                    num++;
                    if (ONo == oppNum.OrderPlanNo)
                    {
                        break;
                    }
                }
                for (int i = num; i < oppList.Count; i++)
                {
                    ItemPoint itemPointD = TheOrderProductionPlanMgr.GetItemPoint(oppList[i].Item)[0];
                    if (i == num)
                    {
                        oppList[i].StartTime = ((DateTime)orderProductionPlan.PlanEndTime).AddDays((double)itemPointD.TransferTime / 1440);
                        oppList[i].PlanEndTime = ((DateTime)oppList[i].StartTime).AddDays((double)oppList[i].PlanOrderHours * 60 / 1400);
                        oppList[i].EndTime = oppList[i].PlanEndTime;
                    }
                    else
                    {
                        oppList[i].StartTime = ((DateTime)oppList[i - 1].PlanEndTime).AddDays((double)itemPointD.TransferTime / 1440);
                        oppList[i].PlanEndTime = ((DateTime)oppList[i].StartTime).AddDays((double)oppList[i].PlanOrderHours * 60 / 1400);
                        oppList[i].EndTime = oppList[i].PlanEndTime;
                    }
                    TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[i]);
                }
            }
            ShowSuccessMessage("MasterData.OrderPlan.CreatSuccess", orderProductionPlan.OrderPlanNo);
        }
        else//新的
        {
            IList<OrderProductionPlan> oppList = TheOrderProductionPlanMgr.GetOrderProductionPlan(null, txProductLineFacility.Text, tbFlow.Text, null, null);
            if (oppList.Count == 0)
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
                orderProductionPlan.EndTime = DateTime.Parse(this.tbPlanInTime.Text);
                orderProductionPlan.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE;
                orderProductionPlan.CreateUser = this.CurrentUser.Code;
                orderProductionPlan.OrderNum = ItemPoint[0].Point * decimal.Parse(this.tbOrderQty.Text.Trim());
                orderProductionPlan.ProductionLineCode = this.txProductLineFacility.Text.Trim();
                TheOrderProductionPlanMgr.CreatOrderProductionPlan(orderProductionPlan);
                ShowSuccessMessage("MasterData.OrderPlan.CreatSuccess", orderProductionPlan.OrderPlanNo);
            }
            else
            {
                OrderProductionPlan orderProductionPlan = new OrderProductionPlan();
                orderProductionPlan.OrderPlanNo = TheNumberControlMgr.GenerateNumber("ORDP");
                orderProductionPlan.Flow = this.tbFlow.Text.Trim();
                orderProductionPlan.Item = this.tbItemCode.Text.Trim();
                orderProductionPlan.OrderQty = Int32.Parse(this.tbOrderQty.Text.Trim());
                orderProductionPlan.PlanInTime = DateTime.Parse(this.tbPlanInTime.Text);
                orderProductionPlan.StartTime = ((DateTime)(oppList[oppList.Count - 1].PlanEndTime)).AddDays((double)ItemPoint[0].TransferTime / 1440);
                orderProductionPlan.PlanOrderHours = decimal.Parse(this.tbOrderQty.Text.Trim()) * ItemPoint[0].EquipmentTime / 3600;
                orderProductionPlan.PlanEndTime = ((DateTime)orderProductionPlan.StartTime).AddDays((double)(orderProductionPlan.PlanOrderHours * 60 / 1440));
                orderProductionPlan.WindowTime = DateTime.Now;
                orderProductionPlan.EndTime = DateTime.Parse(this.tbPlanInTime.Text);
                orderProductionPlan.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE;
                orderProductionPlan.CreateUser = this.CurrentUser.Code;
                orderProductionPlan.OrderNum = ItemPoint[0].Point * decimal.Parse(this.tbOrderQty.Text.Trim());
                orderProductionPlan.ProductionLineCode = this.txProductLineFacility.Text.Trim();
                TheOrderProductionPlanMgr.CreatOrderProductionPlan(orderProductionPlan);
                ShowSuccessMessage("MasterData.OrderPlan.CreatSuccess", orderProductionPlan.OrderPlanNo);
            }
        }
        #region ////
        //OrderProductionPlan oppNew = TheOrderProductionPlanMgr.GetOrderProductionPlan(orderProductionPlan.OrderPlanNo, null, null, null, null)[0];
        
        //for (int i = row + 1; i < oppList.Count; i++)
        //{
        //    ItemPoint itemPointD = TheOrderProductionPlanMgr.GetItemPoint(oppList[i].Item)[0];
        //    if (i == row + 1)
        //    {
        //        oppList[i].StartTime = ((DateTime)orderProductionPlan.EndTime).AddDays((double)itemPointD.TransferTime / 1440);
        //        oppList[i].PlanEndTime = ((DateTime)oppList[i].StartTime).AddDays((double)oppList[i].PlanOrderHours * 60 / 1400);
        //        oppList[i].EndTime = oppList[i].PlanEndTime;
        //    }
        //    else
        //    {
        //        oppList[i].StartTime = ((DateTime)oppList[i - 1].EndTime).AddDays((double)itemPointD.TransferTime / 1440);
        //        oppList[i].PlanEndTime = ((DateTime)oppList[i].StartTime).AddDays((double)oppList[i].PlanOrderHours * 60 / 1400);
        //        oppList[i].EndTime = oppList[i].PlanEndTime;
        //    }
        //    TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[i]);
        //}

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
        #endregion

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
            if (ONo != "")
            {
                if (ItemPoint[0].Flow != tbFlow.Text && ItemPoint[0].Fact != txProductLineFacility.Text)
                {
                    tbItemCode.Text = "";
                    ShowWarningMessage("MasterData.ItemPoint.Mismatch");
                }
            }
            else
            {
                tbFlow.Text = ItemPoint[0].Flow;
                txProductLineFacility.Text = ItemPoint[0].Fact;
            }
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
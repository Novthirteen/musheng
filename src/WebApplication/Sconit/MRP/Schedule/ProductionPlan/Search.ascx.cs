using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Entity;
using com.Sconit.Entity.MRP;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using System.Data;
using System.Data.SqlClient;

public partial class MRP_Schedule_ProductionPlan_Search : ListModuleBase
{
    public event EventHandler NewEvent;

    public event EventHandler FirstNewEvent;

    public string ModuleSubType
    {
        get
        {
            return (string)ViewState["ModuleSubType"];
        }
        set
        {
            ViewState["ModuleSubType"] = value;
        }
    }

    public bool NewItem
    {
        get
        {
            return (bool)ViewState["NewItem"];
        }
        set
        {
            ViewState["NewItem"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.tbFlow.ServiceParameter = "string:" + this.CurrentUser.Code + ",bool:false,bool:false,bool:false,bool:true,bool:false,bool:false,string:" + BusinessConstants.PARTY_AUTHRIZE_OPTION_BOTH;
        radBtnDown.Checked = true;
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        if (FirstNewEvent != null)
        {
            FirstNewEvent(sender, e);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (this.tbProductLineFacility.Text.Trim() == string.Empty || this.tbProductLineFacility.Text.Trim() == null)
        {
            ShowWarningMessage("MasterData.OrderPlan.InputFact");
            return;
        }
        this.DoSearch();
    }

    public void DoSearch()
    {
        IList<OrderProductionPlan> rderProductionPlanList = TheOrderProductionPlanMgr.GetOrderProductionPlan("", this.tbProductLineFacility.Text.Trim(), this.tbFlow.Text.Trim(), "", "");

        this.GV_List.DataSource = rderProductionPlanList;
        this.GV_List.DataBind();
        this.fld_Details.Visible = true;
        this.ltl_Result.Visible = rderProductionPlanList.Count == 0;
    }

    protected void edit(object sender, GridViewEditEventArgs e)
    {
        GV_List.EditIndex = e.NewEditIndex;
        DoSearch();
    }

    protected void cancel(object sender, GridViewCancelEditEventArgs e)
    {
        GV_List.EditIndex = -1;
        DoSearch();
    }

    protected void update(object sender, GridViewUpdateEventArgs e)
    {
        string OrderPlanNo = ((Literal)GV_List.Rows[e.RowIndex].FindControl("lblOrderPlanNo")).Text;
        OrderProductionPlan OrderProductionPlan = TheOrderProductionPlanMgr.GetOrderProductionPlan(OrderPlanNo, null, null, null, null)[0];
        string Status = ((TextBox)GV_List.Rows[e.RowIndex].FindControl("txtStatus")).Text;
        string ActualEndTime = ((TextBox)GV_List.Rows[e.RowIndex].FindControl("txtActualEndTime")).Text;
        string OrderQty = ((TextBox)GV_List.Rows[e.RowIndex].FindControl("txtOrderQty")).Text;
        string ItemCode = ((Literal)GV_List.Rows[e.RowIndex].FindControl("lblItem")).Text;
        DateTime StartTime = DateTime.Parse(((TextBox)GV_List.Rows[e.RowIndex].FindControl("txtStartTime")).Text);

        DateTime OldPlanEndTime = (DateTime)OrderProductionPlan.PlanEndTime;
        OrderProductionPlan.Status = Status;
        if (ActualEndTime != null && ActualEndTime != string.Empty)
        {
            OrderProductionPlan.ActualEndTime = DateTime.Parse(ActualEndTime);
        }

        OrderProductionPlan.StartTime = StartTime;
        OrderProductionPlan.OrderQty = int.Parse(OrderQty);
        ItemPoint ItemPoint = TheOrderProductionPlanMgr.GetItemPoint(ItemCode)[0];
        OrderProductionPlan.PlanOrderHours = decimal.Parse(OrderQty) * ItemPoint.EquipmentTime / 3600;
        OrderProductionPlan.PlanEndTime = ((DateTime)OrderProductionPlan.StartTime).AddDays((double)(OrderProductionPlan.PlanOrderHours * 60 / 1440));
        if (ActualEndTime != null && ActualEndTime != string.Empty)
        {
            OrderProductionPlan.EndTime = OrderProductionPlan.ActualEndTime;
        }
        else
        {
            OrderProductionPlan.EndTime = OrderProductionPlan.PlanEndTime;
        }
        OrderProductionPlan.OrderNum = ItemPoint.Point * decimal.Parse(OrderQty);

        DateTime NewPlanEndTime = (DateTime)OrderProductionPlan.PlanEndTime;
        TheOrderProductionPlanMgr.UpdateOrderProductionPlan(OrderProductionPlan);

        /////////////////////
        int row = int.Parse(((Literal)GV_List.Rows[e.RowIndex].FindControl("lbId")).Text);
        IList<OrderProductionPlan> oppList = TheOrderProductionPlanMgr.GetOrderProductionPlan(null, OrderProductionPlan.ProductionLineCode, OrderProductionPlan.Flow,null,null);
        for (int i = row; i < oppList.Count; i++)
        {
            if(i == row)
            {
                ItemPoint ipD = TheOrderProductionPlanMgr.GetItemPoint(oppList[i].Item)[0];
                oppList[i].StartTime = ((DateTime)OrderProductionPlan.EndTime).AddDays((double)ipD.TransferTime / 1440);
                oppList[i].PlanEndTime = ((DateTime)oppList[i].StartTime).AddDays((double)oppList[i].PlanOrderHours * 60 / 1400);
                oppList[i].EndTime = oppList[i].PlanEndTime;
                TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[i]);
            }
            else
            {
                ItemPoint ipD = TheOrderProductionPlanMgr.GetItemPoint(oppList[i].Item)[0];
                oppList[i].StartTime = ((DateTime)oppList[i - 1].EndTime).AddDays((double)ipD.TransferTime / 1440);
                oppList[i].PlanEndTime = ((DateTime)oppList[i].StartTime).AddDays((double)oppList[i].PlanOrderHours * 60 / 1400);
                oppList[i].EndTime = oppList[i].PlanEndTime;
                TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[i]);
            }
        }

            //IList<OrderProductionPlan> opp = TheOrderProductionPlanMgr.GetOrderProductionPlanByID(OrderProductionPlan.Id, OrderProductionPlan.ProductionLineCode, OrderProductionPlan.Flow);
            //TimeSpan ts = new TimeSpan(NewPlanEndTime.Ticks - OldPlanEndTime.Ticks);
            //double day = (double)ts.Days + (double)ts.Hours / 24 + (double)ts.Minutes / 1440;
            //for (int i = 0; i < opp.Count; i++)
            //{
            //    opp[i].StartTime = ((DateTime)opp[i].StartTime).AddDays(day);
            //    opp[i].PlanEndTime = ((DateTime)opp[i].PlanEndTime).AddDays(day);

            //    opp[i].EndTime = opp[i].PlanEndTime;
            //    TheOrderProductionPlanMgr.UpdateOrderProductionPlan(opp[i]);
            //}

            GV_List.EditIndex = -1;
        DoSearch();
    }

    protected void delete(object sender, GridViewDeleteEventArgs e)
    {
        string OrderPlanNo = ((Literal)GV_List.Rows[e.RowIndex].FindControl("lblOrderPlanNo")).Text;
        TheOrderProductionPlanMgr.DeleteOrderProductionPlan(OrderPlanNo);
        DoSearch();
    }

    OrderProductionPlan updateOrderPlan(DateTime endTime, int transferTime, string orderPlanNo)
    {
        OrderProductionPlan OrderProductionPlan = TheOrderProductionPlanMgr.GetOrderProductionPlan(orderPlanNo, null, null, null, null)[0];
        OrderProductionPlan.StartTime = endTime.AddDays(transferTime / 1440);
        OrderProductionPlan.PlanEndTime = ((DateTime)OrderProductionPlan.StartTime).AddDays(transferTime / 1440);
        OrderProductionPlan.EndTime = OrderProductionPlan.PlanEndTime;
        return OrderProductionPlan;
    }

    #region /////
    //protected void lbtnMove_Click(object sender, EventArgs e)
    //{
    //    string OrderPlanNo = ((LinkButton)sender).CommandArgument;
    //    if (IsNum(hfMoveNum.Value))
    //    {
    //        IList<OrderProductionPlan> rderProductionPlanList = TheOrderProductionPlanMgr.GetOrderProductionPlan("", this.tbProductLineFacility.Text.Trim(), this.tbFlow.Text.Trim(), "", "");
    //        bool IsContainId = false;
    //        for (int i = 0; i < rderProductionPlanList.Count; i++)
    //        {
    //            if (rderProductionPlanList[i].Id == int.Parse(hfMoveNum.Value))
    //            {
    //                IsContainId = true;
    //            }
    //        }
    //        //int UpCount = TheOrderProductionPlanMgr.GetUpOrderProductionPlanByID(oppNew.Id, oppNew.ProductionLineCode, oppNew.Flow).Count;
    //        if (IsContainId)
    //        {
    //            OrderProductionPlan oppNew = TheOrderProductionPlanMgr.GetOrderProductionPlan(OrderPlanNo, null, null, null, null)[0];
    //            OrderProductionPlan oppOld = TheOrderProductionPlanMgr.GetOrderProductionPlanByID(int.Parse(hfMoveNum.Value))[0];
    //            if (oppNew.Id < oppOld.Id)
    //            {
    //                //OrderProductionPlan oppTemp = new OrderProductionPlan();
    //                //oppTemp = oppOld;
    //                //oppOld = oppNew;
    //                //oppNew = oppOld;
    //                int OldId = oppOld.Id;
    //                oppOld = TheOrderProductionPlanMgr.GetOrderProductionPlanByID(oppNew.Id)[0];
    //                oppNew = TheOrderProductionPlanMgr.GetOrderProductionPlanByID(OldId)[0];
    //            }
    //            OrderProductionPlan opp = TheOrderProductionPlanMgr.GetOrderProductionPlanByID(oppOld.Id)[0];
    //            //DataSet ds = TheSqlHelperMgr.GetDatasetBySql("select * from(SELECT *,ROW_NUMBER() OVER(ORDER BY id DESC) NUM FROM OrderProductionPlan where Flow = '" + oppNew.Flow + "'and ProductionLineCode = '" + oppNew.ProductionLineCode + "' and id < " + oppNew.Id + ") A where A.num = " + hfMoveNum.Value + "");
    //            //OrderProductionPlan oppOld = DataSetToEntity(ds);

    //            decimal? day = oppNew.PlanOrderHours - oppOld.PlanOrderHours;

    //            if (oppNew.Id > oppOld.Id)
    //            {
    //                oppOld.OrderPlanNo = oppNew.OrderPlanNo;
    //                oppOld.Status = oppNew.Status;
    //                oppOld.Item = oppNew.Item;
    //                oppOld.OrderQty = oppNew.OrderQty;
    //                oppOld.PlanInTime = oppNew.PlanInTime;
    //                oppOld.PlanEndTime = ((DateTime)oppOld.StartTime).AddDays((double)oppNew.PlanOrderHours * 60 / 1440);
    //                oppOld.EndTime = oppOld.PlanEndTime;
    //                oppOld.PlanOrderHours = oppNew.PlanOrderHours;
    //                oppOld.OrderNum = oppNew.OrderNum;
    //                TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppOld);

    //                IList<OrderProductionPlan> oppBetween = TheOrderProductionPlanMgr.GetOrderProductionPlanByBetween(oppNew.Id, oppOld.Id, oppOld.ProductionLineCode, oppOld.Flow);
    //                if (oppBetween.Count > 0)
    //                {
    //                    for (int j = 0; j < oppBetween.Count; j++)
    //                    {
    //                        ((DateTime)oppBetween[j].StartTime).AddDays((double)day * 60 / 1440);
    //                        ((DateTime)oppBetween[j].PlanEndTime).AddDays((double)day * 60 / 1440);
    //                        oppBetween[j].EndTime = oppBetween[j].PlanEndTime;
    //                        TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppBetween[j]);
    //                    }
    //                }

    //                ItemPoint itemPoint = TheOrderProductionPlanMgr.GetItemPoint(opp.Item)[0];
    //                oppNew.OrderPlanNo = opp.OrderPlanNo;
    //                oppNew.Status = opp.Status;
    //                oppNew.Item = opp.Item;
    //                oppNew.OrderQty = opp.OrderQty;
    //                oppNew.PlanInTime = opp.PlanInTime;
    //                oppNew.StartTime = ((DateTime)oppBetween[oppBetween.Count - 1].EndTime).AddDays((double)itemPoint.TransferTime / 1440);
    //                oppNew.PlanEndTime = ((DateTime)oppNew.StartTime).AddDays((double)opp.PlanOrderHours * 60 / 1440);
    //                oppNew.EndTime = oppNew.PlanEndTime;
    //                oppNew.PlanOrderHours = opp.PlanOrderHours;
    //                oppNew.OrderNum = opp.OrderNum;
    //                TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppNew);

    //                IList<OrderProductionPlan> oppDown = TheOrderProductionPlanMgr.GetOrderProductionPlanByID(oppNew.Id, oppNew.ProductionLineCode, oppNew.Flow);
    //                if (oppDown.Count > 0)
    //                {
    //                    for (int n = 0; n < oppDown.Count; n++)
    //                    {
    //                        ItemPoint itemPointD = TheOrderProductionPlanMgr.GetItemPoint(oppDown[n].Item)[0];
    //                        if (n == 0)
    //                        {
    //                            oppDown[n].StartTime = ((DateTime)oppNew.EndTime).AddDays((double)itemPointD.TransferTime / 1440);
    //                            oppDown[n].PlanEndTime = ((DateTime)oppDown[n].StartTime).AddDays((double)oppDown[n].PlanOrderHours * 60 / 1400);
    //                            oppDown[n].EndTime = oppDown[n].PlanEndTime;
    //                        }
    //                        else
    //                        {
    //                            oppDown[n].StartTime = ((DateTime)oppDown[n - 1].EndTime).AddDays((double)itemPointD.TransferTime / 1440);
    //                            oppDown[n].PlanEndTime = ((DateTime)oppDown[n].StartTime).AddDays((double)oppDown[n].PlanOrderHours * 60 / 1400);
    //                            oppDown[n].EndTime = oppDown[n].PlanEndTime;
    //                        }
    //                        TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppDown[n]);
    //                    }
    //                }
    //            }
    //            DoSearch();
    //        }
    //        else
    //        {
    //            ShowWarningMessage("MasterData.OrderPlan.OutOfRange");
    //            return;
    //        }
    //    }
    //    else
    //    {
    //        ShowWarningMessage("Common.Validator.Valid.Number");
    //        return;
    //    }

    //}
    #endregion

    protected void lbtnAdd_Click(object sender, EventArgs e)
    {
        if (NewEvent != null)
        {
            string Id = ((LinkButton)sender).CommandArgument;
            NewEvent(Id, e);
        }
    }

    bool IsNum(string num)
    {
        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[0-9]\d*$");
        return reg.IsMatch(num);
    }

    OrderProductionPlan DataSetToEntity(DataSet ds)
    {
        DataRow dr = ds.Tables[0].Rows[0];
        OrderProductionPlan OrderProductionPlan = new OrderProductionPlan();
        OrderProductionPlan.Id = int.Parse(dr["Id"].ToString());
        OrderProductionPlan.OrderPlanNo = dr["OrderPlanNo"].ToString();
        OrderProductionPlan.Flow = dr["Flow"].ToString();
        OrderProductionPlan.Item = dr["Item"].ToString();
        OrderProductionPlan.ProductionLineCode = dr["ProductionLineCode"].ToString();
        OrderProductionPlan.OrderQty = int.Parse(dr["OrderQty"].ToString());
        OrderProductionPlan.PlanInTime = DateTime.Parse(dr["PlanInTime"].ToString());
        OrderProductionPlan.StartTime = DateTime.Parse(dr["StartTime"].ToString());
        OrderProductionPlan.PlanEndTime = DateTime.Parse(dr["PlanEndTime"].ToString());
        OrderProductionPlan.ActualEndTime = DateTime.Parse(dr["ActualEndTime"].ToString());
        OrderProductionPlan.EndTime = DateTime.Parse(dr["EndTime"].ToString());
        OrderProductionPlan.PlanOrderHours = decimal.Parse(dr["PlanOrderHours"].ToString());
        OrderProductionPlan.OrderNum = decimal.Parse(dr["OrderNum"].ToString());
        OrderProductionPlan.Status = dr["Status"].ToString();
        OrderProductionPlan.CreateUser = dr["CreateUser"].ToString();
        OrderProductionPlan.WindowTime = DateTime.Parse(dr["WindowTime"].ToString());
        return OrderProductionPlan;
    }

    protected void lbtnExchange_Click(object sender, EventArgs e)
    {
        int LineNum = 0;
        string OrderPlanNo = ((LinkButton)sender).CommandArgument;

        if (IsNum(hfMoveNum.Value))
        {
            LineNum = int.Parse(hfMoveNum.Value);
        }
        else
        {
            ShowWarningMessage("Common.Validator.Valid.Number");
            return;
        }

        OrderProductionPlan oppClick = TheOrderProductionPlanMgr.GetOrderProductionPlan(OrderPlanNo, null, null, null, null)[0];
        IList<OrderProductionPlan> oppList = TheOrderProductionPlanMgr.GetOrderProductionPlan(null, oppClick.ProductionLineCode, oppClick.Flow, null, null);
        ItemPoint ip = TheOrderProductionPlanMgr.GetItemPoint(oppClick.Item)[0];
        int row = ((GridViewRow)((LinkButton)sender).NamingContainer).RowIndex;

        if (LineNum < 1 && LineNum > oppList.Count)
        {
            ShowWarningMessage("MasterData.OrderPlan.OutOfRange");
            return;
        }
        //double day = (double)ip.TransferTime / 1440 + (double)oppClick.PlanOrderHours * 60 / 1400;
        if (radBtnUp.Checked)
        {
            LineNum = LineNum - 1;
        }

        if (LineNum == 0)
        {
            oppClick.StartTime = oppList[0].StartTime;
            oppClick.PlanEndTime = ((DateTime)oppClick.StartTime).AddDays((double)oppClick.PlanOrderHours * 60 / 1400);
            oppClick.EndTime = oppClick.PlanEndTime;
            TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppClick);

            for (int i = 0; i < row - 1; i++)
            {
                if (i == 0)
                {
                    ItemPoint ipD = TheOrderProductionPlanMgr.GetItemPoint(oppList[i].Item)[0];
                    oppList[i].StartTime = ((DateTime)oppList[i].EndTime).AddDays((double)ipD.TransferTime / 1440);
                    oppList[i].PlanEndTime = ((DateTime)oppList[i].StartTime).AddDays((double)oppList[i].PlanOrderHours * 60 / 1400);
                    oppList[i].EndTime = oppList[i].PlanEndTime;
                    TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[i]);
                }
                else
                {
                    ItemPoint ipD = TheOrderProductionPlanMgr.GetItemPoint(oppList[i].Item)[0];
                    oppList[i].StartTime = ((DateTime)oppList[i - 1].EndTime).AddDays((double)ipD.TransferTime / 1440);
                    oppList[i].PlanEndTime = ((DateTime)oppList[i].StartTime).AddDays((double)oppList[i].PlanOrderHours * 60 / 1400);
                    oppList[i].EndTime = oppList[i].PlanEndTime;
                    TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[i]);
                }
            }

            for (int j = row + 1; j < oppList.Count; j++)
            {
                if (j == row + 1)
                {
                    ItemPoint ipD = TheOrderProductionPlanMgr.GetItemPoint(oppList[j].Item)[0];
                    oppList[j].StartTime = ((DateTime)oppList[row - 1].EndTime).AddDays((double)ipD.TransferTime / 1440);
                    oppList[j].PlanEndTime = ((DateTime)oppList[j].StartTime).AddDays((double)oppList[j].PlanOrderHours * 60 / 1400);
                    oppList[j].EndTime = oppList[j].PlanEndTime;
                    TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[j]);
                }
                else
                {
                    ItemPoint ipD = TheOrderProductionPlanMgr.GetItemPoint(oppList[j].Item)[0];
                    oppList[j].StartTime = ((DateTime)oppList[j - 1].EndTime).AddDays((double)ipD.TransferTime / 1440);
                    oppList[j].PlanEndTime = ((DateTime)oppList[j].StartTime).AddDays((double)oppList[j].PlanOrderHours * 60 / 1400);
                    oppList[j].EndTime = oppList[j].PlanEndTime;
                    TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[j]);
                }
            }
        }
        else
        {

            //if (radBtnDown.Checked)
            //{
            if (row + 1 > LineNum)
            {
                if (row == LineNum)
                {
                    ShowWarningMessage("MasterData.OrderPlan.NoMove");
                    return;
                }
                oppClick.StartTime = ((DateTime)oppList[LineNum - 1].EndTime).AddDays((double)ip.TransferTime / 1440);
                oppClick.PlanEndTime = ((DateTime)oppClick.StartTime).AddDays((double)oppClick.PlanOrderHours * 60 / 1400);
                oppClick.EndTime = oppClick.PlanEndTime;
                TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppClick);

                for (int i = LineNum; i < row; i++)
                {
                    if (i == LineNum)
                    {
                        ItemPoint ipB = TheOrderProductionPlanMgr.GetItemPoint(oppClick.Item)[0];
                        oppList[i].StartTime = ((DateTime)oppClick.EndTime).AddDays((double)ipB.TransferTime / 1440);
                        oppList[i].PlanEndTime = ((DateTime)oppList[i].StartTime).AddDays((double)oppList[i].PlanOrderHours * 60 / 1400);
                        oppList[i].EndTime = oppList[i].PlanEndTime;
                        TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[i]);
                    }
                    else
                    {
                        ItemPoint ipD = TheOrderProductionPlanMgr.GetItemPoint(oppList[i].Item)[0];
                        oppList[i].StartTime = ((DateTime)oppList[i - 1].EndTime).AddDays((double)ipD.TransferTime / 1440);
                        oppList[i].PlanEndTime = ((DateTime)oppList[i].StartTime).AddDays((double)oppList[i].PlanOrderHours * 60 / 1400);
                        oppList[i].EndTime = oppList[i].PlanEndTime;
                        TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[i]);
                    }
                }

                for (int j = row + 1; j < oppList.Count; j++)
                {
                    if (j == row + 1)
                    {
                        ItemPoint ipD = TheOrderProductionPlanMgr.GetItemPoint(oppList[j].Item)[0];
                        oppList[j].StartTime = ((DateTime)oppList[row - 1].EndTime).AddDays((double)ipD.TransferTime / 1440);
                        oppList[j].PlanEndTime = ((DateTime)oppList[j].StartTime).AddDays((double)oppList[j].PlanOrderHours * 60 / 1400);
                        oppList[j].EndTime = oppList[j].PlanEndTime;
                        TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[j]);
                    }
                    else
                    {
                        ItemPoint ipD = TheOrderProductionPlanMgr.GetItemPoint(oppList[j].Item)[0];
                        oppList[j].StartTime = ((DateTime)oppList[j - 1].EndTime).AddDays((double)ipD.TransferTime / 1440);
                        oppList[j].PlanEndTime = ((DateTime)oppList[j].StartTime).AddDays((double)oppList[j].PlanOrderHours * 60 / 1400);
                        oppList[j].EndTime = oppList[j].PlanEndTime;
                        TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[j]);
                    }
                }
            }
            else if (row + 1 < LineNum)
            {
                if (row == 0)
                {
                    ItemPoint ipD = TheOrderProductionPlanMgr.GetItemPoint(oppList[1].Item)[0];
                    oppList[1].StartTime = oppList[0].StartTime;
                    oppList[1].EndTime = ((DateTime)oppList[1].StartTime).AddDays((double)oppList[1].PlanOrderHours * 60 / 1400);
                    oppList[1].EndTime = oppList[1].PlanEndTime;
                    TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[1]);

                    for (int i = 2; i < LineNum; i++)
                    {
                        ItemPoint ipB = TheOrderProductionPlanMgr.GetItemPoint(oppList[i].Item)[0];
                        oppList[i].StartTime = ((DateTime)oppList[i - 1].EndTime).AddDays((double)ipB.TransferTime / 1440);
                        oppList[i].PlanEndTime = ((DateTime)oppList[i].StartTime).AddDays((double)oppList[i].PlanOrderHours * 60 / 1400);
                        oppList[i].EndTime = oppList[i].PlanEndTime;
                        TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[i]);
                    }

                    oppClick.StartTime = ((DateTime)oppList[LineNum - 1].EndTime).AddDays((double)ip.TransferTime / 1440);
                    oppClick.PlanEndTime = ((DateTime)oppClick.StartTime).AddDays((double)oppClick.PlanOrderHours * 60 / 1400);
                    oppClick.EndTime = oppClick.PlanEndTime;
                    TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppClick);

                    for (int j = LineNum; j < oppList.Count; j++)
                    {
                        if (j == LineNum)
                        {
                            ItemPoint ipBD = TheOrderProductionPlanMgr.GetItemPoint(oppList[j].Item)[0];
                            oppList[j].StartTime = ((DateTime)oppClick.EndTime).AddDays((double)ipBD.TransferTime / 1440);
                            oppList[j].PlanEndTime = ((DateTime)oppList[j].StartTime).AddDays((double)oppList[j].PlanOrderHours * 60 / 1400);
                            oppList[j].EndTime = oppList[j].PlanEndTime;
                            TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[j]);
                        }
                        else
                        {
                            ItemPoint ipBD = TheOrderProductionPlanMgr.GetItemPoint(oppList[j].Item)[0];
                            oppList[j].StartTime = ((DateTime)oppList[j - 1].EndTime).AddDays((double)ipBD.TransferTime / 1440);
                            oppList[j].PlanEndTime = ((DateTime)oppList[j].StartTime).AddDays((double)oppList[j].PlanOrderHours * 60 / 1400);
                            oppList[j].EndTime = oppList[j].PlanEndTime;
                            TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[j]);
                        }
                    }
                }
                else
                {
                    for (int i = row + 1; i < LineNum; i++)
                    {
                        if (i == row + 1)
                        {
                            ItemPoint ipD = TheOrderProductionPlanMgr.GetItemPoint(oppList[i].Item)[0];
                            oppList[i].StartTime = ((DateTime)oppList[row - 1].EndTime).AddDays((double)ipD.TransferTime / 1440);
                            oppList[i].PlanEndTime = ((DateTime)oppList[i].StartTime).AddDays((double)oppList[i].PlanOrderHours * 60 / 1400);
                            oppList[i].EndTime = oppList[i].PlanEndTime;
                            TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[i]);
                        }
                        else
                        {
                            ItemPoint ipD = TheOrderProductionPlanMgr.GetItemPoint(oppList[i].Item)[0];
                            oppList[i].StartTime = ((DateTime)oppList[i - 1].EndTime).AddDays((double)ipD.TransferTime / 1440);
                            oppList[i].PlanEndTime = ((DateTime)oppList[i].StartTime).AddDays((double)oppList[i].PlanOrderHours * 60 / 1400);
                            oppList[i].EndTime = oppList[i].PlanEndTime;
                            TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[i]);
                        }
                    }

                    oppClick.StartTime = ((DateTime)oppList[LineNum - 1].EndTime).AddDays((double)ip.TransferTime / 1440);
                    oppClick.PlanEndTime = ((DateTime)oppClick.StartTime).AddDays((double)oppClick.PlanOrderHours * 60 / 1400);
                    oppClick.EndTime = oppClick.PlanEndTime;
                    TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppClick);

                    for (int j = LineNum; j < oppList.Count; j++)
                    {
                        if (j == LineNum)
                        {
                            ItemPoint ipBD = TheOrderProductionPlanMgr.GetItemPoint(oppList[j].Item)[0];
                            oppList[j].StartTime = ((DateTime)oppClick.EndTime).AddDays((double)ipBD.TransferTime / 1440);
                            oppList[j].PlanEndTime = ((DateTime)oppList[j].StartTime).AddDays((double)oppList[j].PlanOrderHours * 60 / 1400);
                            oppList[j].EndTime = oppList[j].PlanEndTime;
                            TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[j]);
                        }
                        else
                        {
                            ItemPoint ipBD = TheOrderProductionPlanMgr.GetItemPoint(oppList[j].Item)[0];
                            oppList[j].StartTime = ((DateTime)oppList[j - 1].EndTime).AddDays((double)ipBD.TransferTime / 1440);
                            oppList[j].PlanEndTime = ((DateTime)oppList[j].StartTime).AddDays((double)oppList[j].PlanOrderHours * 60 / 1400);
                            oppList[j].EndTime = oppList[j].PlanEndTime;
                            TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[j]);
                        }
                    }
                }
            }
        }
        #region ///////
        //else if (radBtnUp.Checked)
        //{
        //    if (LineNum == 1)
        //    {
        //        oppClick.StartTime = oppList[0].StartTime;
        //        oppClick.PlanEndTime = ((DateTime)oppClick.StartTime).AddDays((double)oppClick.PlanOrderHours * 60 / 1400);
        //        oppClick.EndTime = oppClick.PlanEndTime;
        //        TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppClick);

        //        for (int i = 0; i < row - 2; i++)
        //        {
        //            if (i == 0)
        //            {
        //                ItemPoint ipD = TheOrderProductionPlanMgr.GetItemPoint(oppList[i].Item)[0];
        //                oppList[i].StartTime = ((DateTime)oppList[i].EndTime).AddDays((double)ipD.TransferTime / 1440);
        //                oppList[i].PlanEndTime = ((DateTime)oppList[i].StartTime).AddDays((double)oppList[i].PlanOrderHours * 60 / 1400);
        //                oppList[i].EndTime = oppList[i].PlanEndTime;
        //                TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[i]);
        //            }
        //            else 
        //            {
        //                ItemPoint ipD = TheOrderProductionPlanMgr.GetItemPoint(oppList[i].Item)[0];
        //                oppList[i].StartTime = ((DateTime)oppList[i - 1].EndTime).AddDays((double)ipD.TransferTime / 1440);
        //                oppList[i].PlanEndTime = ((DateTime)oppList[i - 1].StartTime).AddDays((double)oppList[i].PlanOrderHours * 60 / 1400);
        //                oppList[i].EndTime = oppList[i].PlanEndTime;
        //                TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppList[i]);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        oppClick.StartTime = ((DateTime)oppList[LineNum - 2].EndTime).AddDays((double)ip.TransferTime / 1440);
        //        oppClick.PlanEndTime = ((DateTime)oppClick.StartTime).AddDays((double)oppClick.PlanOrderHours * 60 / 1400);
        //        oppClick.EndTime = oppClick.PlanEndTime;
        //        TheOrderProductionPlanMgr.UpdateOrderProductionPlan(oppClick);
        //    }

        //}
        #endregion

        DoSearch();
    }
    protected void btnTransToOrder_Click(object sender, EventArgs e)
    {
        ltStock.Visible = false;

        string OrderPlanNo = ((LinkButton)sender).CommandArgument;
        OrderProductionPlan opp = TheOrderProductionPlanMgr.GetOrderProductionPlan(OrderPlanNo, null, null, null, null)[0];

        Flow flow = TheFlowMgr.LoadFlow(opp.Flow);
        DataTable dt = TheSqlHelperMgr.GetDatasetByStoredProcedure("IsLackMaterial", new SqlParameter[] { new SqlParameter("@Location", flow.LocationFrom.Code), new SqlParameter("@Item", opp.Item), new SqlParameter("@InputQty", opp.OrderQty), new SqlParameter("@EffDate", DateTime.Now) }).Tables[0];
        if (dt.Rows.Count > 0)
        {
            string Stock = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                decimal qty = (decimal)dt.Rows[i]["Qty"];
                string item = (string)dt.Rows[i]["Item"];
                if (qty < 0)
                {
                    Stock += item + " : " + qty + "<br/>";
                }
            }
            if (Stock != string.Empty && Stock != "")
            {
                ShowWarningMessage("MasterData.OrderPlan.NoStock");
                ltStock.Visible = true;
                ltStock.Text = "以下物料库存不足:<br/>" + Stock;
                return;
            }
        }

        if (opp.Status != "C")
        {
            //IList<OrderProductionPlan> oppList = TheOrderProductionPlanMgr.GetOrderProductionPlan(null, tbProductLineFacility.Text, tbFlow.Text, null, null);
            //for (int i = 0; i < oppList.Count; i++)
            //{
            Flow currentFlow = TheFlowMgr.LoadFlow(opp.Flow, true);
            OrderHead orderHead = TheOrderMgr.TransferFlow2Order(currentFlow);
            foreach (OrderDetail orderDetail in orderHead.OrderDetails)
            {
                if (orderDetail.Item.Code == opp.Item)
                {
                    orderDetail.RequiredQty = decimal.Parse(opp.OrderQty.ToString());
                    orderDetail.OrderedQty = orderDetail.RequiredQty;
                }
            }
            orderHead.WindowTime = (DateTime)opp.PlanInTime;
            orderHead.StartTime = (DateTime)opp.StartTime;
            orderHead.SubType = BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML;
            orderHead.Priority = BusinessConstants.CODE_MASTER_ORDER_PRIORITY_VALUE_NORMAL;
            orderHead.ProductLineFacility = opp.ProductionLineCode;
            TheOrderMgr.CreateOrder(orderHead, this.CurrentUser);
            //}
            opp.Status = "C";
            TheOrderProductionPlanMgr.UpdateOrderProductionPlan(opp);
            ShowSuccessMessage("MasterData.OrderPlan.TransToOrder.Successfully", orderHead.OrderNo);
        }
        else
        {
            ShowWarningMessage("MasterData.OrderPlan.HaveTransToOrder");
        }
    }
}
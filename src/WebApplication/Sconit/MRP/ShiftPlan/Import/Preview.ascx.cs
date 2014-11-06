using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity;
using com.Sconit.Utility;

public partial class MRP_ShiftPlan_Import_Preview : ModuleBase
{
    public event EventHandler BtnCreateClick;

    public string ModuleType
    {
        get { return (string)ViewState["ModuleType"]; }
        set { ViewState["ModuleType"] = value; }
    }

    #region GridViewRow Control Value
    private MRP_ShiftPlan_Import_PreviewDetail GetDetailControl(GridViewRow gvr)
    {
        return (MRP_ShiftPlan_Import_PreviewDetail)gvr.FindControl("ucDetail");
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public void InitPageParameter(IList<OrderHead> orderHeadList, string ModuleType)
    {
        this.ModuleType = ModuleType;
        this.InitialUI();

        this.GV_List.DataSource = orderHeadList;
        this.GV_List.DataBind();
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            OrderHead oh = (OrderHead)e.Row.DataItem;
            GetDetailControl(e.Row).BindList(oh.OrderDetails);
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.Visible = false;
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        //TheOrderMgr.CreateOrder(this.GetList(), this.CurrentUser.Code);
        if (BtnCreateClick != null)
        {
            BtnCreateClick(this.GetList(), null);
        }
    }

    private IList<OrderHead> GetList()
    {
        bool isQuick = this.cbIsQuick.Checked;
        IList<OrderHead> orderHeadList = new List<OrderHead>();
        foreach (GridViewRow gvr in GV_List.Rows)
        {
            string flowCode = ((Label)gvr.FindControl("lblFlow")).Text;
            DateTime startTime = DateTime.Parse(((TextBox)gvr.FindControl("tbStartTime")).Text);
            DateTime windowTime = DateTime.Parse(((TextBox)gvr.FindControl("tbWindowTime")).Text);

            OrderHead oh = new OrderHead();
            oh = TheOrderMgr.TransferFlow2Order(flowCode);
            oh.Priority = BusinessConstants.CODE_MASTER_ORDER_PRIORITY_VALUE_NORMAL;
            oh.StartTime = startTime;
            oh.WindowTime = windowTime;
            if (isQuick)
            {
                oh.IsAutoRelease = true;
                oh.IsAutoStart = true;
                oh.IsAutoShip = true;
                oh.IsAutoReceive = true;
                oh.StartLatency = 0;
                oh.CompleteLatency = 0;
            }
            this.GetDetailControl(gvr).CollectData(oh);
            OrderHelper.FilterZeroOrderQty(oh);
            orderHeadList.Add(oh);
        }
        return orderHeadList;
    }

    private void InitialUI()
    {
        if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION)
        {
            this.GV_List.Columns[0].HeaderText = TheLanguageMgr.TranslateMessage("MasterData.Flow.Flow.Distribution", this.CurrentUser);
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION)
        {
            this.GV_List.Columns[0].HeaderText = TheLanguageMgr.TranslateMessage("MasterData.Flow.Flow.Production", this.CurrentUser);
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT)
        {
            this.GV_List.Columns[0].HeaderText = TheLanguageMgr.TranslateMessage("MasterData.Flow.Flow.Procurement", this.CurrentUser);
        }
    }
}

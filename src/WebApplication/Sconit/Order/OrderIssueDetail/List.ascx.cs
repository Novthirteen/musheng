using System;
using System.Collections;
using System.Collections.Generic;
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
using com.Sconit.Entity.Exception;
using NHibernate.Expression;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using com.Sconit.Entity.Distribution;
using com.Sconit.Service.Ext.Distribution;


using com.Sconit.Utility;

public partial class Order_OrderIssueDetail_List : ModuleBase
{
    public EventHandler EditEvent;
    public EventHandler CreatePickListEvent;

    public EventHandler ShipSuccessEvent;

    public string ModuleType
    {
        get { return (string)ViewState["ModuleType"]; }
        set { ViewState["ModuleType"] = value; }
    }
    public string ModuleSubType
    {
        get { return (string)ViewState["ModuleSubType"]; }
        set { ViewState["ModuleSubType"] = value; }
    }
    public string OrderType
    {
        get { return (string)ViewState["OrderType"]; }
        set { ViewState["OrderType"] = value; }
    }

    public string FlowCode
    {
        get { return (string)ViewState["FlowCode"]; }
        set { ViewState["FlowCode"] = value; }
    }

    public string ItemCode
    {
        get { return (string)ViewState["ItemCode"]; }
        set { ViewState["ItemCode"] = value; }
    }

    public string StartDate
    {
        get { return (string)ViewState["StartDate"]; }
        set { ViewState["StartDate"] = value; }
    }
    public string EndDate
    {
        get { return (string)ViewState["EndDate"]; }
        set { ViewState["EndDate"] = value; }
    }

    public string OrderSubType
    {
        get { return (string)ViewState["OrderSubType"]; }
        set { ViewState["OrderSubType"] = value; }
    }

    public bool IsFLowChange
    {
        get { return (bool)ViewState["IsFLowChange"]; }
        set { ViewState["IsFLowChange"] = value; }
    }
    public bool IsSupplier
    {
        get { return (bool)ViewState["IsSupplier"]; }
        set { ViewState["IsSupplier"] = value; }
    }

    private decimal GetActedQty(GridViewRow gvr)
    {
        return GetCurrentQtyTextBox(gvr).Text.Trim() != string.Empty ? decimal.Parse(GetCurrentQtyTextBox(gvr).Text.Trim()) : 0;
    }
    private TextBox GetCurrentQtyTextBox(GridViewRow gvr)
    {
        return (TextBox)gvr.FindControl("tbCurrentQty");
    }

    //private string flowCode;
    //private string ItemCode;
    //private string startDate;
    //private string endDate;
    //private string orderSubType;
    //private bool isFLowChange;
    //private bool isSupplier;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    //add by ljz start
    public void GV_ListBindNull()
    {
        IList<OrderHead> orderList = new List<OrderHead>();
        orderNoList = new List<string>();
        orderDetIdList = new List<int>();
        this.GV_List.DataSource = orderList;
        this.GV_List.DataBind();
    }
    //add by ljz end

    public void InitPageParameter(string flowCode, string ItemCode, string startDate, string endDate, string orderSubType, bool isFLowChange, bool showChecked, bool isSupplier)
    {
        this.FlowCode = flowCode;
        this.ItemCode = ItemCode;
        this.StartDate = startDate;
        this.EndDate = endDate;
        this.OrderSubType = orderSubType;
        this.IsFLowChange = isFLowChange;
        this.IsSupplier = isSupplier;

        if (isFLowChange == true)
        {
            orderNoList = new List<string>();
            orderDetIdList = new List<int>();
        }
        this.ModuleSubType = orderSubType;
        
        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(OrderHead));
        if (!string.IsNullOrEmpty(flowCode))
        {
            selectCriteria.Add(Expression.Eq("Flow", flowCode))
                .Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS))
                .Add(Expression.Eq("SubType", this.ModuleSubType))
                .AddOrder(Order.Asc("WindowTime"));
        }
        else
        {
            selectCriteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS))
                .Add(Expression.Eq("SubType", this.ModuleSubType))
                .AddOrder(Order.Asc("WindowTime"));
        }

        if (!string.IsNullOrEmpty(ItemCode))
        {
            selectCriteria.CreateCriteria("OrderDetails").Add(Expression.Eq("Item.Code", ItemCode));
        }

        if (isSupplier == false)
        {
            if (!string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
            {
                selectCriteria.Add(Expression.Ge("WindowTime", DateTime.Parse(startDate)));
            }
            else if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                selectCriteria.Add(Expression.Ge("WindowTime", DateTime.Parse(startDate))).Add(Expression.Le("WindowTime", DateTime.Parse(endDate)));
            }
            else if (string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                selectCriteria.Add(Expression.Le("WindowTime", DateTime.Parse(endDate)));
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
            {
                selectCriteria.Add(Expression.Ge("StartDate", DateTime.Parse(startDate)));
            }
            else if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                selectCriteria.Add(Expression.Ge("StartDate", DateTime.Parse(startDate))).Add(Expression.Le("WindowTime", DateTime.Parse(endDate)));
            }
            else if (string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                selectCriteria.Add(Expression.Le("StartDate", DateTime.Parse(endDate)));
            }
        }

        IList<OrderHead> orderList = new List<OrderHead>(); IList<OrderDetail> orderDetailList = new List<OrderDetail>();
        IList<OrderHead> orderHeadList = TheCriteriaMgr.FindAll<OrderHead>(selectCriteria);
        foreach (OrderHead orderHead in orderHeadList)
        {
            //IList<OrderDetail> orderDetailList = orderHead.OrderDetails;
            if (orderHead.OrderDetails.Count > 0)
            {
                foreach (OrderDetail orderDetail in orderHead.OrderDetails)
                {
                    if (orderDetail.RemainShippedQty > 0)
                    {
                        if (!string.IsNullOrEmpty(ItemCode))
                        {
                            if(orderDetail.Item.Code == ItemCode)
                            {
                                if (showChecked)
                                {
                                    if (orderDetIdList.Contains(orderDetail.Id))
                                    {
                                        orderDetailList.Add(orderDetail);
                                    }
                                }
                                else
                                { 
                                    orderDetailList.Add(orderDetail);
                                }
                            }
                        }
                        else
                        {
                            if (showChecked)
                            {
                                if (orderDetIdList.Contains(orderDetail.Id))
                                {
                                    orderDetailList.Add(orderDetail);
                                }
                            }
                            else
                            {
                                orderDetailList.Add(orderDetail);
                            }
                        }
                    }
                }
            }
        }

        if (orderList.Count > 0)
        {
            this.OrderType = orderList[0].Type;
            this.InitialUI();


        }
        this.GV_List.DataSource = orderDetailList;
        this.GV_List.DataBind();
        LoadCK();
    }

    //add by ljz start
    public void InitPageParameterByItemCode(string ItemCode, string orderSubType)
    {
        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(OrderDetail));
        selectCriteria.Add(Expression.Eq("Item.Code", ItemCode));
        IList<OrderHead> orderList = new List<OrderHead>();IList<OrderDetail> orderDetailList = new List<OrderDetail>();
        IList<OrderDetail> orderHeadList = TheCriteriaMgr.FindAll<OrderDetail>(selectCriteria);

        this.ModuleSubType = orderSubType;
        for (int i = 0; i < orderHeadList.Count; i++)
        {
            DetachedCriteria selectCriteriaOrderHead = DetachedCriteria.For(typeof(OrderHead));
            selectCriteriaOrderHead.Add(Expression.Eq("OrderNo", orderHeadList[i].OrderHead.OrderNo))
                .Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS))
                .Add(Expression.Eq("SubType", this.ModuleSubType))
                .AddOrder(Order.Asc("WindowTime"));
            IList<OrderHead> orderHeadListOrderHead = TheCriteriaMgr.FindAll<OrderHead>(selectCriteriaOrderHead);

            foreach (OrderHead orderHead in orderHeadListOrderHead)
            {
                //IList<OrderDetail> orderDetailList = orderHead.OrderDetails;
                if (orderHead.OrderDetails.Count > 0)
                {
                    foreach (OrderDetail orderDetail in orderHead.OrderDetails)
                    {
                        if (orderDetail.RemainShippedQty > 0)
                        {
                            orderDetailList.Add(orderDetail);
                            //orderList.Add(orderHead);
                            //break;
                        }
                    }
                }
            }
        }
        if (orderList.Count > 0)
        {
            this.OrderType = orderList[0].Type;
            this.InitialUI();
        }
        this.GV_List.DataSource = orderDetailList;
        this.GV_List.DataBind();
        LoadCK();
    }
    //add by ljz end

    private void LoadCK()
    {
        foreach (var orderDetId in orderDetIdList)
        {
            foreach(GridViewRow row in GV_List.Rows)
            {
                if ( int.Parse(((Literal)row.FindControl("ltlId")).Text) == orderDetId )
                {
                    ((CheckBox)row.FindControl("CheckBoxGroup")).Checked = true;
                }
            }
        }
    }

    protected void btnEditShipQty_Click(object sender, EventArgs e)
    {
        if (EditEvent != null)
        {
            //modify by ljz start
            //List<string> orderNoList = this.CollectOrderNoList();
            List<string> orderNoListNew = orderNoList;
            //modify by ljz end
            if (orderNoListNew.Count > 0)
            {
                EditEvent(new Object[] { orderNoListNew,ItemList }, e);
            }
            else
            {
                ShowWarningMessage("Common.Business.Warn.Empty");
            }
        }
        orderNoList = null;
        orderNoList = new List<string>();
        //ljz
        ItemList = null;
        ItemList = new List<List<string>>();

        orderDetIdList = null;
        orderDetIdList = new List<int>();
    }

    protected void btnCreatePickList_Click(object sender, EventArgs e)
    {
        if (CreatePickListEvent != null)
        {
            List<string> orderNoList = this.CollectOrderNoList();
            if (orderNoList.Count > 0)
            {
                CreatePickListEvent(new Object[] { orderNoList }, e);
            }
            else 
            {
                ShowWarningMessage("Common.Business.Warn.Empty");
            }
        }
    }

    protected void btnShip_Click(object sender, EventArgs e)
    {
        try
        {
            this.btnShip.Enabled = false;
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (GridViewRow row in GV_List.Rows)
            {
                if (((CheckBox)row.FindControl("CheckBoxGroup")).Checked == true)
                {
                    var qty = GetActedQty(row);
                    var id = int.Parse(((Literal)row.FindControl("ltlId")).Text);
                    if (qty > 0)
                    {
                        var orderDetail = this.TheOrderDetailMgr.LoadOrderDetail(id);
                        orderDetail.CurrentShipQty = qty;
                        orderDetails.Add(orderDetail);
                    }
                }
            }

            if (orderDetails.Count == 0)
            {
                ShowWarningMessage("Common.Business.Warn.Empty");
                this.btnShip.Enabled = true;
            }
            else
            {
                var inporcess = TheOrderMgr.ShipOrder(orderDetails, CurrentUser.Code);
                this.btnShip.Enabled = true ;
                ShowSuccessMessage("MasterData.Distribution.Ship.Successfully", inporcess.IpNo);
                orderNoList = new List<string>();
                orderDetIdList = new List<int>();
                //if (this.cbPrintAsn.Checked == true)
                //{
                //    PrintASN(inporcess);
                //}
                ShipSuccessEvent(new Object[] { inporcess.IpNo, this.cbPrintAsn.Checked }, e);
            }
        }
        catch (BusinessErrorException ex)
        {
            this.btnShip.Enabled = true;
            ShowErrorMessage(ex);
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        foreach (string orderNo in orderNoList)
        {
            OrderHead orderHead = TheOrderHeadMgr.LoadOrderHead(orderNo);
            string orderTemplate = orderHead.OrderTemplate;
            if (orderTemplate == null || orderTemplate.Length == 0)
            {
                ShowErrorMessage("MasterData.Order.OrderHead.PleaseConfigOrderTemplate");
            }
            else
            {
                //IReportBaseMgr iReportBaseMgr = this.GetIReportBaseMgr(orderTemplate, orderHead);
                string printUrl = TheReportMgr.WriteToFile(orderTemplate, orderNo);
                Page.ClientScript.RegisterStartupScript(GetType(), "method", " <script language='javascript' type='text/javascript'>PrintOrder('" + printUrl + "'); </script>");
            }
        }
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            OrderDetail orderDetail = (OrderDetail)e.Row.DataItem;

            Label lblWinTime = (Label)e.Row.FindControl("lblWinTime");

            lblWinTime.ForeColor = OrderHelper.GetWinTimeColor(orderDetail.OrderHead.StartTime, orderDetail.OrderHead.WindowTime);


        }
    }

    private List<string> CollectOrderNoList()
    {
        List<string> orderNoList = new List<string>();
        foreach (GridViewRow gvr in GV_List.Rows)
        {
            CheckBox cbCheckBoxGroup = (CheckBox)gvr.FindControl("CheckBoxGroup");
            if (cbCheckBoxGroup.Checked)
            {
                string orderNo = ((Literal)gvr.FindControl("ltlOrderNo")).Text;
                if (!orderNoList.Contains(orderNo))
                {
                    orderNoList.Add(orderNo);
                }
            }
        }
        return orderNoList;
    }

    private void InitialUI()
    {
        if (this.OrderType != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
        {
            //this.btnCreatePickList.Visible = true;
        }
        else
        {
            //this.btnCreatePickList.Visible = false;
        }
    }

    static List<string> orderNoList = new List<string>();
    static List<int> orderDetIdList = new List<int>();
    static List<List<string>> ItemList = new List<List<string>>();//ljz
    protected void CheckBoxGroup_CheckedChanged(Object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;

        DataControlFieldCell dcf = (DataControlFieldCell)chk.Parent;    //这个对象的父类为cell  
        GridViewRow gr = (GridViewRow)dcf.Parent;                       //cell对象的父类为row
        string orderNo = ((Literal)gr.FindControl("ltlOrderNo")).Text;
        string item = ((Literal)gr.FindControl("ltlItem")).Text;//ljz
        int orderDetId = int.Parse(((Literal)gr.FindControl("ltlId")).Text);

        List<string> olit = new List<string>();
        List<string> it = new List<string>();
        olit.Add(orderNo);
        olit.Add(item);
        if (chk.Checked)
        {
            if (this.IsSupplier == true)
            {
                var orderHead = this.TheOrderHeadMgr.LoadOrderHead(orderNo);
                if (DateTime.Now.AddMonths(1) < orderHead.WindowTime)
                {
                    ShowErrorMessage("订单未到达发货时间，不允许发货。");
                    chk.Checked = false;
                }
            }
            if (!orderNoList.Contains(orderNo))
            {
                orderNoList.Add(orderNo);
            }
            if (!orderDetIdList.Contains(orderDetId))
            {
                orderDetIdList.Add(orderDetId);
            }
            ItemList.Add(olit);//ljz
            
        }
        else
        {
            orderNoList.Remove(orderNo);
            orderDetIdList.Remove(orderDetId);
            ItemList.Remove(ItemList.FirstOrDefault(i => i[0] == orderNo && i[1] == item));
            //ItemList.Remove(olit);//ljz
        }
    }
    protected void CheckAll_CheckedChanged(Object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;

        if (chk.Checked)
        {
            foreach (GridViewRow gvr in GV_List.Rows)
            {
                CheckBox cbCheckBoxGroup = (CheckBox)gvr.FindControl("CheckBoxGroup");
                string orderNo = ((Literal)gvr.FindControl("ltlOrderNo")).Text;
                int orderDetId = int.Parse(((Literal)gvr.FindControl("ltlId")).Text);
                if (!orderNoList.Contains(orderNo))
                {
                    orderNoList.Add(orderNo);
                }
                if (orderDetIdList.Contains(orderDetId))
                {
                    orderDetIdList.Add(orderDetId);
                }

            }
        }
        else
        {
            foreach (GridViewRow gvr in GV_List.Rows)
            {
                CheckBox cbCheckBoxGroup = (CheckBox)gvr.FindControl("CheckBoxGroup");
                string orderNo = ((Literal)gvr.FindControl("ltlOrderNo")).Text;
                int orderDetId = int.Parse(((Literal)gvr.FindControl("ltlId")).Text);
                orderNoList.Remove(orderNo);
                orderDetIdList.Remove(orderDetId);

            }
        }
    }

    private void PrintASN(InProcessLocation inProcessLocation)
    {
        // inProcessLocation.InProcessLocationDetails = TheInProcessLocationDetailMgr.SummarizeInProcessLocationDetails(inProcessLocation.InProcessLocationDetails);
        if (inProcessLocation.AsnTemplate == null || inProcessLocation.AsnTemplate == string.Empty)
        {
            ShowErrorMessage("ASN.PrintError.NoASNTemplate");
            return;
        }

        IList<object> list = new List<object>();
        list.Add(inProcessLocation);
        list.Add(inProcessLocation.InProcessLocationDetails);

        //报表url
        string asnUrl = TheReportMgr.WriteToFile(inProcessLocation.AsnTemplate, list);
        //客户端打印
        //如果在UpdatePanel中调用JavaScript需要使用 ScriptManager.RegisterClientScriptBlock
        //ScriptManager.RegisterClientScriptBlock(this, GetType(), "method", " <script language='javascript' type='text/javascript'>PrintOrder('" + asnUrl + "'); </script>", false);
        Page.ClientScript.RegisterStartupScript(GetType(), "method", " <script language='javascript' type='text/javascript'>PrintOrder('" + asnUrl + "'); </script>");
    }
    
}

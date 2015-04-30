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

public partial class Distribution_OrderIssue_List : ModuleBase
{
    public EventHandler EditEvent;
    public EventHandler CreatePickListEvent;

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

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    //add by ljz start
    public void GV_ListBindNull()
    {
        IList<OrderHead> orderList = new List<OrderHead>();
        this.GV_List.DataSource = orderList;
        this.GV_List.DataBind();
    }
    //add by ljz end

    public void InitPageParameter(string flowCode, string orderSubType)
    {
        this.ModuleSubType = orderSubType;

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(OrderHead));
        selectCriteria.Add(Expression.Eq("Flow", flowCode))
            .Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS))
            .Add(Expression.Eq("SubType", this.ModuleSubType))
            .AddOrder(Order.Asc("WindowTime"));
        IList<OrderHead> orderList = new List<OrderHead>();
        IList<OrderHead> orderHeadList = TheCriteriaMgr.FindAll<OrderHead>(selectCriteria);
        foreach (OrderHead orderHead in orderHeadList)
        {
            IList<OrderDetail> orderDetailList = orderHead.OrderDetails;
            if (orderHead.OrderDetails.Count > 0)
            {
                foreach (OrderDetail orderDetail in orderHead.OrderDetails)
                {
                    if (orderDetail.RemainShippedQty > 0)
                    {
                        orderList.Add(orderHead);
                        break;
                    }
                }
            }
        }

        if (orderList.Count > 0)
        {
            this.OrderType = orderList[0].Type;
            this.InitialUI();


        }
        this.GV_List.DataSource = orderList;
        this.GV_List.DataBind();
    }

    //add by ljz start
    public void InitPageParameterByItemCode(string ItemCode, string orderSubType)
    {
        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(OrderDetail));
        selectCriteria.Add(Expression.Eq("Item.Code", ItemCode));
        IList<OrderHead> orderList = new List<OrderHead>();
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
                IList<OrderDetail> orderDetailList = orderHead.OrderDetails;
                if (orderHead.OrderDetails.Count > 0)
                {
                    foreach (OrderDetail orderDetail in orderHead.OrderDetails)
                    {
                        if (orderDetail.RemainShippedQty > 0)
                        {
                            orderList.Add(orderHead);
                            break;
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
        this.GV_List.DataSource = orderList;
        this.GV_List.DataBind();
    }
    //add by ljz end

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
                EditEvent(new Object[] { orderNoListNew }, e);
            }
            else
            {
                ShowWarningMessage("Common.Business.Warn.Empty");
            }
        }
        orderNoList = null;
        orderNoList = new List<string>();
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

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            OrderHead orderHead = (OrderHead)e.Row.DataItem;

            Label lblWinTime = (Label)e.Row.FindControl("lblWinTime");
            if (orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE
               || orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT
               || orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)
            {
                lblWinTime.ForeColor = OrderHelper.GetWinTimeColor(orderHead.StartTime, orderHead.WindowTime);
            }

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
                orderNoList.Add(orderNo);
            }
        }
        return orderNoList;
    }

    private void InitialUI()
    {
        if (this.OrderType != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
        {
            this.btnCreatePickList.Visible = true;
        }
        else
        {
            this.btnCreatePickList.Visible = false;
        }
    }

    static List<string> orderNoList = new List<string>();
    protected void CheckBoxGroup_CheckedChanged(Object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;

        DataControlFieldCell dcf = (DataControlFieldCell)chk.Parent;    //这个对象的父类为cell  
        GridViewRow gr = (GridViewRow)dcf.Parent;                       //cell对象的父类为row
        string orderNo = ((Literal)gr.FindControl("ltlOrderNo")).Text;

        if (chk.Checked)
        {
            if (!orderNoList.Contains(orderNo))
            {
                orderNoList.Add(orderNo);
            }
        }
        else
        {
            orderNoList.Remove(orderNo);
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
                orderNoList.Add(orderNo);

            }
        }
        else
        {
            foreach (GridViewRow gvr in GV_List.Rows)
            {
                CheckBox cbCheckBoxGroup = (CheckBox)gvr.FindControl("CheckBoxGroup");
                string orderNo = ((Literal)gvr.FindControl("ltlOrderNo")).Text;
                orderNoList.Remove(orderNo);

            }
        }
    }
    
}

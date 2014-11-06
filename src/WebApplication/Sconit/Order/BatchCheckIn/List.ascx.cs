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

public partial class Order_BatchCheckIn_List : ModuleBase
{
   
    public string ModuleType
    {
        get
        {
            return (string)ViewState["ModuleType"];
        }
        set
        {
            ViewState["ModuleType"] = value;
        }
    }
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


    public DetachedCriteria SelectCriteria
    {
        get
        {
            return (DetachedCriteria)ViewState["SelectCriteria"];
        }
        set
        {
            ViewState["SelectCriteria"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void InitPageParameter(DetachedCriteria selectCriteria)
    {
        this.SelectCriteria = selectCriteria;
        IList<OrderHead> orderHeadList = TheCriteriaMgr.FindAll<OrderHead>(selectCriteria);
        this.GV_List.DataSource = orderHeadList;
        this.GV_List.DataBind();

    }
    protected void btnStart_Click(object sender, EventArgs e)
    {

        IList<string> orderList = GetSelectOrder();
        if (orderList.Count == 0)
        {
            ShowErrorMessage("MasterData.Order.BatchCheckIn.PleaseSelectOrder");  
        }
        else
        {
            try
            {
                foreach (string orderNo in orderList)
                {
                    TheOrderMgr.StartOrder(orderNo, this.CurrentUser.Code);
                }
                ShowSuccessMessage("MasterData.Order.BatchStart.Successfully");
                InitPageParameter(this.SelectCriteria);
            }
            catch (BusinessErrorException ex)
            {
                this.ShowErrorMessage(ex);
            }

        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        IList<string> orderList = GetSelectOrder();
        if (orderList.Count == 0)
        {
            ShowErrorMessage("MasterData.Order.BatchCheckIn.PleaseSelectOrder");
        }
        else
        {
            try
            {
                foreach (string orderNo in orderList)
                {
                    TheOrderMgr.CancelOrder(orderNo, this.CurrentUser.Code);
                }
                ShowSuccessMessage("MasterData.Order.BatchCancel.Successfully");
                InitPageParameter(this.SelectCriteria);
            }
            catch (BusinessErrorException ex)
            {
                this.ShowErrorMessage(ex);
            }

        }
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            OrderHead orderHead = (OrderHead)e.Row.DataItem;
            e.Row.Cells[1].Attributes.Add("onmouseover", "e=this.style.backgroundColor; this.style.backgroundColor=this.style.borderColor");
            e.Row.Cells[1].Attributes.Add("onmouseout", "this.style.backgroundColor=e");
            e.Row.Cells[1].Attributes.Add("title", GetDetail(orderHead));
        }
    }

    private IList<string> GetSelectOrder()
    {
        IList<string> orderNoList = new List<string>();
        for (int i = 0; i < this.GV_List.Rows.Count; i++)
        {
            CheckBox cbOrderNo = (CheckBox)this.GV_List.Rows[i].FindControl("cbOrderNo");

            if (cbOrderNo.Checked == true)
            {
                orderNoList.Add(this.GV_List.DataKeys[i].Value.ToString());
            }
        }
        return orderNoList;
    }


    private string GetDetail(OrderHead orderHead)
    {
        string detail = string.Empty;
        detail += "cssbody=[obbd] cssheader=[obhd] header=[" + orderHead.OrderNo + " | " + (orderHead.Flow == null ? string.Empty : orderHead.Flow) + "] body=[<table width=100%>";
        System.Collections.Generic.IList<OrderDetail> ods = orderHead.OrderDetails;
        foreach (OrderDetail od in ods)
        {
            string ItemCode = od.Item.Code;
            string ItemDesc = od.Item.Description.Replace("[", "&#91;").Replace("]", "&#93;");
            string OrderQty = od.OrderedQty.ToString("0.########");
            string Uom = od.Uom.Code;
            string RecQty = od.ReceivedQty == null ? "0" : od.ReceivedQty.Value.ToString("0.########");
            detail += "<tr><td>" + ItemCode + "</td><td>" + ItemDesc + "</td><td>" + OrderQty + "</td><td>" + Uom + "</td><td>" + RecQty + "</td></tr>";
        }
        detail += "</table>]";
        return detail;
    }
   
}

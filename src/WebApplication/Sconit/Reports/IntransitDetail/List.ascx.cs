﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using NHibernate.Expression;
using com.Sconit.Entity.View;
using com.Sconit.Entity.Exception;
using System.Text;

public partial class Reports_IntransitDetail_List : ListModuleBase
{
    public event EventHandler DetailEvent;

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

    private string FlowCode
    {
        get
        {
            return (string)ViewState["FlowCode"];
        }
        set
        {
            ViewState["FlowCode"] = value;
        }
    }

    public void Export()
    {
        this.IsExport = true;
        this.ExportXLS(GV_List);
    }


    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (this.IsExport && e.Row.RowType == DataControlRowType.DataRow)
        {
            IntransitDetail intransitDetail = (IntransitDetail)e.Row.DataItem;

            this.SetLinkButton(e.Row, "lbtDefaultActivity", new string[] { }, false);
            //e.Row.Cells[1].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
        }
    }

    private void SetLinkButton(GridViewRow gvr, string id, string[] commandArgument, bool enabled)
    {
        LinkButton linkButton = (LinkButton)gvr.FindControl(id);
        linkButton.Enabled = enabled && !IsExport;
        if (enabled)
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < commandArgument.Length; i++)
            {
                if (i > 0)
                    str.Append(",");

                str.Append(commandArgument[i]);
            }
            linkButton.CommandArgument = str.ToString();
        }
    }

    public void InitPageParameter(string[] obj)
    {
        this.FlowCode = obj[0];
        string itemCode = obj[1];
        IList<RoutingDetail> routingDetailList = new List<RoutingDetail>();

        if (FlowCode != null && FlowCode.Trim() != string.Empty)
        {
            try
            {
                Flow flow = this.TheFlowMgr.CheckAndLoadFlow(FlowCode);
                if (flow.Routing != null)
                {
                    routingDetailList = this.TheRoutingDetailMgr.GetRoutingDetail(flow.Routing, DateTime.Now);
                }
            }
            catch (BusinessErrorException ex)
            {
                this.ShowErrorMessage(ex);
                return;
            }
        }

        #region 查询
        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(InProcessLocationDetailTrackView));
        selectCriteria.CreateAlias("OrderDetail", "od");
        selectCriteria.CreateAlias("OrderDetail.Item", "oi");
        selectCriteria.CreateAlias("Flow", "f");

        if (FlowCode != null && FlowCode.Trim() != string.Empty)
            selectCriteria.Add(Expression.Eq("f.Code", FlowCode));
        if (itemCode != null && itemCode.Trim() != string.Empty)
        {
           // selectCriteria.Add(Expression.Eq("oi.Code", itemCode));

            selectCriteria.Add(
               Expression.Like("oi.Code", itemCode, MatchMode.Anywhere) ||
               Expression.Like("oi.Desc1", itemCode, MatchMode.Anywhere) ||
               Expression.Like("oi.Desc2", itemCode, MatchMode.Anywhere)
               );
        }
        selectCriteria.AddOrder(Order.Asc("od.Item"));
        selectCriteria.AddOrder(Order.Asc("od.Uom"));
        selectCriteria.AddOrder(Order.Asc("od.UnitCount"));
        //selectCriteria.AddOrder(Order.Asc("CurrentOperation"));

        IList<InProcessLocationDetailTrackView> inProcessLocationDetailTrackViewList = this.TheCriteriaMgr.FindAll<InProcessLocationDetailTrackView>(selectCriteria);
        #endregion

        #region 转换查询结果为IntransitDetail
        if (inProcessLocationDetailTrackViewList != null && inProcessLocationDetailTrackViewList.Count > 0)
        {
            IntransitDetail intransitDetail = null;
            IList<IntransitDetail> intransitDetailList = new List<IntransitDetail>();
            foreach (InProcessLocationDetailTrackView inProcessLocationDetailTrackView in inProcessLocationDetailTrackViewList)
            {
                intransitDetail = new IntransitDetail();
                intransitDetail.PartyFrom = inProcessLocationDetailTrackView.OrderDetail.OrderHead.PartyFrom.Name;
                intransitDetail.PartyTo = inProcessLocationDetailTrackView.OrderDetail.OrderHead.PartyTo.Name;
                intransitDetail.OrderNo = inProcessLocationDetailTrackView.OrderDetail.OrderHead.OrderNo;
                intransitDetail.ItemCode = inProcessLocationDetailTrackView.OrderDetail.Item.Code;
                intransitDetail.ItemName = inProcessLocationDetailTrackView.OrderDetail.Item.Description;
                intransitDetail.ReferenceItem = inProcessLocationDetailTrackView.OrderDetail.ReferenceItemCode;
                intransitDetail.Uom = inProcessLocationDetailTrackView.OrderDetail.Uom.Code;
                intransitDetail.UnitCount = inProcessLocationDetailTrackView.OrderDetail.UnitCount;
                intransitDetail.DefaultActivity = inProcessLocationDetailTrackView.Qty;
                intransitDetail.IpNo = inProcessLocationDetailTrackView.IpNo;

                intransitDetailList.Add(intransitDetail);

                //switch (FindActvitySeq(routingDetailList, inProcessLocationDetailTrackView.CurrentOperation))
                //{
                //    case 0:
                //        intransitDetail.DefaultActivity += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 1:
                //        intransitDetail.Activity1 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 2:
                //        intransitDetail.Activity2 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 3:
                //        intransitDetail.Activity3 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 4:
                //        intransitDetail.Activity4 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 5:
                //        intransitDetail.Activity5 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 6:
                //        intransitDetail.Activity6 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 7:
                //        intransitDetail.Activity7 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 8:
                //        intransitDetail.Activity8 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 9:
                //        intransitDetail.Activity9 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 10:
                //        intransitDetail.Activity10 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 11:
                //        intransitDetail.Activity11 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 12:
                //        intransitDetail.Activity12 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 13:
                //        intransitDetail.Activity13 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 14:
                //        intransitDetail.Activity14 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 15:
                //        intransitDetail.Activity15 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 16:
                //        intransitDetail.Activity16 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 17:
                //        intransitDetail.Activity17 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 18:
                //        intransitDetail.Activity18 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 19:
                //        intransitDetail.Activity19 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 20:
                //        intransitDetail.Activity20 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //}
            }

            this.lblNoRecordFound.Visible = false;
            this.GV_List.DataSource = intransitDetailList;
            this.GV_List.DataBind();
        }
        else
        {
            this.lblNoRecordFound.Visible = true;
            this.GV_List.DataSource = null;
            this.GV_List.DataBind();
        }
        #endregion

    }

    public override void UpdateView()
    {
        this.IsExport = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void lbtnDetail_Click(object sender, EventArgs e)
    {
        string s = ((LinkButton)sender).CommandArgument;
        string[] array = s.Split('$');
        string itemCode = array[0];
        string uom = array[1];
        decimal unitCount = decimal.Parse(array[2]);
        int position = int.Parse(array[3]);

        DetailEvent(new object[] { this.FlowCode, itemCode, uom, unitCount, position }, e);
    }

    private int FindActvitySeq(IList<RoutingDetail> routingDetailList, int? operation)
    {
        if (!operation.HasValue)
        {
            return 0;
        }

        if (routingDetailList != null && routingDetailList.Count > 0)
        {
            for (int i = 0; i < routingDetailList.Count; i++)
            {
                if (routingDetailList[i].Operation == operation.Value)
                {
                    return i + 1;
                }
            }

            return 0;
        }
        else
        {
            return 0;
        }
    }
}

class IntransitDetail
{
    public string PartyFrom { get; set; }
    public string PartyTo { get; set; }
    public string OrderNo { get; set; }
    public string IpNo { get; set; }
    public string ItemCode { get; set; }
    public string ItemName { get; set; }
    public string ReferenceItem { get; set; }
    public string Uom { get; set; }
    public decimal UnitCount { get; set; }
    public decimal DefaultActivity { get; set; }
    public decimal Activity1 { get; set; }
    public decimal Activity2 { get; set; }
    public decimal Activity3 { get; set; }
    public decimal Activity4 { get; set; }
    public decimal Activity5 { get; set; }
    public decimal Activity6 { get; set; }
    public decimal Activity7 { get; set; }
    public decimal Activity8 { get; set; }
    public decimal Activity9 { get; set; }
    public decimal Activity10 { get; set; }
    public decimal Activity11 { get; set; }
    public decimal Activity12 { get; set; }
    public decimal Activity13 { get; set; }
    public decimal Activity14 { get; set; }
    public decimal Activity15 { get; set; }
    public decimal Activity16 { get; set; }
    public decimal Activity17 { get; set; }
    public decimal Activity18 { get; set; }
    public decimal Activity19 { get; set; }
    public decimal Activity20 { get; set; }
}

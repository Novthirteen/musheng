using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using com.Sconit.Utility;
using com.Sconit.Entity.Procurement;
using com.Sconit.Entity.View;
using com.Sconit.Entity.MRP;
using System.Reflection;
using NHibernate.Expression;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using com.Sconit.Utility;

public partial class MRP_Schedule_DmdSchedule_Main : MainModuleBase
{
    private bool enableDiscon = true;

    public event EventHandler lbRunMrpClickEvent;

    #region 变量

    private bool isExport = false;
    private int seq = 1;
    private int seq_Detail = 1;
    private Dictionary<string, List<MrpShipPlan>> mrpShipPlanDic
    {
        get;
        set;
    }

    private Dictionary<string, List<ExpectTransitInventory>> expectTransitInventorieDic
    {
        get;
        set;
    }

    private IList<ItemDiscontinue> itemDiscontinueList
    {
        get;
        set;
    }

    List<List<string>> OrderItemList { get; set; }

    private string OrderNo
    {
        get
        {
            return (string)ViewState["OrderNo"];
        }
        set
        {
            ViewState["OrderNo"] = value;
        }
    }

    private DateTime EffDate
    {
        get
        {
            return DateTime.Parse(this.ddlDate.SelectedValue.Trim());
        }
    }

    private DateTime? WinDate
    {
        get
        {
            if (this.tbScheduleTime.Text.Trim() != string.Empty && isWinTime)
            {
                return DateTime.Parse(this.tbScheduleTime.Text.Trim()).Date;
            }
            else
            {
                return null;
            }
        }
    }

    private DateTime? StartDate
    {
        get
        {
            if (this.tbScheduleTime.Text.Trim() != string.Empty && !isWinTime)
            {
                return DateTime.Parse(this.tbScheduleTime.Text.Trim()).Date;
            }
            else
            {
                return null;
            }
        }
    }


    private string flowOrLoc
    {
        get { return this.tbFlowOrLoc.Text.Trim(); }
    }

    private string itemCode
    {
        get { return this.tbItemCode.Text.Trim(); }
    }

    private bool isWinTime
    {
        get { return this.rblDateType.SelectedIndex == 1; }
    }

    private bool isFlow
    {
        get { return this.rblFlowOrLoc.SelectedIndex == 0; }
    }

    private string FlowCode
    {
        get { return (string)ViewState["FlowCode"]; }
        set { ViewState["FlowCode"] = value; }
    }

    private string FlowType
    {
        get { return (string)ViewState["FlowType"]; }
        set { ViewState["FlowType"] = value; }
    }

    private DateTime? ScheduleDate
    {
        get { return (DateTime)ViewState["ScheduleDate"]; }
        set { ViewState["ScheduleDate"] = value; }
    }

    private string PartyCode
    {
        get { return (string)ViewState["PartyCode"]; }
        set { ViewState["PartyCode"] = value; }
    }

    private int ColumnNum
    {
        get { return (int)ViewState["ColumnNum"]; }
        set { ViewState["ColumnNum"] = value; }
    }

    private bool isSupplier
    {
        get
        {
            if (this.ModuleParameter.ContainsKey("IsSupplier"))
            {
                return bool.Parse(this.ModuleParameter["IsSupplier"]);
            }
            return false;
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (isSupplier)
        {
            this.tbFlowOrLoc.ServiceMethod = "GetFlowListForMushengRequire";
            this.tbFlowOrLoc.ServiceParameter = "string:" + this.CurrentUser.Code;
        }
        else
        {
            this.tbFlowOrLoc.ServiceParameter = "string:" + this.CurrentUser.Code + ",bool:true,bool:true,bool:true,bool:true,bool:true,bool:true,string:" + BusinessConstants.PARTY_AUTHRIZE_OPTION_TO;
        }
        this.tbFlow.ServiceParameter = "string:" + this.CurrentUser.Code + ",bool:true,bool:true,bool:true,bool:true,bool:true,bool:true,string:" + BusinessConstants.PARTY_AUTHRIZE_OPTION_TO;

        this.tbWinTime.Attributes.Add("onclick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm',lang:'" + this.CurrentUser.UserLanguage + "'})");
        this.tbWinTime.Attributes["onchange"] += "setStartTime();";
        this.cbIsUrgent.Attributes["onchange"] += "setStartTime();";
        if (!IsPostBack)
        {
            this.cbReleaseOrder.Visible = this.CurrentUser.HasPermission("SubmitOrder");

            this.ucShift.Date = DateTime.Today;

            DetachedCriteria criteria = DetachedCriteria.For<MrpRunLog>();
            criteria.SetProjection(Projections.ProjectionList().Add(Projections.GroupProperty("RunDate")));
            criteria.AddOrder(Order.Desc("RunDate"));
            IList<DateTime> list = TheCriteriaMgr.FindAll<DateTime>(criteria, 0, 3);

            List<string> effDate = list.Select(l => l.ToString("yyyy-MM-dd")).ToList();

            this.ddlDate.DataSource = effDate;
            this.ddlDate.DataBind();
        }

        this.GV_Order.Columns[7].Visible = enableDiscon;
        this.GV_Order.Columns[8].Visible = enableDiscon;

        if (isSupplier)
        {
            this.rblFlowOrLoc.Items[1].Enabled = false;
            //this.rblListFormat.Visible = false;
        }


    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.DoSearch((Button)sender);

        //OrderDataBind();
        #region ///
        //seq = 1;
        //DataControlFieldCollection dcfc = GV_List.Columns;
        //for (int i = 6; i < dcfc.Count; i++)
        //{
        //    DataControlField dcf = dcfc[i];
        //    //if (dcf.SortExpression == e.SortExpression)
        //    //{
        //        ColumnNum = i - 6;
        //        this.hfLastScheduleTime.Value = dcf.FooterText;
        //        this.ScheduleDate = DateTime.Parse(dcf.HeaderText);

        //        OrderDataBind();

        //        if (isFlow)
        //        {
        //            this.tbFlow.Text = this.flowOrLoc;
        //            Flow flow = TheFlowMgr.LoadFlow(tbFlow.Text.Trim(), false, false);
        //            SetOrderHead(flow);
        //        }
        //        //this.ucShift.Date = DateTime.Today;
        //        break;
        //    //}
        //}
        #endregion
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.fld_Search.Visible = true;
        this.div_OrderDetail.Visible = false;
        this.div_MRP_Detail.Visible = false;
        this.fld_Group.Visible = false;
        this.tbFlow.Text = string.Empty;
        this.tbWinTime.Text = string.Empty;
        this.tbStartTime.Text = string.Empty;
        this.cbIsUrgent.Checked = false;
        this.tbStartTime.Text = string.Empty;
        this.tbRefOrderNo.Text = string.Empty;
        this.tbExtOrderNo.Text = string.Empty;
        this.DoSearch(this.btnSearch);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow gvr in this.GV_Detail.Rows)
            {
                HiddenField hdfId = (HiddenField)gvr.FindControl("hdfId");
                int id = int.Parse(hdfId.Value);

                TextBox tbQty = (TextBox)gvr.FindControl("tbQty");
                decimal qty = decimal.Parse(tbQty.Text.Trim());

                //foreach (MrpShipPlan mrpShipPlan in this.mrpShipPlans)
                //{
                //    if (id == mrpShipPlan.Id)
                //    {
                //        mrpShipPlan.Qty = qty;
                //        break;
                //    }
                //}
            }
            ShowSuccessMessage("MRP.Schedule.Update.CustomerSchedule.Result.Successfully");
            //TheMrpShipPlanMgr.UpdateMrpShipPlan(this.mrpShipPlans);
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage("MRP.Schedule.Create.CustomerSchedule.Result.Successfully");
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(this.OrderNo))
        {
            OrderHead orderHead = TheOrderHeadMgr.LoadOrderHead(this.OrderNo);
            string orderTemplate = orderHead.OrderTemplate;
            if (orderTemplate == null || orderTemplate.Length == 0)
            {
                ShowErrorMessage("MasterData.Order.OrderHead.PleaseConfigOrderTemplate");
            }
            else
            {
                //IReportBaseMgr iReportBaseMgr = this.GetIReportBaseMgr(orderTemplate, orderHead);
                string printUrl = TheReportMgr.WriteToFile(orderTemplate, this.OrderNo);

                Page.ClientScript.RegisterStartupScript(GetType(), "method", " <script language='javascript' type='text/javascript'>PrintOrder('" + printUrl + "'); </script>");
            }
        }
    }

    protected void btnExportOrder_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(this.OrderNo))
        {
            OrderHead orderHead = TheOrderHeadMgr.LoadOrderHead(this.OrderNo);
            string orderTemplate = orderHead.OrderTemplate;
            if (orderTemplate == null || orderTemplate.Length == 0)
            {
                ShowErrorMessage("MasterData.Order.OrderHead.PleaseConfigOrderTemplate");
            }
            else
            {
                //IReportBaseMgr iReportBaseMgr = this.GetIReportBaseMgr(orderTemplate, orderHead);
                TheReportMgr.WriteToClient(orderTemplate, this.OrderNo, "order.xls");
            }
        }
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        try
        {
            //OrderDataBind();
            if (this.tbFlow.Text == string.Empty)
            {
                ShowErrorMessage("MRP.Schedule.Import.CustomerSchedule.Result.SelectFlow");
                return;
            }
            Flow flow = TheFlowMgr.CheckAndLoadFlow(this.tbFlow.Text);

            OrderHead orderHead = this.TheOrderMgr.TransferFlow2Order(flow);

            foreach (GridViewRow row in this.GV_Order.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    string item = row.Cells[1].Text;
                    string uom = row.Cells[4].Text;
                    string qtyStr = ((TextBox)row.Cells[9].FindControl("tbQty")).Text;
                    decimal? qty = null;
                    try
                    {
                        qty = decimal.Parse(qtyStr);
                    }
                    catch (Exception)
                    {
                        this.ShowErrorMessage("MasterData.MiscOrder.WarningMessage.InputQtyFormat.Error");
                        return;
                    }

                    if (qty.HasValue && qty > 0)
                    {
                        OrderDetail orderDetail = (from det in orderHead.OrderDetails
                                                   where det.Item.Code == item
                                                   select det).FirstOrDefault();

                        if (orderDetail != null)
                        {
                            orderDetail.OrderedQty = qty.Value;

                            if (orderDetail.Uom.Code != uom)
                            {
                                orderDetail.OrderedQty = this.TheUomConversionMgr.ConvertUomQty(item, uom, orderDetail.OrderedQty, orderDetail.Uom.Code);
                            }
                        }
                    }
                }
            }

            IList<OrderDetail> resultOrderDetailList = new List<OrderDetail>();

            if (orderHead != null && orderHead.OrderDetails != null && orderHead.OrderDetails.Count > 0)
            {
                foreach (OrderDetail orderDetail in orderHead.OrderDetails)
                {
                    if (orderDetail.OrderedQty != 0)
                    {
                        if (orderDetail.Item.Type == BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_K)
                        {
                            IList<Item> newItemList = new List<Item>(); //填充套件子件
                            decimal? convertRate = null;
                            IList<ItemKit> itemKitList = null;

                            var maxSequence = orderHead.OrderDetails.Max(o => o.Sequence);
                            itemKitList = this.TheItemKitMgr.GetChildItemKit(orderDetail.Item);
                            for (int i = 0; i < itemKitList.Count; i++)
                            {
                                Item item = itemKitList[i].ChildItem;
                                if (!convertRate.HasValue)
                                {
                                    if (itemKitList[i].ParentItem.Uom.Code != orderDetail.Item.Uom.Code)
                                    {
                                        convertRate = this.TheUomConversionMgr.ConvertUomQty(orderDetail.Item, orderDetail.Item.Uom, 1, itemKitList[i].ParentItem.Uom);
                                    }
                                    else
                                    {
                                        convertRate = 1;
                                    }
                                }
                                OrderDetail newOrderDetail = new OrderDetail();

                                newOrderDetail.OrderHead = orderDetail.OrderHead;
                                newOrderDetail.Sequence = maxSequence + (i + 1);
                                newOrderDetail.IsBlankDetail = false;
                                newOrderDetail.Item = item;

                                newOrderDetail.Uom = item.Uom;
                                newOrderDetail.UnitCount = orderDetail.Item.UnitCount * itemKitList[i].Qty * convertRate.Value;
                                newOrderDetail.OrderedQty = orderDetail.OrderedQty * itemKitList[i].Qty * convertRate.Value;
                                newOrderDetail.PackageType = orderDetail.PackageType;

                                #region 价格字段
                                if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
                                {
                                    if (orderDetail.PriceList != null && orderDetail.PriceList.Code != string.Empty)
                                    {
                                        newOrderDetail.PriceList = ThePriceListMgr.LoadPriceList(orderDetail.PriceList.Code);
                                        if (newOrderDetail.PriceList != null)
                                        {
                                            PriceListDetail priceListDetail = this.ThePriceListDetailMgr.GetLastestPriceListDetail(newOrderDetail.PriceList, item, newOrderDetail.OrderHead.StartTime, newOrderDetail.OrderHead.Currency, item.Uom);
                                            newOrderDetail.IsProvisionalEstimate = priceListDetail == null ? true : priceListDetail.IsProvisionalEstimate;
                                            if (priceListDetail != null)
                                            {
                                                newOrderDetail.UnitPrice = priceListDetail.UnitPrice;
                                                newOrderDetail.TaxCode = priceListDetail.TaxCode;
                                                newOrderDetail.IsIncludeTax = priceListDetail.IsIncludeTax;
                                            }
                                        }
                                    }
                                }
                                #endregion
                                resultOrderDetailList.Add(newOrderDetail);
                            }
                        }
                        else
                        {
                            resultOrderDetailList.Add(orderDetail);
                        }
                    }
                }
            }
            if (resultOrderDetailList.Count == 0)
            {
                this.ShowErrorMessage("MasterData.Order.OrderHead.OrderDetail.Required");
                return;
            }
            else
            {
                DateTime winTime = this.tbWinTime.Text.Trim() == string.Empty ? DateTime.Now : DateTime.Parse(this.tbWinTime.Text);
                DateTime startTime = winTime;
                if (this.tbSettleTime.Text.Trim() != string.Empty)
                {
                    //orderHead.SettleTime = DateTime.Parse(this.tbSettleTime.Text);
                }

                if (this.tbStartTime.Text != string.Empty)
                {
                    startTime = DateTime.Parse(this.tbStartTime.Text.Trim());
                }
                else
                {
                    double leadTime = this.hfLeadTime.Value == string.Empty ? 0 : double.Parse(this.hfLeadTime.Value);
                    double emTime = this.hfEmTime.Value == string.Empty ? 0 : double.Parse(this.hfEmTime.Value);
                    double lTime = this.cbIsUrgent.Checked ? emTime : leadTime;
                    startTime = winTime.AddHours(0 - lTime);
                }

                if (orderHead.Type == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION)
                {
                    if (this.ucShift.ShiftCode == string.Empty)
                    {
                        ShowErrorMessage("MasterData.Order.Shift.Empty");
                        return;
                    }
                    orderHead.Shift = TheShiftMgr.LoadShift(this.ucShift.ShiftCode);
                }

                orderHead.OrderDetails = resultOrderDetailList;
                orderHead.WindowTime = winTime;
                orderHead.StartTime = startTime;
                orderHead.IsAutoRelease = this.cbReleaseOrder.Checked;
                orderHead.Memo = this.tbMemo.Text;

                if (this.cbIsUrgent.Checked)
                {
                    orderHead.Priority = BusinessConstants.CODE_MASTER_ORDER_PRIORITY_VALUE_URGENT;
                }
                else
                {
                    orderHead.Priority = BusinessConstants.CODE_MASTER_ORDER_PRIORITY_VALUE_NORMAL;
                }
                if (this.tbRefOrderNo.Text.Trim() != string.Empty)
                {
                    orderHead.ReferenceOrderNo = this.tbRefOrderNo.Text.Trim();
                }
                if (this.tbExtOrderNo.Text.Trim() != string.Empty)
                {
                    orderHead.ExternalOrderNo = this.tbExtOrderNo.Text.Trim();
                }
            }

            TheOrderMgr.CreateOrder(orderHead, this.CurrentUser);

            this.OrderNo = orderHead.OrderNo;

            this.ShowSuccessMessage("MasterData.Order.OrderHead.AddOrder.Successfully", orderHead.OrderNo);

            /*
            if (string.IsNullOrEmpty(this.OrderNo))
            {
                this.btnPrint.Visible = false;
                this.btnExportOrder.Visible = false;
            }
            else
            {
                this.btnPrint.Visible = true;
                this.btnExportOrder.Visible = true;
            }
            */

            if (this.cbPrintOrder.Checked)//不要打印
            {
                IList<OrderDetail> orderDetails = orderHead.OrderDetails;
                IList<object> list = new List<object>();
                list.Add(orderHead);
                list.Add(orderDetails);

                IList<OrderLocationTransaction> orderLocationTransactions = TheOrderLocationTransactionMgr.GetOrderLocationTransaction(orderHead.OrderNo);
                list.Add(orderLocationTransactions);
                string printUrl = TheReportMgr.WriteToFile(orderHead.OrderTemplate, list);
                string printJs = "<script language='javascript' type='text/javascript'>PrintOrder('" + printUrl + "'); </script>";
                Page.ClientScript.RegisterStartupScript(GetType(), "method", " <script language='javascript' type='text/javascript'>PrintOrder('" + printUrl + "'); </script>");
            }

            //跳转到相应的订单查询一面
            if (this.cbIsRedirect.Checked)
            {
                string url = string.Empty;
                if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
                {
                    url = "timedMsg('Main.aspx?mid=Order.OrderHead.Production__mp--ModuleType-Production_ModuleSubType-Nml_StatusGroupId-4__act--ListAction);";
                }
                else if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT
                    || orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_SUBCONCTRACTING
                    || orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_CUSTOMERGOODS)
                {
                    url = "timedMsg('Main.aspx?mid=Order.OrderHead.Procurement__mp--ModuleType-Procurement_ModuleSubType-Nml_StatusGroupId-4__act--ListAction);";
                }
                else if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION
                    || orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_TRANSFER)
                {
                    url = "timedMsg('Main.aspx?mid=Order.OrderHead.Distribution__mp--ModuleType-Distribution_ModuleSubType-Nml_StatusGroupId-4__act--ListAction');";
                }
                else
                {
                    return;
                }
                Page.ClientScript.RegisterStartupScript(GetType(), "method", " <script language='javascript' type='text/javascript'>timedMsg('" + url + "'); </script>");
            }
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
        catch (Exception)
        {


        }
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int ii = 0;
        int columnCount = this.GV_List.Columns.Count;
        if (e.Row.RowType == DataControlRowType.Header && isSupplier)
        {
            for (int i = 6; i < columnCount; i++)
            {
                ((LinkButton)(e.Row.Cells[i].Controls[0])).Enabled = false;
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ScheduleBody body = (com.Sconit.Entity.MRP.ScheduleBody)(e.Row.DataItem);

            e.Row.Cells[0].Text = seq.ToString();
            //LinkButton lbnItem = e.Row.Cells[1].Controls[0] as LinkButton;
            Item item = this.TheItemMgr.GetCatchItem(body.Item);
            if (item != null)
            {
                e.Row.Cells[2].Text = item.Description;
                e.Row.Cells[3].Text = item.DefaultSupplier;
            }
            else
            {
                e.Row.Cells[2].Text = "此物料已禁用";
            }
            string lblUom = e.Row.Cells[4].Text;
            string lblUnitCount = e.Row.Cells[5].Text;

            seq++;
            //lbnItem.Text = body.Item;

            if (!isExport)
            {
                for (int i = 6; i < columnCount; i=i+2)
                {
                    string headerText = this.GV_List.Columns[i].SortExpression;
                    //string headerText = this.GV_List.Columns[i].HeaderText;
                    string lastHeaderText = this.GV_List.Columns[i].FooterText;
                    DateTime headerTextTime = DateTime.Parse(headerText);
                    DateTime? lastHeaderTextTime = null;
                    if (lastHeaderText != string.Empty)
                    {
                        lastHeaderTextTime = DateTime.Parse(lastHeaderText);
                    }
                    if (!isSupplier)
                    {
                        //var itemDiscontinues = new List<ItemDiscontinue>();
                        var locationDetails = new List<LocationDetail>();
                        if (i == 6)
                        {
                            var itemDiscontinues = itemDiscontinueList.Where(p => p.Item.Code == body.Item && (p.StartDate == null ? DateTime.MinValue : p.StartDate) <= DateTime.Now && (p.EndDate == null ? DateTime.MaxValue : p.EndDate) >= DateTime.Now).ToList();
                            decimal qty = 0;
                            foreach (var itemDiscontinue in itemDiscontinues)
                            {
                                var locationDetail = this.TheLocationDetailMgr.GetCatchLocationDetail(body.Location, itemDiscontinue.DiscontinueItem.Code);
                                if (locationDetail != null)
                                {
                                    qty += locationDetail.Qty;
                                    locationDetails.Add(locationDetail);
                                }
                            }
                            var txts = e.Row.Cells[i].Text.Split('|');
                            e.Row.Cells[i].Text = txts[0] + "|<font color='blue'>" + qty.ToString("0.####") + "</font>)";
                        }
                        string detailTitle = GetDetail(body.Location, body.Item, headerTextTime, lastHeaderTextTime, locationDetails,ii);
                        ii++;
                        e.Row.Cells[i].Attributes.Add("title", detailTitle);
                    }
                }
            }
            else
            {
                e.Row.Cells[1].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            }
        }
    }

    protected void GV_Order_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ScheduleBody body = (com.Sconit.Entity.MRP.ScheduleBody)(e.Row.DataItem);
            e.Row.Cells[0].Text = seq.ToString();
            try
            {
                Item item = this.TheItemMgr.GetCatchItem(body.Item);
                e.Row.Cells[2].Text = item.Description;
                e.Row.Cells[3].Text = item.DefaultSupplier;

                List<LocationDetail> locationDetails = new List<LocationDetail>();

                var itemDiscontinues = itemDiscontinueList.Where(p => p.Item.Code == item.Code && (p.StartDate == null ? DateTime.MinValue : p.StartDate) <= DateTime.Now && (p.EndDate == null ? DateTime.MaxValue : p.EndDate) >= DateTime.Now);
                foreach (var itemDiscontinue in itemDiscontinues)
                {
                    var locationDetail = this.TheLocationDetailMgr.GetCatchLocationDetail(body.Location, itemDiscontinue.DiscontinueItem.Code);
                    if (locationDetail != null)
                    {
                        locationDetails.Add(locationDetail);
                    }
                }
                decimal disQty = locationDetails.Sum(p => p.Qty);
                e.Row.Cells[8].Text = disQty.ToString("0.##");

                decimal uc = Convert.ToDecimal(e.Row.Cells[5].Text);

                decimal qty = body.Qty0 > (body.DisconActQty0 + disQty) ? (body.Qty0 - body.DisconActQty0 - disQty) : decimal.Zero;

                if (qty % uc != 0)
                {
                    qty = qty - qty % uc + uc;
                }

                ((TextBox)e.Row.Cells[10].FindControl("tbQty")).Text = qty.ToString("0.##");
            }
            catch (Exception)
            { }
            seq++;
        }
    }

    protected void GV_Detail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                ((Label)e.Row.FindControl("lblSequence")).Text = seq_Detail.ToString();
                seq_Detail++;

                Item item = this.TheItemMgr.GetCatchItem(e.Row.Cells[1].Text);
                e.Row.Cells[2].Text = item.Description;
                e.Row.Cells[3].Text = item.DefaultSupplier;

                if (isExport)
                {
                    e.Row.Cells[1].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                }
            }
            catch (Exception)
            { }
        }
    }

    protected void CV_ServerValidate(object source, ServerValidateEventArgs args)
    {
        CustomValidator cv = (CustomValidator)source;

        switch (cv.ID)
        {
            case "cvStartDate":
                try
                {
                    Convert.ToDateTime(args.Value);
                }
                catch (Exception)
                {
                    ShowWarningMessage("Common.Date.Error");
                    args.IsValid = false;
                }
                break;
            default:
                break;
        }
    }

    protected void GV_List_Sorting(Object sender, GridViewSortEventArgs e)
    {
        if (isSupplier)
        {
            return;
        }
        seq = 1;
        DataControlFieldCollection dcfc = ((GridView)sender).Columns;
        for (int i = 6; i < dcfc.Count; i++)
        {
            DataControlField dcf = dcfc[i];
            if (dcf.SortExpression == e.SortExpression)
            {
                ColumnNum = i - 6;
                this.hfLastScheduleTime.Value = dcf.FooterText;
                this.ScheduleDate = DateTime.Parse(e.SortExpression);

                OrderDataBind();

                if (isFlow)
                {
                    this.tbFlow.Text = this.flowOrLoc;
                    Flow flow = TheFlowMgr.LoadFlow(tbFlow.Text.Trim(), false, false);
                    SetOrderHead(flow);
                }
                //this.ucShift.Date = DateTime.Today;
                break;
            }
        }
    }

    private void OrderDataBind()
    {
        string qty = "Qty" + ColumnNum.ToString();
        string actQty = "ActQty" + ColumnNum.ToString();
        string disconActQty = "DisconActQty" + ColumnNum.ToString();

        //todo wintime or starttime
        DateTime? lastScheduleDate = null;
        if (this.hfLastScheduleTime.Value != string.Empty)
        {
            lastScheduleDate = DateTime.Parse(this.hfLastScheduleTime.Value);
        }

        DetachedCriteria criteria = DetachedCriteria.For<ExpectTransitInventory>();
        criteria.Add(Expression.Eq("EffectiveDate", this.EffDate));
        var expectTransitInventories = this.TheCriteriaMgr.FindAll<ExpectTransitInventory>(criteria);
        var mrpShipPlans = TheMrpShipPlanMgr.GetMrpShipPlans((isFlow ? this.flowOrLoc : null), (!isFlow ? this.flowOrLoc : null), this.itemCode, this.EffDate, this.WinDate, this.StartDate);
        IList<MrpShipPlanView> mrpShipPlanViews = MrpShipPlanToMrpShipPlanView(mrpShipPlans);
        IList<ExpectTransitInventoryView> transitInventoryViews = ExpectTransitInventoryToExpectTransitInventoryView(expectTransitInventories);
        itemDiscontinueList = this.TheCriteriaMgr.FindAll<ItemDiscontinue>();
        ScheduleView scheduleView = TheMrpShipPlanViewMgr.TransferMrpShipPlanViews2ScheduleView(mrpShipPlanViews, transitInventoryViews, itemDiscontinueList, this.rblFlowOrLoc.SelectedValue, this.rblDateType.SelectedValue);

        foreach (ScheduleBody body in scheduleView.ScheduleBodys)
        {
            PropertyInfo qtyProp = typeof(ScheduleBody).GetProperty(qty);
            PropertyInfo actQtyProp = typeof(ScheduleBody).GetProperty(actQty);
            PropertyInfo disconActQtyProp = typeof(ScheduleBody).GetProperty(disconActQty);

            body.Qty0 = (decimal)qtyProp.GetValue(body, null);
            body.ActQty0 = (decimal)actQtyProp.GetValue(body, null);
            body.DisconActQty0 = (decimal)disconActQtyProp.GetValue(body, null);
        }

        this.GV_Order.DataSource = scheduleView.ScheduleBodys;
        this.GV_Order.DataBind();
        this.fld_Search.Visible = false;
        this.div_OrderDetail.Visible = true;
        this.div_MRP_Detail.Visible = false;
        this.fld_Group.Visible = false;
    }

    protected void CustomersGridView_Sorted(Object sender, EventArgs e)
    {
        // Display the sort expression and sort direction.
    }

    protected void rblFlowOrLoc_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (isFlow)
        {
            this.tbFlowOrLoc.ServiceParameter = "string:" + this.CurrentUser.Code + ",bool:true,bool:true,bool:true,bool:true,bool:true,bool:true,string:" + BusinessConstants.PARTY_AUTHRIZE_OPTION_TO;
            this.tbFlowOrLoc.ServicePath = "FlowMgr.service";
            this.tbFlowOrLoc.ServiceMethod = "GetFlowList";
            this.tbFlowOrLoc.DescField = "Description";
        }
        else
        {
            this.tbFlowOrLoc.ServiceParameter = "string:" + this.CurrentUser.Code;
            this.tbFlowOrLoc.ServicePath = "LocationMgr.service";
            this.tbFlowOrLoc.ServiceMethod = "GetLocationByUserCode";
            this.tbFlowOrLoc.DescField = "Name";
        }
        this.tbFlowOrLoc.Text = string.Empty;
        this.tbFlowOrLoc.DataBind();
    }

    private void DoSearch(Button button)
    {
        if (this.flowOrLoc == string.Empty)
        {
            ShowErrorMessage("MRP.Schedule.Import.CustomerSchedule.Result.SelectFlow");
            return;
        }
        else if (isFlow)
        {
            Flow flow = TheFlowMgr.LoadFlow(this.flowOrLoc);
            if (flow.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
            {
                this.PartyCode = flow.PartyTo.Code;
            }
            else
            {
                this.PartyCode = flow.PartyFrom.Code;
            }
        }

        //if (isFlow && this.rblListFormat.SelectedIndex == 1 && button == this.btnSearch)
        //{
        //    this.btnCreate2.Visible = true;
        //}
        //else
        //{
        //    this.btnCreate2.Visible = false;
        //}

        DetachedCriteria criteria = DetachedCriteria.For<ExpectTransitInventory>();
        criteria.Add(Expression.Eq("EffectiveDate", this.EffDate));
        var expectTransitInventories = this.TheCriteriaMgr.FindAll<ExpectTransitInventory>(criteria);
        expectTransitInventorieDic = expectTransitInventories.GroupBy(p => p.Item, (k, g) => new { k, g }).ToDictionary(d => d.k, d => d.g.ToList());

        var mrpShipPlans = TheMrpShipPlanMgr.GetMrpShipPlans((isFlow ? this.flowOrLoc : null), (!isFlow ? this.flowOrLoc : null), this.itemCode, this.EffDate, this.WinDate, this.StartDate);
        mrpShipPlanDic = mrpShipPlans.GroupBy(p => p.Item, (k, g) => new { k, g }).ToDictionary(d => d.k, d => d.g.ToList());

        itemDiscontinueList = this.TheCriteriaMgr.FindAll<ItemDiscontinue>();

        this.GV_List.EnableViewState = true;
        this.GV_Detail.EnableViewState = false;
        this.GV_Order.EnableViewState = false;

        IList<MrpShipPlanView> mrpShipPlanViews = MrpShipPlanToMrpShipPlanView(mrpShipPlans);

        IList<ExpectTransitInventoryView> transitInventoryViews = ExpectTransitInventoryToExpectTransitInventoryView(expectTransitInventories);

        ScheduleView scheduleView = TheMrpShipPlanViewMgr.TransferMrpShipPlanViews2ScheduleView(mrpShipPlanViews, transitInventoryViews, itemDiscontinueList, this.rblFlowOrLoc.SelectedValue, this.rblDateType.SelectedValue);
        this.GV_List_DataBind(scheduleView);
        if (button == this.btnExport)
        {
            this.ExportXLS(this.GV_List);
            this.isExport = true;
        }
    }

    private IList<ExpectTransitInventoryView> ExpectTransitInventoryToExpectTransitInventoryView(IList<ExpectTransitInventory> expectTransitInventories)
    {
        IList<ExpectTransitInventoryView> transitInventoryViews = expectTransitInventories.GroupBy(p =>
            new
            {
                p.Flow,
                p.Item,
                p.Uom,
                p.UnitCount,
                p.Location,
                StartTime = this.rblListFormat.SelectedIndex == 0 ? p.StartTime.Date : DateTime.Parse(p.StartTime.ToString("yyyy-MM-01")),
                WindowTime = this.rblListFormat.SelectedIndex == 0 ? p.WindowTime.Date : DateTime.Parse(p.WindowTime.ToString("yyyy-MM-01")),
                p.EffectiveDate
            }, (k, g) => new ExpectTransitInventoryView
            {
                Id = g.Max(q => q.Id),
                Flow = k.Flow,
                Item = k.Item,
                Uom = k.Uom,
                UnitCount = k.UnitCount,
                Location = k.Location,
                StartTime = k.StartTime,
                WindowTime = k.WindowTime,
                TransitQty = g.Sum(q => q.TransitQty),
                EffectiveDate = k.EffectiveDate
            }
        ).OrderBy(p => p.Flow).ThenBy(p => p.Item).ThenBy(p => p.StartTime).ToList();
        return transitInventoryViews;
    }

    private IList<MrpShipPlanView> MrpShipPlanToMrpShipPlanView(IList<MrpShipPlan> mrpShipPlans)
    {
        IList<MrpShipPlanView> mrpShipPlanViews = mrpShipPlans.GroupBy(p =>
            new
            {
                p.Flow,
                p.FlowType,
                p.Item,
                p.ItemDescription,
                p.ItemReference,
                p.BaseUom,
                p.UnitCount,
                p.LocationTo,
                StartTime = this.rblListFormat.SelectedIndex == 0 ? p.StartTime.Date : DateTime.Parse(p.StartTime.ToString("yyyy-MM-01")),
                WindowTime = this.rblListFormat.SelectedIndex == 0 ? p.WindowTime.Date : DateTime.Parse(p.WindowTime.ToString("yyyy-MM-01")),
                p.EffectiveDate
            }, (k, g) => new MrpShipPlanView
            {
                Flow = k.Flow,
                FlowType = k.FlowType,
                Item = k.Item,
                ItemDescription = k.ItemDescription,
                ItemReference = k.ItemReference,
                Uom = k.BaseUom,
                UnitCount = k.UnitCount,
                Location = k.LocationTo,
                StartTime = k.StartTime,
                WindowTime = k.WindowTime,
                Qty = g.Sum(j => j.Qty * j.UnitQty),
                EffectiveDate = k.EffectiveDate
            }).OrderBy(p => p.Flow).ThenBy(p => p.Item).ThenBy(p => p.StartTime).ToList();
        return mrpShipPlanViews;
    }

    private void GV_List_DataBind(ScheduleView scheduleView)
    {
        for (int i = this.GV_List.Columns.Count; i > 6; i--)
        {
            this.GV_List.Columns.RemoveAt(this.GV_List.Columns.Count - 1);
        }

        this.div_MRP_Detail.Visible = false;
        this.div_OrderDetail.Visible = false;
        this.fld_Group.Visible = true;

        if (scheduleView != null)
        {
            IList<ScheduleBody> scheduleBodys = scheduleView.ScheduleBodys;
            IList<ScheduleHead> scheduleHeads = scheduleView.ScheduleHeads;

            #region add qty column
            if (scheduleHeads != null && scheduleHeads.Count > 0)
            {
                int i = 0;
                foreach (ScheduleHead scheduleHead in scheduleHeads)
                {
                    string qty = "Qty" + i.ToString();
                    if (enableDiscon)
                    {
                        qty = "DisplayQty" + i.ToString();
                    }

                    PropertyInfo[] scheduleBodyPropertyInfo = typeof(ScheduleBody).GetProperties();
                    foreach (PropertyInfo pi in scheduleBodyPropertyInfo)
                    {
                        if (pi.Name != null && StringHelper.Eq(pi.Name.ToLower(), qty))
                        {
                            BoundField bfColumn = new BoundField();
                            bfColumn.DataField = qty;
                            bfColumn.DataFormatString = "{0:#,##0.##}";
                            bfColumn.HtmlEncode = false;
                            if (this.rblListFormat.SelectedIndex == 0)
                            {
                                bfColumn.HeaderText = isWinTime ? scheduleHead.DateTo.ToString("MM-dd") : scheduleHead.DateFrom.ToString("MM-dd");
                            }
                            else
                            {
                                bfColumn.HeaderText = isWinTime ? scheduleHead.DateTo.ToString("yyyy-MM") : scheduleHead.DateFrom.ToString("yyyy-MM");
                            }
                            bfColumn.SortExpression = isWinTime ? scheduleHead.DateTo.ToString("yyyy-MM-dd") : scheduleHead.DateFrom.ToString("yyyy-MM-dd");
                            bfColumn.FooterText = isWinTime ? (scheduleHead.LastDateTo.HasValue ? scheduleHead.LastDateTo.Value.ToString("yyyy-MM-dd") : string.Empty) : (scheduleHead.LastDateFrom.HasValue ? scheduleHead.LastDateFrom.Value.ToString("yyyy-MM-dd") : string.Empty);
                            this.GV_List.Columns.Add(bfColumn);

                            //if (this.rblListFormat.SelectedIndex == 1)
                            //{
                            //    TemplateField tf = new TemplateField();
                            //    tf.HeaderText = "订单数";
                            //    MyTemplate mt = new MyTemplate();
                            //    tf.ItemTemplate = mt;
                            //    this.GV_List.Columns.Add(tf);
                            //}
                            break;
                        }
                    }
                    i++;
                }
                this.ltl_GV_List_Result.Visible = false;
            }
            else
            {
                this.ltl_GV_List_Result.Visible = true;
            }

            this.GV_List.DataSource = scheduleBodys;
            this.GV_List.DataBind();
            #endregion
        }
        else
        {
            this.ltl_GV_List_Result.Visible = true;
            this.GV_List.DataSource = null;
            this.GV_List.DataBind();
        }
    }

    private void GV_Detail_DataBind(IList<MrpShipPlan> mrpShipPlans, IList<ExpectTransitInventory> transitList)
    {
        if (mrpShipPlans == null || mrpShipPlans.Count == 0)
        {
            this.ltl_MRP_List_Result.Visible = true;
            //this.btnSave.Visible = false;
        }
        else
        {
            if (mrpShipPlans.Count > 5000)
            {
                mrpShipPlans = mrpShipPlans.Take(5000).ToList();
                ShowWarningMessage("Common.Export.Warning.GreatThan5000", mrpShipPlans.Count.ToString());
            }
            this.ltl_MRP_List_Result.Visible = false;
            //this.btnSave.Visible = true;
        }
        this.GV_Detail.DataSource = mrpShipPlans;
        this.GV_Detail.DataBind();

        this.div_MRP_Detail.Visible = true;
        this.div_OrderDetail.Visible = false;
        this.fld_Group.Visible = false;
    }

    private string GetDetail(string location, string itemCode, DateTime effTime, DateTime? lastHeaderTextTime, List<LocationDetail> locationDetails,int ii)
    {
        StringBuilder detail = new StringBuilder();
        var mrpShipPlans = this.mrpShipPlanDic.ValueOrDefault(itemCode);
        if (mrpShipPlans != null)
        {
            var q_MrpShipPlans = isFlow ?
                mrpShipPlans.Where(m => StringHelper.Eq(flowOrLoc, m.Flow))
                :
                mrpShipPlans.Where(m => StringHelper.Eq(flowOrLoc, m.LocationTo));

            IList<ExpectTransitInventory> expectTransitInventoryList = new List<ExpectTransitInventory>();
            IList<ExpectTransitInventory> disconExpectTransitInventoryList = new List<ExpectTransitInventory>();

            DateTime startDate = effTime.Date;
            DateTime endDate = effTime.Date;
            if (this.rblListFormat.SelectedIndex == 1)
            {
                endDate = effTime.Date.AddMonths(1).AddDays(-1);

                if (ii == 0)
                {
                    startDate = DateTime.MinValue;
                }
                else
                {
                    startDate = effTime.Date;
                }

            }
            
            if (this.rblListFormat.SelectedIndex == 0)
            {
                if (ii == 0)
                {
                    startDate = DateTime.MinValue;
                }
                else
                {
                    for (int i = 0; i < mrpShipPlans.Count; i++)
                    {
                        if (i != 0)
                        {
                            if (effTime.Date == mrpShipPlans[i].WindowTime)
                            {
                                startDate = mrpShipPlans[i - 1].WindowTime;
                                break;
                            }
                        }
                    }
                }
            }
            var expectTransitInventories = this.expectTransitInventorieDic.ValueOrDefault(itemCode);
            if (expectTransitInventories != null)
            {
                var p = from inv in expectTransitInventories
                        where (isFlow ? inv.Flow == this.flowOrLoc : inv.Location == this.flowOrLoc)
                        && inv.Item == itemCode
                        && (isWinTime ?
                             (inv.WindowTime.Date > startDate && inv.WindowTime.Date <= endDate)
                             :
                             (inv.StartTime.Date > startDate && inv.StartTime.Date <= endDate))
                        select inv;

                if (p != null && p.Count() > 0)
                {
                    expectTransitInventoryList = p.ToList();
                }

                if (itemDiscontinueList != null && itemDiscontinueList.Count() > 0)
                {
                    var r = from discon in itemDiscontinueList
                            join inv in expectTransitInventories
                            on discon.DiscontinueItem.Code equals inv.Item
                            where (isFlow ? inv.Flow == this.flowOrLoc : inv.Location == this.flowOrLoc)
                            && discon.Item.Code == itemCode
                            && (isWinTime ?
                             (inv.WindowTime.Date > startDate && inv.WindowTime.Date <= endDate)
                             :
                             (inv.StartTime.Date > startDate && inv.StartTime.Date <= endDate))
                            && discon.StartDate <= inv.StartTime.Date
                            && (!discon.EndDate.HasValue || discon.EndDate.Value >= inv.WindowTime)
                            select inv;

                    if (r != null && r.Count() >= 0)
                    {
                        disconExpectTransitInventoryList = r.ToList();
                    }
                }
            }

            q_MrpShipPlans = q_MrpShipPlans.Where(m => isWinTime ?
                             (m.WindowTime.Date > startDate && m.WindowTime.Date <= endDate)
                             :
                             (m.StartTime.Date > startDate && m.StartTime.Date <= endDate));

            if (q_MrpShipPlans.Count() > 0 || expectTransitInventoryList.Count > 0
                || disconExpectTransitInventoryList.Count > 0 || locationDetails.Count > 0)
            {
                detail.Append("cssbody=[obbd] cssheader=[obhd] header=[${MRP.Schedule.Detail}] body=[<table width=100%>");

                foreach (MrpShipPlan mrpShipPlan in q_MrpShipPlans)
                {
                    string Qty = mrpShipPlan.Qty.ToString("#,##0.##");
                    string startTime = mrpShipPlan.StartTime.ToString("MM-dd");
                    string winTime = mrpShipPlan.WindowTime.ToString("MM-dd");
                    string periodType = mrpShipPlan.SourceDateType;
                    string sourceType = mrpShipPlan.SourceType;
                    string sourceItemCode = mrpShipPlan.SourceItemCode;
                    string sourceItemDescription = mrpShipPlan.SourceItemDescription;

                    detail.Append("<tr><td>${MRP.Schedule.Demand}</td><td>");
                    detail.Append(Qty);
                    detail.Append("</td><td>");
                    detail.Append(startTime);
                    detail.Append("</td><td>");
                    detail.Append(sourceItemCode);
                    detail.Append("</td><td>");
                    detail.Append(mrpShipPlan.SourceId);
                    detail.Append("</td></tr>");
                }

                foreach (ExpectTransitInventory expectTransitInventory in expectTransitInventoryList)
                {
                    string orderNo = expectTransitInventory.OrderNo;
                    string Qty = expectTransitInventory.TransitQty.ToString("#,##0.##");
                    string startTime = expectTransitInventory.StartTime.ToString("MM-dd");
                    string winTime = expectTransitInventory.WindowTime.ToString("MM-dd");

                    detail.Append("<tr><td>订单</td><td>");
                    detail.Append(Qty);
                    detail.Append("</td><td>");
                    detail.Append(startTime);
                    detail.Append("</td><td>");
                    detail.Append(winTime);
                    detail.Append("</td><td>");
                    detail.Append(orderNo);
                    detail.Append("</td></tr>");
                }

                foreach (ExpectTransitInventory expectTransitInventory in disconExpectTransitInventoryList)
                {
                    string item = expectTransitInventory.Item;
                    string orderNo = expectTransitInventory.OrderNo;
                    string Qty = expectTransitInventory.TransitQty.ToString("#,##0.##");
                    string startTime = expectTransitInventory.StartTime.ToString("MM-dd");
                    string winTime = expectTransitInventory.WindowTime.ToString("MM-dd");

                    detail.Append("<tr><td>${MRP.Schedule.Discon}</td><td>");
                    detail.Append(Qty);
                    detail.Append("</td><td>");
                    detail.Append(winTime);
                    detail.Append("</td><td>");
                    detail.Append(orderNo);
                    detail.Append("</td><td>");
                    detail.Append(item);
                    detail.Append("</td></tr>");
                }

                foreach (var locationDetail in locationDetails)
                {
                    if (locationDetail.Qty > 0)
                    {
                        detail.Append("<tr><td>替代库存</td><td>");
                        detail.Append(locationDetail.Qty.ToString("#,##0.##"));
                        detail.Append("</td><td>");
                        detail.Append(locationDetail.Location.Code);
                        detail.Append("</td><td colspan=2>");
                        detail.Append(locationDetail.Item.Code);
                        detail.Append("</td></tr>");
                    }
                }
                detail.Append("</table>]");
            }
        }
        return detail.ToString();
    }

    protected void tbFlow_TextChanged(Object sender, EventArgs e)
    {
        try
        {
            Flow currentFlow = TheFlowMgr.LoadFlow(tbFlow.Text.Trim(), false, false);
            if (currentFlow != null)
            {
                SetOrderHead(currentFlow);
                OrderDataBind();
            }
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    private void SetOrderHead(Flow currentFlow)
    {
        this.FlowCode = currentFlow.Code;
        this.FlowType = currentFlow.Type;

        this.cbReleaseOrder.Checked = currentFlow.IsAutoRelease;
        this.cbPrintOrder.Checked = currentFlow.NeedPrintOrder;
        if (this.ScheduleDate.HasValue)
        {
            if (isWinTime)
            {
                DateTime winTime = FlowHelper.GetWinTime(currentFlow, this.ScheduleDate.Value);
                this.tbWinTime.Text = winTime.ToString("yyyy-MM-dd HH:mm");
                double leadTime = currentFlow.LeadTime.HasValue ? (double)currentFlow.LeadTime.Value : 0;
                this.tbStartTime.Text = winTime.AddHours(-leadTime).ToString("yyyy-MM-dd HH:mm");
            }
            else
            {
                double leadTime = currentFlow.LeadTime.HasValue ? (double)currentFlow.LeadTime.Value : 0;
                DateTime winTime = FlowHelper.GetWinTime(currentFlow, this.ScheduleDate.Value.AddHours(leadTime));
                this.tbWinTime.Text = winTime.ToString("yyyy-MM-dd HH:mm");
                this.tbStartTime.Text = winTime.AddHours(-leadTime).ToString("yyyy-MM-dd HH:mm");
            }
        }

        this.hfLeadTime.Value = currentFlow.LeadTime.ToString();
        this.hfEmTime.Value = currentFlow.EmTime.ToString();

        //  InitDetailParamater(orderHead);

        if (currentFlow.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
        {
            this.ltlShift.Text = "${MasterData.WorkCalendar.Shift}:";
            this.ltlShift.Visible = true;
            this.ucShift.Visible = true;
            //this.tbScheduleTime.Visible = false;
            this.BindShift(currentFlow);
        }
        else if (!enableDiscon && (currentFlow.Type == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION
           || currentFlow.Type == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING
           || currentFlow.Type == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT))
        {
            this.ltlShift.Visible = true;
            this.ltlShift.Text = "${MasterData.Order.OrderHead.SettleTime}:";
            this.ucShift.Visible = false;
            this.tbSettleTime.Visible = true;
            //this.tbScheduleTime.Visible = true;
            this.tbSettleTime.Text = this.tbWinTime.Text;
        }
        else
        {
            this.ltlShift.Visible = false;
            this.ucShift.Visible = false;
            this.tbSettleTime.Visible = false;
        }
    }

    private void BindShift(Flow currentFlow)
    {
        string regionCode = currentFlow != null ? currentFlow.PartyFrom.Code : string.Empty;
        DateTime dateTime = this.tbStartTime.Text.Trim() == string.Empty ? DateTime.Today : DateTime.Parse(this.tbStartTime.Text);
        this.ucShift.BindList(dateTime, regionCode);
    }

    protected void tbWinTime_TextChanged(object sender, EventArgs e)
    {
        if (this.FlowType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
        {
            Flow currentFlow = TheFlowMgr.LoadFlow(this.FlowCode, false);
            this.BindShift(currentFlow);
        }
    }

    public void btnCreate2_Click(object sender, EventArgs e)
    {

    }
}

public class MyTemplate : System.Web.UI.ITemplate
{
    public MyTemplate()
    {

    }
    public void InstantiateIn(Control container)//关键实现这个方法
    {
        TextBox hi = new TextBox();
        hi.Text = "";
        hi.Width = new Unit(50);
        container.Controls.Add(hi);
    }
}


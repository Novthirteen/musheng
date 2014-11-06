using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Exception;
using com.Sconit.Utility;
using com.Sconit.Entity.View;
using NHibernate.Expression;

/// <summary>
/// 不显示BOM
/// </summary>
public partial class Order_NewOrder_List : ModuleBase
{
    public event EventHandler CreateEvent;
    public event EventHandler QuickCreateEvent;
    public event EventHandler BackEvent;


    public bool IsQuick
    {
        get { return (bool)ViewState["IsQuick"]; }
        set { ViewState["IsQuick"] = value; }
    }

    //不合格品退货
    public bool IsReject
    {
        get { return (bool)ViewState["IsReject"]; }
        set { ViewState["IsReject"] = value; }
    }

    //新品
    public bool NewItem
    {
        get { return (bool)ViewState["NewItem"]; }
        set { ViewState["NewItem"] = value; }
    }

    public string ModuleType
    {
        get
        {
            //return BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION;
            return (string)ViewState["ModuleType"];
        }
        set { ViewState["ModuleType"] = value; }
    }

    public string ModuleSubType
    {
        get { return (string)ViewState["ModuleSubType"]; }
        set { ViewState["ModuleSubType"] = value; }
    }

    private string FlowCode
    {
        get { return (string)ViewState["FlowCode"]; }
        set { ViewState["FlowCode"] = value; }
    }

    private string PartyCode
    {
        get { return (string)ViewState["PartyCode"]; }
        set { ViewState["PartyCode"] = value; }
    }

    private bool IsListDetail
    {
        get { return ViewState["IsListDetail"] == null ? true : (bool)ViewState["IsListDetail"]; }
        set { ViewState["IsListDetail"] = value; }
    }

    private Dictionary<int, string[]> IdQty
    {
        get { return (Dictionary<int, string[]>)ViewState["IdQty"]; }
        set { ViewState["IdQty"] = value; }
    }
    private string ItemCodes
    {
        get { return (string)ViewState["ItemCodes"]; }
        set { ViewState["ItemCodes"] = value; }
    }

    public void PageCleanup()
    {
        this.tbFlow.Text = string.Empty;
        this.tbSettleTime.Text = string.Empty;
        this.tbRefOrderNo.Text = string.Empty;
        this.tbExtOrderNo.Text = string.Empty;
        this.tbWinTime.Text = string.Empty;
        this.cbIsUrgent.Checked = false;
        //this.cbEnableBinding.Checked = true;
        //this.tbStartTime.Text = string.Empty;
        this.InitDetailParamater(null);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.tbWinTime.Attributes.Add("onclick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm',lang:'" + this.CurrentUser.UserLanguage + "'})");
        this.tbWinTime.Attributes["onchange"] += "setStartTime();";
        this.cbIsUrgent.Attributes["onchange"] += "setStartTime();";

        if (!IsPostBack)
        {
            this.ucShift.Date = DateTime.Today;
        }

        if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT)
        {
            this.tbFlow.ServiceParameter = "string:" + this.CurrentUser.Code + ",bool:true,bool:false,bool:true,bool:false,bool:true,bool:true,string:" + BusinessConstants.PARTY_AUTHRIZE_OPTION_FROM;
            this.lblFlow.Text = "${MasterData.Flow.Flow.Procurement}:";
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION)
        {
            this.tbFlow.ServiceParameter = "string:" + this.CurrentUser.Code + ",bool:false,bool:false,bool:false,bool:true,bool:false,bool:false,string:" + BusinessConstants.PARTY_AUTHRIZE_OPTION_FROM;
            //this.lblSettleTime.Visible = false;
            //this.tbSettleTime.Visible = false;
            this.ltlShift.Visible = true;
            this.ucShift.Visible = true;
            this.lblFlow.Text = "${MasterData.Flow.Flow.Production}:";
        }
        else
        {
            this.tbFlow.ServiceParameter = "string:" + this.CurrentUser.Code + ",bool:false,bool:true,bool:true,bool:false,bool:false,bool:false,string:" + BusinessConstants.PARTY_AUTHRIZE_OPTION_TO;
            this.lblFlow.Text = "${MasterData.Flow.Flow.Distribution}:";
        }
    }

    protected void tbFlow_TextChanged(Object sender, EventArgs e)
    {
        try
        {
            this.tbWinTime.Text = string.Empty;
            this.tbStartTime.Text = string.Empty;
            if (this.tbFlow.Text != string.Empty)
            {
                Flow currentFlow = TheFlowMgr.LoadFlow(this.tbFlow.Text, this.CurrentUser, true, true);
                if (currentFlow != null)
                {
                    IList<FlowBinding> flowBindingList = TheFlowBindingMgr.GetFlowBinding(currentFlow.Code);
                    if (flowBindingList != null && flowBindingList.Count > 0)
                    {
                        //this.cbEnableBinding.Visible = true;
                    }
                    SecurityHelper.CheckPermission(currentFlow.Type, currentFlow.PartyFrom.Code, currentFlow.PartyTo.Code, this.CurrentUser);
                    this.FlowCode = currentFlow.Code;

                    if (currentFlow.Type == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION)
                    {
                        this.GV_List.Columns[7].Visible = false;
                    }
                    else if (currentFlow.Type == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS ||
                        currentFlow.Type == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT)
                    {
                        this.GV_List.Columns[6].Visible = false;
                    }

                    this.PartyCode = currentFlow.Type == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION ?
                        currentFlow.PartyTo.Code : currentFlow.PartyFrom.Code;

                    this.cbReleaseOrder.Checked = currentFlow.IsAutoRelease;
                    // this.cbNeedInspect.Checked = currentFlow.NeedPrintOrder;

                    this.ItemCodes = string.Empty;
                    DateTime winTime = com.Sconit.Utility.FlowHelper.GetWinTime(currentFlow, DateTime.Now);
                    this.tbWinTime.Text = winTime.ToString("yyyy-MM-dd HH:mm");
                    this.tbSettleTime.Text = this.tbWinTime.Text;
                    double leadTime = currentFlow.LeadTime.HasValue ? -(double)currentFlow.LeadTime.Value : 0;
                    this.tbStartTime.Text = winTime.AddHours(leadTime).ToString("yyyy-MM-dd HH:mm");

                    if (currentFlow.Type == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION)
                    {
                        string regionCode = currentFlow != null ? currentFlow.PartyFrom.Code : string.Empty;
                        DateTime dateTime = this.tbStartTime.Text.Trim() == string.Empty ? DateTime.Today : DateTime.Parse(this.tbStartTime.Text);
                        this.ucShift.BindList(dateTime, regionCode);
                        if (this.ucShift.ShiftCode != string.Empty)
                        {
                            this.tbStartTime.Text = TheShiftMgr.GetShiftStartTime(dateTime, this.ucShift.ShiftCode).ToString("yyyy-MM-dd HH:mm");
                            this.tbWinTime.Text = TheShiftMgr.GetShiftEndTime(dateTime, this.ucShift.ShiftCode).ToString("yyyy-MM-dd HH:mm");
                        }
                    }

                    this.hfLeadTime.Value = currentFlow.LeadTime.ToString();
                    this.hfEmTime.Value = currentFlow.EmTime.ToString();
                    this.IsListDetail = currentFlow.IsListDetail;

                    if (!this.IsListDetail)
                    {
                        this.InitDetailParamater(null);
                    }
                    else
                    {
                        this.InitDetailParamater(currentFlow.FlowDetails);
                    }
                }
            }
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    private void InitDetailParamater(IList<FlowDetail> flowDetails)
    {
        flowDetails = flowDetails == null ? new List<FlowDetail>() : flowDetails;
        if (!this.IsListDetail)
        {
            FlowDetail flowDetail = new FlowDetail();
            flowDetail.IsBlankDetail = true;
            flowDetails.Add(flowDetail);
        }

        this.GV_List.DataSource = flowDetails;
        this.GV_List.DataBind();
        this.fdDetail.Visible = flowDetails.Count > 0;
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (this.tbFlow.Text == string.Empty)
            {
                ShowErrorMessage("${MRP.Schedule.Import.CustomerSchedule.Result.SelectFlow}");
                return;
            }
            Flow flow = TheFlowMgr.CheckAndLoadFlow(this.tbFlow.Text, this.CurrentUser);

            OrderHead orderHead = this.TheOrderMgr.TransferFlow2Order(flow);

            foreach (GridViewRow row in this.GV_List.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    string seq = ((Label)row.FindControl("lblSeq")).Text;
                    string item = ((Label)row.FindControl("lblItemCode")).Text;
                    string uom = ((Label)row.FindControl("lblUom")).Text;
                    string uc = ((Label)row.FindControl("lblUnitCount")).Text;
                    string qtyStr = ((TextBox)row.FindControl("tbOrderQty")).Text.Trim();
                    string remark = ((TextBox)row.FindControl("tbRemark")).Text.Trim();

                    decimal qty = qtyStr == string.Empty ? 0M : decimal.Parse(qtyStr);

                    if (qty != 0)
                    {
                        var q = from det in orderHead.OrderDetails
                                where det.Item.Code == item && det.Uom.Code == uom && det.UnitCount == decimal.Parse(uc)
                                select det;

                        if (q != null && q.Count() > 0)
                        {
                            OrderDetail orderDetail = q.SingleOrDefault();
                            orderDetail.OrderedQty = qty;
                            orderDetail.Remark = remark;
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
                //orderHead.IsEnableBinding = this.cbEnableBinding.Checked;
                orderHead.SubType = GetOrderSubType(orderHead);
                //orderHead.NeedInspection = this.cbNeedInspect.Checked;

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
            //if (this.cbPrintOrder.Checked && false)//不要打印
            //{
            //    IList<OrderDetail> orderDetails = orderHead.OrderDetails;
            //    IList<object> list = new List<object>();
            //    list.Add(orderHead);
            //    list.Add(orderDetails);

            //    IList<OrderLocationTransaction> orderLocationTransactions = TheOrderLocationTransactionMgr.GetOrderLocationTransaction(orderHead.OrderNo);
            //    list.Add(orderLocationTransactions);
            //    string printUrl = TheReportMgr.WriteToFile(orderHead.OrderTemplate, list);
            //    Page.ClientScript.RegisterStartupScript(GetType(), "method", " <script language='javascript' type='text/javascript'>PrintOrder('" + printUrl + "'); </script>");
            //}
            this.ShowSuccessMessage("MasterData.Order.OrderHead.AddOrder.Successfully", orderHead.OrderNo);
            if (this.cbContinuousCreate.Checked)
            {
                this.PageCleanup();
            }
            else
            {
                if (CreateEvent != null)
                {
                    CreateEvent(orderHead.OrderNo, e);
                }
            }
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
        catch (Exception ex)
        {
            this.ShowErrorMessage(ex.Message);
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
    {
        DateTime winTime = this.tbWinTime.Text.Trim() == string.Empty ? DateTime.Now : DateTime.Parse(this.tbWinTime.Text);
        DateTime startTime = this.tbStartTime.Text.Trim() == string.Empty ? DateTime.Now : DateTime.Parse(this.tbWinTime.Text);
        double leadTime = this.hfLeadTime.Value == string.Empty ? 0 : double.Parse(this.hfLeadTime.Value);
        double emTime = this.hfEmTime.Value == string.Empty ? 0 : double.Parse(this.hfEmTime.Value);
        if (this.tbStartTime.Text != string.Empty)
        {
            startTime = DateTime.Parse(this.tbStartTime.Text.Trim());
        }
        else
        {
            double lTime = this.cbIsUrgent.Checked ? emTime : leadTime;
            startTime = winTime.AddHours(0 - lTime);
        }
        if (startTime < DateTime.Now)
        {
            args.IsValid = false;
        }
    }

    protected void tbWinTime_TextChanged(object sender, EventArgs e)
    {
        if (tbWinTime.Text.Trim() != string.Empty && tbFlow.Text.Trim() != string.Empty)
        {
            OrderHead orderHead = TheOrderMgr.TransferFlow2Order(this.tbFlow.Text.Trim());
            orderHead.WindowTime = DateTime.Parse(tbWinTime.Text.Trim());
            //InitDetailParamater(orderHead);
        }
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            FlowDetail flowDetail = (FlowDetail)e.Row.DataItem;
            Label lblItemCode = (Label)e.Row.FindControl("lblItemCode");
            //string[] refOrderNos = this.tbRefOrderNo.Text.Trim().Split('$');
            //IList<string> orderNoList = refOrderNos.ToList<string>();
            //IList<OrderLocationTransaction> orderLocationTransactionList = this.TheOrderLocationTransactionMgr.GetOrderLocationTransaction(orderNoList, BusinessConstants.IO_TYPE_IN);

            if (flowDetail.ReferenceItemCode == null || flowDetail.ReferenceItemCode == string.Empty)
            {
                ((Label)(e.Row.FindControl("lblReferenceItemCode"))).Text = //this.GetItemReference(lblItemCode.Text, this.PartyCode, null);
                    TheItemReferenceMgr.GetItemReferenceByItem(lblItemCode.Text, this.PartyCode, null);
            }

            if (ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION)
            {
                RangeValidator revOrderQty = (RangeValidator)e.Row.FindControl("revOrderQty");
                revOrderQty.MinimumValue = "0";
            }

            if (ItemCodes == string.Empty)
            {
                ItemCodes = lblItemCode.Text;
            }
            else
            {
                ItemCodes = ItemCodes + "|" + lblItemCode.Text;
            }

            if (flowDetail.IsBlankDetail)
            {
                Controls_TextBox tbItemCode = (Controls_TextBox)e.Row.FindControl("tbItemCode");
                tbItemCode.Visible = true;
                lblItemCode.Visible = false;
            }
            else
            {
                if (!IsListDetail)
                {
                    TextBox tbOrderQty = (TextBox)e.Row.FindControl("tbOrderQty");
                    TextBox tbRemark = (TextBox)e.Row.FindControl("tbRemark");
                    tbOrderQty.Text = IdQty[flowDetail.Id][0];
                    tbRemark.Text = IdQty[flowDetail.Id][1];
                }
                Label lblLocFrom = (Label)e.Row.FindControl("lblLocFrom");
                Label lblLocTo = (Label)e.Row.FindControl("lblLocTo");
                if (flowDetail.DefaultLocationFrom != null)
                {
                    lblLocFrom.Text = flowDetail.DefaultLocationFrom.Code;
                    lblLocFrom.ToolTip = flowDetail.DefaultLocationFrom.Name;
                }
                if (flowDetail.DefaultLocationTo != null)
                {
                    lblLocTo.Text = flowDetail.DefaultLocationTo.Code;
                    lblLocTo.ToolTip = flowDetail.DefaultLocationTo.Name;
                }
            }
        }
    }


    protected void GV_List_RowCreated(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Controls_TextBox tbItemCode = (Controls_TextBox)e.Row.FindControl("tbItemCode");
            tbItemCode.ServiceParameter = "string:" + this.FlowCode + ",string:" + ItemCodes;
        }
    }

    protected void btnRefOrderNo_Click(object sender, EventArgs e)
    {
        string refOrderNo = this.tbRefOrderNo.Text.Trim();
        IList<OrderLocationTransaction> olts = TheOrderLocationTransactionMgr.GetOrderLocationTransaction(refOrderNo, BusinessConstants.IO_TYPE_OUT);
        if (olts == null || olts.Count == 0)
        {
            ShowWarningMessage("MasterData.Order.OrderHead.Equal.Fail", this.tbRefOrderNo.Text.Trim());
            return;
        }
        if (this.FlowCode == null)
        {
            this.FlowCode = olts[0].OrderDetail.OrderHead.Flow;
            this.tbFlow.Text = this.FlowCode;
            this.tbFlow_TextChanged(sender, e);
        }

        var items = olts.Select(o => o.Item).Distinct();
        IList<FlowDetail> flowDetails = new List<FlowDetail>();
        IdQty = new Dictionary<int, string[]>();

        foreach (Item item in items)
        {
            FlowView flowView = TheFlowMgr.LoadFlowView(this.FlowCode, item);
            if (flowView != null && flowView.FlowDetail != null)
            {
                flowDetails.Add(flowView.FlowDetail);
            }
        }
        foreach (FlowDetail fd in flowDetails)
        {
            foreach (var olt in olts)
            {
                if (fd.Item.Code == olt.Item.Code)
                {
                    decimal qty = olt.AccumulateQty.HasValue ? olt.AccumulateQty.Value : olt.OrderedQty;
                    fd.OrderedQty += TheUomConversionMgr.ConvertUomQty(fd.Item, olt.Uom, qty, fd.Uom);
                    string[] qtyremark = new string[] { qty.ToString("0.########"), string.Empty };
                    IdQty.Add(fd.Id, qtyremark);
                }
            }
        }
        this.IsListDetail = false;
        this.InitDetailParamater(flowDetails);
        ShowSuccessMessage("MasterData.Order.OrderHead.Equal.Successfully");
    }

    protected void tbItemCode_TextChanged(object sender, EventArgs e)
    {

        IList<FlowDetail> flowDetails = new List<FlowDetail>();

        IdQty = new Dictionary<int, string[]>();

        foreach (GridViewRow row in this.GV_List.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                string hfId = ((HiddenField)row.FindControl("hfId")).Value;
                int id = int.Parse(hfId);

                string tbOrderQty = ((TextBox)row.FindControl("tbOrderQty")).Text.Trim();
                string remark = ((TextBox)row.FindControl("tbRemark")).Text.Trim();

                string[] qtyremark = new string[] { tbOrderQty, remark };
                if (id > 0)
                {
                    FlowDetail flowDetail = TheFlowDetailMgr.LoadFlowDetail(id);
                    flowDetail.OrderedQty = tbOrderQty == string.Empty ? 0 : decimal.Parse(tbOrderQty);
                    //flowDetail.Memo = remark;
                    flowDetail.IsBlankDetail = false;
                    flowDetails.Add(flowDetail);
                    IdQty.Add(id, qtyremark);
                }
            }
        }
        TextBox tbItem = (System.Web.UI.WebControls.TextBox)(sender);
        Item item = TheItemMgr.LoadItem(tbItem.Text.Trim());
        if (item != null)
        {
            FlowView flowView = TheFlowMgr.LoadFlowView(this.FlowCode, item);
            if (flowView != null && flowView.FlowDetail != null)
            {
                flowView.FlowDetail.IsBlankDetail = false;
                flowDetails.Add(flowView.FlowDetail);
                IdQty.Add(flowView.FlowDetail.Id, new string[] { "0", string.Empty });
                this.InitDetailParamater(flowDetails);
                return;
            }
        }
        ShowErrorMessage("Flow.Error.NotFoundMacthFlow", tbItem.Text);
        tbItem.Text = string.Empty;
    }


    private string GetOrderSubType(OrderHead orderHead)
    {
        bool isNml = true;
        bool isRtn = true;
        string orderSubType = string.Empty;
        foreach (OrderDetail orderDetail in orderHead.OrderDetails)
        {
            if (orderDetail.OrderedQty < 0)
            {
                isNml = false;
            }

            if (orderDetail.OrderedQty > 0)
            {
                isRtn = false;
            }
        }
        if (isNml)
        {
            orderSubType = BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML;
        }
        else if (isRtn)
        {
            orderSubType = BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RTN;
        }
        else
        {
            orderSubType = BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_ADJ;
        }
        return orderSubType;
    }


}

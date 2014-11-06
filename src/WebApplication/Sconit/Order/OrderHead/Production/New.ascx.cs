using System;
using System.Collections;
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
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity;
using com.Sconit.Utility;
using com.Sconit.Control;


using com.Sconit.Entity.Distribution;
using com.Sconit.Service.Ext.Distribution;

public partial class Order_OrderHead_New : NewModuleBase
{
    public event EventHandler CreateEvent;
    public event EventHandler BackEvent;
    public event EventHandler QuickCreateEvent;

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

    public bool IsQuick
    {
        get
        {
            return (bool)ViewState["IsQuick"];
        }
        set
        {
            ViewState["IsQuick"] = value;
        }
    }

    //新品
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
    private string CurrentFlowCode
    {
        get
        {
            return (string)ViewState["CurrentFlowCode"];
        }
        set
        {
            ViewState["CurrentFlowCode"] = value;
        }
    }

    public void PageCleanup()
    {
        this.tbFlow.Text = string.Empty;
        this.tbRefOrderNo.Text = string.Empty;
        this.tbExtOrderNo.Text = string.Empty;
        this.tbWinTime.Text = string.Empty;
        this.cbIsUrgent.Checked = false;
        this.CurrentFlowCode = null;
        this.tbStartTime.Text = string.Empty;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        List<Flow> flowList = new List<Flow>();

        string userLanguage = this.CurrentUser.UserLanguage;
        this.tbWinTime.Attributes.Add("onclick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm',lang:'" + this.CurrentUser.UserLanguage + "'})");
        this.tbWinTime.Attributes["onchange"] += "setStartTime();";
        this.cbIsUrgent.Attributes["onchange"] += "setStartTime();";

        if (this.BackEvent != null)
        {
            this.btnBack.Visible = true;
        }
        else
        {
            this.btnBack.Visible = false;
        }

        if (!IsPostBack)
        {

            this.ucShift.Date = DateTime.Today;
        }

        if (NewItem)
        {
            this.lblItemVersion.Visible = true;
            this.tbItemVersion.Visible = true;
            this.rfvItemVersion.Enabled = true;
            //this.tbFlow.ServiceMethod = "GetProductionFlow";
            //this.tbFlow.ServiceParameter = "string:" + this.CurrentUser.Code;
        }
    }

    protected void tbFlow_TextChanged(Object sender, EventArgs e)
    {
        try
        {
            this.tbWinTime.Text = string.Empty;
            this.tbStartTime.Text = string.Empty;
            if (this.CurrentFlowCode == null || this.CurrentFlowCode != this.tbFlow.Text)
            {
                Flow currentFlow = TheFlowMgr.LoadFlow(tbFlow.Text, false);
                if (currentFlow != null)
                {
                    this.CurrentFlowCode = currentFlow.Code;

                    // OrderHead orderHead = TheOrderMgr.TransferFlow2Order(currentFlow);
                    //orderHead.SubType = this.ModuleSubType;
                    //orderHead.WindowTime = DateTime.Now;

                    DateTime winTime = com.Sconit.Utility.FlowHelper.GetWinTime(currentFlow, DateTime.Now);
                    this.tbWinTime.Text = winTime.ToString("yyyy-MM-dd HH:mm");
                    double leadTime = currentFlow.LeadTime.HasValue ? -(double)currentFlow.LeadTime.Value : 0;
                    this.tbStartTime.Text = winTime.AddHours(leadTime).ToString("yyyy-MM-dd HH:mm");
                    //orderHead.WindowTime = winTime;

                    this.hfLeadTime.Value = currentFlow.LeadTime.ToString();
                    this.hfEmTime.Value = currentFlow.EmTime.ToString();

                    //  InitDetailParamater(orderHead);

                    this.BindShift(currentFlow);
                }

            }
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void tbWinTime_TextChanged(object sender, EventArgs e)
    {
        this.BindShift();
    }

    private void BindShift()
    {
        Flow currentFlow = TheFlowMgr.LoadFlow(tbFlow.Text, false);
        BindShift(currentFlow);
    }

    private void BindShift(Flow currentFlow)
    {
        string regionCode = currentFlow != null ? currentFlow.PartyFrom.Code : string.Empty;
        DateTime dateTime = this.tbStartTime.Text.Trim() == string.Empty ? DateTime.Today : DateTime.Parse(this.tbStartTime.Text);
        this.ucShift.BindList(dateTime, regionCode);
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {

        if (this.ucShift.ShiftCode == string.Empty)
        {
            ShowErrorMessage("MasterData.Order.Shift.Empty");
            return;
        }

        if (this.cvStartTime.IsValid)
        {
            Flow currentFlow = TheFlowMgr.LoadFlow(tbFlow.Text, true);

            OrderHead orderHead = TheOrderMgr.TransferFlow2Order(currentFlow);

            foreach (OrderDetail orderDetail in orderHead.OrderDetails)
            {
                if (orderDetail.Item.Code == this.tbItemCode.Text)
                {
                    orderDetail.RequiredQty = decimal.Parse(tbOrderQty.Text);
                    orderDetail.OrderedQty = orderDetail.RequiredQty;
                    if (NewItem)
                    {
                        orderDetail.ItemVersion = this.tbItemVersion.Text;
                    }
                }
            }

            DateTime winTime = DateTime.Parse(this.tbWinTime.Text);
            DateTime startTime = DateTime.Parse(this.tbWinTime.Text);
            //string shiftCode = this.ucShift.ShiftCode;

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


            orderHead.WindowTime = winTime;
            orderHead.StartTime = startTime;
            orderHead.SubType = this.ModuleSubType;
            //orderHead.IsAutoRelease = this.cbReleaseOrder.Checked;
            orderHead.IsNewItem = this.NewItem;
            if (this.ucShift.ShiftCode != string.Empty)
            {
                orderHead.Shift = TheShiftMgr.LoadShift(this.ucShift.ShiftCode);
            }
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
            if (this.txProductLineFacility.Text.Trim() != string.Empty)
            {
                orderHead.ProductLineFacility = this.txProductLineFacility.Text.Trim();
            }
            //orderHead.NeedSortAndColor = this.cbNeedSortAndColor.Checked;
            //if (this.IsQuick)
            //{
            //    orderHead.IsAutoRelease = true;
            //    orderHead.IsAutoStart = true;
            //    orderHead.IsAutoShip = true;
            //    orderHead.IsAutoReceive = true;
            //    orderHead.StartLatency = 0;
            //    orderHead.CompleteLatency = 0;
            //}

            try
            {
                TheOrderMgr.CreateOrder(orderHead, this.CurrentUser);

                #region 返工更新数量
                EntityPreference entityPreference = TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_IS_DEFAULT_QTY_ZERO);
                if (bool.Parse(entityPreference.Value))
                {
                    if (orderHead.Type == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION
                        && (orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RWO ||
                        orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_ADJ))
                    {
                        TheOrderMgr.TryUpdateWoLoctrans(orderHead.OrderNo, orderHead.SubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RWO);
                    }
                }
                #endregion


                this.ShowSuccessMessage("MasterData.Order.OrderHead.AddOrder.Successfully", orderHead.OrderNo);

                if (CreateEvent != null)
                {
                    CreateEvent(orderHead.OrderNo, e);
                }
            }
            catch (BusinessErrorException ex)
            {
                this.ShowErrorMessage(ex);
                return;
            }

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
        DateTime winTime = DateTime.Parse(this.tbWinTime.Text);
        DateTime startTime = DateTime.Parse(this.tbWinTime.Text);
        double leadTime = this.hfLeadTime.Value == string.Empty ? 0 : double.Parse(this.hfLeadTime.Value);
        double emTime = this.hfEmTime.Value == string.Empty ? 0 : double.Parse(this.hfEmTime.Value);
        if (this.tbStartTime.Text != string.Empty)
        {
            winTime = DateTime.Parse(this.tbWinTime.Text.Trim());
        }

        if (this.tbStartTime.Text != string.Empty)
        {
            startTime = DateTime.Parse(this.tbStartTime.Text.Trim());
        }
        else
        {
            double lTime = this.cbIsUrgent.Checked ? emTime : leadTime;
            startTime = winTime.AddHours(0 - lTime);
        }
        //if (startTime < DateTime.Now)
        //{
        //    args.IsValid = false;
        //    this.cvStartTime.ErrorMessage = "${MasterData.Order.OrderHead.StartTime.Later.Than.Now}";

        //}

        //if (startTime > winTime)
        //{
        //    args.IsValid = false;
        //    this.cvStartTime.ErrorMessage = "${MasterData.Order.OrderHead.StartTime.Later.Than.WinTime}";
        //}
    }
}

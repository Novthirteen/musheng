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
using com.Sconit.Entity.MasterData;
using com.Sconit.Control;
using com.Sconit.Entity;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Procurement;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.Distribution;
using System.IO;
using System.Collections.Generic;



public partial class Order_OrderHead_Edit : EditModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler UpdateLocTransAndActBillEvent;
    public event EventHandler GetLocTransHuListEvent;

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

    public IList<Hu> HuList
    {
        get
        {
            return (IList<Hu>)ViewState["HuList"];
        }
        set
        {
            ViewState["HuList"] = value;
        }
    }

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

    private bool EscapeSortAndColorCheck
    {
        get
        {
            return (bool)ViewState["EscapeSortAndColorCheck"] == null ? false : true;
        }
        set
        {
            ViewState["EscapeSortAndColorCheck"] = value;
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

    //报废
    public bool IsScrap
    {
        get
        {
            return (bool)ViewState["IsScrap"];
        }
        set
        {
            ViewState["IsScrap"] = value;
        }
    }

    //原材料回用
    public bool IsReuse
    {
        get
        {
            return (bool)ViewState["IsReuse"];
        }
        set
        {
            ViewState["IsReuse"] = value;
        }
    }

    public void InitPageParameter(string orderNo)
    {
        this.OrderNo = orderNo;
        this.ODS_Order.SelectParameters["orderNo"].DefaultValue = this.OrderNo;
        this.UpdateView();


        #region 根据ModuleSubType状态调整显示的文字
        if (this.ModuleSubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RTN)
        {
            this.btnReceive.Text = "${Common.Button.Return}";
        }

        #endregion

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucDetail.SaveEvent += new System.EventHandler(this.SaveRender);
        this.ucDetail.ShipEvent += new System.EventHandler(this.ShipRender);
        this.ucDetail.ReceiveEvent += new System.EventHandler(this.ReceiveRender);
        this.ucDetail.UpdateLocTransAndActBillEvent += new System.EventHandler(this.UpdateLocTransAndActBillRender);
        this.ucConfirmInfo.ConfirmEvent += new System.EventHandler(this.ReceiveRender);

        if (!IsPostBack)
        {
            this.ucDetail.ModuleType = this.ModuleType;
            this.ucDetail.ModuleSubType = this.ModuleSubType;
            this.ucDetail.NewItem = this.NewItem;
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        TextBox tbWinTime = (TextBox)this.FV_Order.FindControl("tbWinTime");
        TextBox tbStartTime = (TextBox)this.FV_Order.FindControl("tbStartTime");
        if (tbWinTime.Text.Trim() == string.Empty)
        {
            ShowErrorMessage("MasterData.Order.OrderHead.WinTime.Required");
            return;
        }
        if (tbStartTime.Text.Trim() == string.Empty)
        {
            ShowErrorMessage("MasterData.Order.OrderHead.StartTime.Required");
            return;
        }
        DateTime winTime = Convert.ToDateTime(tbWinTime.Text.Trim());
        DateTime startTime = Convert.ToDateTime(tbStartTime.Text.Trim());
        if (winTime < startTime)
        {
            ShowErrorMessage("MasterData.Order.OrderHead.StartTime.Later.Than.WinTime");
            return;
        }
        this.ucDetail.SaveCallBack("");

        //  UpdateLocTransAndActBillEvent(this.OrderNo, null);

    }
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            // this.ucDetail.SaveCallBack();
            Receipt receipt = new Receipt();
            if (this.IsReuse)
            {
                GetLocTransHuListEvent(sender, e);
                if (this.HuList != null && this.HuList.Count > 0)
                {
                    receipt = TheOrderMgr.ReleaseReuseOrder(this.OrderNo, this.CurrentUser, this.HuList);
                }
                else
                {
                    receipt = TheOrderMgr.ReleaseScrapOrder(this.OrderNo, this.CurrentUser);
                }
            }
            else
            {
                receipt = TheOrderMgr.ReleaseScrapOrder(this.OrderNo, this.CurrentUser);
            }
            this.FV_Order.DataBind();

            ShowSuccessMessage("MasterData.Order.OrderHead.Confirm.Successfully", this.OrderNo, receipt.ReceiptNo);
            UpdateView();

            UpdateLocTransAndActBillEvent(this.OrderNo, null);



        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnCheck_Click(object sender, EventArgs e)
    {
        try
        {
            IDictionary<string, decimal> rmDic = TheOrderMgr.FindRMShortageForWO(this.OrderNo, this.CurrentUser);
            if (rmDic != null && rmDic.Count > 0)
            {
                string rmstr = string.Empty;
                foreach (string rm in rmDic.Keys)
                {
                    if (rmstr != string.Empty)
                    {
                        rmstr += "   " + rm + " " + rmDic[rm].ToString("#.##") + "";
                    }
                    else
                    {
                        rmstr = rm + " " + rmDic[rm].ToString("#.##") + "";
                    }
                }
                this.ShowWarningMessage("Order.Error.RMNotEnough", rmstr);
            }
            else
            {
                this.ShowSuccessMessage("Order.InventoryCheck.RMEnough");
                this.btnSubmit.Visible = true;
                this.btnForceSubmit.Visible = false;
            }
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            IDictionary<string, decimal> rmDic = TheOrderMgr.FindRMShortageForWO(this.OrderNo, this.CurrentUser);
            if (rmDic != null && rmDic.Count > 0)
            {
                string rmstr = string.Empty;
                foreach (string rm in rmDic.Keys)
                {
                    if (rmstr != string.Empty)
                    {
                        rmstr += "   " + rm + " " + rmDic[rm].ToString("#.##") + "";
                    }
                    else
                    {
                        rmstr = rm + " " + rmDic[rm].ToString("#.##") + "";
                    }
                }
                this.ShowWarningMessage("Order.Error.RMNotEnough", rmstr);
            }
            else
            {
                this.ucDetail.SaveCallBack("");

                this.TheOrderMgr.ReleaseOrder(this.OrderNo, this.CurrentUser, true);

                this.FV_Order.DataBind();

                ShowSuccessMessage("MasterData.Order.OrderHead.Submit.Successfully", this.OrderNo);

                UpdateView();

                UpdateLocTransAndActBillEvent(this.OrderNo, null);
            }
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }


    protected void btnForceSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            this.ucDetail.SaveCallBack("");

            this.TheOrderMgr.ReleaseOrder(this.OrderNo, this.CurrentUser, true);

            this.FV_Order.DataBind();

            ShowSuccessMessage("MasterData.Order.OrderHead.Submit.Successfully", this.OrderNo);

            UpdateView();

            UpdateLocTransAndActBillEvent(this.OrderNo, null);
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            this.TheOrderMgr.CancelOrder(this.OrderNo, this.CurrentUser);
            this.FV_Order.DataBind();
            UpdateView();
            ShowSuccessMessage("MasterData.Order.OrderHead.Cancel.Successfully", this.OrderNo);
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnStart_Click(object sender, EventArgs e)
    {
        try
        {
            this.TheOrderMgr.StartOrder(this.OrderNo, this.CurrentUser);
            this.FV_Order.DataBind();
            UpdateView();
            ShowSuccessMessage("MasterData.Order.OrderHead.Start.Successfully", this.OrderNo);
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }


    protected void btnReceive_Click(object sender, EventArgs e)
    {
        this.EscapeSortAndColorCheck = false;
        OrderHead orderHead = this.ucDetail.PopulateReceiveOrder();

        int detailCount = 0;
        bool allowExceedUC = bool.Parse(TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_ALLOW_EXCEED_UC).Value);

        foreach (OrderDetail orderDetail in orderHead.OrderDetails)
        {
            if (orderDetail.CurrentReceiveQty != 0 || orderDetail.CurrentRejectQty != 0 || orderDetail.CurrentScrapQty != 0)
            {
                if (!allowExceedUC && orderDetail.UnitCount < orderDetail.CurrentReceiveQty)
                {
                    ShowErrorMessage("OrderDetail.Error.OrderDetailReceiveQtyExceedUC", orderDetail.Sequence.ToString());
                    return;
                }
                else
                {
                    detailCount++;
                }
            }

        }
        if (detailCount == 0)
        {
            ShowErrorMessage("OrderDetail.Error.OrderDetailReceiveEmpty");
            return;
        }
        //每次只准收一个成品
        bool isReceiptOneItem = bool.Parse(TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_IS_RECEIPT_ONE_ITEM).Value);
        if (isReceiptOneItem && detailCount > 1)
        {
            ShowErrorMessage("OrderDetail.Error.OrderDetailReceiveItem");
            return;
        }

        if (orderHead.IsReceiptScanHu)
        {
            this.Session["Temp_Session_OrderNo"] = this.OrderNo;
            Response.Redirect("~/Main.aspx?mid=Order.GoodsReceipt__mp--ModuleType-Production");
        }
        else
        {
            bool hasOdd = false;
            if (orderHead.CreateHuOption == BusinessConstants.CODE_MASTER_CREATE_HU_OPTION_VALUE_GR)
            {
                foreach (OrderDetail orderDetail in orderHead.OrderDetails)
                {
                    if (orderDetail.CurrentReceiveQty % orderDetail.UnitCount != 0)
                    {
                        hasOdd = true;
                        break;
                    }
                }
            }
            this.ucConfirmInfo.Visible = true;
            this.ucConfirmInfo.InitPageParameter(orderHead, hasOdd, orderHead.IsOddCreateHu);
        }

    }


    protected void btnForceReceive_Click(object sender, EventArgs e)
    {
        this.EscapeSortAndColorCheck = true;
        OrderHead orderHead = this.ucDetail.PopulateReceiveOrder();

        int detailCount = 0;
        bool allowExceedUC = bool.Parse(TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_ALLOW_EXCEED_UC).Value);

        foreach (OrderDetail orderDetail in orderHead.OrderDetails)
        {
            if (orderDetail.CurrentReceiveQty != 0 || orderDetail.CurrentRejectQty != 0 || orderDetail.CurrentScrapQty != 0)
            {
                if (!allowExceedUC && orderDetail.UnitCount < orderDetail.CurrentReceiveQty)
                {
                    ShowErrorMessage("OrderDetail.Error.OrderDetailReceiveQtyExceedUC", orderDetail.Sequence.ToString());
                    return;
                }
                else
                {
                    detailCount++;
                }
            }

        }
        if (detailCount == 0)
        {
            ShowErrorMessage("OrderDetail.Error.OrderDetailReceiveEmpty");
            return;
        }
        //每次只准收一个成品
        bool isReceiptOneItem = bool.Parse(TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_IS_RECEIPT_ONE_ITEM).Value);
        if (isReceiptOneItem && detailCount > 1)
        {
            ShowErrorMessage("OrderDetail.Error.OrderDetailReceiveItem");
            return;
        }

        if (orderHead.IsReceiptScanHu)
        {
            this.Session["Temp_Session_OrderNo"] = this.OrderNo;
            Response.Redirect("~/Main.aspx?mid=Order.GoodsReceipt__mp--ModuleType-Production");
        }
        else
        {
            bool hasOdd = false;
            if (orderHead.CreateHuOption == BusinessConstants.CODE_MASTER_CREATE_HU_OPTION_VALUE_GR)
            {
                foreach (OrderDetail orderDetail in orderHead.OrderDetails)
                {
                    if (orderDetail.CurrentReceiveQty % orderDetail.UnitCount != 0)
                    {
                        hasOdd = true;
                        break;
                    }
                }
            }
            this.ucConfirmInfo.Visible = true;
            this.ucConfirmInfo.InitPageParameter(orderHead, hasOdd, orderHead.IsOddCreateHu);
        }

    }

    protected void btnComplete_Click(object sender, EventArgs e)
    {
        try
        {
            TheOrderMgr.ManualCompleteOrder(this.OrderNo, this.CurrentUser);
            this.FV_Order.DataBind();
            UpdateView();
            ShowSuccessMessage("MasterData.Order.OrderHead.Complete.Successfully", this.OrderNo);
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            TheOrderMgr.DeleteOrder(this.OrderNo, this.CurrentUser);

            if (this.BackEvent != null)
            {
                this.BackEvent(this, e);
                this.PageCleanup();
            }
            ShowSuccessMessage("MasterData.Order.OrderHead.Delete.Successfully", this.OrderNo);
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (this.BackEvent != null)
        {
            this.BackEvent(this, e);
            this.PageCleanup();
        }
    }

    private void SaveRender(object sender, EventArgs e)
    {
        OrderHead orderHead = (OrderHead)sender;

        TextBox tbExtOrderNo = (TextBox)this.FV_Order.FindControl("tbExtOrderNo");
        TextBox tbRefOrderNo = (TextBox)this.FV_Order.FindControl("tbRefOrderNo");
        TextBox tbMemo = (TextBox)this.FV_Order.FindControl("tbMemo");
        TextBox tbWinTime = (TextBox)this.FV_Order.FindControl("tbWinTime");
        TextBox tbStartTime = (TextBox)this.FV_Order.FindControl("tbStartTime");
        //CheckBox cbNeedSortAndColor = (CheckBox)this.FV_Order.FindControl("cbNeedSortAndColor");
        Controls_TextBox tbProductLineFacility = (Controls_TextBox)this.FV_Order.FindControl("tbProductLineFacility");

        if (tbExtOrderNo != null && tbExtOrderNo.Text.Trim() != string.Empty)
        {
            orderHead.ExternalOrderNo = tbExtOrderNo.Text.Trim();
        }
        if (tbRefOrderNo != null && tbRefOrderNo.Text.Trim() != string.Empty)
        {
            orderHead.ReferenceOrderNo = tbRefOrderNo.Text.Trim();
        }
        if (tbMemo != null && tbMemo.Text.Trim() != string.Empty)
        {
            orderHead.Memo = tbMemo.Text.Trim();
        }
        if (tbWinTime != null && tbWinTime.Text.Trim() != string.Empty)
        {
            orderHead.WindowTime = DateTime.Parse(tbWinTime.Text.Trim());
        }
        if (tbStartTime != null && tbStartTime.Text.Trim() != string.Empty)
        {
            orderHead.StartTime = DateTime.Parse(tbStartTime.Text.Trim());
        }
        //orderHead.NeedSortAndColor = cbNeedSortAndColor.Checked;
        if (tbProductLineFacility.Text != string.Empty)
        {
            orderHead.ProductLineFacility = tbProductLineFacility.Text;
        }
        else
        {
            orderHead.ProductLineFacility = null;
        }

        try
        {
            if (orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {
                TheOrderMgr.UpdateOrder(orderHead, this.CurrentUser, true);
            }
            else
            {
                orderHead.LastModifyDate = DateTime.Now;
                orderHead.LastModifyUser = this.CurrentUser;
                TheOrderHeadMgr.UpdateOrderHead(orderHead);
            }
            this.FV_Order.DataBind();
            UpdateView();
            this.UpdateLocTransAndActBillEvent(OrderNo, null);
            ShowSuccessMessage("MasterData.Order.OrderHead.Update.Successfully", this.OrderNo);
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    private void ShipRender(object sender, EventArgs e)
    {
        OrderHead orderHead = (OrderHead)sender;
        try
        {
            InProcessLocation inProcessLocation = TheOrderMgr.ShipOrder(orderHead.OrderDetails, this.CurrentUser);
            this.FV_Order.DataBind();
            UpdateView();
            ShowSuccessMessage("MasterData.Order.OrderHead.Update.Successfully", this.OrderNo);


        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }


    private void ReceiveRender(object sender, EventArgs e)
    {
        OrderHead orderHead = this.ucDetail.PopulateReceiveOrder();
        bool isOddCreateHu = (bool)sender;
        try
        {
            Receipt receipt = TheOrderMgr.ReceiveOrder(orderHead.OrderDetails, this.CurrentUser, isOddCreateHu, this.EscapeSortAndColorCheck);
            this.FV_Order.DataBind();
            UpdateView();
            UpdateLocTransAndActBillRender(this.OrderNo, e);
            ShowSuccessMessage("MasterData.Order.OrderHead.Receive.Successfully", this.OrderNo);

            #region print
            if (orderHead.AutoPrintHu)
            {
                if (receipt.HuTemplate == null || receipt.HuTemplate == string.Empty)
                {
                    ShowSuccessMessage("MasterData.Order.OrderHead.HuTemplate.Empty");
                    return;
                }
                IList<object> huDetailObj = new List<object>();
                IList<Hu> huList = new List<Hu>();
                foreach (ReceiptDetail receiptDetail in receipt.ReceiptDetails)
                {
                    if (receiptDetail.HuId != null && receiptDetail.HuId != string.Empty)
                    {
                        Hu hu = TheHuMgr.LoadHu(receiptDetail.HuId);
                        huList.Add(hu);
                    }
                }
                if (huList.Count > 0)
                {
                    huDetailObj.Add(huList);
                    huDetailObj.Add(CurrentUser.Code);

                   
                    string barCodeUrl = TheReportMgr.WriteToFile(receipt.HuTemplate, huDetailObj, receipt.HuTemplate);
                    Page.ClientScript.RegisterStartupScript(GetType(), "method", " <script language='javascript' type='text/javascript'>PrintOrder('" + barCodeUrl + "'); </script>");
                }
                }
            #endregion
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    private void UpdateLocTransAndActBillRender(object sender, EventArgs e)
    {
        UpdateLocTransAndActBillEvent(sender, e);
    }

    private void UpdateView()
    {
        if (this.OrderNo != null)
        {
            OrderHead orderHead = TheOrderHeadMgr.LoadOrderHead(this.OrderNo);
            string orderStatus = orderHead.Status;
            Controls_TextBox tbProductLineFacility = (Controls_TextBox)this.FV_Order.FindControl("tbProductLineFacility");
            tbProductLineFacility.Text = orderHead.ProductLineFacility;

            #region 根据订单状态显示按钮
            if (orderStatus == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {
                this.btnEdit.Visible = true;
                this.btnCheck.Visible = true;
                if (orderHead.CheckRM)
                {
                    this.btnSubmit.Visible = true;
                    this.btnForceSubmit.Visible = true;
                }
                else
                {
                    this.btnForceSubmit.Visible = true;
                    this.btnSubmit.Visible = false;
                }
                this.btnCancel.Visible = false;
                this.btnStart.Visible = false;
                this.btnReceive.Visible = false;
                this.btnForceReceive.Visible = false;
                this.btnComplete.Visible = false;
                this.btnDelete.Visible = true;
            }
            else if (orderStatus == BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT)
            {
                this.btnEdit.Visible = true;
                this.btnCheck.Visible = false;
                this.btnSubmit.Visible = false;
                this.btnForceSubmit.Visible = false;
                this.btnCancel.Visible = true;
                this.btnStart.Visible = true;
                this.btnReceive.Visible = false;
                this.btnForceReceive.Visible = false;
                this.btnComplete.Visible = false;
                this.btnDelete.Visible = false;
            }
            else if (orderStatus == BusinessConstants.CODE_MASTER_STATUS_VALUE_CANCEL)
            {
                this.btnEdit.Visible = false;
                this.btnCheck.Visible = false;
                this.btnSubmit.Visible = false;
                this.btnForceSubmit.Visible = false;
                this.btnCancel.Visible = false;
                this.btnStart.Visible = false;
                this.btnReceive.Visible = false;
                this.btnForceReceive.Visible = false;
                this.btnComplete.Visible = false;
                this.btnDelete.Visible = false;
            }
            else if (orderStatus == BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)
            {
                this.btnEdit.Visible = false;
                this.btnCheck.Visible = false;
                this.btnSubmit.Visible = false;
                this.btnForceSubmit.Visible = false;
                this.btnCancel.Visible = false;
                this.btnStart.Visible = false;
                this.btnReceive.Visible = true;
                this.btnForceReceive.Visible = true;
                this.btnComplete.Visible = true;
                this.btnDelete.Visible = false;
            }

            else if (orderStatus == BusinessConstants.CODE_MASTER_STATUS_VALUE_COMPLETE)
            {
                this.btnEdit.Visible = false;
                this.btnCheck.Visible = false;
                this.btnSubmit.Visible = false;
                this.btnForceSubmit.Visible = false;
                this.btnCancel.Visible = false;
                this.btnStart.Visible = false;
                this.btnReceive.Visible = false;
                this.btnForceReceive.Visible = false;
                this.btnComplete.Visible = false;
                this.btnDelete.Visible = false;
            }
            else if (orderStatus == BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)
            {
                this.btnEdit.Visible = false;
                this.btnCheck.Visible = false;
                this.btnSubmit.Visible = false;
                this.btnForceSubmit.Visible = false;
                this.btnCancel.Visible = false;
                this.btnStart.Visible = false;
                this.btnReceive.Visible = false;
                this.btnForceReceive.Visible = false;
                this.btnComplete.Visible = false;
                this.btnDelete.Visible = false;
            }
            #endregion


            //原材料报废
            if (this.IsScrap)
            {
                btnEdit.Visible = false;
                btnSubmit.Visible = false;
                btnForceSubmit.Visible = false;
                btnStart.Visible = false;
                btnCancel.Visible = false;
                btnExport.Visible = false;
                btnPrint.Visible = false;
                if (orderHead.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
                {
                    btnConfirm.Visible = true;
                }
                else
                {
                    btnConfirm.Visible = false;
                }
            }

            this.ucDetail.InitPageParameter(this.OrderNo);
        }
    }

    private void PageCleanup()
    {
        this.OrderNo = null;
    }

    protected void btnPrint_Click(object sender, EventArgs e)
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

    protected void btnExport_Click(object sender, EventArgs e)
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

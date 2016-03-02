using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Web;

public partial class Order_OrderHead_Edit : EditModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler UpdateLocTransAndActBillEvent;

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

    public void InitPageParameter(string orderNo)
    {
        this.OrderNo = orderNo;
        this.ODS_Order.SelectParameters["orderNo"].DefaultValue = this.OrderNo;
        this.UpdateView();
        OrderHead oH = TheOrderHeadMgr.LoadOrderHead(orderNo, true);

        #region 根据ModuleSubType状态调整显示的文字
        if (this.ModuleSubType == BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RTN)
        {
            this.btnShip.Text = "${Common.Button.Return}";
        }

        #endregion
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucDetail.SaveEvent += new System.EventHandler(this.SaveRender);
        this.ucDetail.ShipEvent += new System.EventHandler(this.ShipRender);

        this.ucDetail.UpdateLocTransAndActBillEvent += new System.EventHandler(this.UpdateLocTransAndActBillRender);

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

        UpdateLocTransAndActBillEvent(this.OrderNo, null);

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            this.ucDetail.SaveCallBack("");
            this.TheOrderMgr.ReleaseOrder(this.OrderNo, this.CurrentUser, true);
            this.FV_Order.DataBind();
            UpdateView();

            UpdateLocTransAndActBillEvent(this.OrderNo, null);

            //this.ShowSuccessMessage("订单" + this.OrderNo + "提交成功！");
            ShowSuccessMessage("MasterData.Order.OrderHead.Submit.Successfully", this.OrderNo);
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

    protected void btnShip_Click(object sender, EventArgs e)
    {
        OrderHead orderHead = TheOrderHeadMgr.LoadOrderHead(this.OrderNo);
        if (orderHead.IsShipScanHu)
        {
            this.Session["Temp_Session_OrderNo"] = this.OrderNo;
            Response.Redirect("~/Main.aspx?mid=Order.OrderIssue__mp--ModuleType-Distribution");
        }
        else
        {
            this.ucDetail.ShipCallBack();
            UpdateLocTransAndActBillEvent(this.OrderNo, null);
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

        try
        {
            TheOrderMgr.UpdateOrder(orderHead, this.CurrentUser, true);
            this.FV_Order.DataBind();
            UpdateView();
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
            TheOrderMgr.ShipOrder(orderHead.OrderDetails, this.CurrentUser);
            this.FV_Order.DataBind();
            UpdateView();
            ShowSuccessMessage("MasterData.Order.OrderHead.Ship.Successfully", this.OrderNo);

            //    if (inProcessLocation != null)
            //    {

            //        IList<object> huDetailObj = new List<object>();
            //        IList<HuDetail> huDetailList = new List<HuDetail>();
            //        IList<InProcessLocationDetail> inProcessLocationDetailList = inProcessLocation.InProcessLocationDetails;
            //        if (inProcessLocationDetailList != null && inProcessLocationDetailList.Count > 0)
            //        {
            //            foreach (InProcessLocationDetail inProcessLocationDetail in inProcessLocationDetailList)
            //            {
            //                HuDetail huDetail = TheHuDetailMgr.LoadHuDetail(inProcessLocationDetail.HuId, inProcessLocationDetail.LotNo, inProcessLocationDetail.OrderLocationTransaction.Item.Code);
            //                if (huDetail != null)
            //                {
            //                    huDetailList.Add(huDetail);
            //                }
            //            }
            //            if (huDetailList != null && huDetailList.Count > 0)
            //            {
            //                huDetailObj.Add(huDetailList);
            //                huDetailObj.Add(CurrentUser.Code);
            //                //TheReportBarCodeMgr.FillValues("BarCode.xls", huDetailObj);
            //                string barCodeUrl = TheReportMgr.WriteToFile("BarCode.xls", huDetailObj,"BarCode.xls");
            //                Page.ClientScript.RegisterStartupScript(GetType(), "method", " <script language='javascript' type='text/javascript'>PrintOrder('" + barCodeUrl + "'); </script>");
            //            }
            //        }
            //    }

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

            #region 只有Create状态，控件可以输入
            TextBox tbWinTime = (TextBox)this.FV_Order.FindControl("tbWinTime");
            TextBox tbStartTime = (TextBox)this.FV_Order.FindControl("tbStartTime");
            if (orderStatus != BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {
                TextBox tbExtOrderNo = (TextBox)this.FV_Order.FindControl("tbExtOrderNo");
                TextBox tbRefOrderNo = (TextBox)this.FV_Order.FindControl("tbRefOrderNo");
                TextBox tbMemo = (TextBox)this.FV_Order.FindControl("tbMemo");


                if (tbExtOrderNo != null)
                {
                    tbExtOrderNo.Attributes.Add("onfocus", "this.blur();");
                }
                if (tbRefOrderNo != null)
                {
                    tbRefOrderNo.Attributes.Add("onfocus", "this.blur();");
                }
                if (tbMemo != null)
                {
                    tbMemo.Attributes.Add("onfocus", "this.blur();");
                }
                if (tbWinTime != null)
                {
                    tbWinTime.Attributes.Add("onfocus", "this.blur();");
                }
                if (tbStartTime != null)
                {
                    tbStartTime.Attributes.Add("onfocus", "this.blur();");
                }
            }
            else
            {
                string userLanguage = this.CurrentUser.UserLanguage;
                tbWinTime.Attributes.Add("onclick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm',lang:'" + this.CurrentUser.UserLanguage + "'})");
                tbStartTime.Attributes.Add("onclick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm',lang:'" + this.CurrentUser.UserLanguage + "'})");

            }

            #endregion

            #region 根据订单状态显示按钮
            if (orderStatus == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {
                this.btnEdit.Visible = true;
                this.btnSubmit.Visible = true;
                this.btnCancel.Visible = false;
                this.btnStart.Visible = false;
                this.btnShip.Visible = false;
                this.btnComplete.Visible = false;
                this.btnDelete.Visible = true;
            }
            else if (orderStatus == BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT)
            {
                this.btnEdit.Visible = false;
                this.btnSubmit.Visible = false;
                this.btnCancel.Visible = true;
                this.btnStart.Visible = true;
                this.btnShip.Visible = false;
                this.btnComplete.Visible = false;
                this.btnDelete.Visible = false;
            }
            else if (orderStatus == BusinessConstants.CODE_MASTER_STATUS_VALUE_CANCEL)
            {
                this.btnEdit.Visible = false;
                this.btnSubmit.Visible = false;
                this.btnCancel.Visible = false;
                this.btnStart.Visible = false;
                this.btnShip.Visible = false;
                this.btnComplete.Visible = false;
                this.btnDelete.Visible = false;
            }
            else if (orderStatus == BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)
            {

                this.btnEdit.Visible = false;
                this.btnSubmit.Visible = false;
                this.btnCancel.Visible = false;
                this.btnStart.Visible = false;

                this.btnShip.Visible = orderHead.IsShipByOrder && !orderHead.IsShipScanHu;

                this.btnComplete.Visible = true;
                this.btnDelete.Visible = false;

            }

            else if (orderStatus == BusinessConstants.CODE_MASTER_STATUS_VALUE_COMPLETE)
            {
                this.btnEdit.Visible = false;
                this.btnSubmit.Visible = false;
                this.btnCancel.Visible = false;
                this.btnStart.Visible = false;
                this.btnShip.Visible = false;
                this.btnComplete.Visible = false;
                this.btnDelete.Visible = false;
            }
            else if (orderStatus == BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)
            {
                this.btnEdit.Visible = false;
                this.btnSubmit.Visible = false;
                this.btnCancel.Visible = false;
                this.btnStart.Visible = false;
                this.btnShip.Visible = false;
                this.btnComplete.Visible = false;
                this.btnDelete.Visible = false;
            }


            #endregion


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

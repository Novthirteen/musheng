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



public partial class Order_OrderHead_ScrapNew : NewModuleBase
{
    public event EventHandler CreateEvent;
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

    public void PageCleanup()
    {
        this.tbFlow.Text = string.Empty;
        this.tbRefOrderNo.Text = string.Empty;
        this.tbExtOrderNo.Text = string.Empty;

        this.CurrentFlowCode = null;

        this.ucList.PageCleanup();

        this.ucList.Visible = false;


    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucList.SaveEvent += new System.EventHandler(this.SaveRender);

        List<Flow> flowList = new List<Flow>();
        this.tbFlow.ServiceParameter = "string:" + this.CurrentUser.Code + ",bool:false,bool:false,bool:false,bool:true,bool:false,bool:false,string:"+BusinessConstants.PARTY_AUTHRIZE_OPTION_BOTH;

        if (!IsPostBack)
        {
            this.ucList.ModuleType = this.ModuleType;
            this.ucList.ModuleSubType = this.ModuleSubType;

        }
    }

    protected void tbFlow_TextChanged(Object sender, EventArgs e)
    {
        try
        {
            if (this.CurrentFlowCode == null || this.CurrentFlowCode != this.tbFlow.Text)
            {
                Flow currentFlow = TheFlowMgr.LoadFlow(tbFlow.Text, true);
                if (currentFlow != null)
                {
                    this.CurrentFlowCode = currentFlow.Code;
                    this.cbPrintOrder.Checked = currentFlow.NeedPrintOrder;
                    OrderHead orderHead = TheOrderMgr.TransferFlow2Order(currentFlow);
                    orderHead.SubType = this.ModuleSubType;
                    orderHead.WindowTime = DateTime.Now;


                    this.ddlShift.DataSource = TheShiftMgr.GetRegionShift(currentFlow.PartyTo.Code);
                    this.ddlShift.DataBind();

                    InitDetailParamater(orderHead);
                }

            }
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    private void InitDetailParamater(OrderHead orderHead)
    {
        Flow currentFlow = this.TheFlowMgr.LoadFlow(orderHead.Flow);

        if (!currentFlow.IsListDetail)
        {
            orderHead.OrderDetails = new List<OrderDetail>();
        }

        this.ucList.CleanPrice();

        this.ucList.NewItem = this.NewItem;
        this.ucList.InitPageParameter(orderHead);
        this.ucList.Visible = true;


    }


    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        this.ucList.SaveCallBack();
    }

    private void SaveRender(object sender, EventArgs e)
    {
        //创建订单
        try
        {
            OrderHead orderHead = CloneHelper.DeepClone<OrderHead>((OrderHead)sender);  //Clone：避免修改List Page的TheOrder，导致出错
            IList<OrderDetail> resultOrderDetailList = new List<OrderDetail>();

            if (orderHead.OrderDetails != null && orderHead.OrderDetails.Count > 0)
            {
                foreach (OrderDetail orderDetail in orderHead.OrderDetails)
                {
                    if (orderDetail.OrderedQty != 0)
                    {
                        if (!this.IsReuse)
                        {
                            string rejectLocationCode = orderDetail.DefaultRejectLocationFrom;
                            rejectLocationCode = this.ThePartyMgr.GetDefaultRejectLocation(orderHead.PartyFrom.Code, rejectLocationCode);
                            if (rejectLocationCode != null && rejectLocationCode.Trim() != string.Empty)
                            {
                                orderDetail.LocationFrom = this.TheLocationMgr.LoadLocation(rejectLocationCode);
                            }
                            orderDetail.LocationTo = null;
                        }
                        orderDetail.ScrapQty = orderDetail.OrderedQty;
                    }
                }
            }
            orderHead.WindowTime = DateTime.Now;
            orderHead.StartTime = DateTime.Now;

            orderHead.Priority = BusinessConstants.CODE_MASTER_ORDER_PRIORITY_VALUE_NORMAL;

            orderHead.ReferenceOrderNo = this.tbRefOrderNo.Text.Trim();
            orderHead.ExternalOrderNo = this.tbExtOrderNo.Text.Trim();
            orderHead.IsAutoRelease = false;
            orderHead.IsAutoStart = true;
          
            orderHead.IsAutoShip = false;
            orderHead.IsAutoReceive = false;
            orderHead.Shift = TheShiftMgr.LoadShift(ddlShift.SelectedValue);

            if (this.txProductLineFacility.Text.Trim() != string.Empty)
            {
                orderHead.ProductLineFacility = this.txProductLineFacility.Text.Trim();
            }

            TheOrderMgr.CreateOrder(orderHead, this.CurrentUser);

            #region 回用和报废更新数量
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

            if (this.cbPrintOrder.Checked)
            {

                IList<OrderDetail> orderDetails = orderHead.OrderDetails;
                IList<object> list = new List<object>();
                list.Add(orderHead);
                list.Add(orderDetails);

                IList<OrderLocationTransaction> orderLocationTransactions = TheOrderLocationTransactionMgr.GetOrderLocationTransaction(orderHead.OrderNo);
                list.Add(orderLocationTransactions);

                string printUrl = TheReportMgr.WriteToFile(orderHead.OrderTemplate, list);
                Page.ClientScript.RegisterStartupScript(GetType(), "method", " <script language='javascript' type='text/javascript'>PrintOrder('" + printUrl + "'); </script>");
            }
            this.ShowSuccessMessage("MasterData.Order.OrderHead.AddOrder.Successfully", orderHead.OrderNo);
            PageCleanup();
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

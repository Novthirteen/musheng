using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Procurement;
using com.Sconit.Entity;
using com.Sconit.Control;

public partial class ManageSconit_LeanEngine_Single_Order : ModuleBase
{
    public event EventHandler btnBackClick;

    private string FlowCode
    {
        get { return (string)ViewState["FlowCode"]; }
        set { ViewState["FlowCode"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }
    }

    public void InitPageParameter(string flowCode)
    {
        this.FlowCode = flowCode;
        this.Search(null, null, null);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string strategy = ((CodeMstrDropDownList)(this.FV_FormView.FindControl("ddlStrategy"))).SelectedValue;
        string windowTime = ((TextBox)(this.FV_FormView.FindControl("tbWindowTime"))).Text.Trim();
        string nextWindowTime = ((TextBox)(this.FV_FormView.FindControl("tbNextWindowTime"))).Text.Trim();
        DateTime winTime = DateTime.Now;
        DateTime? nextWinTime = null;
        try
        {
            winTime = DateTime.Parse(windowTime);
        }
        catch (Exception)
        {
            //winTime = DateTime.Now;
        }
        try
        {
            nextWinTime = DateTime.Parse(nextWindowTime);
        }
        catch (Exception)
        {
            //winTnextWinTimeime = DateTime.Now;
        }
        this.Search(strategy, winTime, nextWinTime);
        this.ddlStrategy_SelectedIndexChanged(null, null);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (btnBackClick != null)
        {
            btnBackClick(this, null);
        }
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        OrderHead orderHead = TheOrderMgr.TransferFlow2Order(this.FlowCode);
        TextBox tbWindowTime = (TextBox)this.FV_FormView.FindControl("tbWindowTime");
        orderHead.WindowTime = tbWindowTime.Text == string.Empty ? DateTime.Now : DateTime.Parse(tbWindowTime.Text);
        orderHead.SubType = BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML;
        orderHead.Priority = BusinessConstants.CODE_MASTER_ORDER_PRIORITY_VALUE_NORMAL;

        List<Transformer> transformers = this.ucTransformer.GetOrderTracersTransformer();

        foreach (Transformer transformer in transformers)
        {
            foreach (OrderDetail orderDetail in orderHead.OrderDetails)
            {
                if (orderDetail.FlowDetail.Id == transformer.Id)
                {
                    orderDetail.OrderedQty = transformer.CurrentQty;
                    orderDetail.OrderTracers = com.Sconit.Utility.TransformerHelper.TransformersDetails2OrderTracers(transformer.TransformerDetails, orderDetail);
                    if (orderDetail.OrderTracers == null)
                    {
                        orderDetail.OrderTracers = new List<OrderTracer>();
                    }
                    if (transformer.Qty != transformer.CurrentQty)
                    {
                        OrderTracer orderTracer = new OrderTracer();
                        orderTracer.TracerType = "Adj";
                        orderTracer.Item = orderDetail.Item.Code;
                        orderTracer.Qty = transformer.Qty - transformer.CurrentQty;
                        orderTracer.Code = orderDetail.DefaultLocationFrom == null ? orderDetail.DefaultLocationTo.Code : orderDetail.DefaultLocationFrom.Code;
                        orderTracer.ReqTime = DateTime.Now;
                        orderTracer.OrderDetail = orderDetail;
                        orderTracer.OrderedQty = transformer.Qty;
                        orderDetail.OrderTracers.Add(orderTracer);
                    }
                    orderDetail.RequiredQty = transformer.Qty;
                }
            }
        }
        TheOrderMgr.CreateOrder(orderHead, this.CurrentUser);
        transformers = com.Sconit.Utility.TransformerHelper.ConvertOrderHeadToTransformers(orderHead, false);
        this.ucTransformer.IncludeOrderTracers = false;
        this.ucTransformer.Visible = true;
        this.ucTransformer.InitPageParameter(transformers, orderHead.Type, null, false);
        ShowSuccessMessage("MasterData.Order.OrderHead.CreateOrder.Successfully", orderHead.OrderNo);
        #region 控制显示
        this.FV_FormView.FindControl("trOrder").Visible = true;
        ((Literal)this.FV_FormView.FindControl("tbOrderNo")).Text = orderHead.OrderNo;
        ((Literal)this.FV_FormView.FindControl("tbStatus")).Text = orderHead.Status;
        this.btnCreate.Visible = false;
        this.FV_FormView.FindControl("lblFlowStrategy").Visible = false;
        this.FV_FormView.FindControl("ddlStrategy").Visible = false;
        this.FV_FormView.FindControl("tbWindowTime").Visible = false;
        this.FV_FormView.FindControl("lblWindowTime").Visible = true;
        #endregion
    }

    protected void ddlStrategy_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strategy = ((CodeMstrDropDownList)(this.FV_FormView.FindControl("ddlStrategy"))).SelectedValue;
        if (strategy == BusinessConstants.CODE_MASTER_FLOW_STRATEGY_VALUE_JIT || strategy == BusinessConstants.CODE_MASTER_FLOW_STRATEGY_VALUE_MRP)
        {
            (this.FV_FormView.FindControl("ltlTo")).Visible = true;
            (this.FV_FormView.FindControl("tbNextWindowTime")).Visible = true;
        }
        else
        {
            (this.FV_FormView.FindControl("ltlTo")).Visible = false;
            (this.FV_FormView.FindControl("tbNextWindowTime")).Visible = false;
        }
    }

    private void Search(string strategy, DateTime? windowTime, DateTime? nextWindowTime)
    {
        OrderHead orderHead = TheLeanEngineMgr.PreviewGenOrder(this.FlowCode, strategy, windowTime, nextWindowTime);
        IList<Flow> flowList = new List<Flow>();
        List<Transformer> transformers = new List<Transformer>();
        Flow flow = TheFlowMgr.LoadFlow(this.FlowCode);

        if (orderHead != null)
        {
            orderHead.OrderDetails = orderHead.OrderDetails.OrderBy(o => o.Sequence).ThenBy(o => o.Item.Code).ToList();
            flow.NextWinTime = orderHead.WindowTime;
            if (strategy == BusinessConstants.CODE_MASTER_FLOW_STRATEGY_VALUE_KB)
            {
                MinDemandQty(orderHead);
            }
            transformers = com.Sconit.Utility.TransformerHelper.ConvertOrderHeadToTransformers(orderHead, true);
            foreach (Transformer transformer in transformers)
            {
                transformer.CurrentQty = transformer.Qty;
            }
            this.btnCreate.Visible = true;
        }
        else
        {
            ShowWarningMessage("LeanEngine.Detail.Empty");
            //flow.NextWinTime = DateTime.Now;
            this.btnCreate.Visible = false;
        }

        if (flow != null)
        {
            flowList.Add(flow);
        }

        this.ucTransformer.IncludeOrderTracers = true;
        this.ucTransformer.Visible = true;
        this.ucTransformer.InitPageParameter(transformers, flow.Type, BusinessConstants.TRANSFORMER_MODULE_TYPE_TRANSFER, false);

        this.FV_FormView.DataSource = flowList;
        this.FV_FormView.DataBind();
        #region 控制显示
        this.FV_FormView.FindControl("trOrder").Visible = false;
        this.FV_FormView.FindControl("lblFlowStrategy").Visible = true;
        this.FV_FormView.FindControl("ddlStrategy").Visible = true;
        this.FV_FormView.FindControl("tbWindowTime").Visible = true;
        this.FV_FormView.FindControl("lblWindowTime").Visible = false;
        #endregion

        ((CodeMstrDropDownList)(this.FV_FormView.FindControl("ddlStrategy"))).SelectedValue = strategy == null ? flow.FlowStrategy : strategy;
        ((TextBox)(this.FV_FormView.FindControl("tbWindowTime"))).Text = flow.NextWinTime.HasValue ? flow.NextWinTime.Value.ToString("yyyy-MM-dd HH:mm")
            : DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        //if (nextWindowTime.HasValue)
        //{
        //    ((TextBox)(this.FV_FormView.FindControl("tbNextWindowTime"))).Text = nextWindowTime.Value.ToString("yyyy-MM-dd HH:mm");
        //}
        if (orderHead != null)
        {
            if (orderHead.NextWindowTime.HasValue)
            {
                ((TextBox)(this.FV_FormView.FindControl("tbNextWindowTime"))).Text = orderHead.NextWindowTime.Value.ToString("yyyy-MM-dd HH:mm");
            }
            ((TextBox)(this.FV_FormView.FindControl("tbWindowTime"))).Text = orderHead.WindowTime.ToString("yyyy-MM-dd HH:mm");
        }
    }

    //春申客户化,最低送货量
    private void MinDemandQty(OrderHead orderHead)
    {
        if (orderHead != null && orderHead.OrderDetails != null)
        {
            foreach (OrderDetail od in orderHead.OrderDetails)
            {
                if (od.OrderTracers != null)
                {
                    foreach (OrderTracer ot in od.OrderTracers)
                    {
                        if (ot.TracerType == "OnhandInv" && ot.Qty < 0)
                        {
                            od.TextField1 = (-(ot.Qty)).ToString("0.########");
                        }
                    }
                }
            }
        }
    }

}

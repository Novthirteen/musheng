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
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using com.Sconit.Web;
using com.Sconit.Entity;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Service.Ext.Procurement;
using com.Sconit.Service.Ext.MasterData.Impl;
using System.Collections.Generic;

public partial class MasterData_Flow_New : NewModuleBase
{
    private Flow flow;
    public event EventHandler BackEvent;
    public event EventHandler CreateEvent;


    protected void Page_Load(object sender, EventArgs e)
    {
        Controls_TextBox tbRefFlow = (Controls_TextBox)this.FV_Flow.FindControl("tbRefFlow");
        Controls_TextBox tbPartyFrom = (Controls_TextBox)this.FV_Flow.FindControl("tbPartyFrom");
        Controls_TextBox tbPartyTo = (Controls_TextBox)this.FV_Flow.FindControl("tbPartyTo");
        Controls_TextBox tbRejectLocationTo = (Controls_TextBox)this.FV_Flow.FindControl("tbRejectLocationTo");
        Controls_TextBox tbInspectLocationTo = (Controls_TextBox)this.FV_Flow.FindControl("tbInspectLocationTo");

        tbPartyFrom.ServiceParameter = "string:" + BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT + ",string:" + this.CurrentUser.Code;
        tbPartyFrom.DataBind();
        tbPartyTo.ServiceParameter = "string:" + BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT + ",string:" + this.CurrentUser.Code;
        tbPartyTo.DataBind();
        tbRefFlow.ServiceParameter = "string:" + this.CurrentUser.Code;
        tbRefFlow.DataBind();

        tbInspectLocationTo.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:" + BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_INSPECT + ",bool:false";
        tbInspectLocationTo.DataBind();

        tbRejectLocationTo.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:" + BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_REJECT + ",bool:false";
        tbRejectLocationTo.DataBind();
        if (!IsPostBack)
        {
            BillSettleTermDataBind();
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void ODS_Flow_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        flow = (Flow)e.InputParameters[0];

        Controls_TextBox tbRefFlow = (Controls_TextBox)this.FV_Flow.FindControl("tbRefFlow");
        Controls_TextBox tbPartyFrom = (Controls_TextBox)this.FV_Flow.FindControl("tbPartyFrom");
        Controls_TextBox tbPartyTo = (Controls_TextBox)this.FV_Flow.FindControl("tbPartyTo");
        Controls_TextBox tbLocTo = (Controls_TextBox)this.FV_Flow.FindControl("tbLocTo");
        Controls_TextBox tbShipFrom = (Controls_TextBox)this.FV_Flow.FindControl("tbShipFrom");
        Controls_TextBox tbShipTo = (Controls_TextBox)this.FV_Flow.FindControl("tbShipTo");
        Controls_TextBox tbBillAddress = (Controls_TextBox)this.FV_Flow.FindControl("tbBillAddress");
        Controls_TextBox tbCarrier = (Controls_TextBox)this.FV_Flow.FindControl("tbCarrier");
        Controls_TextBox tbCarrierBillAddress = (Controls_TextBox)this.FV_Flow.FindControl("tbCarrierBillAddress");
        Controls_TextBox tbCurrency = (Controls_TextBox)this.FV_Flow.FindControl("tbCurrency");
        Controls_TextBox tbPriceList = (Controls_TextBox)this.FV_Flow.FindControl("tbPriceList");
        Controls_TextBox tbRejectLocationTo = (Controls_TextBox)this.FV_Flow.FindControl("tbRejectLocationTo");
        Controls_TextBox tbInspectLocationTo = (Controls_TextBox)this.FV_Flow.FindControl("tbInspectLocationTo");
        com.Sconit.Control.CodeMstrDropDownList ddlGrGapTo = (com.Sconit.Control.CodeMstrDropDownList)this.FV_Flow.FindControl("ddlGrGapTo");
        com.Sconit.Control.CodeMstrDropDownList ddlOrderTemplate = (com.Sconit.Control.CodeMstrDropDownList)(this.FV_Flow.FindControl("ddlOrderTemplate"));
        com.Sconit.Control.CodeMstrDropDownList ddlAsnTemplate = (com.Sconit.Control.CodeMstrDropDownList)(this.FV_Flow.FindControl("ddlAsnTemplate"));
        com.Sconit.Control.CodeMstrDropDownList ddlReceiptTemplate = (com.Sconit.Control.CodeMstrDropDownList)(this.FV_Flow.FindControl("ddlReceiptTemplate"));
        com.Sconit.Control.CodeMstrDropDownList ddlHuTemplate = (com.Sconit.Control.CodeMstrDropDownList)(this.FV_Flow.FindControl("ddlHuTemplate"));
        DropDownList ddlBillSettleTerm = (DropDownList)this.FV_Flow.FindControl("ddlBillSettleTerm");
        com.Sconit.Control.CodeMstrDropDownList ddlCreateHuOption = (com.Sconit.Control.CodeMstrDropDownList)this.FV_Flow.FindControl("ddlCreateHuOption");

        if (tbRefFlow != null && tbRefFlow.Text.Trim() != string.Empty)
        {
            flow.ReferenceFlow = TheFlowMgr.CheckAndLoadFlow(tbRefFlow.Text.Trim()).Code;
        }

        if (tbPartyFrom != null && tbPartyFrom.Text.Trim() != string.Empty)
        {
            flow.PartyFrom = ThePartyMgr.LoadParty(tbPartyFrom.Text.Trim());
        }

        if (tbPartyTo != null && tbPartyTo.Text.Trim() != string.Empty)
        {
            flow.PartyTo = ThePartyMgr.LoadParty(tbPartyTo.Text.Trim());
        }

        if (tbInspectLocationTo != null && tbInspectLocationTo.Text.Trim() != string.Empty)
        {
            Location inspectLocation = TheLocationMgr.LoadLocation(tbInspectLocationTo.Text.Trim());
            if (inspectLocation.Region.CostGroup != ((Region)flow.PartyTo).CostGroup)
            {
                ShowErrorMessage("MasterData.Flow.InspectLocationTo.CostGroup.Error");
                e.Cancel = true;
            }
            flow.InspectLocationTo = tbInspectLocationTo.Text.Trim();
        }
        else
        {
            string inspectLocation = ((Region)flow.PartyTo).InspectLocation;
            if (inspectLocation == null || inspectLocation == string.Empty)
            {
                ShowErrorMessage("MasterData.Flow.InspectLocationTo.Empty");
                e.Cancel = true;
            }
            flow.InspectLocationTo = inspectLocation;

        }

        if (tbRejectLocationTo != null && tbRejectLocationTo.Text.Trim() != string.Empty)
        {
            Location rejectLocation = TheLocationMgr.LoadLocation(tbRejectLocationTo.Text.Trim());
            if (rejectLocation.Region.CostGroup != ((Region)flow.PartyTo).CostGroup)
            {
                ShowErrorMessage("MasterData.Flow.RejectLocationTo.CostGroup.Error");
                e.Cancel = true;
            }
            flow.RejectLocationTo = tbRejectLocationTo.Text.Trim();
        }
        else
        {
            string regionRejectLocation = ((Region)flow.PartyTo).RejectLocation;
            if (regionRejectLocation == null || regionRejectLocation == string.Empty)
            {
                ShowErrorMessage("MasterData.Flow.RejectLocationTo.Empty");
                e.Cancel = true;
            }
            flow.RejectLocationTo = regionRejectLocation;
        }
        if (tbLocTo != null && tbLocTo.Text.Trim() != string.Empty)
        {
            flow.LocationTo = TheLocationMgr.LoadLocation(tbLocTo.Text.Trim());
        }

        if (tbShipFrom != null && tbShipFrom.Text.Trim() != string.Empty)
        {
            flow.ShipFrom = TheAddressMgr.LoadShipAddress(tbShipFrom.Text.Trim());
        }

        if (tbShipTo != null && tbShipTo.Text.Trim() != string.Empty)
        {
            flow.ShipTo = TheAddressMgr.LoadShipAddress(tbShipTo.Text.Trim());
        }

        if (tbBillAddress != null && tbBillAddress.Text.Trim() != string.Empty)
        {
            flow.BillAddress = TheAddressMgr.LoadBillAddress(tbBillAddress.Text.Trim());
        }
        if (tbCarrier != null && tbCarrier.Text.Trim() != string.Empty)
        {
            flow.Carrier = TheSupplierMgr.LoadSupplier(tbCarrier.Text.Trim());
        }
        if (tbCarrierBillAddress != null && tbCarrierBillAddress.Text.Trim() != string.Empty)
        {
            flow.CarrierBillAddress = TheAddressMgr.LoadBillAddress(tbCarrierBillAddress.Text.Trim());
        }
        if (ddlGrGapTo != null && ddlGrGapTo.SelectedIndex != -1)
        {
            flow.GoodsReceiptGapTo = ddlGrGapTo.SelectedValue;
        }

        if (ddlOrderTemplate.SelectedIndex != -1)
        {
            flow.OrderTemplate = ddlOrderTemplate.SelectedValue;
        }
        if (ddlAsnTemplate.SelectedIndex != -1)
        {
            flow.AsnTemplate = ddlAsnTemplate.SelectedValue;
        }
        if (ddlReceiptTemplate.SelectedIndex != -1)
        {
            flow.ReceiptTemplate = ddlReceiptTemplate.SelectedValue;
        }
        if (ddlHuTemplate.SelectedIndex != -1)
        {
            flow.HuTemplate = ddlHuTemplate.SelectedValue;
        }
        if (tbPriceList != null && tbPriceList.Text.Trim() != string.Empty)
        {
            flow.PriceList = ThePurchasePriceListMgr.LoadPurchasePriceList(tbPriceList.Text.Trim());
        }
        if (ddlBillSettleTerm.SelectedIndex != -1)
        {
            flow.BillSettleTerm = ddlBillSettleTerm.SelectedValue;
        }
        if (ddlCreateHuOption.SelectedIndex != -1)
        {
            flow.CreateHuOption = ddlCreateHuOption.SelectedValue;
        }
        if (tbCurrency != null && tbCurrency.Text.Trim() != string.Empty)
        {
            flow.Currency = TheCurrencyMgr.LoadCurrency(tbCurrency.Text.Trim());
        }
        else
        {
            string currencyCode = TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_BASE_CURRENCY).Value;
            flow.Currency = TheCurrencyMgr.LoadCurrency(currencyCode);
        }

        flow.Type = BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT;
        flow.CheckDetailOption = BusinessConstants.CODE_MASTER_CHECK_ORDER_DETAIL_OPTION_VALUE_NOT_CHECK;
        flow.CreateUser = this.CurrentUser;
        flow.CreateDate = DateTime.Now;
        flow.LastModifyUser = this.CurrentUser;
        flow.LastModifyDate = DateTime.Now;
    }

    protected void ODS_Flow_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (CreateEvent != null)
        {
            CreateEvent(flow.Code, e);
            ShowSuccessMessage("MasterData.Flow.AddFlow.Successfully", flow.Code);
        }
    }

    protected void checkFlowExists(object source, ServerValidateEventArgs args)
    {
        String flowCode = ((TextBox)(this.FV_Flow.FindControl("tbCode"))).Text;
        if (TheFlowMgr.LoadFlow(flowCode) != null)
        {
            args.IsValid = false;
        }

    }

    public void PageCleanup()
    {
        ((Controls_TextBox)this.FV_Flow.FindControl("tbRefFlow")).Text = string.Empty;
        ((Controls_TextBox)this.FV_Flow.FindControl("tbPartyFrom")).Text = string.Empty;
        ((Controls_TextBox)this.FV_Flow.FindControl("tbPartyTo")).Text = string.Empty;
        ((Controls_TextBox)this.FV_Flow.FindControl("tbLocTo")).Text = string.Empty;
        ((Controls_TextBox)this.FV_Flow.FindControl("tbShipFrom")).Text = string.Empty;
        ((Controls_TextBox)this.FV_Flow.FindControl("tbShipTo")).Text = string.Empty;
        ((Controls_TextBox)this.FV_Flow.FindControl("tbBillAddress")).Text = string.Empty;
        ((Controls_TextBox)this.FV_Flow.FindControl("tbCarrier")).Text = string.Empty;
        ((Controls_TextBox)this.FV_Flow.FindControl("tbCarrierBillAddress")).Text = string.Empty;

        ((Controls_TextBox)this.FV_Flow.FindControl("tbPriceList")).Text = string.Empty;
        ((TextBox)(this.FV_Flow.FindControl("tbCode"))).Text = string.Empty;
        ((TextBox)(this.FV_Flow.FindControl("tbDescription"))).Text = string.Empty;
        ((TextBox)(this.FV_Flow.FindControl("tbDockDescription"))).Text = string.Empty;
        ((CheckBox)(this.FV_Flow.FindControl("cbIsActive"))).Checked = true;
        ((CheckBox)(this.FV_Flow.FindControl("cbIsAutoCreate"))).Checked = false;
        ((CheckBox)(this.FV_Flow.FindControl("cbIsAutoRelease"))).Checked = false;
        ((CheckBox)(this.FV_Flow.FindControl("cbIsAutoStart"))).Checked = false;
        ((CheckBox)(this.FV_Flow.FindControl("cbNeedPrintOrder"))).Checked = false;
        ((CheckBox)(this.FV_Flow.FindControl("cbNeedPrintASN"))).Checked = false;
        ((CheckBox)(this.FV_Flow.FindControl("cbNeedPrintReceipt"))).Checked = true;
        ((CheckBox)(this.FV_Flow.FindControl("cbAllowExceed"))).Checked = false;
        ((CheckBox)(this.FV_Flow.FindControl("cbIsAutoBill"))).Checked = false;
        ((CheckBox)(this.FV_Flow.FindControl("cbIsAutoShip"))).Checked = false;
        ((CheckBox)(this.FV_Flow.FindControl("cbIsAutoReceive"))).Checked = false;
        ((CheckBox)(this.FV_Flow.FindControl("cbIsListDetail"))).Checked = true;
        ((CheckBox)(this.FV_Flow.FindControl("cbIsAsnUniqueReceipt"))).Checked = true;
        ((Controls_TextBox)(this.FV_Flow.FindControl("tbCurrency"))).Text = TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_BASE_CURRENCY).Value;
        ((CheckBox)(this.FV_Flow.FindControl("cbAllowCreateDetail"))).Checked = false;
        ((CheckBox)(this.FV_Flow.FindControl("cbFulfillUC"))).Checked = true;
        ((com.Sconit.Control.CodeMstrDropDownList)this.FV_Flow.FindControl("ddlGrGapTo")).DataBind();
        ((DropDownList)this.FV_Flow.FindControl("ddlBillSettleTerm")).SelectedIndex = 0;
        ((com.Sconit.Control.CodeMstrDropDownList)this.FV_Flow.FindControl("ddlCreateHuOption")).DataBind();
    }

    private void BillSettleTermDataBind()
    {
        DropDownList ddlBillSettleTerm = (DropDownList)this.FV_Flow.FindControl("ddlBillSettleTerm");
        ddlBillSettleTerm.DataSource = this.GetBillSettleTermGroup();
        ddlBillSettleTerm.DataBind();
    }
    private IList<CodeMaster> GetBillSettleTermGroup()
    {
        IList<CodeMaster> billSettleTermGroup = new List<CodeMaster>();

        billSettleTermGroup.Add(new CodeMaster()); //添加空选项
        billSettleTermGroup.Add(GetBillSettleTerm(BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_RECEIVING_SETTLEMENT));
        billSettleTermGroup.Add(GetBillSettleTerm(BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_ONLINE_BILLING));
        billSettleTermGroup.Add(GetBillSettleTerm(BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_LINEAR_CLEARING));
        billSettleTermGroup.Add(GetBillSettleTerm(BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_INSPECTION));

        return billSettleTermGroup;
    }
    private CodeMaster GetBillSettleTerm(string billSettleTermValue)
    {
        return TheCodeMasterMgr.GetCachedCodeMaster(BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM, billSettleTermValue);
    }
}

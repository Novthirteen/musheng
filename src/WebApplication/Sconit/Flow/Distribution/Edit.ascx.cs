﻿using System;
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
using com.Sconit.Entity.MasterData;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Web;
using com.Sconit.Entity;
using com.Sconit.Service.Ext.MasterData.Impl;
using com.Sconit.Service.Ext.Procurement;
using com.Sconit.Service.Ext.Distribution;

public partial class MasterData_Flow_Edit : EditModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler UpdateViewEvent;

    protected string FlowCode
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
    private string[] EditFields = new string[]
    {
        "Description",
        "ReferenceFlow",
        "DockDescription",
        "IsAutoCreate",
        "IsAutoRelease",
        "IsAutoStart",
        "IsAutoShip",
        "IsAutoReceive",
        "NeedPrintAsn",
        "IsAutoComplete",
        "IsAutoBill",
        "PartyFrom",
        "PartyTo",
        "LocationFrom",
        "LocationTo",
        "ShipFrom",
        "ShipTo",
        "BillAddress",
        "IsActive",
        "LastModifyUser",
        "LastModifyDate",
        "Carrier",
        "CarrierBillAddress",
        "NeedPrintOrder",
        "NeedPrintReceipt",
        "AllowExceed",
        "PriceList",
        "OrderTemplate",
        "AsnTemplate",
        "ReceiptTemplate",
        "Hutemplate",
        "IsShowPrice",
        "IsListDetail",
        "CheckDetailOption",
        "Currency",
        "AllowCreateDetail",
        "BillSettleTerm",
        "FulfillUnitCount",
        "IsShipScanHu",
        "IsReceiptScanHu",
        "AutoPrintHu",
        "IsOddCreateHu",
        "CreateHuOption",
        "BillSettleTerm",
        "IsAutoCreatePickList",
        "MaxOnlineQty",
        "NeedInspection",
        "IsPickFromBin",
        "IsShipByOrder",
        "IsAsnUniqueReceipt",
        "RejectLocationFrom",
        "InspectLocationFrom",
    };

    public void InitPageParameter(string flowCode)
    {
        this.FlowCode = flowCode;
        this.ODS_Flow.SelectParameters["code"].DefaultValue = FlowCode;
        this.ODS_Flow.DeleteParameters["code"].DefaultValue = FlowCode;

    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            this.FlowCode = null;
            BackEvent(this, e);
        }
    }

    protected void FV_Flow_DataBound(object sender, EventArgs e)
    {
        if (FlowCode != null && FlowCode != string.Empty)
        {
            Flow flow = (Flow)((FormView)sender).DataItem;
            UpdateView(flow);
        }
    }

    protected void ODS_Flow_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        Flow flow = (Flow)e.InputParameters[0];
        Flow oldFlow = TheFlowMgr.LoadFlow(FlowCode);
        CloneHelper.CopyProperty(oldFlow, flow, EditFields, true);

        Controls_TextBox tbRefFlow = (Controls_TextBox)this.FV_Flow.FindControl("tbRefFlow");
        Controls_TextBox tbPartyFrom = (Controls_TextBox)this.FV_Flow.FindControl("tbPartyFrom");
        Controls_TextBox tbPartyTo = (Controls_TextBox)this.FV_Flow.FindControl("tbPartyTo");
        Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_Flow.FindControl("tbLocFrom");
        Controls_TextBox tbShipFrom = (Controls_TextBox)this.FV_Flow.FindControl("tbShipFrom");
        Controls_TextBox tbShipTo = (Controls_TextBox)this.FV_Flow.FindControl("tbShipTo");
        Controls_TextBox tbBillAddress = (Controls_TextBox)this.FV_Flow.FindControl("tbBillAddress");
        Controls_TextBox tbCarrier = (Controls_TextBox)this.FV_Flow.FindControl("tbCarrier");
        Controls_TextBox tbCarrierBillAddress = (Controls_TextBox)this.FV_Flow.FindControl("tbCarrierBillAddress");
        Controls_TextBox tbCurrency = (Controls_TextBox)this.FV_Flow.FindControl("tbCurrency");
        Controls_TextBox tbPriceList = (Controls_TextBox)this.FV_Flow.FindControl("tbPriceList");
        Controls_TextBox tbRejectLocationFrom = (Controls_TextBox)this.FV_Flow.FindControl("tbRejectLocationFrom");

        com.Sconit.Control.CodeMstrDropDownList ddlGrGapTo = (com.Sconit.Control.CodeMstrDropDownList)this.FV_Flow.FindControl("ddlGrGapTo");
        com.Sconit.Control.CodeMstrDropDownList ddlCheckDetailOption = (com.Sconit.Control.CodeMstrDropDownList)(this.FV_Flow.FindControl("ddlCheckDetailOption"));
        com.Sconit.Control.CodeMstrDropDownList ddlOrderTemplate = (com.Sconit.Control.CodeMstrDropDownList)(this.FV_Flow.FindControl("ddlOrderTemplate"));
        com.Sconit.Control.CodeMstrDropDownList ddlAsnTemplate = (com.Sconit.Control.CodeMstrDropDownList)(this.FV_Flow.FindControl("ddlAsnTemplate"));
        com.Sconit.Control.CodeMstrDropDownList ddlReceiptTemplate = (com.Sconit.Control.CodeMstrDropDownList)(this.FV_Flow.FindControl("ddlReceiptTemplate"));
        com.Sconit.Control.CodeMstrDropDownList ddlHuTemplate = (com.Sconit.Control.CodeMstrDropDownList)(this.FV_Flow.FindControl("ddlHuTemplate"));
        
        com.Sconit.Control.CodeMstrDropDownList ddlCreateHuOption = (com.Sconit.Control.CodeMstrDropDownList)this.FV_Flow.FindControl("ddlCreateHuOption");
        DropDownList ddlBillSettleTerm = (DropDownList)this.FV_Flow.FindControl("ddlBillSettleTerm");
        DropDownList ddlMRPOption = (DropDownList)this.FV_Flow.FindControl("ddlMRPOption");

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
        if (tbLocFrom != null && tbLocFrom.Text.Trim() != string.Empty)
        {
            flow.LocationFrom = TheLocationMgr.LoadLocation(tbLocFrom.Text.Trim());
        }

        if (tbRejectLocationFrom != null && tbRejectLocationFrom.Text.Trim() != string.Empty)
        {
            Location rejectLocation = TheLocationMgr.LoadLocation(tbRejectLocationFrom.Text.Trim());
            if (rejectLocation.Region.CostGroup != ((Region)flow.PartyFrom).CostGroup)
            {
                ShowErrorMessage("MasterData.Flow.RejectLocationFrom.CostGroup.Error");
                e.Cancel = true;
            }
            flow.RejectLocationFrom = tbRejectLocationFrom.Text.Trim();
        }
        else
        {
            string regionRejectLocation = ((Region)flow.PartyFrom).RejectLocation;
            if (regionRejectLocation == null || regionRejectLocation == string.Empty)
            {
                ShowErrorMessage("MasterData.Flow.RejectLocationTo.Empty");
                e.Cancel = true;
            }
            flow.RejectLocationFrom = regionRejectLocation;
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
        if (ddlBillSettleTerm.SelectedIndex != -1)
        {
            flow.BillSettleTerm = ddlBillSettleTerm.SelectedValue;
        }
        if (ddlMRPOption.SelectedIndex != -1)
        {
            flow.MRPOption = ddlMRPOption.SelectedValue;
        }
        if (tbCarrier != null && tbCarrier.Text.Trim() != string.Empty)
        {
            flow.Carrier = TheSupplierMgr.LoadSupplier(tbCarrier.Text.Trim());
        }
        if (tbCarrierBillAddress != null && tbCarrierBillAddress.Text.Trim() != string.Empty)
        {
            flow.CarrierBillAddress = TheAddressMgr.LoadBillAddress(tbCarrierBillAddress.Text.Trim());
        }
        if (ddlGrGapTo.SelectedIndex != -1)
        {
            flow.GoodsReceiptGapTo = ddlGrGapTo.SelectedValue;
        }
        if (ddlCheckDetailOption.SelectedIndex != -1)
        {
            flow.CheckDetailOption = ddlCheckDetailOption.SelectedValue;
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
        if (ddlCreateHuOption.SelectedIndex != -1)
        {
            flow.CreateHuOption = ddlCreateHuOption.SelectedValue;
        }
        if (tbPriceList != null && tbPriceList.Text.Trim() != string.Empty)
        {
            flow.PriceList = TheSalesPriceListMgr.LoadSalesPriceList(tbPriceList.Text.Trim());
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

        flow.LastModifyUser = this.CurrentUser;
        flow.LastModifyDate = DateTime.Now;
    }

    protected void ODS_Flow_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {

        UpdateViewEvent(this.FlowCode, e);
        ShowSuccessMessage("MasterData.Flow.UpdateFlow.Successfully", this.FlowCode);
    }

    protected void ODS_Flow_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception == null)
        {
            btnBack_Click(this, e);
            ShowSuccessMessage("MasterData.Flow.DeleteFlow.Successfully", this.FlowCode);
        }
        else if (e.Exception.InnerException is Castle.Facilities.NHibernateIntegration.DataException)
        {
            ShowErrorMessage("MasterData.Flow.DeleteFlow.Failed", this.FlowCode);
            e.ExceptionHandled = true;
        }
    }

    private void UpdateView(Flow flow)
    {

        Controls_TextBox tbRefFlow = (Controls_TextBox)this.FV_Flow.FindControl("tbRefFlow");
        Controls_TextBox tbPartyFrom = (Controls_TextBox)this.FV_Flow.FindControl("tbPartyFrom");
        Controls_TextBox tbPartyTo = (Controls_TextBox)this.FV_Flow.FindControl("tbPartyTo");
        Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_Flow.FindControl("tbLocFrom");
        Controls_TextBox tbShipFrom = (Controls_TextBox)this.FV_Flow.FindControl("tbShipFrom");
        Controls_TextBox tbShipTo = (Controls_TextBox)this.FV_Flow.FindControl("tbShipTo");
        Controls_TextBox tbBillAddress = (Controls_TextBox)this.FV_Flow.FindControl("tbBillAddress");
        Controls_TextBox tbCarrier = (Controls_TextBox)this.FV_Flow.FindControl("tbCarrier");
        Controls_TextBox tbCarrierBillAddress = (Controls_TextBox)this.FV_Flow.FindControl("tbCarrierBillAddress");
        com.Sconit.Control.CodeMstrDropDownList ddlGrGapTo = (com.Sconit.Control.CodeMstrDropDownList)this.FV_Flow.FindControl("ddlGrGapTo");
        Controls_TextBox tbPriceList = (Controls_TextBox)this.FV_Flow.FindControl("tbPriceList");
        Controls_TextBox tbCurrency = (Controls_TextBox)this.FV_Flow.FindControl("tbCurrency");
        Controls_TextBox tbRejectLocationFrom = (Controls_TextBox)this.FV_Flow.FindControl("tbRejectLocationFrom");
        Literal lblCheckDetailOption = (Literal)this.FV_Flow.FindControl("lblCheckDetailOption");

        com.Sconit.Control.CodeMstrDropDownList ddlCheckDetailOption = (com.Sconit.Control.CodeMstrDropDownList)(this.FV_Flow.FindControl("ddlCheckDetailOption"));
        com.Sconit.Control.CodeMstrDropDownList ddlOrderTemplate = (com.Sconit.Control.CodeMstrDropDownList)(this.FV_Flow.FindControl("ddlOrderTemplate"));
        com.Sconit.Control.CodeMstrDropDownList ddlAsnTemplate = (com.Sconit.Control.CodeMstrDropDownList)(this.FV_Flow.FindControl("ddlAsnTemplate"));
        com.Sconit.Control.CodeMstrDropDownList ddlReceiptTemplate = (com.Sconit.Control.CodeMstrDropDownList)(this.FV_Flow.FindControl("ddlReceiptTemplate"));
        com.Sconit.Control.CodeMstrDropDownList ddlHuTemplate = (com.Sconit.Control.CodeMstrDropDownList)(this.FV_Flow.FindControl("ddlHuTemplate"));
       
        com.Sconit.Control.CodeMstrDropDownList ddlCreateHuOption = (com.Sconit.Control.CodeMstrDropDownList)this.FV_Flow.FindControl("ddlCreateHuOption");
        DropDownList ddlBillSettleTerm = (DropDownList)this.FV_Flow.FindControl("ddlBillSettleTerm");
        DropDownList ddlMRPOption = (DropDownList)this.FV_Flow.FindControl("ddlMRPOption");

        tbPartyFrom.ServiceParameter = "string:" + BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION + ",string:" + this.CurrentUser.Code;
        tbPartyFrom.DataBind();
        tbPartyTo.ServiceParameter = "string:" + BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION + ",string:" + this.CurrentUser.Code;
        tbPartyTo.DataBind();
       
        tbRefFlow.ServiceParameter = "string:" + this.CurrentUser.Code;
        tbRefFlow.DataBind();

        tbRejectLocationFrom.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:" + BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_REJECT + ",bool:false";
        tbRejectLocationFrom.DataBind();

        BillSettleTermDataBind();

        MRPOptionDataBind();

        if (flow.ReferenceFlow != null)
        {
            tbRefFlow.Text = flow.ReferenceFlow;
        }
        if (flow.PartyFrom != null)
        {
            tbPartyFrom.Text = flow.PartyFrom.Code;
        }
        if (flow.PartyTo != null)
        {
            tbPartyTo.Text = flow.PartyTo.Code;
        }
        if (flow.LocationFrom != null)
        {
            tbLocFrom.Text = flow.LocationFrom.Code;
        }
        if (flow.ShipFrom != null)
        {
            tbShipFrom.Text = flow.ShipFrom.Code;
        }
        if (flow.ShipTo != null)
        {
            tbShipTo.Text = flow.ShipTo.Code;
        }
        if (flow.BillAddress != null)
        {
            tbBillAddress.Text = flow.BillAddress.Code;
        }
        if (flow.BillSettleTerm != null)
        {
            ddlBillSettleTerm.SelectedValue = flow.BillSettleTerm;
        }
        if (flow.MRPOption != null)
        {
            ddlMRPOption.SelectedValue = flow.MRPOption;
        }
        if (flow.Carrier != null)
        {
            tbCarrier.Text = flow.Carrier.Code;
        }
        if (flow.CarrierBillAddress != null)
        {
            tbCarrierBillAddress.Text = flow.CarrierBillAddress.Code;
        }
        if (flow.GoodsReceiptGapTo != null)
        {
            ddlGrGapTo.SelectedValue = flow.GoodsReceiptGapTo;
        }
      
        if (flow.OrderTemplate != null)
        {
            ddlOrderTemplate.SelectedValue = flow.OrderTemplate;
        }
        if (flow.AsnTemplate != null)
        {
            ddlAsnTemplate.SelectedValue = flow.AsnTemplate;
        }
        if (flow.ReceiptTemplate != null)
        {
            ddlReceiptTemplate.SelectedValue = flow.ReceiptTemplate;
        }
        if (flow.HuTemplate != null)
        {
            ddlHuTemplate.SelectedValue = flow.HuTemplate;
        }
        if (flow.CreateHuOption != null)
        {
            ddlCreateHuOption.SelectedValue = flow.CreateHuOption;
        }
        if (flow.CheckDetailOption != null)
        {
            ddlCheckDetailOption.SelectedValue = flow.CheckDetailOption;
        }
        if (flow.PriceList != null)
        {
            tbPriceList.Text = flow.PriceList.Code;
        }
        if (flow.Currency != null)
        {
            tbCurrency.Text = flow.Currency.Code;
        }
        if (flow.RejectLocationFrom != null)
        {
            tbRejectLocationFrom.Text = flow.RejectLocationFrom;
        }
    }

    private void MRPOptionDataBind()
    {
        DropDownList ddlMRPOption = (DropDownList)this.FV_Flow.FindControl("ddlMRPOption");
        ddlMRPOption.DataSource = this.GetMRPOptionDataBindGroup();
        ddlMRPOption.DataBind();
    }

    private void BillSettleTermDataBind()
    {
        DropDownList ddlBillSettleTerm = (DropDownList)this.FV_Flow.FindControl("ddlBillSettleTerm");
        ddlBillSettleTerm.DataSource = this.GetBillSettleTermGroup();
        ddlBillSettleTerm.DataBind();
    }


    private IList<CodeMaster> GetMRPOptionDataBindGroup()
    {
        IList<CodeMaster> mrpOptionGroup = new List<CodeMaster>();

        mrpOptionGroup.Add(GetMRPOption(BusinessConstants.CODE_MASTER_MRP_OPTION_VALUE_ORDER_BEFORE_PLAN));
        mrpOptionGroup.Add(GetMRPOption(BusinessConstants.CODE_MASTER_MRP_OPTION_VALUE_PLAN_ONLY));
        mrpOptionGroup.Add(GetMRPOption(BusinessConstants.CODE_MASTER_MRP_OPTION_VALUE_ORDER_ONLY));

        return mrpOptionGroup;
    }

    private IList<CodeMaster> GetBillSettleTermGroup()
    {
        IList<CodeMaster> billSettleTermGroup = new List<CodeMaster>();

        billSettleTermGroup.Add(GetBillSettleTerm(BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_RECEIVING_SETTLEMENT));
        billSettleTermGroup.Add(GetBillSettleTerm(BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_CONSIGNMENT));

        return billSettleTermGroup;
    }

    private CodeMaster GetMRPOption(string mrpOptionValue)
    {
        return TheCodeMasterMgr.GetCachedCodeMaster(BusinessConstants.CODE_MASTER_MRP_OPTION, mrpOptionValue);
    }

    private CodeMaster GetBillSettleTerm(string billSettleTermValue)
    {
        return TheCodeMasterMgr.GetCachedCodeMaster(BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM, billSettleTermValue);
    }

    protected void CheckAllItem(object source, ServerValidateEventArgs args)
    {
        Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_Flow.FindControl("tbLocFrom");
        Controls_TextBox tbRefFlow = (Controls_TextBox)this.FV_Flow.FindControl("tbRefFlow");
        CustomValidator cvCheckDetailItem = (CustomValidator)this.FV_Flow.FindControl("cvCheckDetailItem");
        if (tbRefFlow != null && tbRefFlow.Text.Trim() != string.Empty)
        {


            Flow flow = TheFlowMgr.LoadFlow(FlowCode);

            List<FlowDetail> flowDetailList = (List<FlowDetail>)TheFlowDetailMgr.GetFlowDetail(flow);
            List<FlowDetail> refFlowDetailList = (List<FlowDetail>)TheFlowDetailMgr.GetFlowDetail(tbRefFlow.Text.Trim());
            if (!FlowHelper.CheckDetailSeqExists(flowDetailList, refFlowDetailList))
            {
                args.IsValid = false;
                ShowErrorMessage("MasterData.Flow.RefFlow.Sequence.Exists");
                return;
            };


            List<string[]> itemList = new List<string[]>();
            foreach (FlowDetail flowDetail in flowDetailList)
            {
                string[] item = new string[5];
                item[0] = flowDetail.Item.Code;
                item[1] = flowDetail.Uom.Code;
                item[2] = flowDetail.DefaultLocationFrom != null ? flowDetail.DefaultLocationFrom.Code : string.Empty;
                item[3] = string.Empty;
                item[4] = flowDetail.UnitCount.ToString();
                itemList.Add(item);
            }

            foreach (FlowDetail refFlowDetail in refFlowDetailList)
            {
                string[] item = new string[5];
                item[0] = refFlowDetail.Item.Code;
                item[1] = refFlowDetail.Uom.Code;
                item[2] = refFlowDetail.LocationFrom != null ? refFlowDetail.LocationFrom.Code : tbLocFrom.Text.Trim();
                item[3] = string.Empty;
                item[4] = refFlowDetail.UnitCount.ToString();
                if (!FlowHelper.CheckDetailItemExists(itemList, item))
                {
                    args.IsValid = false;
                    ShowErrorMessage("MasterData.Flow.FlowDetail.Item.Code.Exists", item[0]);
                }
                else
                {
                    itemList.Add(item);

                }
            }

        }

    }
   

}

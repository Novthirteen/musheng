using System;
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
        "HuTemplate",
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
        "NeedInspection",
        "MaxOnlineQty",
        "AllowRepeatlyExceed",
        "IsMRP"
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

        Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_Flow.FindControl("tbLocFrom");
        Controls_TextBox tbLocTo = (Controls_TextBox)this.FV_Flow.FindControl("tbLocTo");

        Controls_TextBox tbRejectLocationFrom = (Controls_TextBox)this.FV_Flow.FindControl("tbRejectLocationFrom");
        Controls_TextBox tbRejectLocationTo = (Controls_TextBox)this.FV_Flow.FindControl("tbRejectLocationTo");
        Controls_TextBox tbInspectLocationFrom = (Controls_TextBox)this.FV_Flow.FindControl("tbInspectLocationFrom");
        Controls_TextBox tbInspectLocationTo = (Controls_TextBox)this.FV_Flow.FindControl("tbInspectLocationTo");

        com.Sconit.Control.CodeMstrDropDownList ddlOrderTemplate = (com.Sconit.Control.CodeMstrDropDownList)(this.FV_Flow.FindControl("ddlOrderTemplate"));
        com.Sconit.Control.CodeMstrDropDownList ddlReceiptTemplate = (com.Sconit.Control.CodeMstrDropDownList)(this.FV_Flow.FindControl("ddlReceiptTemplate"));
        com.Sconit.Control.CodeMstrDropDownList ddlHuTemplate = (com.Sconit.Control.CodeMstrDropDownList)(this.FV_Flow.FindControl("ddlHuTemplate"));
        com.Sconit.Control.CodeMstrDropDownList ddlCreateHuOption = (com.Sconit.Control.CodeMstrDropDownList)this.FV_Flow.FindControl("ddlCreateHuOption");


        if (tbRefFlow != null && tbRefFlow.Text.Trim() != string.Empty)
        {
            flow.ReferenceFlow = TheFlowMgr.CheckAndLoadFlow(tbRefFlow.Text.Trim()).Code;
        }
        if (tbPartyFrom != null && tbPartyFrom.Text.Trim() != string.Empty)
        {
            flow.PartyFrom = ThePartyMgr.LoadParty(tbPartyFrom.Text.Trim());
            flow.PartyTo = flow.PartyFrom;
        }
        if (tbLocFrom != null && tbLocFrom.Text.Trim() != string.Empty)
        {
            flow.LocationFrom = TheLocationMgr.LoadLocation(tbLocFrom.Text.Trim());
        }
        if (tbLocTo != null && tbLocTo.Text.Trim() != string.Empty)
        {
            flow.LocationTo = TheLocationMgr.LoadLocation(tbLocTo.Text.Trim());
        }
        if (ddlOrderTemplate.SelectedIndex != -1)
        {
            flow.OrderTemplate = ddlOrderTemplate.SelectedValue;
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
                ShowErrorMessage("MasterData.Flow.RejectLocationFrom.Empty");
                e.Cancel = true;
            }
            flow.RejectLocationFrom = regionRejectLocation;
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

        if (tbInspectLocationFrom != null && tbInspectLocationFrom.Text.Trim() != string.Empty)
        {
            Location inspectLocation = TheLocationMgr.LoadLocation(tbInspectLocationFrom.Text.Trim());
            if (inspectLocation.Region.CostGroup != ((Region)flow.PartyFrom).CostGroup)
            {
                ShowErrorMessage("MasterData.Flow.RejectLocationFrom.CostGroup.Error");
                e.Cancel = true;
            }
            flow.InspectLocationFrom = tbInspectLocationFrom.Text.Trim();
        }
        else
        {
            string regionInspectLocation = ((Region)flow.PartyFrom).InspectLocation;
            if (regionInspectLocation == null || regionInspectLocation == string.Empty)
            {
                ShowErrorMessage("MasterData.Flow.InspectLocationFrom.Empty");
                e.Cancel = true;
            }
            flow.InspectLocationFrom = regionInspectLocation;
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
            string regionInspectLocation = ((Region)flow.PartyTo).InspectLocation;
            if (regionInspectLocation == null || regionInspectLocation == string.Empty)
            {
                ShowErrorMessage("MasterData.Flow.InspectLocationTo.Empty");
                e.Cancel = true;
            }
            flow.InspectLocationTo = regionInspectLocation;
        }
        flow.BillSettleTerm = null;
        flow.CheckDetailOption = BusinessConstants.CODE_MASTER_CHECK_ORDER_DETAIL_OPTION_VALUE_NOT_CHECK;
      
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

        Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_Flow.FindControl("tbLocFrom");
        Controls_TextBox tbLocTo = (Controls_TextBox)this.FV_Flow.FindControl("tbLocTo");

        Controls_TextBox tbRejectLocationFrom = (Controls_TextBox)this.FV_Flow.FindControl("tbRejectLocationFrom");
        Controls_TextBox tbRejectLocationTo = (Controls_TextBox)this.FV_Flow.FindControl("tbRejectLocationTo");

        Controls_TextBox tbInspectLocationFrom = (Controls_TextBox)this.FV_Flow.FindControl("tbInspectLocationFrom");
        Controls_TextBox tbInspectLocationTo = (Controls_TextBox)this.FV_Flow.FindControl("tbInspectLocationTo");

      
        com.Sconit.Control.CodeMstrDropDownList ddlOrderTemplate = (com.Sconit.Control.CodeMstrDropDownList)(this.FV_Flow.FindControl("ddlOrderTemplate"));
        com.Sconit.Control.CodeMstrDropDownList ddlReceiptTemplate = (com.Sconit.Control.CodeMstrDropDownList)(this.FV_Flow.FindControl("ddlReceiptTemplate"));
        com.Sconit.Control.CodeMstrDropDownList ddlHuTemplate = (com.Sconit.Control.CodeMstrDropDownList)(this.FV_Flow.FindControl("ddlHuTemplate"));
        
        com.Sconit.Control.CodeMstrDropDownList ddlCreateHuOption = (com.Sconit.Control.CodeMstrDropDownList)this.FV_Flow.FindControl("ddlCreateHuOption");
        
        tbPartyFrom.ServiceParameter = "string:" + BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION + ",string:" + this.CurrentUser.Code;
        tbPartyFrom.DataBind();

        tbRejectLocationFrom.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:" + BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_REJECT + ",bool:false";
        tbRejectLocationFrom.DataBind();

        tbRejectLocationTo.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:" + BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_REJECT + ",bool:false";
        tbRejectLocationTo.DataBind();

        tbInspectLocationFrom.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:" + BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_INSPECT + ",bool:false";
        tbInspectLocationFrom.DataBind();

        tbInspectLocationTo.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:" + BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_INSPECT + ",bool:false";
        tbInspectLocationTo.DataBind();

        #region 生产类型flow不限制库位
        tbLocFrom.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:";
        tbLocFrom.DataBind();
        tbLocTo.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:";
        tbLocTo.DataBind();
        #endregion


        tbRefFlow.ServiceParameter = "string:" + this.CurrentUser.Code;
        tbRefFlow.DataBind();

        if (flow.ReferenceFlow != null && flow.ReferenceFlow.Trim() != string.Empty)
        {
            tbRefFlow.Text = flow.ReferenceFlow;
        }
        if (flow.PartyFrom != null)
        {
            tbPartyFrom.Text = flow.PartyFrom.Code;
        }

        if (flow.LocationFrom != null)
        {
            tbLocFrom.Text = flow.LocationFrom.Code;
        }
        if (flow.LocationTo != null)
        {
            tbLocTo.Text = flow.LocationTo.Code;
        }
        if (flow.OrderTemplate != null)
        {
            ddlOrderTemplate.SelectedValue = flow.OrderTemplate;
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

        if (flow.RejectLocationFrom != null)
        {
            tbRejectLocationFrom.Text = flow.RejectLocationFrom;
        }
        if (flow.RejectLocationTo != null)
        {
            tbRejectLocationTo.Text = flow.RejectLocationTo;
        }

        if (flow.InspectLocationFrom != null)
        {
            tbInspectLocationFrom.Text = flow.InspectLocationFrom;
        }
        if (flow.InspectLocationTo != null)
        {
            tbInspectLocationTo.Text = flow.InspectLocationTo;
        }
    }


    protected void CheckAllItem(object source, ServerValidateEventArgs args)
    {
        Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_Flow.FindControl("tbLocFrom");
        Controls_TextBox tbLocTo = (Controls_TextBox)this.FV_Flow.FindControl("tbLocTo");
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
                item[3] = flowDetail.DefaultLocationTo != null ? flowDetail.DefaultLocationTo.Code : string.Empty;
                item[5] = flowDetail.UnitCount.ToString();
                itemList.Add(item);
            }

            foreach (FlowDetail refFlowDetail in refFlowDetailList)
            {
                string[] item = new string[5];
                item[0] = refFlowDetail.Item.Code;
                item[1] = refFlowDetail.Uom.Code;
                item[2] = refFlowDetail.LocationFrom != null ? refFlowDetail.LocationFrom.Code : tbLocFrom.Text.Trim();
                item[3] = refFlowDetail.LocationTo != null ? refFlowDetail.LocationTo.Code : tbLocTo.Text.Trim();
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

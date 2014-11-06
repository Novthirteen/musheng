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
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Procurement;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Entity;
using NHibernate.Expression;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Utility;

public partial class MasterData_FlowDetail_Edit : EditModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler EditEvent;
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

    public string FlowCode
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

    public int FlowDetailId
    {
        get
        {
            if (ViewState["FlowDetailId"] == null)
            {
                return 0;
            }
            else
            {
                return (Int32)ViewState["FlowDetailId"];
            }
        }
        set
        {
            ViewState["FlowDetailId"] = value;
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    protected void FV_FlowDetail_DataBound(object sender, EventArgs e)
    {
        if (FlowDetailId == 0) return;

        FlowDetail flowDetail = TheFlowDetailMgr.LoadFlowDetail(FlowDetailId);
        Flow flow = flowDetail.Flow;

        ((Controls_TextBox)(this.FV_FlowDetail.FindControl("tbItemCode"))).Text = flowDetail.Item.Code;
        if (flowDetail.Uom != null)
        {
            ((Controls_TextBox)(this.FV_FlowDetail.FindControl("tbUom"))).Text = flowDetail.Uom.Code;
        }

        Controls_TextBox tbRefItemCode = (Controls_TextBox)(this.FV_FlowDetail.FindControl("tbRefItemCode"));
        tbRefItemCode.ServiceParameter = "string:#tbItemCode,string:" + flow.PartyFrom.Code + ",string:" + flow.PartyTo.Code;
        tbRefItemCode.DataBind();



        if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT
            || this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS
            || this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING)
        {
            this.FV_FlowDetail.FindControl("fdProcurement").Visible = true;
            Controls_TextBox tbLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbProcurementLocTo");
            tbLocTo.ServiceParameter = "string:" + this.CurrentUser.Code;
            tbLocTo.DataBind();
            tbLocTo.Text = flowDetail.LocationTo == null ? string.Empty : flowDetail.LocationTo.Code;

            if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT
                || this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING)
            {
                BillSettleTermDataBind();
                DropDownList ddlBillSettleTerm = (DropDownList)this.FV_FlowDetail.FindControl("ddlBillSettleTerm");
                ddlBillSettleTerm.Text = flowDetail.BillSettleTerm;

                ddlBillSettleTerm.Visible = true;
                this.FV_FlowDetail.FindControl("lblBillSettleTerm").Visible = true;

                ((Literal)this.FV_FlowDetail.FindControl("lblNeedInspect")).Visible = true;
                ((CheckBox)(this.FV_FlowDetail.FindControl("cbNeedInspect"))).Visible = true;


                Literal lblIdMark = ((Literal)this.FV_FlowDetail.FindControl("lblIdMark"));
                TextBox tbIdMark = ((TextBox)this.FV_FlowDetail.FindControl("tbIdMark"));
                Literal lblBarCodeType = ((Literal)this.FV_FlowDetail.FindControl("lblBarCodeType"));
                com.Sconit.Control.CodeMstrDropDownList ddlBarCodeType = (com.Sconit.Control.CodeMstrDropDownList)this.FV_FlowDetail.FindControl("ddlBarCodeType");

                lblIdMark.Visible = true;
                tbIdMark.Visible = true;
                lblBarCodeType.Visible = true;
                ddlBarCodeType.Visible = true;
                if (flowDetail.BarCodeType != string.Empty)
                {
                    ddlBarCodeType.Text = flowDetail.BarCodeType;
                }

                Controls_TextBox tbRejectLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbProcurementRejectLocTo");
                tbRejectLocTo.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:" + BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_REJECT + ",bool:false";
                tbRejectLocTo.DataBind();
                tbRejectLocTo.Text = flowDetail.RejectLocationTo;

                Controls_TextBox tbInspectLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbProcurementInspectLocTo");
                tbInspectLocTo.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:" + BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_INSPECT + ",bool:false";
                tbInspectLocTo.DataBind();
                tbInspectLocTo.Text = flowDetail.InspectLocationTo;
            }
        }

        else if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION)
        {
            BillSettleTermDataBind();
            DropDownList ddlBillSettleTerm = (DropDownList)this.FV_FlowDetail.FindControl("ddlBillSettleTerm");
            ddlBillSettleTerm.Text = flowDetail.BillSettleTerm;
            ddlBillSettleTerm.Visible = true;
            this.FV_FlowDetail.FindControl("lblBillSettleTerm").Visible = true;

            DropDownList ddlOddShipOption = (DropDownList)this.FV_FlowDetail.FindControl("ddlOddShipOption");
            ddlOddShipOption.Text = flowDetail.OddShipOption;
            ddlOddShipOption.Visible = true;
            this.FV_FlowDetail.FindControl("lblOddShipOption").Visible = true;

            this.FV_FlowDetail.FindControl("fdDistribution").Visible = true;
            Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbDistributionLocFrom");
            tbLocFrom.ServiceParameter = "string:" + flow.PartyFrom.Code;
            tbLocFrom.DataBind();
            tbLocFrom.Text = flowDetail.LocationFrom == null ? string.Empty : flowDetail.LocationFrom.Code;

            Controls_TextBox tbRejectLocFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbDistributionRejectLocFrom");
            tbRejectLocFrom.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:" + BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_REJECT + ",bool:false";
            tbRejectLocFrom.DataBind();
            tbRejectLocFrom.Text = flowDetail.RejectLocationFrom;

         
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION)
        {
            this.FV_FlowDetail.FindControl("fdProduction").Visible = true;
            this.FV_FlowDetail.FindControl("trBom").Visible = true;
            this.FV_FlowDetail.FindControl("lblCustomer").Visible = true;
            this.FV_FlowDetail.FindControl("lblCustomerItemCode").Visible = true;
            this.FV_FlowDetail.FindControl("tbCustomerItemCode").Visible = true;
            ((Controls_TextBox)this.FV_FlowDetail.FindControl("tbBom")).Text = flowDetail.Bom == null ? string.Empty : flowDetail.Bom.Code;
            ((TextBox)(this.FV_FlowDetail.FindControl("tbBatchSize"))).Text = string.Empty;
            Controls_TextBox tbCustomer = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbCustomer");
            tbCustomer.ServiceParameter = "string:" + BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS + ",string:" + this.CurrentUser.Code;
            tbCustomer.DataBind();
            tbCustomer.Visible = true;
            tbCustomer.Text = flowDetail.Customer == null ? string.Empty : flowDetail.Customer.Code;
            Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbProductionLocFrom");
            tbLocFrom.ServiceParameter = "string:" + this.CurrentUser.Code;
            tbLocFrom.DataBind();
            tbLocFrom.Text = flowDetail.LocationFrom == null ? string.Empty : flowDetail.LocationFrom.Code;
            Controls_TextBox tbLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbProductionLocTo");
            tbLocTo.ServiceParameter = "string:" + this.CurrentUser.Code;
            tbLocTo.DataBind();
            tbLocTo.Text = flowDetail.LocationTo == null ? string.Empty : flowDetail.LocationTo.Code;

            ((Literal)this.FV_FlowDetail.FindControl("lblNeedInspect")).Visible = true;
            ((CheckBox)(this.FV_FlowDetail.FindControl("cbNeedInspect"))).Visible = true;

            Literal lblIdMark = ((Literal)this.FV_FlowDetail.FindControl("lblIdMark"));
            TextBox tbIdMark = ((TextBox)this.FV_FlowDetail.FindControl("tbIdMark"));
            Literal lblBarCodeType = ((Literal)this.FV_FlowDetail.FindControl("lblBarCodeType"));
            com.Sconit.Control.CodeMstrDropDownList ddlBarCodeType = (com.Sconit.Control.CodeMstrDropDownList)this.FV_FlowDetail.FindControl("ddlBarCodeType");

            lblIdMark.Visible = true;
            tbIdMark.Visible = true;
            lblBarCodeType.Visible = true;
            ddlBarCodeType.Visible = true;
            lblIdMark.Text = "${MasterData.Flow.FlowDetail.IdMark.Production}";
            ddlBarCodeType.Code = "FGBarCodeType";
            ddlBarCodeType.DataBind();
            if (flowDetail.BarCodeType != string.Empty)
            {
                ddlBarCodeType.Text = flowDetail.BarCodeType;
            }

            Controls_TextBox tbRouting = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbRouting");
            tbRouting.ServiceParameter = "string:" + BusinessConstants.CODE_MASTER_ROUTING_TYPE_VALUE_PRODUCTION;
            tbRouting.DataBind();
            if (flowDetail.Routing != null)
            {
                tbRouting.Text = flowDetail.Routing.Code;
            }

            Controls_TextBox tbReturnRouting = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbReturnRouting");
            tbReturnRouting.ServiceParameter = "string:" + BusinessConstants.CODE_MASTER_ROUTING_TYPE_VALUE_REWORK;
            tbReturnRouting.DataBind();
            if (flowDetail.ReturnRouting != null)
            {
                tbReturnRouting.Text = flowDetail.ReturnRouting.Code;
            }


            Controls_TextBox tbRejectLocationFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbRejectLocationFrom");
            Controls_TextBox tbRejectLocationTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbRejectLocationTo");

            Controls_TextBox tbInspectLocationFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbInspectLocationFrom");
            Controls_TextBox tbInspectLocationTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbInspectLocationTo");

            tbRejectLocationFrom.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:" + BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_REJECT + ",bool:false";
            tbRejectLocationFrom.DataBind();
            tbRejectLocationTo.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:" + BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_REJECT + ",bool:false";
            tbRejectLocationTo.DataBind();
            tbInspectLocationFrom.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:" + BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_INSPECT + ",bool:false";
            tbInspectLocationFrom.DataBind();
            tbInspectLocationTo.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:" + BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_INSPECT + ",bool:false";
            tbInspectLocationTo.DataBind();
            if (flowDetail.RejectLocationFrom != null)
            {
                tbRejectLocationFrom.Text = flowDetail.RejectLocationFrom;
            }
            if (flowDetail.RejectLocationTo != null)
            {
                tbRejectLocationTo.Text = flowDetail.RejectLocationTo;
            }

            if (flowDetail.InspectLocationFrom != null)
            {
                tbInspectLocationFrom.Text = flowDetail.InspectLocationFrom;
            }
            if (flowDetail.InspectLocationTo != null)
            {
                tbInspectLocationTo.Text = flowDetail.InspectLocationTo;
            }
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_TRANSFER)
        {
            this.FV_FlowDetail.FindControl("fdTransfer").Visible = true;

            DropDownList ddlOddShipOption = (DropDownList)this.FV_FlowDetail.FindControl("ddlOddShipOption");
            ddlOddShipOption.Text = flowDetail.OddShipOption;
            ddlOddShipOption.Visible = true;
            this.FV_FlowDetail.FindControl("lblOddShipOption").Visible = true;

            Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbTransferLocFrom");
            tbLocFrom.ServiceParameter = "string:" + flow.PartyFrom.Code;
            tbLocFrom.DataBind();
            tbLocFrom.Text = flowDetail.LocationFrom == null ? string.Empty : flowDetail.LocationFrom.Code;
            Controls_TextBox tbLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbTransferLocTo");
            tbLocTo.ServiceParameter = "string:" + flow.PartyTo.Code;
            tbLocTo.DataBind();
            tbLocTo.Text = flowDetail.LocationTo == null ? string.Empty : flowDetail.LocationTo.Code;

            Controls_TextBox tbRejectLocFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbTransferRejectLocFrom");
            tbRejectLocFrom.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:" + BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_REJECT + ",bool:false";
            tbRejectLocFrom.DataBind();
            tbRejectLocFrom.Text = flowDetail.RejectLocationFrom;

            Controls_TextBox tbRejectLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbTransferRejectLocTo");
            tbRejectLocTo.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:" + BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_REJECT + ",bool:false";
            tbRejectLocTo.DataBind();
            tbRejectLocTo.Text = flowDetail.RejectLocationTo;
        }
    }

    public void InitPageParameter(Int32 flowDetailId)
    {
        this.FlowDetailId = flowDetailId;
        this.ODS_FlowDetail.SelectParameters["id"].DefaultValue = flowDetailId.ToString();
        this.ODS_FlowDetail.DeleteParameters["id"].DefaultValue = flowDetailId.ToString();
        this.FV_FlowDetail.DataBind();
    }

    protected void ODS_FlowDetail_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {

        FlowDetail flowDetail = (FlowDetail)e.InputParameters[0];
        Flow flow = TheFlowMgr.LoadFlow(FlowCode, true);
        flowDetail.Flow = flow;

        //seq
        if (flowDetail.Sequence == 0)
        {
            flowDetail.Sequence = TheFlowDetailMgr.LoadFlowDetail(this.FlowDetailId).Sequence;
        }
        Controls_TextBox tbItemCode = (Controls_TextBox)(this.FV_FlowDetail.FindControl("tbItemCode"));
        Controls_TextBox tbUom = (Controls_TextBox)(this.FV_FlowDetail.FindControl("tbUom"));
        if (tbItemCode != null && tbItemCode.Text.Trim() != string.Empty)
        {
            flowDetail.Item = TheItemMgr.LoadItem(tbItemCode.Text.Trim());
        }

        if (tbUom != null && tbUom.Text.Trim() != string.Empty)
        {
            flowDetail.Uom = TheUomMgr.LoadUom(tbUom.Text.Trim());
        }

        if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT
            || this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS
            || this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING)
        {
            Controls_TextBox tbLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbProcurementLocTo");
            if (tbLocTo != null && tbLocTo.Text.Trim() != string.Empty)
            {
                flowDetail.LocationTo = TheLocationMgr.LoadLocation(tbLocTo.Text.Trim());
            }
            if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT)
            {
                DropDownList ddlBillSettleTerm = (DropDownList)this.FV_FlowDetail.FindControl("ddlBillSettleTerm");
                if (ddlBillSettleTerm.SelectedIndex != -1)
                {
                    if (ddlBillSettleTerm.SelectedValue == string.Empty)
                    {
                        flowDetail.BillSettleTerm = null;
                    }
                    else
                    {
                        flowDetail.BillSettleTerm = ddlBillSettleTerm.SelectedValue;
                    }
                }
                com.Sconit.Control.CodeMstrDropDownList ddlBarCodeType = (com.Sconit.Control.CodeMstrDropDownList)this.FV_FlowDetail.FindControl("ddlBarCodeType");
                if (ddlBarCodeType.SelectedIndex != -1)
                {
                    flowDetail.BarCodeType = ddlBarCodeType.SelectedValue;
                }
                Controls_TextBox tbRejectLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbProcurementRejectLocTo");
                if (tbRejectLocTo.Text.Trim() != string.Empty)
                {
                    flowDetail.RejectLocationTo = tbRejectLocTo.Text.Trim();
                }


                Controls_TextBox tbInspectLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbProcurementInspectLocTo");
                if (tbInspectLocTo.Text.Trim() != string.Empty)
                {
                    flowDetail.InspectLocationTo = tbInspectLocTo.Text.Trim();
                }

            }

        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION)
        {
            Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbDistributionLocFrom");
            if (tbLocFrom != null && tbLocFrom.Text.Trim() != string.Empty)
            {
                flowDetail.LocationFrom = TheLocationMgr.LoadLocation(tbLocFrom.Text.Trim());
            }
            DropDownList ddlBillSettleTerm = (DropDownList)this.FV_FlowDetail.FindControl("ddlBillSettleTerm");
            if (ddlBillSettleTerm.SelectedIndex != -1)
            {
                flowDetail.BillSettleTerm = ddlBillSettleTerm.SelectedValue;
            }

            DropDownList ddlOddShipOption = (DropDownList)this.FV_FlowDetail.FindControl("ddlOddShipOption");
            if (ddlOddShipOption.SelectedIndex != -1)
            {

                flowDetail.OddShipOption = ddlOddShipOption.SelectedValue;

            }

            Controls_TextBox tbRejectLocFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbDistributionRejectLocFrom");
            if (tbRejectLocFrom.Text.Trim() != string.Empty)
            {
                flowDetail.RejectLocationFrom = tbRejectLocFrom.Text.Trim();
            }

        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION)
        {
            Controls_TextBox tbBom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbBom");
            if (tbBom != null && tbBom.Text.Trim() != string.Empty)
            {
                flowDetail.Bom = TheBomMgr.LoadBom(tbBom.Text.Trim());
            }

            TextBox tbBatchSize = (TextBox)this.FV_FlowDetail.FindControl("tbBatchSize");
            Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbProductionLocFrom");
            if (tbBatchSize.Text.Trim() != string.Empty)
            {
                flowDetail.BatchSize = decimal.Parse(tbBatchSize.Text.Trim());
            }
            if (tbLocFrom != null && tbLocFrom.Text.Trim() != string.Empty)
            {
                flowDetail.LocationFrom = TheLocationMgr.LoadLocation(tbLocFrom.Text.Trim());
            }
            Controls_TextBox tbLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbProductionLocTo");
            if (tbLocTo != null && tbLocTo.Text.Trim() != string.Empty)
            {
                flowDetail.LocationTo = TheLocationMgr.LoadLocation(tbLocTo.Text.Trim());
            }
            Controls_TextBox tbCustomer = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbCustomer");
            if (tbCustomer != null && tbCustomer.Text.Trim() != string.Empty)
            {
                flowDetail.Customer = TheCustomerMgr.LoadCustomer(tbCustomer.Text.Trim());
            }
            com.Sconit.Control.CodeMstrDropDownList ddlBarCodeType = (com.Sconit.Control.CodeMstrDropDownList)this.FV_FlowDetail.FindControl("ddlBarCodeType");
            if (ddlBarCodeType.SelectedIndex != -1)
            {
                flowDetail.BarCodeType = ddlBarCodeType.SelectedValue;
            }

            Controls_TextBox tbRouting = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbRouting");
            if (tbRouting.Text.Trim() != string.Empty)
            {
                flowDetail.Routing = TheRoutingMgr.LoadRouting(tbRouting.Text.Trim());
            }
            Controls_TextBox tbReturnRouting = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbReturnRouting");
            if (tbReturnRouting.Text.Trim() != string.Empty)
            {
                flowDetail.ReturnRouting = TheRoutingMgr.LoadRouting(tbReturnRouting.Text.Trim());
            }


            Controls_TextBox tbRejectLocationFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbRejectLocationFrom");
            Controls_TextBox tbRejectLocationTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbRejectLocationTo");
            Controls_TextBox tbInspectLocationFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbInspectLocationFrom");
            Controls_TextBox tbInspectLocationTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbInspectLocationTo");

            if (tbRejectLocationFrom != null && tbRejectLocationFrom.Text.Trim() != string.Empty)
            {
                Location rejectLocation = TheLocationMgr.LoadLocation(tbRejectLocationFrom.Text.Trim());
                if (rejectLocation.Region.CostGroup != ((Region)flow.PartyFrom).CostGroup)
                {
                    ShowErrorMessage("MasterData.Flow.RejectLocationFrom.CostGroup.Error");
                    e.Cancel = true;
                }
                flowDetail.RejectLocationFrom = tbRejectLocationFrom.Text.Trim();
            }
            else
            {
                string regionRejectLocation = ((Region)flow.PartyFrom).RejectLocation;
                if (regionRejectLocation == null || regionRejectLocation == string.Empty)
                {
                    ShowErrorMessage("MasterData.Flow.RejectLocationFrom.Empty");
                    e.Cancel = true;
                }
                flowDetail.RejectLocationFrom = regionRejectLocation;
            }
            if (tbRejectLocationTo != null && tbRejectLocationTo.Text.Trim() != string.Empty)
            {
                Location rejectLocation = TheLocationMgr.LoadLocation(tbRejectLocationTo.Text.Trim());
                if (rejectLocation.Region.CostGroup != ((Region)flow.PartyTo).CostGroup)
                {
                    ShowErrorMessage("MasterData.Flow.RejectLocationTo.CostGroup.Error");
                    e.Cancel = true;
                }
                flowDetail.RejectLocationTo = tbRejectLocationTo.Text.Trim();
            }
            else
            {
                string regionRejectLocation = ((Region)flow.PartyTo).RejectLocation;
                if (regionRejectLocation == null || regionRejectLocation == string.Empty)
                {
                    ShowErrorMessage("MasterData.Flow.RejectLocationTo.Empty");
                    e.Cancel = true;
                }
                flowDetail.RejectLocationTo = regionRejectLocation;
            }

            if (tbInspectLocationFrom != null && tbInspectLocationFrom.Text.Trim() != string.Empty)
            {
                Location inspectLocation = TheLocationMgr.LoadLocation(tbInspectLocationFrom.Text.Trim());
                if (inspectLocation.Region.CostGroup != ((Region)flow.PartyFrom).CostGroup)
                {
                    ShowErrorMessage("MasterData.Flow.RejectLocationFrom.CostGroup.Error");
                    e.Cancel = true;
                }
                flowDetail.InspectLocationFrom = tbInspectLocationFrom.Text.Trim();
            }
            else
            {
                string regionInspectLocation = ((Region)flow.PartyFrom).InspectLocation;
                if (regionInspectLocation == null || regionInspectLocation == string.Empty)
                {
                    ShowErrorMessage("MasterData.Flow.InspectLocationFrom.Empty");
                    e.Cancel = true;
                }
                flowDetail.InspectLocationFrom = regionInspectLocation;
            }
            if (tbInspectLocationTo != null && tbInspectLocationTo.Text.Trim() != string.Empty)
            {
                Location inspectLocation = TheLocationMgr.LoadLocation(tbInspectLocationTo.Text.Trim());
                if (inspectLocation.Region.CostGroup != ((Region)flow.PartyTo).CostGroup)
                {
                    ShowErrorMessage("MasterData.Flow.InspectLocationTo.CostGroup.Error");
                    e.Cancel = true;
                }
                flowDetail.InspectLocationTo = tbInspectLocationTo.Text.Trim();
            }
            else
            {
                string regionInspectLocation = ((Region)flow.PartyTo).InspectLocation;
                if (regionInspectLocation == null || regionInspectLocation == string.Empty)
                {
                    ShowErrorMessage("MasterData.Flow.InspectLocationTo.Empty");
                    e.Cancel = true;
                }
                flowDetail.InspectLocationTo = regionInspectLocation;
            }
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_TRANSFER)
        {
            Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbTransferLocFrom");
            if (tbLocFrom != null && tbLocFrom.Text.Trim() != string.Empty)
            {
                flowDetail.LocationFrom = TheLocationMgr.LoadLocation(tbLocFrom.Text.Trim());
            }
            Controls_TextBox tbLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbTransferLocTo");
            if (tbLocTo != null && tbLocTo.Text.Trim() != string.Empty)
            {
                flowDetail.LocationTo = TheLocationMgr.LoadLocation(tbLocTo.Text.Trim());
            }

            DropDownList ddlOddShipOption = (DropDownList)this.FV_FlowDetail.FindControl("ddlOddShipOption");
            if (ddlOddShipOption.SelectedIndex != -1)
            {

                flowDetail.OddShipOption = ddlOddShipOption.SelectedValue;

            }

            Controls_TextBox tbRejectLocFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbTransferRejectLocFrom");
            if (tbRejectLocFrom.Text.Trim() != string.Empty)
            {
                flowDetail.RejectLocationFrom = tbRejectLocFrom.Text.Trim();
            }

            Controls_TextBox tbRejectLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbTransferRejectLocTo");
            if (tbRejectLocTo.Text.Trim() != string.Empty)
            {
                flowDetail.RejectLocationTo = tbRejectLocTo.Text.Trim();
            }
        }
    }

    protected void ODS_FlowDetail_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {

        EditEvent(sender, e);

        FlowDetail flowDetail = TheFlowDetailMgr.LoadFlowDetail(this.FlowDetailId);
        decimal unitCount = flowDetail.UnitCount;
        decimal orderLotSize = flowDetail.OrderLotSize == null ? 0 : (decimal)flowDetail.OrderLotSize;
        if (unitCount != 0 && orderLotSize != 0 && orderLotSize % unitCount != 0)
        {
            ShowWarningMessage("MasterData.Flow.FlowDetail.AddFlowDetail.Successfully.UC.Not.Divisible", flowDetail.Sequence.ToString());
        }
        else
        {
            ShowSuccessMessage("MasterData.Flow.FlowDetail.UpdateFlowDetail.Successfully", flowDetail.Sequence.ToString());
        }
    }

    protected void ODS_FlowDetail_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        btnBack_Click(this, e);
        ShowSuccessMessage("MasterData.Flow.FlowDetail.DeleteFlowDetail.Successfully", ((TextBox)(this.FV_FlowDetail.FindControl("tbSeq"))).Text);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            this.Visible = false;
        }
    }

    protected void checkSeqExists(object source, ServerValidateEventArgs args)
    {
        String seq = ((TextBox)(this.FV_FlowDetail.FindControl("tbSeq"))).Text.Trim();

        IList<FlowDetail> flowDetailList = TheFlowDetailMgr.GetFlowDetail(this.FlowCode, true);
        if (flowDetailList != null && flowDetailList.Count > 0)
        {
            foreach (FlowDetail flowDetail in flowDetailList)
            {
                if (flowDetail.Sequence == int.Parse(seq) && flowDetail.Id != this.FlowDetailId)
                {
                    args.IsValid = false;
                    break;
                }
            }
        }
    }

    private void BillSettleTermDataBind()
    {
        DropDownList ddlBillSettleTerm = (DropDownList)this.FV_FlowDetail.FindControl("ddlBillSettleTerm");
        ddlBillSettleTerm.DataSource = this.GetBillSettleTermGroup(this.ModuleType);
        ddlBillSettleTerm.DataBind();
    }

    private IList<CodeMaster> GetBillSettleTermGroup(string ModuleType)
    {
        IList<CodeMaster> billSettleTermGroup = new List<CodeMaster>();


        if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT
            || this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING)
        {
            billSettleTermGroup.Add(new CodeMaster()); //添加空选项
            billSettleTermGroup.Add(GetBillSettleTerm(BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_RECEIVING_SETTLEMENT));
            billSettleTermGroup.Add(GetBillSettleTerm(BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_ONLINE_BILLING));
            billSettleTermGroup.Add(GetBillSettleTerm(BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_LINEAR_CLEARING));
            billSettleTermGroup.Add(GetBillSettleTerm(BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_INSPECTION));
        }
        if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION)
        {
            billSettleTermGroup.Add(new CodeMaster()); //添加空选项
            billSettleTermGroup.Add(GetBillSettleTerm(BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_RECEIVING_SETTLEMENT));
            billSettleTermGroup.Add(GetBillSettleTerm(BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM_VALUE_CONSIGNMENT));
        }
        return billSettleTermGroup;
    }

    private CodeMaster GetBillSettleTerm(string billSettleTermValue)
    {
        return TheCodeMasterMgr.GetCachedCodeMaster(BusinessConstants.CODE_MASTER_BILL_SETTLE_TERM, billSettleTermValue);
    }

    protected void checkItemExists(object source, ServerValidateEventArgs args)
    {
        Flow flow = TheFlowMgr.LoadFlow(this.FlowCode, true);
        string itemCode = ((Controls_TextBox)(this.FV_FlowDetail.FindControl("tbItemCode"))).Text;
        string uomCode = ((Controls_TextBox)(this.FV_FlowDetail.FindControl("tbUom"))).Text;
        decimal unitCount = decimal.Parse(((TextBox)(this.FV_FlowDetail.FindControl("tbUC"))).Text);
        string locFrom = string.Empty;
        string locTo = string.Empty;
        string bom = string.Empty;

        if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT
            || this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS
            || this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING)
        {
            Controls_TextBox tbLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbProcurementLocTo");
            if (tbLocTo != null && tbLocTo.Text.Trim() != string.Empty)
            {
                locTo = tbLocTo.Text.Trim();
            }
            else
            {
                locTo = flow.LocationTo != null ? flow.LocationTo.Code : string.Empty;
            }
            locFrom = string.Empty;
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION)
        {
            Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbDistributionLocFrom");
            if (tbLocFrom != null && tbLocFrom.Text.Trim() != string.Empty)
            {
                locFrom = tbLocFrom.Text.Trim();
            }
            else
            {
                locFrom = flow.LocationFrom != null ? flow.LocationFrom.Code : string.Empty;
            }
            locTo = string.Empty;
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION)
        {

            Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbProductionLocFrom");
            if (tbLocFrom != null && tbLocFrom.Text.Trim() != string.Empty)
            {
                locFrom = tbLocFrom.Text.Trim();
            }
            else
            {
                locFrom = flow.LocationFrom != null ? flow.LocationFrom.Code : string.Empty;
            }
            Controls_TextBox tbLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbProductionLocTo");
            if (tbLocTo != null && tbLocTo.Text.Trim() != string.Empty)
            {
                locTo = tbLocTo.Text.Trim();
            }
            else
            {
                locTo = flow.LocationTo != null ? flow.LocationTo.Code : string.Empty;
            }
            Controls_TextBox tbBom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbBom");
            if (tbBom != null && tbBom.Text.Trim() != string.Empty)
            {
                bom = tbBom.Text.Trim();
            }
        }
        else if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_TRANSFER)
        {
            Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbTransferLocFrom");
            if (tbLocFrom != null && tbLocFrom.Text.Trim() != string.Empty)
            {
                locFrom = tbLocFrom.Text.Trim();
            }
            else
            {
                locFrom = flow.LocationFrom != null ? flow.LocationFrom.Code : string.Empty;
            }
            Controls_TextBox tbLocTo = (Controls_TextBox)this.FV_FlowDetail.FindControl("tbTransferLocTo");
            if (tbLocTo != null && tbLocTo.Text.Trim() != string.Empty)
            {
                locTo = tbLocTo.Text.Trim();
            }
            else
            {
                locTo = flow.LocationTo != null ? flow.LocationTo.Code : string.Empty;
            }
        }

        IList<FlowDetail> flowDetailList = flow.FlowDetails;
        if (flowDetailList != null && flowDetailList.Count > 0)
        {
            foreach (FlowDetail flowDetail in flowDetailList)
            {
                string defaultLocFrom = flowDetail.DefaultLocationFrom == null ? string.Empty : flowDetail.DefaultLocationFrom.Code;
                string defaultLocTo = flowDetail.DefaultLocationTo == null ? string.Empty : flowDetail.DefaultLocationTo.Code;
                string flowDetailBomCode = flowDetail.Bom == null ? string.Empty : flowDetail.Bom.Code;
                if (flowDetail.Item.Code == itemCode
                    && flowDetail.Uom.Code == uomCode
                    && defaultLocFrom == locFrom
                    && defaultLocTo == locTo
                    && flowDetail.Id != this.FlowDetailId
                    && flowDetail.UnitCount == unitCount
                    && flowDetailBomCode == bom)
                {
                    args.IsValid = false;
                    ((CustomValidator)(this.FV_FlowDetail.FindControl("cvItemCheck"))).ErrorMessage = "${MasterData.Flow.FlowDetail.ItemCode.Exists}";
                    break;
                }
            }
        }
        if (flow.ReferenceFlow != null && flow.ReferenceFlow.Trim() != string.Empty && args.IsValid)
        {
            IList<FlowDetail> refFlowDetailList = TheFlowDetailMgr.GetFlowDetail(flow.ReferenceFlow);
            if (refFlowDetailList != null && refFlowDetailList.Count > 0)
            {
                foreach (FlowDetail flowDetail in refFlowDetailList)
                {
                    string defaultLocFrom = flowDetail.DefaultLocationFrom == null ? string.Empty : flow.LocationFrom.Code;
                    string defaultLocTo = flowDetail.DefaultLocationTo == null ? string.Empty : flow.LocationTo.Code;
                    string flowDetailBomCode = flowDetail.Bom == null ? string.Empty : flowDetail.Bom.Code;
                    if (flowDetail.Item.Code == itemCode
                        && flowDetail.Uom.Code == uomCode
                        && defaultLocFrom == locFrom
                        && defaultLocTo == locTo
                        && flowDetail.UnitCount == unitCount
                        && flowDetailBomCode == bom)
                    {
                        args.IsValid = false;
                        ((CustomValidator)(this.FV_FlowDetail.FindControl("cvItemCheck"))).ErrorMessage = "${MasterData.Flow.FlowDetail.ItemCode.Exists}";
                        break;
                    }
                }
            }
        }
    }
}

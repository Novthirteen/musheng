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

public partial class Order_OrderHead_OrderImport_Import2 : ModuleBase
{
    public event EventHandler ImportEvent;
    public event EventHandler BtnBackClick;

    private string companyCode
    {
        get { return (string)ViewState["companyCode"]; }
        set { ViewState["companyCode"] = value; }
    }

    public string ModuleType
    {
        get { return (string)ViewState["ModuleType"]; }
        set { ViewState["ModuleType"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.companyCode = "ChunShen";//TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_COMPANY_CODE).Value;
            if (this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION)
            {
                this.rblDateType.SelectedValue = "startTime";
            }
        }
        this.InitialUI();
    }

    public void Create(object sender)
    {
        try
        {
            IList<OrderHead> orderHeadList = (IList<OrderHead>)sender;
            if (orderHeadList != null && orderHeadList.Count > 0)
            {
                TheOrderMgr.CreateOrder(orderHeadList, this.CurrentUser.Code);
                ShowSuccessMessage("Common.Business.Result.Insert.Successfully");
            }
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage(ex);
        }
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        this.Import();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BtnBackClick != null)
        {
            BtnBackClick(null, null);
        }
    }

    private void Import()
    {
        try
        {
            bool isWinTime = this.rblDateType.SelectedValue == "winTime";
            string flowCode = this.tbFlow.Text.Trim();
            string timePeriodType = this.ucDateSelect.TimePeriodType;
            DateTime date = this.ucDateSelect.Date;
            IList<FlowPlan> flowPlanList = new List<FlowPlan>();
            if (this.rblListFormat.SelectedValue == "Template2")
            {
                flowPlanList = TheImportMgr.ReadScheduleFromXls(fileUpload.PostedFile.InputStream, this.CurrentUser, this.ModuleType, flowCode, timePeriodType, date);
            }
            else
            {
                if (this.companyCode == "ChunShen" && this.ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION)
                {
                    flowPlanList = TheImportMgr.ReadShipScheduleCSFromXls(fileUpload.PostedFile.InputStream, this.CurrentUser, this.ModuleType, flowCode, timePeriodType, date);
                }
                else
                {
                    flowPlanList = TheImportMgr.ReadShipScheduleYFKFromXls(fileUpload.PostedFile.InputStream, this.CurrentUser, this.ModuleType, flowCode, timePeriodType, date);
                }
            }
            IList<OrderHead> ohList = TheOrderMgr.ConvertFlowPlanToOrders(flowPlanList, isWinTime);
            ohList = ohList.Where(o => o.OrderDetails != null && o.OrderDetails.Count > 0).ToList();
            foreach (OrderHead oh in ohList)
            {
                oh.OrderDetails = oh.OrderDetails.OrderBy(o => o.Sequence).ThenBy(o => o.Item.Code).ToList();
            }

            if (ImportEvent != null)
            {
                ImportEvent(new object[] { ohList }, null);
            }
            ShowSuccessMessage("Import.Result.Successfully");
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage(ex);
        }
    }

    private void InitialUI()
    {

        if (ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION)
        {
            this.tbFlow.ServiceParameter = "string:" + this.CurrentUser.Code + ",bool:false,bool:true,bool:true,bool:false,bool:false,bool:false,string:" + BusinessConstants.PARTY_AUTHRIZE_OPTION_FROM;

            this.hlTemplate2.NavigateUrl = "~/Reports/Templates/ExcelTemplates/Ship.xls";
            if (this.companyCode == "ChunShen")
            {
                this.hlTemplate1.NavigateUrl = "~/Reports/Templates/ChunShenExcelTemplates/ShipSchedule.xls";
            }
            else
            {
                this.hlTemplate1.NavigateUrl = "~/Reports/Templates/ExcelTemplates/ShipSchedule.xls";
            }
        }
        else if (ModuleType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT)
        {
            this.tbFlow.ServiceParameter = "string:" + this.CurrentUser.Code + ",bool:true,bool:false,bool:true,bool:false,bool:true,bool:true,string:" + BusinessConstants.PARTY_AUTHRIZE_OPTION_TO;

            this.hlTemplate1.NavigateUrl = "~/Reports/Templates/ExcelTemplates/ProcurementSchedule.xls";
            this.hlTemplate2.NavigateUrl = "~/Reports/Templates/ExcelTemplates/Procurement.xls";
        }
        else //todo
        {
            this.tbFlow.ServiceParameter = "string:" + this.CurrentUser.Code + ",bool:false,bool:true,bool:true,bool:false,bool:false,bool:false,string:" + BusinessConstants.PARTY_AUTHRIZE_OPTION_FROM;
            this.hlTemplate1.Enabled = false;
            this.hlTemplate2.Enabled = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using NHibernate.Expression;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;

public partial class Finance_Bill_NewSearch : SearchModuleBase
{
    public event EventHandler BackEvent;
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

    private string billNo
    {
        get
        {
            return (string)ViewState["billNo"];
        }
        set
        {
            ViewState["billNo"] = value;
        }
    }

    public void InitPageParameter(bool isPopup, Bill bill)
    {
        if (isPopup)
        {
            this.billNo = bill.BillNo;
            this.tbPartyCode.Visible = false;
            this.ltlParty.Text = bill.BillAddress.Party.Name;
            this.ltlParty.Visible = true;
            this.IsRelease.Visible = false;
            this.btnConfirm.Visible = false;
            this.btnBack.Visible = false;
            this.btnAddDetail.Visible = true;
            this.btnClose.Visible = true;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.ucNewList.ModuleType = this.ModuleType;
            this.PageCleanUp();
            string companyCode = TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_COMPANY_CODE).Value;
            if (companyCode != "ChunShen")
            {
                this.cbZS.Visible = false;
                this.cbGS.Visible = false;
            }
        }

        this.tbFlow.ServiceParameter = "string:" + this.CurrentUser.Code;
        if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_SO)
        {
            this.ltlPartyCode.Text = "${MasterData.ActingBill.Customer}:";
            this.ltlReceiver.Text = "${MasterData.ActingBill.ExternalReceiptNo}:";

            this.tbPartyCode.ServicePath = "CustomerMgr.service";
            this.tbPartyCode.ServiceMethod = "GetAllCustomer";
            this.tbBillAddress.ServiceParameter = "bool:false";
            this.tbFlow.ServiceMethod = "GetDistributionFlow";
        }
        else
        {
            this.tbBillAddress.ServiceParameter = "bool:true";
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        this.ucNewList.IsExport = false;
        if (btn == this.btnExport)
        {
            this.ucNewList.IsExport = true;
            DoSearch();
            this.ucNewList.ExportXLS();
        }
        else
        {
            DoSearch();
        }
    }

    protected override void DoSearch()
    {
        string partyCode = this.tbPartyCode.Text != string.Empty ? this.tbPartyCode.Text.Trim() : string.Empty;
        string receiver = this.tbReceiver.Text != string.Empty ? this.tbReceiver.Text.Trim() : string.Empty;
        string startDate = this.tbStartDate.Text != string.Empty ? this.tbStartDate.Text.Trim() : string.Empty;
        string endDate = this.tbEndDate.Text != string.Empty ? this.tbEndDate.Text.Trim() : string.Empty;
        string itemCode = this.tbItemCode.Text != string.Empty ? this.tbItemCode.Text.Trim() : string.Empty;
        string currency = this.tbCurrency.Text != string.Empty ? this.tbCurrency.Text.Trim() : string.Empty;
        string flowCode = this.tbFlow.Text != string.Empty ? this.tbFlow.Text.Trim() : null;
        string billAddress = this.tbBillAddress.Text != string.Empty ? this.tbBillAddress.Text.Trim() : null;
        bool isOrderByItem = this.cbOrderByItem.Checked;

        DateTime? effDateFrom = null;
        if (startDate != string.Empty)
        {
            effDateFrom = DateTime.Parse(startDate);
        }

        DateTime? effDateTo = null;
        if (endDate != string.Empty)
        {
            effDateTo = DateTime.Parse(endDate).AddDays(1).AddMilliseconds(-1);
        }

        bool needRecalculate = bool.Parse(TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_RECALCULATE_WHEN_BILL).Value);
        if (needRecalculate)
        {
            IList<ActingBill> allactingBillList = TheActingBillMgr.GetActingBill(partyCode, receiver, effDateFrom, effDateTo, itemCode, currency, this.ModuleType, this.billNo, false, flowCode, billAddress);
            TheActingBillMgr.RecalculatePrice(allactingBillList, this.CurrentUser);
        }
        IList<ActingBill> actingBillList = TheActingBillMgr.GetActingBill(partyCode, receiver, effDateFrom, effDateTo, itemCode, currency, this.ModuleType, this.billNo, false, flowCode, billAddress);
        //actingBillList = actingBillList.OrderBy(b => b.ExternalReceiptNo).ToList();
        if (isOrderByItem)
        {
            actingBillList = actingBillList.OrderBy(b => b.Item.Code).ToList();
        }

        #region 春申客户化
        bool isGS = this.cbGS.Checked;
        bool isZS = this.cbZS.Checked;
        if (itemCode == string.Empty)
        {
            if (isGS && !isZS)
            {
                actingBillList = actingBillList.Where(b => (b.Item.Code.StartsWith("18") || b.Item.Code.StartsWith("19"))).ToList();
            }
            else if (!isGS && isZS)
            {
                actingBillList = actingBillList.Where(b => !(b.Item.Code.StartsWith("18") || b.Item.Code.StartsWith("19"))).ToList();
            }
        }
        #endregion

        this.ucNewList.BindDataSource(actingBillList != null && actingBillList.Count > 0 ? actingBillList : null);
        this.ucNewList.Visible = true;

        this.ucNewList.InitializationTotal(); //add by ljz
        
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            IList<ActingBill> actingBillList = this.ucNewList.PopulateSelectedData();
            IList<Bill> billList;
            if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_SO)
            {
                billList = TheBillMgr.CreateBill(actingBillList, this.CurrentUser);
            }
            else
            {
                DateTime startDate;
                DateTime endDate;
                if (this.tbStartDate.Text.Trim() != null)
                {
                    startDate = DateTime.Parse(this.tbStartDate.Text.Trim());
                }
                else
                {
                    startDate = DateTime.Now;
                }
                if (this.tbEndDate.Text.Trim() != null)
                {
                    endDate = DateTime.Parse(this.tbEndDate.Text.Trim());
                }
                else
                {
                    endDate = DateTime.Now;
                }
                billList = TheBillMgr.CreateBill(actingBillList, this.CurrentUser, startDate, endDate);
            }
            this.ShowSuccessMessage("MasterData.Bill.CreateSuccessfully", billList[0].BillNo);

            if (this.IsRelease.Checked)
            {
                TheBillMgr.ReleaseBill(billList[0].BillNo, this.CurrentUser);
                this.ShowSuccessMessage("MasterData.Bill.ReleaseSuccessfully", billList[0].BillNo);
            }
            this.PageCleanUp();
            CreateEvent(billList[0].BillNo, null);
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        BackEvent(this, null);
    }

    protected void btnAddDetail_Click(object sender, EventArgs e)
    {
        try
        {
            IList<ActingBill> actingBillList = this.ucNewList.PopulateSelectedData();
            this.TheBillMgr.AddBillDetail(this.billNo, actingBillList, this.CurrentUser);
            this.ShowSuccessMessage("MasterData.Bill.AddBillDetailSuccessfully");
            BackEvent(this, null);
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnNamedQuery_Click(object sender, EventArgs e)
    {
        IDictionary<string, string> actionParameter = new Dictionary<string, string>();
        if (this.tbStartDate.Text != string.Empty)
        {
            actionParameter.Add("StartDate", this.tbStartDate.Text);
        }
        if (this.tbEndDate.Text != string.Empty)
        {
            actionParameter.Add("EndDate", this.tbEndDate.Text);
        }
        if (this.tbPartyCode.Text != string.Empty)
        {
            actionParameter.Add("PartyCode", this.tbPartyCode.Text);
        }
        if (this.tbReceiver.Text != string.Empty)
        {
            actionParameter.Add("Receiver", this.tbReceiver.Text);
        }
        if (this.tbItemCode.Text != string.Empty)
        {
            actionParameter.Add("ItemCode", this.tbItemCode.Text);
        }
        if (this.tbCurrency.Text != string.Empty)
        {
            actionParameter.Add("Currency", this.tbCurrency.Text);
        }

        //this.SaveNamedQuery(this.tbNamedQuery.Text, actionParameter);
    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {
        if (actionParameter.ContainsKey("StartDate"))
        {
            this.tbStartDate.Text = actionParameter["StartDate"];
        }
        if (actionParameter.ContainsKey("EndDate"))
        {
            this.tbEndDate.Text = actionParameter["EndDate"];
        }
        if (actionParameter.ContainsKey("PartyCode"))
        {
            this.tbPartyCode.Text = actionParameter["PartyCode"];
        }
        if (actionParameter.ContainsKey("Receiver"))
        {
            this.tbReceiver.Text = actionParameter["Receiver"];
        }
        if (actionParameter.ContainsKey("ItemCode"))
        {
            this.tbItemCode.Text = actionParameter["ItemCode"];
        }
        if (actionParameter.ContainsKey("Currency"))
        {
            this.tbCurrency.Text = actionParameter["Currency"];
        }
    }

    public void PageCleanUp()
    {
        this.tbStartDate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
        this.tbEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        this.tbPartyCode.Text = string.Empty;
        this.tbReceiver.Text = string.Empty;
        this.tbItemCode.Text = string.Empty;
        this.tbCurrency.Text = string.Empty;

        this.ucNewList.Visible = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Web;

public partial class Finance_Bill_Edit : ListModuleBase
{
    public event EventHandler BackEvent;

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

    private int DecimalLength
    {
        get
        {
            return (int)ViewState["DecimalLength"];
        }
        set
        {
            ViewState["DecimalLength"] = value;
        }
    }

    public string PartyCode
    {
        get
        {
            return (string)ViewState["PartyCode"];
        }
        set
        {
            ViewState["PartyCode"] = value;
        }
    }

    public string BillNo
    {
        get
        {
            return (string)ViewState["BillNo"];
        }
        set
        {
            ViewState["BillNo"] = value;
        }
    }

    private bool isGroup
    {
        get
        {
            if (this.rblListFormat.SelectedValue == "Detail")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        //if (isGroup)
        //{
        //    this.divscript.Visible = false;
        //    this.ExportXLS(this.gv_Group);
        //    this.divscript.Visible = true;
        //    return;
        //}
        Bill bill = null;
        IList<object> list = new List<object>();
        if (isGroup)
        {
            bill = this.TheBillMgr.LoadBill(this.BillNo, true, true);
            list.Add(bill);
            list.Add(bill.BillDetails);
            if (bill.TransactionType == BusinessConstants.BILL_TRANS_TYPE_PO)
            {
                TheReportMgr.WriteToClient("BillPOGroup.xls", list, "BillPOGroup.xls");
            }
            else
            {
                TheReportMgr.WriteToClient("BillSOGroup.xls", list, "BillSOGroup.xls");
            }
        }
        else
        {
            bill = this.TheBillMgr.LoadBill(this.BillNo);
            IList<BillDetail> billDetails = this.TheBillDetailMgr.GetBillDetailOrderByItem(this.BillNo);
            list.Add(bill);
            list.Add(billDetails);
            if (bill.TransactionType == BusinessConstants.BILL_TRANS_TYPE_PO)
            {
                TheReportMgr.WriteToClient("Bill.xls", list, "Bill.xls");
            }
            else
            {
                TheReportMgr.WriteToClient("BillMarket.xls", list, "BillMarket.xls");
            }
        }
        this.ShowSuccessMessage("MasterData.Bill.Print.Successful");

    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Bill bill = null;
        IList<object> list = new List<object>();
        string barCodeUrl = string.Empty;
        if (isGroup)
        {
            bill = this.TheBillMgr.LoadBill(this.BillNo, true, true);
            list.Add(bill);
            list.Add(bill.BillDetails);
            if (bill.TransactionType == BusinessConstants.BILL_TRANS_TYPE_PO)
            {
                barCodeUrl = TheReportMgr.WriteToFile("BillPOGroup.xls", list);
            }
            else
            {
                barCodeUrl = TheReportMgr.WriteToFile("BillSOGroup.xls", list);
            }
        }
        else
        {
            bill = this.TheBillMgr.LoadBill(this.BillNo);
            IList<BillDetail> billDetails = this.TheBillDetailMgr.GetBillDetailOrderByItem(this.BillNo);
            list.Add(bill);
            list.Add(billDetails);
            if (bill.TransactionType == BusinessConstants.BILL_TRANS_TYPE_PO)
            {
                barCodeUrl = TheReportMgr.WriteToFile("Bill.xls", list);
            }
            else
            {
                barCodeUrl = TheReportMgr.WriteToFile("BillMarket.xls", list);
            }

        }
        if (list.Count == 2)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "method", " <script language='javascript' type='text/javascript'>PrintOrder('" + barCodeUrl + "'); </script>");
            this.ShowSuccessMessage("MasterData.Bill.Print.Successful");
        }
    }

    public void InitPageParameter(string billNo)
    {
        this.BillNo = billNo;
        this.ODS_Bill.SelectParameters["billNo"].DefaultValue = billNo;
        this.ODS_Bill.SelectParameters["includeDetail"].DefaultValue = true.ToString();

        if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_PO)
        {
            this.Gv_List.Columns[2].Visible = false;
        }
        else if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_SO)
        {
            Literal lblParty = this.FV_Bill.FindControl("lblParty") as Literal;
            lblParty.Text = "${MasterData.Bill.Customer}:";

            this.Gv_List.Columns[1].Visible = false;
        }

        Bill bill = this.TheBillMgr.LoadBill(this.BillNo, true);
        if (bill != null)
        {
            CalculateBillPrice(bill);
        }
        
    }

    private void CalculateBillPrice(Bill bill)
    {
        decimal totalPrice = 0;

        for (int i = 0; i < bill.BillDetails.Count; i++)
        {
            BillDetail billDetail = bill.BillDetails[i];
            decimal detailPrice = 0;
            detailPrice = (billDetail.Amount == 0 ? (billDetail.BilledQty * billDetail.UnitPrice) : billDetail.Amount) - billDetail.DiscountRate;



            totalPrice += detailPrice;
        }

        totalPrice -= (bill.Discount.HasValue ? bill.Discount.Value : decimal.Zero);
        this.tbTotalAmount.Text = totalPrice.ToString("F2");
    }


    public override void UpdateView()
    { }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucNewSearch.BackEvent += new EventHandler(AddBack_Render);

        if (!IsPostBack)
        {
            this.ucNewSearch.ModuleType = this.ModuleType;
        }
    }

    protected void FV_Bill_DataBound(object sender, EventArgs e)
    {
        Bill bill = (Bill)((FormView)(sender)).DataItem;
        UpdateView(bill);
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            BillDetail billDetail = (BillDetail)e.Row.DataItem;

            TextBox tbAmount = (TextBox)e.Row.FindControl("tbAmount");
            tbAmount.Attributes["oldValue"] = tbAmount.Text;
            TextBox tbAmountAfterDiscount = (TextBox)e.Row.FindControl("tbAmountAfterDiscount");
            tbAmountAfterDiscount.Attributes["oldValue"] = tbAmountAfterDiscount.Text;

            if (billDetail.Bill.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {
                TextBox tbQty = (TextBox)e.Row.FindControl("tbQty");
                TextBox tbDiscountRate = (TextBox)e.Row.FindControl("tbDiscountRate");
                TextBox tbDiscount = (TextBox)e.Row.FindControl("tbDiscount");


                tbQty.ReadOnly = true;
                tbDiscountRate.ReadOnly = true;
                tbDiscount.ReadOnly = true;
                tbAmount.ReadOnly = true;

                decimal detailPrice = 0;
                detailPrice = (billDetail.Amount == 0 ? (billDetail.BilledQty * billDetail.UnitPrice) : billDetail.Amount) - (billDetail.Discount.HasValue ? billDetail.Discount.Value : 0);
                tbAmountAfterDiscount.Text = detailPrice.ToString("F2");

                tbDiscount.Text = billDetail.Discount.HasValue ? billDetail.Discount.Value.ToString("F2") : string.Empty;
                tbDiscountRate.Text = billDetail.Discount.HasValue ? billDetail.DiscountRate.ToString("F2") : string.Empty;
            }
            else
            {
                if (billDetail.Amount == 0)
                {
                    tbAmount.Text = (billDetail.BilledQty * billDetail.UnitPrice).ToString("0.########");
                }
            }
        }
    }

    protected void lbRefBillNo_Click(object sender, EventArgs e)
    {
        string refBillNo = ((LinkButton)(sender)).CommandArgument;
        InitPageParameter(refBillNo);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Bill bill = this.TheBillMgr.LoadBill(this.BillNo, true);
            if (bill.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT)
            {
                TextBox tbExternalBillNo = this.FV_Bill.FindControl("tbExternalBillNo") as TextBox;
                if (tbExternalBillNo.Text.Trim() != string.Empty)
                {
                    bill.ExternalBillNo = tbExternalBillNo.Text.Trim();
                }
                else
                {
                    bill.ExternalBillNo = null;
                }

                //TextBox tbPaymentAmount = this.FV_Bill.FindControl("tbPaymentAmount") as TextBox;
                //if (tbPaymentAmount.Text.Trim() != string.Empty)
                //{
                //    bill.PaymentAmount = decimal.Parse(tbPaymentAmount.Text.Trim());
                //    if (bill.PaymentAmount > bill.TotalBillAmount)
                //    {
                //        bill.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
                //    }
                //}
                //else
                //{
                //    bill.PaymentAmount = decimal.Zero;
                //}

                TextBox tbInvoiceDate = this.FV_Bill.FindControl("tbInvoiceDate") as TextBox;
                if (tbInvoiceDate.Text.Trim() != string.Empty)
                {
                    bill.InvoiceDate = DateTime.Parse(tbInvoiceDate.Text.Trim());
                }

                bill.LastModifyUser = this.CurrentUser;
                bill.LastModifyDate = DateTime.Now;
                this.TheBillMgr.UpdateBill(bill);
            }
            else
            {
                IList<BillDetail> billDetailList = PopulateData(false);

                //TextBox tbPaymentAmount = this.FV_Bill.FindControl("tbPaymentAmount") as TextBox;
                //if (tbPaymentAmount.Text.Trim() != string.Empty)
                //{
                //    bill.PaymentAmount = decimal.Parse(tbPaymentAmount.Text.Trim());
                //}
                //else
                //{
                //    bill.PaymentAmount = decimal.Zero;
                //}


                TextBox tbInvoiceDate = this.FV_Bill.FindControl("tbInvoiceDate") as TextBox;
                if (tbInvoiceDate.Text.Trim() != string.Empty)
                {
                    bill.InvoiceDate = DateTime.Parse(tbInvoiceDate.Text.Trim());
                }
               

                if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_PO && bill.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
                {
                    TextBox tbStartDate = this.FV_Bill.FindControl("tbStartDate") as TextBox;
                    if (tbStartDate.Text.Trim() != string.Empty)
                    {
                        bill.StartDate = DateTime.Parse(tbStartDate.Text.Trim());
                    }

                    TextBox tbEndDate = this.FV_Bill.FindControl("tbEndDate") as TextBox;
                    if (tbEndDate.Text.Trim() != string.Empty)
                    {
                        bill.EndDate = DateTime.Parse(tbEndDate.Text.Trim());
                    }
                }

                if (this.tbTotalDiscount.Text.Trim() != string.Empty)
                {
                    bill.Discount = decimal.Parse(this.tbTotalDiscount.Text.Trim());
                }
                else
                {
                    bill.Discount = null;
                }
                bill.BillDetails = billDetailList;
                this.TheBillMgr.UpdateBill(bill, this.CurrentUser);
            }

            this.ShowSuccessMessage("MasterData.Bill.UpdateSuccessfully", this.BillNo);
            this.FV_Bill.DataBind();
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        this.btnSave_Click(sender, e);
        try
        {
            this.TheBillMgr.ReleaseBill(this.BillNo, this.CurrentUser);
            this.ShowSuccessMessage("MasterData.Bill.ReleaseSuccessfully", this.BillNo);
            this.FV_Bill.DataBind();
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
            this.TheBillMgr.DeleteBill(this.BillNo, this.CurrentUser);
            this.ShowSuccessMessage("MasterData.Bill.DeleteSuccessfully", this.BillNo);
            this.BackEvent(this, e);
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            this.TheBillMgr.CloseBill(this.BillNo, this.CurrentUser);
            this.ShowSuccessMessage("MasterData.Bill.CloseSuccessfully", this.BillNo);
            this.FV_Bill.DataBind();


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
            this.TheBillMgr.CancelBill(this.BillNo, this.CurrentUser);
            this.ShowSuccessMessage("MasterData.Bill.CancelSuccessfully", this.BillNo);
            this.FV_Bill.DataBind();
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void btnVoid_Click(object sender, EventArgs e)
    {
        try
        {
            this.TheBillMgr.VoidBill(this.BillNo, this.CurrentUser);
            this.ShowSuccessMessage("MasterData.Bill.VoidSuccessfully", this.BillNo);
            this.FV_Bill.DataBind();
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
        }
    }

    protected void btnAddDetail_Click(object sender, EventArgs e)
    {
        IDictionary<string, string> actionParameter = new Dictionary<string, string>();
        actionParameter.Add("PartyCode", this.PartyCode);
        this.ucNewSearch.QuickSearch(actionParameter);
        this.ucNewSearch.Visible = true;
    }

    protected void btnDeleteDetail_Click(object sender, EventArgs e)
    {
        try
        {
            IList<BillDetail> billDetailList = PopulateData(true);
            this.TheBillMgr.DeleteBillDetail(billDetailList, this.CurrentUser);
            this.ShowSuccessMessage("MasterData.Bill.DeleteBillDetailSuccessfully");
            this.FV_Bill.DataBind();
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    protected void AddBack_Render(object sender, EventArgs e)
    {
        this.ucNewSearch.Visible = false;
        this.FV_Bill.DataBind();
    }

    private void UpdateView(Bill bill)
    {
        #region 根据状态显示按钮
        if (bill.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
        {
            this.btnSave.Visible = true;
            this.btnSubmit.Visible = true;
            this.btnDelete.Visible = true;
            this.btnClose.Visible = false;
            this.btnCancel.Visible = false;
            this.btnVoid.Visible = false;
            this.btnAddDetail.Visible = true;
            this.btnDeleteDetail.Visible = true;
            this.btnPrint.Visible = true;

        }
        else if (bill.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT)
        {
            this.btnSave.Visible = true;
            this.btnSubmit.Visible = false;
            this.btnDelete.Visible = false;
            this.btnClose.Visible = true;
            this.btnCancel.Visible = true;
            this.btnVoid.Visible = false;
            this.btnAddDetail.Visible = false;
            this.btnDeleteDetail.Visible = false;
            this.btnPrint.Visible = true;
        }
        else if (bill.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CANCEL)
        {
            this.btnSave.Visible = false;
            this.btnSubmit.Visible = false;
            this.btnDelete.Visible = false;
            this.btnClose.Visible = false;
            this.btnCancel.Visible = false;
            this.btnVoid.Visible = false;
            this.btnAddDetail.Visible = false;
            this.btnDeleteDetail.Visible = false;
            this.btnPrint.Visible = true;
        }
        else if (bill.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)
        {
            this.btnSave.Visible = false;
            this.btnSubmit.Visible = false;
            this.btnDelete.Visible = false;
            this.btnClose.Visible = false;
            this.btnCancel.Visible = false;
            this.btnPrint.Visible = true;

            if (bill.BillType == BusinessConstants.CODE_MASTER_BILL_TYPE_VALUE_CANCEL)
            {
                this.btnVoid.Visible = false; ;
            }
            else
            {
                this.btnVoid.Visible = true;
            }
            this.btnAddDetail.Visible = false;
            this.btnDeleteDetail.Visible = false;
        }
        else if (bill.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_VOID)
        {
            this.btnSave.Visible = false;
            this.btnSubmit.Visible = false;
            this.btnDelete.Visible = false;
            this.btnClose.Visible = false;
            this.btnCancel.Visible = false;
            this.btnVoid.Visible = false;
            this.btnAddDetail.Visible = false;
            this.btnDeleteDetail.Visible = false;
            this.btnPrint.Visible = true;


        }
        #endregion

        #region 根据状态隐藏/显示字段
        if (bill.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
        {
            this.Gv_List.Columns[0].Visible = false;
            this.tbTotalDiscountRate.ReadOnly = true;
            this.tbTotalDiscount.ReadOnly = true;

            ((TextBox)(this.FV_Bill.FindControl("tbStartDate"))).ReadOnly = true;
            ((TextBox)(this.FV_Bill.FindControl("tbEndDate"))).ReadOnly = true;
            RequiredFieldValidator rfvStartDate = this.FV_Bill.FindControl("rfvStartDate") as RequiredFieldValidator;
            rfvStartDate.Enabled = false;
            RequiredFieldValidator rfvEndDate = this.FV_Bill.FindControl("rfvEndDate") as RequiredFieldValidator;
            rfvEndDate.Enabled = false;

        }
        else
        {
            this.Gv_List.Columns[0].Visible = true;
            this.tbTotalDiscountRate.ReadOnly = false;
            this.tbTotalDiscount.ReadOnly = false;

            if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_PO)
            {
                ((TextBox)(this.FV_Bill.FindControl("tbStartDate"))).ReadOnly = false;
                ((TextBox)(this.FV_Bill.FindControl("tbEndDate"))).ReadOnly = false;
                RequiredFieldValidator rfvStartDate = this.FV_Bill.FindControl("rfvStartDate") as RequiredFieldValidator;
                rfvStartDate.Enabled = true;
                RequiredFieldValidator rfvEndDate = this.FV_Bill.FindControl("rfvEndDate") as RequiredFieldValidator;
                rfvEndDate.Enabled = true;
            }
            else
            {
                ((TextBox)(this.FV_Bill.FindControl("tbStartDate"))).ReadOnly = true;
                ((TextBox)(this.FV_Bill.FindControl("tbEndDate"))).ReadOnly = true;
                RequiredFieldValidator rfvStartDate = this.FV_Bill.FindControl("rfvStartDate") as RequiredFieldValidator;
                rfvStartDate.Enabled = false;
                RequiredFieldValidator rfvEndDate = this.FV_Bill.FindControl("rfvEndDate") as RequiredFieldValidator;
                rfvEndDate.Enabled = false;
            }
        }

        if (bill.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE && bill.Status != BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)
        {
            TextBox tbExternalBillNo = this.FV_Bill.FindControl("tbExternalBillNo") as TextBox;
            tbExternalBillNo.ReadOnly = false;

            //TextBox tbPaymentAmount = this.FV_Bill.FindControl("tbPaymentAmount") as TextBox;
            //tbPaymentAmount.ReadOnly = false;
           // RegularExpressionValidator revPaymentAmount = this.FV_Bill.FindControl("revPaymentAmount") as RegularExpressionValidator;
           // revPaymentAmount.Enabled = true;

            TextBox tbInvoiceDate = this.FV_Bill.FindControl("tbInvoiceDate") as TextBox;
            tbInvoiceDate.ReadOnly = false;
        }
        else
        {
            TextBox tbExternalBillNo = this.FV_Bill.FindControl("tbExternalBillNo") as TextBox;
            tbExternalBillNo.ReadOnly = true;

            //TextBox tbPaymentAmount = this.FV_Bill.FindControl("tbPaymentAmount") as TextBox;
            //tbPaymentAmount.ReadOnly = true;
            //RegularExpressionValidator revPaymentAmount = this.FV_Bill.FindControl("revPaymentAmount") as RegularExpressionValidator;
           // revPaymentAmount.Enabled = false;

            TextBox tbInvoiceDate = this.FV_Bill.FindControl("tbInvoiceDate") as TextBox;
            tbInvoiceDate.ReadOnly = true;
        }

        #endregion

        #region 给总金额和折扣赋值
        this.tbTotalDetailAmount.Text = bill.TotalBillDetailAmount.ToString("F2");
        this.tbTotalAmount.Text = bill.TotalBillAmount.ToString("F2");
        this.tbTotalDiscount.Text = (bill.Discount.HasValue ? bill.Discount.Value : 0).ToString("F2");
        this.tbTotalDiscountRate.Text = bill.TotalBillDiscountRate.ToString("F2");
        #endregion

        #region 初始化弹出窗口
        this.PartyCode = bill.BillAddress.Party.Code;
        this.ucNewSearch.InitPageParameter(true, bill);
        #endregion

        UpdateDetailView(bill.BillDetails);
    }

    private void UpdateDetailView(IList<BillDetail> billDetailList)
    {

        if (this.isGroup)
        {
            IList<BillDetail> groupBillDetail = this.TheBillDetailMgr.GroupBillDetail(billDetailList);
            this.gv_Group.DataSource = groupBillDetail;
            this.gv_Group.DataBind();
        }
        else
        {
            this.Gv_List.DataSource = billDetailList;
            this.Gv_List.DataBind();
        }
        this.gv_Group.Visible = this.isGroup;
        this.Gv_List.Visible = !this.isGroup;
        //this.btnAddDetail.Visible = !this.isGroup;
        //this.btnDeleteDetail.Visible = !this.isGroup;
    }

    private IList<BillDetail> PopulateData(bool isChecked)
    {
        if (this.Gv_List.Rows != null && this.Gv_List.Rows.Count > 0)
        {
            IList<BillDetail> billDetailList = new List<BillDetail>();
            foreach (GridViewRow row in this.Gv_List.Rows)
            {
                CheckBox checkBoxGroup = row.FindControl("CheckBoxGroup") as CheckBox;
                if (checkBoxGroup.Checked || !isChecked)
                {
                    HiddenField hfId = row.FindControl("hfId") as HiddenField;
                    TextBox tbQty = row.FindControl("tbQty") as TextBox;
                    TextBox tbDiscount = row.FindControl("tbDiscount") as TextBox;
                    TextBox tbAmount = row.FindControl("tbAmount") as TextBox;

                    BillDetail billDetail = new BillDetail();
                    billDetail.Id = int.Parse(hfId.Value);
                    billDetail.BilledQty = decimal.Parse(tbQty.Text);
                    billDetail.Discount = decimal.Parse(tbDiscount.Text);
                    billDetail.Amount = decimal.Parse(tbAmount.Text);

                    billDetailList.Add(billDetail);
                }
            }
            return billDetailList;
        }

        return null;
    }

    protected void rblListFormat_IndexChanged(object sender, EventArgs e)
    {
        Bill bill = this.TheBillMgr.LoadBill(this.BillNo, true);
        this.UpdateView(bill);
    }
    /*
    private IList<BillDetail> GroupBillDetail(IList<BillDetail> billDetails)
    {
        return (from b in billDetails
                group b by new { b.ActingBill.Item, b.ActingBill.ReferenceItemCode, b.ActingBill.Uom, b.UnitPrice, b.Currency } into g
                select new BillDetail
                {
                    Item = g.Key.Item,
                    ReferenceItemCode = g.Key.ReferenceItemCode,
                    Uom = g.Key.Uom,
                    UnitPrice = g.Key.UnitPrice,
                    Currency = g.Key.Currency,
                    BillQty = g.Sum(b => b.ActingBill.BillQty),
                    BilledQty = g.Sum(b => b.BilledQty),
                    GroupAmount = g.Sum(b => b.Amount) - g.Sum(b => b.Discount.HasValue ? b.Discount.Value : 0)
                }).ToList();
    }
     */
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;

public partial class Finance_Bill_NewList : ListModuleBase
{
    public string ModuleType
    {
        get { return (string)ViewState["ModuleType"]; }
        set { ViewState["ModuleType"] = value; }
    }

    private int DecimalLength
    {
        get { return (int)ViewState["DecimalLength"]; }
        set { ViewState["DecimalLength"] = value; }
    }

    public void BindDataSource(IList<ActingBill> actingBillList)
    {
        if (actingBillList == null)
        {
            actingBillList = new List<ActingBill>();
        }
        this.GV_List.DataSource = actingBillList;
        this.UpdateView();
    }

    public IList<ActingBill> PopulateSelectedData()
    {
        if (this.GV_List.Rows != null && this.GV_List.Rows.Count > 0)
        {
            IList<ActingBill> actingBillList = new List<ActingBill>();
            foreach (GridViewRow row in this.GV_List.Rows)
            {

                CheckBox checkBoxGroup = row.FindControl("CheckBoxGroup") as CheckBox;
                if (checkBoxGroup.Checked)
                {
                    HiddenField hfId = row.FindControl("hfId") as HiddenField;
                    TextBox tbQty = row.FindControl("tbQty") as TextBox;
                    TextBox tbDiscount = row.FindControl("tbDiscount") as TextBox;
                    TextBox tbAmount = row.FindControl("tbAmount") as TextBox;

                    ActingBill actingBill = new ActingBill();
                    actingBill.Id = int.Parse(hfId.Value);
                    actingBill.CurrentBillQty = decimal.Parse(tbQty.Text);
                    actingBill.CurrentDiscount = decimal.Parse(tbDiscount.Text);
                    actingBill.CurrentBillAmount = decimal.Parse(tbAmount.Text);
                    actingBillList.Add(actingBill);
                }
            }
            return actingBillList;
        }
        return null;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_PO)
            {
                this.GV_List.Columns[3].Visible = false;
            }
            else if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_SO)
            {
                this.GV_List.Columns[1].HeaderText = "${MasterData.ActingBill.Customer}";
                this.GV_List.Columns[2].Visible = false;
            }

            EntityPreference entityPreference = TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_AMOUNT_DECIMAL_LENGTH);
            DecimalLength = int.Parse(entityPreference.Value);
        }
    }

    public override void UpdateView()
    {
        this.GV_List.DataBind();
        if (this.GV_List.DataSource != null)
        {
            this.lblNoRecordFound.Visible = false;

        }
        else
        {
            this.lblNoRecordFound.Visible = true;
        }
        this.GV_List.Columns[0].Visible = true;
        this.GV_List.Columns[15].Visible = false;
        this.GV_List.Columns[16].Visible = false;
        this.GV_List.Columns[17].Visible = false;
    }

    public void ExportXLS()
    {
        this.GV_List.Columns[0].Visible = false;
        this.GV_List.Columns[15].Visible = true;
        this.GV_List.Columns[16].Visible = true;
        this.GV_List.Columns[17].Visible = true;
        this.ExportXLS(GV_List, "Bill.xls");
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ActingBill actingBill = (ActingBill)e.Row.DataItem;

            decimal billAmount = actingBill.BillAmount;
            decimal unitPrice = actingBill.UnitPrice;

            decimal remailQty = actingBill.BillQty - actingBill.BilledQty;
            decimal remailAmount = billAmount - actingBill.BilledAmount;
            decimal discount = unitPrice * remailQty - remailAmount;

            TextBox tbQty = e.Row.FindControl("tbQty") as TextBox;
            TextBox tbDiscountRate = e.Row.FindControl("tbDiscountRate") as TextBox;
            TextBox tbDiscount = e.Row.FindControl("tbDiscount") as TextBox;
            TextBox tbAmount = e.Row.FindControl("tbAmount") as TextBox;
            Literal ltlAmount = e.Row.FindControl("ltlAmount") as Literal;
            Literal ltlQty = e.Row.FindControl("ltlQty") as Literal;


            if (IsExport)
            {
                tbQty.Visible = false;
                tbDiscountRate.Visible = false;
                tbDiscount.Visible = false;
                tbAmount.Visible = false;
                ltlQty.Visible = true;
                ltlAmount.Visible = true;

                ltlQty.Text = remailQty.ToString("0.########");
                ltlAmount.Text = remailAmount.ToString("0.########");
                e.Row.Cells[3].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                e.Row.Cells[4].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                e.Row.Cells[6].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            }
            else
            {
                tbQty.Visible = true;
                tbDiscountRate.Visible = true;
                tbDiscount.Visible = true;
                tbAmount.Visible = true;
                ltlQty.Visible = false;
                ltlAmount.Visible = false;

                tbQty.Text = remailQty.ToString("0.########");
                if (unitPrice != 0 && remailQty != 0)
                {
                    tbDiscountRate.Text = (Math.Round(discount / (unitPrice * remailQty), this.DecimalLength, MidpointRounding.AwayFromZero) * 100).ToString("F2");
                }
                tbDiscount.Text = discount.ToString("0.########");
                tbAmount.Text = remailAmount.ToString("0.########");
                tbAmount.Attributes["oldValue"] = tbAmount.Text;
            }
        }
    }

    //add by ljz start
    protected void CheckBoxGroup_CheckedChanged(Object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;

        DataControlFieldCell dcf = (DataControlFieldCell)chk.Parent;    //这个对象的父类为cell  
        GridViewRow gr = (GridViewRow)dcf.Parent;                       //cell对象的父类为row
        string tbQty = ((TextBox)gr.FindControl("tbQty")).Text;
        string tbAmount = ((TextBox)gr.FindControl("tbAmount")).Text;

        if (chk.Checked)
        {
            ltlCurrentBillQtyTotal1.Text = (double.Parse(ltlCurrentBillQtyTotal1.Text) + double.Parse(tbQty)).ToString();
            ltlAmountTotal1.Text = (double.Parse(ltlAmountTotal1.Text) + double.Parse(tbAmount)).ToString();
        }
        else
        {
            ltlCurrentBillQtyTotal1.Text = (double.Parse(ltlCurrentBillQtyTotal1.Text) - double.Parse(tbQty)).ToString();
            ltlAmountTotal1.Text = (double.Parse(ltlAmountTotal1.Text) - double.Parse(tbAmount)).ToString();
        }
    }
    protected void CheckAll_CheckedChanged(Object sender, EventArgs e)
    {
        
    }

    public void InitializationTotal()
    {
        ltlAmountTotal1.Text = "0";
        ltlCurrentBillQtyTotal1.Text = "0";
    }
    //add by ljz end
}

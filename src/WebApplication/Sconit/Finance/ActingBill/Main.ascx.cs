using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using NHibernate.Expression;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using com.Sconit.Utility;

public partial class Finance_ActingBill_Main : MainModuleBase
{
    private int DecimalLength
    {
        get
        {
            EntityPreference entityPreference = TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_AMOUNT_DECIMAL_LENGTH);
            return int.Parse(entityPreference.Value);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string item = this.tbItem.Text.Trim();
        string receiptNo = this.tbReceiptNo.Text.Trim();
        string startDate = this.tbStartDate.Text.Trim();
        string endDate = this.tbEndDate.Text.Trim();

        List<string> Partys = this.CurrentUser.OrganizationPermission.Select(p => p.Code).ToList();
        #region DetachedCriteria

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(ActingBill));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(ActingBill))
            .SetProjection(Projections.Count("Id"));

        selectCriteria.CreateAlias("BillAddress", "ba");
        selectCountCriteria.CreateAlias("BillAddress", "ba");
        selectCriteria.CreateAlias("ba.Party", "pf");
        selectCountCriteria.CreateAlias("ba.Party", "pf");

        selectCriteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));
        selectCountCriteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));

        selectCriteria.Add(Expression.In("pf.Code", Partys));
        selectCountCriteria.Add(Expression.In("pf.Code", Partys));

        if (startDate != string.Empty)
        {
            selectCriteria.Add(Expression.Ge("CreateDate", DateTime.Parse(startDate)));
            selectCountCriteria.Add(Expression.Ge("CreateDate", DateTime.Parse(startDate)));
        }
        if (endDate != string.Empty)
        {
            selectCriteria.Add(Expression.Lt("CreateDate", DateTime.Parse(endDate).AddDays(1).AddMilliseconds(-1)));
            selectCountCriteria.Add(Expression.Lt("CreateDate", DateTime.Parse(endDate).AddDays(1).AddMilliseconds(-1)));
        }

        if (item != string.Empty)
        {
            //selectCriteria.CreateAlias("Item", "it");
            //selectCountCriteria.CreateAlias("Item", "it");

            //selectCriteria.Add(Expression.Eq("it.Code", this.tbItem.Text.Trim()));
            //selectCountCriteria.Add(Expression.Eq("it.Code", this.tbItem.Text.Trim()));

            selectCriteria.CreateAlias("Item", "i");
            selectCriteria.Add(
                Expression.Like("i.Code", this.tbItem.Text.Trim(), MatchMode.Anywhere) ||
                Expression.Like("i.Desc1", this.tbItem.Text.Trim(), MatchMode.Anywhere) ||
                Expression.Like("i.Desc2", this.tbItem.Text.Trim(), MatchMode.Anywhere)
                );

        }

        if (receiptNo != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("ReceiptNo", this.tbReceiptNo.Text.Trim()));
            selectCountCriteria.Add(Expression.Eq("ReceiptNo", this.tbReceiptNo.Text.Trim()));
        }

        selectCriteria.Add(Expression.Eq("TransactionType", BusinessConstants.BILL_TRANS_TYPE_PO));
        selectCountCriteria.Add(Expression.Eq("TransactionType", BusinessConstants.BILL_TRANS_TYPE_PO));

        new SessionHelper(this.Page).AddUserSelectCriteria(this.TemplateControl.AppRelativeVirtualPath, selectCriteria, selectCountCriteria);
        this.GV_List.Execute();
        if ((Button)sender == this.btnExport)
        {
            this.ExportXLS(GV_List, "ActingBill.xls");
        }
        #endregion
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

            Literal ltlAmount = e.Row.FindControl("ltlAmount") as Literal;
            Literal ltlQty = e.Row.FindControl("ltlQty") as Literal;
            Label lblIsProvisionalEstimate = e.Row.FindControl("lblIsProvisionalEstimate") as Label;

            if (actingBill.IsProvisionalEstimate)
            {
                lblIsProvisionalEstimate.Text = "暂估";
            }
            if (actingBill.BilledQty == actingBill.BillQty)
            {
                lblIsProvisionalEstimate.Text = "补开";
            }

            ltlQty.Visible = true;
            ltlAmount.Visible = true;

            ltlQty.Text = remailQty.ToString("0.########");
            ltlAmount.Text = remailAmount.ToString("0.########");
            e.Row.Cells[3].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[4].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[6].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
        }
    }

}

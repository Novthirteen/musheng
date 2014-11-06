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
using com.Sconit.Web;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Control;
using com.Sconit.Utility;
using com.Sconit.Entity.Cost;
using NHibernate.Expression;
using System.Collections.Generic;

public partial class Cost_CostAllocateTransaction_Edit : EditModuleBase
{
    public event EventHandler BackEvent;

    protected Int32 Id
    {
        get
        {
            return (Int32)ViewState["Id"];
        }
        set
        {
            ViewState["Id"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }


    public void InitPageParameter(Int32 id)
    {
        this.Id = id;
        CostAllocateTransaction costAllocateTransaction = TheCostAllocateTransactionMgr.LoadCostAllocateTransaction(id);

        this.tbCostElement.Text = costAllocateTransaction.CostElement.Code;
        this.tbCostCenter.Text = costAllocateTransaction.CostCenter.Code;
        this.ddlAllocateBy.Text = costAllocateTransaction.AllocateBy;

        this.tbDependCostElement.Text = costAllocateTransaction.DependCostElement.Code;
        this.tbExpenseElement.Text = costAllocateTransaction.ExpenseElement == null ? string.Empty : costAllocateTransaction.ExpenseElement.Code;
        this.tbEffDate.Text = costAllocateTransaction.EffectiveDate.ToShortDateString();
        this.tbItemCategorys.Text = costAllocateTransaction.ItemCategorys;
        this.tbItems.Text = costAllocateTransaction.Items;
        this.tbOrders.Text = costAllocateTransaction.Orders;
        this.tbAmount.Text = costAllocateTransaction.Amount.ToString("0.########");
        this.tbReferenceItems.Text = costAllocateTransaction.ReferenceItems;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!this.rfvAllocateBy.IsValid || !this.rfvCostCenter.IsValid || !this.rfvCostElement.IsValid || !this.rfvDependCostElement.IsValid
            || !this.rfvExpenseElement.IsValid)
        {
            return;
        }

        if (!CheckFinanceCalendarOpen())
        {
            ShowErrorMessage("Cost.CostAllocateTransaction.FinaceCalendar.Close");
            return;
        }

        CostAllocateTransaction costAllocateTransaction = TheCostAllocateTransactionMgr.LoadCostAllocateTransaction(this.Id);

        costAllocateTransaction.CostCenter = TheCostCenterMgr.LoadCostCenter(this.tbCostCenter.Text.Trim());
        costAllocateTransaction.CostElement = TheCostElementMgr.LoadCostElement(this.tbCostElement.Text.Trim());
        costAllocateTransaction.DependCostElement = TheCostElementMgr.LoadCostElement(this.tbDependCostElement.Text.Trim());
        costAllocateTransaction.ExpenseElement = TheExpenseElementMgr.LoadExpenseElement(this.tbExpenseElement.Text.Trim());
        costAllocateTransaction.AllocateBy = this.ddlAllocateBy.Text.Trim();
        costAllocateTransaction.Amount = decimal.Parse(this.tbAmount.Text.Trim());
        costAllocateTransaction.EffectiveDate = DateTime.Parse(this.tbEffDate.Text.Trim());
        costAllocateTransaction.Items = this.tbItems.Text.Trim();
        costAllocateTransaction.ItemCategorys = this.tbItemCategorys.Text.Trim();
        costAllocateTransaction.Orders = this.tbOrders.Text.Trim();
        costAllocateTransaction.CreateUser = this.CurrentUser.Code;
        costAllocateTransaction.CreateDate = DateTime.Now;
        costAllocateTransaction.ReferenceItems = this.tbReferenceItems.Text.Trim();

        try
        {

            TheCostAllocateTransactionMgr.UpdateCostAllocateTransaction(costAllocateTransaction);
            ShowSuccessMessage("Cost.CostAllocateTransactionMgr.Update.Successfully");

        }
        catch (Exception ex)
        {
            ShowErrorMessage("Cost.CostAllocateTransactionMgr.Update.Failed");
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            TheCostAllocateTransactionMgr.DeleteCostAllocateTransaction(this.Id);
            ShowSuccessMessage("Cost.CostAllocateTransaction.Delete.Successfully");
            if (BackEvent != null)
            {
                BackEvent(this, e);
            }
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Cost.CostAllocateTransaction.Delete.Failed");
        }
    }

    private bool CheckFinanceCalendarOpen()
    {
        bool isOpen = true;
        DateTime effDate = DateTime.Parse(this.tbEffDate.Text.Trim());

        FinanceCalendar finaceCalendar = TheFinanceCalendarMgr.GetLastestOpenFinanceCalendar();
        if (finaceCalendar.StartDate > effDate)
        {
            isOpen = false;
        }
        return isOpen;
    }

}

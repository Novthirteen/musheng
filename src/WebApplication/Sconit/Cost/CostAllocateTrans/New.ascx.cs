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
using com.Sconit.Web;
using com.Sconit.Utility;
using com.Sconit.Entity.Cost;
using NHibernate.Expression;
using System.Collections.Generic;

public partial class Cost_CostAllocateTransaction_New : NewModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler CreateEvent;

    private CostAllocateTransaction costAllocateTransaction;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    public void PageCleanup()
    {
        this.tbCostCenter.Text = string.Empty;
        this.tbCostElement.Text = string.Empty;
        this.tbDependCostElement.Text = string.Empty;
        this.tbExpenseElement.Text = string.Empty;
        this.ddlAllocateBy.SelectedIndex = 0;
        this.tbItemCategorys.Text = string.Empty;
        this.tbItems.Text = string.Empty;
        this.tbOrders.Text = string.Empty;
        this.tbEffDate.Text = string.Empty;
        this.tbAmount.Text = string.Empty;
        this.tbReferenceItems.Text = string.Empty;
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

        CostAllocateTransaction costAllocateTransaction = new CostAllocateTransaction();

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
            TheCostAllocateTransactionMgr.CreateCostAllocateTransaction(costAllocateTransaction);
            ShowSuccessMessage("Cost.CostAllocateTransaction.Add.Successfully");
            if (CreateEvent != null)
            {
                CreateEvent(costAllocateTransaction.Id, e);
            }
        }
        catch (Exception ex)
        {
            ShowSuccessMessage("Cost.CostAllocateTransaction.Add.Failed");
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

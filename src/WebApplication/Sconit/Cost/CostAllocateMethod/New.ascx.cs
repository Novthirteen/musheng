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

public partial class Cost_CostAllocateMethod_New : NewModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler CreateEvent;

    private CostAllocateMethod costAllocateMethod;
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
        this.tbCostGroup.Text = string.Empty;
        this.tbCostCenter.Text = string.Empty;
        this.tbCostElement.Text = string.Empty;
        this.tbDependCostElement.Text = string.Empty;
        this.tbExpenseElement.Text = string.Empty;
        this.ddlAllocateBy.SelectedIndex = 0;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!this.rfvAllocateBy.IsValid || !this.rfvCostCenter.IsValid || !this.rfvCostElement.IsValid || !this.rfvCostGroup.IsValid || !this.rfvDependCostElement.IsValid
            || !this.rfvExpenseElement.IsValid)
        {
            return;
        }
        CostAllocateMethod costAllocateMethod = new CostAllocateMethod();
        string costGroup = this.tbCostGroup.Text.Trim();
        if (costGroup != string.Empty)
        {
            costAllocateMethod.CostGroup = TheCostGroupMgr.LoadCostGroup(costGroup);
        }

        string costCenter = this.tbCostCenter.Text.Trim();
        if (costCenter != string.Empty)
        {
            costAllocateMethod.CostCenter = TheCostCenterMgr.LoadCostCenter(costCenter);
        }

        string costElement = this.tbCostElement.Text.Trim();
        if (costElement != string.Empty)
        {
            costAllocateMethod.CostElement = TheCostElementMgr.LoadCostElement(costElement);
        }

        string dependCostElement = this.tbDependCostElement.Text.Trim();
        if (dependCostElement != string.Empty)
        {
            costAllocateMethod.DependCostElement = TheCostElementMgr.LoadCostElement(dependCostElement);
        }

        string expenseElement = this.tbExpenseElement.Text.Trim();
        if (expenseElement != string.Empty)
        {
            costAllocateMethod.ExpenseElement = TheExpenseElementMgr.LoadExpenseElement(expenseElement);
        }

        string allocateBy = this.ddlAllocateBy.Text.Trim();
        if (allocateBy != string.Empty)
        {
            costAllocateMethod.AllocateBy = allocateBy;
        }

        try
        {
            if (IsCostAllocateMethodExists(costAllocateMethod))
            {
                ShowErrorMessage("Cost.CostAllocateMethodMgr.Exists");
                return;
            }
            TheCostAllocateMethodMgr.CreateCostAllocateMethod(costAllocateMethod);
            ShowSuccessMessage("Cost.CostAllocateMethodMgr.Add.Successfully");
            if (CreateEvent != null)
            {
                CreateEvent(costAllocateMethod.Id, e);
            }
        }
        catch (Exception ex)
        {
            ShowSuccessMessage("Cost.CostAllocateMethodMgr.Add.Failed");
        }
    }


    private bool IsCostAllocateMethodExists(CostAllocateMethod costAllocateMethod)
    {
        DetachedCriteria criteria = DetachedCriteria.For(typeof(CostAllocateMethod)).SetProjection(Projections.Count("Id")); ;
        criteria.Add(Expression.Eq("CostGroup.Code", costAllocateMethod.CostGroup.Code));
        criteria.Add(Expression.Eq("CostCenter.Code", costAllocateMethod.CostCenter.Code));
        criteria.Add(Expression.Eq("CostElement.Code", costAllocateMethod.CostElement.Code));
        criteria.Add(Expression.Eq("DependCostElement.Code", costAllocateMethod.DependCostElement.Code));
        criteria.Add(Expression.Eq("ExpenseElement.Code", costAllocateMethod.ExpenseElement.Code));
        criteria.Add(Expression.Eq("AllocateBy", costAllocateMethod.AllocateBy));
        IList<int> result = TheCriteriaMgr.FindAll<int>(criteria);
        
        return result[0] == 0;
    }
}

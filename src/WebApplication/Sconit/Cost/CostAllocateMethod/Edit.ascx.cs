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

public partial class Cost_CostAllocateMethod_Edit : EditModuleBase
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
        CostAllocateMethod costAllocateMethod = TheCostAllocateMethodMgr.LoadCostAllocateMethod(id);
        if (costAllocateMethod.CostElement != null)
        {
            this.tbCostElement.Text = costAllocateMethod.CostElement.Code;
        }
        if (costAllocateMethod.CostCenter != null)
        {
            this.tbCostCenter.Text = costAllocateMethod.CostCenter.Code;
        }
        if (costAllocateMethod.AllocateBy != null)
        {
            this.ddlAllocateBy.Text = costAllocateMethod.AllocateBy;
        }
        if (costAllocateMethod.CostGroup != null)
        {
            this.tbCostGroup.Text = costAllocateMethod.CostGroup.Code;
        }
        if (costAllocateMethod.DependCostElement != null)
        {
            this.tbDependCostElement.Text = costAllocateMethod.DependCostElement.Code;
        }
        if (costAllocateMethod.ExpenseElement != null)
        {
            this.tbExpenseElement.Text = costAllocateMethod.ExpenseElement.Code;
        }
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
            ShowSuccessMessage("Cost.CostAllocateMethodMgr.Update.Successfully");
          
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Cost.CostAllocateMethodMgr.Update.Failed");
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            TheCostAllocateMethodMgr.DeleteCostAllocateMethod(this.Id);
            ShowSuccessMessage("Cost.CostAllocateMethod.Delete.Successfully");
            if (BackEvent != null)
            {
                BackEvent(this, e);
            }
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Cost.CostAllocateMethod.Delete.Failed");
        }
    }

    private bool IsCostAllocateMethodExists(CostAllocateMethod costAllocateMethod)
    {
        bool isExists = false;
        DetachedCriteria criteria = DetachedCriteria.For(typeof(CostAllocateMethod));
        criteria.Add(Expression.Eq("CostGroup.Code", costAllocateMethod.CostGroup.Code));
        criteria.Add(Expression.Eq("CostCenter.Code", costAllocateMethod.CostCenter.Code));
        criteria.Add(Expression.Eq("CostElement.Code", costAllocateMethod.CostElement.Code));
        criteria.Add(Expression.Eq("DependCostElement.Code", costAllocateMethod.DependCostElement.Code));
        criteria.Add(Expression.Eq("ExpenseElement.Code", costAllocateMethod.ExpenseElement.Code));
        criteria.Add(Expression.Eq("AllocateBy", costAllocateMethod.AllocateBy));
        IList<CostAllocateMethod> result = TheCriteriaMgr.FindAll<CostAllocateMethod>(criteria);

        if (result.Count > 0 && result[0].Id != this.Id)
        {
            isExists = true;
        }
        return isExists;
    }

}

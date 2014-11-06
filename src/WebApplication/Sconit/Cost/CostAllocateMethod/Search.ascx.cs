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
using System.Collections.Generic;
using NHibernate.Expression;
using com.Sconit.Entity.MasterData;
using com.Sconit.Web;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.Cost;

public partial class Cost_CostAllocateMethod_Search : SearchModuleBase
{
    public event EventHandler SearchEvent;
    public event EventHandler NewEvent;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {

    }

    protected override void DoSearch()
    {
        string costGroup = this.tbCostGroup.Text.Trim();
        string costCenter = this.tbCostCenter.Text.Trim();
        string costElement = this.tbCostElement.Text.Trim();
        string dependCostElement = this.tbDependCostElement.Text.Trim();
        string expenseElement =this.tbExpenseElement.Text.Trim();
        string allocateBy = this.ddlAllocateBy.Text.Trim();
        if (SearchEvent != null)
        {
            #region DetachedCriteria

            DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(CostAllocateMethod));
            DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(CostAllocateMethod)).SetProjection(Projections.Count("Id"));
            if (costGroup != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("CostGroup.Code", costGroup));
                selectCountCriteria.Add(Expression.Eq("CostGroup.Code", costGroup));
            }

            if (costCenter != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("CostCenter.Code", costCenter));
                selectCountCriteria.Add(Expression.Eq("CostCenter.Code", costCenter));
            }

            if (costElement != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("CostElement.Code", costElement));
                selectCountCriteria.Add(Expression.Eq("CostElement.Code", costElement));
            }
            if (dependCostElement != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("DependCostElement.Code", dependCostElement));
                selectCountCriteria.Add(Expression.Eq("DependCostElement.Code", dependCostElement));
            }
            if (expenseElement != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("ExpenseElement.Code", expenseElement));
                selectCountCriteria.Add(Expression.Eq("ExpenseElement.Code", expenseElement));
            }
            if (allocateBy != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("AllocateBy", allocateBy));
                selectCountCriteria.Add(Expression.Eq("AllocateBy", allocateBy));
            }

            SearchEvent((new object[] { selectCriteria, selectCountCriteria }), null);
            #endregion
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        if (NewEvent != null)
        {
            NewEvent(sender, e);
        }
    }


}

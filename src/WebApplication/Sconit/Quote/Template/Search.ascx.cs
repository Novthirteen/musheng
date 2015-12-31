using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using NHibernate.Expression;
using com.Sconit.Entity.Quote;

public partial class Quote_Template_Search : SearchModuleBase
{
    public event EventHandler NewEvent;
    public event EventHandler SearchEvent;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }

    private object[] CollectParam()
    {

        #region DetachedCriteria

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(CostCategory));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(CostCategory)).SetProjection(Projections.Count("Id"));
        if (txtCostCategory.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("Name", txtCostCategory.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("Name", txtCostCategory.Text.Trim(), MatchMode.Anywhere));
        }

        return (new object[] { selectCriteria, selectCountCriteria });

        #endregion

    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        NewEvent(sender, e);
    }

    protected override void DoSearch()
    {
        if (SearchEvent != null)
        {
            object[] criteriaParam = CollectParam();
            SearchEvent(criteriaParam, null);
        }
    }
    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    { }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using NHibernate.Expression;
using com.Sconit.Entity.Quote;

public partial class Quote_Item_Search : SearchModuleBase
{
    public EventHandler SearchEvent;
    public EventHandler NewEvent;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    { }

    protected override void DoSearch()
    { }
    public void btnSearch_Click(object sender, EventArgs e)
    {
        object[] criteriaParam = CollectParam();
        SearchEvent(criteriaParam, null);
    }

    public void btnNew_Click(object sender, EventArgs e)
    {
        NewEvent(sender, e);
    }

    private object[] CollectParam()
    {

        #region DetachedCriteria

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(QuoteItem));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(QuoteItem)).SetProjection(Projections.Count("Id"));
        if (txtProjectId.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("ProjectId", txtProjectId.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("ProjectId", txtProjectId.Text.Trim(), MatchMode.Anywhere));
        }

        if (txtItemCode.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("ItemCode", txtItemCode.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("ItemCode", txtItemCode.Text.Trim(), MatchMode.Anywhere));
        }

        return (new object[] { selectCriteria, selectCountCriteria });

        #endregion

    }
}
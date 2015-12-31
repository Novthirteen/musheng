using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using NHibernate.Expression;
using com.Sconit.Entity.Quote;

public partial class Quote_Quotes_ProjectSearch : SearchModuleBase
{
    public EventHandler SearchEvent;
    public EventHandler NewEvent;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }

    public void btnNew_Click(object sender, EventArgs e)
    {
        NewEvent(sender,e);
    }
    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    { }

    protected override void DoSearch()
    {
        object[] criteriaParam = CollectParam();
        SearchEvent(criteriaParam, null);
    }

    private object[] CollectParam()
    {

        #region DetachedCriteria

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(Project));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(Project)).SetProjection(Projections.Count("Id"));
        if (txtProjectId.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("ProjectId", txtProjectId.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("ProjectId", txtProjectId.Text.Trim(), MatchMode.Anywhere));
        }

        if (txtVision.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("PVision", txtVision.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("PVision", txtVision.Text.Trim(), MatchMode.Anywhere));
        }

        if (txtCustomer.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("CustomerCode", txtCustomer.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("CustomerCode", txtCustomer.Text.Trim(), MatchMode.Anywhere));
        }

        if (txtProductNo.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("ProductNo", txtProductNo.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("ProductNo", txtProductNo.Text.Trim(), MatchMode.Anywhere));
        }

        return (new object[] { selectCriteria, selectCountCriteria });

        #endregion

    }
}
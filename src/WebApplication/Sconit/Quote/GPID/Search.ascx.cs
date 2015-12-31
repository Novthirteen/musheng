using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using NHibernate.Expression;
using com.Sconit.Entity.Quote;

public partial class Quote_GPID_Search : SearchModuleBase
{
    public EventHandler NewEvent;
    public EventHandler SearchEvent;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void btnNew_Click(object sender, EventArgs e)
    {
        NewEvent(sender, e);
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
    private object[] CollectParam()
    {

        #region DetachedCriteria

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(GPID));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(GPID)).SetProjection(Projections.Count("ID"));
        if (txtPID.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("ID", txtPID.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("ID", txtPID.Text.Trim(), MatchMode.Anywhere));
        }

        if (txtCustomer.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("CustomerCode", txtCustomer.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("CustomerCode", txtCustomer.Text.Trim(), MatchMode.Anywhere));
        }

        return (new object[] { selectCriteria, selectCountCriteria });

        #endregion

    }
}
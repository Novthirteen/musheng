using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using NHibernate.Expression;
using com.Sconit.Entity.MasterData;

public partial class MasterData_ItemPack_Search : SearchModuleBase
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

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {
    }

    private object[] CollectParam()
    {
        
        #region DetachedCriteria

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(ItemPack));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(ItemPack)).SetProjection(Projections.Count("Id"));
        if (txtSpec.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("Spec", txtSpec.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("Spec", txtSpec.Text.Trim(), MatchMode.Anywhere));
        }
        return (new object[] { selectCriteria, selectCountCriteria });

        #endregion

    }

    protected override void DoSearch()
    {
        object[] criteriaParam = CollectParam();
        SearchEvent(criteriaParam, null);
    }

    public void btnNew_Click(object sender, EventArgs e)
    {
        NewEvent(sender,e);
    }
}
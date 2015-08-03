using com.Sconit.Entity.MasterData;
using NHibernate.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterData_ItemMap_Search : System.Web.UI.UserControl
{

    public event EventHandler SearchEvent;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private object[] CollectParam()
    {
        string item = this.tbItem.Text.Trim();
        string disConItem = this.tbMapItem.Text.Trim();
        string startDate = this.tbStartDate.Text.Trim();
        string endDate = this.tbEndDate.Text.Trim();


        #region DetachedCriteria

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(ItemMap));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(ItemMap)).SetProjection(Projections.Count("Id"));
        if (item != string.Empty)
        {
            selectCriteria.Add(Expression.Like("Item.Code", item, MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("Item.Code", item, MatchMode.Anywhere));
        }

        if (disConItem != string.Empty)
        {
            selectCriteria.Add(Expression.Like("MapItem.Code", disConItem, MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("MapItem.Code", disConItem, MatchMode.Anywhere));
        }


        if (startDate != string.Empty)
        {
            selectCriteria.Add(Expression.Like("StartDate", startDate, MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("StartDate", startDate, MatchMode.Anywhere));
        }

        if (endDate != string.Empty)
        {
            selectCriteria.Add(Expression.Like("EndDate", endDate, MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("EndDate", endDate, MatchMode.Anywhere));
        }
        return new object[] { selectCriteria, selectCountCriteria };
        //SearchEvent((new object[] { selectCriteria, selectCountCriteria }), null);
        #endregion

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }

    protected void DoSearch()
    {
        if (SearchEvent != null)
        {
            object[] param = CollectParam();
            if (param != null)
                SearchEvent(param, null);
        }
    }
}
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

public partial class Cost_FinanceCalendar_Search : SearchModuleBase
{
    public event EventHandler SearchEvent;
    public event EventHandler NewEvent;
    public event EventHandler UpUpBillPeriodEvent;
   
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }

    protected void btnUpBillPeriod_Click(object sender, EventArgs e)
    {
        if (UpUpBillPeriodEvent != null)
        {
            UpUpBillPeriodEvent(sender, e);
        }
    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {

    }

    protected override void DoSearch()
    {
        string year = this.tbYear.Text.Trim();
        string month = this.tbMonth.Text.Trim();

        if (SearchEvent != null)
        {
            #region DetachedCriteria

            DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(FinanceCalendar));
            DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(FinanceCalendar)).SetProjection(Projections.Count("Id"));
            if (year != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("FinanceYear", int.Parse(year)));
                selectCountCriteria.Add(Expression.Eq("FinanceYear", int.Parse(year)));
            }

            if (month != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("FinanceMonth", int.Parse(month)));
                selectCountCriteria.Add(Expression.Eq("FinanceMonth", int.Parse(month)));
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

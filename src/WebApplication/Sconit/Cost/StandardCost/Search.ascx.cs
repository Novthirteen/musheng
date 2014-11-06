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

public partial class Cost_StandardCost_Search : SearchModuleBase
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
        string itemCode = this.tbItemCode.Text.Trim();
        string costGroup = this.tbCostGroup.Text.Trim();
        string costElement = this.tbCostElement.Text.Trim();
        if (SearchEvent != null)
        {
            #region DetachedCriteria

            DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(StandardCost));
            DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(StandardCost)).SetProjection(Projections.Count("Id"));
            if (itemCode != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("Item", itemCode));
                selectCountCriteria.Add(Expression.Eq("Item", itemCode));
            }

            if (costGroup != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("CostGroup.Code", costGroup));
                selectCountCriteria.Add(Expression.Eq("CostGroup.Code", costGroup));
            }

            if (costElement != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("CostElement.Code", costElement));
                selectCountCriteria.Add(Expression.Eq("CostElement.Code", costElement));
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

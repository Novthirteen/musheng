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
using com.Sconit.Utility;

public partial class MasterData_ItemCategory_Search : SearchModuleBase
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

        if (actionParameter.ContainsKey("Code"))
        {
            this.tbCode.Text = actionParameter["Code"];
        }
        if (actionParameter.ContainsKey("Description"))
        {
            this.tbDesc.Text = actionParameter["Description"];
        }
    }

    protected override void DoSearch()
    {
        string code = this.tbCode.Text.Trim();
        string desc = this.tbDesc.Text.Trim();

        if (SearchEvent != null)
        {
            #region DetachedCriteria

            DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(ItemCategory));
            DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(ItemCategory)).SetProjection(Projections.Count("Code"));
            if (code != string.Empty)
            {
                selectCriteria.Add(Expression.Like("Code", code, MatchMode.Anywhere));
                selectCountCriteria.Add(Expression.Like("Code", code, MatchMode.Anywhere));
            }

            if (desc != string.Empty)
            {
                selectCriteria.Add(Expression.Like("Description", desc, MatchMode.Anywhere));
                selectCountCriteria.Add(Expression.Like("Description", desc, MatchMode.Anywhere));
            }

            SearchEvent((new object[] { selectCriteria, selectCountCriteria }), null);
            #endregion
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        NewEvent(sender, e);
    }
}

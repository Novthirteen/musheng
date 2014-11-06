using System;
using System.Collections;
using System.Collections.Generic;
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
using com.Sconit.Web;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using NHibernate.Expression;
using com.Sconit.Entity.Customize;

public partial class MasterData_Facility_Search : SearchModuleBase
{
    public event EventHandler SearchEvent;
    public event EventHandler NewEvent;
    public event EventHandler BackEvent;

    public string ModuleType
    {
        get
        {
            return (string)ViewState["ModuleType"];
        }
        set
        {
            ViewState["ModuleType"] = value;
        }
    }

    public string FlowCode
    {
        get
        {
            return (string)ViewState["FlowCode"];
        }
        set
        {
            ViewState["FlowCode"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {

        if (actionParameter.ContainsKey("FlowCode"))
        {
            this.FlowCode = actionParameter["FlowCode"];
        }
        if (actionParameter.ContainsKey("Code"))
        {
            this.tbCode.Text = actionParameter["Code"];
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }
    protected override void DoSearch()
    {
        if (SearchEvent != null)
        {
            string code = this.tbCode.Text != string.Empty ? this.tbCode.Text.Trim() : string.Empty;

            #region DetachedCriteria查询

            DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(ProductLineFacility));
            DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(ProductLineFacility))
                .SetProjection(Projections.Count("Code"));
            selectCriteria.AddOrder(Order.Asc("Code"));

            if (FlowCode != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("ProductLine", FlowCode));
                selectCountCriteria.Add(Expression.Eq("ProductLine", FlowCode));
            }

            if (code != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("Code", code));
                selectCountCriteria.Add(Expression.Eq("Code", code));
            }

            SearchEvent((new object[] { selectCriteria, selectCountCriteria }), null);
            #endregion
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        if (NewEvent != null)
        {
            NewEvent(this, e);
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

}

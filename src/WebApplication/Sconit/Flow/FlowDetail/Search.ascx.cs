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

public partial class MasterData_FlowDetail_Search : SearchModuleBase
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
        if (actionParameter.ContainsKey("ItemCode"))
        {
            this.tbItemCode.Text = actionParameter["ItemCode"];
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
            string code = this.tbItemCode.Text != string.Empty ? this.tbItemCode.Text.Trim() : string.Empty;

            #region DetachedCriteria查询

            Flow flow = TheFlowMgr.LoadFlow(FlowCode);

            DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(FlowDetail));
            DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(FlowDetail))
                .SetProjection(Projections.Count("Id"));
            selectCriteria.AddOrder(Order.Asc("Sequence"));

            selectCriteria.Add(Expression.Disjunction().Add(Expression.Ge("EndDate", DateTime.Now.Date)).Add(Expression.IsNull("EndDate")));
            selectCountCriteria.Add(Expression.Disjunction().Add(Expression.Ge("EndDate", DateTime.Now.Date)).Add(Expression.IsNull("EndDate")));
            //selectCriteria.Add(Expression.Disjunction() .Add(Expression.IsNull("EndDate")));

            selectCriteria.Add(Expression.Gt("MRPWeight", 0));
            selectCountCriteria.Add(Expression.Gt("MRPWeight", 0));

            if (flow.ReferenceFlow != null && flow.ReferenceFlow.Trim() != string.Empty)
            {
                //添加零件组引用明细
                selectCriteria.Add(Expression.Or(Expression.Eq("Flow.Code", FlowCode), Expression.Eq("Flow.Code", flow.ReferenceFlow)));
                selectCountCriteria.Add(Expression.Or(Expression.Eq("Flow.Code", FlowCode), Expression.Eq("Flow.Code", flow.ReferenceFlow)));
            }
            else
            {
                selectCriteria.Add(Expression.Eq("Flow.Code", FlowCode));
                selectCountCriteria.Add(Expression.Eq("Flow.Code", FlowCode));
            }

            if (code != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("Item.Code", code));
                selectCountCriteria.Add(Expression.Eq("Item.Code", code));
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

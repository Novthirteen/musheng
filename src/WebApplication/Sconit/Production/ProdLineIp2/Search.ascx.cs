using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Entity;
using com.Sconit.Web;
using NHibernate.Expression;
using com.Sconit.Entity.View;
using com.Sconit.Utility;
using com.Sconit.Entity.Customize;

public partial class Production_ProdLineIp2_Search : SearchModuleBase
{
    public event EventHandler SearchEvent;
    public event EventHandler ExportEvent;

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

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.DoSearch();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (ExportEvent != null)
        {
            object[] param = this.CollectParam();
            if (param != null)
                ExportEvent(param, null);
        }
    }

    protected override void DoSearch()
    {

        if (SearchEvent != null)
        {
            object[] param = CollectParam();
            if (param != null)
                SearchEvent(param, null);
        }
    }

    private object[] CollectParam()
    {
        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(ProdLineIp2));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(ProdLineIp2))
            .SetProjection(Projections.Count("Id"));

        IDictionary<string, string> alias = new Dictionary<string, string>();

        if (this.tbOrderNo.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("OrderNo", this.tbOrderNo.Text.Trim()));
            selectCountCriteria.Add(Expression.Eq("OrderNo", this.tbOrderNo.Text.Trim()));
        }

        if (this.tbProdLine.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("ProdLine", this.tbProdLine.Text.Trim()));
            selectCountCriteria.Add(Expression.Eq("ProdLine", this.tbProdLine.Text.Trim()));
        }

        if (this.tbProdLineFact.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("ProdLineFact", this.tbProdLineFact.Text.Trim()));
            selectCountCriteria.Add(Expression.Eq("ProdLineFact", this.tbProdLineFact.Text.Trim()));
        }

        if (this.tbHu.Text.Trim() != string.Empty || this.tbHuLotNo.Text.Trim() != string.Empty)
        {
            selectCriteria.CreateAlias("Hu", "hu");
            selectCountCriteria.CreateAlias("Hu", "hu");
            alias.Add("Hu", "hu");
        }

        if (this.tbHuLotNo.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("hu.LotNo", this.tbHuLotNo.Text.Trim()));
            selectCountCriteria.Add(Expression.Eq("hu.LotNo", this.tbHuLotNo.Text.Trim()));
        }
        if (this.tbHu.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("hu.HuId", this.tbHu.Text.Trim()));
            selectCountCriteria.Add(Expression.Eq("hu.HuId", this.tbHu.Text.Trim()));
        }

        if (this.tbItem.Text.Trim() != string.Empty)
        {
            // selectCriteria.Add(Expression.Eq("Item", this.tbItem.Text.Trim()));
            // selectCountCriteria.Add(Expression.Eq("Item", this.tbItem.Text.Trim()));

            selectCriteria.Add(
                   Expression.Like("Item", this.tbItem.Text.Trim(), MatchMode.Anywhere) ||
                   Expression.Like("ItemDescription", this.tbItem.Text.Trim(), MatchMode.Anywhere));

            selectCountCriteria.Add(
                 Expression.Like("Item", this.tbItem.Text.Trim(), MatchMode.Anywhere) ||
                 Expression.Like("ItemDescription", this.tbItem.Text.Trim(), MatchMode.Anywhere));

        }

        if (this.tbCreateDateFrom.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Ge("CreateDate", DateTime.Parse(this.tbCreateDateFrom.Text.Trim())));
            selectCountCriteria.Add(Expression.Ge("CreateDate", DateTime.Parse(this.tbCreateDateFrom.Text.Trim())));
        }

        if (this.tbCreateDateTo.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Le("CreateDate", DateTime.Parse(this.tbCreateDateTo.Text.Trim()).AddDays(1).AddMilliseconds(-1)));
            selectCountCriteria.Add(Expression.Le("CreateDate", DateTime.Parse(this.tbCreateDateTo.Text.Trim()).AddDays(1).AddMilliseconds(-1)));
        }

        if (this.tbFG.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("FG", this.tbFG.Text.Trim()));
            selectCountCriteria.Add(Expression.Eq("FG", this.tbFG.Text.Trim()));
        }

        return new object[] { selectCriteria, selectCountCriteria, alias };

    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {
        //if (actionParameter.ContainsKey("Location"))
        //{
        //    this.tbLocation.Text = actionParameter["Location"];
        //}
        //if (actionParameter.ContainsKey("Item"))
        //{
        //    this.tbItem.Text = actionParameter["Item"];
        //}
        //if (actionParameter.ContainsKey("EffDate"))
        //{
        //    this.tbEffDate.Text = actionParameter["EffDate"];
        //}
    }
}
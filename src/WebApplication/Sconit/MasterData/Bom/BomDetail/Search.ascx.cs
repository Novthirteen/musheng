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
using com.Sconit.Web;
using NHibernate.Expression;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;

public partial class MasterData_Bom_BomDetail_Search : SearchModuleBase
{
    public event EventHandler SearchEvent;
    public event EventHandler NewEvent;
    public event EventHandler BtnImportClick;
    public event EventHandler ExportEvent;

    public bool IsView
    {
        get
        {
            return (bool)ViewState["IsView"];
        }
        set
        {
            ViewState["IsView"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && this.IsView == true)
        {
            //this.btnExport.Visible = false;
            this.btnImport.Visible = false;
            this.btnNew.Visible = false;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        if (SearchEvent != null)
        {
            SearchEvent(new object[] { this.tbParCode.Text, this.tbCompCode.Text, this.cbIncludeInactive.Checked, true }, null);
            //object[] criteriaParam = CollectParam();
            //if (criteriaParam != null)
            //{
            //    SearchEvent(criteriaParam, null);
            //}
        }
    }


    private object[] CollectParam()
    {
        string parcode = this.tbParCode.Text.Trim() != string.Empty ? this.tbParCode.Text.Trim() : string.Empty;
        string compcode = this.tbCompCode.Text.Trim() != string.Empty ? this.tbCompCode.Text.Trim() : string.Empty;
        bool isIncludeInactive = this.cbIncludeInactive.Checked;

        #region DetachedCriteria
        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(BomDetail));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(BomDetail))
            .SetProjection(Projections.Count("Id"));

        selectCriteria.CreateAlias("Bom", "b");
        selectCountCriteria.CreateAlias("Bom", "b");

        if (parcode != string.Empty)
        {
            selectCriteria.Add(Expression.Like("b.Code", parcode, MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("b.Code", parcode, MatchMode.Anywhere));
        }
        if (compcode != string.Empty)
        {
            selectCriteria.Add(Expression.Like("Item.Code", compcode, MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("Item.Code", compcode, MatchMode.Anywhere));
        }
        if (!isIncludeInactive)
        {
            selectCriteria.Add(Expression.Eq("b.IsActive", true));
            selectCountCriteria.Add(Expression.Eq("b.IsActive", true));

            selectCriteria.Add(Expression.Le("StartDate", DateTime.Now));
            selectCountCriteria.Add(Expression.Le("StartDate", DateTime.Now));
            selectCriteria.Add(Expression.Or(Expression.Ge("EndDate", DateTime.Now), Expression.IsNull("EndDate")));
            selectCountCriteria.Add(Expression.Or(Expression.Ge("EndDate", DateTime.Now), Expression.IsNull("EndDate")));
        }

        #endregion

        return new object[] { selectCriteria, selectCountCriteria };
    }

    protected void cbIncludeInactive_CheckedChanged(Object sender, EventArgs e)
    {
        this.tbParCode.ServiceParameter = "bool:" + this.cbIncludeInactive.Checked;
        this.tbParCode.DataBind();
    }

    protected override void DoSearch()
    {
        if (SearchEvent != null)
        {
            SearchEvent(new object[] { this.tbParCode.Text, this.tbCompCode.Text, this.cbIncludeInactive.Checked, false }, null);
            //object[] criteriaParam = CollectParam();
            //if (criteriaParam != null)
            //{
            //    SearchEvent(criteriaParam, null);
            //}
        }
    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {
        if (actionParameter.ContainsKey("ParCode"))
        {
            this.tbParCode.Text = actionParameter["ParCode"];
        }
        if (actionParameter.ContainsKey("CompCode"))
        {
            this.tbCompCode.Text = actionParameter["CompCode"];
        }
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        if (BtnImportClick != null)
        {
            BtnImportClick(sender, e);
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        /*
        if (ExportEvent != null)
        {
            object[] param = this.CollectParam();
            if (param != null)
            {
                ExportEvent(param, null);
            }
        }
        */

        object[] param = this.CollectParam();

        IList<BomDetail> bomDetailList = this.TheCriteriaMgr.FindAll<BomDetail>((DetachedCriteria)param[0]);
        IList<object> list = new List<object>();
        if (bomDetailList != null && bomDetailList.Count > 0)
        {
            list.Add(bomDetailList);
        }
        TheReportMgr.WriteToClient("BomDetailSample.xls", list, "BomDetailSample.xls");
    }
}

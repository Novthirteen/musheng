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
using com.Sconit.Entity.Customize;

public partial class Production_PLFeedSeqInfo_Search : SearchModuleBase
{
    public event EventHandler SearchEvent;
    public event EventHandler NewEvent;
    public event EventHandler ExportEvent;
    public event EventHandler BtnImportClick;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (ExportEvent != null)
        {
            object[] param = this.CollectParam();
            if (param != null)
            {
                ExportEvent(param, null);
            }
        }
    }


    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {

        if (actionParameter.ContainsKey("ProductLineFacility"))
        {
            this.tbProductLineFacility.Text = actionParameter["ProductLineFacility"];
        }
        if (actionParameter.ContainsKey("FinishGood"))
        {
            this.tbFinishGood.Text = actionParameter["FinishGood"];
        }
        if (actionParameter.ContainsKey("RawMaterial"))
        {
            this.tbRawMaterial.Text = actionParameter["RawMaterial"];
        }
    }

    private object[] CollectParam()
    {
        string productLineFacility = this.tbProductLineFacility.Text.Trim();
        string code = this.tbCode.Text.Trim();
        string finishGood = this.tbFinishGood.Text.Trim();
        string rawMaterial = this.tbRawMaterial.Text.Trim();

        #region DetachedCriteria

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(ProdutLineFeedSeqence));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(ProdutLineFeedSeqence)).SetProjection(Projections.Count("Id"));
        if (productLineFacility != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("ProductLineFacility", productLineFacility));
            selectCountCriteria.Add(Expression.Eq("ProductLineFacility", productLineFacility));
        }

        if (code != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("Code", code));
            selectCountCriteria.Add(Expression.Eq("Code", code));
        }

        if (finishGood != string.Empty)
        {
           // selectCriteria.Add(Expression.Eq("FinishGood.Code", finishGood));
          //  selectCountCriteria.Add(Expression.Eq("FinishGood.Code", finishGood));

            selectCriteria.CreateAlias("FinishGood", "f");
            selectCriteria.Add(
                  Expression.Like("f.Code", finishGood, MatchMode.Anywhere) ||
                  Expression.Like("f.Desc1", finishGood, MatchMode.Anywhere) ||
                  Expression.Like("f.Desc2", finishGood, MatchMode.Anywhere)
                  );
            selectCountCriteria.CreateAlias("FinishGood", "f");
            selectCountCriteria.Add(
                Expression.Like("f.Code", finishGood, MatchMode.Anywhere) ||
                Expression.Like("f.Desc1", finishGood, MatchMode.Anywhere) ||
                Expression.Like("f.Desc2", finishGood, MatchMode.Anywhere)
                );
        }

        if (rawMaterial != string.Empty)
        {
            //selectCriteria.Add(Expression.Eq("RawMaterial.Code", rawMaterial));
            //selectCountCriteria.Add(Expression.Eq("RawMaterial.Code", rawMaterial));

            selectCriteria.CreateAlias("RawMaterial", "r");
            selectCriteria.Add(
                  Expression.Like("r.Code", rawMaterial, MatchMode.Anywhere) ||
                  Expression.Like("r.Desc1", rawMaterial, MatchMode.Anywhere) ||
                  Expression.Like("r.Desc2", rawMaterial, MatchMode.Anywhere)
                  );
            selectCountCriteria.CreateAlias("RawMaterial", "r");
            selectCountCriteria.Add(
                Expression.Like("r.Code", rawMaterial, MatchMode.Anywhere) ||
                Expression.Like("r.Desc1", rawMaterial, MatchMode.Anywhere) ||
                Expression.Like("r.Desc2", rawMaterial, MatchMode.Anywhere)
                );
        }

        #endregion

        return (new object[] { selectCriteria, selectCountCriteria });
    }

    protected override void DoSearch()
    {
        if (SearchEvent != null)
        {
            object[] criteriaParam = CollectParam();
            if (criteriaParam != null)
            {
                SearchEvent(criteriaParam, null);
            }
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        NewEvent(sender, e);
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        if (BtnImportClick != null)
        {
            BtnImportClick(sender, e);
        }
    }
}

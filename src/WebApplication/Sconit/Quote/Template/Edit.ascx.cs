using com.Sconit.Entity.Quote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using NHibernate.Expression;

public partial class Quote_Template_Edit : EditModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler SearchEvent;
    public EventHandler NewEvent;
    protected string Id
    {
        get
        {
            return (string)ViewState["Id"];
        }
        set
        {
            ViewState["Id"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void btnSave_Click(object sender, EventArgs e)
    {
        if(txtCostCategory.Text.Trim()!=string.Empty)
        {
            CostCategory costCategory = new CostCategory();
            costCategory.Id = Int32.Parse(Id);
            costCategory.Name = txtCostCategory.Text.Trim();
            TheToolingMgr.UpdateCostCategory(costCategory);
            ShowSuccessMessage("Common.Business.Result.Save.Successfully");
        }
    }

    public void InitPageParameter(string id)
    {
        Id = id;
        IList<CostCategory> CcList = TheToolingMgr.GetCostCategoryById(id);
        if(CcList.Count>0)
        {
            txtCostCategory.Text = CcList[0].Name;
        }
    }

    public void btnBack_Click(object sender, EventArgs e)
    {
        BackEvent(sender,e);
    }

    public void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }

    protected void DoSearch()
    {
        if (SearchEvent != null)
        {
            object[] criteriaParam = CollectParam();
            SearchEvent(criteriaParam, null);
        }
    }

    private object[] CollectParam()
    {

        #region DetachedCriteria

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(CostList));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(CostList)).SetProjection(Projections.Count("Id"));
        if (txtCostCategory.Text.Trim() != string.Empty)
        {
            CostCategory cc = new CostCategory();
            cc.Id = Int32.Parse(Id);
            selectCriteria.Add(Expression.Eq("CCId", cc));
            selectCountCriteria.Add(Expression.Eq("CCId", cc));
        }

        return (new object[] { selectCriteria, selectCountCriteria });

        #endregion

    }

    public void btnNew_Click(object sender, EventArgs e)
    {
        NewEvent(Id, e);
    }
}
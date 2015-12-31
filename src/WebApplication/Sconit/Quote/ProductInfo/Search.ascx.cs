using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using NHibernate.Expression;
using com.Sconit.Entity.Quote;
using com.Sconit.Entity;

public partial class Quote_ProductInfo_Search : SearchModuleBase
{
    public event EventHandler NewEvent;
    public event EventHandler SearchEvent;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.tbPartyFrom.ServiceParameter = "string:" + BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS + ",string:" + this.CurrentUser.Code;
        this.tbPartyFrom.DataBind();
        //if(!IsPostBack)
        //{
        //    LoadCustomer();
        //}
    }

    //void LoadCustomer()
    //{
    //    Customer customer = new Customer();
    //    IList<Customer> CList = TheCustomerMgr.GetAllCustomer();
    //    CList.Add(customer);
    //    this.ddlCustomerName.DataSource = CList;
    //    ddlCustomerName.DataTextField = "Name";
    //    ddlCustomerName.DataValueField = "Code";
    //    ddlCustomerName.DataBind();
    //    ddlCustomerName.SelectedValue = "";
    //}
    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    { }

    protected override void DoSearch()
    {
        if (SearchEvent != null)
        {
            object[] criteriaParam = CollectParam();
            SearchEvent(criteriaParam, null);
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        NewEvent(sender, e);
    }

    private object[] CollectParam()
    {

        #region DetachedCriteria

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(ProductInfo));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(ProductInfo)).SetProjection(Projections.Count("Id"));
        if (tbPartyFrom.Text != string.Empty)
        {
            selectCriteria.Add(Expression.Like("CustomerCode", tbPartyFrom.Text, MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("CustomerCode", tbPartyFrom.Text, MatchMode.Anywhere));
        }

        if (txtProductName.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("ProductName", txtProductName.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("ProductName", txtProductName.Text.Trim(), MatchMode.Anywhere));
        }

        if (txtProductNo.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("ProductNo", txtProductNo.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("ProductNo", txtProductNo.Text.Trim(), MatchMode.Anywhere));
        }
        if(txtProjectNo.Text.Trim()!=string.Empty)
        {
            selectCriteria.Add(Expression.Like("ProjectId", txtProjectNo.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("ProjectId", txtProjectNo.Text.Trim(), MatchMode.Anywhere));
        }
        return (new object[] { selectCriteria, selectCountCriteria });

        #endregion

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }
}
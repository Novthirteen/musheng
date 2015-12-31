using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Utility;
using NHibernate.Expression;
using com.Sconit.Entity.Quote;
using com.Sconit.Entity.MasterData;

public partial class Quote_Tooling_Search : SearchModuleBase
{
    public event EventHandler NewEvent;
    public event EventHandler SearchEvent;

    private IDictionary<string, string> parameter = new Dictionary<string, string>();
    protected void Page_Load(object sender, EventArgs e)
    {
        //LoadCustomer();
    }

    #region ///////
    //void LoadCustomer()
    //{
    //    IList<Customer> customerList = TheCustomerMgr.GetAllCustomer();
    //    Customer st = new Customer();
    //    customerList.Add(st);
    //    dplCustomer.DataSource = customerList;
    //    dplCustomer.DataTextField = "Name";
    //    dplCustomer.DataValueField = "Code";
    //    dplCustomer.DataBind();
    //    dplCustomer.SelectedValue = "";
    //}
    #endregion

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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        NewEvent(sender, e);
    }

    #region ////
    //private void FillParameter()
    //{
    //    this.parameter.Clear();
    //    this.parameter.Add("ProjectNo", txtProjectNo.Text.Trim());
    //    this.parameter.Add("TL_No", txtTLNo.Text.Trim());
    //    this.parameter.Add("TL_Name", txtTLName.Text.Trim());
    //    this.parameter.Add("TL_Spec", txtTLSpec.Text.Trim());
    //    this.parameter.Add("Customer", txtCustomerName.Text.Trim());
    //    this.parameter.Add("MSNo", txtMSNo.Text.Trim());
    //    this.parameter.Add("ApplyStartDate", txtApplyStartDate.Text.Trim());
    //    this.parameter.Add("ApplyEndDate", txtApplyEndtDate.Text.Trim());
    //    this.parameter.Add("ApplyUser", txtApplyUser.Text.Trim());
    //    this.parameter.Add("Suppliers", txtSupplier.Text.Trim());
    //    this.parameter.Add("ArriveStartDate",  txtArriveStartDate.Text.Trim());
    //    this.parameter.Add("ArriveEndDate", txtArriveEndDate.Text.Trim());
    //    this.parameter.Add("TL_SalesType", dplTLSalesType.SelectedValue);
    //    if (dplCustomerBStatus.SelectedValue == "未结算")
    //    {
    //        this.parameter.Add("CustomerBStatus", "0");
    //    }
    //    else
    //    {
    //        this.parameter.Add("CustomerBStatus", "1");
    //    }
    //}
    #endregion

    private object[] CollectParam()
    {

        #region DetachedCriteria

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(Tooling));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(Tooling)).SetProjection(Projections.Count("TL_No"));
        if (txtProjectNo.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("ProjectNo", txtProjectNo.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("ProjectNo", txtProjectNo.Text.Trim(), MatchMode.Anywhere));
        }

        if (txtTLNo.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("TL_No", txtTLNo.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("TL_No", txtTLNo.Text.Trim(), MatchMode.Anywhere));
        }

        if (txtTLName.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("TL_Name", txtTLName.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("TL_Name", txtTLName.Text.Trim(), MatchMode.Anywhere));
        }

        if (txtTLName.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("TL_Spec", txtTLSpec.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("TL_Spec", txtTLSpec.Text.Trim(), MatchMode.Anywhere));
        }

        if (dplCustomer.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("Customer", dplCustomer.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("Customer", dplCustomer.Text.Trim(), MatchMode.Anywhere));
        }

        if (txtMSNo.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("MSNo", txtMSNo.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("MSNo", txtMSNo.Text.Trim(), MatchMode.Anywhere));
        }

        if (txtApplyStartDate.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Ge("ApplyDate", txtApplyStartDate.Text.Trim()));
            selectCountCriteria.Add(Expression.Ge("ApplyDate", txtApplyStartDate.Text.Trim()));
        }

        if (txtApplyEndDate.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Le("ApplyDate", txtApplyEndDate.Text.Trim()));
            selectCountCriteria.Add(Expression.Le("ApplyDate", txtApplyEndDate.Text.Trim()));
        }

        if (txtApplyUser.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("ApplyUser", txtApplyUser.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("ApplyUser", txtApplyUser.Text.Trim(), MatchMode.Anywhere));
        }

        if (txtSupplier.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("Suppliers", txtSupplier.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("Suppliers", txtSupplier.Text.Trim(), MatchMode.Anywhere));
        }

        if (txtArriveStartDate.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Ge("ArriveDate", txtArriveStartDate.Text.Trim()));
            selectCountCriteria.Add(Expression.Ge("ArriveDate", txtArriveStartDate.Text.Trim()));
        }

        if (txtArriveEndDate.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Le("ArriveDate", txtArriveEndDate.Text.Trim()));
            selectCountCriteria.Add(Expression.Le("ArriveDate", txtArriveEndDate.Text.Trim()));
        }

        if (dplTLSalesType.SelectedValue.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("TL_SalesType", dplTLSalesType.SelectedValue, MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("TL_SalesType", dplTLSalesType.SelectedValue, MatchMode.Anywhere));
        }

        if(dplCustomerBStatus.SelectedValue.Trim() != string.Empty)
        {
             if (dplCustomerBStatus.SelectedValue == "未结算")
             {
                 selectCriteria.Add(Expression.Eq("CustomerBStatus", false));
                 selectCountCriteria.Add(Expression.Eq("CustomerBStatus", false));
             }
             else
             {
                 selectCriteria.Add(Expression.Eq("CustomerBStatus", true));
                 selectCountCriteria.Add(Expression.Eq("CustomerBStatus",true));
             }
        }
        return (new object[] { selectCriteria, selectCountCriteria });

        #endregion

    }
}
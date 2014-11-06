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
using com.Sconit.Entity;
using com.Sconit.Entity.Distribution;
using com.Sconit.Entity.Procurement;
using com.Sconit.Utility;
using System.Data.SqlClient;

public partial class MasterData_PriceList_PriceList_Search : SearchModuleBase
{
    public event EventHandler SearchEvent;
    public event EventHandler NewEvent;
    public event EventHandler SearchEvent1;

    public string PriceListType
    {
        get
        {
            return (string)ViewState["PriceListType"];
        }
        set
        {
            ViewState["PriceListType"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.PriceListType == BusinessConstants.CODE_MASTER_PRICE_LIST_TYPE_VALUE_PURCHASE)
        {
            this.ltlParty.Text = "${MasterData.Supplier.Code}:";
            tbParty.ServiceMethod = "GetSupplier";
            tbParty.ServicePath = "SupplierMgr.service";
            tbParty.ServiceParameter = "string:" + this.CurrentUser.Code;
        }
        else if (this.PriceListType == BusinessConstants.CODE_MASTER_PRICE_LIST_TYPE_VALUE_SALES
            || this.PriceListType == BusinessConstants.CODE_MASTER_PRICE_LIST_TYPE_VALUE_CUSTOMERGOODS)
        {
            tbParty.ServiceMethod = "GetCustomer";
            tbParty.ServicePath = "CustomerMgr.service";
            tbParty.ServiceParameter = "string:" + this.CurrentUser.Code;

        }
    }

    protected void btnSearchItemPriceList_Click(object sender, EventArgs e)
    {

        try
        {
            string sql = @"select p.code as 供应商,p.name as 供应商描述,m.code as 价格单代码,d.currency as 货币,d.uom as 单位
                            ,d.unitprice as 价格,d.isIncludeTax as 是否含税,d.IsProvEst as 是否暂估,d.StartDate as 开始时间,d.EndDate as 结束时间,m.IsActive as 是否有效
                            from pricelistdet d
                            left join pricelistmstr m on d.pricelist = m.code
                            left join party p on p.code = m.party
                            where item =@p0
                            and m.type ='Purchase'";
            if (this.PriceListType == BusinessConstants.CODE_MASTER_PRICE_LIST_TYPE_VALUE_SALES)
            {
                sql = @"select p.code as 客户,p.name as 客户描述,m.code as 价格单代码,d.currency as 货币,d.uom as 单位
                    ,d.unitprice as 价格,d.isIncludeTax as 是否含税,d.IsProvEst as 是否暂估,d.StartDate as 开始时间,d.EndDate as 结束时间,m.IsActive as 是否有效
                    from pricelistdet d
                    left join pricelistmstr m on d.pricelist = m.code
                    left join party p on p.code = m.party
                    where item =@p0
                    and m.type ='Sales'";
            }
            else if (this.PriceListType == BusinessConstants.CODE_MASTER_PRICE_LIST_TYPE_VALUE_CUSTOMERGOODS)
            {
                sql = @"select p.code as 客户,p.name as 客户描述,m.code as 价格单代码,d.currency as 货币,d.uom as 单位
                    ,d.unitprice as 价格,d.isIncludeTax as 是否含税,d.IsProvEst as 是否暂估,d.StartDate as 开始时间,d.EndDate as 结束时间,m.IsActive as 是否有效
                    from pricelistdet d
                    left join pricelistmstr m on d.pricelist = m.code
                    left join party p on p.code = m.party
                    where item =@p0
                    and m.type ='CustomerGoods'";
            }

            SqlParameter[] sqlParam = new SqlParameter[2];

            sqlParam[0] = new SqlParameter("@p0", this.tbItem.Text.Trim());
            sqlParam[1] = new SqlParameter("@p1", DateTime.Now);
            if (this.cbInclude.Checked)
            {
                //sql += "and m.IsActive = 1";
            }
            else
            {
                sql += "and m.IsActive = 1 and d.StartDate < @p1 and (d.EndDate is null or  d.EndDate > @p1)";
            }

            DataSet dataSet = TheSqlHelperMgr.GetDatasetBySql(sql, sqlParam);

            this.GV_List.DataSource = dataSet;
            this.GV_List.DataBind();
            SearchEvent1(sender, e);
            this.GV_List.Visible = true;
        }
        catch (Exception ex)
        {
            ShowErrorMessage(ex.Message);
        }

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.GV_List.DataSource = null;
        this.GV_List.DataBind();
        DoSearch();
    }


    protected void btnNew_Click(object sender, EventArgs e)
    {
        NewEvent(sender, e);
        this.GV_List.Visible = false;
    }

    protected override void DoSearch()
    {
        string code = this.tbCode.Text.Trim() != string.Empty ? this.tbCode.Text.Trim() : string.Empty;
        string party = this.tbParty.Text.Trim() != string.Empty ? this.tbParty.Text.Trim() : string.Empty;

        if (SearchEvent != null)
        {
            if (this.PriceListType == BusinessConstants.CODE_MASTER_PRICE_LIST_TYPE_VALUE_PURCHASE)
            {
                #region DetachedCriteria
                DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(PurchasePriceList));
                DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(PurchasePriceList))
                    .SetProjection(Projections.Count("Code"));

                //selectCriteria.Add(Expression.Eq("TextField4", BusinessConstants.CODE_MASTER_PRICE_LIST_TYPE_VALUE_PURCHASE));
                //selectCountCriteria.Add(Expression.Eq("TextField4", BusinessConstants.CODE_MASTER_PRICE_LIST_TYPE_VALUE_PURCHASE));

                if (code != string.Empty)
                {
                    selectCriteria.Add(Expression.Like("Code", code, MatchMode.Anywhere));
                    selectCountCriteria.Add(Expression.Like("Code", code, MatchMode.Anywhere));
                }
                if (party != string.Empty)
                {
                    selectCriteria.Add(Expression.Like("Party.Code", party, MatchMode.Anywhere));
                    selectCountCriteria.Add(Expression.Like("Party.Code", party, MatchMode.Anywhere));
                }
                DetachedCriteria[] supplierCrieteria = SecurityHelper.GetSupplierPermissionCriteria(this.CurrentUser.Code);
                selectCriteria.Add(
                    Expression.Or(
                      Expression.Or(
                          Subqueries.PropertyIn("Party.Code", supplierCrieteria[0]),
                          Subqueries.PropertyIn("Party.Code", supplierCrieteria[1])
                                    ),
                          Expression.IsNull("Party.Code")
                                  )
                    );

                selectCountCriteria.Add(
                    Expression.Or(
                      Expression.Or(
                          Subqueries.PropertyIn("Party.Code", supplierCrieteria[0]),
                          Subqueries.PropertyIn("Party.Code", supplierCrieteria[1])
                                    ),
                          Expression.IsNull("Party.Code")
                                  )
                    );
                SearchEvent((new object[] { selectCriteria, selectCountCriteria }), null);
                #endregion
            }
            else if (this.PriceListType == BusinessConstants.CODE_MASTER_PRICE_LIST_TYPE_VALUE_SALES)
            {
                #region DetachedCriteria
                DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(SalesPriceList));
                DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(SalesPriceList))
                    .SetProjection(Projections.Count("Code"));

                //selectCriteria.Add(Expression.Eq("TextField4", BusinessConstants.CODE_MASTER_PRICE_LIST_TYPE_VALUE_SALES));
                //selectCountCriteria.Add(Expression.Eq("TextField4", BusinessConstants.CODE_MASTER_PRICE_LIST_TYPE_VALUE_SALES));

                if (code != string.Empty)
                {
                    selectCriteria.Add(Expression.Like("Code", code, MatchMode.Anywhere));
                    selectCountCriteria.Add(Expression.Like("Code", code, MatchMode.Anywhere));
                }
                if (party != string.Empty)
                {
                    selectCriteria.Add(Expression.Like("Party.Code", party, MatchMode.Anywhere));
                    selectCountCriteria.Add(Expression.Like("Party.Code", party, MatchMode.Anywhere));
                }
                DetachedCriteria[] customerCrieteria = SecurityHelper.GetCustomerPermissionCriteria(this.CurrentUser.Code);
                selectCriteria.Add(
                   Expression.Or(
                     Expression.Or(
                         Subqueries.PropertyIn("Party.Code", customerCrieteria[0]),
                         Subqueries.PropertyIn("Party.Code", customerCrieteria[1])
                                   ),
                         Expression.IsNull("Party.Code")
                                 )
                   );

                selectCountCriteria.Add(
                    Expression.Or(
                      Expression.Or(
                          Subqueries.PropertyIn("Party.Code", customerCrieteria[0]),
                          Subqueries.PropertyIn("Party.Code", customerCrieteria[1])
                                    ),
                          Expression.IsNull("Party.Code")
                                  )
                    );
                SearchEvent((new object[] { selectCriteria, selectCountCriteria }), null);
                #endregion
            }
            else if (this.PriceListType == BusinessConstants.CODE_MASTER_PRICE_LIST_TYPE_VALUE_CUSTOMERGOODS)
            {
                #region DetachedCriteria
                DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(CustomerGoodsPriceList));
                DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(CustomerGoodsPriceList))
                    .SetProjection(Projections.Count("Code"));
                //selectCriteria.Add(Expression.Eq("TextField4",BusinessConstants.CODE_MASTER_PRICE_LIST_TYPE_VALUE_CUSTOMERGOODS));
                //selectCountCriteria.Add(Expression.Eq("TextField4", BusinessConstants.CODE_MASTER_PRICE_LIST_TYPE_VALUE_CUSTOMERGOODS));
                if (code != string.Empty)
                {
                    selectCriteria.Add(Expression.Like("Code", code, MatchMode.Anywhere));
                    selectCountCriteria.Add(Expression.Like("Code", code, MatchMode.Anywhere));
                }
                if (party != string.Empty)
                {
                    selectCriteria.Add(Expression.Like("Party.Code", party, MatchMode.Anywhere));
                    selectCountCriteria.Add(Expression.Like("Party.Code", party, MatchMode.Anywhere));
                }
                DetachedCriteria[] customerCrieteria = SecurityHelper.GetCustomerPermissionCriteria(this.CurrentUser.Code);
                selectCriteria.Add(
                   Expression.Or(
                     Expression.Or(
                         Subqueries.PropertyIn("Party.Code", customerCrieteria[0]),
                         Subqueries.PropertyIn("Party.Code", customerCrieteria[1])
                                   ),
                         Expression.IsNull("Party.Code")
                                 )
                   );

                selectCountCriteria.Add(
                    Expression.Or(
                      Expression.Or(
                          Subqueries.PropertyIn("Party.Code", customerCrieteria[0]),
                          Subqueries.PropertyIn("Party.Code", customerCrieteria[1])
                                    ),
                          Expression.IsNull("Party.Code")
                                  )
                    );
                SearchEvent((new object[] { selectCriteria, selectCountCriteria }), null);
                #endregion
            }
        }
        this.GV_List.Visible = false;
    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {
        if (actionParameter.ContainsKey("Code"))
        {
            this.tbCode.Text = actionParameter["Code"];
        }
        if (actionParameter.ContainsKey("PriceListType"))
        {
            this.tbParty.Text = actionParameter["PriceListType"];
        }
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }

}

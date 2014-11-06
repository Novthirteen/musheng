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
using com.Sconit.Entity;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;
using NHibernate.Expression;
using com.Sconit.Entity.View;
using com.Sconit.Entity.Cost;
using System.Data.SqlClient;

public partial class Cost_Report_InvIOB_List : ReportModuleBase
{
    public string fc
    {
        get { return (string)ViewState["fc"]; }
        set { ViewState["fc"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            Label lblItem = (Label)(e.Row.Cells[1].FindControl("lblItemCode"));
            Balance balance = TheBalanceMgr.GetBalance(fc, lblItem.Text.Trim());
            lblItem.ToolTip = balance.Cost.ToString("0.######");

            for (int i = 5; i < e.Row.Cells.Count; i++)
            {
                if (i == 6)
                {
                    Label lblLocationName = (Label)(e.Row.Cells[1].FindControl("lblLocationName"));
                    string location = lblLocationName.ToolTip.Trim();
                    e.Row.Cells[i].Text = this.GetPurchaseAmount(lblItem.Text.Trim(), location).ToString("0.####");
                }
                else
                {
                    e.Row.Cells[i].Text = (double.Parse(e.Row.Cells[i].Text) * balance.Cost).ToString("0.####");
                }
            }
        }
    }

    public override void InitPageParameter(object sender)
    {
        this._criteriaParam = (CriteriaParam)sender;
        this.fc = this._criteriaParam.FinanceYear + "-" + this._criteriaParam.FinanceMonth;
        this.SetCriteria();
        this.UpdateView();
    }

    public override void UpdateView()
    {
        for (int i = 0; i < this.GV_List.Columns.Count; i++)
        {
            this.GV_List.Columns[i].Visible = true;
        }
        this.GV_List.Execute();
        if (!IsExport)
        {
            int[] alwaysShow = new int[] { 5, 11, 17, 20 };
            com.Sconit.Utility.GridViewHelper.HiddenColumns(this.GV_List, alwaysShow);
        }
    }

    public void Export()
    {
        this.ExportXLS(GV_List);
    }

    protected override void SetCriteria()
    {
        DetachedCriteria criteria = DetachedCriteria.For(typeof(LocationDetail));
        criteria.CreateAlias("Location", "l");
        criteria.CreateAlias("Item", "i");

        #region Customize
        SecurityHelper.SetRegionSearchCriteria(criteria, "l.Region.Code", this.CurrentUser.Code); //区域权限
        #endregion

        #region Select Parameters
        CriteriaHelper.SetLocationCriteria(criteria, "Location.Code", this._criteriaParam);
        //CriteriaHelper.SetItemCriteria(criteria, "Item.Code", this._criteriaParam);
        //CriteriaHelper.SetItemDescCriteria(criteria, "i.Desc1", this._criteriaParam);

        if (this._criteriaParam.Item != null)
        {

            criteria.Add(Expression.Like("i.Code", this._criteriaParam.Item, MatchMode.Start));

        }
        // if (this._criteriaParam.ItemDesc != null)
        //{
        //    criteria.Add(Expression.Like("i.Desc1", this._criteriaParam.Item, MatchMode.Anywhere) ||
        //        Expression.Like("i.Desc2", this._criteriaParam.Item, MatchMode.Anywhere));
        //}

        #endregion

        DetachedCriteria selectCountCriteria = CloneHelper.DeepClone<DetachedCriteria>(criteria);
        selectCountCriteria.SetProjection(Projections.Count("Id"));
        SetSearchCriteria(criteria, selectCountCriteria);
    }

    public override void PostProcess(IList list)
    {
        TheLocationDetailMgr.PostProcessInvIOB(list, _criteriaParam.StartDate, _criteriaParam.EndDate);
    }

    private void HiddenColumns(int[] alwaysVisibleColumns)
    {
        Dictionary<int, bool> dics = new Dictionary<int, bool>();

        foreach (GridViewRow row in this.GV_List.Rows)
        {
            for (int j = 0; j < row.Cells.Count; j++)
            {
                if (!dics.ContainsKey(j))
                {
                    dics.Add(j, false);
                }
                TableCell tc = row.Cells[j];
                if (!(tc.Text.Trim() == "0" || (!tc.HasControls() && tc.Text.Trim() == string.Empty))
                    || alwaysVisibleColumns.Contains(j))
                {
                    dics[j] = true;
                }
            }
        }

        foreach (var dic in dics)
        {
            this.GV_List.Columns[dic.Key].Visible = dic.Value;
        }
        this.GV_List.DataBind();
    }

    private decimal GetPurchaseAmount(string itemCode,string location)
    {
        DetachedCriteria criteria = DetachedCriteria.For(typeof(ActingBill));
        criteria.Add(Expression.Eq("Item.Code", itemCode));
        criteria.Add(Expression.Eq("LocationFrom", location));
        criteria.Add(Expression.Ge("EffectiveDate", this._criteriaParam.StartDate));
        criteria.Add(Expression.Le("EffectiveDate", this._criteriaParam.EndDate));

        criteria.SetProjection(Projections.ProjectionList()
            .Add(Projections.GroupProperty("Item.Code").As("Item"))
            .Add(Projections.Sum("BillAmount").As("BillAmount"))
            .Add(Projections.Sum("BillQty").As("BillQty")));

        IList<object[]> objs = TheCriteriaMgr.FindAll<object[]>(criteria);
        if (objs != null && objs.Count() > 0)
        {
            return Convert.ToDecimal(objs[0][1]);
        }
        return 0M;
    }
}

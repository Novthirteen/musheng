using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.View;
using NHibernate.Expression;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using System.Collections;

public partial class Reports_InvIOBCust_Main : MainModuleBase
{
    private DateTime StartDate
    {
        get
        {
            if (this.tbStartDate.Text.Trim() != string.Empty)
            {
                return DateTime.Parse(this.tbStartDate.Text).Date;
            }
            return DateTime.Now.Date.AddMonths(-1);
        }
    }

    private DateTime EndDate
    {
        get
        {
            if (this.tbEndDate.Text.Trim() != string.Empty)
            {
                return DateTime.Parse(this.tbEndDate.Text.Trim()).Date.AddDays(1);
            }
            return DateTime.Now.Date.AddDays(1);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.tbFlow.ServiceParameter = "string:" + this.CurrentUser.Code;
        this.tbLocaion.ServiceParameter = "string:" + this.CurrentUser.Code;

        if (!IsPostBack)
        {
            this.tbStartDate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            this.tbEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            this.fldGv.Visible = false;
        }
    }

    private void DoSearch()
    {
        try
        {
            if (DateTime.Compare(this.StartDate, this.EndDate) > 0)
            {
                ShowErrorMessage("Common.StarDate.EndDate.Compare");
                return;
            }
        }
        catch (Exception)
        {
            ShowErrorMessage("Common.Business.Error.DateInvalid");
            return;
        }

        string itemCode = this.tbItem.Text.Trim();
      //  string itemDesc = this.tbDesc.Text.Trim();
        string locationCode = this.tbLocaion.Text.Trim();
        string flowCode = this.tbFlow.Text.Trim();

        IList<FlowView> saleFlow = new List<FlowView>();

        DetachedCriteria criteria = null;

        List<Permission> customers = this.CurrentUser.OrganizationPermission.Where(p => (p.Category.Code == BusinessConstants.CODE_MASTER_PERMISSION_CATEGORY_TYPE_VALUE_CUSTOMER)).ToList();

        if (customers != null && customers.Count() > 0)
        {
            criteria = DetachedCriteria.For<FlowView>();
            criteria.CreateAlias("Flow", "f");
            criteria.CreateAlias("FlowDetail", "fd");
            criteria.CreateAlias("f.PartyFrom", "pf");
            criteria.CreateAlias("fd.Item", "item");

            if (flowCode != string.Empty)
            {
                criteria.Add(Expression.Eq("f.Code", flowCode));
            }
            else
            {
                criteria.Add(Expression.In("pf.Code", customers.Select(p => p.Code).ToList()));
            }

            criteria.Add(Expression.Eq("f.Type", BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS));

            criteria.Add(Expression.Eq("f.IsActive", true));

            if (itemCode != null && itemCode.Trim() != string.Empty)
            {
                // criteria.Add(Expression.Eq("item.Code", itemCode));
                criteria.Add(
              Expression.Like("item.Code", itemCode, MatchMode.Anywhere) ||
              Expression.Like("item.Desc1", itemCode, MatchMode.Anywhere) ||
              Expression.Like("item.Desc2", itemCode, MatchMode.Anywhere)
              );
            }

            //if (itemDesc != string.Empty)
            //{
            //    criteria.Add(Expression.Or(Expression.Like("item.Desc1", itemDesc, MatchMode.Anywhere),
            //        Expression.Like("item.Desc2", itemDesc, MatchMode.Anywhere)));
            //}

            saleFlow = this.TheCriteriaMgr.FindAll<FlowView>(criteria);
        }

        IList<FlowDetail> flowDetails = saleFlow.Select(f => f.FlowDetail).ToList();

        List<string> items = flowDetails.Select(f => f.Item.Code).Take(500).ToList();

        #region 查找库存
        criteria = DetachedCriteria.For<LocationDetail>();
        criteria.CreateAlias("Item", "item");
        criteria.CreateAlias("Location", "location");

        //criteria.Add(Expression.Not(Expression.Eq("Qty", decimal.Zero)));
        criteria.Add(Expression.In("item.Code", items));
        if (locationCode != string.Empty)
        {
            criteria.Add(Expression.Eq("location.Code", locationCode));
        }
        else
        {
            IList<Location> locList = this.TheLocationMgr.GetLocationListForMushengRequireForCust(this.CurrentUser.Code);
            if (locList != null)
            {
                criteria.Add(Expression.In("Location", locList.ToArray()));
            }
            else
            {
                criteria.Add(Expression.Eq("location.Code", locationCode));
            }
        }

        IList invList = TheCriteriaMgr.FindAll(criteria);
        #endregion

        TheLocationDetailMgr.PostProcessInvIOB(invList, this.StartDate, this.EndDate);

        this.GV_List.DataSource = invList;
        this.fldGv.Visible = true;
        this.GV_List.DataBind();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        DoSearch();
        this.ExportXLS(GV_List);
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
        }
    }

    class PermissionComparer : IEqualityComparer<Permission>
    {
        public bool Equals(Permission x, Permission y)
        {
            return x.Code == y.Code;
        }

        public int GetHashCode(Permission obj)
        {
            string hCode = obj.Code;
            return hCode.GetHashCode();
        }
    }

}
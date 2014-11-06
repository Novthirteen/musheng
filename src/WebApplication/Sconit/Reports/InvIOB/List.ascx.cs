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

public partial class Reports_InvIOB_List : ReportModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
        }
    }

    public override void InitPageParameter(object sender)
    {
        this._criteriaParam = (CriteriaParam)sender;
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

}

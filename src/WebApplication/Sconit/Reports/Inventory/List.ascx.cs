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
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using NHibernate.Expression;
using com.Sconit.Entity;
using com.Sconit.Entity.View;
using com.Sconit.Utility;

public partial class MasterData_Reports_Inventory_List : ReportModuleBase
{


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //UpdateView();
        }
    }

    public override void UpdateView()
    {
        this.GV_List.Execute();

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

    public void Export()
    {
        this.ExportXLS(GV_List);
    }

    protected override void SetCriteria()
    {
        DetachedCriteria criteria = DetachedCriteria.For(typeof(LocationLotDetailView));
        criteria.CreateAlias("Location", "l");

        #region Customize
        SecurityHelper.SetRegionSearchCriteria(criteria, "l.Region.Code", this.CurrentUser.Code); //区域权限
        #endregion

        #region Select Parameters
        CriteriaHelper.SetLocationCriteria(criteria, "Location.Code", this._criteriaParam);
      //  CriteriaHelper.SetItemCriteria(criteria, "Item.Code", this._criteriaParam);

        if (this._criteriaParam.Item != null)
        {
            criteria.CreateAlias("Item", "i");
            criteria.Add(Expression.Like("i.Code", this._criteriaParam.Item, MatchMode.Start));
        }

        CriteriaHelper.SetLotNoCriteria(criteria, "LotNo", this._criteriaParam);

        #endregion

        DetachedCriteria selectCountCriteria = CloneHelper.DeepClone<DetachedCriteria>(criteria);
        selectCountCriteria.SetProjection(Projections.Count("Id"));
        SetSearchCriteria(criteria, selectCountCriteria);
    }
}

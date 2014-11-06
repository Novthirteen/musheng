using System;
using System.Collections;
using System.Text;
using System.Web.UI.WebControls;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.View;
using com.Sconit.Utility;
using com.Sconit.Web;
using NHibernate.Expression;

public partial class Reports_InvDetail_List : ReportModuleBase
{
    public string ModuleType
    {
        get { return (string)ViewState["ModuleType"]; }
        set { ViewState["ModuleType"] = value; }
    }
    private bool isShowPlan
    {
        get { return (bool)ViewState["isShowPlan"]; }
        set { ViewState["isShowPlan"] = value; }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public override void InitPageParameter(object sender)
    {
        this._criteriaParam = (CriteriaParam)((object[])sender)[0];
        this.isShowPlan = (bool)((object[])sender)[1];
        this.SetCriteria();
        this.UpdateView();
    }

    public override void UpdateView()
    {
        this.IsExport = false;
        this.GV_List.Execute();
    }

    public void Export()
    {
        this.IsExport = true;
        this.GV_List.ExportXLS();
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LocationDetail ld = (LocationDetail)e.Row.DataItem;

            this.SetLinkButton(e.Row, "lbQtyToBeIn", new string[] { ld.Item.Code, ld.Location.Code, BusinessConstants.IO_TYPE_IN }, ld.QtyToBeIn != 0);
            this.SetLinkButton(e.Row, "lbQtyToBeOut", new string[] { ld.Item.Code, ld.Location.Code, BusinessConstants.IO_TYPE_OUT }, ld.QtyToBeOut != 0);
            this.SetLinkButton(e.Row, "lbInTransitQtyIn", new string[] { ld.Item.Code, ld.Location.Code, BusinessConstants.IO_TYPE_IN }, ld.InTransitQty != 0);
            this.SetLinkButton(e.Row, "lbInTransitQtyOut", new string[] { ld.Item.Code, ld.Location.Code, BusinessConstants.IO_TYPE_OUT }, ld.InTransitQtyOut != 0);
            this.SetLinkButton(e.Row, "lbPickedQty", new string[] { ld.Item.Code, ld.Location.Code }, ld.PickedQty != 0);
            e.Row.Cells[1].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
        }
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
       // CriteriaHelper.SetItemCriteria(criteria, "Item.Code", this._criteriaParam);
      //  CriteriaHelper.SetItemDescCriteria(criteria, "i.Desc1", this._criteriaParam);

        if (this._criteriaParam.Item != null)
        {
           // criteria.CreateAlias("Item", "i");
            criteria.Add(Expression.Like("i.Code", this._criteriaParam.Item, MatchMode.Anywhere) ||
                Expression.Like("i.Desc1", this._criteriaParam.Item, MatchMode.Anywhere) ||
                Expression.Like("i.Desc2", this._criteriaParam.Item, MatchMode.Anywhere));
        }

        #endregion
        if (!isShowPlan)
        {
            criteria.Add(Expression.Not(Expression.Eq("Qty", 0M)));
        }

        if (this._criteriaParam.LocationType != null)
        {
            criteria.Add(Expression.Eq("l.Type", this._criteriaParam.LocationType));
        }

        DetachedCriteria selectCountCriteria = CloneHelper.DeepClone<DetachedCriteria>(criteria);
        selectCountCriteria.SetProjection(Projections.Count("Id"));

        SetSearchCriteria(criteria, selectCountCriteria);
    }

    public override void PostProcess(IList list)
    {
        if (isShowPlan)
        {
            TheLocationDetailMgr.PostProcessInvDetail(list);
            this.GV_List.Columns[8].Visible = true;
            this.GV_List.Columns[9].Visible = true;
            this.GV_List.Columns[10].Visible = true;
            this.GV_List.Columns[11].Visible = true;
            this.GV_List.Columns[12].Visible = true;
            this.GV_List.Columns[13].Visible = true;
        }
        else
        {
            this.GV_List.Columns[8].Visible = false;
            this.GV_List.Columns[9].Visible = false;
            this.GV_List.Columns[10].Visible = false;
            this.GV_List.Columns[11].Visible = false;
            this.GV_List.Columns[12].Visible = false;
            this.GV_List.Columns[13].Visible = false;
        }
    }

    protected void lbQtyToBeInOrOut_Click(object sender, EventArgs e)
    {
        LinkButton linkButton = (LinkButton)sender;
        string[] str = linkButton.CommandArgument.Split(',');
        if (str.Length == 3)
        {
            if (linkButton.ID == "lbQtyToBeIn")
            {
                this.ucOrderInList.Visible = true;
                this.ucOrderInList.InitPageParameter(str[0], str[1], str[2], null);
            }
            else if (linkButton.ID == "lbQtyToBeOut")
            {
                this.ucOrderOutList.Visible = true;
                this.ucOrderOutList.InitPageParameter(str[0], str[1], str[2], null);
            }
        }
    }

    protected void lbInTransitQtyInOrOut_Click(object sender, EventArgs e)
    {
        LinkButton linkButton = (LinkButton)sender;
        string[] str = linkButton.CommandArgument.Split(',');
        if (str.Length == 3)
        {
            if (linkButton.ID == "lbInTransitQtyIn")
            {
                this.ucInTransitListIn.Visible = true;
                this.ucInTransitListIn.InitPageParameter(str[0], str[1], str[2]);
            }
            else if (linkButton.ID == "lbInTransitQtyOut")
            {
                this.ucInTransitListOut.Visible = true;
                this.ucInTransitListOut.InitPageParameter(str[0], str[1], str[2]);
            }
        }
    }

    protected void lbPickedQty_Click(object sender, EventArgs e)
    {
        string[] str = ((LinkButton)sender).CommandArgument.Split(',');
        if (str.Length == 2)
        {
            this.ucPickList.Visible = true;
            this.ucPickList.InitPageParameter(str[0], str[1]);
        }
    }

    private void SetLinkButton(GridViewRow gvr, string id, string[] commandArgument, bool enabled)
    {
        LinkButton linkButton = (LinkButton)gvr.FindControl(id);
        linkButton.Enabled = enabled && !IsExport;
        if (enabled)
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < commandArgument.Length; i++)
            {
                if (i > 0)
                    str.Append(",");

                str.Append(commandArgument[i]);
            }
            linkButton.CommandArgument = str.ToString();
        }
    }


}

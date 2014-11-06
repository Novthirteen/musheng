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
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Web;
using com.Sconit.Entity;
using NHibernate.Expression;
using System.Collections.Generic;
using com.Sconit.Entity.Exception;
using NHibernate.SqlCommand;
using com.Sconit.Persistence;


public partial class MasterData_MiscOrder_Search : SearchModuleBase
{
    public event EventHandler EditEvent;
    public event EventHandler BackEvent;
    public event EventHandler NewEvent;
    public event EventHandler SearchEvent;
    public string ModuleType
    {
        get
        {
            return (string)ViewState["ModuleType"];
        }
        set
        {
            ViewState["ModuleType"] = value;
        }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {

        this.tbMiscOrderRegion.ServiceParameter = "string:" + this.CurrentUser.Code;
     
        
    }
    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {
        //todo

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (this.tbItem.Text.Trim() != string.Empty)
        {
            this.rblListFormat.SelectedValue = "Detail";
        }
        DoSearch();
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
       
        NewEvent(sender, e);
    }
    protected override void DoSearch()
    {
        string code = this.tbMiscOrderCode.Text != string.Empty ? this.tbMiscOrderCode.Text.Trim() : string.Empty;
        string type = this.ModuleType;

        string tbRegion = this.tbMiscOrderRegion.Text != string.Empty ? this.tbMiscOrderRegion.Text.Trim() : string.Empty;
        string tbLocation = this.tbMiscOrderLocation.Text != string.Empty ? this.tbMiscOrderLocation.Text.Trim() : string.Empty;
        string tbEffectDate = this.tbMiscOrderEffectDate.Text != string.Empty ? this.tbMiscOrderEffectDate.Text.Trim() : string.Empty;
        string tbStartDate = this.startDate.Text != string.Empty ? this.startDate.Text.Trim() : string.Empty;
        string tbEndDate = this.endDate.Text != string.Empty ? this.endDate.Text.Trim() : string.Empty;
        string subjectCode = this.tbSubjectCode.Text != string.Empty ? this.tbSubjectCode.Text.Trim() : string.Empty;
        string costCenterCode = this.tbCostCenterCode.Text != string.Empty ? this.tbCostCenterCode.Text.Trim() : string.Empty;

        string itemCode = this.tbItem.Text != string.Empty ? this.tbItem.Text.Trim() : string.Empty;

        if (SearchEvent != null)
        {
            #region DetachedCriteria Group
            if (this.rblListFormat.SelectedValue == "Group")
            {
                DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(MiscOrder));
                DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(MiscOrder))
                    .SetProjection(Projections.Count("OrderNo"));
                selectCriteria.CreateAlias("Location", "l");
                selectCountCriteria.CreateAlias("Location", "l");

                selectCriteria.CreateAlias("SubjectList", "s", JoinType.LeftOuterJoin);
                selectCountCriteria.CreateAlias("SubjectList", "s", JoinType.LeftOuterJoin);

                if (code != string.Empty)
                {
                    selectCriteria.Add(Expression.Like("OrderNo", code, MatchMode.Anywhere));
                    selectCountCriteria.Add(Expression.Like("OrderNo", code, MatchMode.Anywhere));
                }
                if (type != string.Empty)
                {
                    selectCriteria.Add(Expression.Like("Type", type, MatchMode.Anywhere));
                    selectCountCriteria.Add(Expression.Like("Type", type, MatchMode.Anywhere));

                }
                if (tbRegion != string.Empty)
                {
                    selectCriteria.Add(Expression.Like("l.Region.Code", tbRegion, MatchMode.Anywhere));
                    selectCountCriteria.Add(Expression.Like("l.Region.Code", tbRegion, MatchMode.Anywhere));

                }
                if (tbLocation != string.Empty)
                {
                    selectCriteria.Add(Expression.Like("Location.Code", tbLocation, MatchMode.Anywhere));
                    selectCountCriteria.Add(Expression.Like("Location.Code", tbLocation, MatchMode.Anywhere));

                }

                if (tbEffectDate != string.Empty)
                {
                    DateTime tmpEffectDate = DateTime.Parse(tbEffectDate);
                    selectCriteria.Add(Expression.Eq("EffectiveDate", tmpEffectDate));
                    selectCountCriteria.Add(Expression.Eq("EffectiveDate", tmpEffectDate));
                }

                if (tbStartDate != string.Empty)
                {
                    DateTime tmpStartDate = DateTime.Parse(tbStartDate);
                    selectCriteria.Add(Expression.Gt("CreateDate", tmpStartDate));
                    selectCountCriteria.Add(Expression.Gt("CreateDate", tmpStartDate));

                }
                if (tbEndDate != string.Empty)
                {
                    DateTime tmpEndDate = DateTime.Parse(tbEndDate);
                    selectCriteria.Add(Expression.Lt("CreateDate", tmpEndDate));
                    selectCountCriteria.Add(Expression.Lt("CreateDate", tmpEndDate));
                }

                if (costCenterCode != string.Empty)
                {
                    selectCriteria.Add(Expression.Eq("s.CostCenterCode", costCenterCode));
                    selectCountCriteria.Add(Expression.Eq("s.CostCenterCode", costCenterCode));
                }
                if (subjectCode != string.Empty)
                {
                    selectCriteria.Add(Expression.Eq("s.SubjectCode", subjectCode));
                    selectCountCriteria.Add(Expression.Eq("s.SubjectCode", subjectCode));
                }
                SearchEvent((new object[] { selectCriteria, selectCountCriteria, this.rblListFormat.SelectedValue == "Group" }), null);
            }
          
            #endregion

            #region DetachedCriteria Detail
            if (this.rblListFormat.SelectedValue == "Detail")
            {
                DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(MiscOrderDetail));
                DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(MiscOrderDetail))
                    .SetProjection(Projections.Count("Id"));

                selectCriteria.CreateAlias("MiscOrder", "m");
                selectCountCriteria.CreateAlias("MiscOrder", "m");

                selectCriteria.CreateAlias("m.Location", "l");
                selectCountCriteria.CreateAlias("m.Location", "l");

                selectCriteria.CreateAlias("m.SubjectList", "s", JoinType.LeftOuterJoin);
                selectCountCriteria.CreateAlias("m.SubjectList", "s", JoinType.LeftOuterJoin);

                if (code != string.Empty)
                {
                    selectCriteria.Add(Expression.Like("m.OrderNo", code, MatchMode.Anywhere));
                    selectCountCriteria.Add(Expression.Like("m.OrderNo", code, MatchMode.Anywhere));
                }
                if (type != string.Empty)
                {
                    selectCriteria.Add(Expression.Like("m.Type", type, MatchMode.Anywhere));
                    selectCountCriteria.Add(Expression.Like("m.Type", type, MatchMode.Anywhere));

                }
                if (tbRegion != string.Empty)
                {
                    selectCriteria.Add(Expression.Like("l.Region.Code", tbRegion, MatchMode.Anywhere));
                    selectCountCriteria.Add(Expression.Like("l.Region.Code", tbRegion, MatchMode.Anywhere));

                }
                if (tbLocation != string.Empty)
                {
                    selectCriteria.Add(Expression.Like("l.Code", tbLocation, MatchMode.Anywhere));
                    selectCountCriteria.Add(Expression.Like("l.Code", tbLocation, MatchMode.Anywhere));

                }

                if (tbEffectDate != string.Empty)
                {
                    DateTime tmpEffectDate = DateTime.Parse(tbEffectDate);
                    selectCriteria.Add(Expression.Eq("m.EffectiveDate", tmpEffectDate));
                    selectCountCriteria.Add(Expression.Eq("m.EffectiveDate", tmpEffectDate));
                }

                if (tbStartDate != string.Empty)
                {
                    DateTime tmpStartDate = DateTime.Parse(tbStartDate);
                    selectCriteria.Add(Expression.Gt("m.CreateDate", tmpStartDate));
                    selectCountCriteria.Add(Expression.Gt("m.CreateDate", tmpStartDate));

                }
                if (tbEndDate != string.Empty)
                {
                    DateTime tmpEndDate = DateTime.Parse(tbEndDate);
                    selectCriteria.Add(Expression.Lt("m.CreateDate", tmpEndDate));
                    selectCountCriteria.Add(Expression.Lt("m.CreateDate", tmpEndDate));
                }

                if (costCenterCode != string.Empty)
                {
                    selectCriteria.Add(Expression.Eq("s.CostCenterCode", costCenterCode));
                    selectCountCriteria.Add(Expression.Eq("s.CostCenterCode", costCenterCode));
                }
                if (subjectCode != string.Empty)
                {
                    selectCriteria.Add(Expression.Eq("s.SubjectCode", subjectCode));
                    selectCountCriteria.Add(Expression.Eq("s.SubjectCode", subjectCode));
                }
                if (itemCode != string.Empty)
                {
                    selectCriteria.CreateAlias("Item", "i");
                    selectCountCriteria.CreateAlias("Item", "i");

                    selectCriteria.Add(
                   Expression.Like("i.Code", itemCode, MatchMode.Anywhere) ||
                   Expression.Like("i.Desc1", itemCode, MatchMode.Anywhere) ||
                   Expression.Like("i.Desc2", itemCode, MatchMode.Anywhere)
                   );
                    selectCountCriteria.Add(
                        Expression.Like("i.Code", itemCode, MatchMode.Anywhere) ||
                        Expression.Like("i.Desc1", itemCode, MatchMode.Anywhere) ||
                        Expression.Like("i.Desc2", itemCode, MatchMode.Anywhere)
                        );
                }
                SearchEvent((new object[] { selectCriteria, selectCountCriteria,this.rblListFormat.SelectedValue == "Group" }),null);
            }

            #endregion
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using NHibernate.Expression;
using com.Sconit.Entity.MasterData;
using com.Sconit.Utility;
using com.Sconit.Entity;

public partial class ManageSconit_LeanEngine_Single_Search : SearchModuleBase
{
    public event EventHandler btnSearchClick;

    protected void Page_Load(object sender, EventArgs e)
    {
        //this.tbFlowCode.ServiceParameter = "string:" + this.CurrentUser.Code;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }

    protected override void DoSearch()
    {
        if (btnSearchClick != null)
        {
            string flowCode = this.tbFlowCode.Text.Trim();
            string flowDesc = this.tbFlowDesc.Text.Trim();

            DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(Flow));
            DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(Flow))
                .SetProjection(Projections.ProjectionList()
               .Add(Projections.Count("Code")));

            selectCriteria.Add(Expression.Eq("IsAutoCreate", true));
            selectCountCriteria.Add(Expression.Eq("IsAutoCreate", true));

            #region partyFrom
            SecurityHelper.SetPartySearchCriteria(selectCriteria, "PartyFrom.Code", this.CurrentUser.Code);
            SecurityHelper.SetPartySearchCriteria(selectCountCriteria, "PartyFrom.Code", this.CurrentUser.Code);
            #endregion

            #region partyTo
            SecurityHelper.SetPartySearchCriteria(selectCriteria, "PartyTo.Code", this.CurrentUser.Code);
            SecurityHelper.SetPartySearchCriteria(selectCountCriteria, "PartyTo.Code", this.CurrentUser.Code); ;
            #endregion

            if (flowCode != string.Empty)
            {
                selectCriteria.Add(Expression.Like("Code", flowCode, MatchMode.Anywhere));
                selectCountCriteria.Add(Expression.Like("Code", flowCode, MatchMode.Anywhere));
            }

            if (flowDesc != string.Empty)
            {
                selectCriteria.Add(Expression.Like("Description", flowDesc, MatchMode.Anywhere));
                selectCountCriteria.Add(Expression.Like("Description", flowDesc, MatchMode.Anywhere));
            }

            btnSearchClick((new object[] { selectCriteria, selectCountCriteria }), null);
        }
    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {
        //if (actionParameter.ContainsKey("Flow"))
        //{
        //    this.tbFlow.Text = actionParameter["Flow"];
        //}
    }
}

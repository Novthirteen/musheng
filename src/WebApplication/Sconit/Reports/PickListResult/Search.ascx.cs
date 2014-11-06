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
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity;
using com.Sconit.Utility;
using NHibernate.Expression;
using com.Sconit.Entity.View;


public partial class Reports_PickListResult_Search : ModuleBase
{

    public event EventHandler SearchEvent;
    public event EventHandler ExportEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
       
     
        if (!IsPostBack)
        {
            this.tbEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.tbStartDate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
          
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (ExportEvent != null)
        {
            DoSearch(true);
        }
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (SearchEvent != null)
        {
            DoSearch(false);
        }
       
    }
    private void DoSearch(bool IsExport)
    {

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(PickListResultView));
        selectCriteria.CreateAlias("PickList", "p");
        selectCriteria.CreateAlias("p.CreateUser", "u");
        selectCriteria.CreateAlias("Item", "i");
        selectCriteria.CreateAlias("p.PartyFrom", "pf");
        selectCriteria.CreateAlias("p.PartyTo", "pt");

        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(PickListResultView)).SetProjection(Projections.Count("Id"));
        selectCountCriteria.CreateAlias("PickList", "p");
        selectCountCriteria.CreateAlias("p.CreateUser", "u");
        selectCountCriteria.CreateAlias("Item", "i");
        selectCountCriteria.CreateAlias("p.PartyFrom", "pf");
       // selectCountCriteria.CreateAlias("p.PartyTo", "pt");


      
        if (this.tbStartDate.Text.Trim() != string.Empty)
        {
            DateTime startDate = DateTime.Parse(this.tbStartDate.Text.Trim());
            selectCriteria.Add(Expression.Ge("p.CreateDate", startDate));
            selectCountCriteria.Add(Expression.Ge("p.CreateDate", startDate));
        }

        if (this.tbEndDate.Text.Trim() != string.Empty)
        {
            DateTime endDate = DateTime.Parse(this.tbEndDate.Text.Trim());
            selectCriteria.Add(Expression.Lt("p.CreateDate", endDate.AddDays(1)));
            selectCountCriteria.Add(Expression.Lt("p.CreateDate", endDate.AddDays(1)));
        }

        if (this.tbItemCode.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("i.Code", tbItemCode.Text.Trim()));
            selectCountCriteria.Add(Expression.Eq("i.Code", tbItemCode.Text.Trim()));
        }
        if (this.tbPickListNo.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("p.PickListNo", tbPickListNo.Text.Trim()));
            selectCountCriteria.Add(Expression.Eq("p.PickListNo", tbPickListNo.Text.Trim()));
        }
        
       
        #region partyFrom
        SecurityHelper.SetPartyFromSearchCriteria(
            selectCriteria, selectCountCriteria, null, BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION, this.CurrentUser.Code);
        #endregion

        if (IsExport)
        {
            ExportEvent((new object[] { selectCriteria, selectCountCriteria }), null);
        }
        else
        {
            SearchEvent((new object[] { selectCriteria, selectCountCriteria }), null);
        }
    }

   
}

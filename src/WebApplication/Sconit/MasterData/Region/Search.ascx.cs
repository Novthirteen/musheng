﻿using System;
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
using com.Sconit.Utility;


public partial class MasterData_Region_Search : SearchModuleBase
{

    public event EventHandler SearchEvent;
    public event EventHandler NewEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {

        if (actionParameter.ContainsKey("Code"))
        {
            this.tbCode.Text = actionParameter["Code"];
        }
        if (actionParameter.ContainsKey("Name"))
        {
            this.tbName.Text = actionParameter["Name"];
        }
    }

    protected override void DoSearch()
    {
        string code = this.tbCode.Text != string.Empty ? this.tbCode.Text.Trim() : string.Empty;
        string name = this.tbName.Text != string.Empty ? this.tbName.Text.Trim() : string.Empty;

        if (SearchEvent != null)
        {
            #region DetachedCriteria

            DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(Region));
            DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(Region))
                .SetProjection(Projections.Count("Code"));

            if (code != string.Empty)
            {
                selectCriteria.Add(Expression.Like("Code", code, MatchMode.Anywhere));
                selectCountCriteria.Add(Expression.Like("Code", code, MatchMode.Anywhere));
            }
            else
            {
                DetachedCriteria[] regionCrieteria = SecurityHelper.GetRegionPermissionCriteria(this.CurrentUser.Code);
                selectCriteria.Add(
                      Expression.Or(
                          Subqueries.PropertyIn("Code", regionCrieteria[0]),
                          Subqueries.PropertyIn("Code", regionCrieteria[1])
                  ));

                selectCountCriteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("Code", regionCrieteria[0]),
                        Subqueries.PropertyIn("Code", regionCrieteria[1])
                ));
            }

            if (name != string.Empty)
            {
                selectCriteria.Add(Expression.Like("Name", name, MatchMode.Anywhere));
                selectCountCriteria.Add(Expression.Like("Name", name, MatchMode.Anywhere));
            }

            SearchEvent((new object[] { selectCriteria, selectCountCriteria }), null);
            #endregion
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        NewEvent(sender, e);
    }
}

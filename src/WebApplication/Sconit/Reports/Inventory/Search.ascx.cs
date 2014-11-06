﻿using System;
using System.Collections;
using System.Collections.Generic;
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
using com.Sconit.Entity.MasterData;
using NHibernate.Expression;
using com.Sconit.Entity.View;

public partial class MasterData_Reports_Inventory_Search : SearchModuleBase
{
    public event EventHandler SearchEvent;
    public event EventHandler ExportEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.tbLocation.ServiceParameter = "string:" + this.CurrentUser.Code;
    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {
        if (actionParameter.ContainsKey("LotNo"))
        {
            this.tbLotNo.Text = actionParameter["LotNo"];
        }
        if (actionParameter.ContainsKey("Location"))
        {
            this.tbLocation.Text = actionParameter["Location"];
        }
        if (actionParameter.ContainsKey("Item"))
        {
            this.tbItem.Text = actionParameter["Item"];
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }

    protected override void DoSearch()
    {
        if (SearchEvent != null)
        {
            object criteriaParam = CollectParam();
            SearchEvent(criteriaParam, null);
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (ExportEvent != null)
        {
            object param = this.CollectParam();
            if (param != null)
                ExportEvent(param, null);
        }
    }

    private CriteriaParam CollectParam()
    {
        CriteriaParam criteriaParam = new CriteriaParam();
        criteriaParam.Item = this.tbItem.Text.Trim() != string.Empty ? this.tbItem.Text.Trim() : null;
        criteriaParam.Location = this.tbLocation.Text.Trim() != string.Empty ? new string[] { this.tbLocation.Text.Trim() } : null;
        criteriaParam.LotNo = this.tbLotNo.Text.Trim() != string.Empty ? this.tbLotNo.Text.Trim() : null;
        return criteriaParam;
    }

}

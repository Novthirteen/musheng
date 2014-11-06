﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity;
using com.Sconit.Entity.View;
using NHibernate.Expression;

public partial class Reports_ActBill_List : ListModuleBase
{
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

    public override void UpdateView()
    {
        this.GV_List.Execute();
    }

    public void Export()
    {
        this.IsExport = true;
        this.ExportXLS(GV_List);
    }

    public void UpdateView(DetachedCriteria criteria)
    {
        IList<ActingBillView> actingBillVieList = TheCriteriaMgr.FindAll<ActingBillView>(criteria);

        if (actingBillVieList != null && actingBillVieList.Count > 0)
        {
            decimal totalAmount = actingBillVieList.Sum(abv => abv.Amount);
            this.tbTotalAmount.Text = totalAmount.ToString("0.########");
            this.tabTotalAmount.Visible = true;
        }
        else
        {
            this.tbTotalAmount.Text = string.Empty;
            this.tabTotalAmount.Visible = false;
        }

        this.UpdateView();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
            {
                this.GV_List.Columns[1].HeaderText = "${Reports.ActBill.PartyFrom}";
                this.GV_List.Columns[4].Visible = false;
            }
            else if (this.ModuleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
            {
                this.GV_List.Columns[1].HeaderText = "${Reports.ActBill.PartyTo}";
                this.GV_List.Columns[3].Visible = false;
            }
        }
    }
}

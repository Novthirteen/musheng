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
using com.Sconit.Entity.MasterData;
using com.Sconit.Control;
using com.Sconit.Utility;
using com.Sconit.Entity.Cost;
using System.Collections.Generic;
using System.Data.SqlClient;
using NHibernate.Expression;

public partial class Cost_FinanceCalendar_Edit : EditModuleBase
{
    public event EventHandler BackEvent;

    public static bool isCBom_Click = false;
    public static bool isRm_Click = false;

    protected Int32 Id
    {
        get
        {
            return (Int32)ViewState["Id"];
        }
        set
        {
            ViewState["Id"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }


    public void InitPageParameter(Int32 id)
    {
        this.Id = id;
        this.ODS_FinanceCalendar.SelectParameters["id"].DefaultValue = this.Id.ToString();
        this.ODS_FinanceCalendar.DataBind();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }
    protected void FV_FinanceCalendar_DataBound(object sender, EventArgs e)
    {
        FinanceCalendar financeCalendar = (FinanceCalendar)((FormView)sender).DataItem;
        ((System.Web.UI.WebControls.Button)(this.FV_FinanceCalendar.FindControl("btnSave"))).Visible = !financeCalendar.IsClosed;
        ((System.Web.UI.WebControls.Button)(this.FV_FinanceCalendar.FindControl("btnCbom"))).Visible = !financeCalendar.IsClosed;
        ((System.Web.UI.WebControls.Button)(this.FV_FinanceCalendar.FindControl("btnClose"))).Visible = !financeCalendar.IsClosed;
        ((System.Web.UI.WebControls.Button)(this.FV_FinanceCalendar.FindControl("btnRm"))).Visible = !financeCalendar.IsClosed;
        //((System.Web.UI.WebControls.Button)(this.FV_FinanceCalendar.FindControl("btnFg"))).Visible = !financeCalendar.IsClosed;
        ((TextBox)(this.FV_FinanceCalendar.FindControl("tbStartDate"))).Text = financeCalendar.StartDate.ToString("yyyy-MM-dd");
        ((TextBox)(this.FV_FinanceCalendar.FindControl("tbEndDate"))).Text = financeCalendar.EndDate.ToString("yyyy-MM-dd");
    }
    protected void ODS_FinanceCalendar_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {

    }
    protected void ODS_FinanceCalendar_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ShowSuccessMessage("Cost.FinanceCalendar.Update.Successfully");
    }

    protected void ODS_FinanceCalendar_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {

            ShowSuccessMessage("Cost.FinanceCalendar.Delete.Successfully");
            if (BackEvent != null)
            {
                BackEvent(this, e);
            }
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Cost.FinanceCalendar.Delete.Failed" + ex.Message);
        }
    }

    protected void btnCBom_Click(object sender, EventArgs e)
    {
        if (isCBom_Click)
        {
            ShowErrorMessage("Cost.FinanceCalendar.Cbom.Occupy");
        }
        else
        {
            try
            {
                isCBom_Click = true;
                FinanceCalendar fc = TheFinanceCalendarMgr.LoadFinanceCalendar(this.Id);
                this.TheBalanceMgr.GenBomTree(fc, this.CurrentUser.Code);
                this.TheBalanceMgr.GenCbom(fc, this.CurrentUser.Code);
                ShowSuccessMessage("Cost.FinanceCalendar.Cbom.Successfully");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Cost.FinanceCalendar.Cbom.Failed" + ex.Message);
            }
            isCBom_Click = false;
        }
    }

    protected void btnRm_Click(object sender, EventArgs e)
    {
        if (isRm_Click)
        {
            ShowErrorMessage("Cost.FinanceCalendar.CostRm.Occupy");
        }
        else
        {
            try
            {
                isRm_Click = true;
                FinanceCalendar fc = TheFinanceCalendarMgr.LoadFinanceCalendar(this.Id);
                string financeCalendar = fc.FinanceYear.ToString() + "-" + fc.FinanceMonth.ToString();

                this.TheBalanceMgr.GenBalance(fc, this.CurrentUser.Code, true, false);
                ShowSuccessMessage("Cost.FinanceCalendar.CostRm.Successfully");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Cost.FinanceCalendar.CostRm.Failed" + ex.Message);
            }
            isRm_Click = false;
        }
    }

    protected void btnFg_Click(object sender, EventArgs e)
    {
        try
        {
            FinanceCalendar fc = TheFinanceCalendarMgr.LoadFinanceCalendar(this.Id);
            string financeCalendar = fc.FinanceYear.ToString() + "-" + fc.FinanceMonth.ToString();

            this.TheBalanceMgr.GenBalance(fc, this.CurrentUser.Code, false, false);
            ShowSuccessMessage("Cost.FinanceCalendar.CostFg.Successfully");
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Cost.FinanceCalendar.CostFg.Failed" + ex.Message);
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            FinanceCalendar fc = TheFinanceCalendarMgr.LoadFinanceCalendar(this.Id);
            fc.IsClosed = true;
            TheFinanceCalendarMgr.UpdateFinanceCalendar(fc);

            ShowSuccessMessage("Cost.FinanceCalendar.Close.Successfully");
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Cost.FinanceCalendar.Close.Failed" + ex.Message);
        }
    }
}

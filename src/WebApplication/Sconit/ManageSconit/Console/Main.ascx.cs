using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Exception;
using NHibernate.Expression;
using com.Sconit.Entity.View;
using com.Sconit.Entity;
using System.Data.SqlClient;
using System.Data;

public partial class ManageSconit_Console_Main : MainModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.GV_List_Sql.DataSource = TheSqlReportMgr.GetAllSqlReport();
            this.GV_List_Sql.DataBind();
            lbPredefined_Click(sender, e);
        }
        if (this.CurrentUser.PagePermission.Select(t => t.Code).Contains("Page_CustomReport"))
        {
            this.tab_Custom.Visible = true;
        }
    }

    protected void lbCustom_Click(object sender, EventArgs e)
    {
        this.tab_Custom.Attributes["class"] = "ajax__tab_active";
        this.tab_Predefined.Attributes["class"] = "ajax__tab_inactive";
        this.tblpredefined.Visible = false;
        this.tblcustom.Visible = true;
        this.fld_Gv_List.Visible = false;
    }

    protected void lbPredefined_Click(object sender, EventArgs e)
    {
        this.tab_Custom.Attributes["class"] = "ajax__tab_inactive";
        this.tab_Predefined.Attributes["class"] = "ajax__tab_active";
        this.tblpredefined.Visible = true;
        this.tblcustom.Visible = false;
        this.GV_List_Sql.DataSource = TheSqlReportMgr.GetAllSqlReport();
        this.GV_List_Sql.DataBind();
        this.fld_Gv_List.Visible = false;
    }

    protected void btnRun_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = this.tbMemo.Text.Trim();

            DataSet dataSet = TheSqlHelperMgr.GetDatasetBySql(sql);

            if (dataSet.Tables.Count == 0)
            {
                dataSet.Tables.Add(new DataTable());
            }

            this.GV_List.DataSource = dataSet;
            this.GV_List.DataBind();
            this.fld_Gv_List.Visible = true;

            if ((Button)sender == this.btnExport)
            {
                this.ExportXLS(this.GV_List);
            }

        }
        catch (Exception ex)
        {
            ShowErrorMessage(ex.Message);
        }
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                //e.Row.Cells[i].Text = this.TrimEnd(e.Row.Cells[i].Text);
                e.Row.Cells[i].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            }
        }
    }

    protected void lbtnRun_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = ((LinkButton)sender).CommandArgument;

            SqlParameter[] sqlParam = new SqlParameter[5];
            if (tbParam1.Text.Trim() != string.Empty)
            {
                sqlParam[0] = new SqlParameter("@p1", tbParam1.Text.Trim());
            }
            if (tbParam2.Text.Trim() != string.Empty)
            {
                sqlParam[1] = new SqlParameter("@p2", tbParam2.Text.Trim());
            }
            if (tbParam3.Text.Trim() != string.Empty)
            {
                sqlParam[2] = new SqlParameter("@p3", tbParam3.Text.Trim());
            }
            if (tbParam4.Text.Trim() != string.Empty)
            {
                sqlParam[3] = new SqlParameter("@p4", tbParam4.Text.Trim());
            }
            if (tbParam5.Text.Trim() != string.Empty)
            {
                sqlParam[4] = new SqlParameter("@p5", tbParam5.Text.Trim());
            }

            DataSet dataSet = TheSqlHelperMgr.GetDatasetBySql(sql, sqlParam);
            if (dataSet.Tables.Count == 0)
            {
                dataSet.Tables.Add(new DataTable());
            }
            this.GV_List.DataSource = dataSet;
            this.GV_List.DataBind();
            this.fld_Gv_List.Visible = true;

        }
        catch (Exception ex)
        {
            ShowErrorMessage(ex.Message);
        }
    }

    protected void lbtnExport_Click(object sender, EventArgs e)
    {
        this.lbtnRun_Click(sender, e);
        this.ExportXLS(this.GV_List);
    }

    protected void GV_List_Sql_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Cells[1].Text = e.Row.Cells[1].Text.Substring(0, 50) + "...";
        }
    }

    private string TrimEnd(string str)
    {
        str = str.TrimEnd('0');
        str = str.TrimEnd('.');
        return str;
    }

}

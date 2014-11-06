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


public partial class Cost_Report_Scrap_Main : MainModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            fld_Gv_List.Visible = false;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (this.tbfc.Text.Trim() == string.Empty)
            {
                ShowErrorMessage("请选择会计年月");
                return;
            }

            string sql = @" select  this.item as 物料,item.desc1 as 描述1,item.desc2 as 描述2
                            ,this.uom as 单位, qty as 数量, this.cost as 成本,Amount as 金额, this.IsProvEst as 暂估
                            from  C_Balance this
                            left join item on item.code = this.item
                            where FinanceCalendar =@p0" ;

            SqlParameter[] sqlParam = new SqlParameter[1];

            sqlParam[0] = new SqlParameter("@p0", this.tbfc.Text.Trim());

            if (this.cbEst.Checked)
            {
                sql += " and this.IsProvEst=1 ";
            }

            DataSet dataSet = TheSqlHelperMgr.GetDatasetBySql(sql, sqlParam);

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
            //e.Row.Cells[3].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            //e.Row.Cells[4].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
        }
    }
}

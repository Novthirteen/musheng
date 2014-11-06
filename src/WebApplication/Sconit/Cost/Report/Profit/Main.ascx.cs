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
public partial class Cost_Report_Profit_Main : MainModuleBase
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
            string sql = @"select party.code as 代码,party.name as 名称, item.code as 物料代码,item.desc1 as 描述1,
                            item.desc2 as 描述2,actbill.uom as 单位, cast(sum(BilledQty) as numeric(12,2)) as 结算数,
                            cast(sum(BilledAmount) as numeric(12,2)) as 结算金额,
                            ( select sum(Cost) as cost from costdet det 
                            where det.CostGroup = @p0 and det.FinanceYear = @p1 and det.FinanceMonth = @p2 and item.code = det.item
                            group by det.item) as 单位成本,0 as 销售成本,0 as 销售毛利
                            from actbill 
                            left join item on item.code = actbill.item
                            left join partyaddr on partyaddr.code = actbill.billaddr
                            left join party on partyaddr.partycode = party.code 
                            where transtype='SO' and effdate>@p3 and effdate <@p4 ";

            SqlParameter[] sqlParam = new SqlParameter[6];

            if (this.tbFinanceYear1.Text.Trim() == string.Empty)
            {
                ShowErrorMessage("Cost.FinanceCalendar.Year.Empty");
                return;
            }

            DateTime fy1 = DateTime.Parse(this.tbFinanceYear1.Text.Trim());
            FinanceCalendar financeCalendar1 = TheFinanceCalendarMgr.GetFinanceCalendar(fy1.Year, fy1.Month);
            if (financeCalendar1 == null)
            {
                ShowErrorMessage("Cost.FinanceCalendar.Year.Empty");
                return;
            }

            sqlParam[0] = new SqlParameter("@p0", "CG1");
            sqlParam[1] = new SqlParameter("@p1", fy1.Year);
            sqlParam[2] = new SqlParameter("@p2", fy1.Month);
            sqlParam[3] = new SqlParameter("@p3", financeCalendar1.StartDate);
            sqlParam[4] = new SqlParameter("@p4", financeCalendar1.EndDate);
            //sqlParam[5] = new SqlParameter("@p5", tbPartyCode.Text.Trim());
            if (this.tbPartyCode.Text.Trim() != string.Empty)
            {
                sql += " and party.code =@p5 ";
            }

            sql += @" group by party.code,party.name ,item.code,item.desc1,item.desc2,actbill.uom                            
                      HAVING  sum(BilledQty)<>0 order by party.code   ";

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
        e.Row.Cells[9].Visible = false;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[3].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[9].Text = e.Row.Cells[9].Text == "&nbsp;" ? "0" : e.Row.Cells[9].Text;
            e.Row.Cells[10].Text = (double.Parse(e.Row.Cells[9].Text) * double.Parse(e.Row.Cells[7].Text)).ToString("0.##");
            double cost = double.Parse(e.Row.Cells[8].Text) - double.Parse(e.Row.Cells[10].Text);
            e.Row.Cells[11].Text = cost.ToString("0.##");
            if (cost < 0)
            {
                e.Row.Cells[11].Attributes.Add("style", "color: red;");
            }
        }
    }

}

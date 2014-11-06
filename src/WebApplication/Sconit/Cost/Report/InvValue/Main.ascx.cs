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
public partial class Cost_Report_InvValue_Main : MainModuleBase
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
            string sql = @"select inv.Item as 物料代码,Item.Desc1 as 描述1,Item.Desc2 as 描述2,
                            cast(Inv.Qty as numeric(12,2)) as 数量, 
                            cast(sum(det.Cost) as numeric(12,2)) as 单位成本,
                            cast(Inv.Qty*sum(det.Cost) as numeric(12,2)) as 金额 
                            from CostInvBalance inv
                            left join CostDet det on inv.Item = det.Item left join Item on Item.code = inv.Item
                            where det.CostGroup = inv.CostGroup and det.FinanceYear = inv.FinanceYear and det.FinanceMonth = inv.FinanceMonth
                            and inv.Location =@p0 and inv.FinanceYear =@p1 and inv.FinanceMonth=@p2 ";
            if ((Button)sender == this.btnExport)
            {
                sql = @"select inv.Item as 物料代码,Item.Desc1 as 描述1,Item.Desc2 as 描述2,
                        Inv.Qty as 数量,
                        sum(det.Cost) as 单位成本,
                        Inv.Qty*sum(det.Cost) as 金额 
                        from CostInvBalance inv
                        left join CostDet det on inv.Item = det.Item left join Item on Item.code = inv.Item
                        where det.CostGroup = inv.CostGroup and det.FinanceYear = inv.FinanceYear and det.FinanceMonth = inv.FinanceMonth
                        and inv.Location =@p0 and inv.FinanceYear =@p1 and inv.FinanceMonth=@p2 ";
            }
            if (this.tbLocation.Text.Trim() == string.Empty)
            {

                ShowErrorMessage("MasterData.MiscOrder.Location.Empty");
                return;
            }
            if (this.tbFinanceYear.Text.Trim() == string.Empty)
            {
                ShowErrorMessage("Cost.FinanceCalendar.Year.Empty");
                return;
            }

            DateTime financeCalendar = DateTime.Parse(this.tbFinanceYear.Text);

            SqlParameter[] sqlParam = new SqlParameter[4];
            sqlParam[0] = new SqlParameter("@p0", tbLocation.Text.Trim());
            sqlParam[1] = new SqlParameter("@p1", financeCalendar.Year);
            sqlParam[2] = new SqlParameter("@p2", financeCalendar.Month);
            if (tbItemCode.Text.Trim() != string.Empty)
            {
                sql += " and inv.Item =@p3 group by inv.Item,Item.Desc1,Item.Desc2,Inv.Qty ";
                sqlParam[3] = new SqlParameter("@p3", tbItemCode.Text.Trim());
            }
            else
            {
                sql += " group by inv.Item,Item.Desc1,Item.Desc2,Inv.Qty ";
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
            //e.Row.Cells[i].Text = this.TrimEnd(e.Row.Cells[i].Text);
            e.Row.Cells[1].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
        }
    }


}

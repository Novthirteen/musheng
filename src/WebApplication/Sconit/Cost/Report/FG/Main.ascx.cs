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


public partial class Cost_Report_FG_Main : MainModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            fld_Gv_List.Visible = false;
        }
        this.cbDiff.Visible = this.CurrentUser.PagePermission.Select(p => p.Code).Contains(BusinessConstants.PERMISSION_PAGE_AUTOPRINT_VALUE_PAGE_COSTDIFF);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (this.tbFinanceYear1.Text.Trim() == string.Empty)
            {
                ShowErrorMessage("请选择会计年月");
                return;
            }

            string sql = string.Empty;
            string itemCode = this.tbItem.Text.Trim();


            SqlParameter[] sqlParam = new SqlParameter[2];
            if (itemCode == string.Empty)
            {
                string costSql = " this.Cost ";
                if (this.cbScrapCost.Checked)
                {
                    costSql = " this.ScrapCost ";
                }
                if (this.cbDiff.Checked)
                {
                    sql = @"select this.item as 物料,item.desc1 as 描述1,item.desc2 as 描述2,this.itemCategory as 产品类
                        ,this.uom as 单位," + costSql + @" as 成本, this.Allocation as 理论分摊,this.Diff as 实际分摊,this.OutQty as 产出,this.IsProvEst as 暂估 
                        from dbo.C_FgCost this
                        left join item on item.code = this.item
                        where financeCalendar =@p0
                        ";
                }
                else
                {
                    sql = @"select this.item as 物料,item.desc1 as 描述1,item.desc2 as 描述2,this.itemCategory as 产品类
                        ,this.uom as 单位, " + costSql + @" as 成本,this.IsProvEst as 暂估
                        from dbo.C_FgCost this
                        left join item on item.code = this.item
                        where financeCalendar =@p0
                        ";
                }
                if (this.cbEst.Checked)
                {
                    sql += " and this.IsProvEst=1 ";
                }
            }
            else
            {
                string costSql = " case when this.ItemCategory='CG' then 0 else this.Cost end ";
                if (this.cbScrapCost.Checked)
                {
                    costSql = " this.Cost ";
                }

                if (this.cbDiff.Checked)
                {
                    sql = @"select this.bom as 成品,this.FGCategory as [半/成品], this.item as 原材料,this.ItemCategory as 产品类,item.desc1 as 描述1,item.desc2 as 描述2
                            ,this.uom as 单位,this.accumqty as 用量," + costSql + @" as 成本, this.Allocation as 理论分摊 ,this.InQty as 投入,this.IsProvEst as 暂估
                            from C_Bom this
                            left join item on item.code = this.item
                            where financeCalendar =@p0
                            and this.bom =@p1 ";
                    if (!this.cbScrapCost.Checked)
                    {
                        sql += " and this.itemCategory ='RM' ";
                    }
                }
                else
                {
                    sql = @"select this.bom as 成品,this.FGCategory as [产品类1], this.item as 原材料,this.ItemCategory as 产品类,item.desc1 as 描述1,item.desc2 as 描述2
                            ,this.uom as 单位,this.accumqty as 用量," + costSql + @" as 成本,this.IsProvEst as 暂估
                            from C_Bom this
                            left join item on item.code = this.item
                            where financeCalendar =@p0
                            and this.bom =@p1 ";
                    if (!this.cbScrapCost.Checked)
                    {
                        sql += " and this.itemCategory ='RM' ";
                    }
                }
            }
            sqlParam[0] = new SqlParameter("@p0", this.tbFinanceYear1.Text.Trim());
            sqlParam[1] = new SqlParameter("@p1", this.tbItem.Text.Trim());

            DataSet dataSet = TheSqlHelperMgr.GetDatasetBySql(sql, sqlParam);
            if (itemCode != string.Empty)
            {
                DataTable dt = dataSet.Tables[0];
                DataRow dr = dt.NewRow();
                dr["成品"] = "合计";
                //dr["成本"] = dt.Compute("Sum(成本*用量) ", " ");
                dt.Rows.Add(dr);
                var c_boms = com.Sconit.Utility.IListHelper.DataTableToList<C_Bom>(dataSet.Tables[0]);
                c_boms.Last().成本 = c_boms.Sum(p => p.成本);
                this.GV_List.DataSource = c_boms;
            }
            else
            {
                this.GV_List.DataSource = dataSet;
            }
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

    protected void tbFinanceYear_TextChange(object sender, EventArgs e)
    {
        DateTime f = DateTime.Parse(this.tbFinanceYear1.Text);
        int year = f.Year;
        int month = f.Month;
        FinanceCalendar financeCalendar = TheFinanceCalendarMgr.GetFinanceCalendar(year, month);
    }

    class C_Bom
    {
        public string 成品 { get; set; }
        public string 产品类1 { get; set; }
        public string 原材料 { get; set; }
        public string 产品类 { get; set; }
        public string 描述1 { get; set; }
        public string 描述2 { get; set; }
        public string 单位 { get; set; }
        public double 用量 { get; set; }
        public double 成本 { get; set; }
        public bool 暂估 { get; set; }
    }
}


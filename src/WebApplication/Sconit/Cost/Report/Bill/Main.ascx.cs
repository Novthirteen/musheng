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


public partial class Cost_Report_Bill_Main : MainModuleBase
{
    public string ModuleType
    {
        get
        {
            return this.ModuleParameter["ModuleType"];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_PO)
        {
            this.tbPartyCode.ServicePath = "SupplierMgr.service";
            this.tbPartyCode.ServiceMethod = "GetAllSupplier";
            this.ltlPartyCode.Text = "供应商代号";
            this.cbGroupByParty.Text = "按供应商汇总";
        }

        if (!Page.IsPostBack)
        {
            fld_Gv_List.Visible = false;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = @"select party.code as 代码,party.name as 名称,";

            if (!this.cbGroupByParty.Checked)
            {
                sql += @" item.code as 物料代码,item.desc1 as 描述1,item.desc2 as 描述2,actbill.uom as 单位, ";
            }

            sql += @"cast(sum(BillQty) as numeric(12,2)) as 总数量,
                    cast(sum(BillAmount) as numeric(12,2)) as 总金额,
                    cast(sum(BilledQty) as numeric(12,2)) as 结算数,
                    cast(sum(BilledAmount) as numeric(12,2)) as 结算金额,
                    cast(sum(BillQty)-sum(BilledQty) as numeric(12,2)) as 未结数量,
                    cast(sum(BillAmount)-sum(BilledAmount) as numeric(12,2)) as 未结金额
                    from actbill 
                    left join item on item.code = actbill.item
                    left join partyaddr on partyaddr.code = actbill.billaddr
                    left join party on partyaddr.partycode = party.code                         
                    where transtype=@p0 
                    and effdate>=@p1 and effdate <@p2";

            SqlParameter[] sqlParam = new SqlParameter[5];

            try
            {
                if (DateTime.Compare(Convert.ToDateTime(this.tbStartTime.Text.Trim()), Convert.ToDateTime(this.tbEndTime.Text.Trim())) > 0)
                {
                    ShowErrorMessage("Common.StarDate.EndDate.Compare");
                    return;
                }
            }
            catch (Exception)
            {
                ShowErrorMessage("Common.Business.Error.DateInvalid");
                return;
            }

            sqlParam[1] = new SqlParameter("@p1", Convert.ToDateTime(this.tbStartTime.Text));
            sqlParam[2] = new SqlParameter("@p2", (Convert.ToDateTime(this.tbEndTime.Text)).AddDays(1));//算头算尾

            sqlParam[0] = new SqlParameter("@p0", this.ModuleType);

            if (this.tbPartyCode.Text.Trim() != string.Empty)
            {
                sql += " and party.code =@p3 ";
                sqlParam[3] = new SqlParameter("@p3", tbPartyCode.Text.Trim());
            }
            if (tbItemCode.Text.Trim() != string.Empty)
            {
                sql += " and item.code =@p4 ";
                sqlParam[4] = new SqlParameter("@p4", tbItemCode.Text.Trim());
            }


            sql += @" group by party.code,party.name ";

            if (!this.cbGroupByParty.Checked)
            {
                sql += @" ,item.code,item.desc1,item.desc2,actbill.uom ";
            }

            sql += " order by party.code ";


            DataSet dataSet = TheSqlHelperMgr.GetDatasetBySql(sql, sqlParam);

            DataTable dt = dataSet.Tables[0];
            DataRow dr = dt.NewRow();
            dr["代码"] = "合计";
            dr["总数量"] = dt.Compute("Sum(总数量) ", " ");
            dr["总金额"] = dt.Compute("Sum(总金额) ", " ");
            dr["结算数"] = dt.Compute("Sum(结算数) ", " ");
            dr["结算金额"] = dt.Compute("Sum(结算金额) ", " ");
            dr["未结数量"] = dt.Compute("Sum(未结数量) ", " ");
            dr["未结金额"] = dt.Compute("Sum(未结金额) ", " ");

            dt.Rows.Add(dr);

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
            e.Row.Cells[3].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[4].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
        }
    }

    protected void tbFinanceYear_TextChange(object sender, EventArgs e)
    {
        DateTime f = DateTime.Parse(this.tbFinanceYear1.Text);
        int year = f.Year;
        int month = f.Month;
        FinanceCalendar financeCalendar = TheFinanceCalendarMgr.GetFinanceCalendar(year, month);

        this.tbStartTime.Text = financeCalendar.StartDate.ToString("yyyy-MM-dd");
        this.tbEndTime.Text = financeCalendar.EndDate.ToString("yyyy-MM-dd");
    }
}

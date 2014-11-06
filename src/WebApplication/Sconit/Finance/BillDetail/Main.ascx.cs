using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using NHibernate.Expression;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using Geekees.Common.Controls;
using System.Data;
using System.Data.SqlClient;

public partial class Finance_BillDetail_Main : MainModuleBase
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.tbStartDate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            this.tbEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            if (this.ModuleParameter.ContainsKey("ModuleType"))
            {
                this.ModuleType = this.ModuleParameter["ModuleType"];
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string itemCode = this.tbItem.Text.Trim();

            if (itemCode == string.Empty)
            {
                ShowErrorMessage("请输入物料号");
                return;
            }

            SqlParameter[] sqlParam = new SqlParameter[3];

            sqlParam[0] = new SqlParameter("@p0", itemCode);

            string sql = @" select d.BillNo as 账单号,m.ExtBillNo as 发票号,p.Name as 供应商,
                            m.CreateDate as 开票日期, m.DateField1 as 发票时间,m.Status as 状态,
                            a.Item as 物料号, i.Desc1 + '['+ISNULL(i.Desc2,'')+']' as 描述,a.Uom as 单位,
                            d.UnitPrice as 单价,d.Currency as 货币,d.BilledQty as 开票数,d.Amount as 金额
                            from billdet d
                            join actbill a on a.Id = d.TransId
                            join BillMstr m on m.BillNo = d.BillNo
                            join PartyAddr pa on pa.Code = m.BillAddr
                            join Party p on p.Code = pa.PartyCode
                            join Item i on i.Code = a.Item
                            where a.Item = @p0 ";

            if (this.tbStartDate.Text.Trim() != string.Empty)
            {
                DateTime startDate = DateTime.Parse(this.tbStartDate.Text.Trim());
                sqlParam[1] = new SqlParameter("@p1", startDate);
                sql += " and m.CreateDate >=@p1 ";
            }
            if (this.tbEndDate.Text.Trim() != string.Empty)
            {
                DateTime endDate = DateTime.Parse(this.tbEndDate.Text.Trim());
                sqlParam[2] = new SqlParameter("@p2", endDate);
                sql += " and m.CreateDate <= @p2 ";
            }

            DataSet dataSet = TheSqlHelperMgr.GetDatasetBySql(sql, sqlParam);
            this.GV_List.DataSource = dataSet.Tables[0];
            this.GV_List.DataBind();
            this.fld_Gv_List.Visible = true;
        }
        catch (Exception ex)
        {
            ShowErrorMessage(ex.Message);
        }
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType ==DataControlRowType.Header)
        {
            if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_PO)
            {
                e.Row.Cells[3].Text = "供应商";
            }
            else
            {
                e.Row.Cells[3].Text = "客户";
            }
        }


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[10].Text = (double.Parse(e.Row.Cells[10].Text)).ToString("0.######");
            e.Row.Cells[12].Text = (double.Parse(e.Row.Cells[12].Text)).ToString("0.######");
            e.Row.Cells[13].Text = (double.Parse(e.Row.Cells[13].Text)).ToString("0.######");
        }
    }

}

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

            string sql = @"select  this.item as 物料,this.ItemCategory as 产品类,item.desc1 as 描述1,item.desc2 as 描述2
                            ,this.uom as 单位,this.location as 库位, sum(qty) as 数量, this.cost as 成本,sum(Amount) as 金额, this.IsProvEst as 暂估
                            from  C_Consume this
                            left join item on item.code = this.item
                            where FinanceCalendar =@p0
                            group by this.item,item.desc1,item.desc2,this.ItemCategory,this.uom,this.IsProvEst,this.cost,this.location";

            if (this.cbDetail.Checked)
            {
                sql = @"select  this.item as 物料,this.ItemCategory as 产品类,item.desc1 as 描述1,item.desc2 as 描述2,this.uom as 单位
                    , this.transtype as 类型,this.location as 库位, this.qty as 数量, this.cost as 成本,this.Amount as 金额, this.IsProvEst as 暂估
                    from  C_Consume this
                    left join item on item.code = this.item
                    where FinanceCalendar =@p0
                    Order by this.TransType";
            }

            SqlParameter[] sqlParam = new SqlParameter[1];

            sqlParam[0] = new SqlParameter("@p0", this.tbfc.Text.Trim());

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

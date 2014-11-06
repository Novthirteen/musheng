using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using com.Sconit.Web;

public partial class Production_ProdLineIp2_Detail : ModuleBase
{
    public event EventHandler BackEvent;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.Visible = false;
        //this.BackEvent(this, e);
    }

    public void InitPageParameter(string orderno, string fg, string rm)
    {
        try
        {
            SqlParameter[] sqlParam = new SqlParameter[3];

            sqlParam[0] = new SqlParameter("@p0", orderno);
            sqlParam[1] = new SqlParameter("@p1", fg);
            sqlParam[2] = new SqlParameter("@p2", rm);

            string sqlfg = @" select t.OrderNo as 工单号,t.Item as 物料号,
                            t.ItemDesc as 物料描述,t.Qty as 收货数, t.PartyFrom as 区域,
                            t.Loc as 库位,t.CreateDate as 创建时间
                             from LocTrans t 
                             where t.OrderNo =@p0
                             and t.Item =@p1 ";

            string sqlrm = @"select t.OrderNo as 工单号,t.Item as 物料号,
                            t.ItemDesc as 物料描述,-t.Qty as 投入数, t.PartyFrom as 区域,
                            t.Loc as 库位,t.CreateDate as 创建时间,l.UnitQty as 单位用量
                             from LocTrans t 
                            join orderloctrans as l on l.id = t.orderloctransId
                             where t.OrderNo =@p0
                             and t.Item =@p2";

            DataSet dataSetfg = TheSqlHelperMgr.GetDatasetBySql(sqlfg, sqlParam);
            this.GV_List_FG.DataSource = dataSetfg.Tables[0];
            this.GV_List_FG.DataBind();


            DataSet dataSetrm = TheSqlHelperMgr.GetDatasetBySql(sqlrm, sqlParam);
            this.GV_List_RM.DataSource = dataSetrm.Tables[0];
            this.GV_List_RM.DataBind();

            this.totalfg.Text = ((decimal)(dataSetfg.Tables[0].Compute("Sum(收货数)", "true"))).ToString("0.######");
            this.totalrm.Text = ((decimal)(dataSetrm.Tables[0].Compute("Sum(投入数)", "true"))).ToString("0.######");
            //this.fld_fg_List.Visible = true;
        }
        catch (Exception ex)
        {
            ShowErrorMessage(ex.Message);
        }
    }
    protected void GV_List_FG_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[4].Text = (double.Parse(e.Row.Cells[4].Text)).ToString("0.######");
        }
    }

    protected void GV_List_RM_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[4].Text = (double.Parse(e.Row.Cells[4].Text)).ToString("0.######");
            e.Row.Cells[8].Text = (double.Parse(e.Row.Cells[8].Text)).ToString("0.######");
        }
    }
}
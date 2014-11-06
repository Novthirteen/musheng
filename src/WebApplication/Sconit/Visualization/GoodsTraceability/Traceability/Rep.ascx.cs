using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using com.Sconit.Web;

public partial class Visualization_GoodsTraceability_Traceability_Rep : ModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.Visible = false;
    }

    public void InitPageParameter(string orderNo)
    {
        try
        {
            this.Visible = true;
            SqlParameter[] sqlParam = new SqlParameter[3];

            sqlParam[0] = new SqlParameter("@p0", orderNo);


            string sqlrm = @"select r.RepNo as 订单号,l.Item as 物料号, i.Desc1 + '['+ISNULL(i.Desc2,'')+']' as 描述,
                            r.IOType as [In/Out], l.HuId as 条码,l.LotNo as 批号 from RepackDet r
                            join LocationLotDet l on r.LocLotDetId = l.Id
                            join Item i on i.Code = l.Item
                            where r.RepNo =@p0 ";

            DataSet dataSetrm = TheSqlHelperMgr.GetDatasetBySql(sqlrm, sqlParam);
            this.GV_List.DataSource = dataSetrm.Tables[0];
            this.GV_List.DataBind();

            //this.fld_fg_List.Visible = true;
        }
        catch (Exception ex)
        {
            ShowErrorMessage(ex.Message);
        }
    }
}

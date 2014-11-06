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
using com.Sconit.Utility;
using com.Sconit.Entity.Cost;


public partial class Reports_InvIOBnew_Main : MainModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.tbStartDate.Text = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
            this.tbEndDate.Text = DateTime.Now.ToString("yyyyMMdd");
        }
        this.tbLocation.ServiceParameter = "string:" + this.CurrentUser.Code;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (this.tbStartDate.Text.Trim() == string.Empty)
            {
                ShowErrorMessage("请选择开始日期");
                return;
            }
            int endDate = int.Parse(DateTime.Now.AddDays(1).ToString("yyyyMMdd"));
            if (this.tbEndDate.Text.Trim() != string.Empty)
            {
                endDate = int.Parse(this.tbEndDate.Text.Trim());
            }

            int startDate = int.Parse(this.tbStartDate.Text.Trim());
            string itemCode = this.tbItem.Text.Trim();
            string locationCode = this.tbLocation.Text.Trim();

            SqlParameter[] sqlParamInv = new SqlParameter[2];
            SqlParameter[] sqlParamStart = new SqlParameter[3];
            SqlParameter[] sqlParamEnd = new SqlParameter[3];

            sqlParamStart[2] = new SqlParameter("@p2",startDate);
            sqlParamEnd[2] = new SqlParameter("@p2", endDate);

            string sqlInv = @"select locationdet.item as 物料,item.desc1 as 物料描述,item.uom as 单位,item.category as 产品类,locationdet.location as 库位,locationdet.qty as 数量
                            from locationdet left join item on item.code = locationdet.item  
                            where 1=1 ";

            string sqlStart = @"select 物料,类型,库位,sum(数量) as 数量 from Z_TransMonthly where DT > @p2 ";
            string sqlEnd = @"select 物料,类型,库位,sum(数量) as 数量 from Z_TransMonthly where DT > @p2 ";

            if (itemCode != string.Empty)
            {
                sqlInv += " and locationdet.item =@p0 ";
                sqlParamInv[0] = new SqlParameter("@p0", itemCode);

                sqlStart += " and 物料 =@p0 ";
                sqlParamStart[0] = new SqlParameter("@p0", itemCode);

                sqlEnd += " and 物料 =@p0 ";
                sqlParamEnd[0] = new SqlParameter("@p0", itemCode);
            }

            if (locationCode != string.Empty)
            {
                sqlInv += " and locationdet.location =@p1 ";
                sqlParamInv[1] = new SqlParameter("@p1", locationCode);

                sqlStart += " and (库位 =@p1) ";
                sqlParamStart[1] = new SqlParameter("@p1", locationCode);

                sqlEnd += " and (库位 =@p1 ) ";
                sqlParamEnd[1] = new SqlParameter("@p1", locationCode);

            }

            sqlInv += "order by locationdet.location,locationdet.item";
            sqlStart += " group by 物料,类型,库位 ";
            sqlEnd += " group by 物料,类型,库位 ";

            DataSet dataSetInv = TheSqlHelperMgr.GetDatasetBySql(sqlInv, sqlParamInv);
            DataSet dataSetStart = TheSqlHelperMgr.GetDatasetBySql(sqlStart, sqlParamStart);
            DataSet dataSetEnd = TheSqlHelperMgr.GetDatasetBySql(sqlEnd, sqlParamEnd);

            List<Inv> invs = IListHelper.DataTableToList<Inv>(dataSetInv.Tables[0]);
            List<Trans> starts = IListHelper.DataTableToList<Trans>(dataSetStart.Tables[0]);
            List<Trans> ends = IListHelper.DataTableToList<Trans>(dataSetEnd.Tables[0]);

            List<Iob> iobs = new List<Iob>();
            foreach (var inv in invs)
            {
                Iob iob = new Iob();
                //Item item = TheItemMgr.LoadItem(inv.物料);
                //Location location = TheLocationMgr.LoadLocation(inv.库位);
                iob.物料 = inv.物料;
                //iob.物料描述 = item.Description;
                iob.产品类 = inv.产品类;
                iob.单位 = inv.单位;
                iob.库位 = inv.库位;
                //iob.库位描述 = location.Name;

                var si = starts.Where(s => s.库位.Trim().ToLower() == inv.库位.Trim().ToLower() && s.物料.Trim().ToLower() == inv.物料.Trim().ToLower());
                var ei = ends.Where(s => s.库位.Trim().ToLower() == inv.库位.Trim().ToLower() && s.物料.Trim().ToLower() == inv.物料.Trim().ToLower());

                iob.期初 = (inv.数量 - si.Sum(s => s.数量));
                iob.计划外 = ((si.Where(s => (s.类型 == "RCT-UNP" || s.类型 == "ISS-UNP"))).Sum(s => s.数量) - (ei.Where(s => (s.类型 == "RCT-UNP" || s.类型 == "ISS-UNP"))).Sum(s => s.数量));
                iob.检验 = (si.Where(s => (s.类型 == "RCT-INP" || s.类型 == "ISS-INP")).Sum(s => s.数量) - ei.Where(s => (s.类型 == "RCT-INP" || s.类型 == "ISS-INP")).Sum(s => s.数量));
                iob.盘差 = (si.Where(s => s.类型 == "CYC-CNT").Sum(s => s.数量) - ei.Where(s => s.类型 == "CYC-CNT").Sum(s => s.数量));
                iob.期末 = (inv.数量 - ei.Sum(s => s.数量));
                iob.生产 = (si.Where(s => (s.类型 == "ISS-WO" || s.类型 == "ISS-WO-BF" || s.类型 == "RCT-WO")).Sum(s => s.数量) - ei.Where(s => (s.类型 == "ISS-WO" || s.类型 == "ISS-WO-BF" || s.类型 == "RCT-WO")).Sum(s => s.数量));
                iob.移库 = (si.Where(s => (s.类型 == "RCT-TR" || s.类型 == "ISS-TR")).Sum(s => s.数量) - ei.Where(s => (s.类型 == "RCT-TR" || s.类型 == "ISS-TR")).Sum(s => s.数量));
                iob.采购 = (si.Where(s => s.类型 == "RCT-PO").Sum(s => s.数量) - ei.Where(s => s.类型 == "RCT-PO").Sum(s => s.数量));
                iob.销售 = (si.Where(s => s.类型 == "ISS-SO").Sum(s => s.数量) - ei.Where(s => s.类型 == "ISS-SO").Sum(s => s.数量));
                iob.其它 = (iob.期末 - iob.计划外 - iob.期初 - iob.检验 - iob.盘差 - iob.生产 - iob.移库 - iob.采购 - iob.销售);
                iobs.Add(iob);
            }

            this.GV_List.DataSource = iobs;
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

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            for (int i = 5; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Text = (double.Parse(e.Row.Cells[i].Text)).ToString("0.####");
            }
        }
    }

    protected void tbFinanceYear_TextChange(object sender, EventArgs e)
    {
        if (this.tbFinanceYear.Text.Trim() == string.Empty)
        {
            return;
        }
        DateTime f = DateTime.Parse(this.tbFinanceYear.Text.Trim());
        int year = f.Year;
        int month = f.Month;
        FinanceCalendar financeCalendar = TheFinanceCalendarMgr.GetFinanceCalendar(year, month);

        this.tbStartDate.Text = financeCalendar.StartDate.ToString("yyyyMMdd");
        this.tbEndDate.Text = financeCalendar.EndDate.ToString("yyyyMMdd");
    }

    class Iob
    {
        public string 物料 { get; set; }
        //public string 物料描述 { get; set; }
        public string 库位 { get; set; }
        //public string 库位描述 { get; set; }
        public string 单位 { get; set; }
        public string 产品类 { get; set; }
        public decimal 期初 { get; set; }
        public decimal 采购 { get; set; }
        public decimal 销售 { get; set; }
        public decimal 移库 { get; set; }
        public decimal 检验 { get; set; }
        public decimal 生产 { get; set; }
        public decimal 计划外 { get; set; }
        public decimal 盘差 { get; set; }
        public decimal 其它 { get; set; }
        public decimal 期末 { get; set; }
    }

    class Inv
    {
        public string 物料 { get; set; }
        public string 库位 { get; set; }
        public decimal 数量 { get; set; }
        public string 单位 { get; set; }
        public string 产品类 { get; set; }
    }

    class Trans
    {
        public string 物料 { get; set; }
        public string 库位 { get; set; }
        public string 类型 { get; set; }
        public decimal 数量 { get; set; }
    }

    private decimal GetPurchaseAmount(string itemCode, string location)
    {
        DateTime endDate = DateTime.Now.Date.AddDays(1);
        if (this.tbEndDate.Text.Trim() != string.Empty)
        {
            endDate = DateTime.Parse(this.tbEndDate.Text.Trim());
        }

        DateTime startDate = DateTime.Parse(this.tbStartDate.Text.Trim());

        DetachedCriteria criteria = DetachedCriteria.For(typeof(ActingBill));
        criteria.Add(Expression.Eq("Item.Code", itemCode));
        criteria.Add(Expression.Eq("LocationFrom", location));
        criteria.Add(Expression.Ge("EffectiveDate", startDate));
        criteria.Add(Expression.Le("EffectiveDate", endDate));

        criteria.SetProjection(Projections.ProjectionList()
            .Add(Projections.GroupProperty("Item.Code").As("Item"))
            .Add(Projections.Sum("BillAmount").As("BillAmount"))
            .Add(Projections.Sum("BillQty").As("BillQty")));

        IList<object[]> objs = TheCriteriaMgr.FindAll<object[]>(criteria);
        if (objs != null && objs.Count() > 0)
        {
            return Convert.ToDecimal(objs[0][1]);
        }
        return 0M;
    }
}

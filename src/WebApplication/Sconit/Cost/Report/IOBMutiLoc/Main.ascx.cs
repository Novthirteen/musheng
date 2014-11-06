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
using System.Threading;

public partial class Cost_Report_IOBMutiLoc_Main : MainModuleBase
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
            this.tbStartDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            this.tbEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            if (this.ModuleParameter.ContainsKey("ModuleType"))
            {
                this.ModuleType = this.ModuleParameter["ModuleType"];
            }
            if (this.ModuleType == "Qty")
            {
                this.cbOnlyTotal.Visible = false;
            }
        }
        //this.tbLocation.ServiceParameter = "string:" + this.CurrentUser.Code;
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

            DateTime endDate = DateTime.Now.Date.AddDays(1);
            if (this.tbEndDate.Text.Trim() != string.Empty)
            {
                endDate = DateTime.Parse(this.tbEndDate.Text.Trim());
            }

            DateTime startDate = DateTime.Parse(this.tbStartDate.Text.Trim());
            string itemCode = this.tbItem.Text.Trim();

            string[] locations = this.tbLocation.Text.Trim().Split(',');

            SqlParameter[] sqlParamInv = new SqlParameter[3 + locations.Length];
            SqlParameter[] sqlParamStart = new SqlParameter[3 + locations.Length];
            SqlParameter[] sqlParamEnd = new SqlParameter[3 + locations.Length];

            sqlParamInv[0] = new SqlParameter("@p0", itemCode);
            sqlParamStart[0] = new SqlParameter("@p0", itemCode);
            sqlParamEnd[0] = new SqlParameter("@p0", itemCode);

            //sqlParamInv[1] = new SqlParameter("@p1", locationCode);
            //sqlParamStart[1] = new SqlParameter("@p1", locationCode);
            //sqlParamEnd[1] = new SqlParameter("@p1", locationCode);

            int fcYear = int.Parse(this.tbFinanceYear.Text.Trim().Split('-')[0]);
            int fcMonth = int.Parse(this.tbFinanceYear.Text.Trim().Split('-')[1]);
            var lastFinanceYear = TheFinanceCalendarMgr.GetFinanceCalendar(fcYear, fcMonth, -1);
            string lastFinanceYearCode = lastFinanceYear.FinanceYear.ToString() + "-" + lastFinanceYear.FinanceMonth.ToString();

            sqlParamInv[1] = new SqlParameter("@p1", this.tbFinanceYear.Text.Trim());
            sqlParamStart[1] = new SqlParameter("@p1", startDate);
            sqlParamEnd[1] = new SqlParameter("@p1", endDate);
            sqlParamInv[2] = new SqlParameter("@p2", lastFinanceYearCode);

            string sqlInv = @"select locationdet.item as 物料,item.desc1+'['+isnull(item.desc2,'')+']' as 物料描述,item.uom as 单位,item.category as 产品类,
                            '' as 库位,sum(locationdet.qty) as 数量,b1.Cost as 期初成本,b2.Cost as 期末成本,b1.Uom as CStartUom,b2.Uom as CEndUom
                            from locationdet left join item on item.code = locationdet.item 
                            left join C_Balance b1 on locationdet.item = b1.item and b1.FinanceCalendar=@p2 
                            left join C_Balance b2 on locationdet.item = b2.item and b2.FinanceCalendar=@p1 
                            where 1=1 ";

            string sqlStart = @"select loctrans.item as 物料,'' as 库位,loctrans.transtype as 类型,sum(loctrans.qty) as 数量 ,loctrans.IsSubcontract as IsSubcontract from loctrans
                                where loctrans.effdate>=@p1 ";
            string sqlEnd = @"select loctrans.item as 物料,'' as 库位,loctrans.transtype as 类型,sum(loctrans.qty) as 数量 ,loctrans.IsSubcontract as IsSubcontract from loctrans
                                where loctrans.effdate>@p1 ";

            if (itemCode != string.Empty)
            {
                sqlInv += " and locationdet.item =@p0 ";
                sqlStart += " and loctrans.item =@p0 ";
                sqlEnd += " and loctrans.item =@p0 ";
            }

            if (locations.Length > 0)
            {
                for (int k = 0; k < locations.Length; k++)
                {
                    int j = k + 3;
                    if (k == 0)
                    {
                        sqlInv += " and (locationdet.location = @p" + j;
                        sqlStart += " and (loctrans.loc = @p" + j;
                        sqlEnd += " and (loctrans.loc = @p" + j;
                    }
                    else
                    {
                        sqlInv += " or locationdet.location = @p" + j;
                        sqlStart += " or loctrans.loc = @p" + j;
                        sqlEnd += " or loctrans.loc = @p" + j;
                    }
                    sqlParamInv[j] = new SqlParameter("@p" + j, locations[k]);
                    sqlParamStart[j] = new SqlParameter("@p" + j, locations[k]);
                    sqlParamEnd[j] = new SqlParameter("@p" + j, locations[k]);
                }
                sqlInv += " ) ";
                sqlStart += " ) ";
                sqlEnd += " ) ";
            }
            else
            {
                ShowErrorMessage("请输入库位,多个库位,逗号分隔");
            }

            sqlInv += @" group by locationdet.item,item.desc1,item.desc2,item.uom,item.category,
                        b1.Cost,b2.Cost,b1.Uom,b2.Uom order by locationdet.item ";
            sqlStart += " group by loctrans.item,loctrans.transtype,loctrans.IsSubcontract ";
            sqlEnd += " group by loctrans.item,loctrans.transtype,loctrans.IsSubcontract ";

            DataSet dataSetInv = TheSqlHelperMgr.GetDatasetBySql(sqlInv, sqlParamInv);
            DataSet dataSetStart = TheSqlHelperMgr.GetDatasetBySql(sqlStart, sqlParamStart);
            DataSet dataSetEnd = TheSqlHelperMgr.GetDatasetBySql(sqlEnd, sqlParamEnd);

            List<Inv> invs = IListHelper.DataTableToList<Inv>(dataSetInv.Tables[0]);
            List<Trans> starts = IListHelper.DataTableToList<Trans>(dataSetStart.Tables[0]);
            List<Trans> ends = IListHelper.DataTableToList<Trans>(dataSetEnd.Tables[0]);
            invs = (from p in invs
                    group p by new
                    {
                        p.物料,
                        p.物料描述,
                        p.单位,
                        p.产品类
                    } into g
                    select new Inv
                    {
                        CEndUom = g.First().CEndUom,
                        CStartUom = g.First().CStartUom,
                        产品类 = g.Key.产品类,
                        单位 = g.Key.单位,
                        库位 = string.Empty,
                        期初成本 = g.First().期初成本,
                        期末成本 = g.First().期末成本,
                        数量= g.Sum(q=>q.数量),
                        物料 = g.Key.物料,
                        物料描述 = g.Key.物料描述,
                    }).ToList();

            var startDic = (from p in starts
                            group p by new
                            {
                                库位 = p.库位.Trim().ToLower(),
                                物料 = p.物料.Trim().ToLower()
                            } into g
                            select new
                            {
                                库位 = g.Key.库位,
                                物料 = g.Key.物料,
                                List = g.ToList()
                            }).ToDictionary(d => new
                            {
                                库位 = d.库位,
                                物料 = d.物料,
                            }, d => d.List);

            var endDic = (from p in ends
                          group p by new
                          {
                              库位 = p.库位.Trim().ToLower(),
                              物料 = p.物料.Trim().ToLower()
                          } into g
                          select new
                          {
                              库位 = g.Key.库位,
                              物料 = g.Key.物料,
                              List = g.ToList()
                          }).ToDictionary(d => new
                          {
                              库位 = d.库位,
                              物料 = d.物料,
                          }, d => d.List);

            List<Iob> iobs = new List<Iob>();
            List<Iob> iobTotal = new List<Iob>();
            Iob tiob = new Iob();

            string currentLocation = string.Empty;
            int rowCount = invs.Count();
            int i = 0;

            //lblLocation.Text = DateTime.Now.ToLongTimeString();
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Purchase));
            criteria.Add(Expression.Eq("FinanceCalendar", this.tbFinanceYear.Text.Trim()));
            IList<Purchase> purchases = TheCriteriaMgr.FindAll<Purchase>(criteria);

            #region foreach invs
            foreach (var inv in invs)
            {
                Iob iob = new Iob();
                iob.物料 = inv.物料;
                iob.物料描述 = inv.物料描述;
                iob.产品类 = inv.产品类;
                iob.单位 = inv.单位;
                iob.库位 = inv.库位;

                string 库位 = inv.库位.ToLower().Trim();
                string 物料 = inv.物料.ToLower().Trim();

                if (iob.产品类 != "CG")
                {
                    iob.期初成本 = TheUomConversionMgr.ConvertUomQty(物料, inv.单位, (decimal)(inv.期初成本), inv.CStartUom);
                    iob.期末成本 = TheUomConversionMgr.ConvertUomQty(物料, inv.单位, (decimal)(inv.期末成本), inv.CEndUom);
                    if (iob.产品类 == "RM")
                    {
                        iob.采购成本 = this.GetPurchasePrice(物料, endDate, inv.单位, purchases);
                    }
                }
                iob.期初数量 = inv.数量;
                iob.期末数量 = inv.数量;

                #region foreach starts and ends
                var _starts = new List<Trans>();
                startDic.TryGetValue(new
                {
                    库位 = 库位,
                    物料 = 物料
                }, out _starts);
                if (_starts != null)
                {
                    foreach (var s in _starts)
                    {
                        switch (s.类型)
                        {
                            case "RCT-PO":
                                iob.采购数量 += s.数量;
                                break;
                            case "RCT-TR":
                                iob.移库入数量 += s.数量;
                                break;
                            case "RCT-INP":
                                iob.检验入数量 += s.数量;
                                break;
                            case "RCT-WO":
                                if (!s.IsSubcontract)
                                {
                                    iob.生产入数量 += s.数量;
                                }
                                else
                                {
                                    iob.委外数量 += s.数量;
                                }
                                break;
                            case "ISS-WO":
                            case "ISS-MIN":
                                if (!s.IsSubcontract)
                                {
                                    iob.生产出数量 += s.数量;
                                }
                                else
                                {
                                    iob.委外数量 += s.数量;
                                }
                                break;
                            case "ISS-SO":
                                iob.销售数量 += s.数量;
                                break;
                            case "ISS-TR":
                                iob.移库出数量 += s.数量;
                                break;
                            case "ISS-INP":
                                iob.检验出数量 += s.数量;
                                break;
                            case "CYC-CNT":
                                iob.盘差数量 += s.数量;
                                break;
                            case "RCT-UNP":
                            case "ISS-UNP":
                                iob.计划外数量 += s.数量;
                                break;
                            default:
                                iob.其它数量 += s.数量;
                                break;
                        }
                        iob.期初数量 -= s.数量;
                    }
                }

                var _ends = new List<Trans>();
                endDic.TryGetValue(new
                {
                    库位 = 库位,
                    物料 = 物料
                }, out _ends);
                if (_ends != null)
                {
                    foreach (var s in _ends)
                    {
                        switch (s.类型)
                        {
                            case "RCT-PO":
                                iob.采购数量 -= s.数量;
                                break;
                            case "RCT-TR":
                                iob.移库入数量 -= s.数量;
                                break;
                            case "RCT-INP":
                                iob.检验入数量 -= s.数量;
                                break;
                            case "RCT-WO":
                                if (!s.IsSubcontract)
                                {
                                    iob.生产入数量 -= s.数量;
                                }
                                else
                                {
                                    iob.委外数量 -= s.数量;
                                }
                                break;
                            case "ISS-WO":
                            case "ISS-MIN":
                                if (!s.IsSubcontract)
                                {
                                    iob.生产出数量 -= s.数量;
                                }
                                else
                                {
                                    iob.委外数量 -= s.数量;
                                }
                                break;
                            case "ISS-SO":
                                iob.销售数量 -= s.数量;
                                break;
                            case "ISS-TR":
                                iob.移库出数量 -= s.数量;
                                break;
                            case "ISS-INP":
                                iob.检验出数量 -= s.数量;
                                break;
                            case "CYC-CNT":
                                iob.盘差数量 -= s.数量;
                                break;
                            case "RCT-UNP":
                            case "ISS-UNP":
                                iob.计划外数量 -= s.数量;
                                break;
                            default:
                                iob.其它数量 -= s.数量;
                                break;
                        }
                        iob.期末数量 -= s.数量;
                    }
                }
                #endregion

                #region
                iob.采购金额 = iob.采购数量 * iob.采购成本;
                iob.移库入金额 = iob.移库入数量 * iob.期末成本;
                iob.检验入金额 = iob.检验入数量 * iob.期末成本;
                iob.生产入金额 = iob.生产入数量 * iob.期末成本;
                iob.生产出金额 = iob.生产出数量 * iob.期末成本;
                iob.委外金额 = iob.委外数量 * iob.期末成本;
                iob.销售金额 = iob.销售数量 * iob.期末成本;
                iob.移库出金额 = iob.移库出数量 * iob.期末成本;
                iob.检验出金额 = iob.检验出数量 * iob.期末成本;
                iob.盘差金额 = iob.盘差数量 * iob.期末成本;
                iob.计划外金额 = iob.计划外数量 * iob.期末成本;
                iob.其它金额 = iob.其它数量 * iob.期末成本;
                iob.期初金额 = iob.期初数量 * iob.期初成本;
                iob.期末金额 = iob.期末数量 * iob.期末成本;
                #endregion

                if ((currentLocation.Trim().ToLower() != string.Empty && currentLocation.Trim().ToLower() != inv.库位.Trim().ToLower()))
                {
                    tiob.物料 = "合计";
                    tiob.库位 = currentLocation;
                    Iob newIob = this.CopyIob(tiob);
                    iobs.Add(newIob);
                    iobTotal.Add(newIob);
                    tiob = new Iob();
                }

                #region 累加
                /////数量汇总
                tiob.期初数量 += iob.期初数量;
                tiob.采购数量 += iob.采购数量;
                tiob.生产入数量 += iob.生产入数量;
                tiob.生产出数量 += iob.生产出数量;
                tiob.检验入数量 += iob.检验入数量;
                tiob.移库入数量 += iob.移库入数量;

                tiob.销售数量 += iob.销售数量;
                tiob.移库出数量 += iob.移库出数量;
                tiob.检验出数量 += iob.检验出数量;

                tiob.委外数量 += iob.委外数量;
                tiob.盘差数量 += iob.盘差数量;
                tiob.计划外数量 += iob.计划外数量;
                tiob.其它数量 += iob.其它数量;
                tiob.期末数量 += iob.期末数量;

                /////金额汇总
                tiob.期初金额 += iob.期初金额;
                tiob.采购金额 += iob.采购金额;
                tiob.生产入金额 += iob.生产入金额;
                tiob.生产出金额 += iob.生产出金额;
                tiob.检验入金额 += iob.检验入金额;
                tiob.移库入金额 += iob.移库入金额;

                tiob.销售金额 += iob.销售金额;
                tiob.移库出金额 += iob.移库出金额;
                tiob.检验出金额 += iob.检验出金额;

                tiob.委外金额 += iob.委外金额;
                tiob.盘差金额 += iob.盘差金额;
                tiob.计划外金额 += iob.计划外金额;
                tiob.其它金额 += iob.其它金额;
                tiob.期末金额 += iob.期末金额;
                #endregion

                iobs.Add(iob);

                i++;
                if (rowCount == i)
                {
                    tiob.物料 = "合计";
                    tiob.库位 = iob.库位;
                    Iob newIob = this.CopyIob(tiob);
                    iobs.Add(newIob);
                    iobTotal.Add(newIob);
                }
                currentLocation = inv.库位;

            }
            //lblItem.Text = DateTime.Now.ToLongTimeString();
            #endregion

            if (this.cbOnlyTotal.Checked)
            {
                this.GV_List.DataSource = iobTotal;
            }
            else
            {
                this.GV_List.DataSource = iobs;
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
        if (this.ModuleType == "Qty")
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (i == 6 || i == 7 || i == 8 || i == 10 || i == 12 || i == 14 || i == 16 || i == 18 || i == 20 || i == 22 || i == 24 || i == 26
                       || i == 28 || i == 30 || i == 32 || i == 34 || i == 36)
                {
                    e.Row.Cells[i].Visible = false;
                }
                else
                {
                    if (e.Row.Cells[i].Text.Contains("数量"))
                    {
                        e.Row.Cells[i].Text = e.Row.Cells[i].Text.Substring(0, e.Row.Cells[i].Text.Length - 2);
                    }
                }
            }
        }

        //if (this.ModuleType == "Qty")
        //{
        //    for (int i = 0; i < e.Row.Cells.Count; i++)
        //    {
        //        if (e.Row.Cells[i].Text.Contains("成本") || e.Row.Cells[i].Text.Contains("金额"))
        //        {
        //            e.Row.Cells[i].c
        //        }
        //    }
        //}

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            e.Row.Cells[5].Attributes.Add("style", "vnd.ms-excel.numberformat:@");

            string fc = this.tbFinanceYear.Text.Trim();

            for (int i = 6; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Text = (double.Parse(e.Row.Cells[i].Text)).ToString("0.######");
            }
        }
        e.Row.Cells[3].Visible = false;
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

        this.tbStartDate.Text = financeCalendar.StartDate.ToString("yyyy-MM-dd");
        this.tbEndDate.Text = financeCalendar.EndDate.ToString("yyyy-MM-dd");
    }

    class Iob
    {
        public string 物料 { get; set; }
        public string 物料描述 { get; set; }
        public string 库位 { get; set; }
        //public string 库位描述 { get; set; }
        public string 单位 { get; set; }
        public string 产品类 { get; set; }
        public decimal 期初成本 { get; set; }
        public decimal 期末成本 { get; set; }
        public decimal 采购成本 { get; set; }

        public decimal 期初数量 { get; set; }
        public decimal 期初金额 { get; set; }
        public decimal 采购数量 { get; set; }
        public decimal 采购金额 { get; set; }
        public decimal 生产入数量 { get; set; }
        public decimal 生产入金额 { get; set; }
        public decimal 移库入数量 { get; set; }
        public decimal 移库入金额 { get; set; }
        public decimal 检验入数量 { get; set; }
        public decimal 检验入金额 { get; set; }

        public decimal 销售数量 { get; set; }
        public decimal 销售金额 { get; set; }
        public decimal 生产出数量 { get; set; }
        public decimal 生产出金额 { get; set; }
        public decimal 检验出数量 { get; set; }
        public decimal 检验出金额 { get; set; }
        public decimal 移库出数量 { get; set; }
        public decimal 移库出金额 { get; set; }

        public decimal 委外数量 { get; set; }
        public decimal 委外金额 { get; set; }
        public decimal 计划外数量 { get; set; }
        public decimal 计划外金额 { get; set; }
        public decimal 盘差数量 { get; set; }
        public decimal 盘差金额 { get; set; }
        public decimal 其它数量 { get; set; }
        public decimal 其它金额 { get; set; }
        public decimal 期末数量 { get; set; }
        public decimal 期末金额 { get; set; }
    }

    class Inv
    {
        public string 物料 { get; set; }
        public string 物料描述 { get; set; }
        public string 库位 { get; set; }
        public decimal 数量 { get; set; }
        public double 期初成本 { get; set; }
        public double 期末成本 { get; set; }
        public string 单位 { get; set; }
        public string 产品类 { get; set; }
        public string CStartUom { get; set; }
        public string CEndUom { get; set; }
    }

    class Trans
    {
        public string 物料 { get; set; }
        public string 物料描述 { get; set; }
        public string 库位 { get; set; }
        public string 类型 { get; set; }
        public decimal 数量 { get; set; }
        public bool IsSubcontract { get; set; }
    }

    private decimal GetPurchasePrice(string itemCode, DateTime endDate, string targetUom, IList<Purchase> purchaseList)
    {
        if (this.ModuleType == "Qty")
        {
            return 1M;
        }
        else
        {
            var purchase = purchaseList.FirstOrDefault(l => l.Item.Trim().ToLower() == itemCode.Trim().ToLower());
            if (purchase != null)
            {
                return TheUomConversionMgr.ConvertUomQty(itemCode, targetUom, (decimal)purchase.AvgPrice, purchase.Uom);
            }
            else
            {
                Price price = GetPoPrice(itemCode, endDate);
                return TheUomConversionMgr.ConvertUomQty(itemCode, targetUom, price.UnitPrice, price.Uom);
            }
        }
    }

    private Price GetPoPrice(string itemCode, DateTime effDate)
    {
        string sql = @"select top 1 UnitPrice,Uom ,IsProvEst as IsProvisionalEstimate  from pricelistdet
                        left join pricelistmstr on pricelistmstr.Code = pricelistdet.pricelist
                        where item = @p0 and pricelistmstr.Type = 'Purchase'
                        and startdate<= @p1 and (enddate is null or enddate> @p1)
                        order by StartDate desc";

        SqlParameter[] sqlParam = new SqlParameter[2];

        sqlParam[0] = new SqlParameter("@p0", itemCode);
        sqlParam[1] = new SqlParameter("@p1", effDate);

        DataSet dataSet = TheSqlHelperMgr.GetDatasetBySql(sql, sqlParam);

        List<Price> priceList = IListHelper.DataTableToList<Price>(dataSet.Tables[0]);

        if (priceList != null && priceList.Count() > 0)
        {
            return priceList.First();
        }
        return new Price();
    }

    private Iob CopyIob(Iob iob)
    {
        Iob tiob = new Iob();
        tiob.物料 = "合计";
        tiob.库位 = iob.库位;
        /////数量汇总
        tiob.期初数量 = iob.期初数量;
        tiob.采购数量 = iob.采购数量;
        tiob.生产入数量 = iob.生产入数量;
        tiob.生产出数量 = iob.生产出数量;
        tiob.检验入数量 = iob.检验入数量;
        tiob.移库入数量 = iob.移库入数量;

        tiob.销售数量 = iob.销售数量;
        tiob.移库出数量 = iob.移库出数量;
        tiob.检验出数量 = iob.检验出数量;

        tiob.委外数量 = iob.委外数量;
        tiob.盘差数量 = iob.盘差数量;
        tiob.计划外数量 = iob.计划外数量;
        tiob.其它数量 = iob.其它数量;
        tiob.期末数量 = iob.期末数量;

        /////金额汇总
        tiob.期初金额 = iob.期初金额;
        tiob.采购金额 = iob.采购金额;
        tiob.生产入金额 = iob.生产入金额;
        tiob.检验入金额 = iob.检验入金额;
        tiob.移库入金额 = iob.移库入金额;

        tiob.销售金额 = iob.销售金额;
        tiob.移库出金额 = iob.移库出金额;
        tiob.检验出金额 = iob.检验出金额;

        tiob.委外金额 = iob.委外金额;
        tiob.盘差金额 = iob.盘差金额;
        tiob.计划外金额 = iob.计划外金额;
        tiob.其它金额 = iob.其它金额;
        tiob.期末金额 = iob.期末金额;
        return iob;
    }
}

class Price
{
    public decimal UnitPrice { get; set; }
    public string Uom { get; set; }
    public bool IsProvisionalEstimate { get; set; }
}

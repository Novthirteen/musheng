using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using System.Data;
using System.Data.SqlClient;

public partial class Cost_FinanceClose_Main : MainModuleBase
{


    protected void Page_Load(object sender, EventArgs e)
    {
        BindDataSource();
    }

    protected void lbtnClose_Click(object sender, EventArgs e)
    {
        try
        {
            //TheCostMgr.CloseFinanceMonth(this.CurrentUser);
            int id = int.Parse(((LinkButton)sender).CommandArgument);
            FinanceCalendar finaceCalendar = TheFinanceCalendarMgr.LoadFinanceCalendar(id);

            string sql = @"--删除已有的库存数据
                        delete from CostInvBalance where financeyear=@p0 and financemonth = @p1
                        --查找库存事务
                        select item,uom,loc,sum(qty) as qty into #trans
                        from loctrans where createdate>=@p2 group by item,uom,loc order by loc,item
                        --现在的库存
                        select item,location,sum(qty) as qty into #loc 
                        from locationlotdet group by item,location order by location,item
                        --倒推月结库存
                        insert into 
                        CostInvBalance(Item,ItemCategory,Location,CostGroup,Qty,FinanceYear,FinanceMonth,CreateDate,CreateUser)
                        select #loc.item,item.category,#loc.Location,'CG1',#loc.qty-isnull(#trans.qty,0) as sumqty,
                        @p0,@p1,getdate(),@p3
                        from #loc left join #trans on #loc.item = #trans.item
                        and #trans.loc = #loc.location
                        left join item on item.code = #loc.item";

            SqlParameter[] sqlParam = new SqlParameter[4];
            sqlParam[0] = new SqlParameter("@p0", finaceCalendar.FinanceYear);
            sqlParam[1] = new SqlParameter("@p1", finaceCalendar.FinanceMonth);
            sqlParam[2] = new SqlParameter("@p2", finaceCalendar.EndDate);
            sqlParam[3] = new SqlParameter("@p3", this.CurrentUser.Code);

            DataSet dataSet = TheSqlHelperMgr.GetDatasetBySql(sql, sqlParam);

            BindDataSource();
            ShowSuccessMessage("Cost.FinanceCalendar.Close.Successfully");
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Cost.FinanceCalendar.Close.Failed");
        }
    }

    private void BindDataSource()
    {
        FinanceCalendar finaceCalendar = TheFinanceCalendarMgr.GetLastestOpenFinanceCalendar();
        IList<FinanceCalendar> financeCalendarList = new List<FinanceCalendar>();
        financeCalendarList.Add(finaceCalendar);
        this.GV_List.DataSource = financeCalendarList;
        this.GV_List.DataBind();
    }

}

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.Sconit.Web;
using com.Sconit.Service.Ext.MasterData;
using System.IO;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Cost;

public partial class Cost_CostAllocateTransaction_List : ListModuleBase
{

    protected DateTime StartDate
    {
        get
        {
            return (DateTime)ViewState["StartDate"];
        }
        set
        {
            ViewState["StartDate"] = value;
        }
    }

    public EventHandler EditEvent;
    public EventHandler ViewEvent;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public override void UpdateView()
    {

        FinanceCalendar finaceCalendar = TheFinanceCalendarMgr.GetLastestOpenFinanceCalendar();
        this.StartDate = finaceCalendar.StartDate;
        this.GV_List.Execute();
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CostAllocateTransaction costAllocateTransaction = (CostAllocateTransaction)e.Row.DataItem;
            e.Row.FindControl("lbtnView").Visible = costAllocateTransaction.EffectiveDate < this.StartDate;
            e.Row.FindControl("lbtnDelete").Visible = costAllocateTransaction.EffectiveDate >= this.StartDate;
            e.Row.FindControl("lbtnEdit").Visible = costAllocateTransaction.EffectiveDate >= this.StartDate;
        }
    }

    protected void lbtnEdit_Click(object sender, EventArgs e)
    {
        if (EditEvent != null)
        {
            string code = ((LinkButton)sender).CommandArgument;
            EditEvent(code, e);
        }
    }

    protected void lbtnView_Click(object sender, EventArgs e)
    {
        if (ViewEvent != null)
        {
            string code = ((LinkButton)sender).CommandArgument;
            ViewEvent(code, e);
        }
    }


    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int id = Int32.Parse(((LinkButton)sender).CommandArgument);
            TheCostAllocateTransactionMgr.DeleteCostAllocateTransaction(id);
            ShowSuccessMessage("Cost.CostAllocateTransaction.Delete.Successfully");
            UpdateView();
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Cost.CostAllocateTransaction.Delete.Failed");
        }

    }

}

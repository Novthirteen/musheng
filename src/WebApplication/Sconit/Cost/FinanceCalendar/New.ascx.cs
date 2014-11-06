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
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Web;
using com.Sconit.Utility;
using com.Sconit.Entity.Cost;

public partial class Cost_FinanceCalendar_New : NewModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler CreateEvent;
    private FinanceCalendar financeCalendar;

    protected void Page_Load(object sender, EventArgs e)
    {
      
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    public void PageCleanup()
    {
       
     
    }

    protected void ODS_FinanceCalendar_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
         financeCalendar = (FinanceCalendar)e.InputParameters[0];
    }

   
    protected void ODS_FinanceCalendar_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ShowSuccessMessage("Cost.FinanceCalendar.Add.Successfully");
        if (CreateEvent != null)
        {
            CreateEvent(financeCalendar.Id, e);
        }
    }

    protected void checkFinanceCalendarExists(object source, ServerValidateEventArgs args)
    {
        TextBox tbYear = this.FV_FinanceCalendar.FindControl("tbYear") as TextBox;
        TextBox tbMonth = this.FV_FinanceCalendar.FindControl("tbMonth") as TextBox;
        if (tbYear.Text.Trim() == string.Empty || tbMonth.Text.Trim() == string.Empty)
        {
            args.IsValid = false;
            return;
        }
        if (TheFinanceCalendarMgr.GetFinanceCalendar(Int32.Parse(tbYear.Text.Trim()), Int32.Parse(tbMonth.Text.Trim())) != null)
        {
            args.IsValid = false;
            return;
        }
    }

  
}

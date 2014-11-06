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

public partial class Cost_ExpenseElement_New : NewModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler CreateEvent;

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
        ((TextBox)(this.FV_ExpenseElement.FindControl("tbCode"))).Text = string.Empty;

        ((TextBox)(this.FV_ExpenseElement.FindControl("tbDescription"))).Text = string.Empty;
     
    }

    protected void ODS_ExpenseElement_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
       
    }

   
    protected void ODS_ExpenseElement_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ShowSuccessMessage("Cost.ExpenseElement.Add.Successfully");
        if (CreateEvent != null)
        {
            string code = ((TextBox)(this.FV_ExpenseElement.FindControl("tbCode"))).Text;
            CreateEvent(code, e);
        }
    }

    protected void checkExpenseElementExists(object source, ServerValidateEventArgs args)
    {
        string code = ((TextBox)(this.FV_ExpenseElement.FindControl("tbCode"))).Text;

        if (TheExpenseElementMgr.LoadExpenseElement(code) != null)
        {
            args.IsValid = false;
        }
    }

  
}

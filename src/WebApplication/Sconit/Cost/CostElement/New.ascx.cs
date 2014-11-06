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

public partial class Cost_CostElement_New : NewModuleBase
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
        ((TextBox)(this.FV_CostElement.FindControl("tbCode"))).Text = string.Empty;

        ((TextBox)(this.FV_CostElement.FindControl("tbDescription"))).Text = string.Empty;
     
    }

    protected void ODS_CostElement_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        CostElement costElement = (CostElement)e.InputParameters[0];
        costElement.Category = ((com.Sconit.Control.CodeMstrDropDownList)(this.FV_CostElement.FindControl("ddlCategory"))).SelectedValue;
    }

   
    protected void ODS_CostElement_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ShowSuccessMessage("Cost.CostElement.Add.Successfully");
        if (CreateEvent != null)
        {
            string code = ((TextBox)(this.FV_CostElement.FindControl("tbCode"))).Text;
            CreateEvent(code, e);
        }
    }

    protected void checkCostElementExists(object source, ServerValidateEventArgs args)
    {
        string code = ((TextBox)(this.FV_CostElement.FindControl("tbCode"))).Text;

        if (TheCostElementMgr.LoadCostElement(code) != null)
        {
            args.IsValid = false;
        }
    }

  
}

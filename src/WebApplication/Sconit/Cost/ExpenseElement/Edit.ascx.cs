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
using com.Sconit.Entity.MasterData;
using com.Sconit.Control;
using com.Sconit.Utility;
using com.Sconit.Entity.Cost;

public partial class Cost_ExpenseElement_Edit : EditModuleBase
{
    public event EventHandler BackEvent;

    protected string Code
    {
        get
        {
            return (string)ViewState["Code"];
        }
        set
        {
            ViewState["Code"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }


    public void InitPageParameter(string code)
    {
        this.Code = code;
        this.ODS_ExpenseElement.SelectParameters["code"].DefaultValue = this.Code;
        this.ODS_ExpenseElement.DataBind();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }
    protected void FV_ExpenseElement_DataBound(object sender, EventArgs e)
    {
       
    }
    protected void ODS_ExpenseElement_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
    }
    protected void ODS_ExpenseElement_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ShowSuccessMessage("Cost.ExpenseElement.Update.Successfully");
    }

    protected void ODS_ExpenseElement_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            TheExpenseElementMgr.DeleteExpenseElement(this.Code);
            ShowSuccessMessage("Cost.ExpenseElement.Delete.Successfully");
            if (BackEvent != null)
            {
                BackEvent(this, e);
            }
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Cost.ExpenseElement.Delete.Failed");
        }
    }

}

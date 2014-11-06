using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Linq;
using log4net;
using com.Sconit.Web;
using com.Sconit.Service.Ext.Cost;
using com.Sconit.Entity.Cost;
using com.Sconit.Utility;

//TODO: Add other using statements here.by liqiuyun
public partial class Modules_Cost_RawIOB_Edit : EditModuleBase
{
    public event EventHandler Back;

    //Get the logger
    private static ILog log = LogManager.GetLogger("Cost");

    protected void Page_Load(object sender, EventArgs e)
    {
        //TODO: Add code for Page_Load here.
        if (!IsPostBack)
        {

        }
    }

    public void InitPageParameter(object Code)
    {
        this.ODS_RawIOB.SelectParameters["Id"].DefaultValue = Code.ToString();
        this.ODS_RawIOB.DeleteParameters["Id"].DefaultValue = Code.ToString();
        this.FV_RawIOB.DataBind();
    }

    protected void FV_RawIOB_DataBound(object sender, EventArgs e)
    {
        RawIOB dataItem = (RawIOB)this.FV_RawIOB.DataItem;
        if (dataItem != null)
        {
            if (dataItem.EndQty == 0)
            {
                ((TextBox)this.FV_RawIOB.FindControl("FV_RawIOB")).ReadOnly = true;
            }
        }
    }

    protected void ODS_RawIOB_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        RawIOB dataItem = (RawIOB)e.InputParameters[0];
        dataItem.LastModifyTime = DateTime.Now;
        dataItem.LastModifyUser = this.CurrentUser.Code;
        //Controls_TextBox tbPartyFrom = (Controls_TextBox)this.FV_RawIOB.FindControl("tbPartyFrom");
    }

    protected void ODS_RawIOB_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            ShowErrorMessage("Common.Business.Result.Update.Failed");
            e.ExceptionHandled = true;
        }
        else
        {
            Back(sender, e);
            ShowSuccessMessage("Common.Business.Result.Update.Successfully");
        }
    }

    protected void ODS_RawIOB_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            ShowErrorMessage("Common.Business.Result.Delete.Failed");
            e.ExceptionHandled = true;
        }
        else
        {
            btnBack_Click(this, e);
            ShowSuccessMessage("Common.Business.Result.Delete.Successfully");
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (Back != null)
        {
            this.Visible = false;
            Back(sender, e);
        }
    }
}
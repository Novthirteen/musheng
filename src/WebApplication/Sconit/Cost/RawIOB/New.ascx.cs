using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Linq;
using log4net;
using com.Sconit.Web;
using com.Sconit.Service.Ext.Cost;
using com.Sconit.Entity.Cost;
using com.Sconit.Utility;

//TODO:Add other using statements here.by liqiuyun
public partial class Modules_Cost_RawIOB_New : NewModuleBase
{
    public event EventHandler Back;
    public event EventHandler Create;
    public object name
    {
        get { return ViewState["name"]; }
        set { ViewState["name"] = value; }
    }
    //Get the logger
    private static ILog log = LogManager.GetLogger("Cost");

    protected void Page_Load(object sender, EventArgs e)
    {
        //Add code for Page_Load here.
        //Controls_TextBox tbLocFrom = (Controls_TextBox)this.FV_RawIOB.FindControl("tbLocFrom");
        //tbLocFrom.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:";
        //tbLocFrom.DataBind();
        if (!IsPostBack)
        {

        }
    }

    protected void ODS_RawIOB_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        RawIOB dataItem = (RawIOB)e.InputParameters[0];
        this.name = dataItem.Id;
        dataItem.CreateUser = this.CurrentUser.Code;
        dataItem.CreateTime = DateTime.Now;
        dataItem.LastModifyUser = this.CurrentUser.Code;
        dataItem.LastModifyTime = DateTime.Now;
    }

    protected void ODS_RawIOB_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            ShowErrorMessage("Common.Business.Result.Insert.Failed");
            e.ExceptionHandled = true;
        }
        else
        {
            if (Create != null)
            {
                Create(this.name, e);
                ShowSuccessMessage("Common.Business.Result.Insert.Successfully");
            }
        }
    }

    //The event handler when user click button "Back"
    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (Back != null)
        {
            Back(this, e);
        }
    }

    public void PageCleanup()
    {
        ((TextBox)(this.FV_RawIOB.FindControl("tbItem"))).Text = string.Empty;
        ((TextBox)(this.FV_RawIOB.FindControl("tbUom"))).Text = string.Empty;
        ((TextBox)(this.FV_RawIOB.FindControl("tbStartQty"))).Text = string.Empty;
        ((TextBox)(this.FV_RawIOB.FindControl("tbStartAmount"))).Text = string.Empty;
        ((TextBox)(this.FV_RawIOB.FindControl("tbStartCost"))).Text = string.Empty;
        ((TextBox)(this.FV_RawIOB.FindControl("tbInQty"))).Text = string.Empty;
        ((TextBox)(this.FV_RawIOB.FindControl("tbInAmount"))).Text = string.Empty;
        ((TextBox)(this.FV_RawIOB.FindControl("tbInCost"))).Text = string.Empty;
        ((TextBox)(this.FV_RawIOB.FindControl("tbDiffAmount"))).Text = string.Empty;
        ((TextBox)(this.FV_RawIOB.FindControl("tbDiffCost"))).Text = string.Empty;
        ((TextBox)(this.FV_RawIOB.FindControl("tbEndQty"))).Text = string.Empty;
        ((TextBox)(this.FV_RawIOB.FindControl("tbEndAmount"))).Text = string.Empty;
        ((TextBox)(this.FV_RawIOB.FindControl("tbEndCost"))).Text = string.Empty;
        ((TextBox)(this.FV_RawIOB.FindControl("tbFinanceCalendar"))).Text = string.Empty;
        ((TextBox)(this.FV_RawIOB.FindControl("tbCreateDate"))).Text = string.Empty;

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;

public partial class Quote_Customer_List : ListModuleBase
{
    public EventHandler EditEvent;

    public object Para
    {
        get
        {
            return (object)ViewState["Para"];
        }
        set
        {
            ViewState["Para"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void InitPageParameter(object para)
    {
        Para = para;
        GV_List.DataSource = TheToolingMgr.GetQuoteCustomer(para);
        GV_List.DataBind();
    }

    protected void edit(object sender, GridViewEditEventArgs e)
    {
        //GridViewRow row = GV_List.Rows[e.NewEditIndex];
        //string stype = ((Literal)row.FindControl("ltlSType")).Text;

        //GV_List.EditIndex = e.NewEditIndex;
        //InitPageParameter();

        //DropDownList dplSType = (DropDownList)row.FindControl("ddlSType");
        //IList<SType> st = TheToolingMgr.GetQuoteSType();
        //dplSType.DataSource = st;
        //dplSType.DataTextField = "Name";
        //dplSType.DataValueField = "Name";
        //dplSType.DataBind();

        //dplSType.SelectedValue = stype;
    }

    protected void cancel(object sender, GridViewCancelEditEventArgs e)
    { }

    protected void update(object sender, GridViewUpdateEventArgs e)
    { }

    protected void delete(object sender, GridViewDeleteEventArgs e)
    { }

    protected void lbtnEdit_Click(object sender, EventArgs e)
    {
        string id = ((LinkButton)sender).CommandArgument;
        EditEvent(id, e);
    }
    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        string id = ((LinkButton)sender).CommandArgument;
        TheToolingMgr.DeleteQuoteCustomerById(id);

        GV_List.DataSource = TheToolingMgr.GetQuoteCustomer(Para);
        GV_List.DataBind();
    }
}
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
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using NHibernate.Expression;
using com.Sconit.Entity;

public partial class MasterData_Facility_List : ListModuleBase
{
    public event EventHandler ListEditEvent;
    public event EventHandler ListViewEvent;

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

    public string FlowCode
    {
        get
        {
            return (string)ViewState["FlowCode"];
        }
        set
        {
            ViewState["FlowCode"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UpdateView();
        }
    }

    public override void UpdateView()
    {
        this.GV_List.Execute();
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        Int32 id = Int32.Parse(((LinkButton)sender).CommandArgument);
        TheProductLineFacilityMgr.DeleteProductLineFacility(id);
        ShowSuccessMessage("MasterData.Flow.Facility.DeleteProductLineFacility.Successfully", id.ToString());
        UpdateView();
    }

    protected void lbtnEdit_Click(object sender, EventArgs e)
    {
        string code = ((LinkButton)sender).CommandArgument;
        ListEditEvent(code, e);
    }

    protected void lbtnView_Click(object sender, EventArgs e)
    {
        string code = ((LinkButton)sender).CommandArgument;
        ListViewEvent(code, e);
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        /*
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ProductLineFacility productLineFacility = (ProductLineFacility)e.Row.DataItem;
            
        }*/
    }
}

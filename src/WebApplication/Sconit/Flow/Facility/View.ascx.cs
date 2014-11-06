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
using com.Sconit.Service.Ext.Procurement;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Entity;

public partial class MasterData_Facility_View : ModuleBase
{
    public event EventHandler BackEvent;
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

        }
    }


    protected void FV_ProductLineFacility_DataBound(object sender, EventArgs e)
    {

    }

    public void InitPageParameter(Int32  id)
    {
        this.ODS_ProductLineFacility.SelectParameters["id"].DefaultValue = id.ToString();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }




}

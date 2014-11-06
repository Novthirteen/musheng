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
using com.Sconit.Utility;
using com.Sconit.Web;
using NHibernate.Expression;
using com.Sconit.Entity;
using System.Drawing.Imaging;
using System.Drawing;

public partial class MasterData_List : ListModuleBase
{
    public EventHandler ViewEvent;
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

        public bool IsGroup
    {
        get { return ViewState["isGroup"] == null ? true : (bool)ViewState["isGroup"]; }
        set { ViewState["isGroup"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public override void UpdateView()
    {
        if (IsGroup)
        {
            this.GV_List.Execute();
            this.GV_List.Visible = true;
            this.gp.Visible = true;
            this.GV_List_Detail.Visible = false;
            this.gp_Detail.Visible = false;
        }
        else
        {
            this.GV_List_Detail.Execute();
            this.GV_List.Visible = false;
            this.GV_List_Detail.Visible = true;
            this.gp.Visible = false;
            this.gp_Detail.Visible = true;
        }

       // this.GV_List.Execute();
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            
        }
    }

    protected void GV_List_Detail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

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
    }
}

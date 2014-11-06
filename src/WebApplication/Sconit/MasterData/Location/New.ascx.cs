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
using com.Sconit.Control;

public partial class MasterData_Location_New : NewModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler CreateEvent;
    public event EventHandler NewEvent;

    private Location location;

    protected void Page_Load(object sender, EventArgs e)
    {
        ((Controls_TextBox)(this.FV_Location.FindControl("tbRegion"))).ServiceParameter = "string:" + this.CurrentUser.Code;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void FV_Location_OnDataBinding(object sender, EventArgs e)
    {

    }

    public void PageCleanup()
    {
        ((CheckBox)(this.FV_Location.FindControl("tbIsActive"))).Checked = true;
        ((CheckBox)(this.FV_Location.FindControl("tbIsMRP"))).Checked = true;
        ((TextBox)(this.FV_Location.FindControl("tbCode"))).Text = string.Empty;
        ((Controls_TextBox)(this.FV_Location.FindControl("tbRegion"))).Text = string.Empty;
        ((TextBox)(this.FV_Location.FindControl("tbName"))).Text = string.Empty;
        ((TextBox)(this.FV_Location.FindControl("tbVolume"))).Text = "1";
        ((CheckBox)(this.FV_Location.FindControl("tbAllowNegativeInventory"))).Checked = false;
        //((CheckBox)(this.FV_Location.FindControl("cbEnableAdv"))).Checked = false;
        ((com.Sconit.Control.CodeMstrDropDownList)(this.FV_Location.FindControl("ddlLocatoinType"))).SelectedIndex = 0;

    }

    protected void ODS_Location_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        location = (Location)e.InputParameters[0];
        location.Code = location.Code.Trim();
        location.Name = location.Name.Trim();
        if (location != null)
        {
            string region = ((Controls_TextBox)(this.FV_Location.FindControl("tbRegion"))).Text.Trim();
            location.Region = TheRegionMgr.LoadRegion(region);
        }
        if (TheLocationMgr.LoadLocation(location.Code) != null)
        {
            ShowErrorMessage("MasterData.Location.CodeExist", location.Code);
            NewEvent(sender, e);
            e.Cancel = true;
        }
        com.Sconit.Control.CodeMstrDropDownList ddlLocatoinType = ((com.Sconit.Control.CodeMstrDropDownList)(this.FV_Location.FindControl("ddlLocatoinType")));
        location.Type = ddlLocatoinType.SelectedValue;
  
    }

    protected void ODS_Location_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (CreateEvent != null)
        {
            CreateEvent(location.Code + "," + location.EnableAdvWM, e);
            ShowSuccessMessage("MasterData.Location.AddLocation.Successfully", location.Code);
        }
    }

    protected void checkLocationExists(object source, ServerValidateEventArgs args)
    {
        string code = ((TextBox)(this.FV_Location.FindControl("tbCode"))).Text;

        if (TheLocationMgr.LoadLocation(code) != null)
        {
            ShowErrorMessage("MasterData.Location.CodeExist", code);
            args.IsValid = false;
        }
    }
}

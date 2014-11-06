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
using com.Sconit.Service.MasterData;
using com.Sconit.Utility;
using com.Sconit.Web;
using com.Sconit.Entity;
using com.Sconit.Entity.Customize;

public partial class MasterData_LedColorLevel_New : NewModuleBase
{
    private LedColorLevel ledColorLevel;
    public event EventHandler CreateEvent;
    public event EventHandler BackEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
    }
    
    public void PageCleanup()
    {
        ((Controls_TextBox)(this.FV_LedColorLevel.FindControl("tbItemCode"))).Text = string.Empty;
        ((TextBox)(this.FV_LedColorLevel.FindControl("tbSequence"))).Text = string.Empty;
        ((TextBox)(this.FV_LedColorLevel.FindControl("tbValue"))).Text = string.Empty;
    }

  

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void ODS_LedColorLevel_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        ledColorLevel = (LedColorLevel)e.InputParameters[0];

        Controls_TextBox tbItemCode = (Controls_TextBox)(this.FV_LedColorLevel.FindControl("tbItemCode"));
        DropDownList ddlItemBrand = (DropDownList)(this.FV_LedColorLevel.FindControl("ddlItemBrand"));
        if (tbItemCode != null && tbItemCode.Text.Trim() != string.Empty)
        {
            ledColorLevel.Item = TheItemMgr.LoadItem(tbItemCode.Text.Trim());
        }
        if (ddlItemBrand != null && ddlItemBrand.Text.Trim() != string.Empty)
        {
            ledColorLevel.Brand = TheItemBrandMgr.LoadItemBrand(ddlItemBrand.Text.Trim());
        }

        ledColorLevel.CreateDate = DateTime.Now;
        ledColorLevel.CreateUser = this.CurrentUser.Code;
        ledColorLevel.LastModifyDate = DateTime.Now;
        ledColorLevel.LastModifyUser = this.CurrentUser.Code;
   
    }

    protected void ODS_LedColorLevel_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (CreateEvent != null)
        {
            CreateEvent(ledColorLevel.Id, e);
            ShowSuccessMessage("MasterData.LedColorLevel.AddLedColorLevel.Successfully");
        }
    }
}

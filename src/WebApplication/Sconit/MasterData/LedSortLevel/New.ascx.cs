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

public partial class MasterData_LedSortLevel_New : NewModuleBase
{
    private LedSortLevel ledSortLevel;
    public event EventHandler CreateEvent;
    public event EventHandler BackEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
    }
    
    public void PageCleanup()
    {
        ((Controls_TextBox)(this.FV_LedSortLevel.FindControl("tbItemCode"))).Text = string.Empty;
        ((TextBox)(this.FV_LedSortLevel.FindControl("tbSequence"))).Text = string.Empty;
        ((TextBox)(this.FV_LedSortLevel.FindControl("tbValue"))).Text = string.Empty;
    }

  

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void ODS_LedSortLevel_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        ledSortLevel = (LedSortLevel)e.InputParameters[0];

        Controls_TextBox tbItemCode = (Controls_TextBox)(this.FV_LedSortLevel.FindControl("tbItemCode"));
        DropDownList ddlItemBrand = (DropDownList)(this.FV_LedSortLevel.FindControl("ddlItemBrand"));
        if (tbItemCode != null && tbItemCode.Text.Trim() != string.Empty)
        {
            ledSortLevel.Item = TheItemMgr.LoadItem(tbItemCode.Text.Trim());
        }
        if (ddlItemBrand != null && ddlItemBrand.Text.Trim() != string.Empty)
        {
            ledSortLevel.Brand = TheItemBrandMgr.LoadItemBrand(ddlItemBrand.Text.Trim());
        }

        ledSortLevel.CreateDate = DateTime.Now;
        ledSortLevel.CreateUser = this.CurrentUser.Code;
        ledSortLevel.LastModifyDate = DateTime.Now;
        ledSortLevel.LastModifyUser = this.CurrentUser.Code;
   
    }

    protected void ODS_LedSortLevel_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (CreateEvent != null)
        {
            CreateEvent(ledSortLevel.Id, e);
            ShowSuccessMessage("MasterData.LedSortLevel.AddLedSortLevel.Successfully");
        }
    }
}

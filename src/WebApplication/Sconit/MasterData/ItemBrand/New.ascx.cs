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

public partial class MasterData_ItemBrand_New : NewModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler CreateEvent;
    public event EventHandler NewEvent;

    private ItemBrand ItemBrand;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void checkItemExists(object source, ServerValidateEventArgs args)
    {
        string code = ((TextBox)(this.FV_ItemBrand.FindControl("tdCode"))).Text;

        if (TheItemBrandMgr.LoadItemBrand(code) != null)
        {
            ShowErrorMessage("MasterData.ItemBrand.CodeExist", code);
            args.IsValid = false;
        }
    }

    protected void FV_ItemBrand_OnDataBinding(object sender, EventArgs e)
    {

    }

    public void PageCleanup()
    {
        ((TextBox)(this.FV_ItemBrand.FindControl("tdCode"))).Text = string.Empty;
        ((TextBox)(this.FV_ItemBrand.FindControl("tdDescription"))).Text = string.Empty;
        ((TextBox)(this.FV_ItemBrand.FindControl("tdAbbreviation"))).Text = string.Empty;
        ((TextBox)(this.FV_ItemBrand.FindControl("tbManufactureParty"))).Text = string.Empty;
        ((TextBox)(this.FV_ItemBrand.FindControl("tbOrigin"))).Text = string.Empty;
        ((TextBox)(this.FV_ItemBrand.FindControl("tbManufactureAddress"))).Text = string.Empty;
        ((CheckBox)(this.FV_ItemBrand.FindControl("tbIsActive"))).Checked = true;
    }

    protected void ODS_ItemBrand_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        ItemBrand = (ItemBrand)e.InputParameters[0];
        
    }

    protected void ODS_ItemBrand_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (CreateEvent != null)
        {
          //  CreateEvent(ItemBrand.Code, e);
            ShowSuccessMessage("MasterData.ItemBrand.AddItemBrand.Successfully", ItemBrand.Code);
        }
    }
}

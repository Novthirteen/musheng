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
using com.Sconit.Web;
using com.Sconit.Entity;
using com.Sconit.Entity.Customize;

public partial class MasterData_LedColorLevel_Edit : EditModuleBase
{
    public event EventHandler BackEvent;

    protected Int32 Id
    {
        get
        {
            return (Int32)ViewState["Id"];
        }
        set
        {
            ViewState["Id"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public void InitPageParameter(int id)
    {
        this.Id = id;
        this.ODS_LedColorLevel.SelectParameters["Id"].DefaultValue = this.Id.ToString();
        this.ODS_LedColorLevel.DeleteParameters["Id"].DefaultValue = this.Id.ToString();
    }

    protected void FV_LedColorLevel_DataBound(object sender, EventArgs e)
    {

        LedColorLevel ledColorLevel = TheLedColorLevelMgr.LoadLedColorLevel(Id);
        if (ledColorLevel.Item != null)
        {
            ((Controls_TextBox)(this.FV_LedColorLevel.FindControl("tbItemCode"))).Text = ledColorLevel.Item.Code;
        }
        if (ledColorLevel.Brand != null)
        {
            ((DropDownList)(this.FV_LedColorLevel.FindControl("ddlItemBrand"))).Text = ledColorLevel.Brand.Code;
        }


    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void ODS_LedColorLevel_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        LedColorLevel oldledColorLevel = TheLedColorLevelMgr.LoadLedColorLevel(Id);
        LedColorLevel ledColorLevel = (LedColorLevel)e.InputParameters[0];
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
        ledColorLevel.CreateDate = oldledColorLevel.CreateDate;
        ledColorLevel.CreateUser = oldledColorLevel.CreateUser;
        ledColorLevel.LastModifyDate = DateTime.Now;
        ledColorLevel.LastModifyUser = this.CurrentUser.Code;
    }

    protected void ODS_LedColorLevel_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ShowSuccessMessage("MasterData.LedColorLevel.UpdateLedColorLevel.Successfully");

    }

    protected void ODS_LedColorLevel_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception == null)
        {
            btnBack_Click(this, e);
            ShowSuccessMessage("MasterData.LedColorLevel.DeleteLedColorLevel.Successfully");
        }
        else if (e.Exception.InnerException is Castle.Facilities.NHibernateIntegration.DataException)
        {
            ShowErrorMessage("MasterData.LedColorLevel.DeleteLedColorLevel.Fail");
            e.ExceptionHandled = true;
        }
    }
}

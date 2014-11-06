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

public partial class MasterData_LedSortLevel_Edit : EditModuleBase
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
        this.ODS_LedSortLevel.SelectParameters["Id"].DefaultValue = this.Id.ToString();
        this.ODS_LedSortLevel.DeleteParameters["Id"].DefaultValue = this.Id.ToString();
    }

    protected void FV_LedSortLevel_DataBound(object sender, EventArgs e)
    {

        LedSortLevel ledSortLevel = TheLedSortLevelMgr.LoadLedSortLevel(Id);
        if (ledSortLevel.Item != null)
        {
            ((Controls_TextBox)(this.FV_LedSortLevel.FindControl("tbItemCode"))).Text = ledSortLevel.Item.Code;
        }
        if (ledSortLevel.Brand != null)
        {
            ((DropDownList)(this.FV_LedSortLevel.FindControl("ddlItemBrand"))).Text = ledSortLevel.Brand.Code;
        }


    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void ODS_LedSortLevel_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        LedSortLevel oldledSortLevel = TheLedSortLevelMgr.LoadLedSortLevel(Id);
        LedSortLevel ledSortLevel = (LedSortLevel)e.InputParameters[0];
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
        ledSortLevel.CreateDate = oldledSortLevel.CreateDate;
        ledSortLevel.CreateUser = oldledSortLevel.CreateUser;
        ledSortLevel.LastModifyDate = DateTime.Now;
        ledSortLevel.LastModifyUser = this.CurrentUser.Code;
    }

    protected void ODS_LedSortLevel_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ShowSuccessMessage("MasterData.LedSortLevel.UpdateLedSortLevel.Successfully");

    }

    protected void ODS_LedSortLevel_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception == null)
        {
            btnBack_Click(this, e);
            ShowSuccessMessage("MasterData.LedSortLevel.DeleteLedSortLevel.Successfully");
        }
        else if (e.Exception.InnerException is Castle.Facilities.NHibernateIntegration.DataException)
        {
            ShowErrorMessage("MasterData.LedSortLevel.DeleteLedSortLevel.Fail");
            e.ExceptionHandled = true;
        }
    }
}

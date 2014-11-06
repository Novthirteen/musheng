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
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Control;

public partial class MasterData_ItemBrand_Edit : EditModuleBase
{
    public event EventHandler BackEvent;

    private ItemBrand itemBrand;
    protected string code
    {
        get
        {
            return (string)ViewState["Code"];
        }
        set
        {
            ViewState["Code"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {


    }


    protected void FV_ItemBrand_DataBound(object sender, EventArgs e)
    {

        ItemBrand itemBrand = (ItemBrand)(((FormView)(sender)).DataItem);
 
    }
    public void InitPageParameter(string code)
    {
        this.code = code;
        this.ODS_ItemBrand.SelectParameters["Code"].DefaultValue = this.code;
        itemBrand =  TheItemBrandMgr.LoadItemBrand(code);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void ODS_ItemBrand_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ShowSuccessMessage("MasterData.ItemBrand.UpdateItemBrand.Successfully", code);
       // btnBack_Click(this, e);
    }

    protected void ODS_ItemBrand_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        ItemBrand itemBrand = (ItemBrand)e.InputParameters[0];

    }

    protected void ODS_ItemBrand_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception == null)
        {
            btnBack_Click(this, e);
            ShowSuccessMessage("MasterData.ItemBrand.DeleteItemBrand.Successfully", code);
        }
        else if (e.Exception.InnerException is Castle.Facilities.NHibernateIntegration.DataException)
        {
            ShowErrorMessage("MasterData.ItemBrand.DeleteItemBrand.Fail", code);
            e.ExceptionHandled = true;
        }
    }
}

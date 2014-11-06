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

public partial class MasterData_TaxRate_Edit : EditModuleBase
{
    public event EventHandler BackEvent;

    protected string TaxRateCode
    {
        get
        {
            return (string)ViewState["TaxRateCode"];
        }
        set
        {
            ViewState["TaxRateCode"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {


    }

    protected void FV_TaxRate_DataBound(object sender, EventArgs e)
    {
        
    }

    public void InitPageParameter(string code)
    {
        this.TaxRateCode = code;
        this.ODS_TaxRate.SelectParameters["code"].DefaultValue = this.TaxRateCode;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void ODS_TaxRate_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ShowSuccessMessage("MasterData.TaxRate.UpdateTaxRate.Successfully", TaxRateCode);
        btnBack_Click(this, e);
    }

    protected void ODS_TaxRate_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        TaxRate taxRate = (TaxRate)e.InputParameters[0];
        if (taxRate != null)
        {
            taxRate.Code = taxRate.Code.Trim();
        }
    }

    protected void ODS_TaxRate_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception == null)
        {
            btnBack_Click(this, e);
            ShowSuccessMessage("MasterData.TaxRate.DeleteTaxRate.Successfully", TaxRateCode);
        }
        else if (e.Exception.InnerException is Castle.Facilities.NHibernateIntegration.DataException)
        {
            ShowErrorMessage("MasterData.TaxRate.DeleteTaxRate.Fail", TaxRateCode);
            e.ExceptionHandled = true;
        }
    }
}

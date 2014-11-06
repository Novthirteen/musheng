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

public partial class MasterData_TaxRate_New : NewModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler CreateEvent;
    public event EventHandler NewEvent;

    private TaxRate taxRate;
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

    protected void FV_TaxRate_OnDataBinding(object sender, EventArgs e)
    {

    }

    public void PageCleanup()
    {
        ((TextBox)(this.FV_TaxRate.FindControl("tbCode"))).Text = string.Empty;
        ((TextBox)(this.FV_TaxRate.FindControl("tbDesc"))).Text = string.Empty;
        ((TextBox)(this.FV_TaxRate.FindControl("tbTaxRate"))).Text = string.Empty;
    }

    protected void ODS_TaxRate_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        taxRate = (TaxRate)e.InputParameters[0];
        if (taxRate != null)
        {
            taxRate.Code = taxRate.Code.Trim();
            taxRate.Description = taxRate.Description.Trim();
        }
    }

    protected void ODS_TaxRate_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (CreateEvent != null)
        {
            CreateEvent(taxRate.Code, e);
            ShowSuccessMessage("MasterData.TaxRate.AddTaxRate.Successfully", taxRate.Code);
        }
    }

    protected void checkTaxRate(object source, ServerValidateEventArgs args)
    {
        CustomValidator cv = (CustomValidator)source;

        switch (cv.ID)
        {
            case "cvInsert":
                if (TheTaxRateMgr.LoadTaxRate(args.Value) != null)
                {
                    ShowErrorMessage("MasterData.TaxRate.CodeExist", args.Value);
                    args.IsValid = false;
                }
                break;
            default:
                break;
        }
    }
}

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
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;

public partial class MasterData_Customer_New : NewModuleBase
{
    private Customer customer;
    public event EventHandler CreateEvent;
    public event EventHandler BackEvent;

    protected void Page_Load(object sender, EventArgs e)
    {

    }


    public void PageCleanup()
    {
        ((TextBox)(this.FV_Customer.FindControl("tbCode"))).Text = string.Empty;
        ((TextBox)(this.FV_Customer.FindControl("tbName"))).Text = string.Empty;
        ((TextBox)(this.FV_Customer.FindControl("tbAging"))).Text = "0";
        ((CheckBox)(this.FV_Customer.FindControl("cbIsActive"))).Checked = true;
    }

    protected void checkCustomerExists(object source, ServerValidateEventArgs args)
    {
        string code = ((TextBox)(this.FV_Customer.FindControl("tbCode"))).Text;

        if (TheCustomerMgr.LoadCustomer(code) != null)
        {
            args.IsValid = false;
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        CustomValidator cvInsert = ((CustomValidator)(this.FV_Customer.FindControl("cvInsert")));
        if (cvInsert.IsValid)
        {
            TextBox tbCode = (TextBox)(this.FV_Customer.FindControl("tbCode"));
            TextBox tbName = (TextBox)(this.FV_Customer.FindControl("tbName"));
            TextBox tbAging = (TextBox)(this.FV_Customer.FindControl("tbAging"));
            CheckBox cbIsActive = (CheckBox)(this.FV_Customer.FindControl("cbIsActive"));
            Customer customer = new Customer();
            customer.Code = tbCode.Text.Trim();
            customer.Name = tbName.Text.Trim();
            customer.Aging = Int32.Parse(tbAging.Text.Trim());
            customer.IsActive = cbIsActive.Checked;
            try
            {
                TheCustomerMgr.CreateCustomer(customer, this.CurrentUser);
                ShowSuccessMessage("MasterData.Customer.AddCustomer.Successfully", customer.Code);
                if (CreateEvent != null)
                {
                    CreateEvent(customer.Code, e);

                }
            }
            catch (BusinessErrorException ex)
            {
                ShowErrorMessage(ex);
            }
        }
    }

}

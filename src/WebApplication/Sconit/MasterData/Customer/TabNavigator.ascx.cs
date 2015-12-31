﻿using System;
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

public partial class MasterData_Customer_TabNavigator : System.Web.UI.UserControl
{
    public event EventHandler lblCustomerClickEvent;
    public event EventHandler lblBillAddressClickEvent;
    public event EventHandler lblShipAddressClickEvent;
    public event EventHandler lblQuoteFeeClickEvent;

    public void UpdateView()
    {
        lbCustomer_Click(this, null);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void lbCustomer_Click(object sender, EventArgs e)
    {
        if (lblCustomerClickEvent != null)
        {
            lblCustomerClickEvent(this, e);
        }

        this.tab_customer.Attributes["class"] = "ajax__tab_active";
        this.tab_billaddress.Attributes["class"] = "ajax__tab_inactive";
        this.tab_shipaddress.Attributes["class"] = "ajax__tab_inactive";
        this.tab_quoteinfo.Attributes["class"] = "ajax__tab_inactive";
    }


    protected void lbBillAddress_Click(object sender, EventArgs e)
    {
        if (lblBillAddressClickEvent != null)
        {
            lblBillAddressClickEvent(this, e);

            this.tab_customer.Attributes["class"] = "ajax__tab_inactive";
            this.tab_billaddress.Attributes["class"] = "ajax__tab_active";
            this.tab_shipaddress.Attributes["class"] = "ajax__tab_inactive";
            this.tab_quoteinfo.Attributes["class"] = "ajax__tab_inactive";
        }
    }

    protected void lbShipAddress_Click(object sender, EventArgs e)
    {
        if (lblShipAddressClickEvent != null)
        {
            lblShipAddressClickEvent(this, e);

            this.tab_customer.Attributes["class"] = "ajax__tab_inactive";
            this.tab_billaddress.Attributes["class"] = "ajax__tab_inactive";
            this.tab_shipaddress.Attributes["class"] = "ajax__tab_active";
            this.tab_quoteinfo.Attributes["class"] = "ajax__tab_inactive";
        }
    }

    protected void lblQuoteFee_Click(object sender, EventArgs e)
    {
        lblQuoteFeeClickEvent(this, e);

        this.tab_customer.Attributes["class"] = "ajax__tab_inactive";
        this.tab_billaddress.Attributes["class"] = "ajax__tab_inactive";
        this.tab_shipaddress.Attributes["class"] = "ajax__tab_inactive";
        this.tab_quoteinfo.Attributes["class"] = "ajax__tab_active";
    }
}

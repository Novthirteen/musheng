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

public partial class MasterData_Supplier_TabNavigator : System.Web.UI.UserControl
{
    public event EventHandler lblSupplierClickEvent;
    public event EventHandler lblBillAddressClickEvent;
    public event EventHandler lblShipAddressClickEvent;

    public void UpdateView()
    {
        lbSupplier_Click(this, null);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void lbSupplier_Click(object sender, EventArgs e)
    {
        if (lblSupplierClickEvent != null)
        {
            lblSupplierClickEvent(this, e);
        }

        this.tab_supplier.Attributes["class"] = "ajax__tab_active";
        this.tab_billaddress.Attributes["class"] = "ajax__tab_inactive";
        this.tab_shipaddress.Attributes["class"] = "ajax__tab_inactive";
    }

  

    protected void lbBillAddress_Click(object sender, EventArgs e)
    {
        if (lblBillAddressClickEvent != null)
        {
            lblBillAddressClickEvent(this, e);

            this.tab_supplier.Attributes["class"] = "ajax__tab_inactive";
            this.tab_billaddress.Attributes["class"] = "ajax__tab_active";
            this.tab_shipaddress.Attributes["class"] = "ajax__tab_inactive";
        }
    }

    protected void lbShipAddress_Click(object sender, EventArgs e)
    {
        if (lblShipAddressClickEvent != null)
        {
            lblShipAddressClickEvent(this, e);

            this.tab_supplier.Attributes["class"] = "ajax__tab_inactive";
            this.tab_billaddress.Attributes["class"] = "ajax__tab_inactive";
            this.tab_shipaddress.Attributes["class"] = "ajax__tab_active";
        }
    }
}

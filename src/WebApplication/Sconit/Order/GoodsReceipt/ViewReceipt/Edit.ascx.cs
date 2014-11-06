﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;

public partial class Order_GoodsReceipt_ViewReceipt_Edit : EditModuleBase
{
    public string ReceiptNo
    {
        get { return (string)ViewState["ReceiptNo"]; }
        set { ViewState["ReceiptNo"] = value; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void InitPageParameter(string receiptNo)
    {
        this.ODS_Receipt.SelectParameters["code"].DefaultValue = receiptNo;
        this.ReceiptNo = receiptNo;
        this.FV_Receipt.DataBind();
    }

    protected void FV_DataBound(object sender, EventArgs e)
    {
    }
}

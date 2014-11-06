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
using com.Sconit.Web;
using com.Sconit.Entity;

public partial class Cost_Report_InvIOB_Main : MainModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucSearch.SearchEvent += new System.EventHandler(this.Search_Render);
        this.ucSearch.ExportEvent += new EventHandler(ucSearch_ExportEvent);

        if (!IsPostBack)
        {
            if (this.Action == BusinessConstants.PAGE_LIST_ACTION)
            {
                ucSearch.QuickSearch(this.ActionParameter);
            }
        }
    }

    void ucSearch_ExportEvent(object sender, EventArgs e)
    {
        this.ucList.IsExport = true;
        this.ucList.InitPageParameter(sender);
        this.ucList.Visible = true;
        this.ucList.Export();
    }

    //The event handler when user click button "Search" button
    void Search_Render(object sender, EventArgs e)
    {
        this.ucList.IsExport = true;
        //this.ucList.ShowReports(sender);
        this.ucList.InitPageParameter(sender);
        this.ucList.Visible = true;
    }

}

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
using com.Sconit.Entity;
using NHibernate.Expression;

public partial class Production_PLFeedSeqInfo_Main : MainModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucSearch.SearchEvent += new System.EventHandler(this.Search_Render);
        this.ucSearch.ExportEvent += new EventHandler(ucSearch_ExportEvent);
        this.ucList.EditEvent += new System.EventHandler(this.ListEdit_Render);

        if (!IsPostBack)
        {
            if (this.Action == BusinessConstants.PAGE_LIST_ACTION)
            {
                ucSearch.QuickSearch(this.ActionParameter);
            }
        }
    }

    //The event handler when user click button "Search" button
    void Search_Render(object sender, EventArgs e)
    {
        this.ucList.SetSearchCriteria((DetachedCriteria)((object[])sender)[0], (DetachedCriteria)((object[])sender)[1]);
        this.ucList.Visible = true;
        this.ucList.UpdateView();
        this.CleanMessage();
    }

   

    //The event handler when user click link "Edit" link of ucList
    void ListEdit_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = false;
        this.ucList.Visible = false;
    }


    void ucSearch_ExportEvent(object sender, EventArgs e)
    {

        this.ucList.SetSearchCriteria((DetachedCriteria)((object[])sender)[0], (DetachedCriteria)((object[])sender)[1]);
        this.ucList.Visible = true;
        this.ucList.UpdateView();
        this.CleanMessage();
        this.ucList.Export();
    }


    protected void ucSearch_BtnImportClick(object sender, EventArgs e)
    {
        this.ucSearch.Visible = false;
        this.ucList.Visible = false;
    }
}

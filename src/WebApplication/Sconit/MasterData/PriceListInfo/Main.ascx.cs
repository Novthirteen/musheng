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

public partial class MasterData_PriceList_PriceList_Main : MainModuleBase
{
    public string PriceListType
    {
        get
        {
            return (string)ViewState["PriceListType"];
        }
        set
        {
            ViewState["PriceListType"] = value;
        }
    }
    public string PriceListCode
    {
        get
        {
            return (string)ViewState["PriceListCode"];
        }
        set
        {
            ViewState["PriceListCode"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucSearch.SearchEvent += new System.EventHandler(this.Search_Render);
        this.ucSearch.NewEvent += new System.EventHandler(this.New_Render);
        this.ucSearch.SearchEvent1 += new System.EventHandler(this.Search1_Render);
        this.ucList.EditEvent += new System.EventHandler(this.ListEdit_Render);
        this.ucEdit.BackEvent += new System.EventHandler(this.EditBack_Render);

        if (!IsPostBack)
        {
            if (this.ModuleParameter.ContainsKey("PriceListType"))
            {
                //string priceListType = BusinessConstants.CODE_MASTER_PRICE_LIST_TYPE_VALUE_PURCHASE;
                this.PriceListType = this.ModuleParameter["PriceListType"];
                //this.ucNew.PriceListType = priceListType;
                //this.ucSearch.PriceListType = priceListType;
                //this.ucEdit.PriceListType = priceListType;
                //this.ucList.PriceListType = priceListType;
            }
            if (this.Action == BusinessConstants.PAGE_LIST_ACTION)
            {
                ucSearch.QuickSearch(this.ActionParameter);
            }
            if (this.Action == BusinessConstants.PAGE_EDIT_ACTION)
            {
                ListEdit_Render(this.ActionParameter["Code"], null);
            }
            
        }
        //string priceListType = BusinessConstants.CODE_MASTER_PRICE_LIST_TYPE_VALUE_CUSTOMERGOODS;
        this.ucSearch.PriceListType = this.PriceListType;
        this.ucEdit.PriceListType = this.PriceListType;
        this.ucList.PriceListType = this.PriceListType;
    }

    //The event handler when user click button "Search" button
    void Search_Render(object sender, EventArgs e)
    {
        this.ucList.SetSearchCriteria((DetachedCriteria)((object[])sender)[0], (DetachedCriteria)((object[])sender)[1]);
        this.ucList.Visible = true;
        this.ucList.UpdateView();
    }

    void Search1_Render(object sender, EventArgs e)
    {
        this.ucList.Visible = false;
    }

    //The event handler when user click button "New" button
    void New_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = false;
        this.ucList.Visible = false;
    }

    //The event handler when user click button "Back" button of ucNew
    void NewBack_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = true;
        this.ucList.UpdateView();
    }

    //The event handler when user click button "Save" button of ucNew
    void CreateBack_Render(object sender, EventArgs e)
    {
        this.ucEdit.Visible = true;
        this.ucList.Visible = false;
        this.ucEdit.InitPageParameter((string)sender);
        this.PriceListCode = (string)sender;
    }

    //The event handler when user click link "Edit" link of ucList
    void ListEdit_Render(object sender, EventArgs e)
    {
        this.ucEdit.Visible = true;
        this.ucSearch.Visible = false;
        this.ucList.Visible = false;
        this.ucEdit.InitPageParameter((string)sender);
        this.PriceListCode = (string)sender;
    }

    //The event handler when user click button "Back" button of ucEdit
    void EditBack_Render(object sender, EventArgs e)
    {
        this.ucEdit.Visible = false;
        this.ucSearch.Visible = true;
        this.ucList.Visible = true;
        this.ucList.UpdateView();
    }
}

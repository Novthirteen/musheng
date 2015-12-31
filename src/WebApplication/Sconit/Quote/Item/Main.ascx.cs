using NHibernate.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;

public partial class Quote_Item_Main : MainModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucSearch.SearchEvent += new EventHandler(this.Search_Render);
        this.ucList.EditEvent += new EventHandler(this.ListEdit_Render);
        this.ucNew.BackEvent += new EventHandler(this.NewBack_Render);
        this.ucEdit.BackEvent += new EventHandler(this.EditBack_Render);
        this.ucSearch.NewEvent += new EventHandler(this.New_Render);
        this.ucNewSearch.SearchEvent += new EventHandler(this.NewSearch_Render);
        this.ucNewSearch.ItemSearchEvent += new EventHandler(this.ItemSearch_Render);

        this.ucTabNavigator.lbBomClickEvent += new System.EventHandler(this.TabBomClick_Render);
        this.ucTabNavigator.lbPriceClickEvent += new System.EventHandler(this.TabPriceClick_Render);
        this.ucNewSearch.SaveEvent += new System.EventHandler(this.SearchSave_Render);

        if (!IsPostBack)
        {
            this.ucNewSearch.Visible = true;
            this.ucNewList.Visible = false;
            this.ucPriceList.Visible = false;
        }
    }

    void Search_Render(object sender, EventArgs e)
    {
        this.ucList.SetSearchCriteria((DetachedCriteria)((object[])sender)[0], (DetachedCriteria)((object[])sender)[1]);

        this.ucList.Visible = true;

        this.ucNew.Visible = false;

        this.ucEdit.Visible = false;

        this.ucList.UpdateView();
    }

    void ListEdit_Render(object sender, EventArgs e)
    {
        string id = (string)sender;

        this.ucEdit.Visible = true;

        this.ucSearch.Visible = false;

        this.ucList.Visible = false;

        this.ucNew.Visible = false;

        this.ucEdit.InitPageParameter(id);
    }

    void NewBack_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = true;
        this.ucList.Visible = true;
        this.ucEdit.Visible = false;
        this.ucNew.Visible = false;
    }

    void EditBack_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = true;
        this.ucList.Visible = true;
        this.ucEdit.Visible = false;
        this.ucNew.Visible = false;
    }

    void New_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = false;
        this.ucList.Visible = false;
        //this.ucNew.Visible = true;
        this.ucEdit.Visible = false;
        //this.ucNewList.Visible = true;
        this.ucNewSearch.Visible = true;
    }

    void NewSearch_Render(object sender, EventArgs e)
    {
        string pid = (string)sender;
        this.ucNewList.Visible = true;
        this.ucNewList.InitPageParameter(pid);
    }

    void ItemSearch_Render(object sender, EventArgs e)
    {
        string pid = (string)sender;
        string[] para = pid.Split(',');
        this.ucNewList.Visible = true;
        this.ucNewList.InitPageParameter(para[0],para[1]);
    }

    void TabBomClick_Render(object sender, EventArgs e)
    {
        this.ucNewList.PriceBack(this.ucPriceList.GetQuoteItem());
        this.ucNewSearch.Visible = true;
        this.ucNewList.Visible = true;
        this.ucPriceList.Visible = false;
    }

    void TabPriceClick_Render(object sender, EventArgs e)
    {
        this.ucPriceList.InitPageParameter(ucNewList.GetQIList());
        this.ucNewSearch.Visible = false;
        this.ucNewList.Visible = false;
        this.ucPriceList.Visible = true;
    }

    void SearchSave_Render(object sender, EventArgs e)
    {
        this.ucNewList.SaveData();
    }
}
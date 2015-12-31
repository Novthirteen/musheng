using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using NHibernate.Expression;

public partial class Quote_ProductInfo_Main : MainModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucSearch.NewEvent += new System.EventHandler(this.New_Render);
        this.ucNew.BackEvent += new System.EventHandler(this.Back_Render);
        this.ucSearch.SearchEvent += new EventHandler(this.Search_Render);
        this.ucList.EditEvent += new EventHandler(this.ListEdit_Render);
        this.ucEdit.BackEvent += new System.EventHandler(this.EditBack_Render);
    }

    protected void New_Render(object sender, EventArgs e)
    {
        this.ucNew.Visible = true;
        this.ucSearch.Visible = false;
        this.ucList.Visible = false;
        this.ucEdit.Visible = false;
    }

    protected void Back_Render(object sender, EventArgs e)
    {
        this.ucNew.Visible = false;
        this.ucSearch.Visible = true;
        this.ucList.Visible = true;
        this.ucEdit.Visible = false;
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

    void EditBack_Render(object sender, EventArgs e)
    {
        this.ucEdit.Visible = false;
        this.ucNew.Visible = false;
        this.ucList.Visible = true;
        this.ucSearch.Visible = true;
    }
}
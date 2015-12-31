using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;

public partial class Quote_Customer_Main : MainModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucSearch.SearchEvent += new EventHandler(this.Search_Render);
        this.ucList.EditEvent += new EventHandler(this.ListEdit_Render);
        this.ucEdit.BackEvent += new EventHandler(this.EditBack_Render);
        this.ucNew.BackEvent += new EventHandler(this.NewBack_Render);
        this.ucSearch.NewEvent += new EventHandler(this.SearchNew_Render);
    }

    void Search_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = true;
        this.ucNew.Visible = false;
        this.ucList.Visible = true;
        this.ucEdit.Visible = false;
        this.ucList.InitPageParameter(sender);
    }

    void ListEdit_Render(object sender, EventArgs e)
    {
        string id = (string)sender;
        this.ucSearch.Visible = false;
        this.ucNew.Visible = false;
        this.ucList.Visible = false;
        this.ucEdit.Visible = true;
        this.ucEdit.InitPageParameter(id);
    }

    void EditBack_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = true;
        this.ucNew.Visible = false;
        this.ucList.Visible = true;
        this.ucEdit.Visible = false;
    }

    void NewBack_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = true;
        this.ucNew.Visible = false;
        this.ucList.Visible = true;
        this.ucEdit.Visible = false;
    }

    void SearchNew_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = false;
        this.ucNew.Visible = true;
        this.ucList.Visible = false;
        this.ucEdit.Visible = false;
    }
}
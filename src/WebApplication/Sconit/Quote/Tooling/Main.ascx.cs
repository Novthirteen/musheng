using com.Sconit.Web;
using NHibernate.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Quote_Tooling_Main : MainModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucSearch.NewEvent += new System.EventHandler(this.New_Render);

        this.ucNew.NewBackEvent += new System.EventHandler(this.NewBack_Render);

        this.ucEdit.EditBackEvent += new System.EventHandler(this.EditBack_Render);

        this.ucSearch.SearchEvent += new EventHandler(this.Search_Render);

        this.ucList.EditEvent += new EventHandler(this.ListEdit_Render);
    }

    void New_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = false;

        this.ucNew.Visible = true;

        this.ucEdit.Visible = false;

        this.ucList.Visible = false;
    }

    void NewBack_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = true;

        this.ucNew.Visible = false;

        this.ucEdit.Visible = false;

        this.ucList.Visible = true;
    }

    void EditBack_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = true;

        this.ucNew.Visible = false;

        this.ucEdit.Visible = false;

        this.ucList.Visible = true;
    }

    void Search_Render(object sender, EventArgs e)
    {
        this.ucList.SetSearchCriteria((DetachedCriteria)((object[])sender)[0], (DetachedCriteria)((object[])sender)[1]);

        this.ucList.Visible = true;

        this.ucList.UpdateView();
    }

    void ListEdit_Render(object sender, EventArgs e)
    {
        string tlNo = (string)sender;

        this.ucEdit.Visible = true;

        this.ucSearch.Visible = false;

        this.ucList.Visible = false;

        this.ucEdit.InitPageParameter(tlNo);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using NHibernate.Expression;
using com.Sconit.Entity.Quote;

public partial class Quote_Template_Main : MainModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucSearch.SearchEvent += new EventHandler(this.Search_Render);
        this.ucSearch.NewEvent += new System.EventHandler(this.New_Render);
        this.ucNew.BackEvent += new System.EventHandler(this.Back_Render);
        this.ucList.EditEvent += new EventHandler(this.ListEdit_Render);
        this.ucEdit.BackEvent += new System.EventHandler(this.EditBack_Render);
        this.ucEdit.SearchEvent += new System.EventHandler(this.ListSearch_Render);
        this.ucListCostList.EditEvent += new System.EventHandler(this.EditCostList_Render);
        this.ucEditCostList.BackEvent += new System.EventHandler(this.BackCostList_Render);
        this.ucEdit.NewEvent += new System.EventHandler(this.NewCostList_Render);
        this.ucNewCostList.BackEvent += new System.EventHandler(this.BackNewCostList_Render);
    }

    void Search_Render(object sender, EventArgs e)
    {
        this.ucList.SetSearchCriteria((DetachedCriteria)((object[])sender)[0], (DetachedCriteria)((object[])sender)[1]);

        this.ucList.Visible = true;

        this.ucNew.Visible = false;

        this.ucEdit.Visible = false;

        this.ucList.UpdateView();
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
        this.ucList.UpdateView();
        this.ucEdit.Visible = false;
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
        this.ucListCostList.Visible = false;
    }

    void ListSearch_Render(object sender, EventArgs e)
    {
        this.ucListCostList.SetSearchCriteria((DetachedCriteria)((object[])sender)[0], (DetachedCriteria)((object[])sender)[1]);

        this.ucListCostList.Visible = true;

        this.ucNew.Visible = false;

        this.ucEdit.Visible = true;

        this.ucListCostList.UpdateView();
    }

    void EditCostList_Render(object sender, EventArgs e)
    {
        string id = (string)sender;
        IList<CostList> cl = TheToolingMgr.GetCostListById(id);
        if(cl.Count>0)
        {
            this.ucEditCostList.Name = cl[0].CCId.Name;
        }
        this.ucEdit.Visible = false;
        this.ucListCostList.Visible = false;
        this.ucEditCostList.Visible = true;

        this.ucEditCostList.InitPageParameter(cl[0]);
    }

    void BackCostList_Render(object sender, EventArgs e)
    {
        this.ucEditCostList.Visible = false;
        this.ucListCostList.Visible = true;
        this.ucEdit.Visible = true;
    }

    void NewCostList_Render(object sender, EventArgs e)
    {
        string id = (string)sender;
        this.ucEdit.Visible = false;
        this.ucListCostList.Visible = false;
        this.ucNewCostList.Visible = true;
        this.ucNewCostList.InitPageParameter(id);
    }

    void BackNewCostList_Render(object sender, EventArgs e)
    {
        this.ucNewCostList.Visible = false;
        this.ucEdit.Visible = true;
        this.ucListCostList.Visible = true;
        this.ucListCostList.UpdateView();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using NHibernate.Expression;
using com.Sconit.Entity.Quote;

public partial class Quote_Quotes_Main : MainModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucSearch.SearchEvent += new EventHandler(this.Search_Render);
        this.ucList.EditEvent += new EventHandler(this.Edit_Render);
        this.ucProjectSearch.SearchEvent += new EventHandler(this.ProjectSearch_Render);
        this.ucProjectSearch.NewEvent += new EventHandler(this.ProjectNew_Render);
        this.ucProjectList.EditEvent += new EventHandler(this.ProjectListEdit_Render);
        this.ucProjectList.ViewEvent += new EventHandler(this.ProjectListView_Render);
    }

    void Search_Render(object sender, EventArgs e)
    {
        this.ucList.SetSearchCriteria((DetachedCriteria)((object[])sender)[0], (DetachedCriteria)((object[])sender)[1]);

        this.ucList.Visible = true;

        this.ucEdit.Visible = false;

        this.ucList.UpdateView();
    }

    void Edit_Render(object sender, EventArgs e)
    {
        string id = (string)sender;
        #region
        IList<ProductInfo> PList = TheToolingMgr.GetProductInfoById(id);
        if(PList.Count>0)
        {
            if (PList[0].Status == "Create" || PList[0].Status == "Submit")
            {
                this.ucEdit.Visible = true;
                this.ucEdit.InitPageParameter(id, PList[0].Status);
            }
            else if(PList[0].Status == "Complete")
            {
                this.ucSubmitView.Visible = true;
                this.ucSubmitView.InitPageParameter(id);
            }
        }
        #endregion
        this.ucList.Visible = false;
        this.ucSearch.Visible = false;
    }

    void ProjectSearch_Render(object sender, EventArgs e)
    {
        this.ucProjectList.SetSearchCriteria((DetachedCriteria)((object[])sender)[0], (DetachedCriteria)((object[])sender)[1]);
        this.ucSearch.Visible = false;
        this.ucList.Visible = false;
        this.ucSubmitView.Visible = false;
        this.ucProjectList.Visible = true;
        this.ucEdit.Visible = false;
        this.ucProjectList.UpdateView();
    }

    void ProjectNew_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = true;
        this.ucList.Visible = false;
        this.ucSubmitView.Visible = false;
        this.ucProjectList.Visible = false;
        this.ucEdit.Visible = false;
        this.ucProjectSearch.Visible = false;
    }

    void ProjectListEdit_Render(object sender, EventArgs e)
    {
        string id = (string)sender;
        this.ucSubmitView.InitPageParameter(id);

        this.ucSearch.Visible = false;
        this.ucList.Visible = false;
        this.ucSubmitView.Visible = true;
        this.ucProjectList.Visible = false;
        this.ucEdit.Visible = false;
        this.ucProjectSearch.Visible = false;
    }

    void ProjectListView_Render(object sender, EventArgs e)
    {
        string id = (string)sender;
        this.ucView.InitPageParameter(id);

        this.ucView.Visible = true;
        this.ucSearch.Visible = false;
        this.ucList.Visible = false;
        this.ucSubmitView.Visible = false;
        this.ucProjectList.Visible = false;
        this.ucEdit.Visible = false;
        this.ucProjectSearch.Visible = false;
    }
}
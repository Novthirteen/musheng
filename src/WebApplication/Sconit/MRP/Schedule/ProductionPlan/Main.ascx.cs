using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;

public partial class MRP_Schedule_ProductionPlan_Main : MainModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucSearch.NewEvent += new System.EventHandler(this.New_Render);
        this.ucNew.BackEvent += new System.EventHandler(this.SearchBack_Render);

        this.ucList.BackEvent += new System.EventHandler(this.SearchBack_Render);

        this.ucSearch.FirstNewEvent += new System.EventHandler(this.FirstNew_Render);
    }

    public bool IsScrap
    {
        get
        {
            return (bool)ViewState["IsScrap"];
        }
        set
        {
            ViewState["IsScrap"] = value;
        }
    }

    void New_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = true;

        this.ucNew.Visible = true;

        this.ucNew.InitPageParameter((string)sender);
    }

    void SearchBack_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = true;
        this.ucNew.Visible = false;
        this.ucList.Visible = false;
    }

    void FirstNew_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = false;

        this.ucNew.Visible = true;

        this.ucList.Visible = false;

        this.ucNew.InitPageParameter((string)sender);
    }
}
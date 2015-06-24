using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;

public partial class MasterData_ItemPoint_Main : MainModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucSearch.NewEvent += new System.EventHandler(this.New_Render);
        this.ucNew.BackEvent += new System.EventHandler(this.SearchBack_Render);
        this.ucSearch.EditEvent += new System.EventHandler(this.ListEdit_Render);
        this.ucEdit.BackEvent += new System.EventHandler(this.EditBack_Render);
    }

    void New_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = false;
        this.ucNew.Visible = true;
        this.ucEdit.Visible = false;
    }

    void SearchBack_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = true;
        this.ucNew.Visible = false;
        this.ucEdit.Visible = false;
    }

    void ListEdit_Render(object sender, EventArgs e)
    {
        this.ucEdit.Visible = true;
        this.ucSearch.Visible = false;
        this.ucEdit.InitPageParameter((string)sender);
    }

    void EditBack_Render(object sender, EventArgs e)
    {
        this.ucEdit.Visible = false;
        this.ucSearch.Visible = true;
        this.ucSearch.UpdateView();
    }
}
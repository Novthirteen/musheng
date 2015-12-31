using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;

public partial class Quote_CusTemplate_Main : MainModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucSearch.NewEvent += new EventHandler(this.New_Render);
    }
    void New_Render(object sender, EventArgs e)
    {
        string id = (string)sender;
        this.ucSearch.Visible = false;
        this.ucNew.Visible = true;
        this.ucNew.InitPageParameter(id);
    }
}
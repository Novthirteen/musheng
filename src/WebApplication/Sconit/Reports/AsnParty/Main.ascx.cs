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
using NHibernate.Expression;
using com.Sconit.Entity.Distribution;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;

public partial class Order_GoodsReceipt_AsnReceipt_Main : MainModuleBase
{
   
  
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucSearch.SearchEvent += new System.EventHandler(this.Search_Render);
     
        this.ucList.ViewEvent += new System.EventHandler(this.ListView_Render);
        //this.ucList.CloseEvent += new System.EventHandler(this.ListClose_Render);
      
        this.ucViewMain.CloseEvent += new System.EventHandler(this.Back_Render);

        if (!IsPostBack)
        {
            
            if (this.ModuleParameter != null && this.ModuleParameter.ContainsKey("IsSupplier"))
            {
                this.ucSearch.IsSupplier = bool.Parse(this.ModuleParameter["IsSupplier"]);
                this.ucList.IsSupplier = this.ucSearch.IsSupplier;
            }
           
          
        }
    }

    //The event handler when user click button "Search" button
    void Search_Render(object sender, EventArgs e)
    {
        this.ucList.SetSearchCriteria((DetachedCriteria)((object[])sender)[0], (DetachedCriteria)((object[])sender)[1]);
        this.ucList.Visible = true;
        this.ucList.IsExport = (bool)((object[])sender)[2];
        this.ucList.IsGroup = (bool)((object[])sender)[3];
        this.ucList.UpdateView();
    }

  

    void ListView_Render(object sender, EventArgs e)
    {
        this.ucViewMain.Visible = true;
        this.ucViewMain.InitPageParameter((string)sender);
    }

    void ListClose_Render(object sender, EventArgs e)
    {
        this.ucViewMain.Visible = true;
        this.ucViewMain.InitPageParameter((string)sender,"Edit");
    }


    void RefreshList_Render(object sender, EventArgs e)
    {
       
        this.ucSearch.Visible = true;
        this.ucList.Visible = true;
        this.ucList.UpdateView();
    }

    void Back_Render(object sender, EventArgs e)
    {
       
        this.ucSearch.Visible = true;
        this.ucList.Visible = true;
        this.ucSearch.QuickSearch(this.ActionParameter);
    }
}

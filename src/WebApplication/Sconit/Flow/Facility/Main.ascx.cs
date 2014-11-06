using System;
using System.Collections;
using System.Collections.Generic;
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
using com.Sconit.Entity.MasterData;
using com.Sconit.Web;
using NHibernate.Expression;
using com.Sconit.Entity;

public partial class MasterData_Facility_Main : MainModuleBase
{
    public event EventHandler BackEvent;

    public string ModuleType
    {
        get
        {
            return (string)ViewState["ModuleType"];
        }
        set
        {
            ViewState["ModuleType"] = value;
        }
    }

    public string FlowCode
    {
        get
        {
            return (string)ViewState["FlowCode"];
        }
        set
        {
            ViewState["FlowCode"] = value;
        }
    }

    public void InitPageParameter()
    {
        this.ucSearch.Visible = true;
        this.ucList.Visible = true;
        this.ucNew.Visible = false;
        this.ucEdit.Visible = false;

        IDictionary<string, string> mpDic = new Dictionary<string, string>();
        mpDic.Add("FlowCode", this.FlowCode);
        this.ucSearch.QuickSearch(mpDic);
    }

    public MasterData_Facility_Main()
    {
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        
        this.ucSearch.SearchEvent += new System.EventHandler(this.Search_Render);
        this.ucSearch.NewEvent += new System.EventHandler(this.New_Render);
        this.ucSearch.BackEvent += new System.EventHandler(this.BackFlow_Render);
        this.ucNew.BackEvent += new System.EventHandler(this.Back_Render);
        this.ucNew.EditEvent += new System.EventHandler(this.Edit_Render);
        this.ucList.ListEditEvent += new System.EventHandler(this.ListEdit_Render);
        this.ucList.ListViewEvent += new System.EventHandler(this.View_Render);
        this.ucEdit.BackEvent += new System.EventHandler(this.Back_Render);
        this.ucEdit.EditEvent += new System.EventHandler(this.Edit_Render);
        this.ucView.BackEvent += new System.EventHandler(this.Back_Render);
        
        this.ucNew.FlowCode = this.FlowCode;
        this.ucSearch.FlowCode = this.FlowCode;
        this.ucEdit.FlowCode = this.FlowCode;
        this.ucList.FlowCode = this.FlowCode;
        this.ucView.FlowCode = this.FlowCode;

        if (!IsPostBack)
        {
            this.ucSearch.ModuleType = this.ModuleType;
            this.ucList.ModuleType = this.ModuleType;
            this.ucNew.ModuleType = this.ModuleType;
            this.ucEdit.ModuleType = this.ModuleType;
            this.ucView.ModuleType = this.ModuleType;
            
            if (this.Action == BusinessConstants.PAGE_LIST_ACTION)
            {
                InitPageParameter();
            }
            if (this.Action == BusinessConstants.PAGE_NEW_ACTION)
            {
                New_Render(this, null);
            }
            if (this.Action == BusinessConstants.PAGE_EDIT_ACTION)
            {
                ListEdit_Render(this.ActionParameter["Code"], null);
            }
        }
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }

    void BackFlow_Render(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    //The event handler when user click button "Search" button
    void Search_Render(object sender, EventArgs e)
    {
        this.ucList.Visible = true;
        this.ucList.FlowCode = this.FlowCode;
        this.ucList.SetSearchCriteria((DetachedCriteria)((object[])sender)[0], (DetachedCriteria)((object[])sender)[1]);
        this.ucList.UpdateView();
    }

    //The event handler when user click button "New" button
    void New_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = true;
        this.ucList.Visible = true;
        this.ucNew.Visible = true;
        this.ucEdit.Visible = false;
        this.ucView.Visible = false;
        this.ucNew.InitPageParameter(this.FlowCode);
    }

    //The event handler when user click button "Back" button
    void Back_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = true;
        this.ucList.FlowCode = this.FlowCode;
        this.ucList.Visible = true;
        this.ucList.UpdateView();
        this.ucNew.Visible = false;
        this.ucEdit.Visible = false;
        this.ucView.Visible = false;
    }

    //The event handler when user save itemflowdetail
    void Edit_Render(object sender, EventArgs e)
    {
        InitPageParameter();
    }

    //The event handler when user click list edit
    void ListEdit_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = true;
        this.ucList.Visible = true;
        this.ucNew.Visible = false;
        this.ucEdit.Visible = true;
        this.ucView.Visible = false;
        this.ucEdit.FlowCode = this.FlowCode;
        this.ucEdit.InitPageParameter(Int32.Parse((string)sender));
    }

    //The event handler when user view itemflowdetail
    void View_Render(object sender, EventArgs e)
    {
        this.ucSearch.Visible = true;
        this.ucList.Visible = true;
        this.ucNew.Visible = false;
        this.ucEdit.Visible = false;
        this.ucView.Visible = true;
        this.ucView.FlowCode = this.FlowCode;
        this.ucView.InitPageParameter(Int32.Parse((string)sender));
    }

}

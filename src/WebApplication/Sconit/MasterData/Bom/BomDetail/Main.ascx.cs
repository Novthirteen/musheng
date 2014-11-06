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
using com.Sconit.Entity;
using NHibernate.Expression;
using System.Collections.Generic;

public partial class MasterData_Bom_BomDetail_Main : MainModuleBase
{
    public string BomCode
    {
        get
        {
            return (string)ViewState["BomCode"];
        }
        set
        {
            ViewState["BomCode"] = value;
        }
    }

    public bool IsView
    {
        get
        {
            if (ViewState["IsView"] == null)
            {
                if (ViewState["IsView"] == null)
                {
                    return false;
                }
                else
                {
                    return (bool)ViewState["IsView"];
                }
            }
            else
            {
                return (bool)ViewState["IsView"];
            }
        }
        set
        {
            ViewState["IsView"] = value;
        }
    }

    public void InitPageParameter()
    {
        this.ucSearch.Visible = true;
        this.ucList.Visible = true;
        this.ucNew.Visible = false;
        this.ucEdit.Visible = false;

        IDictionary<string, string> mpDic = new Dictionary<string, string>();
        mpDic.Add("ParCode", this.BomCode);
        this.ucSearch.QuickSearch(mpDic);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucSearch.SearchEvent += new System.EventHandler(this.Search_Render);
        this.ucSearch.NewEvent += new System.EventHandler(this.New_Render);
        this.ucSearch.ExportEvent += new EventHandler(ucSearch_ExportEvent);
        this.ucList.EditEvent += new System.EventHandler(this.ListEdit_Render);
        this.ucNew.BackEvent += new System.EventHandler(this.NewBack_Render);
        this.ucNew.CreateEvent += new System.EventHandler(this.CreateBack_Render);
        this.ucEdit.BackEvent += new System.EventHandler(this.EditBack_Render);
        this.ucImport.BtnBackClick += new System.EventHandler(this.ImportBack_Render);
        if (!IsPostBack)
        {
            if (this.Action == BusinessConstants.PAGE_LIST_ACTION)
            {
                ucSearch.QuickSearch(this.ActionParameter);
            }
            if (this.Action == BusinessConstants.PAGE_NEW_ACTION)
            {
                New_Render(this, null);
            }
            if (this.Action == BusinessConstants.PAGE_EDIT_ACTION)
            {
                ListEdit_Render(this.ActionParameter["ID"], null);
            }
            if (this.ModuleParameter!=null && this.ModuleParameter.ContainsKey("IsView"))
            {
                this.IsView = bool.Parse(this.ModuleParameter["IsView"]);
            }

            this.ucSearch.IsView = this.IsView;
            this.ucEdit.IsView = this.IsView;
            this.ucList.IsView = this.IsView;
        }
    }

    void ucSearch_ExportEvent(object sender, EventArgs e)
    {

        this.ucList.SetSearchCriteria((DetachedCriteria)((object[])sender)[0], (DetachedCriteria)((object[])sender)[1]);
        this.ucList.Visible = true;
        this.ucList.UpdateView();
        this.ucList.Export();
    }

    //The event handler when user click button "Search" button
    void Search_Render(object sender, EventArgs e)
    {
        this.ucImport.Visible = false;
        this.ucList.SetSearchCriteria((DetachedCriteria)((object[])sender)[0], (DetachedCriteria)((object[])sender)[1]);
        this.ucList.Visible = true;
        this.ucList.UpdateView();
    }

    //The event handler when user click button "New" button
    void New_Render(object sender, EventArgs e)
    {
        this.ucImport.Visible = false;
        this.ucSearch.Visible = false;
        this.ucList.Visible = false;
        this.ucNew.Visible = true;
        this.ucNew.BomCode = this.BomCode;
        this.ucNew.PageCleanup();
    }

    //The event handler when user click button "Back" button of ucNew
    void NewBack_Render(object sender, EventArgs e)
    {
        this.ucImport.Visible = false;
        this.ucNew.Visible = false;
        this.ucSearch.Visible = true;
        if (this.BomCode != null && this.BomCode.Trim() != string.Empty)
        {
            this.ucList.Visible = true;
        }
    }

    //The event handler when user click button "Save" button of ucNew
    void CreateBack_Render(object sender, EventArgs e)
    {
        this.ucImport.Visible = false;
        this.ucNew.Visible = false;
        this.ucEdit.Visible = true;
        this.ucList.Visible = false;
        this.ucEdit.InitPageParameter((string)sender);
    }

    //The event handler when user click link "Edit" link of ucList
    void ListEdit_Render(object sender, EventArgs e)
    {
        this.ucImport.Visible = false;
        this.ucEdit.Visible = true;
        this.ucSearch.Visible = false;
        this.ucList.Visible = false;
        this.ucEdit.InitPageParameter((string)sender);
    }

    //The event handler when user click button "Back" button of ucEdit
    void EditBack_Render(object sender, EventArgs e)
    {
        this.ucImport.Visible = false;
        this.ucEdit.Visible = false;
        this.ucSearch.Visible = true;
        this.ucList.Visible = true;
        this.ucList.UpdateView();
    }


    //The event handler when user click button "Back" button of ucImport
    void ImportBack_Render(object sender, EventArgs e)
    {
        this.ucImport.Visible = false;
        this.ucEdit.Visible = false;
        this.ucSearch.Visible = true;
        this.ucList.Visible = true;
        this.ucList.UpdateView();
    }

    protected void ucSearch_BtnImportClick(object sender, EventArgs e)
    {
        this.ucSearch.Visible = false;
        this.ucList.Visible = false;
        this.ucNew.Visible = false;
        this.ucEdit.Visible = false;
        this.ucImport.Visible = true;
    }

    protected void ucImport_BtnBackClick(object sender, EventArgs e)
    {
        this.ucImport.Visible = false;
        this.ucSearch.Visible = true;
    }
}

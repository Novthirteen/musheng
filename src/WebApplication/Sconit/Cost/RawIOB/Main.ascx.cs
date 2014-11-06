using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;
using log4net;
using com.Sconit.Web;
using com.Sconit.Service.Ext.Cost;
using com.Sconit.Entity.Cost;
using com.Sconit.Utility;
using NHibernate.Expression;

//TODO: Add other using statements here.liqiuyun

public partial class Modules_Cost_RawIOB_Main : MainModuleBase
{
    public Modules_Cost_RawIOB_Main()
    { }
    //Get the logger
    private static ILog log = LogManager.GetLogger("Cost");

    protected void Page_Load(object sender, EventArgs e)
    {
        //TODO: Add code for Page_Load here.
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.ucNew.Back += new System.EventHandler(this.NewBack_Render);
        this.ucNew.Create += new System.EventHandler(this.CreateBack_Render);
        this.ucEdit.Back += new System.EventHandler(this.EditBack_Render);
        //TODO: Add other init code here.
    }

    public void UpdateView()
    {
        this.GV_List.Execute();
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    //The event handler when user click button "Back" button of ucNew
    void NewBack_Render(object sender, EventArgs e)
    {
        this.fldSearch.Visible = true;
        this.fldList.Visible = true;
        this.UpdateView();
        this.ucNew.Visible = false;
    }

    //The event handler when user click button "Save" button of ucNew
    void CreateBack_Render(object sender, EventArgs e)
    {
        if (sender.ToString() == "0")
        {
            this.fldSearch.Visible = true;
            this.fldList.Visible = true;
            this.UpdateView();
            this.ucEdit.Visible = false;
            this.ucNew.Visible = false;
        }
        else
        {
            this.fldSearch.Visible = false;
            this.fldList.Visible = false;
            this.ucNew.Visible = false;
            this.ucEdit.Visible = true;
            this.ucEdit.InitPageParameter(sender);
        }
    }

    //The event handler when user click button "Back" button of ucEdit
    void EditBack_Render(object sender, EventArgs e)
    {
        this.fldSearch.Visible = true;
        this.fldList.Visible = true;
        this.UpdateView();
        this.ucEdit.Visible = false;
    }

    //The event handler when user button "Search".
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch(sender);
        //TODO: Add other event handler code here.
    }

    //Do data query and binding.
    private void DoSearch(object sender)
    {
        if (this.tbFinanceCalendar.Text.Trim() == string.Empty)
        {
            ShowErrorMessage("请选择会计年月");
            return;
        }
        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(RawIOB));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(RawIOB))
        .SetProjection(Projections.ProjectionList()
        .Add(Projections.Count("Id")));
        string item = this.tbItem.Text.Trim();
        if (item != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("Item", item));
            selectCountCriteria.Add(Expression.Eq("Item", item));
        }
        string financecalendar = this.tbFinanceCalendar.Text.Trim();
        if (financecalendar != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("FinanceCalendar", financecalendar));
            selectCountCriteria.Add(Expression.Eq("FinanceCalendar", financecalendar));
        }
        if (this.cbAdjust.Checked)
        {
            selectCriteria.Add(Expression.Not(Expression.Eq("DiffAmount", 0D)));
            selectCountCriteria.Add(Expression.Not(Expression.Eq("DiffAmount", 0D)));
        }
        this.SetSearchCriteria(selectCriteria, selectCountCriteria);
        this.UpdateView();
        this.fldList.Visible = true;
        this.ucEdit.Visible = false;
        if ((Button)sender == this.btnExport)
        {
            this.ExportXLS(this.GV_List);
        }

        //TODO: Add your code to do data query and binding here.
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        this.fldSearch.Visible = false;
        this.fldList.Visible = false;
        this.ucNew.Visible = true;
        this.ucNew.PageCleanup();
        //TODO: Add othere code here.
    }

    protected void lbtnEdit_Click(object sender, EventArgs e)
    {
        string code_ = ((LinkButton)sender).CommandArgument;

        this.fldSearch.Visible = false;
        this.fldList.Visible = false;
        this.ucEdit.Visible = true;
        this.ucEdit.InitPageParameter(code_);
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        string code_ = ((LinkButton)sender).CommandArgument;
        try
        {
            //TheRawIOBMgr.DeleteRawIOB(code_);
            ShowSuccessMessage("Cost.RawIOB.DeleteRawIOB.Successfully");
            UpdateView();
        }
        catch (Castle.Facilities.NHibernateIntegration.DataException ex)
        {
            ShowErrorMessage("Cost.RawIOB.DeleteRawIOB.Failed");
        }
    }
}

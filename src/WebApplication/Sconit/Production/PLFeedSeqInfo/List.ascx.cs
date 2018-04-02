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
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.Customize;

public partial class Production_PLFeedSeqInfo_List : ListModuleBase
{
    public EventHandler EditEvent;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void Export()
    {
        this.IsExport = true;
        if (GV_List.Rows.Count > 0)
        {
            GV_List.Columns.RemoveAt(GV_List.Columns.Count - 1);
        }
        GV_List.ExportXLS();
        //this.ExportXLS(GV_List);
    }

    public override void UpdateView()
    {
        this.IsExport = false;
        this.GV_List.Execute();
    }

    protected void lbtnEdit_Click(object sender, EventArgs e)
    {
        if (EditEvent != null)
        {
            int id = int.Parse(((LinkButton)sender).CommandArgument);
            EditEvent(id, e);
        }
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        int id = int.Parse(((LinkButton)sender).CommandArgument);
        try
        {
            TheProdutLineFeedSeqenceMgr.DeleteProdutLineFeedSeqence(id);
            ShowSuccessMessage("Production.ProdutLineFeedSeqence.DeleteProdutLineFeedSeqence.Successfully");
            UpdateView();
        }
        catch (Castle.Facilities.NHibernateIntegration.DataException ex)
        {
            ShowErrorMessage("Production.ProdutLineFeedSeqence.DeleteProdutLineFeedSeqence.Fail");
        }
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ProdutLineFeedSeqence plfs = (ProdutLineFeedSeqence)e.Row.DataItem;


            //Label lblIsActive = (Label)e.Row.Cells[5].FindControl("lblIsActive");
            //if (plfs.IsActive)
            //{
            //    //e.Row.Cells[5].Text = "Y";
            //    lblIsActive.Text = "Y";
            //}
            //else
            //{
            //    //e.Row.Cells[5].Text = "N";
            //    lblIsActive.Text = "N";
            //}

            if (this.IsExport)
            {
                e.Row.Cells[2].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                e.Row.Cells[4].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                e.Row.Cells[5].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            }
        }
    }
}

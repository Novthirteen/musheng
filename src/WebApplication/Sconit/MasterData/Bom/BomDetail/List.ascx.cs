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
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;

public partial class MasterData_Bom_BomDetail_List : ListModuleBase
{
    public EventHandler EditEvent;

    public bool IsView
    {
        get
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
        set
        {
            ViewState["IsView"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public override void UpdateView()
    {
        this.IsExport = false;
        this.GV_List.Execute();
    }

    public void Export()
    {
        this.IsExport = true;

        GV_List.Columns[15].Visible = true;
        GV_List.Columns[16].Visible = true;

        GV_List.Columns.RemoveAt(17);
        GV_List.Columns.RemoveAt(4);
        //GV_List.Columns.RemoveAt(0);

        GV_List.ExportXLS("BomDetailSample.xls");

        //this.ExportXLS(GV_List);
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            BomDetail bomDetail = (BomDetail)e.Row.DataItem;
            if (this.IsExport)
            {
                //e.Row.Cells[5].Text = TheCodeMasterMgr.GetCachedCodeMaster(BusinessConstants.CODE_MASTER_BOM_DETAIL_TYPE, e.Row.Cells[5].Text.Trim()).Description;
                //e.Row.Cells[0].Text = bomDetail.Operation.ToString();
                e.Row.Cells[11].Text = bomDetail.BackFlushMethod;
                e.Row.Cells[12].Text = bomDetail.IsShipScanHu.ToString();
                e.Row.Cells[13].Text = bomDetail.NeedPrint.ToString();
            }
            else
            {
                //e.Row.Cells[7].Text = TheCodeMasterMgr.GetCachedCodeMaster(BusinessConstants.CODE_MASTER_BOM_DETAIL_TYPE, e.Row.Cells[7].Text.Trim()).Description;
            }
            if (this.IsView)
            {
                ((LinkButton)e.Row.FindControl("lbtnDelete")).Visible = false;
            }
        }
    }
    protected void lbtnEdit_Click(object sender, EventArgs e)
    {
        if (EditEvent != null)
        {
            string code = ((LinkButton)sender).CommandArgument;
            EditEvent(code, e);
        }
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        string code = ((LinkButton)sender).CommandArgument;
        try
        {
            TheBomDetailMgr.DeleteBomDetail(Convert.ToInt32(code));
            ShowSuccessMessage("Common.Business.Result.Delete.Successfully");
            UpdateView();
        }
        catch (Castle.Facilities.NHibernateIntegration.DataException ex)
        {
            ShowErrorMessage("Common.Business.Result.Delete.Failed");
        }
        catch (Exception ex)
        {
            ShowErrorMessage("MasterData.BomDetail.WarningMessage.CreateDOrder");
        }
    }
}

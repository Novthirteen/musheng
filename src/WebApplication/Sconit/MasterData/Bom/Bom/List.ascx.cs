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

public partial class MasterData_Bom_Bom_List : ListModuleBase
{
    public EventHandler EditEvent;
    public EventHandler NewEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public override void UpdateView()
    {
        this.GV_List.Execute();
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
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
            TheBomMgr.DeleteBom(code);
            ShowSuccessMessage("MasterData.Bom.Delete.Successfully", code);
            UpdateView();
        }
        catch (Castle.Facilities.NHibernateIntegration.DataException ex)
        {
            ShowErrorMessage("MasterData.Bom.Delete.Failed", code);
        }
    }

    protected void lbtnClone_Click(object sender, EventArgs e)
    {
        if (NewEvent != null)
        {
            string code = ((LinkButton)sender).CommandArgument;
            NewEvent(code, e);
        }
    }

    protected void GV_List_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GV_List.EditIndex = e.NewEditIndex;
        UpdateView();
    }
    protected void GV_List_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GV_List.EditIndex = -1;
        UpdateView();
    }

    protected void GV_List_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            Bom bom = new Bom();
            Uom uom = new Uom();
            string code = GV_List.Rows[e.RowIndex].Cells[1].Text;
            string desc = ((TextBox)(GV_List.Rows[e.RowIndex].Cells[2].Controls[0])).Text;
            string uom1 = ((TextBox)(GV_List.Rows[e.RowIndex].Cells[3].Controls[1])).Text;
            bool isactive = ((CheckBox)(GV_List.Rows[e.RowIndex].Cells[5].Controls[0])).Checked;
            bom.Code = code;
            bom.Description = desc;
            uom.Code = uom1;
            bom.Uom = uom;
            bom.IsActive = isactive;
            TheBomMgr.UpdateBom(bom);
            ShowSuccessMessage("MasterData.Bom.Update.Successfully", code);
            GV_List.EditIndex = -1;
            UpdateView();
        }
        catch
        {
            ShowErrorMessage("");
        }
    }
}

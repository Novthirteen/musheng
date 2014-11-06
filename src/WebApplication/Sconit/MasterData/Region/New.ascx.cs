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
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using com.Sconit.Web;
using com.Sconit.Entity;

public partial class MasterData_Region_New : NewModuleBase
{
    private Region region;
    public event EventHandler CreateEvent;
    public event EventHandler BackEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
        Controls_TextBox tbInspectLocation = (Controls_TextBox)this.FV_Region.FindControl("tbInspectLocation");
        Controls_TextBox tbRejectLocation = (Controls_TextBox)this.FV_Region.FindControl("tbRejectLocation");

        tbInspectLocation.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:" + BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_INSPECT + ",bool:false";
        tbInspectLocation.DataBind();

        tbRejectLocation.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:" + BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_REJECT + ",bool:false";
        tbRejectLocation.DataBind();
    }


    public void PageCleanup()
    {
        ((TextBox)(this.FV_Region.FindControl("tbCode"))).Text = string.Empty;
        ((TextBox)(this.FV_Region.FindControl("tbName"))).Text = string.Empty;
        ((CheckBox)(this.FV_Region.FindControl("cbIsActive"))).Checked = true;
    }

    protected void checkRegionExists(object source, ServerValidateEventArgs args)
    {
        string code = ((TextBox)(this.FV_Region.FindControl("tbCode"))).Text;
        if (TheRegionMgr.LoadRegion(code) != null)
        {
            args.IsValid = false;
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        CustomValidator cvInsert = ((CustomValidator)(this.FV_Region.FindControl("cvInsert")));
        if (cvInsert.IsValid)
        {
            TextBox tbCode = (TextBox)(this.FV_Region.FindControl("tbCode"));
            TextBox tbName = (TextBox)(this.FV_Region.FindControl("tbName"));
            Controls_TextBox tbInspectLocation = (Controls_TextBox)(this.FV_Region.FindControl("tbInspectLocation"));
            Controls_TextBox tbRejectLocation = (Controls_TextBox)(this.FV_Region.FindControl("tbRejectLocation"));
            Controls_TextBox tbCostCenter = (Controls_TextBox)(this.FV_Region.FindControl("tbCostCenter"));
            CheckBox cbIsActive = (CheckBox)(this.FV_Region.FindControl("cbIsActive"));
            Region region = new Region();
            region.Code = tbCode.Text.Trim();
            region.Name = tbName.Text.Trim();
            region.IsActive = cbIsActive.Checked;
            region.CostCenter = tbCostCenter.Text.Trim();
            region.CostGroup = TheCostCenterMgr.LoadCostCenter(region.CostCenter).CostGroup.Code;

            if (tbInspectLocation.Text.Trim() != string.Empty)
            {
                Location inspectLocation = TheLocationMgr.LoadLocation(tbInspectLocation.Text.Trim());
                if (inspectLocation.Region.CostGroup != region.CostGroup)
                {
                    ShowErrorMessage("MasterData.Region.InspectLocation.CostGroup.Error");
                    return;
                }
                region.InspectLocation = inspectLocation.Code;
            }
            if (tbRejectLocation.Text.Trim() != string.Empty)
            {
                Location rejectLocation = TheLocationMgr.LoadLocation(tbRejectLocation.Text.Trim());
                if (rejectLocation.Region.CostGroup != region.CostGroup)
                {
                    ShowErrorMessage("MasterData.Region.RejectLocation.CostGroup.Error");
                    return;
                }
                region.RejectLocation = rejectLocation.Code;
            }
            TheRegionMgr.CreateRegion(region, this.CurrentUser);
            ShowSuccessMessage("MasterData.Region.AddRegion.Successfully", region.Code);
            if (CreateEvent != null)
            {
                CreateEvent(region.Code, e);
               
            }
        }
    }
}

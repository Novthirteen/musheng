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
using com.Sconit.Web;
using com.Sconit.Entity;

public partial class MasterData_Region_Edit : EditModuleBase
{
    public event EventHandler BackEvent;

    protected string RegionCode
    {
        get
        {
            return (string)ViewState["RegionCode"];
        }
        set
        {
            ViewState["RegionCode"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      
    }

    public void InitPageParameter(string code)
    {
        this.RegionCode = code;
        this.ODS_Region.SelectParameters["code"].DefaultValue = this.RegionCode;
        this.ODS_Region.DeleteParameters["code"].DefaultValue = this.RegionCode;
    }

    protected void FV_Region_DataBound(object sender, EventArgs e)
    {

        Controls_TextBox tbInspectLocation = (Controls_TextBox)this.FV_Region.FindControl("tbInspectLocation");
        Controls_TextBox tbRejectLocation = (Controls_TextBox)this.FV_Region.FindControl("tbRejectLocation");


        tbInspectLocation.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:" + BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_INSPECT + ",bool:false";
        tbInspectLocation.DataBind();

        tbRejectLocation.ServiceParameter = "string:" + this.CurrentUser.Code + ",string:" + BusinessConstants.CODE_MASTER_LOCATION_TYPE_VALUE_REJECT + ",bool:false";
        tbRejectLocation.DataBind();

        Region region = (Region)((FormView)sender).DataItem;

        if (region.InspectLocation != null && region.InspectLocation != string.Empty)
        {
            ((Controls_TextBox)(this.FV_Region.FindControl("tbInspectLocation"))).Text = region.InspectLocation;
        }
        if (region.RejectLocation != null && region.RejectLocation != string.Empty)
        {
            ((Controls_TextBox)(this.FV_Region.FindControl("tbRejectLocation"))).Text = region.RejectLocation;
        }
        if (region.CostCenter != null && region.CostCenter != string.Empty)
        {
            ((Controls_TextBox)(this.FV_Region.FindControl("tbCostCenter"))).Text = region.CostCenter;
        }

       
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }


    protected void ODS_Region_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        Region region = (Region)e.InputParameters[0];

        Controls_TextBox tbInspectLocation = (Controls_TextBox)(this.FV_Region.FindControl("tbInspectLocation"));
        Controls_TextBox tbRejectLocation = (Controls_TextBox)(this.FV_Region.FindControl("tbRejectLocation"));
        region.CostCenter = ((Controls_TextBox)(this.FV_Region.FindControl("tbCostCenter"))).Text;

        region.CostGroup = TheCostCenterMgr.LoadCostCenter(region.CostCenter).CostGroup.Code;
        if (tbInspectLocation.Text.Trim() != string.Empty)
        {
            Location inspectLocation = TheLocationMgr.LoadLocation(tbInspectLocation.Text.Trim());
            if (inspectLocation.Region.CostGroup != region.CostGroup)
            {
                ShowErrorMessage("MasterData.Region.InspectLocation.CostGroup.Error");
                e.Cancel = true;
            }
            region.InspectLocation = inspectLocation.Code;
        }
        if (tbRejectLocation.Text.Trim() != string.Empty)
        {
            Location rejectLocation = TheLocationMgr.LoadLocation(tbRejectLocation.Text.Trim());
            if (rejectLocation.Region.CostGroup != region.CostGroup)
            {
                ShowErrorMessage("MasterData.Region.RejectLocation.CostGroup.Error");
                e.Cancel = true;
            }
            region.RejectLocation = rejectLocation.Code;
        }


    }

    protected void ODS_Region_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ShowSuccessMessage("MasterData.Region.UpdateRegion.Successfully", RegionCode);

    }

    protected void ODS_Region_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception == null)
        {
            btnBack_Click(this, e);
            ShowSuccessMessage("MasterData.Region.DeleteRegion.Successfully", RegionCode);
        }
        else if (e.Exception.InnerException is Castle.Facilities.NHibernateIntegration.DataException)
        {
            ShowErrorMessage("MasterData.Region.DeleteRegion.Fail", RegionCode);
            e.ExceptionHandled = true;
        }
    }

}

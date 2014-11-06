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
using com.Sconit.Service.MasterData;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity.Exception;
using com.Sconit.Utility;

public partial class Inventory_Stocktaking_Edit : EditModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public void InitPageParameter(string code)
    {
        CycleCount cycleCount = TheCycleCountMgr.LoadCycleCount(code);
        InitPageParameter(cycleCount);
    }

    public void InitPageParameter(CycleCount cycleCount)
    {
        this.tbOrderNo.Text = cycleCount.Code;
        this.tbStatus.Text = cycleCount.Status;
        this.tbRegion.Text = StringHelper.GetCodeDescriptionString(cycleCount.Location.Region.Code, cycleCount.Location.Region.Name);
        this.tbLocation.Text = StringHelper.GetCodeDescriptionString(cycleCount.Location.Code, cycleCount.Location.Name);
        this.tbEffDate.Text = cycleCount.EffectiveDate.ToString("yyyy-MM-dd");
        this.ddlType.Value = cycleCount.Type;
        this.tbLastModifyDate.Text = cycleCount.LastModifyDate.ToString("yyyy-MM-dd HH:mm");
        this.tbLastModifyUser.Text = StringHelper.GetCodeDescriptionString(cycleCount.LastModifyUser.Code, cycleCount.LastModifyUser.Name);
        this.cbIsScanHu.Checked = cycleCount.IsScanHu;
        this.ddlPhyCntGroupBy.Value = cycleCount.PhyCntGroupBy;
        this.cbIsDynamic.Checked = cycleCount.IsDynamic;


        if (cycleCount.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE
            || cycleCount.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT
            || cycleCount.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS
            || cycleCount.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_COMPLETE)
        {
            this.cbIsScanHu.Enabled = true;
        }
        else
        {
            this.cbIsScanHu.Enabled = false;
        }

    }

    protected void btnHu2Qty_Click(object sender, EventArgs e)
    {
        try
        {
            CycleCount cycleCount = TheCycleCountMgr.LoadCycleCount(this.tbOrderNo.Text.Trim());

            cycleCount.IsScanHu = this.cbIsScanHu.Checked;
            TheCycleCountMgr.UpdateCycleCount(cycleCount);

            ShowSuccessMessage("Common.Business.Result.Update.Successfully");
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage("Common.Business.Result.Update.Failed.Reason", ex.Message);
        }
    }


}

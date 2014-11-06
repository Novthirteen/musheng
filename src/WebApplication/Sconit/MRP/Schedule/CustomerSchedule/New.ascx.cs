using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using com.Sconit.Utility;
using com.Sconit.Entity.Procurement;
using com.Sconit.Entity.View;
using System.Text;
using com.Sconit.Entity.MRP;

public partial class MRP_Schedule_CustomerSchedule_New : EditModuleBase
{
    public event EventHandler backClickEvent;
    private int seq = 1;

    private DateTime? StartDate
    {
        get
        {
            if (this.tbStartDate.Text.Trim() == string.Empty)
            {
                return null;
            }
            else
            {
                return DateTime.Parse(this.tbStartDate.Text.Trim());
            }
        }
    }

    private DateTime? EndDate
    {
        get
        {
            if (this.tbEndDate.Text.Trim() == string.Empty)
            {
                return null;
            }
            else
            {
                return DateTime.Parse(this.tbEndDate.Text.Trim());
            }
        }
    }

    private CustomerSchedule customerSchedule
    {
        get { return (CustomerSchedule)ViewState["CustomerSchedule"]; }
        set { ViewState["CustomerSchedule"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.tbFlow.ServiceParameter = "string:" + this.CurrentUser.Code + ",bool:false,bool:true,bool:true,bool:false,bool:false,bool:true,string:"
            + BusinessConstants.PARTY_AUTHRIZE_OPTION_TO;
        //this.hlTemplate.NavigateUrl = "~/Reports/Templates/ImportTemplates/CustomerSchedule.xls";
    }

    protected void rblTemplateType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblTemplateType.SelectedIndex == 0)
        {
            this.tbStartDate.CssClass = string.Empty;//DistributionOrder3.xls
            this.tbEndDate.CssClass = string.Empty;
            this.hlTemplate.NavigateUrl = "~/Reports/Templates/ImportTemplates/CustomerSchedule.xls";
            this.hlTemplate.Text = "${Common.Business.Template1}";
        }
        else
        {
            this.tbStartDate.CssClass = "inputRequired";
            this.tbEndDate.CssClass = "inputRequired";
            this.hlTemplate.NavigateUrl = "~/Reports/Templates/ImportTemplates/DistributionOrder3.xls";
            this.hlTemplate.Text = "${Common.Business.Template2}";
        }
    }

    public void InitPageParameter(object customerScheduleId)
    {
        if (customerScheduleId != null)
        {
            this.customerSchedule = TheCustomerScheduleMgr.LoadCustomerSchedule(Convert.ToInt32(customerScheduleId), true);
        }
        else
        {
            this.customerSchedule = new CustomerSchedule();
            this.customerSchedule.CustomerScheduleDetails = new List<CustomerScheduleDetail>();
        }
        this.GVDataBind();
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        try
        {
            string flowCode = this.tbFlow.Text.Trim() != string.Empty ? this.tbFlow.Text.Trim() : string.Empty;

            if (true || rblTemplateType.SelectedIndex == 0)
            {
                customerSchedule = TheImportMgr.ReadCustomerScheduleFromXls(fileUpload.PostedFile.InputStream, this.CurrentUser,
                    this.StartDate, this.EndDate, flowCode, this.tbRefScheduleNo.Text.Trim(), this.cbItemRef.Checked);
            }
            else
            {
                if (!this.StartDate.HasValue || !this.EndDate.HasValue)
                {
                    ShowErrorMessage("开始时间和结束时间必填");
                    return;
                }
                //if (this.StartDate.Value.Month != this.EndDate.Value.Month)
                //{
                //    ShowErrorMessage("开始时间和结束时间不能跨月");
                //    return;
                //}

                //customerSchedule = TheImportMgr.ReadCustomerSchedulePanaFromXls(fileUpload.PostedFile.InputStream, this.CurrentUser,
                // this.StartDate.Value, this.EndDate.Value, flowCode, this.tbRefScheduleNo.Text.Trim(), this.cbItemRef.Checked);
            }
            this.GVDataBind();
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage(ex);
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            this.customerSchedule.CreateDate = DateTime.Now;
            this.customerSchedule.CreateUser = this.CurrentUser.Code;
            this.customerSchedule.LastModifyDate = DateTime.Now;
            this.customerSchedule.LastModifyUser = this.CurrentUser.Code;
            this.customerSchedule.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE;

            foreach (GridViewRow gvr in this.GV_List_Detail.Rows)
            {
                Literal ltlType = (Literal)gvr.FindControl("ltlType");
                Literal ltlStartTime = (Literal)gvr.FindControl("ltlStartTime");
                Label lblItemCode = (Label)gvr.FindControl("lblItemCode");
                Literal ltlUom = (Literal)gvr.FindControl("ltlUom");
                Literal ltlUnitCount = (Literal)gvr.FindControl("ltlUnitCount");
                Literal ltlLocation = (Literal)gvr.FindControl("ltlLocation");

                TextBox tbQty = (TextBox)gvr.FindControl("tbQty");
                decimal qty = decimal.Parse(tbQty.Text.Trim());
                foreach (CustomerScheduleDetail customerScheduleDetail in this.customerSchedule.CustomerScheduleDetails)
                {
                    if (ltlType.Text == customerScheduleDetail.Type
                        && DateTime.Parse(ltlStartTime.Text) == customerScheduleDetail.StartTime
                        && lblItemCode.Text == customerScheduleDetail.Item
                        && ltlUom.Text == customerScheduleDetail.Uom
                        && decimal.Parse(ltlUnitCount.Text) == customerScheduleDetail.UnitCount
                        && ltlLocation.Text == customerScheduleDetail.Location)
                    {
                        customerScheduleDetail.Qty = qty;
                        break;
                    }
                }
            }
            if (this.customerSchedule.Id > 0)
            {
                TheCustomerScheduleMgr.UpdateCustomerSchedule(this.customerSchedule);
                ShowSuccessMessage("MRP.Schedule.Update.CustomerSchedule.Result.Successfully");
            }
            else
            {
                TheCustomerScheduleMgr.CreateCustomerSchedule(this.customerSchedule);
                ShowSuccessMessage("MRP.Schedule.Update.CustomerSchedule.Result.Successfully");
            }
            this.GVDataBind();
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage(ex);
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (this.backClickEvent != null)
        {
            this.backClickEvent(this, e);
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            CustomerSchedule customerSchedule = TheCustomerScheduleMgr.LoadCustomerSchedule(this.customerSchedule.Id, true);
            TheCustomerScheduleMgr.DeleteCustomerSchedule(customerSchedule);
            this.backClickEvent(this, e);
            ShowSuccessMessage("MRP.Schedule.Delete.Successfully");
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage(ex);
        }
        catch (Exception)
        {
            ShowErrorMessage("MRP.Schedule.Delete.Fail");
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            TheCustomerScheduleMgr.CancelCustomerSchedule(this.customerSchedule.Id, this.CurrentUser.Code);
            this.backClickEvent(this, e);
            ShowSuccessMessage("MRP.Schedule.Cancel.Successfully");
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage(ex);
        }
        catch (Exception)
        {
            ShowErrorMessage("MRP.Schedule.Cancel.Fail");
        }
    }

    protected void btnRelease_Click(object sender, EventArgs e)
    {
        try
        {
            if (this.customerSchedule != null && this.customerSchedule.Id > 0)
            {
                TheCustomerScheduleMgr.ReleaseCustomerSchedule(this.customerSchedule.Id, this.CurrentUser.Code);
                this.customerSchedule = TheCustomerScheduleMgr.LoadCustomerSchedule(this.customerSchedule.Id, true);
                ShowSuccessMessage("MRP.Schedule.Release.Successfully");
                //this.GVDataBind();
                this.backClickEvent(this, e);
            }
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage(ex);
        }
        catch (Exception ex)
        {
            ShowErrorMessage("MRP.Schedule.Release.Fail");
        }
    }

    protected void GV_List_Detail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ((Literal)e.Row.FindControl("ltlSequence")).Text = seq.ToString();

            seq++;
            if (this.customerSchedule == null && this.customerSchedule.CustomerScheduleDetails == null
                || this.customerSchedule.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE
                || this.customerSchedule.Id == 0)
            {
                ((TextBox)(e.Row.FindControl("tbQty"))).Enabled = true;
            }
            else
            {
                ((TextBox)(e.Row.FindControl("tbQty"))).Enabled = false;
            }
        }
    }

    private void GVDataBind()
    {
        if (this.customerSchedule != null && this.customerSchedule.CustomerScheduleDetails != null)
        {
            if (this.customerSchedule.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)
            {
                this.btnDelete.Visible = true;
                this.btnRelease.Visible = true;
                this.btnSave.Visible = true;
                this.btnCancel.Visible = false;
            }
            else if (this.customerSchedule.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT)
            {
                this.btnDelete.Visible = false;
                this.btnRelease.Visible = false;
                this.btnSave.Visible = false;
                this.btnCancel.Visible = true;
            }
            else if (this.customerSchedule.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CANCEL)
            {
                this.btnDelete.Visible = false;
                this.btnRelease.Visible = false;
                this.btnSave.Visible = false;
                this.btnCancel.Visible = false;
            }
            else
            {
                this.btnDelete.Visible = false;
                this.btnRelease.Visible = false;
                this.btnSave.Visible = true;
                this.btnCancel.Visible = false;
            }
        }
        else
        {
            this.customerSchedule = new CustomerSchedule();
            this.customerSchedule.CustomerScheduleDetails = new List<CustomerScheduleDetail>();
            this.btnSave.Visible = true;

        }
        this.GV_List_Detail.DataSource = this.customerSchedule.CustomerScheduleDetails.OrderBy(c => c.StartTime).Take(500);
        this.GV_List_Detail.Visible = true;
        this.GV_List_Detail.DataBind();
        this.ltllegend.Text = customerSchedule.ReferenceScheduleNo;
        this.div_List_Detail.Visible = this.customerSchedule.CustomerScheduleDetails.Count > 0;
        if (customerSchedule.CustomerScheduleDetails.Count > 500)
        {
            ShowWarningMessage("Common.ListCount.Warning.GreatThan500");
        }
    }
}

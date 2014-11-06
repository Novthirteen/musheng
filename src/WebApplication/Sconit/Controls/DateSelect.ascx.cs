using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using com.Sconit.Web;
using com.Sconit.Utility;

public partial class Controls_DateSelect : ModuleBase
{
    public DateTime StartDate
    {
        get { return this.tbStartDate.Text == string.Empty ? DateTime.Now.AddMonths(-3) : DateTime.Parse(this.tbStartDate.Text); }
        set { this.tbStartDate.Text = value.ToString("yyyy-MM-dd"); }
    }

    public DateTime EndDate
    {
        get { return this.tbEndDate.Text == string.Empty ? DateTime.Now.AddMonths(3) : DateTime.Parse(this.tbEndDate.Text); }
        set { this.tbEndDate.Text = value.ToString("yyyy-MM-dd"); }
    }

    public string TimePeriodType
    {
        get { return this.ddlTimePeriodType.SelectedValue; }
        set { this.ddlTimePeriodType.SelectedValue = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.tbStartDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            this.tbEndDate.Text = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
            this.ddlTimePeriodType.DataSource = this.GetTimePeriodType();
            this.ddlTimePeriodType.DataBind();

            this.UpdateStartInfo();
            this.UpdateEndInfo();
        }
    }

    protected void ddlTimePeriodType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.UpdateStartInfo();
        this.UpdateEndInfo();
    }

    protected void tbStartDate_TextChanged(object sender, EventArgs e)
    {
        this.UpdateStartInfo();
    }

    protected void tbEndDate_TextChanged(object sender, EventArgs e)
    {
        this.UpdateEndInfo();
    }

    private IList<CodeMaster> GetTimePeriodType()
    {
        IList<CodeMaster> statusGroup = new List<CodeMaster>();

        statusGroup.Add(GetTimePeriodType(BusinessConstants.CODE_MASTER_TIME_PERIOD_TYPE_VALUE_DAY));
        statusGroup.Add(GetTimePeriodType(BusinessConstants.CODE_MASTER_TIME_PERIOD_TYPE_VALUE_WEEK));
        statusGroup.Add(GetTimePeriodType(BusinessConstants.CODE_MASTER_TIME_PERIOD_TYPE_VALUE_MONTH));

        return statusGroup;
    }
    private CodeMaster GetTimePeriodType(string statusValue)
    {
        return TheCodeMasterMgr.GetCachedCodeMaster(BusinessConstants.CODE_MASTER_TIME_PERIOD_TYPE, statusValue);
    }

    private void UpdateStartInfo()
    {
        if (this.TimePeriodType == BusinessConstants.CODE_MASTER_TIME_PERIOD_TYPE_VALUE_MONTH)
        {
            string year = this.StartDate.Year.ToString();
            string month = this.StartDate.Month.ToString();
            this.lblStartInfo.Text = TheLanguageMgr.TranslateMessage("MRP.MonthNo", this.CurrentUser, new string[] { year, month });
        }
        else
        {
            string year = this.StartDate.Year.ToString();
            string week = DateTimeHelper.GetWeekIndex(this.StartDate).ToString();
            this.lblStartInfo.Text = TheLanguageMgr.TranslateMessage("MRP.WeekNo", this.CurrentUser, new string[] { year, week });
        }

        this.tbStartDate.Text = DateTimeHelper.GetStartTime(this.TimePeriodType, this.StartDate).ToString("yyyy-MM-dd");
    }

    private void UpdateEndInfo()
    {
        if (this.TimePeriodType == BusinessConstants.CODE_MASTER_TIME_PERIOD_TYPE_VALUE_MONTH)
        {
            string year = this.EndDate.Year.ToString();
            string month = this.EndDate.Month.ToString();
            this.lblEndInfo.Text = TheLanguageMgr.TranslateMessage("MRP.MonthNo", this.CurrentUser, new string[] { year, month });
        }
        else
        {
            string year = this.EndDate.Year.ToString();
            string week = DateTimeHelper.GetWeekIndex(this.EndDate).ToString();
            this.lblEndInfo.Text = TheLanguageMgr.TranslateMessage("MRP.WeekNo", this.CurrentUser, new string[] { year, week });
        }

        this.tbEndDate.Text = DateTimeHelper.GetEndTime(this.TimePeriodType, this.EndDate).ToString("yyyy-MM-dd");
    }
}

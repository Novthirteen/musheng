using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;

public partial class MRP_ShiftPlan_Manual_Shift : ModuleBase
{
    public DateTime Date { get; set; }
    public string RegionCode { get; set; }

    public string ShiftCode
    {
        get { return this.ddlShift.SelectedValue; }
        set { this.ddlShift.SelectedValue = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.BindList(this.Date, this.RegionCode);
        }
    }

    public void BindList(DateTime date, string regionCode)
    {
        List<Shift> shiftList = new List<Shift>();
        shiftList.Add(new Shift());
        IList<Shift> newShiftList = TheWorkCalendarMgr.GetShiftByDate(date, regionCode, null);
        if (newShiftList != null || newShiftList.Count > 0)
        {
            shiftList.AddRange(newShiftList.OrderBy(s=>s.Code).ToList());
        }

        this.ddlShift.DataSource = shiftList;
        this.ddlShift.DataBind();
    }
}

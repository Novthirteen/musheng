using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_TimeSelect : System.Web.UI.UserControl
{
    public DateTime SelectTime
    {
        get
        {
            DateTime dateTime = DateTime.MinValue;

            dateTime.AddHours(double.Parse(this.ddlHour.SelectedValue));
            dateTime.AddMinutes(double.Parse(this.ddlMinute.SelectedValue));

            return dateTime;
        }

        set
        {
            this.ddlHour.SelectedValue = value.ToString("HH");
            this.ddlMinute.SelectedValue = value.ToString("mm");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}

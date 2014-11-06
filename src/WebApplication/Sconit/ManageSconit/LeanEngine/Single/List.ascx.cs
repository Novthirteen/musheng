using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;

public partial class ManageSconit_LeanEngine_Single_List : ListModuleBase
{
    public event EventHandler lbViewClick;

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
            Flow flow = (Flow)e.Row.DataItem;
            Label lblCountDown = (Label)e.Row.FindControl("lblCountDown");
            this.CountDown(lblCountDown, flow);
        }
    }

    protected void lbtnView_Click(object sender, EventArgs e)
    {
        string flowCode = ((LinkButton)sender).CommandArgument;
        if (lbViewClick != null)
        {
            lbViewClick(flowCode, null);
        }
    }

    private void CountDown(Label label, Flow flow)
    {
        int d = 0;
        int h = 0;
        int m = 0;

        if (flow.NextOrderTime.HasValue)
        {
            if (DateTime.Compare(flow.NextOrderTime.Value, DateTime.Now) > 0)
            {
                TimeSpan ts = flow.NextOrderTime.Value - DateTime.Now;
                d = ts.Days;
                h = ts.Hours;
                m = ts.Minutes;
            }
        }

        label.Text = d.ToString() + "D " + h.ToString() + "H " + m.ToString() + "M ";
    }
}

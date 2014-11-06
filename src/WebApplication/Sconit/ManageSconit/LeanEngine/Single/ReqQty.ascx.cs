using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Entity.Procurement;

public partial class ManageSconit_LeanEngine_Single_ReqQty : System.Web.UI.UserControl
{
    public event EventHandler lbReqQtyClick;

    #region Property
    public string Text
    {
        get
        {
            return this.lbReqQty.Text;
        }
        set
        {
            this.lbReqQty.Text = value;
        }
    }

    public List<OrderTracer> OrderTracer
    {
        get
        {
            return ViewState["OrderTracer"] != null ? (List<OrderTracer>)ViewState["OrderTracer"] : null;
        }
        set
        {
            ViewState["OrderTracer"] = value;
        }
    }

    public bool Enabled
    {
        get
        {
            return this.lbReqQty.Enabled;
        }
        set
        {
            this.lbReqQty.Enabled = value;
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void lbReqQty_Click(object sender, EventArgs e)
    {
        if (lbReqQtyClick != null)
        {
            lbReqQtyClick(this.OrderTracer, null);
        }
    }
}

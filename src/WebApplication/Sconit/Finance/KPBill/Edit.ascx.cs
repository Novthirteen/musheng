using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;

public partial class Finance_Bill_Edit : EditModuleBase
{
    public event EventHandler BackEvent;

    public decimal OrderId
    {
        get
        {
            return (decimal)ViewState["OrderId"];
        }
        set
        {
            ViewState["OrderId"] = value;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            KPOrder kpOrder = TheKPOrderMgr.LoadKPOrder(this.OrderId, true);
            IList<object> list = new List<object>();
            if (kpOrder != null)
            {
                list.Add(kpOrder);
                list.Add(kpOrder.KPItems);
            }
            string barCodeUrl = TheReportMgr.WriteToFile("Bill.xls", list);
            Page.ClientScript.RegisterStartupScript(GetType(), "method", " <script language='javascript' type='text/javascript'>PrintOrder('" + barCodeUrl + "'); </script>");

            kpOrder.ORDER_PRINT = "Y";
            kpOrder.PRINT_MODIFY_DATE = DateTime.Now;
            TheKPOrderMgr.UpdateKPOrder(kpOrder);

            this.ShowSuccessMessage("MasterData.Bill.Print.Successful");
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage(ex);
        }
    }

    public void InitPageParameter(decimal orderId)
    {
        this.OrderId = orderId;
        KPOrder kpOrder = TheKPOrderMgr.LoadKPOrder(orderId, true);
        this.tbOrderId.Text = kpOrder.QAD_ORDER_ID;
        if (kpOrder.ORDER_PUB_DATE != null)
        {
            this.tbCreateDate.Text = ((DateTime)kpOrder.ORDER_PUB_DATE).ToString("yyyy-MM-dd");
        }
        this.Gv_List.DataSource = kpOrder.KPItems;
        this.Gv_List.DataBind();

    }



    protected void Page_Load(object sender, EventArgs e)
    {

    }



    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (this.BackEvent != null)
        {
            this.BackEvent(this, e);
        }
    }



}

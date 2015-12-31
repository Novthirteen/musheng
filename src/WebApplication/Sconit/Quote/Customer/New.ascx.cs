using com.Sconit.Entity.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.Quote;

public partial class Quote_Customer_New : NewModuleBase
{
    public EventHandler BackEvent;
    protected void Page_Load(object sender, EventArgs e)
    {
        Load();
    }

    void Load()
    {
        IList<SType> STypeList = TheToolingMgr.GetQuoteSType();
        SType stype = new SType();
        stype.Name = "";
        STypeList.Add(stype);
        ddlSType.DataSource = STypeList;
        ddlSType.DataTextField = "Name";
        ddlSType.DataValueField = "Name";
        ddlSType.DataBind();
        ddlSType.SelectedValue = "";
    }

    //public void InitPageParameter(string CusCode)
    //{
    //    IList<Customer> cList = TheCustomerMgr.GetCustomer(QcCode);
    //    if (cList.Count > 0)
    //    {
    //        this.txtCustomerName.Text = cList[0].Name;
    //    }
    //}
    public void btnNew_Click(object sender, EventArgs e)
    {
        newAdd();
    }

    public void btnBack_Click(object sender, EventArgs e)
    {
        BackEvent(sender, e);
        CleanControl();
    }

    void newAdd()
    {
        QuoteCustomerInfo qc = new QuoteCustomerInfo();
        qc.Code = txtCustomerCode.Text.Trim();
        if (txtCustomerName.Text.Trim() != string.Empty)
        {
            qc.Name = txtCustomerName.Text;
        }
        if (txtBillPeriod.Text.Trim() != string.Empty)
        {
            qc.BillPeriod = Int32.Parse(txtBillPeriod.Text);
        }
        if (txtDeliveryAdd.Text.Trim() != string.Empty)
        {
            qc.DeliveryAdd = txtDeliveryAdd.Text;
        }
        if (txtDeliveryCity.Text.Trim() != string.Empty)
        {
            qc.DeliveryCity = txtDeliveryCity.Text;
        }
        if (txtEndDate.Text.Trim() != string.Empty)
        {
            qc.EndDate = DateTime.Parse(txtEndDate.Text);
        }
        if (txtM_FinanceFee.Text.Trim() != string.Empty)
        {
            qc.M_FinanceFee = txtM_FinanceFee.Text;
        }
        if (txtM_ManageFee.Text.Trim() != string.Empty)
        {
            qc.M_ManageFee = txtM_ManageFee.Text;
        }
        if (txtP_FinanceFee.Text.Trim() != string.Empty)
        {
            qc.P_FinanceFee = txtP_FinanceFee.Text;
        }
        if (txtP_LossRate.Text.Trim() != string.Empty)
        {
            qc.P_LossRate = txtP_LossRate.Text;
        }
        if (txtP_ManageFee.Text.Trim() != string.Empty)
        {
            qc.P_ManageFee = txtP_ManageFee.Text;
        }
        if (txtP_Profit.Text.Trim() != string.Empty)
        {
            qc.P_Profit = txtP_Profit.Text;
        }
        if (txtStartDate.Text.Trim() != string.Empty)
        {
            qc.StartDate = DateTime.Parse(txtStartDate.Text);
        }
        qc.SType = ddlSType.SelectedValue;

        qc.Status = ckbStatus.Checked;
        TheToolingMgr.CreateQuoteCustomerInfo(qc);
        ShowSuccessMessage("Common.Business.Result.Insert.Successfully");
    }

    public void CleanControl()
    {
        foreach (Control ctl in this.Controls)
        {
            if (ctl is TextBox)
            {
                ((TextBox)ctl).Text = string.Empty;
            }
        }
    }
}
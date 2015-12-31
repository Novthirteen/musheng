using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;

public partial class MasterData_Customer_EditQuoteFee : EditModuleBase
{
    public EventHandler BackEvent;

    public string QcId
    {
        get
        {
            return (string)ViewState["QcId"];
        }
        set
        {
            ViewState["QcId"] = value;
        }
    }

    public string QcCode
    {
        get
        {
            return (string)ViewState["QcCode"];
        }
        set
        {
            ViewState["QcCode"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public void InitPageParameter(string id)
    {
        QcId = id;
        IList<QuoteCustomerInfo> qcList = TheToolingMgr.GetQuoteCustomerInfoById(Int32.Parse(id));
        if (qcList.Count > 0)
        {
            Load(qcList[0]);
        }
    }

    void Load(QuoteCustomerInfo qc)
    {
        txtCustomerName.Text = qc.Name;
        if (qc.BillPeriod != null)
        {
            txtBillPeriod.Text = qc.BillPeriod.ToString();
        }
        txtDeliveryAdd.Text = qc.DeliveryAdd;
        txtDeliveryCity.Text = qc.DeliveryCity;
        if (qc.EndDate != null)
        {
            txtEndDate.Text = DateTime.Parse(qc.EndDate.ToString()).ToString("yyyy-MM-dd");
        }
        txtM_FinanceFee.Text = qc.M_FinanceFee;
        txtM_ManageFee.Text = qc.M_ManageFee;
        txtP_FinanceFee.Text = qc.P_FinanceFee;
        txtP_LossRate.Text = qc.P_LossRate;
        txtP_ManageFee.Text = qc.P_ManageFee;
        txtP_Profit.Text = qc.P_Profit;
        if (qc.StartDate != null)
        {
            txtStartDate.Text = DateTime.Parse(qc.StartDate.ToString()).ToString("yyyy-MM-dd");
        }
        ddlSType.DataSource = TheToolingMgr.GetQuoteSType();
        ddlSType.DataTextField = "Name";
        ddlSType.DataValueField = "Name";
        ddlSType.DataBind();
        ddlSType.SelectedValue = qc.SType;
        ckbStatus.Checked = qc.Status;
    }

    public void btnBack_Click(object sender, EventArgs e)
    {
        BackEvent(sender, e);
    }

    public void btnSave_Click(object sender, EventArgs e)
    {
        save();
    }

    void save()
    {
        QuoteCustomerInfo qc = new QuoteCustomerInfo();
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
        if (txtEndDate.Text.Trim()!=string.Empty)
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
           qc.P_FinanceFee = txtP_FinanceFee.Text ;
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
        if (txtStartDate.Text.Trim()!=string.Empty)
        {
            qc.StartDate = DateTime.Parse(txtStartDate.Text);
        }
        qc.SType = ddlSType.SelectedValue;
        
        qc.Status = ckbStatus.Checked;
        qc.Id = Int32.Parse(QcId);
        qc.Code = QcCode;
        TheToolingMgr.SaveQuoteCustomerInfo(qc);
        ShowSuccessMessage("Common.Business.Result.Save.Successfully");
    }

    public void CleanControl()
    {
        foreach(Control ctl in this.Controls)
        {
            if(ctl is TextBox)
            {
                ((TextBox)ctl).Text = string.Empty;
            }
        }
    }
}
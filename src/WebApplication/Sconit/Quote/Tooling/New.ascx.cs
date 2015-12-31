using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.Quote;
using com.Sconit.Entity.MasterData;

public partial class Quote_Tooling_New : NewModuleBase
{
    public event EventHandler NewBackEvent;
    protected void Page_Load(object sender, EventArgs e)
    {
        //LoadCustomer();
        LoadPermission();
    }

    void LoadPermission()
    {
        if (this.CurrentUser.PagePermission != null && this.CurrentUser.PagePermission.Count > 0)
        {
            foreach (Permission permission in this.CurrentUser.PagePermission)
            {
                #region 供应商
                if (permission.Code == "CustomerBStatus")
                {
                    this.dplCustomerBStatus.Enabled = true;
                }
                else if (permission.Code == "ProjectID")
                {
                    this.ckIsHavePID.Disabled = false;
                }
                #endregion

                #region 设备
                else if (permission.Code == "TL_Name")
                {
                    this.txtTLName.Enabled = true;
                }
                else if (permission.Code == "TL_Spec")
                {
                    this.txtTLSpec.Enabled = true;
                }
                else if (permission.Code == "CustomerName")
                {
                    //
                }
                else if (permission.Code == "ProductName")
                {
                    this.txtProductName.Enabled = true;
                }
                else if (permission.Code == "MSNo")
                {
                    this.txtMSNo.Enabled = true;
                }
                else if (permission.Code == "Price")
                {
                    this.txtPrice.Enabled = true;
                }
                else if (permission.Code == "Number")
                {
                    this.txtNumber.Enabled = true;
                }
                else if (permission.Code == "ApplyDate")
                {
                    this.txtApplyDate.Enabled = true;
                }
                else if (permission.Code == "ApplyUser")
                {
                    this.txtApplyUser.Enabled = true;
                }
                else if (permission.Code == "ArriveDate")
                {
                    this.txtArriveDate.Enabled = true;
                }
                else if (permission.Code == "TL_SalesType")
                {
                    this.dplTLSalesType.Enabled = true;
                }
                else if (permission.Code == "Supplier")
                {
                    this.txtSupplier.Enabled = true;
                }
                else if (permission.Code == "SupplierInNo")
                {
                    this.txtSupplierInNo.Enabled = true;
                }
                else if(permission.Code == "PCBNo")
                {
                    this.txtPCBNo.Enabled = true;
                }
                #endregion
            }
        }
    }

    #region /////
    //void LoadCustomer()
    //{
    //    IList<Customer> customerList = TheCustomerMgr.GetAllCustomer();
    //    Customer st = new Customer();
    //    customerList.Add(st);
    //    dplCustomer.DataSource = customerList;
    //    dplCustomer.DataTextField = "Name";
    //    dplCustomer.DataValueField = "Code";
    //    dplCustomer.DataBind();
    //    dplCustomer.SelectedValue = "";
    //}
    #endregion

    protected void btnNewAdd_Click(object sender, EventArgs e)
    {
        Tooling tl = TheToolingMgr.CreateTooling(SetParameter());
        ShowSuccessMessage("Quote.Tooling.CreateSuccess",tl.TL_No);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        NewBackEvent(sender, e);
    }

    Tooling SetParameter()
    {
        Tooling tl = new Tooling();
        if (txtProjectNo.Text.Trim()!=string.Empty)
        {
            tl.ProjectNo = txtProjectNo.Text.Trim();
        }

        tl.TL_No = TheNumberControlMgr.GenerateNumber("TL");

        if (txtProductName.Text.Trim() != string.Empty)
        {
            tl.ProductName = txtProductName.Text.Trim();
        }

        if (txtTLName.Text.Trim() != string.Empty)
        {
            tl.TL_Name = txtTLName.Text.Trim();
        }

        if (txtTLSpec.Text.Trim() != string.Empty)
        {
            tl.TL_Spec = txtTLSpec.Text.Trim();
        }

        if (dplCustomer.Text.Trim() != string.Empty)
        {
            tl.Customer = dplCustomer.Text.Trim();
        }

        if (txtMSNo.Text.Trim() != string.Empty)
        {
            tl.MSNo = txtMSNo.Text.Trim();
        }

        if (txtArriveDate.Text.Trim() != string.Empty)
        {
            tl.ArriveDate = DateTime.Parse(txtArriveDate.Text.Trim());
        }

        if (txtApplyDate.Text.Trim() != string.Empty)
        {
            tl.ApplyDate = DateTime.Parse(txtApplyDate.Text.Trim());
        }

        if (txtApplyUser.Text.Trim() != string.Empty)
        {
            tl.ApplyUser = txtApplyUser.Text.Trim();
        }

        if (txtSupplier.Text.Trim() != string.Empty)
        {
            tl.Suppliers = txtSupplier.Text.Trim();
        }

        if (txtPCBNo.Text.Trim() != string.Empty)
        {
            tl.PCBNo = txtPCBNo.Text.Trim();
        }

        if (txtSupplierInNo.Text.Trim() != string.Empty)
        {
            tl.SuppliersInNo = txtSupplierInNo.Text.Trim();
        }

        if (txtPrice.Text.Trim() != string.Empty)
        {
            tl.Price = decimal.Parse(txtPrice.Text.Trim());
        }

        if (txtNumber.Text.Trim() != string.Empty)
        {
            tl.Number = Int32.Parse(txtNumber.Text.Trim());
        }

        tl.TL_SalesType = dplTLSalesType.SelectedValue;

        if (dplCustomerBStatus.SelectedValue == "未结算")
        {
            tl.CustomerBStatus = false;
        }
        else if (dplCustomerBStatus.SelectedValue == "已结算")
        {
            tl.CustomerBStatus = true;
        }
        else
        {
            tl.CustomerBStatus = false;
        }
        if(txtCustomerBillDate.Text.Trim()!=string.Empty)
        {
            tl.CustomerBillDate = DateTime.Parse(txtCustomerBillDate.Text.Trim());
        }
        if(txtCustomerBillNo.Text.Trim()!=string.Empty)
        {
            tl.CustomerBillNo = txtCustomerBillNo.Text.Trim();
        }
        return tl;
    }
}
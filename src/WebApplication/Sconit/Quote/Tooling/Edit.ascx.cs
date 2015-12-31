using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.Quote;
using com.Sconit.Entity.MasterData;

public partial class Quote_Tooling_Edit : EditModuleBase
{
    public event EventHandler EditBackEvent;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            //LoadCustomer();
        }
        LoadPermission();
    }

    #region //////
    //void LoadCustomer()
    //{
    //    IList<Customer> customerList = TheCustomerMgr.GetAllCustomer();
    //    Customer st = new Customer();
    //    customerList.Add(st);
    //    dplCustomer.DataSource = customerList;
    //    dplCustomer.DataTextField = "Name";
    //    dplCustomer.DataValueField = "Code";
    //    dplCustomer.DataBind();
    //}
    #endregion

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
                    //this.txtProjectNo.Enabled = true;
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
                else if (permission.Code == "PCBNo")
                {
                    this.txtPCBNo.Enabled = true;
                }
                #endregion
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Tooling tl = TheToolingMgr.SaveTooling(Save());
        ShowSuccessMessage("Quote.Tooling.SaveSuccess", tl.TL_No);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        EditBackEvent(sender, e);
    }

    public void InitPageParameter(string tlNo)
    {
        IList<Tooling> tlList = TheToolingMgr.GetToolingByTLNo(tlNo);
        if (tlList.Count > 0)
        {
            Tooling tl = tlList[0];
            txtTLNo.Text = tl.TL_No;
            txtProjectNo.Text = tl.ProjectNo;
            txtProductName.Text = tl.ProductName;
            txtTLName.Text = tl.TL_Name;
            txtTLSpec.Text = tl.TL_Spec;
            if (tl.Customer != null)
            {
                dplCustomer.Text = tl.Customer;
            }
            else
            {
                dplCustomer.Text = "";
            }
            txtMSNo.Text = tl.MSNo;
            if (tl.ArriveDate != null)
            {
                txtArriveDate.Text = DateTime.Parse(tl.ArriveDate.ToString()).ToString("yyyy-MM-dd HH:mm");
            }
            if(tl.ApplyDate!=null)
            {
                txtApplyDate.Text = DateTime.Parse(tl.ApplyDate.ToString()).ToString("yyyy-MM-dd HH:mm");
            }
            txtApplyUser.Text = tl.ApplyUser;
            txtSupplier.Text = tl.Suppliers;
            txtPCBNo.Text = tl.PCBNo;
            txtSupplierInNo.Text = tl.SuppliersInNo;
            if (tl.Price != null)
            {
                txtPrice.Text = tl.Price.ToString();
            }
            if(tl.Number!=null)
            {
                txtNumber.Text = tl.Number.ToString();
            }
            
            dplTLSalesType.SelectedValue = tl.TL_SalesType;
            
            if(tl.CustomerBStatus)
            {
                dplCustomerBStatus.SelectedIndex = 1;
            }
            else
            {
                dplCustomerBStatus.SelectedIndex = 0;
            }
            if(tl.CustomerBillDate!=null)
            {
                txtCustomerBillDate.Text = DateTime.Parse(tl.CustomerBillDate.ToString()).ToString("yyyy-MM-dd HH:mm");
            }
            txtCustomerBillNo.Text = tl.CustomerBillNo;
        }
    }

    Tooling Save()
    {
        Tooling tl = new Tooling();

        if (txtProjectNo.Text.Trim() != string.Empty)
        {
            tl.ProjectNo = txtProjectNo.Text.Trim();
        }

        tl.TL_No = txtTLNo.Text.Trim();

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

        tl.Customer = dplCustomer.Text;

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
        if(txtCustomerBillDate.Text.Trim()!= string.Empty)
        {
            tl.CustomerBillDate = DateTime.Parse(txtCustomerBillDate.Text.Trim());
        }
        if(txtCustomerBillNo.Text.Trim()!= string.Empty)
        {
            tl.CustomerBillNo = txtCustomerBillNo.Text.Trim();
        }
        return tl;
    }
}
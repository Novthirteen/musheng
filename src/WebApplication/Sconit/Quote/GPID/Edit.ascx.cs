using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.Quote;

public partial class Quote_GPID_Edit : EditModuleBase
{
    public EventHandler BackEvent;
    protected void Page_Load(object sender, EventArgs e)
    {

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

    public void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            GPID gpid = new GPID();
            gpid.ID = this.ltID.Text;
            gpid.CustomerCode = this.txtCustomer.Text.Trim();
            gpid.Descr = this.txtDesc.Text.Trim();
            if (this.txtStartDate.Text.Trim() != string.Empty)
            {
                gpid.StartDate = DateTime.Parse(this.txtStartDate.Text.Trim());
            }
            gpid.Status = this.cbStatus.Checked;
            gpid.Product = txtProdut.Text.Trim();
            gpid.EndCustomer = txtEndCustomer.Text.Trim();
            gpid.Addr = txtAddr.Text.Trim();
            gpid.LifeCycle = txtLifeCycle.Text.Trim();
            if (txtOTS.Text.Trim() != string.Empty)
            {
                gpid.OTS = DateTime.Parse(txtOTS.Text.Trim());
            }
            if (txtPPAP.Text.Trim() != string.Empty)
            {
                gpid.PPAP = DateTime.Parse(txtPPAP.Text.Trim());
            }
            if (txtSOP.Text.Trim() != string.Empty)
            {
                gpid.SOP = DateTime.Parse(txtSOP.Text.Trim());
            }
            gpid.ProjectManager = txtProjectManager.Text.Trim();
            gpid.Technology = txtTechnology.Text.Trim();
            gpid.Buyer = txtBuyer.Text.Trim();
            gpid.Quality = txtQuality.Text.Trim();
            gpid.Desc1 = txtDesc1.Text.Trim();
            gpid.Desc2 = txtDesc2.Text.Trim();
            TheToolingMgr.UpdateGPID(gpid);
            ShowSuccessMessage("Quote.GPID.Update.Success");
        }
        catch
        {
            ShowErrorMessage("Quote.GPID.Update.Fail");
        }
    }

    public void InitPageParameter(string id)
    {
        CleanControl();
        IList<GPID> gpidList = TheToolingMgr.GetGPIDById(id);
        if (gpidList.Count > 0)
        {
            GPID gpid = gpidList[0];
            this.ltID.Text = gpid.ID;
            this.txtCustomer.Text = gpid.CustomerCode;
            this.txtDesc.Text = gpid.Descr;
            if (gpid.StartDate != null)
            {
                this.txtStartDate.Text = DateTime.Parse(gpid.StartDate.ToString()).ToString("yyyy-MM-dd");
            }
            this.cbStatus.Checked = gpid.Status;

            this.txtProdut.Text = gpid.Product;
            gpid.EndCustomer = txtEndCustomer.Text.Trim();
            gpid.Addr = txtAddr.Text.Trim();
            gpid.LifeCycle = txtLifeCycle.Text.Trim();
            if (gpid.OTS != null)
            {
                txtOTS.Text = DateTime.Parse(gpid.OTS.ToString()).ToString("yyyy-MM-dd");
            }

            if (gpid.PPAP != null)
            {
                txtPPAP.Text = DateTime.Parse(gpid.PPAP.ToString()).ToString("yyyy-MM-dd");
            }
            if (gpid.SOP != null)
            {
                txtSOP.Text = DateTime.Parse(gpid.SOP.ToString()).ToString("yyyy-MM-dd");
            }

            txtProjectManager.Text = gpid.ProjectManager;
            txtTechnology.Text = gpid.Technology;
            txtBuyer.Text = gpid.Buyer;
            txtQuality.Text = gpid.Quality;
            txtDesc1.Text = gpid.Desc1;
            txtDesc2.Text = gpid.Desc2;
        }
    }
    public void btnBack_Click(object sender, EventArgs e)
    {
        BackEvent(sender,e);
    }
}
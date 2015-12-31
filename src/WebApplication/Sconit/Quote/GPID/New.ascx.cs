using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.Quote;

public partial class Quote_GPID_New : NewModuleBase
{
    public EventHandler BackEvent;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void btnNew_Click(object sender, EventArgs e)
    {
        try
        {
            GPID gpid = new GPID();
            gpid.ID = TheNumberControlMgr.GenerateNumber("AF");
            gpid.CustomerCode = txtCustomer.Text.Trim();
            gpid.Descr = txtDesc.Text.Trim();
            if (txtStartDate.Text.Trim() != string.Empty)
            {
                gpid.StartDate = DateTime.Parse(txtStartDate.Text.Trim());
            }
            if (cbStatus.Checked)
            {
                gpid.Status = true;
            }
            else
            {
                gpid.Status = false;
            }
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
            if(txtSOP.Text.Trim()!=string.Empty)
            {
                gpid.SOP = DateTime.Parse(txtSOP.Text.Trim());
            }
            gpid.ProjectManager = txtProjectManager.Text.Trim();
            gpid.Technology = txtTechnology.Text.Trim();
            gpid.Buyer = txtBuyer.Text.Trim();
            gpid.Quality = txtQuality.Text.Trim();
            gpid.Desc1 = txtDesc1.Text.Trim();
            gpid.Desc2 = txtDesc2.Text.Trim();
            TheToolingMgr.CreateGPID(gpid);
            ShowSuccessMessage("Quote.GPID.New.Success", gpid.ID);
        }
        catch
        {
            ShowErrorMessage("Quote.GPID.New.Fail");
        }
    }

    public void btnBack_CLick(object sender, EventArgs e)
    {
        BackEvent(sender, e);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.Quote;

public partial class Quote_Template_NewCostList : NewModuleBase
{
    public EventHandler BackEvent;

    protected string CCId
    {
        get
        {
            return (string)ViewState["CCId"];
        }
        set
        {
            ViewState["CCId"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void btnNew_Click(object sender, EventArgs e)
    {
        if (txtName.Text.Trim() != string.Empty)
        {
            CostList cl = new CostList();
            CostCategory cc = new CostCategory();
            cc.Id = Int32.Parse(CCId);
            cl.CCId = cc;
            cl.Name = txtName.Text.Trim();
            cl.Number = txtNumber.Text.Trim();
            cl.Unit = txtUnit.Text.Trim();
            cl.Price = txtPrice.Text.Trim();
            TheToolingMgr.CreateCostList(cl);
            ShowSuccessMessage("Common.Business.Result.Save.Successfully");
        }
    }

    public void btnBack_Click(object sender, EventArgs e)
    {
        BackEvent(sender, e);
    }

    public void InitPageParameter(string id)
    {
        CCId = id;
        CleanPage();
    }
    public void CleanPage()
    {
        this.txtName.Text = string.Empty;
        this.txtNumber.Text = string.Empty;
        this.txtPrice.Text = string.Empty;
        this.txtUnit.Text = string.Empty;
    }
}
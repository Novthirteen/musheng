using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.Quote;

public partial class Quote_Template_EditCostList : EditModuleBase
{
    public EventHandler BackEvent;
    public string Name
    {
        get
        {
            return (string)ViewState["Name"];
        }
        set
        {
            ViewState["Name"] = value;
        }
    }

    protected string Id
    {
        get
        {
            return (string)ViewState["Id"];
        }
        set
        {
            ViewState["Id"] = value;
        }
    }

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

    public void InitPageParameter(CostList cl)
    {
        Id = cl.Id.ToString() ;
        CCId = cl.CCId.Id.ToString();
        ltlCCName.Text = Name;
        txtCostList.Text = cl.Name;
        txtNumber.Text = cl.Number;
        txtPrice.Text = cl.Price;
        txtUnit.Text = cl.Unit;
    }

    public void btnSave_Click(object sender, EventArgs e)
    {
        CostList cl = new CostList();
        cl.Id = Int32.Parse(Id);
        CostCategory cc = new CostCategory();
        cc.Id = Int32.Parse(CCId);
        cl.CCId = cc;
        cl.Name = txtCostList.Text.Trim();
        cl.Number = txtNumber.Text.Trim(); 
        cl.Unit = txtUnit.Text.Trim();
        cl.Price = txtPrice.Text.Trim();
        TheToolingMgr.UpdateCostList(cl);
        ShowSuccessMessage("Common.Business.Result.Save.Successfully");
    }

    public void btnBack_Click(object sender, EventArgs e)
    {
        BackEvent(sender, e);
    }
}

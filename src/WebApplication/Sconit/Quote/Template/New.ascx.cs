using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.Quote;

public partial class Quote_Template_New : NewModuleBase
{
    public event EventHandler BackEvent;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void btnSave_Click(object sender, EventArgs e)
    {
        if(txtCostCategory.Text.Trim()!=string.Empty)
        {
            CostCategory costCategory = new CostCategory();
            costCategory.Name = txtCostCategory.Text.Trim();
            TheToolingMgr.CreateCostCategory(costCategory);
            ShowSuccessMessage("Common.Business.Result.Save.Successfully");
        }
    }

    public void btnBack_Click(object sender, EventArgs e)
    {
        BackEvent(sender, e);
    }
}
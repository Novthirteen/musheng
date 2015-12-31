using com.Sconit.Entity.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;

public partial class MasterData_ItemPack_New : NewModuleBase
{
    public EventHandler BackEvent;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            ItemPack itemPack = new ItemPack();
            itemPack.Spec = this.txtSpec.Text.Trim();
            itemPack.Descr = this.txtDesc.Text.Trim();
            if (txtPinNum.Text.Trim() != string.Empty)
            {
                itemPack.PinNum = decimal.Parse(txtPinNum.Text.Trim());
            }
            if (txtPinConversion.Text.Trim() != string.Empty)
            {
                itemPack.PinConversion = decimal.Parse(txtPinConversion.Text.Trim());
            }
            TheToolingMgr.CreateItemPack(itemPack);
            ShowSuccessMessage("MasterData.ItemPack.Save.Success");
        }
        catch
        {
            ShowErrorMessage("MasterData.ItemPack.Save.Fail");
        }
    }

    public void btnBack_Click(object sender, EventArgs e)
    {
        BackEvent(sender, e);
    }
}
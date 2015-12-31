using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;

public partial class MasterData_ItemPack_Edit : EditModuleBase
{
    public EventHandler BackEvent;
    protected int id
    {
        get
        {
            return (int)ViewState["Id"];
        }
        set
        {
            ViewState["Id"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void InitPageParameter(int id)
    {
        CleanControl();
        IList<ItemPack> itemPackLsit = TheToolingMgr.GetItemPack(id);
        if (itemPackLsit.Count > 0)
        {
            ItemPack itemPack = itemPackLsit[0];
            this.txtSpec.Text = itemPack.Spec;
            this.txtDesc.Text = itemPack.Descr;
            if (itemPack.PinNum != null)
            {
                this.txtPinNum.Text = itemPack.PinNum.ToString();
            }
            if(itemPack.PinConversion!=null)
            {
                this.txtPinConversion.Text = itemPack.PinConversion.ToString();
            }
        }
    }

    public void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            ItemPack itemPack = new ItemPack();
            itemPack.Id = id;
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
            TheToolingMgr.UpdateItemPack(itemPack);
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
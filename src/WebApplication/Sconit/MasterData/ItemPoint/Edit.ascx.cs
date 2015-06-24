using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.Customize;
using com.Sconit.Entity.MasterData;

public partial class MasterData_ItemPoint_Edit : EditModuleBase
{
    public event EventHandler BackEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.tbFlow.ServiceParameter = "string:" + this.tbItemCode.Text;
    }

    public void InitPageParameter(string Item)
    {
        this.tbItemCode.Text = Item;
        ItemPoint ItemPoint = TheOrderProductionPlanMgr.GetItemPoint(Item)[0];
        this.tbFlow.Text = ItemPoint.Flow;
        this.txProductLineFacility.Text = ItemPoint.Fact;
        ProductLineFacility productLineFacility = TheOrderProductionPlanMgr.GetProductLineFacility(ItemPoint.Fact)[0];
        if (productLineFacility.PointTime != null && productLineFacility.PointTime.ToString() != string.Empty)
        {
            txtEquipmentTime.ReadOnly = true;
        }
        else
        {
            txtEquipmentTime.ReadOnly = false;
        }
        this.tbModel.Text = ItemPoint.Model;
        this.tbPoint.Text = ItemPoint.Point.ToString();
        this.txtPCBNum.Text = ItemPoint.PCBNumber.ToString();
        this.txtTransferTime.Text = ItemPoint.TransferTime.ToString();
        this.txtEquipmentTime.Text = ItemPoint.EquipmentTime.ToString();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txProductLineFacility.Text == string.Empty || txProductLineFacility.Text == null)
        {
            ShowWarningMessage("MasterData.ItemPoint.ChooseFact");
            return;
        }
        ItemPoint ItemPoint = new ItemPoint();
        ItemPoint.Item = tbItemCode.Text;
        ItemPoint.Flow = tbFlow.Text;
        ItemPoint.Fact = txProductLineFacility.Text;
        IList<ProductLineFacility> productLineFacility = TheOrderProductionPlanMgr.GetProductLineFacility(ItemPoint.Fact);
        ItemPoint.Model = tbModel.Text;
        if (tbPoint.Text != string.Empty && tbPoint.Text != null)
        {
            ItemPoint.Point = int.Parse(tbPoint.Text);
        }
        if (txtTransferTime.Text != string.Empty && txtTransferTime.Text != null)
        {
            ItemPoint.TransferTime = int.Parse(txtTransferTime.Text);
        }
        if (productLineFacility[0].PointTime != null && productLineFacility[0].PointTime.ToString() != string.Empty)
        {
            ItemPoint.EquipmentTime = ItemPoint.Point * productLineFacility[0].PointTime;
        }
        else
        {
            if (this.txtEquipmentTime.Text != null || this.txtEquipmentTime.Text != "")
            {
                ItemPoint.EquipmentTime = decimal.Parse(this.txtEquipmentTime.Text);
            }
        }
        if (txtPCBNum.Text != string.Empty && txtPCBNum.Text != null)
        {
            ItemPoint.PCBNumber = int.Parse(txtPCBNum.Text);
        }
        TheOrderProductionPlanMgr.UpdateItemPoint(ItemPoint);
        ShowSuccessMessage("MasterData.ItemPoint.SaveItemPoint.Successfully", ItemPoint.Item);

    }

    protected void txProductLineFacility_TextChange(object sender, EventArgs e)
    {
        IList<ProductLineFacility> productLineFacility = TheOrderProductionPlanMgr.GetProductLineFacility(txProductLineFacility.Text);
        if (productLineFacility.Count > 0)
        {
            if (productLineFacility[0].PointTime != null && productLineFacility[0].PointTime.ToString() != string.Empty)
            {
                txtEquipmentTime.ReadOnly = true;
            }
            else
            {
                txtEquipmentTime.ReadOnly = false;
            }
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }
}
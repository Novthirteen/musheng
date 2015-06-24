using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Customize;

public partial class MasterData_ItemPoint_New : ListModuleBase
{
    public event EventHandler BackEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
        //this.tbFlow.ServiceParameter = "string:" + this.CurrentUser.Code + ",bool:false,bool:false,bool:false,bool:true,bool:false,bool:false,string:" + BusinessConstants.PARTY_AUTHRIZE_OPTION_BOTH;
        this.tbFlow.ServiceParameter = "string:" + this.tbItemCode.Text;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        if (txProductLineFacility.Text == string.Empty || txProductLineFacility.Text == null)
        {
            ShowWarningMessage("MasterData.ItemPoint.ChooseFact");
            return;
        }
        IList<ItemPoint> ipt = TheOrderProductionPlanMgr.GetItemPoint(tbItemCode.Text);
        if (ipt.Count > 0)
        {
            ShowWarningMessage("MasterData.ItemPoint.Repeat");
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
        if (productLineFacility[0].PointTime != null && productLineFacility[0].PointTime.ToString()!= string.Empty)
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
        TheOrderProductionPlanMgr.CreatItemPoint(ItemPoint);
        ShowSuccessMessage("MasterData.ItemPoint.CreatSuccess");

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
}
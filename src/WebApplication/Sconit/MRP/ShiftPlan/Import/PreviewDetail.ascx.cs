using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;

public partial class MRP_ShiftPlan_Import_PreviewDetail : ModuleBase
{
    int seq = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ((Label)e.Row.FindControl("lblSequence")).Text = seq.ToString();
            seq++;
        }
    }

    public void BindList(IList<OrderDetail> orderDetailList)
    {
        int i = orderDetailList.Where(o => (o.Remark != null && o.Remark != string.Empty)).Count();
        if (i > 0)
        {
            this.GV_List.Columns[5].Visible = true;
        }
        else
        {
            this.GV_List.Columns[5].Visible = false;
        }
        this.GV_List.DataSource = orderDetailList;
        this.GV_List.DataBind();
        this.lblTotal.Text = orderDetailList.Select(o => o.OrderedQty).Sum().ToString();
    }

    public void CollectData(OrderHead orderHead)
    {
        bool isNml = true;
        bool isRtn = true;

        foreach (GridViewRow gvr in GV_List.Rows)
        {
            int flowDetailId = 0;
            HiddenField hfFlowDetailId = ((HiddenField)gvr.FindControl("hfFlowDetailId"));
            if (hfFlowDetailId.Value != string.Empty)
            {
                flowDetailId = int.Parse(hfFlowDetailId.Value);
            }
            string itemCode = ((Label)gvr.FindControl("lblItemCode")).Text;
            string bomStr = ((HiddenField)gvr.FindControl("hfBom")).Value.Trim();
            decimal reqQty = decimal.Parse(((Label)gvr.FindControl("lblOrderedQty")).Text);
            string memo = string.Empty;
            if (this.GV_List.Columns.Count == 6)
            {
                memo = gvr.Cells[5].Text.Trim() == "&nbsp;" ? null : gvr.Cells[5].Text.Trim();
            }
            memo = memo == string.Empty ? null : memo;            

            Item item = this.TheItemMgr.LoadItem(itemCode);
            if (item.Type != BusinessConstants.CODE_MASTER_ITEM_TYPE_VALUE_K)
            {
                orderHead.GetOrderDetailByFlowDetailIdAndItemCode(flowDetailId, itemCode).RequiredQty = reqQty;
                orderHead.GetOrderDetailByFlowDetailIdAndItemCode(flowDetailId, itemCode).OrderedQty = reqQty;
                orderHead.GetOrderDetailByFlowDetailIdAndItemCode(flowDetailId, itemCode).Remark = memo;
                if (bomStr != string.Empty)
                {
                    orderHead.GetOrderDetailByFlowDetailIdAndItemCode(flowDetailId, itemCode).Bom = TheBomMgr.LoadBom(bomStr);
                }
            }
            else
            {
                IList<ItemKit> kitList = this.TheItemKitMgr.GetChildItemKit(itemCode);
                if (kitList != null && kitList.Count > 0)
                {
                    foreach (ItemKit kit in kitList)
                    {
                        orderHead.GetOrderDetailByFlowDetailIdAndItemCode(flowDetailId, kit.ChildItem.Code).RequiredQty = reqQty;
                        orderHead.GetOrderDetailByFlowDetailIdAndItemCode(flowDetailId, kit.ChildItem.Code).OrderedQty = reqQty;
                        orderHead.GetOrderDetailByFlowDetailIdAndItemCode(flowDetailId, kit.ChildItem.Code).Remark = memo;
                    }
                }
            }
            //
            if (reqQty < 0)
            {
                isNml = false;
            }

            if (reqQty > 0)
            {
                isRtn = false;
            }
        }
        if (isNml)
        {
            orderHead.SubType = BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_NML;
        }
        else if (isRtn)
        {
            orderHead.SubType = BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_RTN;
        }
        else
        {
            orderHead.SubType = BusinessConstants.CODE_MASTER_ORDER_SUB_TYPE_VALUE_ADJ;
        }
    }
}

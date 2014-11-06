using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.Sconit.Web;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity;
using com.Sconit.Utility;

public partial class Inventory_InspectOrder_NewQty : ModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler CreateEvent;


    private IList<InspectItem> InspectItemList
    {
        get
        {
            return (IList<InspectItem>)ViewState["InspectItemList"];
        }
        set
        {
            ViewState["InspectItemList"] = value;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        this.tbLocation.ServiceParameter = "string:" + this.CurrentUser.Code;
        if (!IsPostBack)
        {
            this.InspectItemList = new List<InspectItem>();
        }
    }


    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        UpdateInspectItemDic();

        var i = (
                   from l in InspectItemList
                   where l.InspectQty > 0
                   select l).Count();

        if (i == 0)
        {
            ShowErrorMessage("MasterData.InspectOrder.Detail.Empty");
            return;
        }

        try
        {
            InspectOrder inspectOrder = TheInspectOrderMgr.CreateInspectOrder(this.tbLocation.Text.Trim(), this.InspectItemList, this.CurrentUser);
            inspectOrder.TextField1 = this.tbTextField1.Text.Trim();
            this.TheInspectOrderMgr.UpdateInspectOrder(inspectOrder);
            ShowSuccessMessage("MasterData.InspectOrder.Create.Successfully", inspectOrder.InspectNo);
            if (CreateEvent != null)
            {
                CreateEvent(inspectOrder.InspectNo, e);
            }
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage(ex);
        }

    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(sender, e);
        }

    }

    public void UpdateView()
    {
        this.tbLocation.Text = string.Empty;
        this.InitPageParameter();
    }

    public void InitPageParameter()
    {

        IList<InspectItem> inspectItemList = new List<InspectItem>();

        IListHelper.AddRange<InspectItem>(inspectItemList, this.InspectItemList);
        //新行
        InspectItem blankInspectItem = new InspectItem();
        blankInspectItem.IsBlank = true;
        inspectItemList.Add(blankInspectItem);

        this.GV_List.DataSource = inspectItemList;
        this.GV_List.DataBind();

    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            InspectItem inspectItem = (InspectItem)e.Row.DataItem;

            if (inspectItem.IsBlank)
            {
                e.Row.FindControl("lblItemCode").Visible = false;
                e.Row.FindControl("tbItemCode").Visible = true;
                e.Row.FindControl("lbtnAdd").Visible = true;
            }
            else
            {
                e.Row.FindControl("lblItemCode").Visible = true;
                e.Row.FindControl("tbItemCode").Visible = false;
                e.Row.FindControl("lbtnAdd").Visible = false;
                e.Row.FindControl("lbtnDel").Visible = true;
            }

        }
    }

    private void UpdateInspectItemDic()
    {
        this.InspectItemList = new List<InspectItem>();
        for (int i = 0; i < this.GV_List.Rows.Count - 1; i++)
        {
            InspectItem inspectItem = new InspectItem();
            GridViewRow row = this.GV_List.Rows[i];
            string itemCode = ((Label)row.FindControl("lblItemCode")).Text.Trim();
            TextBox tbInspectQty = (TextBox)row.FindControl("tbInspectQty");
            inspectItem.InspectQty = tbInspectQty.Text.Trim() == string.Empty ? 0 : decimal.Parse(tbInspectQty.Text.Trim());
            inspectItem.LotNo = ((TextBox)row.FindControl("tbLotNo")).Text.Trim();
            inspectItem.Item = TheItemMgr.LoadItem(itemCode);
            InspectItemList.Add(inspectItem);
        }
    }

    protected void lbtnAdd_Click(object sender, EventArgs e)
    {
        UpdateInspectItemDic();
        GridViewRow row = (GridViewRow)((LinkButton)sender).BindingContainer;
        TextBox tbInspectQty = (TextBox)row.FindControl("tbInspectQty");
        Controls_TextBox tbItem = (Controls_TextBox)row.FindControl("tbItemCode");
        TextBox tbLotNo = ((TextBox)row.FindControl("tbLotNo"));
        string itemCode = tbItem.Text.Trim();
        decimal inspectQty = tbInspectQty.Text.Trim() == string.Empty ? 0 : decimal.Parse(tbInspectQty.Text.Trim());
        if (itemCode != string.Empty)
        {
            Item item = TheItemMgr.LoadItem(itemCode);
            if (item != null)
            {
                var i = (
                         from l in InspectItemList
                         where l.Item.Code.Trim().ToUpper() == itemCode.Trim().ToUpper()
                            && l.LotNo.Trim().ToUpper() == tbLotNo.Text.Trim().ToUpper()
                         select l).Count();

                if (i > 0)
                {
                    ShowErrorMessage("MasterData.Production.Feed.Item.Exists", itemCode);
                    return;
                }

                InspectItem inspectItem = new InspectItem();
                inspectItem.Item = item;
                inspectItem.InspectQty = inspectQty;
                inspectItem.LotNo = tbLotNo.Text.Trim();
                inspectItem.IsBlank = false;

                InspectItemList.Add(inspectItem);

            }
            InitPageParameter();
        }

    }

    protected void lbtnDel_Click(object sender, EventArgs e)
    {
        UpdateInspectItemDic();
        int rowNo = Int32.Parse(((LinkButton)sender).CommandArgument);
        InspectItemList.RemoveAt(rowNo);
        InitPageParameter();
    }
}

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

public partial class Inventory_InspectOrder_New : ModuleBase
{

    public event EventHandler CreateEvent;


    private IDictionary<string, decimal> InspectItemDic
    {
        get
        {
            return (IDictionary<string, decimal>)ViewState["InspectItemDic"];
        }
        set
        {
            ViewState["InspectItemDic"] = value;
        }
    }

    private string LocationCode
    {
        get
        {
            return (string)ViewState["LocationCode"];
        }
        set
        {
            ViewState["LocationCode"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        this.tbFlow.ServiceParameter = "string:" + this.CurrentUser.Code;
        if (!IsPostBack)
        {
            InspectItemDic = new Dictionary<string, decimal>();

        }
    }


    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        UpdateInspectItemDic();

        IDictionary<string, decimal> targetInspectItemDic = new Dictionary<string, decimal>();
        if (this.InspectItemDic.Count > 0)
        {
            foreach (string fgItemCode in this.InspectItemDic.Keys)
            {
                if (InspectItemDic[fgItemCode] != 0)
                {
                    targetInspectItemDic.Add(fgItemCode.Split('$')[0], InspectItemDic[fgItemCode]);
                }
            }
        }
        if (targetInspectItemDic.Count == 0)
        {
            ShowErrorMessage("MasterData.InspectOrder.Detail.Empty");
            return;
        }

        try
        {
            InspectOrder inspectOrder = TheInspectOrderMgr.CreateFgInspectOrder(this.LocationCode, targetInspectItemDic, this.CurrentUser);
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

    protected void tbItemCode_TextChanged(object sender, EventArgs e)
    {
        string itemCode = ((TextBox)sender).Text.Trim();
        GridViewRow row = (GridViewRow)((TextBox)sender).BindingContainer;
        TextBox tbInspectQty = (TextBox)row.FindControl("tbInspectQty");
        decimal inspectQty = tbInspectQty.Text.Trim() == string.Empty ? 0 : decimal.Parse(tbInspectQty.Text.Trim());
        if (itemCode != string.Empty)
        {
            if (InspectItemDic.ContainsKey(itemCode))
            {
                ShowErrorMessage("MasterData.Production.Feed.Item.Exists", itemCode);
                return;
            }
            InspectItemDic.Add(itemCode, inspectQty);
            UpdateInspectItemDic();
            this.InitPageParameter();
        }

    }
    public void UpdateView()
    {
        this.tbFlow.Text = string.Empty;
        this.InspectItemDic = new Dictionary<string, decimal>();
        this.InitPageParameter();
    }

    public void InitPageParameter()
    {

        IList<InspectItem> inspectItemList = new List<InspectItem>();
        foreach (string fgItemCode in this.InspectItemDic.Keys)
        {
            string[] fgItem = fgItemCode.Split('$');
            string itemCode = fgItem[0];
            string fgCode = fgItem[1];
            InspectItem inspectItem = new InspectItem();
            inspectItem.IsBlank = false;
            inspectItem.InspectQty = InspectItemDic[fgItemCode];
            inspectItem.Item = TheItemMgr.CheckAndLoadItem(itemCode);
            inspectItem.FinishGoods = TheItemMgr.CheckAndLoadItem(fgCode);
            inspectItemList.Add(inspectItem);
        }

        //新行
        InspectItem blankInspectItem = new InspectItem();
        blankInspectItem.IsBlank = true;
        inspectItemList.Add(blankInspectItem);

        this.GV_List.DataSource = inspectItemList;
        this.GV_List.DataBind();



    }

    protected void tbFlow_Changed(object sender, EventArgs e)
    {
        if (tbFlow.Text.Trim() != string.Empty)
        {
            Flow flow = TheFlowMgr.LoadFlow(tbFlow.Text.Trim());
            if (flow != null)
            {
                this.LocationCode = flow.LocationFrom.Code;
                InitPageParameter();
            }
        }
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            InspectItem inspectItem = (InspectItem)e.Row.DataItem;

            if (inspectItem.IsBlank)
            {
                e.Row.FindControl("lblItemCode").Visible = false;
                Controls_TextBox tbItemCode = (Controls_TextBox)e.Row.FindControl("tbItemCode");
                tbItemCode.Visible = true;
                tbItemCode.ServiceParameter = "string:" + tbFlow.Text.Trim();
                tbItemCode.DataBind();
                e.Row.FindControl("lbtnAdd").Visible = true;
            }
            else
            {
                e.Row.FindControl("lblItemCode").Visible = true;
                e.Row.FindControl("tbItemCode").Visible = false;
            }

        }
    }

    protected void lbtnAdd_Click(object sender, EventArgs e)
    {
        UpdateInspectItemDic();
        Controls_TextBox tbItemCode = (Controls_TextBox)((LinkButton)sender).Parent.FindControl("tbItemCode");
        TextBox tbInspectQty = (TextBox)((LinkButton)sender).Parent.FindControl("tbInspectQty");

        Item newItem = TheItemMgr.LoadItem(tbItemCode.Text.Trim());
        IList<BomDetail> bomDetailList = TheBomDetailMgr.GetFlatBomDetail(tbItemCode.Text.Trim(), DateTime.Now);

        EntityPreference entityPreference = TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_IS_DEFAULT_QTY_ZERO);
        if (bool.Parse(entityPreference.Value))
        {
            tbInspectQty.Text = "0";
        }

        foreach (BomDetail bomDetail in bomDetailList)
        {
           if(bomDetail.BackFlushMethod != BusinessConstants.CODE_MASTER_BACKFLUSH_METHOD_VALUE_BATCH_FEED)
           {
               if (InspectItemDic.ContainsKey(bomDetail.Item.Code + "$" + bomDetail.Bom.Code))
               {
                   InspectItemDic[bomDetail.Item.Code + "$" + bomDetail.Bom.Code] += bomDetail.RateQty * decimal.Parse(tbInspectQty.Text.Trim());
               }
               else
               {
                   InspectItemDic.Add(bomDetail.Item.Code + "$" + bomDetail.Bom.Code, bomDetail.RateQty * decimal.Parse(tbInspectQty.Text.Trim()));

               }
           }
        }
        InitPageParameter();

    }

    private void UpdateInspectItemDic()
    {
        for (int i = 0; i < this.GV_List.Rows.Count - 1; i++)
        {
            GridViewRow row = this.GV_List.Rows[i];
            string itemCode = ((Label)row.FindControl("lblItemCode")).Text.Trim();
            TextBox tbInspectQty = (TextBox)row.FindControl("tbInspectQty");
            string fgCode = ((HiddenField)row.FindControl("hfFgCode")).Value;
            decimal inspectQty = tbInspectQty.Text.Trim() == string.Empty ? 0 : decimal.Parse(tbInspectQty.Text.Trim());
            InspectItemDic[itemCode + "$" + fgCode] = inspectQty;
        }
    }

}

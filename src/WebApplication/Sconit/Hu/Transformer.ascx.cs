using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using com.Sconit.Utility;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.Distribution;

public partial class Hu_Transformer : ModuleBase
{
    public bool ReadOnly { get; set; }
    public bool DetailReadOnly { get; set; }

    public bool DetailVisible
    {
        get { return ViewState["DetailVisible"] != null ? (bool)ViewState["DetailVisible"] : false; }
        set { ViewState["DetailVisible"] = value; }
    }

    public bool IncludeOrderTracers
    {
        get { return ViewState["IncludeOrderTracers"] != null ? (bool)ViewState["IncludeOrderTracers"] : false; }
        set { ViewState["IncludeOrderTracers"] = value; }
    }

    private bool IsScanHu
    {
        get { return ViewState["IsScanHu"] != null ? (bool)ViewState["IsScanHu"] : false; }
        set { ViewState["IsScanHu"] = value; }
    }

    private bool IsPick
    { 
        get { return ViewState["IsPick"] != null ? (bool)ViewState["IsPick"] : false; }
        set { ViewState["IsPick"] = value; }
    }

    private List<Transformer> TransformerList
    {
        get { return (List<Transformer>)ViewState["TransformerList"]; }
        set { ViewState["TransformerList"] = value; }
    }

    #region GridViewRow Control Value
    private decimal GetActedQty(GridViewRow gvr)
    {
        return GetCurrentQtyTextBox(gvr).Text.Trim() != string.Empty ? decimal.Parse(GetCurrentQtyTextBox(gvr).Text.Trim()) : 0;
    }
    private decimal GetActedRejectQty(GridViewRow gvr)
    {
        return GetCurrentRejectQtyTextBox(gvr).Text.Trim() != string.Empty ? decimal.Parse(GetCurrentRejectQtyTextBox(gvr).Text.Trim()) : 0;
    }
    private TextBox GetCurrentQtyTextBox(GridViewRow gvr)
    {
        return (TextBox)gvr.FindControl("tbCurrentQty");
    }
    private TextBox GetCurrentRejectQtyTextBox(GridViewRow gvr)
    {
        return (TextBox)gvr.FindControl("tbCurrentRejectQty");
    }
    private Hu_TransformerDetail GetTransformerDetailControl(GridViewRow gvr)
    {
        return (Hu_TransformerDetail)gvr.FindControl("ucTransformerDetail");
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        foreach (GridViewRow gvr in GV_List.Rows)
        {
            GetTransformerDetailControl(gvr).QtyChangeEvent += new EventHandler(this.HuInputQtyChange_Render);
        }
    }

    void HuInputQtyChange_Render(object sender, EventArgs e)
    {
        Hu_TransformerDetail ucTransformerDetail = (Hu_TransformerDetail)((((TextBox)sender).NamingContainer).NamingContainer).Parent;
        GridViewRow gvr = (GridViewRow)ucTransformerDetail.NamingContainer;
        GetCurrentQtyTextBox(gvr).Text = ucTransformerDetail.SumQty().ToString("0.########");
    }

    public void InitPageParameter(List<Transformer> transformerList, string moduleType, string transformerType, bool isScanHu)
    {
        this.IsScanHu = isScanHu;
        this.TransformerList = transformerList;
        this.InitialUI(moduleType, transformerType);
        this.BindTransformer(transformerList);
    }

    public void InitPageParameter(Resolver resolver)
    {
        this.IsScanHu = resolver.IsScanHu;
        this.IsPick = resolver.Result == "Pick" ? true : false;
        this.TransformerList = resolver.Transformers;
        this.InitialUI(resolver.OrderType, resolver.ModuleType);
        this.BindTransformer(resolver.Transformers);
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string receiptOpt = TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_DEFAULT_RECEIPT_OPTION).Value;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Transformer transformer = (Transformer)e.Row.DataItem;
            GetTransformerDetailControl(e.Row).GV_DataBind(transformer.TransformerDetails);

            if (transformer.CurrentQty == 0)
            {
                GetCurrentQtyTextBox(e.Row).Text = string.Empty;//0不显示
            }
            GetCurrentQtyTextBox(e.Row).ReadOnly = this.IsScanHu || this.ReadOnly;
            if (transformer.CurrentRejectQty == 0)
            {
                GetCurrentRejectQtyTextBox(e.Row).Text = string.Empty;
            }
            GetCurrentRejectQtyTextBox(e.Row).ReadOnly = this.IsScanHu || this.ReadOnly;

            if (transformer.LocationFromCode != null && transformer.LocationFromCode.Trim() != string.Empty)
                GV_List.Columns[7].Visible = true;//LocationFrom
            if (transformer.LocationToCode != null && transformer.LocationToCode.Trim() != string.Empty)
                GV_List.Columns[8].Visible = true;//LocationTo

            if (receiptOpt == BusinessConstants.ENTITY_PREFERENCE_CODE_DEFAULT_RECEIPT_OPTION_GOODS_RECEIPT_LOTSIZE && !GetCurrentQtyTextBox(e.Row).ReadOnly)
            {
                if (this.IsPick == true)
                {
                    GetCurrentQtyTextBox(e.Row).Text = (transformer.OrderedQty - transformer.ShippedQty).ToString("0.#########");
                }
                else
                {
                    GetCurrentQtyTextBox(e.Row).Text = transformer.CurrentQty.ToString("0.#########");
                }
            }
            if (IncludeOrderTracers)
            {
                e.Row.Cells[2].Attributes.Add("title", GetDetail(transformer));
            }
        }
    }

    public void BindTransformer(List<Transformer> transformerList)
    {
        this.GV_List.DataSource = transformerList;
        this.GV_List.DataBind();
    }

    public List<Transformer> GetTransformer()
    {
        List<Transformer> transformerList = new List<Transformer>();
        foreach (GridViewRow gvr in GV_List.Rows)
        {
            Transformer transformer = new Transformer();

            string hfOrderLocTransId = ((HiddenField)gvr.FindControl("hfOrderLocTransId")).Value;
            if (hfOrderLocTransId == string.Empty || hfOrderLocTransId == null)
            {
                transformer.OrderLocTransId = 0;
            }
            else
            {
                transformer.OrderLocTransId = int.Parse(hfOrderLocTransId);
            }
            transformer.Id = int.Parse(((HiddenField)gvr.FindControl("hfId")).Value);
            transformer.OrderNo = ((Label)gvr.FindControl("lblOrderNo")).Text;
            transformer.Sequence = int.Parse(((Label)gvr.FindControl("lblSequence")).Text);
            transformer.ItemCode = ((Label)gvr.FindControl("lblItemCode")).Text;
            transformer.ItemDescription = ((Label)gvr.FindControl("lblItemDescription")).Text;
            transformer.ReferenceItemCode = ((Label)gvr.FindControl("lblReferenceItemCode")).Text;
            transformer.UomCode = ((Label)gvr.FindControl("lblUom")).Text;
            transformer.UnitCount = decimal.Parse(((Label)gvr.FindControl("lblUnitCount")).Text);
            transformer.LocationFromCode = ((Label)gvr.FindControl("lblLocationFrom")).Text;
            transformer.LocationToCode = ((Label)gvr.FindControl("lblLocationTo")).Text;
            transformer.Qty = decimal.Parse(((Label)gvr.FindControl("lblQty")).Text);
            //订单数，为ASN收货加的
            transformer.OrderedQty = decimal.Parse(((Label)gvr.FindControl("lblQty")).Text);
            transformer.ReceivedQty = decimal.Parse(((Label)gvr.FindControl("lblReceivedQty")).Text);
            transformer.CurrentQty = this.GetActedQty(gvr);
            transformer.TransformerDetails = GetTransformerDetailControl(gvr).GetHuList();
            transformerList.Add(transformer);
        }
        return transformerList;
    }

    public List<Transformer> GetOrderTracersTransformer()
    {
        foreach (GridViewRow gvr in GV_List.Rows)
        {
            int id = int.Parse(((HiddenField)gvr.FindControl("hfId")).Value);
            decimal currentQty = this.GetActedQty(gvr);
            string memo = ((TextBox)gvr.FindControl("tbMemo")).Text.Trim();
            foreach (Transformer transformer in this.TransformerList)
            {
                if (transformer.Id == id)
                {
                    transformer.CurrentQty = currentQty;
                }
            }
        }
        return this.TransformerList;
    }

    public List<Transformer> GetInspectTransformer()
    {
        List<Transformer> transformerList = new List<Transformer>();
        foreach (GridViewRow gvr in GV_List.Rows)
        {
            Transformer transformer = new Transformer();
            transformer.Id = int.Parse(((HiddenField)gvr.FindControl("hfId")).Value);
            transformer.ItemCode = ((Label)gvr.FindControl("lblItemCode")).Text;
            transformer.ItemDescription = ((Label)gvr.FindControl("lblItemDescription")).Text;
            transformer.UomCode = ((Label)gvr.FindControl("lblUom")).Text;
            transformer.UnitCount = decimal.Parse(((Label)gvr.FindControl("lblUnitCount")).Text);
            transformer.LocationFromCode = ((Label)gvr.FindControl("lblLocationFrom")).Text;
            transformer.LocationToCode = ((Label)gvr.FindControl("lblLocationTo")).Text;
            transformer.CurrentQty = this.GetActedQty(gvr);
            transformer.CurrentRejectQty = this.GetActedRejectQty(gvr);
            transformer.TransformerDetails = GetTransformerDetailControl(gvr).GetHuList();

            transformerList.Add(transformer);
        }

        return transformerList;
    }

    private void InitialUI(string moduleType, string transformerType)
    {
        this.GV_List.Columns[GV_List.Columns.Count - 1].Visible = IsScanHu && DetailVisible;
        //this.GV_List.Columns[14].Visible = false;

        if (transformerType == BusinessConstants.TRANSFORMER_MODULE_TYPE_SHIP)
        {
            this.GV_List.Columns[9].HeaderText = TheLanguageMgr.TranslateContent("Common.Business.QtyToShip", this.CurrentUser.UserLanguage);
            this.GV_List.Columns[11].HeaderText = TheLanguageMgr.TranslateContent("MasterData.Order.OrderDetail.CurrentShipQty", this.CurrentUser.UserLanguage);
        }
        else if (transformerType == BusinessConstants.TRANSFORMER_MODULE_TYPE_RECEIVE)
        {
            this.GV_List.Columns[9].HeaderText = TheLanguageMgr.TranslateContent("MasterData.Order.OrderDetail.CurrentShipQty", this.CurrentUser.UserLanguage);
            this.GV_List.Columns[11].HeaderText = TheLanguageMgr.TranslateContent("MasterData.Order.OrderDetail.CurrentReceiveQty", this.CurrentUser.UserLanguage);
            this.GV_List.Columns[10].Visible = true;

            if (moduleType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
            {
                this.GV_List.Columns[9].Visible = false;
            }
            else
            {
                //int i = this.TransformerList.Where(o => (o.s1 != null && o.s1 != string.Empty)).Count();
                //if (i > 0)
                //{
                   // this.GV_List.Columns[13].Visible = true;
                //}
                //else
                //{
                    this.GV_List.Columns[13].Visible = false;
                //}
            }
        }
        else if (transformerType == BusinessConstants.TRANSFORMER_MODULE_TYPE_PICKUP || transformerType == BusinessConstants.TRANSFORMER_MODULE_TYPE_SHIPVIEW)
        {
            this.GV_List.Columns[11].HeaderText = TheLanguageMgr.TranslateContent("Common.Business.Qty", this.CurrentUser.UserLanguage);
            this.GV_List.Columns[9].Visible = false;
        }
        else if (transformerType == BusinessConstants.TRANSFORMER_MODULE_TYPE_INSPECT)
        {
            this.GV_List.Columns[0].Visible = false;
            this.GV_List.Columns[1].Visible = false;
            this.GV_List.Columns[3].Visible = false;
            this.GV_List.Columns[4].Visible = false;
            this.GV_List.Columns[12].Visible = true;

            this.GV_List.Columns[9].HeaderText = TheLanguageMgr.TranslateContent("MasterData.InspectOrder.InspectOrderDetail.InspectQty", this.CurrentUser.UserLanguage);
            this.GV_List.Columns[11].HeaderText = TheLanguageMgr.TranslateContent("MasterData.InspectOrder.InspectOrderDetail.CurrentQualifyQty", this.CurrentUser.UserLanguage);
            this.GV_List.Columns[12].HeaderText = TheLanguageMgr.TranslateContent("MasterData.InspectOrder.InspectOrderDetail.CurrentRejectQty", this.CurrentUser.UserLanguage);
        }
        else if (transformerType == BusinessConstants.TRANSFORMER_MODULE_TYPE_TRANSFER)
        {
            this.GV_List.Columns[0].Visible = false;
            this.GV_List.Columns[9].HeaderText = TheLanguageMgr.TranslateContent("Common.Business.QtyToShip", this.CurrentUser.UserLanguage);
            this.GV_List.Columns[11].Visible = true;
            this.GV_List.Columns[11].HeaderText = TheLanguageMgr.TranslateContent("MasterData.Order.OrderDetail.CurrentShipQty", this.CurrentUser.UserLanguage);
        }
        else
        {
            this.GV_List.Columns[0].Visible = false;
            this.GV_List.Columns[9].HeaderText = TheLanguageMgr.TranslateContent("MasterData.Order.OrderDetail.OrderedQty", this.CurrentUser.UserLanguage);
            this.GV_List.Columns[11].Visible = false;
            //this.GV_List.Columns[14].Visible = true;
        }
    }


    //临时解决一下问题，ASN多次收货，执行中的会显示发货数和已收数
    public void InitialUIForAsn(InProcessLocation ip)
    {
        if (ip.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)
        {
            this.GV_List.Columns[9].Visible = true;
            this.GV_List.Columns[9].HeaderText = TheLanguageMgr.TranslateContent("Common.Business.Qty", this.CurrentUser.UserLanguage);
            this.GV_List.Columns[11].HeaderText = TheLanguageMgr.TranslateContent("Common.Business.QtyToReceive", this.CurrentUser.UserLanguage);
            this.GV_List.DataBind();
        }
        else if (ip.Status == BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE)
        {
            this.GV_List.Columns[9].Visible = true;
            this.GV_List.Columns[9].HeaderText = TheLanguageMgr.TranslateContent("Common.Business.Qty", this.CurrentUser.UserLanguage);
            this.GV_List.Columns[11].Visible = false;
            this.GV_List.DataBind();
        }
        //int i = this.TransformerList.Where(o => (o.s1 != null && o.s1 != string.Empty)).Count();
        //if (i > 0)
        //{
            //this.GV_List.Columns[13].Visible = true;
        //}
        //else
        //{
            this.GV_List.Columns[13].Visible = false;
        //}
    }

    private string GetDetail(Transformer transformer)
    {
        string detail = string.Empty;
        if (transformer.TransformerDetails != null)
        {
            detail += "cssbody=[obbd] cssheader=[obhd] header=[" + transformer.ItemCode + " | " + transformer.ItemDescription
                + " | " + transformer.Qty.ToString("0.######") + "] body=[<table width=100%>";
            //IList<OrderDetail> ods = transformer.OrderDetails;
            foreach (TransformerDetail td in transformer.TransformerDetails)
            {
                string type = TheLanguageMgr.TranslateContent("LeanEngine."+ td.IOType,this.CurrentUser.UserLanguage);
                detail += "<tr><td>" + type + "</td><td>" + td.s1 + "</td><td>" + td.s2 + "</td><td>"
                    + td.Qty.ToString("0.######") + "</td><td>" + td.s3 + "</td></tr>";
            }
            detail += "</table>]";
        }
        return detail;
    }
}

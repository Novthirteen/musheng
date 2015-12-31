using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Web;



public partial class Order_OrderIssue_ShipItemList : BusinessModuleBase
{
    public event EventHandler ShipEvent;
    public event EventHandler ShipBackEvent;
    public event EventHandler BindInfoEvent;
    public event EventHandler CreatePickListEvent;


    #region ViewState
    private List<string> orderNoList
    {
        get { return (List<string>)ViewState["orderNoList"]; }
        set { ViewState["orderNoList"] = value; }
    }

    public string ModuleType
    {
        get { return (string)ViewState["ModuleType"]; }
        set { ViewState["ModuleType"] = value; }
    }
    private bool IsPickList
    {
        get { return (bool)ViewState["IsPickList"]; }
        set { ViewState["IsPickList"] = value; }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ucHuList.HuSaveEvent += new EventHandler(this.HuListSave_Render);

        if (!IsPostBack)
        {
            InitialResolver(this.CurrentUser.Code, BusinessConstants.TRANSFORMER_MODULE_TYPE_SHIP);
        }
    }

    public void InitPageParameter(List<string> orderNoList, List<List<string>> itemList)
    {
        this.InitPageParameter(orderNoList,itemList, false);
        this.orderNoList = orderNoList;
    }
    public void InitPageParameter(List<string> orderNoList, List<List<string>> itemList, bool createPickList)
    {
        this.orderNoList = orderNoList;
        try
        {
            //先清空上次记录
            this.CacheResolver.Transformers = new List<Transformer>();

            this.IsPickList = createPickList;
            foreach (string orderNo in orderNoList)
            {
                ResolveInput(orderNo);
            }
            if (IsPickList)
            {
                this.CacheResolver.IsScanHu = false;
                //this.CacheResolver.IsDetailContainHu = false;
            }
            else
            {
                foreach (Transformer t in this.CacheResolver.Transformers)
                {
                    t.CurrentQty = 0;
                }
            }
            //ljz s
            List<Transformer> tfList = new List<Transformer>();
            foreach(List<string> item in itemList)
            {
                foreach (Transformer t in this.CacheResolver.Transformers)
                {
                    if (item[1] == t.ItemCode && t.OrderNo == item[0])
                    {
                        tfList.Add(t);
                    }
                }
            }
            this.CacheResolver.Transformers = tfList;
            //ljz e
            this.ucTransformer.InitPageParameter(this.CacheResolver);

            if (BindInfoEvent != null)
                BindInfoEvent(new object[] { orderNoList[0] }, null);

            this.InitialUI();
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage(ex);
        }
    }

    protected void btnScanHu_Click(object sender, EventArgs e)
    {
        this.ucHuList.Visible = true;
        this.ucHuList.InitPageParameter();
    }

    protected void btnShip_Click(object sender, EventArgs e)
    {
        try
        {
            this.CacheResolver.Transformers = this.ucTransformer.GetTransformer();
            ExecuteSubmit();
            ShowSuccessMessage("MasterData.Distribution.Ship.Successfully", this.CacheResolver.Code);
            if (ShipEvent != null)
            {
                ShipEvent(new object[] { this.CacheResolver.Code, cbPrintAsn.Checked }, null);
            }

        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage(ex);
        }
    }

    protected void btnCreatePickList_Click(object sender, EventArgs e)
    {
        try
        {
            PickList pickList = ThePickListMgr.CreatePickList(this.ucTransformer.GetTransformer(), this.CurrentUser);
            ShowSuccessMessage("MasterData.PickList.CreatePickList.Successfully", pickList.PickListNo);
            if (CreatePickListEvent != null)
            {
                CreatePickListEvent(pickList.PickListNo, null);
            }
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage(ex);
        }
    }

    protected override void BindTransformer()
    {
        this.ucTransformer.BindTransformer(this.CacheResolver.Transformers);
    }

    protected override void BindTransformerDetail()
    {
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.Visible = false;
        if (this.ShipBackEvent != null)
        {
            this.ShipBackEvent(this, e);
        }
    }

    protected void tbScanBarcode_TextChanged(object sender, EventArgs e)
    {
        this.lblMessage.Text = string.Empty;
        this.HuInput(this.tbScanBarcode.Text.Trim());
        this.InitialHuInput();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        foreach (string orderNo in this.orderNoList)
        {
            OrderHead orderHead = TheOrderHeadMgr.LoadOrderHead(orderNo);
            string orderTemplate = orderHead.OrderTemplate;
            if (orderTemplate == null || orderTemplate.Length == 0)
            {
                ShowErrorMessage("MasterData.Order.OrderHead.PleaseConfigOrderTemplate");
            }
            else
            {
                //IReportBaseMgr iReportBaseMgr = this.GetIReportBaseMgr(orderTemplate, orderHead);
                string printUrl = TheReportMgr.WriteToFile(orderTemplate, orderNo);
                Page.ClientScript.RegisterStartupScript(GetType(), "method", " <script language='javascript' type='text/javascript'>PrintOrder('" + printUrl + "'); </script>");
            }
        }
    }

    private void HuInput(string huId)
    {
        try
        {
            this.CacheResolver.Transformers = this.ucTransformer.GetTransformer();
            ResolveInput(huId);
            BindTransformer();
        }
        catch (BusinessErrorException ex)
        {
            this.lblMessage.Text = TheLanguageMgr.TranslateMessage(ex.Message, this.CurrentUser, ex.MessageParams);
        }
    }

    private void InitialHuInput()
    {
        this.tbScanBarcode.Text = string.Empty;
        this.tbScanBarcode.Focus();
    }

    void HuListSave_Render(object sender, EventArgs e)
    {
        IList<Hu> huList = (IList<Hu>)((object[])sender)[0];
        foreach (Hu hu in huList)
        {
            this.HuInput(hu.HuId);
        }
    }

    private void InitialUI()
    {
        this.lblMessage.Text = string.Empty;

        this.ltlScanBarcode.Visible = !IsPickList && CacheResolver.IsScanHu;
        this.tbScanBarcode.Visible = !IsPickList && CacheResolver.IsScanHu;
        this.btnScanHu.Visible = !IsPickList && CacheResolver.IsScanHu;
        this.cbPrintAsn.Visible = !IsPickList;
        this.btnShip.Visible = !IsPickList;
        this.btnCreatePickList.Visible = IsPickList;

        if (!IsPickList && CacheResolver.IsScanHu)
            this.InitialHuInput();
    }
}

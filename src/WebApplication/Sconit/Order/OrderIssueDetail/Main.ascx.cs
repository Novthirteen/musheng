using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.Distribution;
using com.Sconit.Entity;

public partial class Order_OrderIssueDetail_Main : MainModuleBase
{
    public event EventHandler CreatePickListEvent;
    public event EventHandler BackEvent;
    public string ModuleType
    {
        get
        {
            return (string)ViewState["ModuleType"];
        }
        set
        {
            ViewState["ModuleType"] = value;
        }
    }
    public string ModuleSubType
    {
        get
        {
            return (string)ViewState["ModuleSubType"];
        }
        set
        {
            ViewState["ModuleSubType"] = value;
        }
    }
    public bool IsSupplier
    {
        get { return ViewState["IsSupplier"] != null ? (bool)ViewState["IsSupplier"] : false; }
        set { ViewState["IsSupplier"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (this.ModuleParameter.ContainsKey("ModuleType"))
            {
                this.ModuleType = this.ModuleParameter["ModuleType"];
                this.ucFlowInfo.ModuleType = this.ModuleType;
                this.ucList.ModuleType = this.ModuleType;
                this.ucSearch.ModuleType = this.ModuleType;
            }
            if (this.ModuleParameter.ContainsKey("ModuleSubType"))
            {
                this.ModuleSubType = this.ModuleParameter["ModuleSubType"];
                this.ucFlowInfo.ModuleType = this.ModuleType;
                this.ucList.ModuleType = this.ModuleType;
                this.ucSearch.ModuleType = this.ModuleType;
            }
            if (this.ModuleParameter != null && this.ModuleParameter.ContainsKey("IsSupplier"))
            {
                this.IsSupplier = bool.Parse(this.ModuleParameter["IsSupplier"]);
                this.ucSearch.IsSupplier = this.IsSupplier;
            }
            else
            {
                this.ucSearch.IsSupplier = false;
            }

            if (this.Session["Temp_Session_OrderNo"] != null && this.Session["Temp_Session_OrderNo"] != string.Empty)
            {
                List<string> orderNoList = new List<string>();
                orderNoList.Add((string)this.Session["Temp_Session_OrderNo"]);
                Session.Contents.Remove("Temp_Session_OrderNo");
            }
        }

        this.ucSearch.SearchEvent += new System.EventHandler(this.Search_Render);

        this.ucSearch.SearchEventByNull += new System.EventHandler(this.Search_RenderNull); //add by ljz 
        this.ucList.ShipSuccessEvent += new System.EventHandler(this.Search_Render);
    }


    void ShipSuccess_Render(object sender, EventArgs e)
    {
        this.ucViewShipList.Visible = true;
        this.ucViewShipList.InitPageParameter((string)((object[])sender)[0], (bool)((object[])sender)[1]);
    }

    //add by ljz start
    void Search_RenderNull(object sender, EventArgs e)
    {
        this.ucList.GV_ListBindNull();
    }
    //add by ljz end

    //The event handler when user click button "Search" button
    void Search_Render(object sender, EventArgs e)
    {
        //if (this.ModuleType == BusinessConstants.ORDER_MODULETYPE_VALUE_SUPPLIERDISTRIBUTION)
        //if (this.IsSupplier)
        //{
        //    this.ucList.Visible = false;
        //    this.ucSupplierList.Visible = true;
        //    this.ucSupplierList.InitPageParameter((string)((object[])sender)[0], (string)((object[])sender)[1]);
        //}
        //else
        //{
            //this.ucSupplierList.Visible = false;
            this.ucList.Visible = true;
            //modify by ljz start
            //this.ucList.InitPageParameter((string)((object[])sender)[0], (string)((object[])sender)[1]);
            //if (((string)((object[])sender)[2]) == "Flow")
            //{
            this.ucList.InitPageParameter((string)((object[])sender)[0], (string)((object[])sender)[1], (string)((object[])sender)[2], (string)((object[])sender)[3], (string)((object[])sender)[4], (bool)((object[])sender)[5], (bool)((object[])sender)[6], this.IsSupplier);
            //}
            //else if (((string)((object[])sender)[2]) == "ItemCode")
            //{
            //    this.ucList.InitPageParameterByItemCode((string)((object[])sender)[0], (string)((object[])sender)[1]);
            //}
            //modify by ljz end
        //}
    }





}

using System;
using System.Collections;
using System.Collections.Generic;
using com.Sconit.Entity;
using com.Sconit.Entity.Distribution;
using com.Sconit.Entity.MasterData;
using com.Sconit.Utility;
using com.Sconit.Web;
using NHibernate.Expression;
using System.Web.UI.WebControls;
using System.Linq;

public partial class Order_GoodsReceipt_AsnReceipt_Search : SearchModuleBase
{
    public event EventHandler SearchEvent;



    public bool IsSupplier
    {
        get { return ViewState["IsSupplier"] != null ? (bool)ViewState["IsSupplier"] : false; }
        set { ViewState["IsSupplier"] = value; }
    }

    private IDictionary<string, string> dicParam;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.tbFlow.ServiceParameter = "string:" + this.CurrentUser.Code;

        if (!IsPostBack)
        {
            this.tbStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.tbEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            IList<CodeMaster> statusList = GetStatusGroup();
            statusList.Insert(0, new CodeMaster()); //添加空选项
            this.ddlStatus.DataSource = statusList;
            this.ddlStatus.DataBind();

        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillParameter();
        int rblListIndex = this.rblListFormat.SelectedIndex;
        if ((this.dicParam["OrderNo"] != string.Empty || this.dicParam["Item"] != string.Empty) && rblListIndex == 0)
        {
            rblListIndex = 1;
        }
        this.rblListFormat.SelectedIndex = rblListIndex;

        Button btn = (Button)sender;
        if (SearchEvent != null)
        {
            if (btn == this.btnExport)
            {
                if (this.rblListFormat.SelectedValue == "Detail")
                {
                    object criteriaParam = this.CollectDetailParam(true);
                    SearchEvent(criteriaParam, null);
                }
                else
                {
                    object criteriaParam = this.CollectMasterParam(true);
                    SearchEvent(criteriaParam, null);
                }
            }
            else
            {
                DoSearch();
            }
        }
    }

    protected override void DoSearch()
    {
        FillParameter();
        if (this.rblListFormat.SelectedValue == "Detail")
        {
            object criteriaParam = this.CollectDetailParam(false);
            SearchEvent(criteriaParam, null);
        }
        else
        {
            object criteriaParam = this.CollectMasterParam(false);
            SearchEvent(criteriaParam, null);
        }
    }

    private object CollectDetailParam(bool IsExport)
    {
        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(InProcessLocationDetail));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(InProcessLocationDetail))
            .SetProjection(Projections.Count("Id"));
        selectCriteria.CreateAlias("InProcessLocation", "ip");
        selectCountCriteria.CreateAlias("InProcessLocation", "ip");
        selectCriteria.CreateAlias("ip.Flow", "f");
        selectCountCriteria.CreateAlias("ip.Flow", "f");

        selectCriteria.CreateAlias("ip.PartyFrom", "pf");
        selectCountCriteria.CreateAlias("ip.PartyFrom", "pf");
      
        selectCriteria.CreateAlias("OrderLocationTransaction", "olt");
        selectCountCriteria.CreateAlias("OrderLocationTransaction", "olt");
        selectCriteria.CreateAlias("olt.OrderDetail", "od");
        selectCountCriteria.CreateAlias("olt.OrderDetail", "od");
        selectCriteria.CreateAlias("od.OrderHead", "o");
        selectCountCriteria.CreateAlias("od.OrderHead", "o");


        if (this.dicParam["IpNo"] != string.Empty)
        {
            selectCriteria.Add(Expression.Like("ip.IpNo", this.dicParam["IpNo"], MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("ip.IpNo", this.dicParam["IpNo"], MatchMode.Anywhere));
        }

        #region date
        if (this.dicParam["StartDate"] != string.Empty)
        {
            selectCriteria.Add(Expression.Ge("ip.CreateDate", DateTime.Parse(this.dicParam["StartDate"])));
            selectCountCriteria.Add(Expression.Ge("ip.CreateDate", DateTime.Parse(this.dicParam["StartDate"])));
        }
        if (this.dicParam["EndDate"] != string.Empty)
        {
            selectCriteria.Add(Expression.Lt("ip.CreateDate", DateTime.Parse(this.dicParam["EndDate"]).AddDays(1)));
            selectCountCriteria.Add(Expression.Lt("ip.CreateDate", DateTime.Parse(this.dicParam["EndDate"]).AddDays(1)));
        }
        #endregion

        #region 权限
        List<Permission> suppliers = this.CurrentUser.OrganizationPermission.Where(p => (p.Category.Code == BusinessConstants.CODE_MASTER_PERMISSION_CATEGORY_TYPE_VALUE_SUPPLIER)).ToList();
        if (this.dicParam["Flow"] != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("f.Code", this.dicParam["Flow"]));
            selectCountCriteria.Add(Expression.Eq("f.Code", this.dicParam["Flow"]));
        }
        else
        {
            selectCriteria.Add(Expression.In("pf.Code", suppliers.Select(p => p.Code).ToList()));
            selectCountCriteria.Add(Expression.In("pf.Code", suppliers.Select(p => p.Code).ToList()));
        }

        List<string> flowTypes = new List<string>() 
                    {BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT,
                        BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS
                        ,BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING};

        selectCriteria.Add(Expression.In("f.Type", flowTypes));
        selectCountCriteria.Add(Expression.In("f.Type", flowTypes));
        selectCriteria.Add(Expression.Eq("f.IsActive", true));
        selectCountCriteria.Add(Expression.Eq("f.IsActive", true));
        #endregion



        #region item order
        if (this.dicParam["Item"] != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("olt.Item.Code", this.dicParam["Item"]));
            selectCountCriteria.Add(Expression.Eq("olt.Item.Code", this.dicParam["Item"]));
        }
        if (this.dicParam["OrderNo"] != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("o.OrderNo", this.dicParam["OrderNo"]));
            selectCountCriteria.Add(Expression.Eq("o.OrderNo", this.dicParam["OrderNo"]));
        }
        #endregion

        if (this.dicParam["Status"] != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("ip.Status", this.dicParam["Status"]));
            selectCountCriteria.Add(Expression.Eq("ip.Status", this.dicParam["Status"]));
        }

        return new object[] { selectCriteria, selectCountCriteria, IsExport, false };
    }

    private object CollectMasterParam(bool IsExport)
    {
        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(InProcessLocation));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(InProcessLocation))
            .SetProjection(Projections.Count("IpNo"));
        selectCriteria.CreateAlias("PartyFrom", "pf");
        selectCriteria.CreateAlias("Flow", "f");
        selectCountCriteria.CreateAlias("PartyFrom", "pf");
        selectCountCriteria.CreateAlias("Flow", "f");

        if (this.dicParam["IpNo"] != string.Empty)
        {
            selectCriteria.Add(Expression.Like("IpNo", this.dicParam["IpNo"], MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("IpNo", this.dicParam["IpNo"], MatchMode.Anywhere));
        }
        #region date
        if (this.dicParam["StartDate"] != string.Empty)
        {
            selectCriteria.Add(Expression.Ge("CreateDate", DateTime.Parse(this.dicParam["StartDate"])));
            selectCountCriteria.Add(Expression.Ge("CreateDate", DateTime.Parse(this.dicParam["StartDate"])));
        }
        if (this.dicParam["EndDate"] != string.Empty)
        {
            selectCriteria.Add(Expression.Lt("CreateDate", DateTime.Parse(this.dicParam["EndDate"]).AddDays(1)));
            selectCountCriteria.Add(Expression.Lt("CreateDate", DateTime.Parse(this.dicParam["EndDate"]).AddDays(1)));
        }
        #endregion


        #region 权限
        List<Permission> suppliers = this.CurrentUser.OrganizationPermission.Where(p => (p.Category.Code == BusinessConstants.CODE_MASTER_PERMISSION_CATEGORY_TYPE_VALUE_SUPPLIER)).ToList();
        if (this.dicParam["Flow"] != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("f.Code", this.dicParam["Flow"]));
            selectCountCriteria.Add(Expression.Eq("f.Code", this.dicParam["Flow"]));
        }
        else
        {
            selectCriteria.Add(Expression.In("pf.Code", suppliers.Select(p => p.Code).ToList()));
            selectCountCriteria.Add(Expression.In("pf.Code", suppliers.Select(p => p.Code).ToList()));
        }

        List<string> flowTypes = new List<string>() 
                    {BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT,
                        BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS
                        ,BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING};

        selectCriteria.Add(Expression.In("f.Type", flowTypes));
        selectCountCriteria.Add(Expression.In("f.Type", flowTypes));
        selectCriteria.Add(Expression.Eq("f.IsActive", true));
        selectCountCriteria.Add(Expression.Eq("f.IsActive", true));
        #endregion


        if (this.dicParam["Status"] != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("Status", this.dicParam["Status"]));
            selectCountCriteria.Add(Expression.Eq("Status", this.dicParam["Status"]));
        }


        return new object[] { selectCriteria, selectCountCriteria, IsExport, true };
    }

    private void FillParameter()
    {
        this.dicParam = new Dictionary<string, string>();
        this.dicParam["IpNo"] = this.tbIpNo.Text.Trim();

        this.dicParam["Status"] = this.ddlStatus.SelectedValue;
        this.dicParam["OrderNo"] = this.tbOrderNo.Text.Trim();
        this.dicParam["Item"] = this.tbItem.Text.Trim();
        this.dicParam["StartDate"] = this.tbStartDate.Text.Trim();
        this.dicParam["EndDate"] = this.tbEndDate.Text.Trim();
        this.dicParam["Flow"] = this.tbFlow.Text.Trim();
    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {
        //todo
    }

    private IList<CodeMaster> GetStatusGroup()
    {
        IList<CodeMaster> statusGroup = new List<CodeMaster>();

        statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));
        statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS));
        statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE));

        return statusGroup;
    }

    private CodeMaster GetStatus(string statusValue)
    {
        return TheCodeMasterMgr.GetCachedCodeMaster(BusinessConstants.CODE_MASTER_STATUS, statusValue);
    }
}

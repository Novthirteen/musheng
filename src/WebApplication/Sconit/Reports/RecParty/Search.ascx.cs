using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Service.Ext.MasterData;
using NHibernate.Expression;
using com.Sconit.Entity.Distribution;
using com.Sconit.Utility;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;

public partial class Order_ReceiptNotes_Search : SearchModuleBase
{
    public event EventHandler SearchEvent;
    public event EventHandler ExportEvent;



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
            this.tbStartDate.Text = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            this.tbEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
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

    private object CollectMasterParam(bool IsExport)
    {
        List<Permission> suppliers = this.CurrentUser.OrganizationPermission.Where(p => (p.Category.Code == BusinessConstants.CODE_MASTER_PERMISSION_CATEGORY_TYPE_VALUE_SUPPLIER)).ToList();

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(Receipt));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(Receipt))
            .SetProjection(Projections.Count("ReceiptNo"));
        selectCriteria.CreateAlias("PartyFrom", "pf");
        selectCriteria.CreateAlias("PartyTo", "pt");
        selectCriteria.CreateAlias("Flow", "f");
        selectCountCriteria.CreateAlias("PartyFrom", "pf");
        selectCountCriteria.CreateAlias("PartyTo", "pt");
        selectCountCriteria.CreateAlias("Flow", "f");

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

        if (this.dicParam["ReceiptNo"] != string.Empty)
        {
            selectCriteria.Add(Expression.Like("ReceiptNo", this.dicParam["ReceiptNo"], MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("ReceiptNo", this.dicParam["ReceiptNo"], MatchMode.Anywhere));
        }

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

        if (this.dicParam["IpNo"] != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("ReferenceIpNo", this.dicParam["IpNo"]));
            selectCountCriteria.Add(Expression.Eq("ReferenceIpNo", this.dicParam["IpNo"]));
        }
        return (new object[] { selectCriteria, selectCountCriteria, IsExport, true });
    }

    private object CollectDetailParam(bool IsExport)
    {
        List<Permission> suppliers = this.CurrentUser.OrganizationPermission.Where(p => (p.Category.Code == BusinessConstants.CODE_MASTER_PERMISSION_CATEGORY_TYPE_VALUE_SUPPLIER)).ToList();

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(ReceiptDetail));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(ReceiptDetail))
            .SetProjection(Projections.Count("Id"));
        selectCriteria.CreateAlias("Receipt", "r");
        selectCountCriteria.CreateAlias("Receipt", "r");
        selectCriteria.CreateAlias("r.PartyFrom", "pf");
        selectCountCriteria.CreateAlias("r.PartyFrom", "pf");
        selectCriteria.CreateAlias("r.Flow", "f");
        selectCountCriteria.CreateAlias("r.Flow", "f");

        selectCriteria.CreateAlias("OrderLocationTransaction", "olt");
        selectCountCriteria.CreateAlias("OrderLocationTransaction", "olt");
        selectCriteria.CreateAlias("olt.OrderDetail", "od");
        selectCountCriteria.CreateAlias("olt.OrderDetail", "od");
        selectCriteria.CreateAlias("od.OrderHead", "o");
        selectCountCriteria.CreateAlias("od.OrderHead", "o");

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

        if (this.dicParam["ReceiptNo"] != string.Empty)
        {
            selectCriteria.Add(Expression.Like("r.ReceiptNo", this.dicParam["ReceiptNo"], MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("r.ReceiptNo", this.dicParam["ReceiptNo"], MatchMode.Anywhere));
        }

        if (this.dicParam["StartDate"] != string.Empty)
        {
            selectCriteria.Add(Expression.Ge("r.CreateDate", DateTime.Parse(this.dicParam["StartDate"])));
            selectCountCriteria.Add(Expression.Ge("r.CreateDate", DateTime.Parse(this.dicParam["StartDate"])));
        }
        if (this.dicParam["EndDate"] != string.Empty)
        {
            selectCriteria.Add(Expression.Lt("r.CreateDate", DateTime.Parse(this.dicParam["EndDate"]).AddDays(1)));
            selectCountCriteria.Add(Expression.Lt("r.CreateDate", DateTime.Parse(this.dicParam["EndDate"]).AddDays(1)));
        }

        if (this.dicParam["IpNo"] != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("r.ReferenceIpNo", this.dicParam["IpNo"]));
            selectCountCriteria.Add(Expression.Eq("r.ReferenceIpNo", this.dicParam["IpNo"]));
        }



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

        return (new object[] { selectCriteria, selectCountCriteria, IsExport, false });
    }

    private void FillParameter()
    {
        this.dicParam = new Dictionary<string, string>();
        this.dicParam["ReceiptNo"] = this.tbReceiptNo.Text.Trim();
        this.dicParam["IpNo"] = this.tbIpNo.Text.Trim();

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
}

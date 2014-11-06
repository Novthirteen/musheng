using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using NHibernate.Expression;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using Geekees.Common.Controls;
using System.Data.SqlClient;
using System.Data;

public partial class Finance_Bill_Search : SearchModuleBase
{
    public event EventHandler SearchEvent;
    public event EventHandler SearchEvent1;
    public event EventHandler NewEvent;

    public bool IsSupplier
    {
        get { return ViewState["IsSupplier"] != null ? (bool)ViewState["IsSupplier"] : false; }
        set { ViewState["IsSupplier"] = value; }
    }

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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //this.ddlStatus.DataSource = this.GetAllStatus();
            //this.ddlStatus.DataBind();

            GenerateTree();

            this.tbStartDate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            this.tbEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        if (ModuleType == BusinessConstants.BILL_TRANS_TYPE_SO)
        {
            this.ltlPartyCode.Text = "${MasterData.Bill.Customer}:";
            this.tbPartyCode.ServicePath = "CustomerMgr.service";
            this.tbPartyCode.ServiceMethod = "GetAllCustomer";
        }

        if (this.IsSupplier)
        {
            this.tbPartyCode.ServicePath = "PartyMgr.service";
            this.tbPartyCode.ServiceMethod = "GetFromParty";
            this.tbPartyCode.ServiceParameter = "string:" + BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT + ",string:" + this.CurrentUser.Code;
        }
        var a = TheBillAddressMgr.GetAllBillAddress(false);
    }

    private IList<CodeMaster> GetStatusGroup()
    {
        IList<CodeMaster> statusGroup = new List<CodeMaster>();
        if (this.IsSupplier == null || this.IsSupplier == false)
        {
            statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));
        }
        statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT));
        if (this.IsSupplier == null || this.IsSupplier == false)
        {
            statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CANCEL));
            statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE));
            statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_VOID));
        }
        return statusGroup;
    }

    private void GenerateTree()
    {
        IList<CodeMaster> statusList = GetStatusGroup();

        foreach (CodeMaster status in statusList)
        {
            this.astvMyTree.RootNode.AppendChild(new ASTreeViewLinkNode(status.Description, status.Value, string.Empty));
        }

        this.astvMyTree.RootNode.ChildNodes[0].CheckedState = ASTreeViewCheckboxState.Checked;
        if (this.IsSupplier == null || this.IsSupplier == false)
        {
            this.astvMyTree.RootNode.ChildNodes[1].CheckedState = ASTreeViewCheckboxState.Checked;
            this.astvMyTree.InitialDropdownText = BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE + "," + BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT;
            this.astvMyTree.DropdownText = BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE + "," + BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT;
        }
        else if (this.IsSupplier == true)
        {
            this.astvMyTree.InitialDropdownText = BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT;
            this.astvMyTree.DropdownText = BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }

    protected override void DoSearch()
    {
        string code = this.tbCode.Text != string.Empty ? this.tbCode.Text.Trim() : string.Empty;
        //string status = this.ddlStatus.SelectedValue;
        List<string> statusList = new List<string>();
        string partyCode = this.tbPartyCode.Text != string.Empty ? this.tbPartyCode.Text.Trim() : string.Empty;
        string externalBillNo = this.tbExternalBillNo.Text != string.Empty ? this.tbExternalBillNo.Text.Trim() : string.Empty;
        string startDate = this.tbStartDate.Text != string.Empty ? this.tbStartDate.Text.Trim() : string.Empty;
        string endDate = this.tbEndDate.Text != string.Empty ? this.tbEndDate.Text.Trim() : string.Empty;

        if (SearchEvent != null)
        {

            #region status
            List<ASTreeViewNode> nodes = this.astvMyTree.GetCheckedNodes();
            foreach (ASTreeViewNode node in nodes)
            {
                statusList.Add(node.NodeValue);
            }
            #endregion


            #region DetachedCriteria

            DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(Bill));
            DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(Bill))
                .SetProjection(Projections.Count("BillNo"));

            selectCriteria.CreateAlias("BillAddress", "ba");
            selectCountCriteria.CreateAlias("BillAddress", "ba");
            selectCriteria.CreateAlias("ba.Party", "pf");
            selectCountCriteria.CreateAlias("ba.Party", "pf");

            if (code != string.Empty)
            {
                selectCriteria.Add(Expression.Like("BillNo", code, MatchMode.Anywhere));
                selectCountCriteria.Add(Expression.Like("BillNo", code, MatchMode.Anywhere));
            }
            if (statusList.Count > 0)
            {
                selectCriteria.Add(Expression.In("Status", statusList));
                selectCountCriteria.Add(Expression.In("Status", statusList));
            }
            else if (this.IsSupplier)
            {
                selectCriteria.Add(Expression.Not(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)));
                selectCountCriteria.Add(Expression.Not(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE)));
            }
            if (partyCode != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("pf.Code", partyCode));
                selectCountCriteria.Add(Expression.Eq("pf.Code", partyCode));
            }
            else if (this.IsSupplier)
            {
                SecurityHelper.SetPartyFromSearchCriteria(selectCriteria, selectCountCriteria, (this.tbPartyCode != null ? this.tbPartyCode.Text : null), BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT, this.CurrentUser.Code);
            }
            if (externalBillNo != string.Empty)
            {
                selectCriteria.Add(Expression.Like("ExternalBillNo", externalBillNo, MatchMode.Anywhere));
                selectCountCriteria.Add(Expression.Like("ExternalBillNo", externalBillNo, MatchMode.Anywhere));
            }
            if (startDate != string.Empty)
            {
                selectCriteria.Add(Expression.Ge("CreateDate", DateTime.Parse(startDate)));
                selectCountCriteria.Add(Expression.Ge("CreateDate", DateTime.Parse(startDate)));
            }
            if (endDate != string.Empty)
            {
                selectCriteria.Add(Expression.Lt("CreateDate", DateTime.Parse(endDate).AddDays(1).AddMilliseconds(-1)));
                selectCountCriteria.Add(Expression.Lt("CreateDate", DateTime.Parse(endDate).AddDays(1).AddMilliseconds(-1)));
            }

            selectCriteria.Add(Expression.Eq("TransactionType", ModuleType));
            selectCountCriteria.Add(Expression.Eq("TransactionType", ModuleType));

            SearchEvent((new object[] { selectCriteria, selectCountCriteria }), null);
            #endregion
        }
        this.fld_Gv_List.Visible = false;
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        NewEvent(sender, e);
        this.fld_Gv_List.Visible = false;
    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {
        if (actionParameter.ContainsKey("Code"))
        {
            this.tbCode.Text = actionParameter["Code"];
        }
    }

    public IList<CodeMaster> GetAllStatus()
    {
        IList<CodeMaster> statusGroup = new List<CodeMaster>();

        statusGroup.Add(new CodeMaster());//空行
        if (!this.IsSupplier)
        {
            statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));
        }
        statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT));
        statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CANCEL));
        statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE));
        statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_VOID));

        return statusGroup;
    }

    private CodeMaster GetStatus(string statusValue)
    {
        return TheCodeMasterMgr.GetCachedCodeMaster(BusinessConstants.CODE_MASTER_STATUS, statusValue);
    }

    protected void btnSearch_Click1(object sender, EventArgs e)
    {
        try
        {
            string itemCode = this.tbItem.Text.Trim();

            if (itemCode == string.Empty)
            {
                ShowErrorMessage("请输入物料号");
                return;
            }

            SqlParameter[] sqlParam = new SqlParameter[3];

            sqlParam[0] = new SqlParameter("@p0", "%" + itemCode + "%");

            string sql = @" select d.BillNo as 账单号,m.ExtBillNo as 发票号,p.Name as 供应商,
                            m.CreateDate as 开票日期, m.DateField1 as 发票时间,m.Status as 状态,
                            a.Item as 物料号, i.Desc1 + '['+ISNULL(i.Desc2,'')+']' as 描述,a.Uom as 单位,
                            d.UnitPrice as 单价,d.Currency as 货币,d.BilledQty as 开票数,d.Amount as 金额
                            from billdet d
                            join actbill a on a.Id = d.TransId
                            join BillMstr m on m.BillNo = d.BillNo
                            join PartyAddr pa on pa.Code = m.BillAddr
                            join Party p on p.Code = pa.PartyCode
                            join Item i on i.Code = a.Item
                            where a.Item like @p0 ";

            if (this.tbStartDate.Text.Trim() != string.Empty)
            {
                DateTime startDate = DateTime.Parse(this.tbStartDate.Text.Trim());
                sqlParam[1] = new SqlParameter("@p1", startDate);
                sql += " and m.CreateDate >=@p1 ";
            }
            if (this.tbEndDate.Text.Trim() != string.Empty)
            {
                DateTime endDate = DateTime.Parse(this.tbEndDate.Text.Trim());
                sqlParam[2] = new SqlParameter("@p2", endDate);
                sql += " and m.CreateDate <= @p2 ";
            }

            DataSet dataSet = TheSqlHelperMgr.GetDatasetBySql(sql, sqlParam);
            this.GV_List.DataSource = dataSet.Tables[0];
            this.GV_List.DataBind();
            this.fld_Gv_List.Visible = true;

            SearchEvent1(sender, e);
        }
        catch (Exception ex)
        {
            ShowErrorMessage(ex.Message);
        }
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (this.ModuleType == BusinessConstants.BILL_TRANS_TYPE_PO)
            {
                e.Row.Cells[3].Text = "供应商";
            }
            else
            {
                e.Row.Cells[3].Text = "客户";
            }
        }


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[10].Text = (double.Parse(e.Row.Cells[10].Text)).ToString("0.######");
            e.Row.Cells[12].Text = (double.Parse(e.Row.Cells[12].Text)).ToString("0.######");
            e.Row.Cells[13].Text = (double.Parse(e.Row.Cells[13].Text)).ToString("0.######");
        }
    }

}

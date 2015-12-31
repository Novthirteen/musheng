using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity;
using NHibernate.Expression;
using com.Sconit.Entity.Quote;
using Geekees.Common.Controls;
using com.Sconit.Entity.MasterData;

public partial class Quote_Quotes_Search : SearchModuleBase
{
    public EventHandler SearchEvent;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.tbPartyFrom.ServiceParameter = "string:" + BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS + ",string:" + this.CurrentUser.Code;
            this.tbPartyFrom.DataBind();

            GenerateTree();
        }
    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    { }

    protected override void DoSearch()
    {
        object[] criteriaParam = CollectParam();
        SearchEvent(criteriaParam, null);
    }

    public void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }

    private object[] CollectParam()
    {

        #region DetachedCriteria

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(ProductInfo));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(ProductInfo)).SetProjection(Projections.Count("Id"));
        if (tbPartyFrom.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("CustomerCode", tbPartyFrom.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("CustomerCode", tbPartyFrom.Text.Trim(), MatchMode.Anywhere));
        }

        if (txtProductName.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("ProductName", txtProductName.Text.Trim(), MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("ProductName", txtProductName.Text.Trim(), MatchMode.Anywhere));
        }

        if (txtStartDate.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Ge("CreateDate", DateTime.Parse(txtStartDate.Text.Trim())));
            selectCountCriteria.Add(Expression.Ge("CreateDate", DateTime.Parse(txtStartDate.Text.Trim())));
        }

        if (txtEndDate.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Le("CreateDate", DateTime.Parse(txtEndDate.Text.Trim())));
            selectCountCriteria.Add(Expression.Le("CreateDate", DateTime.Parse(txtEndDate.Text.Trim())));
        }

        //List<string> status = new List<string>();
        //List<ASTreeViewNode> nodes = this.astvMyTree.GetCheckedNodes();
        //foreach (ASTreeViewNode node in nodes)
        //{
        //    status.Add(node.NodeValue);
        //}
        //if (status != null && status.Count > 0)
        //{
        //    selectCriteria.Add(Expression.In("Status", status));
        //    selectCountCriteria.Add(Expression.In("Status", status));
        //}
        selectCriteria.Add(Expression.Eq("Status","Create"));
        selectCountCriteria.Add(Expression.Eq("Status", "Create"));
        return (new object[] { selectCriteria, selectCountCriteria });

        #endregion

    }
    private void GenerateTree()
    {
        IList<CodeMaster> statusList = GetStatusGroup(7);
        foreach (CodeMaster status in statusList)
        {
            this.astvMyTree.RootNode.AppendChild(new ASTreeViewLinkNode(status.Description, status.Value, string.Empty));
        }

        this.astvMyTree.RootNode.ChildNodes[0].CheckedState = ASTreeViewCheckboxState.Checked;
        this.astvMyTree.RootNode.ChildNodes[1].CheckedState = ASTreeViewCheckboxState.Checked;
        this.astvMyTree.RootNode.ChildNodes[2].CheckedState = ASTreeViewCheckboxState.Checked;
        this.astvMyTree.InitialDropdownText = BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE + "," + BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT + "," + BusinessConstants.CODE_MASTER_STATUS_VALUE_COMPLETE;
        this.astvMyTree.DropdownText = BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE + "," + BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT + "," + BusinessConstants.CODE_MASTER_STATUS_VALUE_COMPLETE;
    }
    private IList<CodeMaster> GetStatusGroup(int statusGroupId)
    {
        IList<CodeMaster> statusGroup = new List<CodeMaster>();
        switch (statusGroupId)
        {
            case 1:   //新建
                statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));
                statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT));
                break;
            case 2:   //发货
                statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS));
                break;
            case 3:   //收货
                statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS));
                break;
            case 4:   //All
            case 7:   //首页/订单跟踪
                statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CREATE));
                statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT));
                //statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CANCEL));
                //statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS));
                statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_COMPLETE));
                //statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE));
                break;
            case 5:   //生产上线/取消
                statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT));
                statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS));
                statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_CANCEL));
                break;
            default:
                break;
        }

        return statusGroup;
    }

    private CodeMaster GetStatus(string statusValue)
    {
        return TheCodeMasterMgr.GetCachedCodeMaster(BusinessConstants.CODE_MASTER_STATUS, statusValue);
    }
}
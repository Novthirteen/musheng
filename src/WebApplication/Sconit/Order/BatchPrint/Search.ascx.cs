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
using NHibernate.Expression;


public partial class Order_BatchPrint_Search : ModuleBase
{

    public event EventHandler SearchEvent;

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


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.StatusDataBind();
            tbRegion.ServiceParameter = "string:" + BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION + ",string:" + this.CurrentUser.Code;
            tbRegion.DataBind();
        }
    }

  

    protected void btnSearch_Click(object sender, EventArgs e)
    {

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(OrderHead));
        selectCriteria.Add(Expression.Or(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT),
            Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS)));

        if (this.tbRegion.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Eq("PartyFrom.Code", this.tbRegion.Text.Trim()));
        }
        if (this.tbOrderNo.Text.Trim() !=string.Empty)
        {
            selectCriteria.Add(Expression.Like("OrderNo", this.tbOrderNo.Text.Trim(),MatchMode.Anywhere));
        }
        if (this.tbStartDate.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Ge("StartTime",  DateTime.Parse(this.tbStartDate.Text.Trim())));
            selectCriteria.Add(Expression.Lt("StartTime", DateTime.Parse(this.tbStartDate.Text.Trim()).AddDays(1)));
        }
        if (this.ddlStatus.SelectedIndex != 0)
        {
            selectCriteria.Add(Expression.Eq("Status",ddlStatus.SelectedValue));
            selectCriteria.Add(Expression.Eq("Status", ddlStatus.SelectedValue));
        }

        #region partyFrom
        SecurityHelper.SetPartySearchCriteria(selectCriteria, "PartyFrom.Code", this.CurrentUser.Code);
        #endregion

        #region partyTo
        SecurityHelper.SetPartySearchCriteria(selectCriteria, "PartyTo.Code", this.CurrentUser.Code);
        #endregion

        #region 设置订单Type查询条件
        selectCriteria.Add(Expression.Eq("Type", this.ModuleType));
        #endregion

        SearchEvent(selectCriteria, e);

    }

    private void StatusDataBind()
    {
        this.ddlStatus.DataSource = this.GetStatusGroup();
        this.ddlStatus.DataBind();
    }
    private IList<CodeMaster> GetStatusGroup()
    {
        IList<CodeMaster> statusGroup = new List<CodeMaster>();

        statusGroup.Add(new CodeMaster()); //添加空选项
        statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT));
        statusGroup.Add(GetStatus(BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS));

        return statusGroup;
    }

    private CodeMaster GetStatus(string statusValue)
    {
        return TheCodeMasterMgr.GetCachedCodeMaster(BusinessConstants.CODE_MASTER_STATUS, statusValue);
    }
}

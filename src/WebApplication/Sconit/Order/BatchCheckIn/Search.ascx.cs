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


public partial class Order_BatchCheckIn_Search : ModuleBase
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
        this.lblFlow.Text = FlowHelper.GetFlowLabel(this.ModuleType);
        this.tbFlow.ServiceMethod = FlowHelper.GetFlowServiceMethod(this.ModuleType);
        this.tbFlow.ServiceParameter = "string:" + this.CurrentUser.Code;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(OrderHead));
        selectCriteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_SUBMIT));

      
        if (this.tbOrderNo.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Like("OrderNo", this.tbOrderNo.Text.Trim(), MatchMode.Anywhere));
        }
        if (this.tbFlow.Text.Trim() != string.Empty)
        {
            selectCriteria.Add(Expression.Ge("Flow", this.tbFlow.Text.Trim()));
            selectCriteria.Add(Expression.Ge("Flow", this.tbFlow.Text.Trim()));
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
    protected void InitPageParameter(IDictionary<string, string> actionParameter)
    {
        //todo
    }
   

}

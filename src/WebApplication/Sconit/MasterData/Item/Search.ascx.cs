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
using System.Collections.Generic;
using NHibernate.Expression;
using com.Sconit.Entity.MasterData;
using com.Sconit.Web;
using com.Sconit.Service.Ext.MasterData;

public partial class MasterData_Item_Search : SearchModuleBase
{
    public event EventHandler SearchEvent;
    public event EventHandler NewEvent;
    public event EventHandler BtnImportClick;
    public event EventHandler ExportEvent;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {

        if (actionParameter.ContainsKey("Code"))
        {
            this.tbCode.Text = actionParameter["Code"];
        }
        if (actionParameter.ContainsKey("Desc"))
        {
            this.tbDesc.Text = actionParameter["Desc"];
        }
    }

    private object[] CollectParam()
    {
        string code = this.tbCode.Text.Trim();
        string desc = this.tbDesc.Text.Trim();
        bool isActive = this.cbIsActive.Checked;
        bool showImage = this.cbShowImage.Checked;


        #region DetachedCriteria

        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(Item));
        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(Item)).SetProjection(Projections.Count("Code"));
        if (code != string.Empty)
        {
            selectCriteria.Add(Expression.Like("Code", code, MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("Code", code, MatchMode.Anywhere));
        }

        if (desc != string.Empty)
        {
            selectCriteria.Add(Expression.Or(Expression.Like("Desc1", desc, MatchMode.Anywhere),Expression.Like("Desc2",desc,MatchMode.Anywhere)));
            selectCountCriteria.Add(Expression.Or(Expression.Like("Desc1", desc, MatchMode.Anywhere), Expression.Like("Desc2", desc, MatchMode.Anywhere)));
        }
        selectCriteria.Add(Expression.Eq("IsActive", isActive));
        selectCountCriteria.Add(Expression.Eq("IsActive", isActive));

        return (new object[] { selectCriteria, selectCountCriteria, showImage });

        #endregion

    }

    protected override void DoSearch()
    {
        if (SearchEvent != null)
        {
            object[] criteriaParam = CollectParam();
            if (criteriaParam != null)
            {
                SearchEvent(criteriaParam, null);
            }
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        NewEvent(sender, e);
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        if (BtnImportClick != null)
        {
            BtnImportClick(sender, e);
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        /*
        if (ExportEvent != null)
        {
            object[] param = this.CollectParam();
            if (param != null)
            {
                ExportEvent(param, null);
            }
        }
        */
        object[] param = this.CollectParam();

        IList<Item> itemList = this.TheCriteriaMgr.FindAll<Item>((DetachedCriteria)param[0]);
        IList<object> list = new List<object>();
        if (itemList != null && itemList.Count > 0)
        {
            list.Add(itemList);
        }
        TheReportMgr.WriteToClient("ItemSample.xls", list, "ItemSample.xls");
    }
}

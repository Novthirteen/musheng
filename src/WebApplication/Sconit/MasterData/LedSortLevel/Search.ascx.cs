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
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.MasterData;
using com.Sconit.Service.Criteria;
using com.Sconit.Web;
using com.Sconit.Entity;
using NHibernate.Expression;
using System.Collections.Generic;
using com.Sconit.Utility;
using com.Sconit.Entity.Customize;

public partial class MasterData_LedSortLevel_Search : SearchModuleBase
{
    public event EventHandler SearchEvent;
    public event EventHandler NewEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {
       
    }

    protected override void DoSearch()
    {
        string itemBrand = this.ddlItemBrand.Text != string.Empty ? this.ddlItemBrand.Text.Trim() : string.Empty;
        string itemCode = this.tbItemCode.Text != string.Empty ? this.tbItemCode.Text.Trim() : string.Empty;

        if (SearchEvent != null)
        {
            #region DetachedCriteria

            DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(LedSortLevel));
            DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(LedSortLevel))
                .SetProjection(Projections.Count("Id"));

            if (itemCode != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("Item.Code", itemCode));
                selectCountCriteria.Add(Expression.Eq("Item.Code", itemCode));
            }

            if (itemBrand != string.Empty)
            {
                selectCriteria.Add(Expression.Eq("Brand.Code", itemBrand));
                selectCountCriteria.Add(Expression.Eq("Brand.Code", itemBrand));
            }


            SearchEvent((new object[] { selectCriteria, selectCountCriteria }), null);
            #endregion
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        NewEvent(sender, e);
    }
}

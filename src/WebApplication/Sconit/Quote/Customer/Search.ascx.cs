using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using Geekees.Common.Controls;
using com.Sconit.Entity;

public partial class Quote_Customer_Search : SearchModuleBase
{
    public event EventHandler SearchEvent;
    public event EventHandler NewEvent;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DoSearch();
    }

    protected override void DoSearch()
    {
        string code = this.tbCode.Text.Trim();
        string name = this.tbName.Text.Trim();
        bool isTrue = cbTrue.Checked;
        bool isFalse = cbFalse.Checked;
        SearchEvent(new object[] { code, name, isTrue, isFalse }, null);
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        NewEvent(sender, e);
    }

    protected override void InitPageParameter(IDictionary<string, string> actionParameter)
    {
    }

}
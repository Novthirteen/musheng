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
using NHibernate.Expression;
using com.Sconit.Web;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;

public partial class Cost_CostGroup_EditMain : MainModuleBase
{
    public event EventHandler BackEvent;

    protected string Code
    {
        get
        {
            return (string)ViewState["Code"];
        }
        set
        {
            ViewState["Code"] = value;
        }
    }

    public void InitPageParameter(string code)
    {
        this.Code = code;
        this.ucEdit.InitPageParameter(code);
        this.ucTabNavigator.UpdateView();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ucTabNavigator.lbCostCenterClickEvent += new System.EventHandler(this.TabCostCenterClick_Render);
        this.ucTabNavigator.lbCostGroupClickEvent += new System.EventHandler(this.TabCostGroupClick_Render);
        this.ucEdit.BackEvent += new System.EventHandler(this.Back_Render);
        this.ucCostCenter.BackEvent += new System.EventHandler(this.Back_Render);
    }

    protected void Back_Render(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void TabCostGroupClick_Render(object sender, EventArgs e)
    {
        this.ucEdit.Visible = true;
        this.ucCostCenter.Visible = false;
        this.ucEdit.InitPageParameter(Code);
        
    }

    protected void TabCostCenterClick_Render(object sender, EventArgs e)
    {
        this.ucEdit.Visible = false;
        this.ucCostCenter.Visible = true;
        this.ucCostCenter.CostGroupCode = this.Code;
        this.ucCostCenter.InitPageParameter();
    }

   
}

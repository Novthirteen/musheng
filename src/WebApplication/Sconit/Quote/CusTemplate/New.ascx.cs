using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using NCalc;

public partial class Quote_CusTemplate_New : NewModuleBase
{
    protected string Id
    {
        get
        {
            return (string)ViewState["Id"];
        }
        set
        {
            ViewState["Id"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    public void InitPageParameter(string id)
    {
        Id = id;
        cblCostList.DataSource = TheToolingMgr.GetCostListByCCId(Id);
        cblCostList.DataTextField = "Name";
        cblCostList.DataValueField = "Id";
        cblCostList.DataBind();
    }

    public void  btnSave_Click(object sender, EventArgs e)
    {
        //string a = string.Empty;
        //for(int i = 0;i<cblCostList.Items.Count;i++)
        //{
        //    if(cblCostList.Items[i].Selected)
        //    {
        //        a += cblCostList.Items[i].Value;
        //    }
        //}
        //Response.Write(a);
        Expression ee = new Expression("2 + 3 * 5");
        Response.Write(ee.Evaluate());
    }

    public void btnBack_Click(object sender, EventArgs e)
    { }
}
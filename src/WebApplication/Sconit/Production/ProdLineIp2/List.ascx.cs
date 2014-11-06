using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity;
using com.Sconit.Service.Customize;
using com.Sconit.Service.Customize.Impl;
using com.Sconit.Entity.Customize;

public partial class Production_ProdLineIp2_List : ListModuleBase
{

    protected IProdLineIp2Mgr TheProdLineIp2Mgr { get { return GetService<IProdLineIp2Mgr>("ProdLineIp2Mgr.service"); } }


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

    public override void UpdateView()
    {
        this.GV_List.Execute();
    }

    public void Export()
    {
        this.ExportXLS(GV_List);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    protected void lbtnView_Click(object sender, EventArgs e)
    {
        this.ucdetail.Visible = true;

        ProdLineIp2 prodLineIp2 = this.TheProdLineIp2Mgr.LoadProdLineIp2(int.Parse(((LinkButton)sender).CommandArgument));
        this.ucdetail.InitPageParameter(prodLineIp2.OrderNo, prodLineIp2.FG, prodLineIp2.Item);
    }
}

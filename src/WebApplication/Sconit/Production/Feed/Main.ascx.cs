using System;
using System.Collections;
using System.Collections.Generic;
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
using com.Sconit.Entity;
using NHibernate.Expression;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Distribution;
using com.Sconit.Service.Ext.Distribution;

public partial class Production_Feed_Main : MainModuleBase
{

    private Resolver resolver
    {
        get { return (Resolver)ViewState["CacheResolver"]; }
        set { ViewState["CacheResolver"] = value; }
    }

    public Production_Feed_Main()
    {

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.resolver = new Resolver();
            this.resolver.UserCode = this.CurrentUser.Code;
            this.resolver.ModuleType = BusinessConstants.TRANSFORMER_MODULE_TYPE_MATERIALIN;
            this.resolver.Transformers = null;
            this.resolver.Result = string.Empty;
            this.resolver.BinCode = string.Empty;
            this.resolver.Code = string.Empty;
            this.resolver.CodePrefix = string.Empty;
        }
        this.tbProductLine.ServiceParameter = "string:" + this.CurrentUser.Code;
        this.divInputHu.Visible = this.resolver.IsScanHu;
    }

    protected void tbProductLine_TextChanged(object sender, EventArgs e)
    {
        if (this.tbProductLine.Text.Trim() == string.Empty)
        {
            ShowErrorMessage("MasterData.Feed.ProdLine.Empty");
            return;
        }

        this.resolver.Input = BusinessConstants.BARCODE_SPECIAL_MARK + BusinessConstants.BARCODE_HEAD_FLOW + this.tbProductLine.Text.Trim();
        this.UpdateView();
        this.divInputHu.Visible = this.resolver.IsScanHu;
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        this.resolver.Input = BusinessConstants.BARCODE_SPECIAL_MARK + BusinessConstants.BARCODE_HEAD_OK;
        if (this.resolver.Transformers != null)
        {
            for (int i = 0; i < this.resolver.Transformers.Count; i++)
            {
                GridViewRow row = this.GV_List.Rows[i];

                TextBox tbCurrentQty = (TextBox)row.FindControl("tbCurrentQty");
                this.resolver.Transformers[i].CurrentQty = decimal.Parse(tbCurrentQty.Text.Trim());
            }
        }
        this.resolver = TheResolverMgr.Resolve(this.resolver);
        this.resolver.Input = BusinessConstants.BARCODE_SPECIAL_MARK + BusinessConstants.BARCODE_HEAD_FLOW + this.tbProductLine.Text.Trim();
        this.UpdateView();
    }

    protected void tbHuScan_TextChanged(object sender, EventArgs e)
    {
        foreach (var t in this.resolver.Transformers)
        {
            if (t.TransformerDetails == null)
                t.TransformerDetails = new List<TransformerDetail>();
        }
        this.resolver.Input = this.tbHuScan.Text.Trim();
        this.UpdateView();
    }

    private void UpdateView()
    {
        try
        {
            this.resolver = TheResolverMgr.Resolve(this.resolver);
        }
        catch (Exception ex)
        {
            this.lblMessage.Text = TheLanguageMgr.TranslateMessage(ex.Message, this.CurrentUser, this.tbHuScan.Text);
        }

        this.GV_List.DataSource = this.resolver.Transformers;
        this.GV_List.DataBind();
        this.tbHuScan.Text = string.Empty;
    }
}

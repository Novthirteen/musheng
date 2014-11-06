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
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Web;
using com.Sconit.Control;
using com.Sconit.Entity.Customize;
using System.Collections.Generic;

public partial class Production_PLFeedSeq_New : NewModuleBase
{
    public event EventHandler BackEvent;
    public event EventHandler CreateEvent;
    public event EventHandler NewEvent;

    private ProdutLineFeedSeqence produtLineFeedSeqence;
    protected void Page_Load(object sender, EventArgs e)
    {
        Controls_TextBox tbProductLineCode = ((Controls_TextBox)this.FV_ProdutLineFeedSeqence.FindControl("tbProductLineCode"));
        //tbProductLineFacility.ServiceParameter = "string:" + this.CurrentUser.Code;
        //tbProductLineFacility.DataBind();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void FV_ProdutLineFeedSeqence_OnDataBinding(object sender, EventArgs e)
    {
       
    }

    public void PageCleanup()
    {
        ((TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbProductLineFacility"))).Text = string.Empty;
        //((CheckBox)(this.FV_ProdutLineFeedSeqence.FindControl("cbIsActive"))).Checked = true;
        ((TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbSequence"))).Text = string.Empty;
        ((Controls_TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbFinishGoodCode"))).Text = string.Empty;
        ((Controls_TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbRawMaterialCode"))).Text = string.Empty;
    }

    protected void ODS_ProdutLineFeedSeqence_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        produtLineFeedSeqence = (ProdutLineFeedSeqence)e.InputParameters[0];
        if (produtLineFeedSeqence != null)
        {
            TextBox tbProductLineFacility = (TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbProductLineFacility"));
            Controls_TextBox tbFinishGoodCode = (Controls_TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbFinishGoodCode"));
            Controls_TextBox tbRawMaterialCode = (Controls_TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbRawMaterialCode"));
            if (tbProductLineFacility != null && tbProductLineFacility.Text.Trim() != string.Empty)
            {
                produtLineFeedSeqence.ProductLineFacility = tbProductLineFacility.Text.Trim();
            }
            if (tbFinishGoodCode != null && tbFinishGoodCode.Text.Trim() != string.Empty)
            {
                produtLineFeedSeqence.FinishGood = TheItemMgr.LoadItem(tbFinishGoodCode.Text.Trim());
            }
            if (tbRawMaterialCode != null && tbRawMaterialCode.Text.Trim() != string.Empty)
            {
                produtLineFeedSeqence.RawMaterial = TheItemMgr.LoadItem(tbRawMaterialCode.Text.Trim());
            }
            DateTime now = DateTime.Now;
            produtLineFeedSeqence.CreateUser = this.CurrentUser.Code;
            produtLineFeedSeqence.CreateDate = now;
            produtLineFeedSeqence.LastModifyUser = this.CurrentUser.Code;
            produtLineFeedSeqence.LastModifyDate = now;
            produtLineFeedSeqence.IsActive = true;
        }
    }

    protected void ODS_ProdutLineFeedSeqence_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (CreateEvent != null)
        {
            CreateEvent(produtLineFeedSeqence.Id, e);
            ShowSuccessMessage("Production.ProdutLineFeedSeqence.AddProdutLineFeedSeqence.Successfully", produtLineFeedSeqence.Sequence.ToString());
        }
    }


    protected void checkSequenceExists(object source, ServerValidateEventArgs args)
    {
        int sequence = int.Parse(((TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbSequence"))).Text.Trim());
        String productLineFacility = ((TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbProductLineFacility"))).Text.Trim();
        String finishGood = ((Controls_TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbFinishGoodCode"))).Text.Trim();

        IList<ProdutLineFeedSeqence> produtLineFeedSeqenceList = TheProdutLineFeedSeqenceMgr.GetProdutLineFeedSeqence(productLineFacility, finishGood, sequence);
        if (produtLineFeedSeqenceList != null && produtLineFeedSeqenceList.Count > 0)
        {
            foreach (ProdutLineFeedSeqence produtLineFeedSeqence in produtLineFeedSeqenceList)
            {
                if (produtLineFeedSeqence.Sequence == sequence)
                {
                    args.IsValid = false;
                    break;
                }
            }
        }
    }

    protected void checkCodeExists(object source, ServerValidateEventArgs args)
    {
        String code = ((TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbCode"))).Text.Trim();
        String productLineFacility = ((TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbProductLineFacility"))).Text.Trim();
        String finishGood = ((Controls_TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbFinishGoodCode"))).Text.Trim();

        IList<ProdutLineFeedSeqence> produtLineFeedSeqenceList = TheProdutLineFeedSeqenceMgr.GetProdutLineFeedSeqence(productLineFacility, finishGood, code);
        if (produtLineFeedSeqenceList != null && produtLineFeedSeqenceList.Count > 0)
        {
            foreach (ProdutLineFeedSeqence produtLineFeedSeqence in produtLineFeedSeqenceList)
            {
                if (produtLineFeedSeqence.Code == code)
                {
                    args.IsValid = false;
                    break;
                }
            }
        }
    }

}

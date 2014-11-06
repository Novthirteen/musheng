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
using com.Sconit.Entity.MasterData;
using com.Sconit.Control;
using com.Sconit.Entity.Customize;
using System.Collections.Generic;

public partial class Production_PLFeedSeq_Edit : EditModuleBase
{
    public event EventHandler BackEvent;

    protected int Id
    {
        get
        {
            if (ViewState["Id"] == null)
            {
                return 0;
            }
            else
            {
                return (Int32)ViewState["Id"];
            }
        }
        set
        {
            ViewState["Id"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void FV_ProdutLineFeedSeqence_DataBound(object sender, EventArgs e)
    {

        ProdutLineFeedSeqence produtLineFeedSeqence = TheProdutLineFeedSeqenceMgr.LoadProdutLineFeedSeqence(this.Id);

        ((Controls_TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbRawMaterialCode"))).Text = produtLineFeedSeqence.RawMaterial.Code;
        ((Controls_TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbFinishGoodCode"))).Text = produtLineFeedSeqence.FinishGood.Code;
    }

    public void InitPageParameter(Int32 id)
    {
        this.Id = id;
        this.ODS_ProdutLineFeedSeqence.SelectParameters["id"].DefaultValue = this.Id.ToString();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void ODS_ProdutLineFeedSeqence_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ProdutLineFeedSeqence produtLineFeedSeqence = TheProdutLineFeedSeqenceMgr.LoadProdutLineFeedSeqence(this.Id);

        ShowSuccessMessage("Production.ProdutLineFeedSeqence.UpdateProdutLineFeedSeqence.Successfully",produtLineFeedSeqence.Sequence.ToString());
        btnBack_Click(this, e);
    }

    protected void ODS_ProdutLineFeedSeqence_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        ProdutLineFeedSeqence produtLineFeedSeqence = (ProdutLineFeedSeqence)e.InputParameters[0];

        if (produtLineFeedSeqence != null)
        {
            produtLineFeedSeqence.Id = this.Id;

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

            ProdutLineFeedSeqence produtLineFeedSeqenceT = TheProdutLineFeedSeqenceMgr.LoadProdutLineFeedSeqence(this.Id);
            produtLineFeedSeqence.CreateDate = produtLineFeedSeqenceT.CreateDate;
            produtLineFeedSeqence.CreateUser = produtLineFeedSeqenceT.CreateUser;

            produtLineFeedSeqence.LastModifyUser = this.CurrentUser.Code;
            produtLineFeedSeqence.LastModifyDate = DateTime.Now;

            produtLineFeedSeqence.IsActive = true;
        }

    }

    protected void ODS_ProdutLineFeedSeqence_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception == null)
        {
            btnBack_Click(this, e);
            ShowSuccessMessage("Production.ProdutLineFeedSeqence.DeleteProdutLineFeedSeqence.Successfully", ((TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbSequence"))).Text);
        }
        else if (e.Exception.InnerException is Castle.Facilities.NHibernateIntegration.DataException)
        {
            ShowErrorMessage("Production.ProdutLineFeedSeqence.DeleteProdutLineFeedSeqence.Fail", ((TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbSequence"))).Text);
            e.ExceptionHandled = true;
        }
    }

    protected void checkSequenceExists(object source, ServerValidateEventArgs args)
    {
        int id = int.Parse(((HiddenField)(this.FV_ProdutLineFeedSeqence.FindControl("hfId"))).Value.Trim());
        int sequence = int.Parse(((TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbSequence"))).Text.Trim());
        String productLineFacility = ((TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbProductLineFacility"))).Text.Trim();
        String finishGood = ((Controls_TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbFinishGoodCode"))).Text.Trim();

        IList<ProdutLineFeedSeqence> produtLineFeedSeqenceList = TheProdutLineFeedSeqenceMgr.GetProdutLineFeedSeqence(productLineFacility, finishGood, sequence);
        if (produtLineFeedSeqenceList != null && produtLineFeedSeqenceList.Count > 0)
        {
            foreach (ProdutLineFeedSeqence produtLineFeedSeqence in produtLineFeedSeqenceList)
            {
                if (produtLineFeedSeqence.Sequence == sequence && id != produtLineFeedSeqence.Id)
                {
                    args.IsValid = false;
                    break;
                }
            }
        }
    }

    protected void checkCodeExists(object source, ServerValidateEventArgs args)
    {
        int id = int.Parse(((HiddenField)(this.FV_ProdutLineFeedSeqence.FindControl("hfId"))).Value.Trim());
        String code = ((TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbCode"))).Text.Trim();
        String productLineFacility = ((TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbProductLineFacility"))).Text.Trim();
        String finishGood = ((Controls_TextBox)(this.FV_ProdutLineFeedSeqence.FindControl("tbFinishGoodCode"))).Text.Trim();

        IList<ProdutLineFeedSeqence> produtLineFeedSeqenceList = TheProdutLineFeedSeqenceMgr.GetProdutLineFeedSeqence(productLineFacility, finishGood, code);
        if (produtLineFeedSeqenceList != null && produtLineFeedSeqenceList.Count > 0)
        {
            foreach (ProdutLineFeedSeqence produtLineFeedSeqence in produtLineFeedSeqenceList)
            {
                if (produtLineFeedSeqence.Code == code && id != produtLineFeedSeqence.Id)
                {
                    args.IsValid = false;
                    break;
                }
            }
        }
    }
}

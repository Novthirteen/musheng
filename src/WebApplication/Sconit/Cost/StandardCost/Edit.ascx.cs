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
using com.Sconit.Utility;
using com.Sconit.Entity.Cost;

public partial class Cost_StandardCost_Edit : EditModuleBase
{
    public event EventHandler BackEvent;

    protected Int32 Id
    {
        get
        {
            return (Int32)ViewState["Id"];
        }
        set
        {
            ViewState["Id"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }


    public void InitPageParameter(Int32 id)
    {
        this.Id = id;
        this.ODS_StandardCost.SelectParameters["id"].DefaultValue = this.Id.ToString();
        this.ODS_StandardCost.DataBind();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }
    protected void FV_StandardCost_DataBound(object sender, EventArgs e)
    {
        StandardCost standardCost = (StandardCost)((FormView)sender).DataItem;
        
        if (standardCost.CostElement != null)
        {
            ((Label)(this.FV_StandardCost.FindControl("tbCostElement"))).Text = standardCost.CostElement.Code;
        }
        if (standardCost.CostGroup != null)
        {
            ((Label)(this.FV_StandardCost.FindControl("tbCostGroup"))).Text = standardCost.CostGroup.Code;
        }
       
    }
    protected void ODS_StandardCost_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        StandardCost standardCost = (StandardCost)e.InputParameters[0];
        StandardCost oldStandardCost = TheStandardCostMgr.LoadStandardCost(standardCost.Id);

        CloneHelper.CopyProperty(oldStandardCost, standardCost);
        standardCost.Cost = Decimal.Parse(((TextBox)(this.FV_StandardCost.FindControl("tbCost"))).Text.Trim());
        standardCost.LastModifyDate = DateTime.Now;
        standardCost.LastModifyUser = this.CurrentUser;
        
    }
    protected void ODS_StandardCost_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ShowSuccessMessage("Cost.StandardCost.Update.Successfully");
    }

    protected void ODS_StandardCost_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            TheStandardCostMgr.DeleteStandardCost(this.Id);
            ShowSuccessMessage("Cost.StandardCost.Delete.Successfully");
            if (BackEvent != null)
            {
                BackEvent(this, e);
            }
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Cost.StandardCost.Delete.Failed");
        }
    }

}

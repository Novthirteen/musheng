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
using NHibernate.Expression;
using System.Collections.Generic;

public partial class Cost_CostAllocateTransaction_View : EditModuleBase
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
        CostAllocateTransaction costAllocateTransaction = TheCostAllocateTransactionMgr.LoadCostAllocateTransaction(id);

        this.tbCostElement.Text = costAllocateTransaction.CostElement.Description;
        this.tbCostCenter.Text = costAllocateTransaction.CostCenter.Description;
        //this.ddlAllocateBy.Text = costAllocateTransaction.AllocateBy;

        this.tbDependCostElement.Text = costAllocateTransaction.DependCostElement.Description;
        this.tbExpenseElement.Text = costAllocateTransaction.ExpenseElement.Description;
        this.tbEffDate.Text = costAllocateTransaction.EffectiveDate.ToShortDateString();
        this.tbItemCategorys.Text = costAllocateTransaction.ItemCategorys;
        this.tbItems.Text = costAllocateTransaction.Items;
        this.tbOrders.Text = costAllocateTransaction.Orders;
        this.tbAmount.Text = costAllocateTransaction.Amount.ToString("0.########");
        this.tbReferenceItems.Text = costAllocateTransaction.ReferenceItems;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

}

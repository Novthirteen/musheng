using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.Quote;
using com.Sconit.Entity.MasterData;

public partial class Quote_Item_PriceList : ModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void InitPageParameter(IList<QuoteItem> list)
    {
        this.GV_List.DataSource = list;
        this.GV_List.DataBind();
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox tbItemCode = (TextBox)e.Row.FindControl("txtItemCode");

            TextBox txtSN = ((TextBox)e.Row.FindControl("txtSingleNum"));
            //txtSN.Attributes.Add("onkeyup", "txtSingleNumChange(this);");

            TextBox txtPP1 = ((TextBox)e.Row.FindControl("txtPurchasePrice1"));

            TextBox txtPP = ((TextBox)e.Row.FindControl("txtPurchasePrice"));
            txtPP.Attributes.Add("onkeyup", "txtPurchasePriceChange(this);");

            IList<EntityPreference> entityPreferences = TheEntityPreferenceMgr.GetAllEntityPreference();
            string priceCode = string.Empty;
            if (entityPreferences != null && entityPreferences.Count > 0)
            {
                foreach (EntityPreference ep in entityPreferences)
                {
                    if (ep.Code == "QuotePrice")
                    {
                        priceCode = ep.Value;
                        break;
                    }
                }
            }
            PriceList priceList = ThePriceListMgr.LoadPriceList(priceCode, true);
            foreach (PriceListDetail pd in priceList.PriceListDetails)
            {
                if (pd.Item.Code == tbItemCode.Text)
                {
                    if ((pd.StartDate == null ? DateTime.MinValue : pd.StartDate) <= DateTime.Now && (pd.EndDate == null ? DateTime.MaxValue : pd.EndDate) >= DateTime.Now)
                    {
                        txtPP1.Text = pd.UnitPrice.ToString();
                    }
                }
            }
            if(txtPP1.Text.Trim() == string.Empty)
            {
                txtPP1.Text = "0";
            }
            //if(txtPP.Text.Trim() == string.Empty)
            //txtPP.Text = txtPP1.Text;

            TextBox txtP = ((TextBox)e.Row.FindControl("txtPrice"));
            txtP.Text = (decimal.Parse(txtSN.Text) * decimal.Parse(txtPP.Text)).ToString();
        }
    }

    public IList<QuoteItem> GetQuoteItem()
    {
        IList<QuoteItem> qiList = new List<QuoteItem>();
        foreach(GridViewRow row in this.GV_List.Rows)
        {
            QuoteItem qi = new QuoteItem();
            qi.ItemCode = ((TextBox)row.FindControl("txtItemCode")).Text;
            qi.PurchasePrice = ((TextBox)row.FindControl("txtPurchasePrice")).Text == "" ? 0 : decimal.Parse(((TextBox)row.FindControl("txtPurchasePrice")).Text);
            qiList.Add(qi);
        }
        return qiList;
    }
}
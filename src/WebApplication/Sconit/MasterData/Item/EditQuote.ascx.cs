using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.Quote;

public partial class MasterData_Item_EditQuote : EditModuleBase
{
    public EventHandler BackEvent;
    protected string ItemCode
    {
        get
        {
            return (string)ViewState["ItemCode"];
        }
        set
        {
            ViewState["ItemCode"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void InitPageParameter(string itemCode)
    {
        CleanControl();

        ItemCode = itemCode;
        this.txtItemCode.Text = itemCode;

        IList<QuoteItem> QuoteItemList = TheToolingMgr.GetQuoteItemByCode(itemCode);
        if (QuoteItemList.Count > 0)
        {
            QuoteItem qt = QuoteItemList[0];
            txtBrand.Text = qt.Brand;
            txtCategory.Text = qt.Category;
            txtModel.Text = qt.Model;
            txtPinConversion.Text = qt.PinConversion;
            txtPinNum.Text = qt.PinNum;
            txtPoint.Text = qt.Point;
            txtPrice.Text = qt.Price.ToString();
            txtPurchasePrice.Text = qt.PurchasePrice.ToString();
            txtSingleNum.Text = qt.SingleNum;
            txtSupplier.Text = qt.Supplier;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        QuoteItem qt = new QuoteItem();
        qt.Brand = txtBrand.Text.Trim();
        qt.Category = txtCategory.Text.Trim();
        qt.ItemCode = txtItemCode.Text.Trim();
        qt.Model = txtModel.Text.Trim();
        qt.PinConversion = txtPinConversion.Text.Trim();
        qt.PinNum = txtPinNum.Text.Trim();
        qt.Point = txtPoint.Text.Trim();
        if (txtPrice.Text.Trim() != string.Empty)
        {
            qt.Price = decimal.Parse(txtPrice.Text.Trim());
        }
        if (txtPurchasePrice.Text.Trim() != string.Empty)
        {
            qt.PurchasePrice = decimal.Parse(txtPurchasePrice.Text.Trim());
        }
        qt.SingleNum = txtSingleNum.Text;
        qt.Supplier = txtSupplier.Text.Trim();

        try
        {
            IList<QuoteItem> QuoteItemList = TheToolingMgr.GetQuoteItemByCode(ItemCode);
            if (QuoteItemList.Count > 0)
            {
                TheToolingMgr.UpdateQuoteItem(qt);
            }
            else
            {
                TheToolingMgr.CreateQuoteItem(qt);
            }
            ShowSuccessMessage("Common.Business.Result.Save.Successfully");
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Common.Business.Result.Save.Failed");
        }
    }

    public void CleanControl()
    {
        foreach (Control ctl in this.Controls)
        {
            if (ctl is TextBox)
            {
                ((TextBox)ctl).Text = string.Empty;
            }
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        BackEvent(sender, e);
    }
}
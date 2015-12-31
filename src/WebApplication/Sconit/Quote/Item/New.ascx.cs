using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.Quote;
using NCalc;

public partial class Quote_Item_New : NewModuleBase
{
    public EventHandler BackEvent;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        QuoteItem qt = new QuoteItem();
        qt.ProjectId = txtProjectId.Text.Trim();
        qt.ItemCode = txtItemCode.Text.Trim();
        qt.Brand = txtBrand.Text.Trim();
        qt.Category = txtCategory.Text.Trim();
        qt.ItemCode = txtItemCode.Text.Trim();
        qt.Model = txtModel.Text.Trim();
        qt.PinConversion = GetPinConversion();
        txtPinConversion.Text = qt.PinConversion;
        qt.PinNum = txtPinNum.Text.Trim();
        qt.Point = GetPoint();
        txtPoint.Text = qt.Point;
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
            TheToolingMgr.CreateQuoteItem(qt);
            ShowSuccessMessage("Common.Business.Result.Save.Successfully");
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Common.Business.Result.Save.Failed");
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        BackEvent(sender, e);
    }

    public string GetPinConversion()
    {
        try
        {
            string str = "if('" + txtItemCode.Text.Trim() + "'='' or '" + txtPinNum.Text.Trim() + "'='' or '" + txtSingleNum.Text.Trim() + "'='','',if('" + txtCategory.Text.Trim() + "'='DIP'," + txtPinNum.Text.Trim() + ",if(" + txtPinNum.Text.Trim() + "<3,1," + Math.Ceiling(decimal.Parse((Int32.Parse(txtPinNum.Text.Trim()) / 4).ToString())) + ")))";
            return new Expression(str).Evaluate().ToString();
        }
        catch
        {
            return "";
        }
    }

    public string GetPoint()
    {
        try
        {
            string str = "if('" + txtItemCode.Text.Trim() + "'='' or '" + txtPinNum.Text.Trim() + "'='' or '" + txtSingleNum.Text.Trim() + "'='',''," + txtSingleNum.Text.Trim() + "*" + txtPinConversion.Text.Trim()+")";
            return new Expression(str).Evaluate().ToString();
        }
        catch
        {
            return "";
        }
    }
}
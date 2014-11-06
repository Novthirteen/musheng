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

public partial class MasterData_Currency_CurrencyExchange : ModuleBase
{
    private string[] itemMessage = new string[4];

    protected void GV_CurrencyExchange_OnDataBind(object sender, EventArgs e)
    {
        this.fldGV.Visible = true;
        if (((GridView)(sender)).Rows.Count == 0)
            this.lblResult.Visible = true;
        else
            this.lblResult.Visible = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.GV_CurrencyExchange.DataBind();
        this.fldSearch.Visible = true;
        this.fldInsert.Visible = false;
        this.fldGV.Visible = true;
    }

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        this.fldInsert.Visible = true;
        ((Controls_TextBox)(this.FV_CurrencyExchange.FindControl("tbBaseCurrency"))).Text = string.Empty;
        ((Controls_TextBox)(this.FV_CurrencyExchange.FindControl("tbAltCurrency"))).Text = string.Empty;
        ((TextBox)(this.FV_CurrencyExchange.FindControl("tbBaseQty"))).Text = string.Empty;
        ((TextBox)(this.FV_CurrencyExchange.FindControl("tbAltQty"))).Text = string.Empty;
        ((TextBox)(this.FV_CurrencyExchange.FindControl("tbStartDate"))).Text = string.Empty;
        ((TextBox)(this.FV_CurrencyExchange.FindControl("tbEndDate"))).Text = string.Empty;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.fldSearch.Visible = true;
        this.fldGV.Visible = true;
        this.fldInsert.Visible = false;
        // this.GV_UomConversion.DataBind();
    }

    protected void ODS_GV_CurrencyExchange_OnUpdating(object source, ObjectDataSourceMethodEventArgs e)
    {
        CurrencyExchange currencyExchange = (CurrencyExchange)e.InputParameters[0];
        CurrencyExchange oldExchange = TheCurrencyExchangeMgr.LoadCurrencyExchange(currencyExchange.Id);
        currencyExchange.BaseCurrency = oldExchange.BaseCurrency;
        currencyExchange.ExchangeCurrency = oldExchange.ExchangeCurrency;
        currencyExchange.StartDate = oldExchange.StartDate;

        itemMessage[0] = currencyExchange.BaseQty.ToString();
        itemMessage[1] = oldExchange.BaseCurrency;
        itemMessage[2] = currencyExchange.ExchangeQty.ToString();
        itemMessage[3] = oldExchange.ExchangeCurrency;
        ShowSuccessMessage("MasterData.CurrencyExchang.UpdateCurrencyExchang.Successfully", itemMessage);
    }



    protected void ODS_CurrencyExchange_Inserting(object source, ObjectDataSourceMethodEventArgs e)
    {
        CurrencyExchange currencyExchange = (CurrencyExchange)e.InputParameters[0];

        string exchangeCurrency = ((Controls_TextBox)(this.FV_CurrencyExchange.FindControl("tbAltCurrency"))).Text;
        string baseCurrency = ((Controls_TextBox)(this.FV_CurrencyExchange.FindControl("tbBaseCurrency"))).Text;
        string endDate = ((TextBox)(this.FV_CurrencyExchange.FindControl("tbEndDate"))).Text.ToString().Trim();

        itemMessage[0] = currencyExchange.BaseQty.ToString();
        itemMessage[1] = baseCurrency;
        itemMessage[2] = currencyExchange.ExchangeQty.ToString();
        itemMessage[3] = exchangeCurrency;

        //if (itemCode == null || itemCode.Trim() == string.Empty)
        //{
        //    ShowWarningMessage("MasterData.UomConversion.Required.itemCode", "");
        //    e.Cancel = true;
        //    return;
        //}
        //if (exchangeCurrency == null || exchangeCurrency.Trim() == string.Empty)
        //{
        //    ShowWarningMessage("MasterData.CurrencyExchang.Required.ExchangeCurrency", exchangeCurrency);
        //    e.Cancel = true;
        //    return;
        //}
        if (exchangeCurrency == baseCurrency)
        {
            ShowWarningMessage("MasterData.CurrencyExchang.Same.CurrencyExchang", baseCurrency);
            e.Cancel = true;
            return;
        }
        //if (baseCurrency == null || baseCurrency.Trim() == string.Empty)
        //{
        //    ShowWarningMessage("MasterData.CurrencyExchang.Required.baseCurrency", baseCurrency);
        //    e.Cancel = true;
        //    return;
        //}

        currencyExchange.BaseCurrency = baseCurrency;
        currencyExchange.ExchangeCurrency = exchangeCurrency;
        if (endDate == string.Empty)
            currencyExchange.EndDate = null;
        else
            currencyExchange.EndDate = Convert.ToDateTime(endDate);
        ShowSuccessMessage("MasterData.CurrencyExchang.AddCurrencyExchang.Successfully", itemMessage);
    }

    protected void ODS_CurrencyExchange_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        btnSearch_Click(this, null);
    }

    protected void ODS_GV_CurrencyExchange_OnDeleting(object sender, ObjectDataSourceMethodEventArgs e)
    {

        CurrencyExchange currencyExchange = (CurrencyExchange)e.InputParameters[0];
        CurrencyExchange delCurrencyExchange = TheCurrencyExchangeMgr.LoadCurrencyExchange(currencyExchange.Id);
        itemMessage[0] = delCurrencyExchange.BaseQty.ToString();
        itemMessage[1] = delCurrencyExchange.BaseCurrency;
        itemMessage[2] = delCurrencyExchange.ExchangeQty.ToString();
        itemMessage[3] = delCurrencyExchange.ExchangeCurrency;
    }

    protected void ODS_GV_CurrencyExchange_OnDeleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            ShowSuccessMessage("MasterData.CurrencyExchange.DeleteCurrencyExchange.Successfully", itemMessage);
        }
        catch (Castle.Facilities.NHibernateIntegration.DataException ex)
        {
            ShowErrorMessage("MasterData.CurrencyExchange.DeleteCurrencyExchange.Fail", itemMessage);
        }
    }


    protected void GV_CurrencyExchange_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string BaseQty = ((TextBox)GV_CurrencyExchange.Rows[e.RowIndex].Cells[2].Controls[1]).Text.ToString().Trim();
        string AlterQty = ((TextBox)GV_CurrencyExchange.Rows[e.RowIndex].Cells[4].Controls[1]).Text.ToString().Trim();
        string endDate = ((TextBox)GV_CurrencyExchange.Rows[e.RowIndex].Cells[6].Controls[1]).Text.ToString().Trim();

        try
        {
            Convert.ToDecimal(BaseQty);
        }
        catch (Exception)
        {
            e.Cancel = true;
            ShowErrorMessage("Common.Decimal.Error", itemMessage);
            return;
        }

        try
        {
            Convert.ToDecimal(AlterQty);
        }
        catch (Exception)
        {
            e.Cancel = true;
            ShowErrorMessage("Common.Decimal.Error", itemMessage);
            return;
        }
        if (endDate == string.Empty)
            endDate = null;
        else
        {
            try
            {
                Convert.ToDateTime(endDate);
            }
            catch (Exception)
            {

                e.Cancel = true;
                ShowErrorMessage("Common.Date.Error", itemMessage);
                return;
            }
        }


    }

    protected void CV_ServerValidate(object source, ServerValidateEventArgs args)
    {
        CustomValidator cv = (CustomValidator)source;

        switch (cv.ID)
        {
            case "cvStartDate":
                try
                {
                    Convert.ToDateTime(args.Value);
                }
                catch (Exception)
                {
                    ShowWarningMessage("Common.Date.Error");
                    args.IsValid = false;
                }
                break;
            case "cvEndDate":
                try
                {
                    if (args.Value.Trim() != "")
                    {
                        DateTime startDate = Convert.ToDateTime(((TextBox)(this.FV_CurrencyExchange.FindControl("tbStartDate"))).Text.Trim());
                        if (DateTime.Compare(startDate, Convert.ToDateTime(args.Value)) >= 0)
                        {
                            ShowErrorMessage("MasterData.CurrencyExchange.TimeCompare");
                            args.IsValid = false;
                        }
                    }
                }
                catch (Exception)
                {
                    ShowWarningMessage("Common.Date.Error");
                    args.IsValid = false;
                }
                break;
        }
        
    }
}

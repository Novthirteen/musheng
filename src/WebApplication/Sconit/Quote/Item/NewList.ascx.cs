using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.Quote;
using com.Sconit.Entity.MasterData;

public partial class Quote_Item_NewList : ModuleBase
{
    public int newRow
    {
        get
        {
            return (int)ViewState["newRow"];
        }
        set
        {
            ViewState["newRow"] = value;
        }
    }

    public string PID
    {
        get
        {
            return (string)ViewState["PID"];
        }
        set
        {
            ViewState["PID"] = value;
        }
    }

    public string Id
    {
        get
        {
            return (string)ViewState["Id"];
        }
        set
        {
            ViewState["Id"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ((LinkButton)e.Row.FindControl("lbtnAdd")).Visible = false;
            ((LinkButton)e.Row.FindControl("lbtnDelete")).Visible = true;
            ((LinkButton)e.Row.FindControl("lbtnEdit")).Visible = false;
            QuoteItem qt = (QuoteItem)e.Row.DataItem;
            if (qt.ProjectId == "NewBlank")
            {
                //((Label)e.Row.FindControl("lblItemCode")).Visible = false;
                //Controls_TextBox tbItemCode = (Controls_TextBox)e.Row.FindControl("tbItemCode");
                TextBox tbItemCode = (TextBox)e.Row.FindControl("txtItemCode");
                //tbItemCode.Visible = true;
                tbItemCode.Enabled = true;
                //tbItemCode.SuggestTextBox.Attributes.Add("onchange", "GenerateFlowDetail(this);");

                //((Label)e.Row.FindControl("lblSupplier")).Visible = false;
                ((TextBox)e.Row.FindControl("txtSupplier")).Visible = true;

                //((Label)e.Row.FindControl("lblCategory")).Visible = false;
                ((TextBox)e.Row.FindControl("txtCategory")).Visible = true;

                //((Label)e.Row.FindControl("lblBrand")).Visible = false;
                ((TextBox)e.Row.FindControl("txtBrand")).Visible = true;

                //((Label)e.Row.FindControl("lblModel")).Visible = false;
                ((TextBox)e.Row.FindControl("txtModel")).Visible = true;

                //((Label)e.Row.FindControl("lblSingleNum")).Visible = false;
                TextBox txtSN = ((TextBox)e.Row.FindControl("txtSingleNum"));
                txtSN.Visible = true;
                txtSN.Attributes.Add("onkeyup", "txtSingleNumChange(this);");

                //((Label)e.Row.FindControl("lblPurchasePrice")).Visible = false;
                TextBox txtPP = ((TextBox)e.Row.FindControl("txtPurchasePrice"));
                txtPP.Visible = true;
                txtPP.Attributes.Add("onkeyup", "txtPurchasePriceChange(this);");

                //((Label)e.Row.FindControl("lblPrice")).Visible = false;
                ((TextBox)e.Row.FindControl("txtPrice")).Visible = true;

                //((Label)e.Row.FindControl("lblPinNum")).Visible = false;
                ((TextBox)e.Row.FindControl("txtPinNum")).Visible = true;

                //((Label)e.Row.FindControl("lblPinConversion")).Visible = false;
                TextBox txtPC = ((TextBox)e.Row.FindControl("txtPinConversion"));
                txtPC.Attributes.Add("onkeyup", "txtPinConversionChange(this);");

                //((Label)e.Row.FindControl("lblPoint")).Visible = false;
                ((TextBox)e.Row.FindControl("txtPoint")).Visible = true;

                //((Label)e.Row.FindControl("lblBitNum")).Visible = false;
                ((TextBox)e.Row.FindControl("txtBitNum")).Visible = true;

                //((Label)e.Row.FindControl("lblSide")).Visible = false;
                ((TextBox)e.Row.FindControl("txtSide")).Visible = true;

                ((LinkButton)e.Row.FindControl("lbtnAdd")).Visible = true;
                ((LinkButton)e.Row.FindControl("lbtnDelete")).Visible = false;
                ((LinkButton)e.Row.FindControl("lbtnEdit")).Visible = false;
            }
            else
            {
                TextBox txtSN = ((TextBox)e.Row.FindControl("txtSingleNum"));
                txtSN.Visible = true;
                txtSN.Attributes.Add("onkeyup", "txtSingleNumChange(this);");

                TextBox txtPP = ((TextBox)e.Row.FindControl("txtPurchasePrice"));
                txtPP.Visible = true;
                txtPP.Attributes.Add("onkeyup", "txtPurchasePriceChange(this);");

                TextBox txtPC = ((TextBox)e.Row.FindControl("txtPinConversion"));
                txtPC.Attributes.Add("onkeyup", "txtPinConversionChange(this);");
            }
        }
    }

    public void InitPageParameter(string pid)
    {
        PID = pid;
        IList<QuoteItem> qtList = TheToolingMgr.GetItemByProductId(Int32.Parse(pid));
        newRow = qtList.Count;
        QuoteItem qt = new QuoteItem();
        qt.ProjectId = "NewBlank";
        qtList.Add(qt);
        GV_List.DataSource = qtList;
        GV_List.DataBind();
    }

    public void InitPageParameter(string productid,string itemcode)
    {
        #region Load BOM
        IList<BomDetail> bomDetailList = new List<BomDetail>();
        if (itemcode != "")
        {
            Item item = TheItemMgr.LoadItem(itemcode);
            bomDetailList = TheBomDetailMgr.GetBomView_Nml(item, DateTime.Now);
        }
        #endregion
        PID = productid;
        //IList<QuoteItem> qtList = TheToolingMgr.GetItemByProductId(Int32.Parse(productid));
        IList<QuoteItem> qtList = new List<QuoteItem>();
        #region Add BOM

        #region Load Before List
        foreach(GridViewRow row in this.GV_List.Rows)
        {
            if (((TextBox)row.FindControl("txtItemCode")).Text != string.Empty)
            {
                QuoteItem qi = new QuoteItem();
                qi.ItemCode = ((TextBox)row.FindControl("txtItemCode")).Text;
                qi.Supplier = ((TextBox)row.FindControl("txtSupplier")).Text;
                qi.Category = ((TextBox)row.FindControl("txtCategory")).Text;
                qi.Brand = ((TextBox)row.FindControl("txtBrand")).Text;
                qi.Model = ((TextBox)row.FindControl("txtModel")).Text;
                qi.SingleNum = ((TextBox)row.FindControl("txtSingleNum")).Text;
                string PurchasePrice = ((TextBox)row.FindControl("txtPurchasePrice")).Text;
                qi.PurchasePrice = PurchasePrice == string.Empty ? 0 : decimal.Parse(PurchasePrice);
                string Price = ((TextBox)row.FindControl("txtPrice")).Text;
                qi.Price = Price == string.Empty ? 0 : decimal.Parse(Price);
                qi.PinNum = ((TextBox)row.FindControl("txtPinNum")).Text;
                qi.PinConversion = ((TextBox)row.FindControl("txtPinConversion")).Text;
                qi.Point = ((TextBox)row.FindControl("txtPoint")).Text;
                qi.ProductId = Int32.Parse(PID);
                qi.Side = ((TextBox)row.FindControl("txtSide")).Text;
                qi.BitNum = ((TextBox)row.FindControl("txtBitNum")).Text;
                qtList.Add(qi);
            }
        }
        #endregion

        #region load bom
        if (bomDetailList != null)
        {
            if (bomDetailList.Count > 0)
            {
                foreach (BomDetail bd in bomDetailList)
                {
                    QuoteItem qi = new QuoteItem();
                    //qi.BitNum = "";
                    //qi.Brand = "";
                    //qi.Category = "";
                    qi.ItemCode = bd.Item.Code;
                    //qi.Model = "";
                    qi.SingleNum = bd.RateQty.ToString("f0");
                    if (bd.Item.ItemPack != null)
                    {
                        qi.PinConversion = bd.Item.ItemPack.PinConversion.ToString();
                        qi.PinNum = bd.Item.ItemPack.PinNum.ToString();
                        qi.Point = (decimal.Parse(qi.SingleNum) * decimal.Parse(qi.PinConversion)).ToString("f4");
                    }
                    //qi.ProductId = Int32.Parse(productid);
                    //qi.Price = 0;
                    //qi.PurchasePrice = 0;
                    //qi.Side = "";
                    //qi.Supplier = "";
                    qtList.Add(qi);
                    //TheToolingMgr.CreateQuoteItem(qi);
                }
            }
        }
        #endregion

        #region add new row
        //if (itemcode == "")
        //{
        //    GridViewRow newQT = GV_List.Rows[newRow];
        //    QuoteItem qiNew = new QuoteItem();
        //    //qt.ProjectId = PID;
        //    qiNew.ItemCode = ((TextBox)newQT.FindControl("txtItemCode")).Text;
        //    qiNew.Supplier = ((TextBox)newQT.FindControl("txtSupplier")).Text;
        //    qiNew.Category = ((TextBox)newQT.FindControl("txtCategory")).Text;
        //    qiNew.Brand = ((TextBox)newQT.FindControl("txtBrand")).Text;
        //    qiNew.Model = ((TextBox)newQT.FindControl("txtModel")).Text;
        //    qiNew.SingleNum = ((TextBox)newQT.FindControl("txtSingleNum")).Text;
        //    string PurchasePrice = ((TextBox)newQT.FindControl("txtPurchasePrice")).Text;
        //    qiNew.PurchasePrice = PurchasePrice == string.Empty ? 0 : decimal.Parse(PurchasePrice);
        //    string Price = ((TextBox)newQT.FindControl("txtPrice")).Text;
        //    qiNew.Price = Price == string.Empty ? 0 : decimal.Parse(Price);
        //    qiNew.PinNum = ((TextBox)newQT.FindControl("txtPinNum")).Text;
        //    qiNew.PinConversion = ((TextBox)newQT.FindControl("txtPinConversion")).Text;
        //    qiNew.Point = ((TextBox)newQT.FindControl("txtPoint")).Text;
        //    qiNew.ProductId = Int32.Parse(PID);
        //    qiNew.Side = ((TextBox)newQT.FindControl("txtSide")).Text;
        //    qiNew.BitNum = ((TextBox)newQT.FindControl("txtBitNum")).Text;
        //    qtList.Add(qiNew);
        //}
        #endregion

        #endregion
        newRow = qtList.Count;
        QuoteItem qt = new QuoteItem();
        qt.ProjectId = "NewBlank";
        qtList.Add(qt);
        GV_List.DataSource = qtList;
        GV_List.DataBind();
    }

    public void lbtnAdd_Click(object sender, EventArgs e)
    {
        InitPageParameter(PID,"");
    }

    public void lbtnDelete_Click(object sender, EventArgs e)
    {
        string seq = ((LinkButton)sender).CommandArgument;
        foreach(GridViewRow row in GV_List.Rows)
        {
            string seq1 = ((Literal)row.FindControl("ltlNo")).Text;
            if(seq == (Int32.Parse(seq1) - 1).ToString())
            {
                //GV_List.Rows[Int32.Parse(seq)].Visible = false;
                row.Visible = false;
            }
        }
        //GV_List.Rows[4].Visible = false;
    }

    protected void GV_List_RowEditing(object sender, GridViewEditEventArgs e)
    {
        string id = ((Label)GV_List.Rows[e.NewEditIndex].FindControl("lblId")).Text ; 
        GV_List.EditIndex = e.NewEditIndex;
        InitPageParameter(PID);

        Label lblIC = ((Label)GV_List.Rows[e.NewEditIndex].FindControl("lblSupplier"));
        TextBox txtIC = ((TextBox)GV_List.Rows[e.NewEditIndex].FindControl("txtSupplier"));
        txtIC.Text = lblIC.Text;
        lblIC.Visible = false;
        txtIC.Visible = true;

        Label lblC = ((Label)GV_List.Rows[e.NewEditIndex].FindControl("lblCategory"));
        TextBox txtC = ((TextBox)GV_List.Rows[e.NewEditIndex].FindControl("txtCategory"));
        txtC.Text = lblC.Text;
        lblC.Visible = false;
        txtIC.Visible = true;

        Label lblB = ((Label)GV_List.Rows[e.NewEditIndex].FindControl("lblBrand"));
        TextBox txtB = ((TextBox)GV_List.Rows[e.NewEditIndex].FindControl("txtBrand"));
        txtB.Text = lblB.Text;
        lblB.Visible = false;
        txtB.Visible = true;

        Label lblM = ((Label)GV_List.Rows[e.NewEditIndex].FindControl("lblModel"));
        TextBox txtM = ((TextBox)GV_List.Rows[e.NewEditIndex].FindControl("txtModel"));
        txtM.Text = lblM.Text;
        lblM.Visible = false;
        txtB.Visible = true;

        Label lblSN = ((Label)GV_List.Rows[e.NewEditIndex].FindControl("lblSingleNum"));
        TextBox txtSN = ((TextBox)GV_List.Rows[e.NewEditIndex].FindControl("txtSingleNum"));
        txtSN.Visible = true;
        txtSN.Attributes.Add("onkeyup", "test(this);");
        txtSN.Text = lblSN.Text;
        lblSN.Visible = false;

        Label lblPP = ((Label)GV_List.Rows[e.NewEditIndex].FindControl("lblPurchasePrice"));
        TextBox txtPP = ((TextBox)GV_List.Rows[e.NewEditIndex].FindControl("txtPurchasePrice"));
        txtPP.Visible = true;
        txtPP.Attributes.Add("onkeyup", "txtPurchasePriceChange(this);");
        txtPP.Text = lblPP.Text;
        lblPP.Visible = false;

        Label lblP = ((Label)GV_List.Rows[e.NewEditIndex].FindControl("lblPrice"));
        TextBox txtP = ((TextBox)GV_List.Rows[e.NewEditIndex].FindControl("txtPrice"));
        txtP.Text = lblP.Text;
        lblP.Visible = false;
        txtP.Visible = true;

        Label lblPN = ((Label)GV_List.Rows[e.NewEditIndex].FindControl("lblPinNum"));
        TextBox txtPN = ((TextBox)GV_List.Rows[e.NewEditIndex].FindControl("txtPinNum"));
        txtPN.Text = lblPN.Text;
        lblPN.Visible = false;
        txtPN.Visible = true;

        Label lblPC = ((Label)GV_List.Rows[e.NewEditIndex].FindControl("lblPinConversion"));
        TextBox txtPC = ((TextBox)GV_List.Rows[e.NewEditIndex].FindControl("txtPinConversion"));
        txtPC.Text = lblPC.Text;
        lblPC.Visible = false;
        txtPC.Visible = true;

        Label lblPo = ((Label)GV_List.Rows[e.NewEditIndex].FindControl("lblPoint"));
        TextBox txtPo = ((TextBox)GV_List.Rows[e.NewEditIndex].FindControl("txtPoint"));
        txtPo.Text = lblPo.Text;
        txtPo.Visible = true;
        lblPo.Visible = false;

        Label lblBN = ((Label)GV_List.Rows[e.NewEditIndex].FindControl("lblBitNum"));
        TextBox txtBN = ((TextBox)GV_List.Rows[e.NewEditIndex].FindControl("txtBitNum"));
        txtBN.Text = lblBN.Text;
        lblBN.Visible = false;
        txtBN.Visible = true;

        Label lblSi = ((Label)GV_List.Rows[e.NewEditIndex].FindControl("lblSide"));
        TextBox txtSi = ((TextBox)GV_List.Rows[e.NewEditIndex].FindControl("txtSide"));
        txtSi.Text = lblSi.Text;
        lblSi.Visible = false;
        txtSi.Visible = true;
        
    }

    protected void GV_List_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GV_List.EditIndex = -1;
        InitPageParameter(PID);
    }
    protected void GV_List_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string id = ((Label)GV_List.Rows[e.RowIndex].FindControl("lblId")).Text; 

    }

    public IList<QuoteItem> GetQIList()
    {
        IList<QuoteItem> QIList = new List<QuoteItem>();
        foreach(GridViewRow row in GV_List.Rows)
        {
            if (((TextBox)row.FindControl("txtItemCode")).Text != "")
            {
                QuoteItem QI = new QuoteItem();
                if (row.Visible)
                {
                    QI.ItemCode = ((TextBox)row.FindControl("txtItemCode")).Text;
                    QI.SingleNum = ((TextBox)row.FindControl("txtSingleNum")).Text;
                    QI.PurchasePrice = ((TextBox)row.FindControl("txtPurchasePrice")).Text == "" ? 0 : decimal.Parse(((TextBox)row.FindControl("txtPurchasePrice")).Text);
                    QIList.Add(QI);
                }
            }
        }

        var newList = from i in QIList group i by new { i.ItemCode ,i.PurchasePrice} into g let sum = g.Sum(x => Int32.Parse(x.SingleNum)) select new { code = g.Key.ItemCode, Sum = sum ,Price = g.Key.PurchasePrice};

        IList<QuoteItem> QIList1 = new List<QuoteItem>();
        foreach(var list in newList)
        {
            QuoteItem QI = new QuoteItem();
            QI.ItemCode = list.code;
            QI.SingleNum = list.Sum.ToString();
            QI.PurchasePrice = list.Price;
            QIList1.Add(QI);
        }

        return QIList1;
    }

    public void PriceBack(IList<QuoteItem> list)
    {
        foreach(GridViewRow row in this.GV_List.Rows)
        {
            foreach(QuoteItem qi in list)
            {
                if(((TextBox)row.FindControl("txtItemCode")).Text == qi.ItemCode)
                {
                    ((TextBox)row.FindControl("txtPurchasePrice")).Text = qi.PurchasePrice.ToString();
                    ((TextBox)row.FindControl("txtPrice")).Text = (decimal.Parse(((TextBox)row.FindControl("txtSingleNum")).Text) * qi.PurchasePrice).ToString();
                }
            }
        }
    }

    public void SaveData()
    {
        #region delete
        //IList<QuoteItem> qtListDelete = TheToolingMgr.GetItemByProductId(Int32.Parse(PID));
        TheToolingMgr.DeleteQuoteItemByProductId(PID);
        #endregion

        #region save
        //IList<QuoteItem> qtListSave = new List<QuoteItem>();
        foreach (GridViewRow row in this.GV_List.Rows)
        {
            if (((TextBox)row.FindControl("txtItemCode")).Text != string.Empty)
            {
                QuoteItem qi = new QuoteItem();
                qi.ItemCode = ((TextBox)row.FindControl("txtItemCode")).Text;
                qi.Supplier = ((TextBox)row.FindControl("txtSupplier")).Text;
                qi.Category = ((TextBox)row.FindControl("txtCategory")).Text;
                qi.Brand = ((TextBox)row.FindControl("txtBrand")).Text;
                qi.Model = ((TextBox)row.FindControl("txtModel")).Text;
                qi.SingleNum = ((TextBox)row.FindControl("txtSingleNum")).Text;
                string PurchasePrice = ((TextBox)row.FindControl("txtPurchasePrice")).Text;
                qi.PurchasePrice = PurchasePrice == string.Empty ? 0 : decimal.Parse(PurchasePrice);
                string Price = ((TextBox)row.FindControl("txtPrice")).Text;
                qi.Price = Price == string.Empty ? 0 : decimal.Parse(Price);
                qi.PinNum = ((TextBox)row.FindControl("txtPinNum")).Text;
                qi.PinConversion = ((TextBox)row.FindControl("txtPinConversion")).Text;
                qi.Point = ((TextBox)row.FindControl("txtPoint")).Text;
                qi.ProductId = Int32.Parse(PID);
                qi.Side = ((TextBox)row.FindControl("txtSide")).Text;
                qi.BitNum = ((TextBox)row.FindControl("txtBitNum")).Text;
                //qtListSave.Add(qi);
                TheToolingMgr.SaveQuoteItem(qi);
            }
        }
        #endregion
    }
}
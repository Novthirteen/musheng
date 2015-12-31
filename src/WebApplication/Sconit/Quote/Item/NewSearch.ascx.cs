using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using com.Sconit.Entity.Quote;

public partial class Quote_Item_NewSearch : ModuleBase
{
    public EventHandler SearchEvent;
    public EventHandler ItemSearchEvent;
    public EventHandler SaveEvent;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void txtPID_Change(object sender, EventArgs e)
    {
        SearchEvent(this.txtProjectId.Text, e);
    }

    public void txtItem_Change(object sender, EventArgs e)
    {
        if(this.txtProjectId.Text.Trim() == string.Empty)
        {
            ShowWarningMessage("Quote.Item.ProductId.NoSelect");
            return;
        }
        ItemSearchEvent(this.txtProjectId.Text.Trim() + "," + this.tbItem.Text.Trim(), e);
    }

    public void btnImport_Click(object sender, EventArgs e)
    {
        if(rbXLS.Checked)
        {
            //Response.Write(Path.GetFullPath(fuXLS.PostedFile.InputStream));
            try
            {
                IList<QuoteItem> qiList = TheToolingMgr.GetXLSQuoteItem(fuXLS.PostedFile.InputStream);
            }
            catch (Exception ex)
            {
                
            }
        }
    }

    public void btnSave_Click(object sender, EventArgs e)
    {
        SaveEvent(sender, e);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using com.Sconit.Entity.Customize;

public partial class Production_PLFeedSeq_Import_Main : MainModuleBase
{
    public event EventHandler BtnBackClick;

    public string ModuleType
    {
        get { return (string)ViewState["ModuleType"]; }
        set { ViewState["ModuleType"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void ucImport_ImportEvent(object sender, EventArgs e)
    {
       
    }

    protected void ucImport_BtnBackClick(object sender, EventArgs e)
    {
        if (BtnBackClick != null)
        {
            BtnBackClick(null, null);
        }
    }

    protected void ucPreview_BtnCreateClick(object sender, EventArgs e)
    {
        
    }
}

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
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.MasterData;
using com.Sconit.Utility;
using com.Sconit.Web;
using com.Sconit.Entity;

public partial class MasterData_ItemType_New : NewModuleBase
{
    private ItemType itemType;
    public event EventHandler CreateEvent;
    public event EventHandler BackEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
    }
    
    public void PageCleanup()
    {
        ((TextBox)(this.FV_ItemType.FindControl("tbCode"))).Text = string.Empty;
        ((TextBox)(this.FV_ItemType.FindControl("tbName"))).Text = string.Empty;
        ((TextBox)(this.FV_ItemType.FindControl("tbShortName"))).Text = string.Empty;
        ((TextBox)(this.FV_ItemType.FindControl("tbLevel"))).Text = string.Empty;
    }

    protected void CV_ServerValidate(object source, ServerValidateEventArgs args)
    {
        CustomValidator cv = (CustomValidator)source;
        switch (cv.ID)
        {
            case "cvCode":
                if (TheItemTypeMgr.LoadItemType(args.Value) != null)
                {
                    ShowWarningMessage("MasterData.ItemType.Code.Exists", args.Value);
                    args.IsValid = false;
                }
                break;
            default:
                break;
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void ODS_ItemType_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        itemType = (ItemType)e.InputParameters[0];
        itemType.Code = itemType.Code.Trim();
        itemType.Name = itemType.Name.Trim();
        itemType.ShortName = itemType.ShortName.Trim();
    }

    protected void ODS_ItemType_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (CreateEvent != null)
        {
            CreateEvent(itemType.Code, e);
            ShowSuccessMessage("MasterData.ItemType.AddItemType.Successfully", itemType.Code);
        }
    }
}

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
using com.Sconit.Web;
using com.Sconit.Entity;

public partial class MasterData_ItemType_Edit : EditModuleBase
{
    private ItemType itemType;
    public event EventHandler BackEvent;

    protected string ItemTypeCode
    {
        get
        {
            return (string)ViewState["ItemTypeCode"];
        }
        set
        {
            ViewState["ItemTypeCode"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public void InitPageParameter(string code)
    {
        this.ItemTypeCode = code;
        this.ODS_ItemType.SelectParameters["code"].DefaultValue = this.ItemTypeCode;
        this.ODS_ItemType.DeleteParameters["code"].DefaultValue = this.ItemTypeCode;
    }

    protected void FV_ItemType_DataBound(object sender, EventArgs e)
    {
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }

    protected void ODS_ItemType_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        itemType = (ItemType)e.InputParameters[0];
        itemType.Code = itemType.Code.Trim();
        itemType.Name = itemType.Name.Trim();
        itemType.ShortName = itemType.ShortName.Trim();
    }

    protected void ODS_ItemType_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ShowSuccessMessage("MasterData.ItemType.UpdateItemType.Successfully", ItemTypeCode);
        
    }

    protected void ODS_ItemType_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (e.Exception == null)
        {
            btnBack_Click(this, e);
            ShowSuccessMessage("MasterData.ItemType.DeleteItemType.Successfully", ItemTypeCode);
        }
        else if (e.Exception.InnerException is Castle.Facilities.NHibernateIntegration.DataException)
        {
            ShowErrorMessage("MasterData.ItemType.DeleteItemType.Fail", ItemTypeCode);
            e.ExceptionHandled = true;
        }
    }
}

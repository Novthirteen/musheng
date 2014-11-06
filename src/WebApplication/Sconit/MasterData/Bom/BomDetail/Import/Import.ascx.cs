using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.Customize;

public partial class MasterData_Bom_BomDetail_Import_Import : ModuleBase
{
    public event EventHandler ImportEvent;
    public event EventHandler BtnBackClick;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
        }

         
    }

    public void Create(object sender)
    {
        try
        {
            IList<OrderHead> orderHeadList = (IList<OrderHead>)sender;
            if (orderHeadList != null && orderHeadList.Count > 0)
            {
                //TheOrderMgr.CreateOrder(orderHeadList, this.CurrentUser.Code);
                ShowSuccessMessage("Common.Business.Result.Insert.Successfully");
            }
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage(ex);
        }
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        this.Import();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BtnBackClick != null)
        {
            BtnBackClick(null, null);
        }
    }
    
     private void Import()
    {
        try
        {
            int count = TheBomDetailMgr.ReadFromXls(fileUpload.PostedFile.InputStream, this.CurrentUser);
            ShowSuccessMessage("MasterData.Bom.Import.Result.Successfully", count.ToString());
        }
        catch (BusinessErrorException ex)
        {
            ShowErrorMessage(ex);
        }
    }
}

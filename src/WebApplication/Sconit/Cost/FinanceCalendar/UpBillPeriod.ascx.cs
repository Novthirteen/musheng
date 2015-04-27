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
using com.Sconit.Control;
using com.Sconit.Utility;
using com.Sconit.Entity.Cost;
using System.Collections.Generic;
using System.Data.SqlClient;
using NHibernate.Expression;

public partial class Cost_FinanceCalendar_UpBillPeriod : NewModuleBase
{
    public event EventHandler BackEvent;


    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }



    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        string purchaseAccountDate = this.tbPurchaseAccountDate.Text.Trim();
        string financeAccountDate = this.tbFinanceAccountDate.Text.Trim();
        try
        {
            if (string.IsNullOrEmpty(purchaseAccountDate))
            {
                throw new Exception("采购结账日不能为空。");
            }
            if (string.IsNullOrEmpty(financeAccountDate))
            {
                throw new Exception("财务结账日不能为空。");
            }
           SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@PurchaseDate", SqlDbType.DateTime);
            param[1] = new SqlParameter("@FinanceDate", SqlDbType.DateTime);
            param[2] = new SqlParameter("@ModifyUser", SqlDbType.VarChar,50);

            param[0].Value = Convert.ToDateTime(purchaseAccountDate);
            param[1].Value = Convert.ToDateTime(financeAccountDate);
            param[2].Value =CurrentUser.Code;

            TheGenericMgr.GetDatasetBySql("exec Proc_UpdateBillPeriod @PurchaseDate,@FinanceDate,@ModifyUser", param);
            ShowSuccessMessage("Cost.FinanceCalendar.UpBillPeriod.Successfully");
        }
        catch (Exception ex)
        {
            ShowErrorMessage("Cost.FinanceCalendar.UpBillPeriod.Failed" + ex.Message);
        }
    }

}

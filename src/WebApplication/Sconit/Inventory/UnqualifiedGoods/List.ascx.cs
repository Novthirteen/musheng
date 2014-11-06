using System;
using System.Collections;
using System.Collections.Generic;
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
using com.Sconit.Entity.Exception;
using NHibernate.Expression;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using System.Text;

public partial class Inventory_UnqualifiedGoods_List : ModuleBase
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void InitPageParameter(DetachedCriteria selectCriteria)
    {

        IList<InspectOrderDetail> inspectOrderDetailList = TheCriteriaMgr.FindAll<InspectOrderDetail>(selectCriteria);

        this.GV_List.DataSource = inspectOrderDetailList;
        this.GV_List.DataBind();
        if (inspectOrderDetailList.Count == 0)
        {
            this.lblMessage.Visible = true;
            this.lblMessage.Text = "${Common.GridView.NoRecordFound}";
            this.btnPrint.Visible = false;
        }
        else
        {
            this.lblMessage.Visible = false;
            this.btnPrint.Visible = true;
        }

    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {

        IList<object> list = new List<object>();
        IList<InspectOrderDetail> inspectOrderDetailList = PopulateUnqualifiedInspectOrder();
        if (inspectOrderDetailList.Count == 0)
        {
            ShowErrorMessage("Common.Business.Warn.DetailEmpty");
            return;
        }

        InspectOrder inspectOrder = new InspectOrder();
        inspectOrder.CreateUser = this.CurrentUser;
        inspectOrder.CreateDate = DateTime.Now;
        inspectOrder.IsPrinted = true;
        list.Add(inspectOrder);
        list.Add(inspectOrderDetailList);
        string printUrl = TheReportMgr.WriteToFile("BelowBrade.xls", list);
        Page.ClientScript.RegisterStartupScript(GetType(), "method", " <script language='javascript' type='text/javascript'>PrintOrder('" + printUrl + "'); </script>");
        this.ShowSuccessMessage("MasterData.Inventory.InspectOrder.Unqualified.Print.Successful");


    }


    public IList<InspectOrderDetail> PopulateUnqualifiedInspectOrder()
    {
        IList<InspectOrderDetail> inspectOrderDetailList = new List<InspectOrderDetail>();
        for (int i = 0; i < this.GV_List.Rows.Count; i++)
        {
            GridViewRow row = this.GV_List.Rows[i];
            CheckBox checkBoxGroup = row.FindControl("CheckBoxGroup") as CheckBox;
            if (checkBoxGroup.Checked)
            {
                HiddenField hfId = (HiddenField)row.FindControl("hfId");
                InspectOrderDetail inspectOrderDetail = TheInspectOrderDetailMgr.LoadInspectOrderDetail(int.Parse(hfId.Value));
                inspectOrderDetailList.Add(inspectOrderDetail);
            }
        }
        return inspectOrderDetailList;
    }




}

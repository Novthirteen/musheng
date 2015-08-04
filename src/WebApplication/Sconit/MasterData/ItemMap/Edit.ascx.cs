using com.Sconit.Entity.MasterData;
using com.Sconit.Web;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterData_ItemMap_Edit : EditModuleBase
{
    public event EventHandler BackEvent;
    static ItemMap itemMap;
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    public void InitPageParameter(string item)
    {
        itemMap = TheItemMapMgr.LoadItemMap(item);
        tdItem.Text = itemMap.Item;
        tdMapItem.Text = itemMap.MapItem;
        tbStartDate.Text = itemMap.StartDate.ToString();
        tbEndDate.Text = itemMap.EndDate.ToString();
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //itemMap.Item = tdItem.Text;
            //itemMap.MapItem = tdMapItem.Text;
            //itemMap.StartDate = DateTime.Parse(tbStartDate.Text);
            //itemMap.EndDate = DateTime.Parse(tbEndDate.Text);
            //itemMap.LastModifyDate = DateTime.Now;
            //itemMap.LastModifyUser = CurrentUser.Code;
            //TheItemMapMgr.UpdateItemMap(itemMap);
            string sql = "update ItemMap set Item = @Item,MapItem = @MapItem,StartDate = @StartDate,EndDate = @EndDate,LastModifyDate = @LastModifyDate,LastModifyUser = @LastModifyUser where Id = @Id";
            SqlParameter[] sps = new SqlParameter[]{
                new SqlParameter("@Item",tdItem.Text),
                new SqlParameter("@MapItem",tdMapItem.Text),
                new SqlParameter("@StartDate",DateTime.Parse(tbStartDate.Text)),
                new SqlParameter("@EndDate",DateTime.Parse(tbEndDate.Text)),
                new SqlParameter("@LastModifyDate",DateTime.Now),
                new SqlParameter("@LastModifyUser",CurrentUser.Code),
                new SqlParameter("@Id",itemMap.Id)
            };
            TheSqlHelperMgr.Update(sql, sps);
            ShowSuccessMessage("MasterData.ItemMap.UpdateItemMap.Successfully");
        }
        catch(Exception ex)
        {
            ShowErrorMessage("MasterData.ItemMap.UpdateItemMap.Fail");
        }
    }
    protected void checkItemExists(object source, ServerValidateEventArgs args)
    {
        string code = tdItem.Text;

        if (TheItemMgr.LoadItem(code) == null || TheItemMgr.LoadItem(code).Equals(""))
        {
            ShowErrorMessage("MasterData.ItemMap.CodeExist11", code);
            args.IsValid = false;
        }
    }

    protected void checkItemExists1(object source, ServerValidateEventArgs args)
    {
        string code = tdMapItem.Text;

        if (TheItemMgr.LoadItem(code) == null || TheItemMgr.LoadItem(code).Equals(""))
        {
            ShowErrorMessage("MasterData.ItemMap.CodeExist22", code);
            args.IsValid = false;
        }
    }
    #region ////
    //protected void FV_ItemMap_DataBound(object sender, EventArgs e)
    //{

    //    ItemMap itemMap = (ItemMap)(((FormView)(sender)).DataItem);
    //    if (itemMap != null)
    //    {
    //        ((Controls_TextBox)(this.FV_ItemDisCon.FindControl("tdItem"))).Text = (itemMap.Item) == null ? string.Empty : itemMap.Item;
    //        ((Controls_TextBox)(this.FV_ItemDisCon.FindControl("tdDiscontinueItem"))).Text = (itemMap.MapItem == null) ? string.Empty : itemMap.MapItem;

    //    }
    //}

    //protected void ODS_ItemMap_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    //{
    //    //ShowSuccessMessage("MasterData.ItemDisCon.UpdateItemDisCon.Successfully", id);
    //}

    //protected void ODS_ItemMap_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    //{
    //    //ItemDiscontinue ItemDisCon = (ItemDiscontinue)e.InputParameters[0];
    //    //string bom = ((TextBox)(this.FV_ItemDisCon.FindControl("tdBom"))).Text.Trim();
    //    //ItemDisCon.Bom = TheBomMgr.LoadBom(bom);
    //    //string item = ((Controls_TextBox)(this.FV_ItemDisCon.FindControl("tdItem"))).Text.Trim();
    //    //ItemDisCon.Item = TheItemMgr.LoadItem(item);
    //    //string discontinueItem = ((Controls_TextBox)(this.FV_ItemDisCon.FindControl("tdDiscontinueItem"))).Text.Trim();
    //    //ItemDisCon.DiscontinueItem = TheItemMgr.LoadItem(discontinueItem);
    //    //ItemDisCon.LastModifyDate = DateTime.Now;
    //    //ItemDisCon.LastModifyUser = this.CurrentUser.Code;

    //    //string endDate = ((TextBox)(this.FV_ItemDisCon.FindControl("tbEndDate"))).Text;
    //    //if (endDate != string.Empty)
    //    //{
    //    //    ItemDisCon.EndDate = DateTime.Parse(endDate);
    //    //}
    //    //else
    //    //{
    //    //    ItemDisCon.EndDate = null;
    //    //}
    //}

    //protected void ODS_ItemMap_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
    //{
    //    //if (e.Exception == null)
    //    //{
    //    //    btnBack_Click(this, e);
    //    //    ShowSuccessMessage("MasterData.ItemDisCon.DeleteItemDisCon.Successfully", id);
    //    //}
    //    //else if (e.Exception.InnerException is Castle.Facilities.NHibernateIntegration.DataException)
    //    //{
    //    //    ShowErrorMessage("MasterData.ItemDisCon.DeleteItemDisCon.Fail", id);
    //    //    e.ExceptionHandled = true;
    //    //}
    //}
    #endregion
}
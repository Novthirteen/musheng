﻿using System;
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
using System.IO;

public partial class MasterData_Item_List : ListModuleBase
{
    public EventHandler EditEvent;

    public bool ShowImage
    {
        get
        {
            if (ViewState["ShowImage"] != null)
            {
                return (bool)ViewState["ShowImage"];
            }
            else
            {
                return false;
            }
        }
        set
        {
            ViewState["ShowImage"] = value;
        }
    }

    public void Export()
    {
        this.IsExport = true;
        if (GV_List.Rows.Count > 0)
        {

            for (int i = 4; i <= 18; i++)
            {
                GV_List.Columns[i].Visible = true;
            }
            
            for (int i = 26; i >=19; i--)
            {
                GV_List.Columns.RemoveAt(i);
            }
            GV_List.Columns.RemoveAt(3);
            GV_List.Columns.RemoveAt(0);
        }
        
        GV_List.ExportXLS("Item.xls");
        //this.ExportXLS(GV_List);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public override void UpdateView()
    {
        this.IsExport = false;
        this.GV_List.Columns[1].Visible = ShowImage;
        this.GV_List.Execute();
    }

    protected void lbtnEdit_Click(object sender, EventArgs e)
    {
        if (EditEvent != null)
        {
            string code = ((LinkButton)sender).CommandArgument;
            EditEvent(code, e);
        }
    }

    protected void lbtnDelete_Click(object sender, EventArgs e)
    {
        string code = ((LinkButton)sender).CommandArgument;
        try
        {
            string imageUrl = TheItemMgr.LoadItem(code).ImageUrl;
            if (File.Exists(Server.MapPath(imageUrl)))
            {
                File.Delete(Server.MapPath(imageUrl));
            }
            TheItemMgr.DeleteItem(code);
            ShowSuccessMessage("MasterData.Item.DeleteItem.Successfully", code);
            UpdateView();
        }
        catch (Exception ex)
        {
            ShowErrorMessage("MasterData.Item.DeleteItem.Fail", code);
        }

    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (this.IsExport)
            {
                //e.Row.Cells[2].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                //e.Row.Cells[4].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                //e.Row.Cells[5].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
            }
            else if (ShowImage)
            {
                if (((Image)(e.Row.FindControl("imgImageUrl"))).ImageUrl == string.Empty)
                {
                    ((Image)(e.Row.FindControl("imgImageUrl"))).ImageUrl = null;
                }
            }


            
        }
    }
}

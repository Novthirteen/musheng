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
using com.Sconit.Entity.MasterData;

public partial class MasterData_Bom_Bom_New : NewModuleBase
{
    private Bom bom;
    private Item item;
    public event EventHandler CreateEvent;
    public event EventHandler BackEvent;

    private string BomCode
    {
        get
        {
            return (string)ViewState["BomCode"];
        }
        set
        {
            ViewState["BomCode"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ((Controls_TextBox)(this.FV_Bom.FindControl("tbRegion"))).ServiceParameter = "string:" + this.CurrentUser.Code;
    }

    public void PageCleanup()
    {
        ((TextBox)(this.FV_Bom.FindControl("tbCode"))).Text = string.Empty;
        ((TextBox)(this.FV_Bom.FindControl("tbDescription"))).Text = string.Empty;
        ((Controls_TextBox)(this.FV_Bom.FindControl("tbUom"))).Text = string.Empty;
        ((Controls_TextBox)(this.FV_Bom.FindControl("tbRegion"))).Text = string.Empty;
        ((CheckBox)(this.FV_Bom.FindControl("cbIsActive"))).Checked = true;
    }

    public void InitPageParameter(string code)
    {
        if (code != null)
        {
            Bom bom = TheBomMgr.LoadBom(code);
            this.BomCode = code;
            ((TextBox)(this.FV_Bom.FindControl("tbCode"))).Text = string.Empty; // bom.Code + "[clone]";
            ((TextBox)(this.FV_Bom.FindControl("tbDescription"))).Text = bom.Description;
            ((Controls_TextBox)(this.FV_Bom.FindControl("tbUom"))).Text = bom.Uom == null ? string.Empty : bom.Uom.Code;
            ((Controls_TextBox)(this.FV_Bom.FindControl("tbRegion"))).Text = bom.Region == null ? string.Empty : bom.Region.Code;
        }
        else
        {
            ((TextBox)(this.FV_Bom.FindControl("tbCode"))).Text = string.Empty;
            ((TextBox)(this.FV_Bom.FindControl("tbDescription"))).Text = string.Empty;
            ((Controls_TextBox)(this.FV_Bom.FindControl("tbUom"))).Text = string.Empty;
            ((Controls_TextBox)(this.FV_Bom.FindControl("tbRegion"))).Text = string.Empty;
        }
        ((CheckBox)(this.FV_Bom.FindControl("cbIsActive"))).Checked = true;
    }

    protected void CV_ServerValidate(object source, ServerValidateEventArgs args)
    {
        CustomValidator cv = (CustomValidator)source;

        switch (cv.ID)
        {
            case "cvCode":
                if (TheBomMgr.LoadBom(args.Value) != null)
                {
                    ShowWarningMessage("MasterData.Bom.WarningMessage.CodeExist", args.Value);
                    args.IsValid = false;
                }
                //if (TheItemMgr.LoadItem(args.Value) == null)
                //{
                //    ShowWarningMessage("MasterData.Bom.WarningMessage.CodeItem", args.Value);
                //    args.IsValid = false;
                //}
                break;
            case "cvUom":
                if (TheUomMgr.LoadUom(args.Value) == null)
                {
                    ShowWarningMessage("MasterData.Bom.WarningMessage.UomInvalid", args.Value);
                    args.IsValid = false;
                }
                break;
            case "cvRegion":
                if (args.Value.Trim() != "")
                {
                    if (TheRegionMgr.LoadRegion(args.Value) == null)
                    {
                        ShowWarningMessage("MasterData.Bom.WarningMessage.RegionInvalid", args.Value);
                        args.IsValid = false;
                    }
                }
                break;
            default:
                break;
        }
    }

    protected void ODS_Bom_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        string uom = ((Controls_TextBox)(this.FV_Bom.FindControl("tbUom"))).Text.Trim();
        string region = ((Controls_TextBox)(this.FV_Bom.FindControl("tbRegion"))).Text.Trim();

        bom = (Bom)e.InputParameters[0];
        item = TheItemMgr.LoadItem(bom.Code);
        if (item != null)
        {
            //default description and uom
            if (bom.Description.Trim() == "")
            {
                bom.Description = item.Description;
            }
            if (uom.Trim() == string.Empty)
            {
                bom.Uom = item.Uom;
            }
            else
            {
                bom.Uom = TheUomMgr.LoadUom(uom);
            }
        }
        else
        {
            if (uom.Trim() != string.Empty)
            {
                bom.Uom = TheUomMgr.LoadUom(uom);
            }
        }
        if (region == "")
        {
            bom.Region = null;
        }
        else
        {
            bom.Region = TheRegionMgr.LoadRegion(region);
        }
    }

    protected void ODS_Bom_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if (CreateEvent != null)
        {
            if (BomCode != null)
            {
                Bom SourceBom = TheBomMgr.LoadBom(BomCode, true);

                if (SourceBom.BomDetails != null)
                {
                    foreach (BomDetail bomDetail in SourceBom.BomDetails)
                    {
                        bomDetail.Bom = bom;
                        bomDetail.Id = 0;
                        TheBomDetailMgr.CreateBomDetail(bomDetail);
                    }
                }
                CreateEvent(bom.Code, e);
                ShowSuccessMessage("MasterData.Bom.Insert.Successfully", bom.Code);
            }
            else
            {
                CreateEvent(bom.Code, e);
                ShowSuccessMessage("MasterData.Bom.Insert.Successfully", bom.Code);
            }
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (BackEvent != null)
        {
            BackEvent(this, e);
        }
    }
}

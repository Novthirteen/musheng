using System;
using System.Collections;
using System.Web.UI.WebControls;
using com.Sconit.Entity.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity;
using com.Sconit.Web;
using System.Drawing;
using com.Sconit.Entity.Exception;
using com.Sconit.Utility;

public partial class Inventory_InspectOrder_InspectDetailList : ModuleBase
{

    public event EventHandler BackEvent;

    public bool IsPartQualified
    {
        get
        {
            return (bool)ViewState["IsPartQualified"];
        }
        set
        {
            ViewState["IsPartQualified"] = value;
        }
    }
    private bool IsDetailHasHu
    {
        get
        {
            return (bool)ViewState["IsDetailHasHu"];
        }
        set
        {
            ViewState["IsDetailHasHu"] = value;
        }
    }




    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            InspectOrderDetail inspectOrderDetail = (InspectOrderDetail)e.Row.DataItem;

            decimal qualifiedQty = inspectOrderDetail.QualifiedQty.HasValue ? (decimal)inspectOrderDetail.QualifiedQty : 0;
            decimal rejectedQty = inspectOrderDetail.RejectedQty.HasValue ? (decimal)inspectOrderDetail.RejectedQty : 0;
            if (inspectOrderDetail.InspectQty > qualifiedQty + rejectedQty)
            {
                ((TextBox)e.Row.FindControl("tbCurrentQualifiedQty")).ReadOnly = false;
                ((TextBox)e.Row.FindControl("tbCurrentRejectedQty")).ReadOnly = false;

                ((com.Sconit.Control.CodeMstrLabel)e.Row.FindControl("lblDisposition")).Visible = false;

            }
            else
            {
                //((CheckBox)e.Row.FindControl("CheckBoxGroup")).Visible = false;

                ((com.Sconit.Control.CodeMstrDropDownList)e.Row.FindControl("ddlDisposition")).Visible = false;


            }
        }
    }

    public void InitPageParameter(string inspectNo)
    {

        InitPageParameter(inspectNo, false);
    }

    public void InitPageParameter(string inspectNo, bool isWorkShop)
    {
        InspectOrder inspectOrder = TheInspectOrderMgr.LoadInspectOrder(inspectNo, true);
        this.GV_List.DataSource = inspectOrder.InspectOrderDetails;
        this.GV_List.DataBind();

        this.GV_List.Columns[8].Visible = inspectOrder.IsDetailHasHu;

        this.IsDetailHasHu = inspectOrder.IsDetailHasHu;

        if (this.IsPartQualified || !inspectOrder.IsDetailHasHu)
        {
            this.GV_List.Columns[12].Visible = true;
            this.GV_List.Columns[13].Visible = true;
            //  this.GV_List.Columns[0].Visible = false;
        }
        else
        {
            //this.GV_List.Columns[0].Visible = true;
            this.GV_List.Columns[12].Visible = false;
            this.GV_List.Columns[13].Visible = false;
        }

        if (isWorkShop)
        {
            this.GV_List.Columns[10].Visible = false;
            this.GV_List.Columns[11].Visible = false;
            this.GV_List.Columns[12].Visible = false;
            this.GV_List.Columns[13].Visible = false;
            this.GV_List.Columns[14].Visible = false;
        }


    }


    public IList<InspectOrderDetail> PopulateInspectOrder(bool? isQualified)
    {
        IList<InspectOrderDetail> inspectOrderDetailList = new List<InspectOrderDetail>();
        if (this.IsPartQualified || !this.IsDetailHasHu)
        {
            #region 允许部分合格/按照数量
            for (int i = 0; i < this.GV_List.Rows.Count; i++)
            {
                GridViewRow row = this.GV_List.Rows[i];
                TextBox tbCurrentQualifiedQty = (TextBox)row.FindControl("tbCurrentQualifiedQty");
                decimal currentQualifiedQty = 0;
                if (tbCurrentQualifiedQty.Text.Trim() != string.Empty)
                {
                    currentQualifiedQty = decimal.Parse(tbCurrentQualifiedQty.Text.Trim());
                }


                TextBox tbCurrentRejectedQty = (TextBox)row.FindControl("tbCurrentRejectedQty");
                decimal currentRejectedQty = 0;
                if (tbCurrentRejectedQty.Text.Trim() != string.Empty)
                {
                    currentRejectedQty = decimal.Parse(tbCurrentRejectedQty.Text.Trim());
                }
                if (currentQualifiedQty != 0 || currentRejectedQty != 0)
                {
                    HiddenField hfId = (HiddenField)row.FindControl("hfId");
                    InspectOrderDetail inspectOrderDetail = TheInspectOrderDetailMgr.LoadInspectOrderDetail(int.Parse(hfId.Value));
                    inspectOrderDetail.CurrentQualifiedQty = currentQualifiedQty;
                    inspectOrderDetail.CurrentRejectedQty = currentRejectedQty;
                    com.Sconit.Control.DropDownList ddlDisposition = (com.Sconit.Control.DropDownList)row.FindControl("ddlDisposition");
                    if (ddlDisposition.SelectedIndex != -1 && inspectOrderDetail.CurrentRejectedQty > 0)
                    {
                        inspectOrderDetail.Disposition = ddlDisposition.SelectedValue;
                    }
                    inspectOrderDetailList.Add(inspectOrderDetail);
                }
            }
            #endregion
        }
        else
        {
            #region 全部合格/不合格
            for (int i = 0; i < this.GV_List.Rows.Count; i++)
            {
                GridViewRow row = this.GV_List.Rows[i];
                CheckBox checkBoxGroup = row.FindControl("CheckBoxGroup") as CheckBox;
                if (checkBoxGroup.Checked)
                {
                    HiddenField hfId = (HiddenField)row.FindControl("hfId");
                    InspectOrderDetail inspectOrderDetail = TheInspectOrderDetailMgr.LoadInspectOrderDetail(int.Parse(hfId.Value));
                    if ((bool)isQualified)
                    {
                        inspectOrderDetail.CurrentQualifiedQty = inspectOrderDetail.InspectQty;
                    }
                    else
                    {
                        inspectOrderDetail.CurrentRejectedQty = inspectOrderDetail.InspectQty;
                    }

                    com.Sconit.Control.DropDownList ddlDisposition = (com.Sconit.Control.DropDownList)row.FindControl("ddlDisposition");
                    if (ddlDisposition.SelectedIndex != -1 && inspectOrderDetail.CurrentRejectedQty > 0)
                    {
                        inspectOrderDetail.Disposition = ddlDisposition.SelectedValue;
                    }

                    inspectOrderDetailList.Add(inspectOrderDetail);
                }

            }
            #endregion
        }

        return inspectOrderDetailList;

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

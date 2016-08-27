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
using com.Sconit.Entity;
using NHibernate.Expression;

public partial class MasterData_Bom_BomDetail_List : ListModuleBase
{
    public EventHandler EditEvent;

    public bool IsView
    {
        get
        {
            if (ViewState["IsView"] == null)
            {
                return false;
            }
            else
            {
                return (bool)ViewState["IsView"];
            }
        }
        set
        {
            ViewState["IsView"] = value;
        }
    }

    public string BomCode
    {
        get { return (string)ViewState["BomCode"]; }
        set { ViewState["BomCode"] = value; }
    }

    public string ItemCode
    {
        get { return (string)ViewState["ItemCode"]; }
        set { ViewState["ItemCode"] = value; }
    }

    public string StartDate
    {
        get { return (string)ViewState["StartDate"]; }
        set { ViewState["StartDate"] = value; }
    }

    public bool IsIncludeInactive
    {
        get
        {
            if (ViewState["IsIncludeInactive"] == null)
            {
                return false;
            }
            else
            {
                return (bool)ViewState["IsIncludeInactive"];
            }
        }
        set
        {
            ViewState["IsIncludeInactive"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public override void UpdateView()
    {
        this.IsExport = false;
        //this.GV_List.Execute();
        this.InitPageParameter(this.BomCode, this.ItemCode, this.StartDate, this.IsIncludeInactive, false, 0);
    }


    public void InitPageParameter(string bomCode, string itemCode, string startDate, bool isIncludeInactive, bool isAddNew, int editRow)
    {
        this.BomCode = bomCode;
        this.ItemCode = ItemCode;
        this.StartDate = startDate;
        this.IsIncludeInactive = isIncludeInactive;

        DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(BomDetail)).SetProjection(Projections.Count("Id"));
        DetachedCriteria selectCriteria = DetachedCriteria.For(typeof(BomDetail));
        selectCriteria.CreateAlias("Bom", "b");
        selectCountCriteria.CreateAlias("Bom", "b");
        if (!string.IsNullOrEmpty(bomCode))
        {
            selectCriteria.Add(Expression.Like("b.Code", bomCode, MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("b.Code", bomCode, MatchMode.Anywhere));
        }
        if (!string.IsNullOrEmpty(itemCode))
        {
            selectCriteria.Add(Expression.Like("Item.Code", itemCode, MatchMode.Anywhere));
            selectCountCriteria.Add(Expression.Like("Item.Code", itemCode, MatchMode.Anywhere));
        }

        if (IsIncludeInactive == false)
        {
            selectCriteria.Add(Expression.Eq("b.IsActive", true));
            selectCountCriteria.Add(Expression.Eq("b.IsActive", true));
        }

        selectCriteria.Add(Expression.Le("StartDate", DateTime.Now));
        selectCriteria.Add(Expression.Or(Expression.Ge("EndDate", DateTime.Now), Expression.IsNull("EndDate")));
        selectCountCriteria.Add(Expression.Le("StartDate", DateTime.Now));
        selectCountCriteria.Add(Expression.Or(Expression.Ge("EndDate", DateTime.Now), Expression.IsNull("EndDate")));
        //this.GV_List.SelectCriteria = selectCriteria;
        //var recordCount = this.TheCriteriaMgr.FindAll(selectCountCriteria)[0];
        //var recordCount = this.TheCriteriaMgr.FindAll(selectCriteria);
        this.SetSearchCriteria(selectCriteria, selectCountCriteria);
        com.Sconit.Control.GridPager pager = GV_List.FindPager();
        int pageSize = pager.PageSize;
        int pageIndex = pager.CurrentPageIndex;
        int firstRow = (pageIndex - 1) * pageSize;
        int maxRows = pageSize;
        pager.RecordCount = FindCount(selectCountCriteria);
        var list = TheCriteriaMgr.FindAll<BomDetail>(selectCriteria, firstRow, maxRows);

        if (isAddNew == true)
        {
            if (list.Count == pageSize)
            {
                list.RemoveAt(list.Count - 1);                
            }
            var bomDet = new BomDetail();
            bomDet.IsNewRow = true;
            bomDet.StartDate = DateTime.Now;
            list.Insert(0, bomDet);
            
        }

        if (editRow > 0)
        {
            var editBomDet = list.Where(b => b.Id == editRow).FirstOrDefault();
            editBomDet.IsEditRow = true;
        }


        this.GV_List.DataSource = list;
        this.GV_List.DataBind();
        //DetachedCriteria selectCountCriteria = DetachedCriteria.For(typeof(BomDetail));
        

        //if (!string.IsNullOrEmpty(flowCode))
        //{
        //    selectCriteria.Add(Expression.Eq("Flow", flowCode))
        //        .Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS))
        //        .Add(Expression.Eq("SubType", this.ModuleSubType))
        //        .AddOrder(Order.Asc("WindowTime"));
        //}
        //else
        //{
        //    selectCriteria.Add(Expression.Eq("Status", BusinessConstants.CODE_MASTER_STATUS_VALUE_INPROCESS))
        //        .Add(Expression.Eq("SubType", this.ModuleSubType))
        //        .AddOrder(Order.Asc("WindowTime"));
        //}

        //if (!string.IsNullOrEmpty(ItemCode))
        //{
        //    selectCriteria.CreateCriteria("OrderDetails").Add(Expression.Eq("Item.Code", ItemCode));
        //}

        //if (isSupplier == false)
        //{
        //    if (!string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
        //    {
        //        selectCriteria.Add(Expression.Ge("WindowTime", DateTime.Parse(startDate)));
        //    }
        //    else if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
        //    {
        //        selectCriteria.Add(Expression.Ge("WindowTime", DateTime.Parse(startDate))).Add(Expression.Le("WindowTime", DateTime.Parse(endDate)));
        //    }
        //    else if (string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
        //    {
        //        selectCriteria.Add(Expression.Le("WindowTime", DateTime.Parse(endDate)));
        //    }
        //}
        //else
        //{
        //    if (!string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
        //    {
        //        selectCriteria.Add(Expression.Ge("StartDate", DateTime.Parse(startDate)));
        //    }
        //    else if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
        //    {
        //        selectCriteria.Add(Expression.Ge("StartDate", DateTime.Parse(startDate))).Add(Expression.Le("WindowTime", DateTime.Parse(endDate)));
        //    }
        //    else if (string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
        //    {
        //        selectCriteria.Add(Expression.Le("StartDate", DateTime.Parse(endDate)));
        //    }
        //}

        //IList<OrderHead> orderList = new List<OrderHead>(); IList<OrderDetail> orderDetailList = new List<OrderDetail>();
        //IList<OrderHead> orderHeadList = TheCriteriaMgr.FindAll<OrderHead>(selectCriteria);
        //foreach (OrderHead orderHead in orderHeadList)
        //{
        //    //IList<OrderDetail> orderDetailList = orderHead.OrderDetails;
        //    if (orderHead.OrderDetails.Count > 0)
        //    {
        //        foreach (OrderDetail orderDetail in orderHead.OrderDetails)
        //        {
        //            if (orderDetail.RemainShippedQty > 0)
        //            {
        //                if (!string.IsNullOrEmpty(ItemCode))
        //                {
        //                    if (orderDetail.Item.Code == ItemCode)
        //                    {
        //                        if (showChecked)
        //                        {
        //                            if (orderDetIdList.Contains(orderDetail.Id))
        //                            {
        //                                orderDetailList.Add(orderDetail);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            orderDetailList.Add(orderDetail);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (showChecked)
        //                    {
        //                        if (orderDetIdList.Contains(orderDetail.Id))
        //                        {
        //                            orderDetailList.Add(orderDetail);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        orderDetailList.Add(orderDetail);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //this.GV_List.DataSource = orderDetailList;
        //this.GV_List.DataBind();
    }


    public int FindCount(DetachedCriteria selectCriteria)
    {
        IList list = TheCriteriaMgr.FindAll(selectCriteria);
        if (list != null && list.Count > 0)
        {
            if (list[0] is int)
            {
                return int.Parse(list[0].ToString());
            }
            else if (list[0] is object[])
            {
                return int.Parse(((object[])list[0])[0].ToString());
            }
            //由于性能问题,此后禁用该方法。
            //else if (list[0] is object)
            //{
            //    return list.Count;
            //}
            else
            {
                throw new Exception("unknow result type");
            }
        }
        else
        {
            return 0;
        }
    }

    public void Export()
    {
        this.IsExport = true;

        GV_List.Columns[15].Visible = true;
        GV_List.Columns[16].Visible = true;

        GV_List.Columns.RemoveAt(17);
        GV_List.Columns.RemoveAt(4);
        //GV_List.Columns.RemoveAt(0);

        GV_List.ExportXLS("BomDetailSample.xls");

        //this.ExportXLS(GV_List);
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            BomDetail bomDetail = (BomDetail)e.Row.DataItem;
            if (this.IsExport)
            {
                //e.Row.Cells[5].Text = TheCodeMasterMgr.GetCachedCodeMaster(BusinessConstants.CODE_MASTER_BOM_DETAIL_TYPE, e.Row.Cells[5].Text.Trim()).Description;
                //e.Row.Cells[0].Text = bomDetail.Operation.ToString();
                e.Row.Cells[11].Text = bomDetail.BackFlushMethod;
                e.Row.Cells[12].Text = bomDetail.IsShipScanHu.ToString();
                e.Row.Cells[13].Text = bomDetail.NeedPrint.ToString();
            }
            else
            {
                //e.Row.Cells[7].Text = TheCodeMasterMgr.GetCachedCodeMaster(BusinessConstants.CODE_MASTER_BOM_DETAIL_TYPE, e.Row.Cells[7].Text.Trim()).Description;
            }
            if (this.IsView)
            {
                ((LinkButton)e.Row.FindControl("lbtnDelete")).Visible = false;
            }
            if (bomDetail.IsNewRow == true )
            {
                ((Label)e.Row.FindControl("lblOperation")).Visible = false;
                ((TextBox)e.Row.FindControl("tbOperation")).Visible = true;

                ((Label)e.Row.FindControl("lblReference")).Visible = false;
                ((TextBox)e.Row.FindControl("tbReference")).Visible = true;

                ((Label)e.Row.FindControl("lblParCode")).Visible = false;
                Controls_TextBox tbParCode = (Controls_TextBox)e.Row.FindControl("tbParCode");
                tbParCode.Visible = true;
                tbParCode.SuggestTextBox.Attributes.Add("onchange", "GenerateBom(this);");

                ((Label)e.Row.FindControl("lblParUom")).Visible = false;
                ((TextBox)e.Row.FindControl("tbParUom")).Visible = true;

                ((Label)e.Row.FindControl("lblCompUom")).Visible = false;
                ((TextBox)e.Row.FindControl("tbCompUom")).Visible = true;

                ((Label)e.Row.FindControl("lblStartDate")).Visible = false;
                ((TextBox)e.Row.FindControl("tbStartDate")).Visible = true;

                ((Label)e.Row.FindControl("lblEndDate")).Visible = false;
                ((TextBox)e.Row.FindControl("tbEndDate")).Visible = true;

                ((Label)e.Row.FindControl("lblRateQty")).Visible = false;
                ((TextBox)e.Row.FindControl("tbRateQty")).Visible = true;

                ((Label)e.Row.FindControl("lblScrapPercentage")).Visible = false;
                ((TextBox)e.Row.FindControl("tbScrapPercentage")).Visible = true;

                ((Label)e.Row.FindControl("lblStructureType")).Visible = false;
                Controls_TextBox tbStructureType = (Controls_TextBox)e.Row.FindControl("tbStructureType");
                tbStructureType.Visible = true;

                ((Label)e.Row.FindControl("lblIsShipScanHu")).Visible = false;
                ((CheckBox)e.Row.FindControl("cbIsShipScanHu")).Visible = true;

                ((CheckBox)e.Row.FindControl("cbNeedPrint")).Enabled = true;

                ((com.Sconit.Control.CodeMstrLabel)e.Row.FindControl("lblBackFlushMethod")).Visible = false;
                com.Sconit.Control.CodeMstrDropDownList ddlBackFlushMethod = (com.Sconit.Control.CodeMstrDropDownList)e.Row.FindControl("ddlBackFlushMethod");
                ddlBackFlushMethod.Visible = true;

                ((LinkButton)e.Row.FindControl("lbtnEdit")).Visible = false;
                ((LinkButton)e.Row.FindControl("lbtnView")).Visible = false;
                ((LinkButton)e.Row.FindControl("lbtnDelete")).Visible = false;
                ((LinkButton)e.Row.FindControl("lbtnSave")).Visible = true;
                ((LinkButton)e.Row.FindControl("lbtnCancel")).Visible = true;

                ((Label)e.Row.FindControl("lblCompCode")).Visible = false;
                Controls_TextBox tbCompCode = (Controls_TextBox)e.Row.FindControl("tbCompCode");
                tbCompCode.Visible = true;
                tbCompCode.SuggestTextBox.Attributes.Add("onchange", "GenerateComp(this);");
            }

            if (bomDetail.IsEditRow == true)
            {
                ((Label)e.Row.FindControl("lblOperation")).Visible = false;
                ((TextBox)e.Row.FindControl("tbOperation")).Visible = true;

                ((Label)e.Row.FindControl("lblReference")).Visible = false;
                ((TextBox)e.Row.FindControl("tbReference")).Visible = true;

                ((Label)e.Row.FindControl("lblStartDate")).Visible = false;
                ((TextBox)e.Row.FindControl("tbStartDate")).Visible = true;

                ((Label)e.Row.FindControl("lblEndDate")).Visible = false;
                ((TextBox)e.Row.FindControl("tbEndDate")).Visible = true;

                ((Label)e.Row.FindControl("lblRateQty")).Visible = false;
                ((TextBox)e.Row.FindControl("tbRateQty")).Visible = true;

                ((Label)e.Row.FindControl("lblScrapPercentage")).Visible = false;
                ((TextBox)e.Row.FindControl("tbScrapPercentage")).Visible = true;

                //((Label)e.Row.FindControl("lblStructureType")).Visible = false;
                //Controls_TextBox tbStructureType = (Controls_TextBox)e.Row.FindControl("tbStructureType");
                //tbStructureType.Visible = true;

                ((Label)e.Row.FindControl("lblIsShipScanHu")).Visible = false;
                ((CheckBox)e.Row.FindControl("cbIsShipScanHu")).Visible = true;

                ((CheckBox)e.Row.FindControl("cbNeedPrint")).Enabled = true;

                ((com.Sconit.Control.CodeMstrLabel)e.Row.FindControl("lblBackFlushMethod")).Visible = false;
                com.Sconit.Control.CodeMstrDropDownList ddlBackFlushMethod = (com.Sconit.Control.CodeMstrDropDownList)e.Row.FindControl("ddlBackFlushMethod");
                ddlBackFlushMethod.Visible = true;

                ((LinkButton)e.Row.FindControl("lbtnEdit")).Visible = false;
                ((LinkButton)e.Row.FindControl("lbtnView")).Visible = false;
                ((LinkButton)e.Row.FindControl("lbtnDelete")).Visible = false;
                ((LinkButton)e.Row.FindControl("lbtnSave")).Visible = true;
                ((LinkButton)e.Row.FindControl("lbtnCancel")).Visible = true;
            }
        }
    }

    protected void lbtnSave_Click(object sender, EventArgs e)
    {
        LinkButton lbtn = (LinkButton)sender;
        //Get the row that contains this button
        GridViewRow gvr = (GridViewRow)lbtn.NamingContainer;
        var bomDetId = int.Parse(lbtn.CommandArgument);
        if (bomDetId == 0)
        {
            BomDetail bomDet = new BomDetail();
            Controls_TextBox tbParCode = (Controls_TextBox)gvr.FindControl("tbParCode");
            Controls_TextBox tbCompCode = (Controls_TextBox)gvr.FindControl("tbCompCode");
            Controls_TextBox tbStructureType = (Controls_TextBox)gvr.FindControl("tbStructureType");
            com.Sconit.Control.CodeMstrDropDownList ddlBackFlushMethod = (com.Sconit.Control.CodeMstrDropDownList)gvr.FindControl("ddlBackFlushMethod");
            TextBox tbOperation = ((TextBox)gvr.FindControl("tbOperation"));
            TextBox tbReference = ((TextBox)gvr.FindControl("tbReference"));
            TextBox tbStartDate = ((TextBox)gvr.FindControl("tbStartDate"));
            TextBox tbEndDate = ((TextBox)gvr.FindControl("tbEndDate"));
            TextBox tbRateQty = ((TextBox)gvr.FindControl("tbRateQty"));
            TextBox tbScrapPercentage = ((TextBox)gvr.FindControl("tbScrapPercentage"));
            CheckBox cbIsShipScanHu = ((CheckBox)gvr.FindControl("cbIsShipScanHu"));
            CheckBox cbNeedPrint = ((CheckBox)gvr.FindControl("cbNeedPrint"));

            if (string.IsNullOrEmpty(tbParCode.Text) || string.IsNullOrEmpty(tbCompCode.Text))
            {
                ShowErrorMessage("Bom主键或者组件的零件号未填写");
                return;
            }

            if (string.IsNullOrEmpty(ddlBackFlushMethod.Text))
            {
                ShowErrorMessage("回冲方式未填写");
                return;
            }

            if (string.IsNullOrEmpty(tbStructureType.Text))
            {
                ShowErrorMessage("类型未填写");
                return;
            }

            if (string.IsNullOrEmpty(tbRateQty.Text))
            {
                ShowErrorMessage("用量未填写");
                return;
            }

            bomDet.Bom = TheBomMgr.LoadBom(tbParCode.Text, false);
            bomDet.Item = TheItemMgr.LoadItem(tbCompCode.Text);
            bomDet.Uom = bomDet.Item.Uom;
            bomDet.RateQty = decimal.Parse(tbRateQty.Text);
            bomDet.Reference = tbReference.Text;
            bomDet.StructureType = tbStructureType.Text;
            if (!string.IsNullOrEmpty(tbScrapPercentage.Text))
            {
                bomDet.ScrapPercentage = decimal.Parse(tbScrapPercentage.Text);
            }
            bomDet.StartDate = string.IsNullOrEmpty(tbStartDate.Text) ? DateTime.Now : DateTime.Parse(tbStartDate.Text);
            if (!string.IsNullOrEmpty(tbEndDate.Text))
            {
                bomDet.EndDate = DateTime.Parse(tbEndDate.Text);
            }
            //bomDet.EndDate = string.IsNullOrEmpty(tbEndDate.Text) ? DateTime.MaxValue : DateTime.Parse(tbEndDate.Text);
            bomDet.BackFlushMethod = ddlBackFlushMethod.Text;
            bomDet.IsShipScanHu = cbIsShipScanHu.Checked;
            bomDet.NeedPrint = cbNeedPrint.Checked;
            bomDet.Operation = int.Parse(tbOperation.Text);
            bomDet.Location = null;


            TheBomDetailMgr.CreateBomDetail(bomDet);
            ShowSuccessMessage("创建成功");
            this.UpdateView();
        }
        else
        {
            BomDetail bomDet = TheBomDetailMgr.LoadBomDetail(bomDetId);
            TextBox tbStartDate = ((TextBox)gvr.FindControl("tbStartDate"));
            TextBox tbRateQty = ((TextBox)gvr.FindControl("tbRateQty"));
            TextBox tbScrapPercentage = ((TextBox)gvr.FindControl("tbScrapPercentage"));
            CheckBox cbIsShipScanHu = ((CheckBox)gvr.FindControl("cbIsShipScanHu"));
            CheckBox cbNeedPrint = ((CheckBox)gvr.FindControl("cbNeedPrint"));

            bomDet.RateQty = decimal.Parse(tbRateQty.Text);
            bomDet.IsShipScanHu = cbIsShipScanHu.Checked;
            bomDet.NeedPrint = cbNeedPrint.Checked;
            bomDet.RateQty = decimal.Parse(tbRateQty.Text);
            TheBomDetailMgr.UpdateBomDetail(bomDet);
            ShowSuccessMessage("修改成功");
            this.UpdateView();
        }
        
    }

    protected void lbtnCancel_Click(object sender, EventArgs e)
    {
        this.UpdateView();
    }
    

    protected void lbtnEdit_Click(object sender, EventArgs e)
    {
        var lbtn = (LinkButton)sender;

        this.InitPageParameter(this.BomCode, this.ItemCode, this.StartDate, this.IsIncludeInactive, false, int.Parse(lbtn.CommandArgument));
    }

    protected void lbtnView_Click(object sender, EventArgs e)
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
            TheBomDetailMgr.DeleteBomDetail(Convert.ToInt32(code));
            ShowSuccessMessage("Common.Business.Result.Delete.Successfully");
            UpdateView();
        }
        catch (Castle.Facilities.NHibernateIntegration.DataException ex)
        {
            ShowErrorMessage("Common.Business.Result.Delete.Failed");
        }
        catch (Exception ex)
        {
            ShowErrorMessage("MasterData.BomDetail.WarningMessage.CreateDOrder");
        }
    }
}

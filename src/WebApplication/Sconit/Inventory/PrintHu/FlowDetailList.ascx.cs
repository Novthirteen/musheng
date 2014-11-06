using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Exception;


public partial class Inventory_PrintHu_FlowDetailList : ModuleBase
{
    public string ModuleType
    {
        get
        {
            return (string)ViewState["ModuleType"];
        }
        set
        {
            ViewState["ModuleType"] = value;
        }
    }

    protected string PartyFromCode
    {
        get
        {
            return (string)ViewState["PartyFromCode"];
        }
        set
        {
            ViewState["PartyFromCode"] = value;
        }
    }

    protected string PartyToCode
    {
        get
        {
            return (string)ViewState["PartyToCode"];
        }
        set
        {
            ViewState["PartyToCode"] = value;
        }
    }

    protected string FlowCode
    {
        get
        {
            return (string)ViewState["FlowCode"];
        }
        set
        {
            ViewState["FlowCode"] = value;
        }
    }

    public string FlowType
    {
        get
        {
            return (string)ViewState["FlowType"];
        }
        set
        {
            ViewState["FlowType"] = value;
        }
    }


    public void InitPageParameter(Flow flow)
    {
        this.PartyFromCode = flow.PartyFrom.Code;
        this.PartyToCode = flow.PartyTo.Code;
        this.FlowType = flow.Type;
        this.FlowCode = flow.Code;

        int seqInterval = int.Parse(TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_SEQ_INTERVAL).Value);

        if (flow.AllowCreateDetail && false) //新增的Detail打印有问题，暂时不支持
        {
            FlowDetail blankFlowDetail = new FlowDetail();
            if (flow.FlowDetails == null || flow.FlowDetails.Count == 0)
            {
                blankFlowDetail.Sequence = seqInterval;
            }
            else
            {
                int CurrentSeq = flow.FlowDetails.Last<FlowDetail>().Sequence + seqInterval;
                blankFlowDetail.Sequence = CurrentSeq;
            }
            blankFlowDetail.IsBlankDetail = true;
            flow.AddFlowDetail(blankFlowDetail);
        }

        #region 设置默认LotNo
        string lotNo = LotNoHelper.GenerateLotNo();
        foreach (FlowDetail flowDetail in flow.FlowDetails)
        {
            flowDetail.HuLotNo = lotNo;
        }
        #endregion

        this.GV_List.DataSource = flow.FlowDetails;
        this.GV_List.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            FlowDetail flowDetail = (FlowDetail)e.Row.DataItem;

            TextBox tbSortLevel1 = (TextBox)e.Row.FindControl("tbSortLevel1");
            TextBox tbColorLevel1 = (TextBox)e.Row.FindControl("tbColorLevel1");
            TextBox tbSortLevel2 = (TextBox)e.Row.FindControl("tbSortLevel2");
            TextBox tbColorLevel2 = (TextBox)e.Row.FindControl("tbColorLevel2");
            TextBox tbManufactureDate = (TextBox)e.Row.FindControl("tbManufactureDate");

            tbManufactureDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            if (flowDetail.Item.IsSortAndColor.HasValue && flowDetail.Item.IsSortAndColor.Value)
            {
                tbSortLevel1.Visible = true;
                tbColorLevel1.Visible = true;
                tbSortLevel2.Visible = true;
                tbColorLevel2.Visible = true;
            }
            else
            {
                tbSortLevel1.Visible = false;
                tbColorLevel1.Visible = false;
                tbSortLevel2.Visible = false;
                tbColorLevel2.Visible = false;
            }
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            IList<FlowDetail> flowDetailList = this.PopulateFlowDetailList(false);
            IList<FlowDetail> targetFlowDetailList = new List<FlowDetail>();

            if (flowDetailList != null && flowDetailList.Count > 0)
            {
                foreach (FlowDetail flowDetail in flowDetailList)
                {
                    if (flowDetail.OrderedQty > 0)
                    {
                        targetFlowDetailList.Add(flowDetail);
                    }
                }
            }

            if (targetFlowDetailList.Count == 0)
            {
                this.ShowErrorMessage("Inventory.Error.PrintHu.FlowDetail.Required");
                return;
            }

            IList<Hu> huList = null;

            #region  内/外包装
            string packageType = null;
            RadioButtonList rblPackageType = (RadioButtonList)this.Parent.FindControl("rblPackageType");
            if (rblPackageType.SelectedValue == "0")
            {
                packageType = BusinessConstants.CODE_MASTER_PACKAGETYPE_INNER;
            }
            else if (rblPackageType.SelectedValue == "1")
            {
                packageType = BusinessConstants.CODE_MASTER_PACKAGETYPE_OUTER;
            }
            #endregion

            if (this.ModuleType == BusinessConstants.CODE_MASTER_PARTY_TYPE_VALUE_SUPPLIER)
            {
                huList = TheHuMgr.CreateHu(targetFlowDetailList, this.CurrentUser, null, packageType);
            }
            else
            {
                EntityPreference entityPreference = this.TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_COMPANY_ID_MARK);
                huList = TheHuMgr.CreateHu(targetFlowDetailList, this.CurrentUser, entityPreference.Value, packageType);
            }

            String huTemplate = "";
            if (this.ModuleType == BusinessConstants.CODE_MASTER_PARTY_TYPE_VALUE_REGION)
            {
                huTemplate = TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_DEFAULT_HU_TEMPLATE).Value;
            }
            else if (targetFlowDetailList != null
                        && targetFlowDetailList.Count > 0
                        && targetFlowDetailList[0].Flow != null
                        && targetFlowDetailList[0].Flow.HuTemplate != null
                        && targetFlowDetailList[0].Flow.HuTemplate.Length > 0)
            {
                huTemplate = targetFlowDetailList[0].Flow.HuTemplate;
            }

            if (huTemplate != null && huTemplate.Length > 0)
            {

                IList<object> huDetailObj = new List<object>();

                huDetailObj.Add(huList);
                huDetailObj.Add(CurrentUser.Code);

                string barCodeUrl = "";
                if (true || packageType == BusinessConstants.CODE_MASTER_PACKAGETYPE_OUTER)
                {
                    //"BarCode.xls"
                    //targetFlowDetailList[0].Flow.HuTemplate
                    barCodeUrl = TheReportMgr.WriteToFile(huTemplate, huDetailObj, huTemplate);
                }
                else
                {
                    //"InsideBarCodeA4.xls" 
                    barCodeUrl = TheReportMgr.WriteToFile("Inside" + huTemplate, huDetailObj, "Inside" + huTemplate);
                }
                Page.ClientScript.RegisterStartupScript(GetType(), "method", " <script language='javascript' type='text/javascript'>PrintOrderByQty('" + barCodeUrl + "',"+ this.tbCopies.Text.Trim()+ "); </script>");
				//Page.ClientScript.RegisterStartupScript(GetType(), "method", " <script language='javascript' type='text/javascript'>PrintOrder('" + barCodeUrl + "'); </script>");

                this.ShowSuccessMessage("Inventory.PrintHu.Successful");
            }
        }
        catch (BusinessErrorException ex)
        {
            this.ShowErrorMessage(ex);
        }
    }

    //返回订单明细
    private IList<FlowDetail> PopulateFlowDetailList(bool includeBlank)
    {
        if (this.GV_List.Rows != null && this.GV_List.Rows.Count > 0)
        {
            Flow flow = null;
            IList<FlowDetail> flowDetailList = new List<FlowDetail>();

            int seqInterval = int.Parse(TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_SEQ_INTERVAL).Value);
            int lastSeq = 0;

            foreach (GridViewRow row in this.GV_List.Rows)
            {
                HiddenField hfId = (HiddenField)row.FindControl("hfId");
                TextBox tbManufactureDate = (TextBox)row.FindControl("tbManufactureDate");
                TextBox tbOrderQty = (TextBox)row.FindControl("tbOrderQty");
                TextBox tbSupplierLotNo = (TextBox)row.FindControl("tbSupplierLotNo");
                TextBox tbSortLevel1 = (TextBox)row.FindControl("tbSortLevel1");
                TextBox tbColorLevel1 = (TextBox)row.FindControl("tbColorLevel1");
                TextBox tbSortLevel2 = (TextBox)row.FindControl("tbSortLevel2");
                TextBox tbColorLevel2 = (TextBox)row.FindControl("tbColorLevel2");

                if (hfId.Value != string.Empty && hfId.Value != "0")
                {
                    if (tbSupplierLotNo.Text.Trim() == string.Empty)
                    {
                        this.ShowErrorMessage("Inventory.PrintHu.Item.SupplierLotNo.Required");
                        return null;
                    }

                    FlowDetail flowDetail = TheFlowDetailMgr.LoadFlowDetail(int.Parse(hfId.Value));
                    //flowDetail.HuLotNo = tbLotNo.Text.Trim() != string.Empty ? tbLotNo.Text.Trim() : null;
                    if (tbManufactureDate.Text.Trim() != string.Empty)
                    {
                        flowDetail.HuLotNo = LotNoHelper.GenerateLotNo(DateTime.Parse(tbManufactureDate.Text.Trim()));
                    }
                    flowDetail.OrderedQty = tbOrderQty.Text != string.Empty ? decimal.Parse(tbOrderQty.Text) : decimal.Zero;
                    flowDetail.HuSupplierLotNo = tbSupplierLotNo.Text != string.Empty ? tbSupplierLotNo.Text.Trim() : null;
                    if (flowDetail.Item.IsSortAndColor.HasValue && flowDetail.Item.IsSortAndColor.Value)
                    {
                        if (flowDetail.Item.SortLevel1From != null && flowDetail.Item.SortLevel1From != string.Empty && flowDetail.Item.SortLevel1From != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                        {
                            flowDetail.HuSortLevel1 = tbSortLevel1.Text != string.Empty ? tbSortLevel1.Text.Trim() : null;
                            flowDetail.HuColorLevel1 = tbColorLevel1.Text != string.Empty ? tbColorLevel1.Text.Trim() : null;
                        }

                        if (flowDetail.Item.SortLevel2From != null && flowDetail.Item.SortLevel2From != string.Empty && flowDetail.Item.SortLevel2From != BusinessConstants.SORT_COLOR_IGNORE_LABEL)
                        {
                            flowDetail.HuSortLevel2 = tbSortLevel2.Text != string.Empty ? tbSortLevel2.Text.Trim() : null;
                            flowDetail.HuColorLevel2 = tbColorLevel2.Text != string.Empty ? tbColorLevel2.Text.Trim() : null;
                        }
                    }

                    flowDetailList.Add(flowDetail);
                    lastSeq = flowDetail.Sequence;
                    if (flow == null)
                    {
                        flow = flowDetail.Flow;
                    }
                }
                else
                {
                    //if (!includeBlank)
                    //{
                    //    continue;
                    //}

                    //if (flow == null)
                    //{
                    //    flow = this.TheFlowMgr.LoadFlow(this.FlowCode);
                    //}

                    //TextBox tbNewSeq = (TextBox)row.FindControl("tbSeq");
                    //Controls_TextBox tbNewItemCode = (Controls_TextBox)row.FindControl("tbItemCode");
                    //Controls_TextBox tbNewUom = (Controls_TextBox)row.FindControl("tbUom");
                    //com.Sconit.Control.CodeMstrDropDownList ddlPackageType = (com.Sconit.Control.CodeMstrDropDownList)row.FindControl("ddlPackageType");
                    //TextBox tbNewUnitCount = (TextBox)row.FindControl("tbUnitCount");
                    //TextBox tbNewOrderQty = (TextBox)row.FindControl("tbOrderQty");

                    //FlowDetail newFlowDetail = new FlowDetail();
                    //newFlowDetail.Sequence = tbNewSeq.Text != string.Empty ? int.Parse(tbNewSeq.Text) : (lastSeq + seqInterval);
                    //newFlowDetail.Item = this.TheItemMgr.LoadItem(tbNewItemCode.Text.Trim());
                    //newFlowDetail.Uom = this.TheUomMgr.LoadUom(tbNewUom.Text.Trim());
                    //newFlowDetail.PackageType = ddlPackageType.SelectedValue;
                    //newFlowDetail.UnitCount = tbNewUnitCount.Text.Trim() != string.Empty ? decimal.Parse(tbNewUnitCount.Text) : 1;
                    //newFlowDetail.OrderedQty = tbNewOrderQty.Text.Trim() == string.Empty ? 0 : decimal.Parse(tbNewOrderQty.Text.Trim());
                    //newFlowDetail.Flow = flow;
                    //newFlowDetail.IsBlankDetail = true;

                    //if (flow.Type != BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
                    //{
                    //    newFlowDetail.HuLotNo = tbLotNo.Text.Trim() != string.Empty ? tbLotNo.Text.Trim() : null;
                    //}
                    //else
                    //{
                    //    newFlowDetail.TextField1 = tbLotNo.Text.Trim() != string.Empty ? tbLotNo.Text.Trim() : null;
                    //    newFlowDetail.HuLotNo = LotNoHelper.GenerateLotNo(winTime.Value);
                    //}

                    //flowDetailList.Add(newFlowDetail);
                }


            }

            return flowDetailList;
        }

        return null;
    }
}

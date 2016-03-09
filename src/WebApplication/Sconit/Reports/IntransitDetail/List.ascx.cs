using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using NHibernate.Expression;
using com.Sconit.Entity.View;
using com.Sconit.Entity.Exception;
using System.Text;
using System.Data.SqlClient;
using System.Data;

public partial class Reports_IntransitDetail_List : ListModuleBase
{
    public event EventHandler DetailEvent;

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

    private string FlowCode
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

    public void Export()
    {
        this.IsExport = true;
        this.ExportXLS(GV_List);
    }


    protected void GV_List_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (this.IsExport && e.Row.RowType == DataControlRowType.DataRow)
        {
            IntransitDetail intransitDetail = (IntransitDetail)e.Row.DataItem;

            this.SetLinkButton(e.Row, "lbtDefaultActivity", new string[] { }, false);
            //e.Row.Cells[1].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
        }
    }

    private void SetLinkButton(GridViewRow gvr, string id, string[] commandArgument, bool enabled)
    {
        LinkButton linkButton = (LinkButton)gvr.FindControl(id);
        linkButton.Enabled = enabled && !IsExport;
        if (enabled)
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < commandArgument.Length; i++)
            {
                if (i > 0)
                    str.Append(",");

                str.Append(commandArgument[i]);
            }
            linkButton.CommandArgument = str.ToString();
        }
    }

    public void InitPageParameter(string[] obj)
    {
        this.FlowCode = obj[0];
        string itemCode = obj[1];
        IList<RoutingDetail> routingDetailList = new List<RoutingDetail>();

        if (FlowCode != null && FlowCode.Trim() != string.Empty)
        {
            try
            {
                Flow flow = this.TheFlowMgr.CheckAndLoadFlow(FlowCode);
                if (flow.Routing != null)
                {
                    routingDetailList = this.TheRoutingDetailMgr.GetRoutingDetail(flow.Routing, DateTime.Now);
                }
            }
            catch (BusinessErrorException ex)
            {
                this.ShowErrorMessage(ex);
                return;
            }
        }

        #region 查询
        IList<SqlParameter> commandParameters = new List<SqlParameter>();;
        string sql = @"select om.OrderNo, im.IpNo, pf.Name, pt.Name, od.Item, i.Desc1 + '[' + ISNULL(i.Desc2, '') + ']', od.RefItemCode, od.Uom, od.UC, id.Qty - id.RecQty
                        from IpMstr as im 
                        inner join IpDet as id on im.IpNo = id.IpNo
                        inner join OrderLocTrans as olt on id.OrderLocTransId = olt.Id
                        inner join OrderDet as od on olt.OrderDetId = od.Id
                        inner join OrderMstr as om on od.OrderNo = om.OrderNo
                        inner join Party as pf on om.PartyFrom = pf.Code
                        inner join Party as pt on om.PartyTo = pt.Code
                        inner join Item as i on od.Item = i.Code
                        WHERE im.Status IN ('Create', 'In-Process') and im.Type = 'Nml' and 
                            id.Qty > id.RecQty and om.Type = 'Procurement' ";

        if (FlowCode != null && FlowCode.Trim() != string.Empty)
        {
            sql += "and om.Flow = @Flow ";
            SqlParameter parameter= new SqlParameter();
            parameter = new SqlParameter("@Flow", SqlDbType.NVarChar, 50);
            parameter.Value = FlowCode;
            commandParameters.Add(parameter);
        }
          
        if (itemCode != null && itemCode.Trim() != string.Empty)
        {
            sql += "and (i.Code like @Item or i.Desc1 like @Item or i.Desc2 like @Item)";

            SqlParameter parameter= new SqlParameter();
            parameter = new SqlParameter("@Item", SqlDbType.NVarChar, 50);
            parameter.Value = itemCode;
            commandParameters.Add(parameter);
        }

        DataSet dataSet = this.TheGenericMgr.GetDatasetBySql(sql, commandParameters.ToArray());
        #endregion



        #region 转换查询结果为IntransitDetail
        if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
        {
            IntransitDetail intransitDetail = null;
            IList<IntransitDetail> intransitDetailList = new List<IntransitDetail>();
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                intransitDetail = new IntransitDetail();
                intransitDetail.OrderNo = dataSet.Tables[0].Rows[i][0] is DBNull ? string.Empty : (string)dataSet.Tables[0].Rows[i][0];
                intransitDetail.IpNo = dataSet.Tables[0].Rows[i][1] is DBNull ? string.Empty : (string)dataSet.Tables[0].Rows[i][1];
                intransitDetail.PartyFrom = dataSet.Tables[0].Rows[i][2] is DBNull ? string.Empty : (string)dataSet.Tables[0].Rows[i][2];
                intransitDetail.PartyTo = dataSet.Tables[0].Rows[i][3] is DBNull ? string.Empty : (string)dataSet.Tables[0].Rows[i][3];
                intransitDetail.ItemCode = dataSet.Tables[0].Rows[i][4] is DBNull ? string.Empty : (string)dataSet.Tables[0].Rows[i][4];
                intransitDetail.ItemName = dataSet.Tables[0].Rows[i][5] is DBNull ? string.Empty : (string)dataSet.Tables[0].Rows[i][5];
                intransitDetail.ReferenceItem = dataSet.Tables[0].Rows[i][6] is DBNull ? string.Empty : (string)dataSet.Tables[0].Rows[i][6];
                intransitDetail.Uom = dataSet.Tables[0].Rows[i][7] is DBNull ? string.Empty : (string)dataSet.Tables[0].Rows[i][7];
                intransitDetail.UnitCount = (decimal)dataSet.Tables[0].Rows[i][8];
                intransitDetail.DefaultActivity = (decimal)dataSet.Tables[0].Rows[i][9];

                intransitDetailList.Add(intransitDetail);

                //switch (FindActvitySeq(routingDetailList, inProcessLocationDetailTrackView.CurrentOperation))
                //{
                //    case 0:
                //        intransitDetail.DefaultActivity += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 1:
                //        intransitDetail.Activity1 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 2:
                //        intransitDetail.Activity2 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 3:
                //        intransitDetail.Activity3 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 4:
                //        intransitDetail.Activity4 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 5:
                //        intransitDetail.Activity5 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 6:
                //        intransitDetail.Activity6 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 7:
                //        intransitDetail.Activity7 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 8:
                //        intransitDetail.Activity8 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 9:
                //        intransitDetail.Activity9 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 10:
                //        intransitDetail.Activity10 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 11:
                //        intransitDetail.Activity11 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 12:
                //        intransitDetail.Activity12 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 13:
                //        intransitDetail.Activity13 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 14:
                //        intransitDetail.Activity14 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 15:
                //        intransitDetail.Activity15 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 16:
                //        intransitDetail.Activity16 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 17:
                //        intransitDetail.Activity17 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 18:
                //        intransitDetail.Activity18 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 19:
                //        intransitDetail.Activity19 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //    case 20:
                //        intransitDetail.Activity20 += inProcessLocationDetailTrackView.Qty;
                //        break;
                //}
            }

            this.lblNoRecordFound.Visible = false;
            this.GV_List.DataSource = intransitDetailList;
            this.GV_List.DataBind();
        }
        else
        {
            this.lblNoRecordFound.Visible = true;
            this.GV_List.DataSource = null;
            this.GV_List.DataBind();
        }
        #endregion

    }

    public override void UpdateView()
    {
        this.IsExport = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void lbtnDetail_Click(object sender, EventArgs e)
    {
        string s = ((LinkButton)sender).CommandArgument;
        string[] array = s.Split('$');
        string itemCode = array[0];
        string uom = array[1];
        decimal unitCount = decimal.Parse(array[2]);
        int position = int.Parse(array[3]);

        DetailEvent(new object[] { this.FlowCode, itemCode, uom, unitCount, position }, e);
    }

    private int FindActvitySeq(IList<RoutingDetail> routingDetailList, int? operation)
    {
        if (!operation.HasValue)
        {
            return 0;
        }

        if (routingDetailList != null && routingDetailList.Count > 0)
        {
            for (int i = 0; i < routingDetailList.Count; i++)
            {
                if (routingDetailList[i].Operation == operation.Value)
                {
                    return i + 1;
                }
            }

            return 0;
        }
        else
        {
            return 0;
        }
    }
}

class IntransitDetail
{
    public string PartyFrom { get; set; }
    public string PartyTo { get; set; }
    public string OrderNo { get; set; }
    public string IpNo { get; set; }
    public string ItemCode { get; set; }
    public string ItemName { get; set; }
    public string ReferenceItem { get; set; }
    public string Uom { get; set; }
    public decimal UnitCount { get; set; }
    public decimal DefaultActivity { get; set; }
    public decimal Activity1 { get; set; }
    public decimal Activity2 { get; set; }
    public decimal Activity3 { get; set; }
    public decimal Activity4 { get; set; }
    public decimal Activity5 { get; set; }
    public decimal Activity6 { get; set; }
    public decimal Activity7 { get; set; }
    public decimal Activity8 { get; set; }
    public decimal Activity9 { get; set; }
    public decimal Activity10 { get; set; }
    public decimal Activity11 { get; set; }
    public decimal Activity12 { get; set; }
    public decimal Activity13 { get; set; }
    public decimal Activity14 { get; set; }
    public decimal Activity15 { get; set; }
    public decimal Activity16 { get; set; }
    public decimal Activity17 { get; set; }
    public decimal Activity18 { get; set; }
    public decimal Activity19 { get; set; }
    public decimal Activity20 { get; set; }
}

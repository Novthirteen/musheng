using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.Quote;
using com.Sconit.Entity.MasterData;
using System.Web.Script.Serialization;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;

public partial class Quote_Quotes_SubmitView : EditModuleBase
{
    protected string Id
    {
        get
        {
            return (string)ViewState["Id"];
        }
        set
        {
            ViewState["Id"] = value;
        }
    }
    protected string ProjectId
    {
        get
        {
            return (string)ViewState["ProjectId"];
        }
        set
        {
            ViewState["ProjectId"] = value;
        }
    }

    protected string Datas
    {
        get
        {
            return (string)ViewState["Datas"];
        }
        set
        {
            ViewState["Datas"] = value;
        }
    }
    public class Json
    {
        public string Name;
        public string Number;
        public string Unit;
        public string Price;
        public string CountPrice;
    }
    public class OutJson
    {
        public string Title;
        public List<Json> Data;
        public string amount;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCooperationMode();
            LoadSType();
            LoadQFor();
            LoadTSType();
            LoadLogisticsMode();

        }
    }

    #region //////////
    //public void InitPageParameter(string id)
    //{
    //    Id = id;
    //    IList<ProductInfo> PList = TheToolingMgr.GetProductInfoById(id);
    //    IList<Project> projectList = null;
    //    if (PList != null)
    //    {
    //        if (PList.Count > 0)
    //        {
    //            ProductInfo p = PList[0];
    //            ProjectId = p.ProjectId;
    //            //CustomerCode = p.CustomerCode;
    //            projectList = TheToolingMgr.GetProjectByProjectId(ProjectId);
    //            if (projectList.Count > 0)
    //            {
    //                Datas = projectList[0].Data;
    //                Project project = projectList[0];
    //                #region Load
    //                ltCustomerName.Text = project.CustomerName;
    //                ltProductName.Text = project.ProductName;
    //                ltProductNo.Text = project.ProductNo;
    //                ltVersionNo.Text = project.VersionNo;
    //                IList<QuoteCustomerInfo> cusList = TheToolingMgr.GetQuoteCustomerInfoByCode(p.CustomerCode, true);
    //                if (cusList.Count > 0)
    //                {
    //                    ltDeliveryAdd.Text = cusList[0].DeliveryAdd;
    //                    if (cusList[0].BillPeriod != null)
    //                    {
    //                        ltBillPeriod.Text = cusList[0].BillPeriod.ToString();
    //                    }
    //                }
    //                ltPT.Text = project.PT;
    //                ltPCBNum.Text = project.PCBNum;
    //                ltDoubleSideMount.Text = project.DoubleSideMount;
    //                ltChipBurning.Text = project.ChipBurning;
    //                ltLightNum.Text = project.LightNum;
    //                ltBoardMode.Text = project.BoardMode;
    //                ltConnPoint.Text = project.ConnPoint;
    //                ltDeviceShaping.Text = project.DeviceShaping;
    //                ltDeviceCoding.Text = project.DeviceCoding;
    //                ltCodingType.Text = project.CodingType;
    //                ltSurfaceCoating.Text = project.SurfaceCoating;
    //                ltCoatingAcreage.Text = project.CoatingAcreage;
    //                ltPackMode.Text = project.PackMode;
    //                ltEachBox.Text = project.EachBox;
    //                dplCooperationMode.Text = project.CooperationMode;
    //                dplIsBack.Text = project.IsBack;
    //                dplLogisticsMode.Text = project.LogisticsMode;
    //                dplQFor.Text = project.QFor;
    //                dplSType.Text = project.SType;
    //                dplTSType.Text = project.TSType;

    //                ltInputUserName.Text = project.InputUserName;
    //                if (project.InputDate != null)
    //                {
    //                    ltInputDate.Text = project.InputDate.ToString();
    //                }
    //                ltMonthlyDemand.Text = project.MonthlyDemand;
    //                #endregion
    //            }
                
    //        }
    //    }
    //    #region
    //    this.ltSalesUP.Text = (decimal.Parse(getCategoryAmount("加工费用")) + decimal.Parse(getCategoryAmount("材料费用"))).ToString();
    //    this.ltSalesUPI.Text = (decimal.Parse(this.ltSalesUP.Text.Trim()) * decimal.Parse("1.17")).ToString();

    //    this.ltLumpSumFee.Text = (decimal.Parse(getCategoryAmount("专用治具")) + decimal.Parse(getCategoryAmount("一次性定单开机费"))).ToString();
    //    this.ltLumpSumFeeI.Text = (decimal.Parse(this.ltLumpSumFee.Text.Trim()) * decimal.Parse("1.17")).ToString();

    //    #endregion
    //}
    #endregion

    public void InitPageParameter(string id)
    {
        Id = id;
        IList<Project> projectList = null;

        projectList = TheToolingMgr.GetProjectById(Id);
        if (projectList.Count > 0)
        {
            Datas = projectList[0].Data;
            Project project = projectList[0];
            #region Load
            ltCustomerName.Text = project.CustomerName;
            ltProductName.Text = project.ProductName;
            ltProductNo.Text = project.ProductNo;
            ltVersionNo.Text = project.VersionNo;
            IList<QuoteCustomerInfo> cusList = TheToolingMgr.GetQuoteCustomerInfoByCode(project.CustomerCode, true);
            if (cusList.Count > 0)
            {
                ltDeliveryAdd.Text = cusList[0].DeliveryAdd;
                if (cusList[0].BillPeriod != null)
                {
                    ltBillPeriod.Text = cusList[0].BillPeriod.ToString();
                }
            }
            ltPT.Text = project.PT;
            ltPCBNum.Text = project.PCBNum;
            ltDoubleSideMount.Text = project.DoubleSideMount;
            ltChipBurning.Text = project.ChipBurning;
            ltLightNum.Text = project.LightNum;
            ltBoardMode.Text = project.BoardMode;
            ltConnPoint.Text = project.ConnPoint;
            ltDeviceShaping.Text = project.DeviceShaping;
            ltDeviceCoding.Text = project.DeviceCoding;
            ltCodingType.Text = project.CodingType;
            ltSurfaceCoating.Text = project.SurfaceCoating;
            ltCoatingAcreage.Text = project.CoatingAcreage;
            ltPackMode.Text = project.PackMode;
            ltEachBox.Text = project.EachBox;
            dplCooperationMode.SelectedValue = project.CooperationMode;
            dplIsBack.SelectedValue = project.IsBack;
            dplLogisticsMode.SelectedValue = project.LogisticsMode;
            dplQFor.SelectedValue = project.QFor;
            dplSType.SelectedValue = project.SType;
            dplTSType.SelectedValue = project.TSType;

            txtInputUserName.Text = project.InputUserName;
            if (project.InputDate != null)
            {
                txtInputDate.Text = DateTime.Parse( project.InputDate.ToString()).ToString("yyyy-MM-dd HH:mm");
            }
            txtMonthlyDemand.Text = project.MonthlyDemand;
            txtPVision.Text = project.PVision;
            #endregion
        }

        #region
        this.ltSalesUP.Text = (decimal.Parse(getCategoryAmount("加工费用")) + decimal.Parse(getCategoryAmount("材料费用"))).ToString();
        this.ltSalesUPI.Text = (decimal.Parse(this.ltSalesUP.Text.Trim()) * decimal.Parse("1.17")).ToString();

        this.ltLumpSumFee.Text = (decimal.Parse(getCategoryAmount("专用治具")) + decimal.Parse(getCategoryAmount("一次性定单开机费"))).ToString();
        this.ltLumpSumFeeI.Text = (decimal.Parse(this.ltLumpSumFee.Text.Trim()) * decimal.Parse("1.17")).ToString();

        #endregion
    }

    public string getCategoryAmount(string name)
    {
        string result = string.Empty;
        IList<Project> projectList = TheToolingMgr.GetProjectByProjectId(ProjectId);
        if(projectList.Count>0)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<OutJson> jsonList = js.Deserialize<List<OutJson>>(projectList[0].Data);
            foreach (OutJson outjson in jsonList)
            { 
                if(outjson.Title == name)
                {
                    result = outjson.amount;
                }
            }
        }
        if (result != string.Empty)
        {
            return result;
        }
        else
        {
            return "0";
        }
            
    }

    public string OutHtml()
    {
        #region
        IList<Project> projectList = TheToolingMgr.GetProjectById(Id);
        StringBuilder sb = new StringBuilder();
        sb.Append("<fieldset>");
        int num = 0;
        int txtNum = 0;
        if (projectList.Count > 0)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<OutJson> jsonList = js.Deserialize<List<OutJson>>(projectList[0].Data);
            foreach (OutJson outjson in jsonList)
            {
                sb.Append("<table class='mtable'>");
                sb.Append("<tr><th style='text-align:left;width:10%'>序号</th><th style='text-align:left;width:20%'>项目构成_" + outjson.Title + "</th><th style='text-align:left;width:20%'>数量</th><th style='text-align:left;width:10%'>单位</th><th style='text-align:left;width:20%'>单价</th><th style='text-align:left;width:20%'>金额<th></tr>");
                foreach (Json json in outjson.Data)
                {
                    num++; txtNum++;
                    sb.Append("<tr>");
                    sb.Append("<td>" + num + "</td>");
                    sb.Append("<td>" + json.Name + "</td>");
                    sb.Append("<td> <input style='width:50px' type='text' id='txtNum" + txtNum + "' value='" + json.Number + "' onkeyup='NumberChange(this)' /></td>");
                    sb.Append("<td>" + json.Unit + "</td>");
                    sb.Append("<td> <input style='width:50px' type='text' id='txtPrice" + txtNum + "' value='" + json.Price + "' onkeyup='PriceChange(this)' /></td>");
                    sb.Append("<td> <input style='width:50px' type='text' id='txtCountPrice" + txtNum + "' value='" + json.CountPrice + "' /> </td>");
                    sb.Append("</tr>");
                }
                sb.Append("<tr><td colspan='5' style='text-align:center'>小计_" + outjson.Title + "</td><td>" +"<lable class='lbAmount"+txtNum+"'>"+ outjson.amount + "</lable></td></tr>");
                sb.Append("</table>");
                num = 0;
            }
            sb.Append("</fieldset>");
        }
        this.hfNum.Value = txtNum.ToString();
        #endregion
        return sb.ToString();
    }

    public void btnExport_Click(object sender, EventArgs e)
    {
        IList<object> list = new List<object>();
        IList<Project> projectList = TheToolingMgr.GetProjectById(Id);
        if (projectList.Count > 0)
        {
            list.Add(projectList);
        }
        TheReportMgr.WriteToClient("QuoteProject.xls", list, "QuoteProject.xls");
    }

    public void btnTest_Click(object sender, EventArgs e)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();
        List<OutJson> jsonList = js.Deserialize<List<OutJson>>(Datas);
        //foreach (OutJson outjson in jsonList)
        //{
        //    foreach (Json json in outjson.Data)
        //    {
        //        json.Number = "111";
        //    }
        //}
        ToJson(jsonList);
    }

    public void ToJson(IList<OutJson> jsonList)
    {
        string[] strsNum = hfNumList.Value.Split(',');
        string[] strsPrice = hfPriceList.Value.Split(',');
        string[] strsCountPrice = hfCountPriceList.Value.Split(',');
        string[] strsAmount = hfAmount.Value.Split(',');
        int num = 0;
        int numAmount = 0;
        StringBuilder sb = new StringBuilder();
        sb.Append("[");
        foreach (OutJson outjson in jsonList)
        {
            sb.Append("{'Title':'" + outjson.Title+"','Data':[");
            foreach (Json json in outjson.Data)
            {
                sb.Append("{'Name':'" + json.Name + "','Number':'" + strsNum[num] + "','Unit':'" + json.Unit + "','Price':'" + strsPrice[num] + "','CountPrice':'" + strsCountPrice[num] + "'},");
                num++;
            }
            sb = sb.Remove(sb.ToString().LastIndexOf(','), 1);
            sb.Append("],'amount':'" + strsAmount[numAmount] + "'},");
            numAmount++;
        }
        string result = sb.ToString().Substring(0, sb.Length - 1) + "]";
        string sql = @"update Quote_Project set Data = @Data,InputUserName = @InputUserName,InputDate = @InputDate,CooperationMode = @CooperationMode,PVision = @PVision,
                        SType = @SType,QFor = @QFor,TSType = @TSType,LogisticsMode = @LogisticsMode,IsBack = @IsBack,PlanAllocationNum = @PlanAllocationNum,MonthlyDemand = @MonthlyDemand
                        where Id = @Id";
        SqlParameter[] sp = new SqlParameter[] 
        { 
            new SqlParameter("@Data", result), 
            new SqlParameter("@Id", Id),
            new SqlParameter("@InputUserName", txtInputUserName.Text.Trim()),
            new SqlParameter("@InputDate", txtInputDate.Text.Trim()),
            new SqlParameter("@CooperationMode", dplCooperationMode.SelectedValue),
            new SqlParameter("@SType", dplSType.SelectedValue),
            new SqlParameter("@QFor", dplQFor.SelectedValue),
            new SqlParameter("@TSType", dplTSType.SelectedValue),
            new SqlParameter("@LogisticsMode", dplLogisticsMode.SelectedValue),
            new SqlParameter("@IsBack", dplIsBack.SelectedValue),
            new SqlParameter("@PlanAllocationNum", ltPlanAllocationNum.Text.Trim()),
            new SqlParameter("@MonthlyDemand", txtMonthlyDemand.Text.Trim()),
            new SqlParameter("@PVision", txtPVision.Text.Trim()),
        };
        TheSqlHelperMgr.Update(sql, sp);
    }

    #region
    void LoadCooperationMode()
    {
        IList<CooperationMode> CooperationModeList = TheToolingMgr.GetCooperationMode();
        CooperationMode c = new CooperationMode();
        CooperationModeList.Add(c);
        if (CooperationModeList.Count > 0)
        {
            dplCooperationMode.DataSource = CooperationModeList;
            dplCooperationMode.DataTextField = "Name";
            dplCooperationMode.DataValueField = "Name";
            dplCooperationMode.DataBind();
        }
    }

    void LoadSType()
    {
        IList<SType> STypeList = TheToolingMgr.GetQuoteSType();
        if (STypeList.Count > 0)
        {
            dplSType.DataSource = STypeList;
            dplSType.DataTextField = "Name";
            dplSType.DataValueField = "Name";
            dplSType.DataBind();
        }
    }

    void LoadQFor()
    {
        IList<QFor> QForList = TheToolingMgr.GetQFor();
        if (QForList.Count > 0)
        {
            dplQFor.DataSource = QForList;
            dplQFor.DataTextField = "Name";
            dplQFor.DataValueField = "Name";
            dplQFor.DataBind();
        }
    }

    void LoadTSType()
    {
        IList<TSType> TSTypeList = TheToolingMgr.GetTSType();
        if (TSTypeList.Count > 0)
        {
            dplTSType.DataSource = TSTypeList;
            dplTSType.DataTextField = "Name";
            dplTSType.DataValueField = "Name";
            dplTSType.DataBind();
        }
        if (dplTSType.SelectedValue == "分摊结算")
        {
            ltPlanAllocationNum.Text = "1000";
        }
        else
        {
            ltPlanAllocationNum.Text = "0";
        }
    }

    void LoadLogisticsMode()
    {
        IList<LogisticsMode> LogisticsModeList = TheToolingMgr.GetLogisticsMode();
        if (LogisticsModeList.Count > 0)
        {
            dplLogisticsMode.DataSource = LogisticsModeList;
            dplLogisticsMode.DataTextField = "Name";
            dplLogisticsMode.DataValueField = "Name";
            dplLogisticsMode.DataBind();
        }
    }
    #endregion

    public void dplTSType_Changed(object sender, EventArgs e)
    {
        if (dplTSType.SelectedValue == "分摊结算")
        {
            ltPlanAllocationNum.Text = "1000";
        }
        else
        {
            ltPlanAllocationNum.Text = "0";
        }
    }

    public void btnCopy_Click(object sender, EventArgs e)
    {
        try
        {
            TheToolingMgr.QuoteProjectCopy(Id);
            ShowSuccessMessage("Quote.Project.Copy.Success");
        }
        catch
        {
            ShowErrorMessage("Quote.Project.Copy.Fail");
        }
    }

    public void btnExportToPDF_Click(object sender, EventArgs e)
    {
        Project p = new Project();
        IList<Project> projectList = TheToolingMgr.GetProjectById(Id);
        if (projectList.Count > 0)
        {
            p = projectList[0];
        }
        string imgUrlTest = "F:/code/musheng/src/WebApplication/Sconit/Images/musheng.png";
        string imgUrl = "Images/musheng.png";
        TheToolingMgr.ExportToPDF(p, imgUrlTest);
    }
}
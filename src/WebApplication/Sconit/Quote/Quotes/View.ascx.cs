using com.Sconit.Entity.Quote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.MasterData;
using System.Web.Script.Serialization;
using System.Text;

public partial class Quote_Quotes_View : ModuleBase
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

    }

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
            dplCooperationMode.Text = project.CooperationMode;
            dplIsBack.Text = project.IsBack;
            dplLogisticsMode.Text = project.LogisticsMode;
            dplQFor.Text = project.QFor;
            dplSType.Text = project.SType;
            dplTSType.Text = project.TSType;

            txtInputUserName.Text = project.InputUserName;
            if (project.InputDate != null)
            {
                txtInputDate.Text = DateTime.Parse(project.InputDate.ToString()).ToString("yyyy-MM-dd HH:mm");
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
        if (projectList.Count > 0)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<OutJson> jsonList = js.Deserialize<List<OutJson>>(projectList[0].Data);
            foreach (OutJson outjson in jsonList)
            {
                if (outjson.Title == name)
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
                    sb.Append("<td>" + json.Number + "</td>");
                    sb.Append("<td>" + json.Unit + "</td>");
                    sb.Append("<td>" + json.Price + "</td>");
                    sb.Append("<td>" + json.CountPrice + "</td>");
                    sb.Append("</tr>");
                }
                sb.Append("<tr><td colspan='5' style='text-align:center'>小计_" + outjson.Title + "</td><td>" + outjson.amount + "</td></tr>");
                sb.Append("</table>");
                num = 0;
            }
            sb.Append("</fieldset>");
        }
        #endregion
        return sb.ToString();
    }

    public void btnExport_Click(object sender, EventArgs e)
    {
        IList<object> list = new List<object>();
        IList<Project> projectList = TheToolingMgr.GetProjectByProjectId(ProjectId);
        if (projectList.Count > 0)
        {
            list.Add(projectList);
        }
        TheReportMgr.WriteToClient("QuoteProject.xls", list, "QuoteProject.xls");
    }
}
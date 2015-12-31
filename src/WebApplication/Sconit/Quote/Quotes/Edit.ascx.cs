using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.Quote;
using System.Text;
using com.Sconit.Entity.MasterData;
using System.Text.RegularExpressions;
using NCalc;
using com.Sconit.Service.Ext.Hql;
using NHibernate;
using NHibernate.Type;
using System.Web.Script.Serialization;

public partial class Quote_Quotes_Edit : EditModuleBase
{
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
        public string Price;
    }
    public IHqlMgrE hqlMgr { get; set; }
    protected string CustomerCode
    {
        get
        {
            return (string)ViewState["CustomerCode"];
        }
        set
        {
            ViewState["CustomerCode"] = value;
        }
    }

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

    protected string strsJson
    {
        get
        {
            return (string)ViewState["strsJson"];
        }
        set
        {
            ViewState["strsJson"] = value;
        }
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

    public string OutHtml()
    {
        int num = 0;
        decimal amount = 0;
        StringBuilder strJson = new StringBuilder();
        strJson.Append("[");
        IList<CostCategory> ccList = TheToolingMgr.GetCostCategory("");
        IList<CusTemplate> ctList1 = TheToolingMgr.GetCusTemplateByCusCode(CustomerCode);
        #region Sort
        var ctList11 = ctList1.OrderBy(n => n.SortId);
        IList<CusTemplate> ctList = ctList11.ToList<CusTemplate>();
        #endregion
        IList<CostCategory> ccNewList = new List<CostCategory>();

        foreach (CostCategory cc in ccList)
        {
            foreach (CusTemplate ct in ctList)
            {
                if (ct.CostCategory.Id == cc.Id)
                {
                    if (!ccNewList.Contains(ct.CostCategory))
                    {
                        ccNewList.Add(ct.CostCategory);
                    }
                }
            }
        }

        StringBuilder sb = new StringBuilder();

        if (ccNewList != null)
        {
            foreach (CostCategory cc in ccNewList)
            {
                sb.Append("<table class='mtable'>");
                sb.Append("<tr><th style='text-align:left;width:10%'>序号</th><th style='text-align:left;width:20%'>项目构成_" + cc.Name + "</th><th style='text-align:left;width:20%'>数量</th><th style='text-align:left;width:10%'>单位</th><th style='text-align:left;width:20%'>单价</th><th style='text-align:left;width:20%'>金额<th></tr>");
                strJson.Append("{'Title':'" + cc.Name + "','Data':[");
                foreach (CusTemplate ct in ctList)
                {
                    if (ct.CostCategory.Id == cc.Id)
                    {
                        decimal number, price, countPrice;
                        num++;
                        sb.Append("<tr>");
                        sb.Append("<td>" + num + "</td>");
                        sb.Append("<td>" + ct.CostList.Name + "</td>");
                        number = AnalyticalFunction(ct.CostList.Number, null); sb.Append("<td>" + number + "</td>");
                        sb.Append("<td>" + ct.CostList.Unit + "</td>");
                        price = AnalyticalFunction(ct.CostList.Price, number.ToString()); sb.Append("<td>" + price + "</td>");
                        countPrice = number * price; sb.Append("<td>" + countPrice + "</td>");
                        sb.Append("</tr>");
                        amount = amount + countPrice;

                        strJson.Append("{'Name':'" + ct.CostList.Name + "','Number':'" + number + "','Unit':'" + ct.CostList.Unit + "','Price':'" + price + "','CountPrice':'" + countPrice + "'},");
                        strsJson = strJson.ToString();
                    }
                }
                sb.Append("<tr><td colspan='5' style='text-align:center'>小计_" + cc.Name + "</td><td>" + amount + "</td></tr>");
                sb.Append("</table>");
                strJson = strJson.Remove(strJson.ToString().LastIndexOf(','), 1);
                strJson.Append("],'amount':'" + amount + "'},");
                num = 0;
                amount = 0;
            }
        }
        hfdData.Value = strJson.ToString().Substring(0, strJson.Length - 1) + "]";
        return sb.ToString();
    }

    public void InitPageParameter(string id,string status)
    {
        #region show btn
        if(status == "Create")
        {
            this.btnApproval.Visible = false;
        }
        else
        {
            this.btnApproval.Visible = true;
        }
        #endregion
        Id = id;
        IList<ProductInfo> PList = TheToolingMgr.GetProductInfoById(id);
        if (PList != null)
        {
            if (PList.Count > 0)
            {
                ProductInfo p = PList[0];
                ProjectId = p.ProjectId;
                CustomerCode = p.CustomerCode;
                #region Load
                ltCustomerName.Text = p.CustomerName;
                ltProductName.Text = p.ProductName;
                ltProductNo.Text = p.ProductNo;
                ltVersionNo.Text = p.VersionNo;
                IList<QuoteCustomerInfo> cusList = TheToolingMgr.GetQuoteCustomerInfoByCode(CustomerCode, true);
                if (cusList.Count > 0)
                {
                    ltDeliveryAdd.Text = cusList[0].DeliveryAdd;
                    if (cusList[0].BillPeriod != null)
                    {
                        ltBillPeriod.Text = cusList[0].BillPeriod.ToString();
                    }
                }
                ltPT.Text = p.PT;
                ltPCBNum.Text = p.PCBNum;
                if (p.DoubleSideMount)
                {
                    ltDoubleSideMount.Text = "是";
                }
                else
                {
                    ltDoubleSideMount.Text = "否";
                }
                if (p.ChipBurning)
                {
                    ltChipBurning.Text = "是";
                }
                else
                {
                    ltChipBurning.Text = "否";
                }
                ltLightNum.Text = p.LightNum;
                ltBoardMode.Text = p.BoardMode;
                ltConnPoint.Text = p.ConnPoint;
                if (p.DeviceShaping)
                {
                    ltDeviceShaping.Text = "是";
                }
                else
                {
                    ltDeviceShaping.Text = "否";
                }
                if (p.DeviceCoding)
                {
                    ltDeviceCoding.Text = "是";
                }
                else
                {
                    ltDeviceCoding.Text = "否";
                }
                ltCodingType.Text = p.CodingType;
                if (p.SurfaceCoating)
                {
                    ltSurfaceCoating.Text = "是";
                }
                else
                {
                    ltSurfaceCoating.Text = "否";
                }
                ltCoatingAcreage.Text = p.CoatingAcreage;
                ltPackMode.Text = p.PackMode;
                ltEachBox.Text = p.FCLNum;
                #endregion
                #region submit load
                if (status == "Submit")
                {
                    IList<Project> projectList = TheToolingMgr.GetProjectByProjectId(ProjectId);
                    if (projectList.Count > 0)
                    {
                        Project project = projectList[0];
                        if (project.InputDate != null)
                        {
                            this.txtInputDate.Text = project.InputDate.ToString();
                        }
                        this.txtInputUserName.Text = project.InputUserName;
                        this.txtMonthlyDemand.Text = project.MonthlyDemand;
                        this.dplCooperationMode.SelectedValue = project.CooperationMode;
                        this.dplIsBack.SelectedValue = project.IsBack;
                        this.dplLogisticsMode.SelectedValue = project.LogisticsMode;
                        this.dplQFor.SelectedValue = project.QFor;
                        this.dplSType.SelectedValue = project.SType;
                        this.dplTSType.SelectedValue = project.TSType;
                        this.ltPlanAllocationNum.Text = project.PlanAllocationNum;
                    }
                }
                #endregion
            }
        }

        #region 
        this.ltSalesUP.Text = (decimal.Parse(getCategoryAmount("加工费用")) + decimal.Parse(getCategoryAmount("材料费用"))).ToString();
        this.ltSalesUPI.Text = (decimal.Parse(this.ltSalesUP.Text.Trim()) * decimal.Parse("1.17")).ToString();

        this.ltLumpSumFee.Text = (decimal.Parse(getCategoryAmount("专用治具")) + decimal.Parse(getCategoryAmount("一次性定单开机费"))).ToString();
        this.ltLumpSumFeeI.Text = (decimal.Parse(this.ltLumpSumFee.Text.Trim()) * decimal.Parse("1.17")).ToString();

        #endregion
    }

    #region
    void LoadCooperationMode()
    {
        IList<CooperationMode> CooperationModeList = TheToolingMgr.GetCooperationMode();
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
        if(dplTSType.SelectedValue == "分摊结算")
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

    public decimal AnalyticalFunction(string str,string strNum)
    {
        #region
        //if (str != null && str != string.Empty)
        //{
        //    string res = string.Empty;
        //    int Num = str.Split('[').Length - 1;
        //    if (Num == 1)
        //    {
        //        //Regex.Match(str, "(?<=[).*?(?=])").Value;
        //        string result = string.Empty;
        //        int begin = str.IndexOf('[');
        //        int end = str.IndexOf(']');
        //        result = str.Substring(begin + 1, end - begin -1);
        //        if (result[0] == '0')
        //        {
        //            switch (result.Substring(1))
        //            {
        //                case "合作方式":
        //                    res = dplCooperationMode.SelectedValue;
        //                    break;
        //            }
        //            string NewStr = str.Replace(result, res).Replace("[", "").Replace("]", "");
        //            Expression ep = new Expression(NewStr);
        //            return ep.Evaluate().ToString();
        //        }
        //    }
        //    return "";
        //}
        //return "";
        #endregion
        if (str != null && str != string.Empty)
        {
            string[] strs = split(str);
            string result = string.Empty;
            for (int i = 0; i < strs.Length; i++)
            {
                //页面上填写的值
                if (strs[i][0] == '0')
                {
                    #region
                    switch (strs[i].Substring(1))
                    {
                        case "合作方式":
                            result = dplCooperationMode.SelectedValue;
                            break;
                        case "分板方式":
                            result = ltBoardMode.Text.Trim();
                            break;
                        case "连接点":
                            if (ltConnPoint.Text.Trim() != string.Empty)
                            {
                                result = ltConnPoint.Text.Trim();
                            }
                            else
                            {
                                result = "0";
                            }
                            break;
                        case "客户名称":
                            result = ltCustomerName.Text.Trim();
                            break;
                        case "点灯数量":
                            if (ltLightNum.Text.Trim() != string.Empty)
                            {
                                result = ltLightNum.Text.Trim();
                            }
                            else
                            {
                                result = "0";
                            }
                            break;
                        case "双面贴装":
                            result = ltDoubleSideMount.Text.Trim();
                            break;
                        case "拼版数":
                            if (ltPCBNum.Text.Trim() != string.Empty)
                            {
                                result = ltPCBNum.Text.Trim();
                            }
                            else
                            {
                                result = "0";
                            }
                            break;
                        case "计划分摊数量":
                            if (ltPlanAllocationNum.Text.Trim() != string.Empty)
                            {
                                result = ltPlanAllocationNum.Text.Trim();
                            }
                            else
                            {
                                result = "0";
                            }
                            break;
                        case "工装结算方式":
                            result = dplTSType.SelectedValue;
                            break;
                        default:
                            break;
                    }
                    #endregion
                    str = str.Replace('[' + strs[i] + ']', result);
                }
                //数据库中 表 字段名 值
                else if(strs[i][0] == '1')
                {
                    str = str.Replace('[' + strs[i] + ']', getValue(strs[i]));
                }
                //取数量值
                else if(strs[i][0] == '数')
                {
                    str = str.Replace('[' + strs[i] + ']', strNum);
                    //return AnalyticalFunction(str, null);
                }
                //bom 值 汇总
                else if(strs[i][0] == '2')
                {
                    str = str.Replace('[' + strs[i] + ']', getItemCount(strs[i].Substring(1)));
                }
                //获取金额
                else if (strs[i][0] == '3')
                {
                    str = str.Replace('[' + strs[i] + ']', getPrice1(strs[i].Substring(1)));
                }
                //获取类别金额总和
                else if(strs[i][0] == '4')
                {
                    str = str.Replace('[' + strs[i] + ']', getCategoryAmount(strs[i].Substring(1)));
                }
                //获取数量
                else if(strs[i][0] == '5')
                {
                    str = str.Replace('[' + strs[i] + ']', getNumber1(strs[i].Substring(1)));
                }
                //bom 金额 汇总
                else if(strs[i][0] == '6')
                {
                    str = str.Replace('[' + strs[i] + ']', getItemPrice());
                }
            }
            string r = new Expression(str).Evaluate().ToString();
            if (r != null && r != string.Empty)
            {
                return decimal.Parse(r);
            }
            else
            {
                return 0;
            }
        }
        return 0;
    }

    public string[] split(string str)
    {
        List<string> strList = new List<string>();
        string[] strs = str.Split(']');
        for(int i = 0;i<strs.Length-1;i++)
        {
            int begin = strs[i].IndexOf('[');
            int len = strs[i].Length;
            strList.Add(strs[i].Substring(begin + 1, len - begin -1));
        }
        return strList.ToArray();
    }

    public string getValue(string arg)
    {
        string result = string.Empty;
        string NewArg = arg.Substring(1);
        string[] args = NewArg.Split('.');
        if(args[0] == "ProductInfo")
        {
            IList<ProductInfo> PList = TheToolingMgr.GetProductInfoById(Id);
            ProductInfo p = new ProductInfo();
            if(PList!= null)
            {
                if(PList.Count>0)
                {
                    p=PList[0];
                }
            }
            switch(args[1])
            {
                case "ShapingSecCount":
                    if (p.ShapingSecCount != string.Empty && p.ShapingSecCount != null)
                    {
                        result = p.ShapingSecCount;
                    }
                    else
                    {
                        result = "0";
                    }
                    break;
                case "CodingSecCount":
                    if (p.CodingSecCount != string.Empty && p.CodingSecCount != null)
                    {
                        result = p.CodingSecCount;
                    }
                    else
                    {
                        result = "0";
                    }
                    break;
                case "LogisticsCost":
                    if (p.LogisticsCost != null)
                    {
                        result = p.LogisticsCost.ToString();
                    }
                    else
                    {
                        result = "0";
                    }
                    break;
                case "PackCost":
                    if (p.PackCost != null)
                    {
                        result = p.PackCost.ToString();
                    }
                    else
                    {
                        result = "0";
                    }
                    break;
                case "BurningNum":
                    if (p.BurningNum != string.Empty && p.BurningNum != null)
                    {
                        result = p.BurningNum;
                    }
                    else
                    {
                        result = "0";
                    }
                    break;
                default:
                    result = "0";
                    break;
            }
            return result;
        }
        else if (args[0] == "CalculatePara")
        {
            IList<CalculatePara> cpList = TheToolingMgr.GetCalculatePara();
            if(cpList.Count>0)
            {
                foreach(CalculatePara cp in cpList)
                {
                    if(cp.Name == args[1])
                    {
                        return cp.Price.ToString();
                    }
                }
            }
        }
        else if (args[0] == "CustomerInfo")
        {
            IList<QuoteCustomerInfo> qciList = TheToolingMgr.GetQuoteCustomerInfoByCode(CustomerCode);
            if(qciList.Count>0)
            {
                QuoteCustomerInfo qci = qciList[0];
                switch (args[1])
                {
                    case "P_LossRate":
                        result = (decimal.Parse( qci.P_LossRate)/100).ToString();
                        break;
                }
                return result;
            }
        }
        return "0";
    }

    #region 获取bom 加工点数 金额 汇总
    public string getItemCount(string category)
    {
        if (ProjectId != null)
        {
            string sql = @"select sum(convert(float, Point)) as num from Quote_Item where Category = ? and ProjectId = ?";
            IDictionary<String, IType> columns = new Dictionary<String, IType>();
            columns.Add("num", NHibernateUtil.Decimal);
            IList<object> numList = TheHqlMgr.FindAllWithNativeSql<object>(
                    sql, new Object[] { category, ProjectId }, columns);
            if (numList.Count > 0)
            {
                if (numList[0] != null)
                {
                    return numList[0].ToString();
                }
                return "0";
            }
            return "0";
        }
        return "0";
    }

    public string getItemPrice()
    {
        if (ProjectId != null)
        {
            string sql = @"select sum(convert(float, Price)) as num from Quote_Item where ProjectId = ?";
            IDictionary<String, IType> columns = new Dictionary<String, IType>();
            columns.Add("num", NHibernateUtil.Decimal);
            IList<object> numList = TheHqlMgr.FindAllWithNativeSql<object>(
                    sql, new Object[] { ProjectId }, columns);
            if (numList.Count > 0)
            {
                if (numList[0] != null)
                {
                    return numList[0].ToString();
                }
                return "0";
            }
            return "0";
        }
        return "0";
    }
    #endregion
    public string getPrice(string name)
    {
        string str = strsJson.Substring(0, strsJson.Length - 1) + "]}]";
        JavaScriptSerializer js = new JavaScriptSerializer();
        List<OutJson> jsonList = js.Deserialize<List<OutJson>>(str);
        foreach (OutJson outjson in jsonList)
        {
            foreach (Json json in outjson.Data)
            {
                if (json.Name == name)
                {
                    return json.CountPrice;
                }
            }
        }
        return "0";
    }

    public string getPrice1(string name)
    {
        decimal price1 = 0;
        IList<CusTemplate> ctList = TheToolingMgr.GetCusTemplateByCusCode(CustomerCode);
        foreach (CusTemplate ct in ctList)
        {
            if(ct.CostList.Name == name)
            {
                decimal number = AnalyticalFunction(ct.CostList.Number, null);
                decimal price = AnalyticalFunction(ct.CostList.Price, number.ToString());
                price1 = price;
            }
        }
        return price1.ToString();
    }

    public string getNumber(string name)
    {
        string str = strsJson.Substring(0, strsJson.Length - 1) + "]}]";
        JavaScriptSerializer js = new JavaScriptSerializer();
        List<OutJson> jsonList = js.Deserialize<List<OutJson>>(str);
        foreach (OutJson outjson in jsonList)
        {
            foreach (Json json in outjson.Data)
            {
                if (json.Name == name)
                {
                    return json.Number;
                }
            }
        }
        return "0";
    }

    public string getNumber1(string name)
    {
        decimal number1 = 0;
        IList<CusTemplate> ctList = TheToolingMgr.GetCusTemplateByCusCode(CustomerCode);
        foreach (CusTemplate ct in ctList)
        {
            if (ct.CostList.Name == name)
            {
                decimal number = AnalyticalFunction(ct.CostList.Number, null);
                //decimal price = AnalyticalFunction(ct.CostList.Price, number.ToString());
                number1 = number;
            }
        }
        return number1.ToString();
    }

    public string getCategoryAmount(string name)
    {
        decimal amount = 0;
        IList<CostCategory> ccList = TheToolingMgr.GetCostCategory("");
        IList<CusTemplate> ctList = TheToolingMgr.GetCusTemplateByCusCode(CustomerCode);
        foreach(CusTemplate ct in ctList)
        {
            if(ct.CostCategory.Name == name)
            {
                decimal number = AnalyticalFunction(ct.CostList.Number, null);
                decimal price = AnalyticalFunction(ct.CostList.Price, number.ToString());
                amount += number * price;
            }
        }
        return amount.ToString();
    }

    public void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //IList<Project> projectList = TheToolingMgr.GetProjectByProjectId(ProjectId);
            #region project
            Project project = new Project();
            project.BillPeriod = ltBillPeriod.Text.Trim();
            project.BoardMode = ltBoardMode.Text.Trim();
            project.ChipBurning = ltChipBurning.Text.Trim();
            project.CoatingAcreage = ltCoatingAcreage.Text.Trim();
            project.CodingType = ltCodingType.Text.Trim();
            project.ConnPoint = ltConnPoint.Text.Trim();
            project.CooperationMode = dplCooperationMode.SelectedValue;
            project.CustomerCode = CustomerCode;
            project.CustomerName = ltCustomerName.Text.Trim();
            project.Data = hfdData.Value;
            project.DeliveryAdd = ltDeliveryAdd.Text.Trim();
            project.DeviceCoding = ltDeviceCoding.Text.Trim();
            project.DeviceShaping = ltDeviceShaping.Text.Trim();
            project.DoubleSideMount = ltDoubleSideMount.Text.Trim();
            project.EachBox = ltEachBox.Text.Trim();
            if (txtInputDate.Text.Trim() != string.Empty)
            {
                project.InputDate = DateTime.Parse(txtInputDate.Text.Trim());
            }
            project.InputUserName = txtInputUserName.Text.Trim();
            project.IsBack = dplIsBack.SelectedValue;
            project.LightNum = ltLightNum.Text.Trim();
            project.LogisticsMode = dplLogisticsMode.SelectedValue;
            project.LumpSumFee = ltLumpSumFee.Text.Trim();
            project.LumpSumFeeI = ltLumpSumFeeI.Text.Trim();
            project.MonthlyDemand = txtMonthlyDemand.Text.Trim();
            project.PackMode = ltPackMode.Text.Trim();
            project.PCBNum = ltPCBNum.Text.Trim();
            project.PlanAllocationNum = ltPlanAllocationNum.Text.Trim();
            project.ProductName = ltProductName.Text.Trim();
            project.ProductNo = ltProductNo.Text.Trim();
            project.ProjectId = ProjectId;
            project.PT = ltPT.Text.Trim();
            project.QFor = dplQFor.SelectedValue;
            project.SalesUP = ltSalesUP.Text.Trim();
            project.SalesUPI = ltSalesUPI.Text.Trim();
            project.Status = "Submit";
            project.SType = dplSType.Text.Trim();
            project.SurfaceCoating = ltSurfaceCoating.Text;
            project.ToCustomerDate = DateTime.Now;
            project.ToCustomerName = this.CurrentUser.CodeName;
            project.TSType = dplTSType.SelectedValue;
            project.VersionNo = ltVersionNo.Text.Trim();
            #endregion
            //if (projectList.Count > 0)
            //{
            //    project.Id = projectList[0].Id;
            //    TheToolingMgr.UpdateProject(project);
            //}
            //else
            //{
                TheToolingMgr.CreateProject(project);
                #region update status
                IList<ProductInfo> pList = TheToolingMgr.GetProductInfoById(Id);
                if (pList.Count > 0)
                {
                    ProductInfo p = pList[0];
                    p.Status = "Submit";
                    TheToolingMgr.UpdateProductInfoStatus(p);
                }
                #endregion
            //}
            ShowSuccessMessage("Common.Business.Result.Save.Successfully");
        }
        catch
        {
            ShowErrorMessage("Common.Business.Result.Save.Failed");
        }

    }

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

    public void btnApproval_Click(object sender, EventArgs e)
    {
        try
        {
            #region update status Complete
            IList<ProductInfo> pList = TheToolingMgr.GetProductInfoById(Id);
            if (pList.Count > 0)
            {
                ProductInfo p = pList[0];
                p.Status = "Complete";
                TheToolingMgr.UpdateProductInfoStatus(p);
            }

            IList<Project> projectList = TheToolingMgr.GetProjectByProjectId(ProjectId);
            if (projectList.Count > 0)
            {
                Project project = projectList[0];
                project.Status = "Complete";
                TheToolingMgr.UpdateProject(project);
            }
            this.btnSubmit.Visible = false;
            ShowSuccessMessage("");
        }
        catch
        {
            ShowErrorMessage("");
        }
        #endregion
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
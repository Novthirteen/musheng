using Castle.Services.Transaction;
using com.Sconit.Entity.Quote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace com.Sconit.Service.Report.Impl
{
    [Transactional]
    public class RepQuoteProjectMgr : RepTemplate1
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
            public string amount;
        }

        protected override bool FillValuesImpl(String templateFileName, IList<object> list)
        {
            if (list != null && list.Count > 0)
            {
                IList<Project> projectList = (IList<Project>)list[0];
                Project project = projectList[0];
                #region 表头
                //项目ID
                this.SetRowCell(2, 4, project.ProjectId);
                //客户名称
                this.SetRowCell(3, 4, project.CustomerName);
                //填表人
                this.SetRowCell(3, 25, project.InputUserName);
                //填表日期
                if (project.InputDate != null)
                {
                    this.SetRowCell(3, 36, project.InputDate.ToString());
                }
                //产品名称
                this.SetRowCell(4, 4, project.ProductName);
                //产品图号
                this.SetRowCell(4, 25, project.ProductNo);
                //版本号
                this.SetRowCell(4, 39, project.VersionNo);
                //交付地点
                this.SetRowCell(5, 4, project.DeliveryAdd);
                //合作方式
                this.SetRowCell(5, 15, project.CooperationMode);
                //结算方式
                this.SetRowCell(5, 23, project.SType);
                //结算账期
                this.SetRowCell(5, 31, project.BillPeriod);
                //月度需求
                this.SetRowCell(5, 38, project.MonthlyDemand);
                //报价针对
                this.SetRowCell(6, 4, project.QFor);
                //工装结算方式
                this.SetRowCell(6, 13, project.TSType);
                //计算分摊数量
                this.SetRowCell(6, 23, project.PlanAllocationNum);
                //加工工艺
                this.SetRowCell(6, 33, project.PT);
                //拼版数
                this.SetRowCell(6, 40, project.PCBNum);
                //双面贴装
                this.SetRowCell(7, 4, project.DoubleSideMount);
                //芯片贴装前烧写
                this.SetRowCell(7, 12, project.ChipBurning);
                //点灯数量
                this.SetRowCell(7, 18, project.LightNum);
                //分板方式
                this.SetRowCell(7, 26, project.BoardMode);
                //连接点
                this.SetRowCell(7, 33, project.ConnPoint);
                //器件特殊整形
                this.SetRowCell(7, 41, project.DeviceShaping);
                //器件编带
                this.SetRowCell(8, 4, project.DeviceCoding);
                //器件编带种类
                this.SetRowCell(8, 12, project.CodingType);
                //销售单价(未税)
                this.SetRowCell(8, 20, project.SalesUP);
                //销售单价(含17%增值税)
                this.SetRowCell(8, 34, project.SalesUPI);
                //表面涂覆
                this.SetRowCell(9, 4, project.SurfaceCoating);
                //表面涂覆面积
                this.SetRowCell(9, 12, project.CoatingAcreage);
                //包装方式
                this.SetRowCell(10, 4, project.PackMode);
                //每箱
                this.SetRowCell(10, 12, project.EachBox);
                //一次性结算费用(未税)
                this.SetRowCell(10, 20, project.LumpSumFee);
                //一次性结算费用(含17%增值税)
                this.SetRowCell(10, 34, project.LumpSumFeeI);
                //物流方式
                this.SetRowCell(11, 4, project.LogisticsMode);
                //包装物需回收
                this.SetRowCell(11, 14, project.IsBack);
                #endregion
                #region
                JavaScriptSerializer js = new JavaScriptSerializer();
                List<OutJson> jsonList = js.Deserialize<List<OutJson>>(projectList[0].Data);

                int row = 12;
                int num = 0;
                foreach (OutJson outjson in jsonList)
                {
                    this.SetRowCell(row, 0, "序号");
                    this.SetRowCell(row, 4, "项目构成_" + outjson.Title);
                    this.SetRowCell(row, 15, "数量");
                    this.SetRowCell(row, 20, "单位");
                    this.SetRowCell(row, 25, "单价");
                    this.SetRowCell(row, 30, "金额");
                    foreach(Json json in outjson.Data)
                    {
                        num++;
                        row ++;
                        this.SetRowCell(row, 0, num);
                        this.SetRowCell(row, 4, json.Name);
                        this.SetRowCell(row, 15, json.Number);
                        this.SetRowCell(row, 20, json.Unit);
                        this.SetRowCell(row, 25, json.Price);
                        this.SetRowCell(row, 30, (decimal.Parse(json.Number) * decimal.Parse(json.Price)).ToString());
                    }
                    row++;
                    this.SetRowCell(row, 20, "小计_" + outjson.Title);
                    this.SetRowCell(row, 30, outjson.amount);
                    row++;
                    num = 0;
                }
                #endregion
            }
            return true;
        }

        public override void CopyPageValues(int pageIndex)
        { }
    }
}

#region Extend Class

namespace com.Sconit.Service.Ext.Report.Impl
{
    [Transactional]
    public partial class RepQuoteProjectMgrE : com.Sconit.Service.Report.Impl.RepQuoteProjectMgr, IReportBaseMgrE
    {

    }
}

#endregion

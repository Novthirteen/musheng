using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Quote;
using com.Sconit.Service.Ext.Criteria;
using NHibernate;
using NHibernate.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Web.Script.Serialization;

namespace com.Sconit.Service.Quote.Impl
{
    [Transactional]
    public class ToolingMgr : IToolingMgr
    {
        public ICriteriaMgrE criteriaMgr { get; set; }

        [Transaction(TransactionMode.Requires)]
        public Tooling CreateTooling(Tooling tl)
        {
            criteriaMgr.Create(tl);
            return tl;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Tooling> GetToolingByTLNo(string tlNo)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Tooling));
            if(tlNo.Trim()!=string.Empty)
            {
                criteria.Add(Expression.Eq("TL_No", tlNo));
            }
            return criteriaMgr.FindAll<Tooling>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public Tooling SaveTooling(Tooling tl)
        {
            criteriaMgr.Save(tl);
            return tl;
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteTooling(Tooling tl)
        {
            criteriaMgr.Delete(tl);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<QuoteCustomerInfo> GetQuoteCustomerInfoByCode(string code)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(QuoteCustomerInfo));
            if (code.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Code", code));
            }
            criteria.Add(Expression.Eq("Status", true));
            criteria.Add(Expression.Lt("StartDate", DateTime.Now));
            criteria.Add(Expression.Gt("EndDate", DateTime.Now));
            return criteriaMgr.FindAll<QuoteCustomerInfo>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<QuoteCustomerInfo> GetQuoteCustomerInfoByCode1(string code)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(QuoteCustomerInfo));
            if (code.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Code", code));
            }
            //criteria.Add(Expression.Eq("Status", true));
            //criteria.Add(Expression.Lt("StartDate", DateTime.Now));
            //criteria.Add(Expression.Gt("EndDate", DateTime.Now));
            return criteriaMgr.FindAll<QuoteCustomerInfo>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<SType> GetQuoteSType()
        {
            return criteriaMgr.FindAll<SType>();
        }

        [Transaction(TransactionMode.Requires)]
        public IList<QuoteCustomerInfo> GetQuoteCustomerInfoById(int id)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(QuoteCustomerInfo));
            criteria.Add(Expression.Eq("Id", id));
            return criteriaMgr.FindAll<QuoteCustomerInfo>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public void SaveQuoteCustomerInfo(QuoteCustomerInfo qc)
        {
            criteriaMgr.Save(qc);
        }

        [Transaction(TransactionMode.Requires)]
        public void CreateQuoteCustomerInfo(QuoteCustomerInfo qc)
        {
            criteriaMgr.Create(qc);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<QuoteItem> GetQuoteItemByCode(string code)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(QuoteItem));
            criteria.Add(Expression.Eq("ItemCode", code));
            return criteriaMgr.FindAll<QuoteItem>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<QuoteItem> GetQuoteItemById(string Id)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(QuoteItem));
            criteria.Add(Expression.Eq("Id", Int32.Parse(Id)));
            return criteriaMgr.FindAll<QuoteItem>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteQuoteItemById(string id)
        {
            criteriaMgr.DeleteWithHql("from QuoteItem where Id = " + id);
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateQuoteItem(QuoteItem qt)
        {
            criteriaMgr.Update(qt);
        }

        [Transaction(TransactionMode.Requires)]
        public void CreateQuoteItem(QuoteItem qt)
        {
            criteriaMgr.Create(qt);
        }

        #region Load
        [Transaction(TransactionMode.Requires)]
        public IList<PT> GetPT()
        {
            return criteriaMgr.FindAll<PT>();
        }

        [Transaction(TransactionMode.Requires)]
        public IList<BoardMode> GetBoardMode()
        {
            return criteriaMgr.FindAll<BoardMode>();
        }

        [Transaction(TransactionMode.Requires)]
        public IList<PackMode> GetPackMode()
        {
            return criteriaMgr.FindAll<PackMode>();
        }

        [Transaction(TransactionMode.Requires)]
        public IList<OutBox> GetOutBox()
        {
            return criteriaMgr.FindAll<OutBox>();
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Plate> GetPlate()
        {
            return criteriaMgr.FindAll<Plate>();
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Partition> GetPartition()
        {
            return criteriaMgr.FindAll<Partition>();
        }

        [Transaction(TransactionMode.Requires)]
        public IList<BubbleBag> GetBubbleBag()
        {
            return criteriaMgr.FindAll<BubbleBag>();
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Blister> GetBlister()
        {
            return criteriaMgr.FindAll<Blister>();
        }

        [Transaction(TransactionMode.Requires)]
        public IList<LogisticsFee> GetLogisticsFeeByCityName(string cityName)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(LogisticsFee));
            criteria.Add(Expression.Eq("CityName", cityName));
            return criteriaMgr.FindAll<LogisticsFee>(criteria);
        }
        #endregion

        [Transaction(TransactionMode.Requires)]
        public void CreatProductInfo(ProductInfo productInfo)
        {
            criteriaMgr.Create(productInfo);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<ProductInfo> GetProductInfoById(string id)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(ProductInfo));
            criteria.Add(Expression.Eq("Id", Int32.Parse(id)));
            return criteriaMgr.FindAll<ProductInfo>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateProductInfo(ProductInfo productInfo)
        {
            criteriaMgr.Update(productInfo);
        }

        [Transaction(TransactionMode.Requires)]
        public void CreateCostCategory(CostCategory costCategory)
        {
            criteriaMgr.Create(costCategory);
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateCostCategory(CostCategory costCategory)
        {
            criteriaMgr.Update(costCategory);
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteCostCategory(CostCategory costCategory)
        {
            criteriaMgr.Delete(costCategory);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<CostCategory> GetCostCategoryById(string id)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(CostCategory));
            criteria.Add(Expression.Eq("Id", Int32.Parse(id)));
            return criteriaMgr.FindAll<CostCategory>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<CostList> GetCostListById(string id)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(CostList));
            criteria.Add(Expression.Eq("Id", Int32.Parse(id)));
            return criteriaMgr.FindAll<CostList>(criteria);
        }

        public IList<CostCategory> GetCostCategory(string userCode)
        {
            return criteriaMgr.FindAll<CostCategory>();
        }

        [Transaction(TransactionMode.Requires)]
        public IList<CostList> GetCostListByCCId(string id)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(CostList));
            criteria.Add(Expression.Eq("CCId.Id", Int32.Parse(id)));
            return criteriaMgr.FindAll<CostList>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<CusTemplate> GetCostListByCusCodeAndCCId(string cusCode, string ccId)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(CusTemplate));
            criteria.Add(Expression.Eq("CustomerCode", cusCode));
            criteria.Add(Expression.Eq("CostCategory.Id", Int32.Parse(ccId)));
            return criteriaMgr.FindAll<CusTemplate>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<CusTemplate> GetCostListByIdAndCusCodeAndCCId(string id,string cusCode, string ccId)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(CusTemplate));
            criteria.Add(Expression.Eq("CostList.Id", Int32.Parse(id)));
            criteria.Add(Expression.Eq("CustomerCode", cusCode));
            criteria.Add(Expression.Eq("CostCategory.Id", Int32.Parse(ccId)));
            return criteriaMgr.FindAll<CusTemplate>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<CusTemplate> GetCusTemplateByCusCode(string code)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(CusTemplate));
            criteria.Add(Expression.Eq("CustomerCode", code));
            return criteriaMgr.FindAll<CusTemplate>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteCusTemplate(CusTemplate ct)
        {
            criteriaMgr.Delete(ct);
        }

        [Transaction(TransactionMode.Requires)]
        public void CreateCusTemplate(CusTemplate ct)
        {
            criteriaMgr.Create(ct);
        }

        [Transaction(TransactionMode.Requires)]
        public void CreateCostList(CostList cl)
        {
            criteriaMgr.Create(cl);
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateCostList(CostList cl)
        {
            criteriaMgr.Update(cl);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<CooperationMode> GetCooperationMode()
        {
            return criteriaMgr.FindAll<CooperationMode>();
        }

        [Transaction(TransactionMode.Requires)]
        public IList<QFor> GetQFor()
        {
            return criteriaMgr.FindAll<QFor>();
        }

        [Transaction(TransactionMode.Requires)]
        public IList<TSType> GetTSType()
        {
            return criteriaMgr.FindAll<TSType>();
        }

        [Transaction(TransactionMode.Requires)]
        public IList<LogisticsMode> GetLogisticsMode()
        {
            return criteriaMgr.FindAll<LogisticsMode>();
        }

        [Transaction(TransactionMode.Requires)]
        public IList<QuoteCustomerInfo> GetQuoteCustomerInfoByCode(string code,bool status)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(QuoteCustomerInfo));
            criteria.Add(Expression.Eq("Code", code));
            criteria.Add(Expression.Eq("Status",status));
            criteria.Add(Expression.Gt("EndDate", DateTime.Now));
            return criteriaMgr.FindAll<QuoteCustomerInfo>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<CalculatePara> GetCalculatePara()
        {
            return criteriaMgr.FindAll<CalculatePara>();
        }

        [Transaction(TransactionMode.Requires)]
        public void CreateProject(Project project)
        {
            criteriaMgr.Create(project);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Project> GetProjectByProjectId(string projectId)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Project));
            criteria.Add(Expression.Eq("ProjectId", projectId));
            return criteriaMgr.FindAll<Project>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<Project> GetProjectById(string Id)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Project));
            criteria.Add(Expression.Eq("Id",Int32.Parse(Id)));
            return criteriaMgr.FindAll<Project>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateProductInfoStatus(ProductInfo p)
        {
            criteriaMgr.Update(p);
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateCusTemplate(CusTemplate cusTemplate)
        {
            criteriaMgr.Update(cusTemplate);
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateProject(Project project)
        {
            criteriaMgr.Update(project);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<QuoteCustomerInfo> GetQuoteCustomer(object para)
        {
            string code = (string)((object[])para)[0];
            string name = (string)((object[])para)[1];
            bool isTrue = (bool)((object[])para)[2];
            bool isFalse = (bool)((object[])para)[3];
            DetachedCriteria criteria = DetachedCriteria.For(typeof(QuoteCustomerInfo));
            if(code!=string.Empty)
            {
                criteria.Add(Expression.Like("Code", code, MatchMode.Anywhere));
            }
            if(name!=string.Empty)
            {
                criteria.Add(Expression.Like("Name", name, MatchMode.Anywhere));
            }
            if(isTrue!=isFalse)
            {
                if(isTrue)
                {
                    criteria.Add(Expression.Eq("Status", true));
                }
                else
                {
                    criteria.Add(Expression.Eq("Status", false));
                }
            }
            return criteriaMgr.FindAll<QuoteCustomerInfo>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteQuoteCustomerById(string id)
        {
            criteriaMgr.DeleteWithHql("from QuoteCustomerInfo where Id = " + id);
        }

        public IList<QuoteCustomerInfo> GetCustomer()
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(QuoteCustomerInfo));
            criteria.Add(Expression.Eq("Status", true));
            return criteriaMgr.FindAll<QuoteCustomerInfo>(criteria);
        }

        #region 生成项目ID
        [Transaction(TransactionMode.Requires)]
        public void CreateGPID(GPID gpid)
        {
            criteriaMgr.Create(gpid);
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteGPIDById(string id)
        {
            criteriaMgr.DeleteWithHql("from GPID where ID = " + id);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<GPID> GetGPIDById(string id)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(GPID));
            criteria.Add(Expression.Eq("ID", id));
            return criteriaMgr.FindAll<GPID>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateGPID(GPID gpid)
        {
            criteriaMgr.Update(gpid);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<GPID> GetGPID(bool Status)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(GPID));
            if (Status)
            {
                criteria.Add(Expression.Eq("Status", Status));
            }
            return criteriaMgr.FindAll<GPID>(criteria);
        }
        #endregion

        [Transaction(TransactionMode.Requires)]
        public IList<QuoteItem> GetQuoteItemByPID(string pid)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(QuoteItem));
            criteria.Add(Expression.Eq("ProjectId", pid));
            return criteriaMgr.FindAll<QuoteItem>(criteria);
        }

        #region ItemPack
        [Transaction(TransactionMode.Requires)]
        public void CreateItemPack(ItemPack itemPack)
        {
            criteriaMgr.Create(itemPack);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<ItemPack> GetItemPack(int id)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(ItemPack));
            if (id != null)
            {
                criteria.Add(Expression.Eq("Id", id));
            }
            return criteriaMgr.FindAll<ItemPack>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteItemPackById(int id)
        {
            criteriaMgr.DeleteWithHql("from ItemPack where ID = " + id);
        }

        [Transaction(TransactionMode.Requires)]
        public void UpdateItemPack(ItemPack itemPack)
        {
            criteriaMgr.Update(itemPack);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<ItemPack> GetItemPack(string spec)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(ItemPack));
            if (spec != string.Empty)
            {
                criteria.Add(Expression.Eq("Spec", spec));
            }
            return criteriaMgr.FindAll<ItemPack>(criteria);
        }
        #endregion

        [Transaction(TransactionMode.Requires)]
        public void QuoteProjectCopy(string id)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Project));
            criteria.Add(Expression.Eq("Id", Int32.Parse(id)));
            IList<Project> pList = criteriaMgr.FindAll<Project>(criteria);
            if(pList.Count > 0)
            {
                Project p = new Project();
                Project pp = pList[0];
                #region
                p.BillPeriod = pp.BillPeriod;
                p.BoardMode = pp.BoardMode;
                p.ChipBurning = pp.ChipBurning;
                p.CoatingAcreage = pp.CoatingAcreage;
                p.CodingType = pp.CodingType;
                p.ConnPoint = pp.CooperationMode;
                p.CustomerCode = pp.CustomerCode;
                p.CustomerName = pp.CustomerName;
                p.Data = pp.Data;
                p.DeliveryAdd = pp.DeliveryAdd;
                p.DeviceCoding = pp.DeviceCoding;
                p.DeviceShaping = pp.DeviceShaping;
                p.DoubleSideMount = pp.DoubleSideMount;
                p.EachBox = pp.EachBox;
                p.InputDate = pp.InputDate;
                p.InputUserName = pp.InputUserName;
                p.IsBack = pp.IsBack;
                p.LightNum = pp.LightNum;
                p.LogisticsMode = pp.LogisticsMode;
                p.LumpSumFee = pp.LumpSumFee;
                p.LumpSumFeeI = pp.LumpSumFeeI;
                p.MonthlyDemand = pp.MonthlyDemand;
                p.PackMode = pp.PackMode;
                p.PCBNum = pp.PCBNum;
                p.PlanAllocationNum = pp.PlanAllocationNum;
                p.ProductName = pp.ProductName;
                p.ProductNo = pp.ProductNo;
                p.ProjectId = pp.ProjectId;
                p.PT = pp.PT;
                p.PVision = pp.PVision + ".1";
                p.QFor = pp.QFor;
                p.SalesUP = pp.SalesUP;
                p.SalesUPI = pp.SalesUPI;
                p.Status = pp.Status;
                p.SType = pp.SType;
                p.SurfaceCoating = pp.SurfaceCoating;
                p.ToCustomerDate = pp.ToCustomerDate;
                p.ToCustomerName = pp.ToCustomerName;
                p.TSType = pp.TSType;
                p.VersionNo = pp.VersionNo;
                #endregion
                criteriaMgr.Create(p);
            }
        }

        #region 导出成PDF
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
        public void ExportToPDF(Project project,string imgUrl)
        {
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\Fonts\SIMKAI.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            PdfPTable table = new PdfPTable(43);
            Font ft = new Font(bfChinese, 8);
            #region title
            PdfPCell cell1 = CreateCell("", ft, 10);
            cell1.Border = 0;
            table.AddCell(cell1);
            Image img = Image.GetInstance(imgUrl);
            img.ScalePercent(24f);
            PdfPCell cellTile = new PdfPCell(img);
            cellTile.Colspan = 4;
            cellTile.Border = 0;
            table.AddCell(cellTile);
            cellTile = CreateCell("上海慕盛实业有限公司报价单", new Font(bfChinese, 16), 29);
            cellTile.Border = 0;
            table.AddCell(cellTile);
            #endregion
            table.AddCell(CreateCell("客户名称", ft, 4));
            table.AddCell(CreateCell(project.CustomerName, ft, 17));
            table.AddCell(CreateCell("填表人", ft, 4));
            table.AddCell(CreateCell(project.InputUserName, ft, 7));
            table.AddCell(CreateCell("填表日期", ft, 4));
            if (project.InputDate != null)
            {
                table.AddCell(CreateCell(project.InputDate.ToString(), ft, 7));
            }
            else
            {
                table.AddCell(CreateCell("", ft, 7));
            }
            table.AddCell(CreateCell("产品名称", ft, 4));
            table.AddCell(CreateCell(project.ProductName, ft, 17));
            table.AddCell(CreateCell("产品图号", ft, 4));
            table.AddCell(CreateCell(project.ProductNo, ft, 11));
            table.AddCell(CreateCell("版本号", ft, 3));
            table.AddCell(CreateCell(project.PVision, ft, 4));
            table.AddCell(CreateCell("交付地点", ft, 4));
            table.AddCell(CreateCell(project.DeliveryAdd, ft, 7));
            table.AddCell(CreateCell("合作方式", ft, 4));
            table.AddCell(CreateCell(project.CooperationMode, ft, 4));
            table.AddCell(CreateCell("结算方式", ft, 4));
            table.AddCell(CreateCell(project.SType, ft, 4));
            table.AddCell(CreateCell("结算账期", ft, 4));
            table.AddCell(CreateCell(project.BillPeriod + "天", ft, 3));
            table.AddCell(CreateCell("月度需求", ft, 4));
            table.AddCell(CreateCell(project.MonthlyDemand + "套", ft, 5));
            table.AddCell(CreateCell("报价针对", ft, 4));
            table.AddCell(CreateCell(project.QFor, ft, 4));
            table.AddCell(CreateCell("工装结算方式", ft, 5));
            table.AddCell(CreateCell(project.TSType, ft, 5));
            table.AddCell(CreateCell("计算分摊数量", ft, 5));
            table.AddCell(CreateCell(project.PlanAllocationNum + "套", ft, 6));
            table.AddCell(CreateCell("加工工艺", ft, 4));
            table.AddCell(CreateCell(project.PT, ft, 3));
            table.AddCell(CreateCell("拼版数", ft, 4));
            table.AddCell(CreateCell(project.PCBNum, ft, 3));
            table.AddCell(CreateCell("双面贴装", ft, 4));
            table.AddCell(CreateCell(project.DoubleSideMount, ft, 2));
            table.AddCell(CreateCell("芯片贴装前烧写", ft, 6));
            table.AddCell(CreateCell(project.ChipBurning, ft, 2));
            table.AddCell(CreateCell("点灯数量", ft, 4));
            table.AddCell(CreateCell(project.LightNum + "颗", ft, 4));
            table.AddCell(CreateCell("分板方式", ft, 4));
            table.AddCell(CreateCell(project.BoardMode, ft, 4));
            table.AddCell(CreateCell("连接点", ft, 3));
            table.AddCell(CreateCell(project.ConnPoint + "点", ft, 3));
            table.AddCell(CreateCell("器件特殊整形", ft, 5));
            table.AddCell(CreateCell(project.DeviceShaping, ft, 2));
            table.AddCell(CreateCell("器件编带", ft, 4));
            table.AddCell(CreateCell(project.DeviceCoding, ft, 2));
            table.AddCell(CreateCell("器件编带种类", ft, 6));
            table.AddCell(CreateCell(project.CodingType, ft, 4));
            PdfPCell cell = CreateCell("销售单价(未税)", ft, 4);
            cell.Rowspan = 2;
            table.AddCell(cell);
            cell = CreateCell(project.SalesUP, ft, 6);
            cell.Rowspan = 2;
            table.AddCell(cell);
            cell = CreateCell("元(RMB)", ft, 3);
            cell.Rowspan = 2;
            table.AddCell(cell);
            cell = CreateCell("销售单价(含17%增值税)", ft, 5);
            cell.Rowspan = 2;
            table.AddCell(cell);
            cell = CreateCell(project.SalesUPI, ft, 6);
            cell.Rowspan = 2;
            table.AddCell(cell);
            cell = CreateCell("元(RMB)", ft, 3);
            cell.Rowspan = 2;
            table.AddCell(cell);
            table.AddCell(CreateCell("表面涂覆", ft, 4));
            table.AddCell(CreateCell(project.SurfaceCoating, ft, 2));
            table.AddCell(CreateCell("表面涂覆面积", ft, 6));
            table.AddCell(CreateCell(project.CoatingAcreage+ "cm²", ft, 4));
            table.AddCell(CreateCell("包装方式", ft, 4));
            table.AddCell(CreateCell(project.PackMode, ft, 6));
            table.AddCell(CreateCell("每箱", ft, 2));
            table.AddCell(CreateCell(project.PackMode+"块", ft, 4));
            cell = CreateCell("一次性结算费用(未税)", ft, 4);
            cell.Rowspan = 2;
            table.AddCell(cell);
            cell = CreateCell(project.LumpSumFee, ft, 6);
            cell.Rowspan = 2;
            table.AddCell(cell);
            cell = CreateCell("元(RMB)", ft, 3);
            cell.Rowspan = 2;
            table.AddCell(cell);
            cell = CreateCell("一次性结算费用(含17%增值税)", ft, 5);
            cell.Rowspan = 2;
            table.AddCell(cell);
            cell = CreateCell(project.LumpSumFeeI, ft, 6);
            cell.Rowspan = 2;
            table.AddCell(cell);
            cell = CreateCell("元(RMB)", ft, 3);
            cell.Rowspan = 2;
            table.AddCell(cell);
            table.AddCell(CreateCell("物流方式", ft, 4));
            table.AddCell(CreateCell(project.LogisticsMode, ft, 5));
            table.AddCell(CreateCell("包装物需回收", ft, 5));
            table.AddCell(CreateCell(project.IsBack, ft, 2));

            JavaScriptSerializer js = new JavaScriptSerializer();
            List<OutJson> jsonList = js.Deserialize<List<OutJson>>(project.Data);
            int num = 0;
            foreach (OutJson outjson in jsonList)
            {
                table.AddCell(CreateCell("序号", ft, 3));
                table.AddCell(CreateCell("项目构成_"+outjson.Title, ft, 11));
                table.AddCell(CreateCell("数量", ft, 3));
                table.AddCell(CreateCell("单位", ft, 3));
                table.AddCell(CreateCell("单价", ft, 5));
                table.AddCell(CreateCell("金额", ft, 19));
                foreach (Json json in outjson.Data)
                {
                    num ++;
                    table.AddCell(CreateCell(num.ToString(), ft, 3));
                    table.AddCell(CreateCell(json.Name, ft, 11));
                    table.AddCell(CreateCell(json.Number, ft, 3));
                    table.AddCell(CreateCell(json.Unit, ft, 3));
                    table.AddCell(CreateCell(json.Price, ft, 5));
                    table.AddCell(CreateCell(json.CountPrice, ft, 18));
                }
                cell = CreateCell("小计_"+outjson.Title,ft,25);
                cell.Colspan= 25;
                table.AddCell(cell);
                table.AddCell(CreateCell(outjson.amount, ft, 18));
                num = 0;
            }

            Document document = new Document();
            string fileNamePDF = @"D:\Test\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "报价单.pdf";
            PdfWriter.GetInstance(document, new FileStream(fileNamePDF, FileMode.Create));
            document.Open();
            document.Add(table);
            document.Close();
        }

        public PdfPCell CreateCell(string text, Font textFont, int colspan)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, textFont));
            cell.Colspan = colspan;
            return cell;
        }
        #endregion

        [Transaction(TransactionMode.Requires)]
        public IList<QuoteItem> GetItemByProductId(int id)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(QuoteItem));
            criteria.Add(Expression.Eq("ProductId", id));
            return criteriaMgr.FindAll<QuoteItem>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteQuoteItemByProductId(string id)
        {
            criteriaMgr.DeleteWithHql("from QuoteItem where ProductId = " + id); ;
        }

        [Transaction(TransactionMode.Requires)]
        public void SaveQuoteItem(QuoteItem qi)
        {
            criteriaMgr.Create(qi);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<QuoteItem> GetXLSQuoteItem(Stream stream)
        {
            IList<QuoteItem> qiList = new List<QuoteItem>();
            NPOI.SS.UserModel.Workbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook(stream);
            NPOI.SS.UserModel.Sheet sheet = workbook.GetSheetAt(0);
            //Execel第一行是标题，不是要导入数据库的数据
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {

                QuoteItem qi = new QuoteItem();
                NPOI.SS.UserModel.Row row = sheet.GetRow(i);
                string ItemCode = row.GetCell(0).StringCellValue;
                if(ItemCode != "")
                {
                    qi.ItemCode = ItemCode;
                    qi.Supplier = row.GetCell(1).StringCellValue;
                    qi.Category = row.GetCell(2).StringCellValue;
                    qi.Brand = row.GetCell(3).StringCellValue;
                    qi.Model = row.GetCell(4).StringCellValue;
                    qi.SingleNum = row.GetCell(5).StringCellValue;
                    qi.PurchasePrice = decimal.Parse( row.GetCell(6).StringCellValue);
                    qi.Price = decimal.Parse(row.GetCell(7).StringCellValue);
                    qi.PinNum = row.GetCell(8).StringCellValue;
                    qi.PinConversion = row.GetCell(9).StringCellValue;
                    qi.Point = row.GetCell(10).StringCellValue;
                    qi.ProductId = Int32.Parse(row.GetCell(11).StringCellValue);
                    qi.BitNum = row.GetCell(12).StringCellValue;
                    qi.Side = row.GetCell(13).StringCellValue;
                    qiList.Add(qi);
                }

            }
            return qiList;
        }
    }
}

#region Extend Class

namespace com.Sconit.Service.Ext.Quote.Impl
{
    [Transactional]
    public partial class ToolingMgrE : com.Sconit.Service.Quote.Impl.ToolingMgr, IToolingMgrE
    {
    }
}

#endregion Extend Class

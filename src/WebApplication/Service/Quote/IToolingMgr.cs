using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.Quote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace com.Sconit.Service.Quote
{
    public interface IToolingMgr
    {
        Tooling CreateTooling(Tooling tl);

        IList<Tooling> GetToolingByTLNo(string tlNo);

        Tooling SaveTooling(Tooling tl);

        void DeleteTooling(Tooling tl);

        IList<QuoteCustomerInfo> GetQuoteCustomerInfoByCode(string code);

        IList<QuoteCustomerInfo> GetQuoteCustomerInfoByCode1(string code);

        IList<SType> GetQuoteSType();

        IList<QuoteCustomerInfo> GetQuoteCustomerInfoById(int id);

        void SaveQuoteCustomerInfo(QuoteCustomerInfo qc);

        void CreateQuoteCustomerInfo(QuoteCustomerInfo qc);

        IList<QuoteItem> GetQuoteItemByCode(string code);

        IList<QuoteItem> GetQuoteItemById(string Id);

        void DeleteQuoteItemById(string id);
        void UpdateQuoteItem(QuoteItem qt);

        void CreateQuoteItem(QuoteItem qt);

        #region Load
        IList<PT> GetPT();

        IList<BoardMode> GetBoardMode();

        IList<PackMode> GetPackMode();

        IList<OutBox> GetOutBox();

        IList<Plate> GetPlate();

        IList<Partition> GetPartition();

        IList<BubbleBag> GetBubbleBag();

        IList<Blister> GetBlister();

        IList<CooperationMode> GetCooperationMode();

        IList<QFor> GetQFor();

        IList<TSType> GetTSType();

        IList<LogisticsMode> GetLogisticsMode();

        IList<LogisticsFee> GetLogisticsFeeByCityName(string cityName);
        #endregion

        void CreatProductInfo(ProductInfo productInfo);

        IList<ProductInfo> GetProductInfoById(string id);

        void UpdateProductInfo(ProductInfo productInfo);

        void CreateCostCategory(CostCategory costCategory);

        void UpdateCostCategory(CostCategory costCategory);

        void DeleteCostCategory(CostCategory costCategory);

        IList<CostCategory> GetCostCategoryById(string id);

        IList<CostList> GetCostListById(string id);

        IList<CostCategory> GetCostCategory(string userCode);

        IList<CostList> GetCostListByCCId(string id);

        IList<CusTemplate> GetCostListByCusCodeAndCCId(string cusCode, string ccId);

        IList<CusTemplate> GetCostListByIdAndCusCodeAndCCId(string id,string cusCode, string ccId);

        void DeleteCusTemplate(CusTemplate ct);

        void CreateCusTemplate(CusTemplate ct);

        void CreateCostList(CostList cl);

        void UpdateCostList(CostList cl);

        IList<CusTemplate> GetCusTemplateByCusCode(string code);

        IList<QuoteCustomerInfo> GetQuoteCustomerInfoByCode(string code,bool status);

        IList<CalculatePara> GetCalculatePara();

        void CreateProject(Project project);

        IList<Project> GetProjectByProjectId(string projectId);

        IList<Project> GetProjectById(string Id);

        void UpdateProductInfoStatus(ProductInfo p);

        void UpdateCusTemplate(CusTemplate cusTemplate);

        void UpdateProject(Project project);

        IList<QuoteCustomerInfo> GetQuoteCustomer(object para);

        void DeleteQuoteCustomerById(string id);

        IList<QuoteCustomerInfo> GetCustomer();

        #region 生成项目ID
        void CreateGPID(GPID gpid);

        void DeleteGPIDById(string id);

        IList<GPID> GetGPIDById(string id);

        void UpdateGPID(GPID gpid);

        IList<GPID> GetGPID(bool Status);

        IList<QuoteItem> GetQuoteItemByPID(string pid);
        #endregion
        #region ItemPack
        void CreateItemPack(ItemPack itemPack);

        IList<ItemPack> GetItemPack(int id);

        void DeleteItemPackById(int id);

        void UpdateItemPack(ItemPack itemPack);

        IList<ItemPack> GetItemPack(string spec);
        #endregion

        void QuoteProjectCopy(string id);

        #region 导出成PDF
        void ExportToPDF(Project project,string imgUrl);

        PdfPCell CreateCell(string text, Font textFont, int colspan);
        #endregion

        IList<QuoteItem> GetItemByProductId(int id);

        void DeleteQuoteItemByProductId(string id);

        void SaveQuoteItem(QuoteItem qi);

        IList<QuoteItem> GetXLSQuoteItem(Stream stream);
    }
}

#region Extend Interface

namespace com.Sconit.Service.Ext.Quote
{
    public partial interface IToolingMgrE : com.Sconit.Service.Quote.IToolingMgr
    {
    }
}

#endregion Extend Interface

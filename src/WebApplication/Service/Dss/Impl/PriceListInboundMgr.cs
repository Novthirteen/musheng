using com.Sconit.Service.Ext.Dss;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Utility;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity;
using com.Sconit.Service.Distribution.Impl;
using com.Sconit.Service.Distribution;
using com.Sconit.Service.Procurement;
using com.Sconit.Entity.Procurement;
using com.Sconit.Entity.Distribution;
using Castle.Services.Transaction;
using com.Sconit.Entity.Dss;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Service.Ext.Procurement;

namespace com.Sconit.Service.Dss.Impl
{
    public class PriceListInboundMgr : AbstractInboundMgr
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger("Log.DssInbound");
        
        
        
        public IPriceListMgrE priceListMgrE { get; set; }
        public IPartyMgrE partyMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public IItemMgrE itemMgrE { get; set; }
        public IUomMgrE uomMgrE { get; set; }
        public ICurrencyMgrE currencyMgrE { get; set; }
        public IPriceListDetailMgrE priceListDetailMgrE { get; set; }
        public ISalesPriceListMgrE salesPriceListMgrE { get; set; }
        public IPurchasePriceListMgrE purchasePriceListMgrE { get; set; }

        private string[] fields = new string[] 
            { 
                "UnitPrice",
                "EndDate"
            };

		public PriceListInboundMgr(
            IDssImportHistoryMgrE dssImportHistoryMgrE)
            : base(dssImportHistoryMgrE)
        {
           
        }
        protected override object DeserializeForDelete(DssImportHistory dssImportHistory)
        {
            return this.Deserialize(dssImportHistory, false);
        }

        protected override object DeserializeForCreate(DssImportHistory dssImportHistory)
        {
            return this.Deserialize(dssImportHistory, true);
        }

        private object Deserialize(DssImportHistory dssImportHistory, bool isUpdate)
        {
            PriceListDetail priceListDetail = new PriceListDetail();
            Party party = partyMgrE.CheckAndLoadParty(dssImportHistory[1]);
            if (party.Type.Equals(BusinessConstants.PARTY_TYPE_CUSTOMER))
            {
                priceListDetail.PriceList = this.LoadSalesPriceList(dssImportHistory[1], party);//销售价格单
            }
            else
            {
                priceListDetail.PriceList = this.LoadPurchasePriceList(dssImportHistory[1], party);//采购价格单
            }
            priceListDetail.Currency = this.currencyMgrE.CheckAndLoadCurrency(dssImportHistory[2]);//货币
            priceListDetail.Item = this.itemMgrE.CheckAndLoadItem(dssImportHistory[3]);//零件号
            priceListDetail.Uom = this.uomMgrE.CheckAndLoadUom(dssImportHistory[4]);//单位
            priceListDetail.StartDate = dssImportHistory[6] != null ? DssHelper.GetDate(dssImportHistory[6], BusinessConstants.DSS_SYSTEM_CODE_QAD) : DateTime.Now;//开始日期
            if (isUpdate)
            {
                priceListDetail.UnitPrice = decimal.Parse(dssImportHistory[5]);//单价
                if (dssImportHistory[7] != null) priceListDetail.EndDate = DssHelper.GetDate(dssImportHistory[7], BusinessConstants.DSS_SYSTEM_CODE_QAD);//结束日期
            }

            #region 默认值
            priceListDetail.TaxCode = string.Empty;//todo
            priceListDetail.IsIncludeTax = false;
            priceListDetail.IsProvisionalEstimate = this.CheckProvisionalEstimate(priceListDetail.UnitPrice);
            #endregion

            return priceListDetail;
        }

        protected override void CreateOrUpdateObject(object obj)
        {
            PriceListDetail priceListDetail = (PriceListDetail)obj;

            PriceListDetail newPriceListDetail =
                this.priceListDetailMgrE.LoadPriceListDetail(priceListDetail.PriceList.Code, priceListDetail.Currency.Code,
                priceListDetail.Item.Code, priceListDetail.Uom.Code, priceListDetail.StartDate);
            if (newPriceListDetail == null)
            {
                this.priceListDetailMgrE.CreatePriceListDetail(priceListDetail);
            }
            else
            {
                CloneHelper.CopyProperty(priceListDetail, newPriceListDetail, this.fields);
                this.priceListDetailMgrE.UpdatePriceListDetail(newPriceListDetail);
            }
        }

        protected override void DeleteObject(object obj)
        {
            PriceListDetail priceListDetail = (PriceListDetail)obj;

            PriceListDetail newPriceListDetail =
                this.priceListDetailMgrE.LoadPriceListDetail(priceListDetail.PriceList.Code, priceListDetail.Currency.Code,
                priceListDetail.Item.Code, priceListDetail.Uom.Code, priceListDetail.StartDate);
            if (newPriceListDetail != null)
            {
                newPriceListDetail.EndDate = DateTime.Today.AddDays(-1);
                this.priceListDetailMgrE.UpdatePriceListDetail(newPriceListDetail);
            }
        }

        #region Private Method
        /// <summary>
        /// YFK客户化,小数点后三四位为11的为暂沽价
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        private bool CheckProvisionalEstimate(decimal price)
        {
            int i = Convert.ToInt32((price * 10000) % 100);
            if (i == 11)
            {
                return true;
            }

            return false;
        }

        [Transaction(TransactionMode.Requires)]
        private SalesPriceList LoadSalesPriceList(string code, Party party)
        {
            SalesPriceList salesPriceList = salesPriceListMgrE.LoadSalesPriceList(code);
            if (salesPriceList == null)
            {
                salesPriceList = new SalesPriceList();
                salesPriceList.Code = code;
                salesPriceList.Party = party;
                salesPriceList.IsActive = true;
                this.salesPriceListMgrE.CreateSalesPriceList(salesPriceList);
            }
            return salesPriceList;
        }

        [Transaction(TransactionMode.Requires)]
        private PurchasePriceList LoadPurchasePriceList(string code, Party party)
        {
            PurchasePriceList purchasePriceList = purchasePriceListMgrE.LoadPurchasePriceList(code);
            if (purchasePriceList == null)
            {
                purchasePriceList = new PurchasePriceList();
                purchasePriceList.Code = code;
                purchasePriceList.Party = party;
                purchasePriceList.IsActive = true;
                this.purchasePriceListMgrE.CreatePurchasePriceList(purchasePriceList);
            }
            return purchasePriceList;
        }
        #endregion
    }
}




#region Extend Class



namespace com.Sconit.Service.Ext.Dss.Impl
{
    public partial class PriceListInboundMgrE : com.Sconit.Service.Dss.Impl.PriceListInboundMgr, IInboundMgrE
    {
        public PriceListInboundMgrE(
            IDssImportHistoryMgrE dssImportHistoryMgrE)
            : base(dssImportHistoryMgrE)
        {
           
        }
    }
}

#endregion

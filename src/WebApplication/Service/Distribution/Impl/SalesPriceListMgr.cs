using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.Distribution;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.Distribution;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Distribution;
using NHibernate.Expression;

//TODO: Add other using statements here.

namespace com.Sconit.Service.Distribution.Impl
{
    [Transactional]
    public class SalesPriceListMgr : SalesPriceListBaseMgr, ISalesPriceListMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IBillAddressMgrE billAddressMgrE { get; set; }
        

        #region Customized Methods
        //TODO: Add other methods here.
        
        [Transaction(TransactionMode.Unspecified)]
        public SalesPriceList LoadSalesPriceList(string code, bool includeDetail)
        {
            SalesPriceList salesPriceList = this.LoadSalesPriceList(code);
            if (salesPriceList != null && includeDetail && salesPriceList.PriceListDetails != null && salesPriceList.PriceListDetails.Count > 0)
            {

            }
            return salesPriceList;
        }

        [Transaction(TransactionMode.Requires)]
        public IList<SalesPriceList> GetAllSalesPriceList(string addressCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For<SalesPriceList>();
            criteria.Add(Expression.Eq("IsActive", true));
            if (addressCode != null && addressCode != string.Empty)
            {
                BillAddress billAddress = billAddressMgrE.LoadBillAddress(addressCode);
                criteria.Add(Expression.Or(Expression.Eq("Party.Code", billAddress.Party.Code), Expression.IsNull("Party")));
            }
            else
            {
                criteria.Add(Expression.IsNull("Party"));
            }
            return criteriaMgrE.FindAll<SalesPriceList>(criteria);
        }

        [Transaction(TransactionMode.Requires)]
        public IList<SalesPriceList> GetAllSalesPriceList(BillAddress address)
        {
            return GetAllSalesPriceList(address.Party.Code);
        }

        #endregion Customized Methods
    }
}



#region Extend Class


namespace com.Sconit.Service.Ext.Distribution.Impl
{
    [Transactional]
    public partial class SalesPriceListMgrE : com.Sconit.Service.Distribution.Impl.SalesPriceListMgr, ISalesPriceListMgrE
    {
        
    }
}
#endregion

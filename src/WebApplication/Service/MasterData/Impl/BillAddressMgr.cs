using System.Collections.Generic;
using System.Linq;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.MasterData;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class BillAddressMgr : BillAddressBaseMgr, IBillAddressMgr
    {
        public ICustomerMgrE customerMgrE { get; set; }
        public ISupplierMgrE supplierMgrE { get; set; }

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public IList<BillAddress> GetBillAddress(bool isSupplier)
        {
            IList<BillAddress> billAddressList = GetAllBillAddress();
            if (isSupplier)
            {
                IList<Supplier> supplierList = supplierMgrE.GetAllSupplier();
                billAddressList = billAddressList.Where(b => (supplierList.Select(s => s.Code).ToList()).Contains(b.Party.Code)).ToList();
            }
            else
            {
                IList<Customer> customerList = customerMgrE.GetAllCustomer();
                billAddressList = billAddressList.Where(b => (customerList.Select(c => c.Code).ToList()).Contains(b.Party.Code)).ToList();
            }
            return billAddressList;
        }
        #endregion Customized Methods
    }
}


#region Extend Class





namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class BillAddressMgrE : com.Sconit.Service.MasterData.Impl.BillAddressMgr, IBillAddressMgrE
    {

    }
}
#endregion

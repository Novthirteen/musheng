using com.Sconit.Service.Ext.MasterData;


using System.Collections;
using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using NHibernate.Expression;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class AddressMgr : AddressBaseMgr, IAddressMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }


        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public ShipAddress LoadShipAddress(string code)
        {
            return this.entityDao.LoadShipAddress(code);
        }

        [Transaction(TransactionMode.Unspecified)]
        public BillAddress LoadBillAddress(string code)
        {
            return this.entityDao.LoadBillAddress(code);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList GetAllBillAddress()
        {
            return GetAllBillAddress(false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList GetAllBillAddress(bool includeInactive)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(BillAddress));
            if (!includeInactive)
            {
                criteria.Add(Expression.Eq("IsActive", true));
            }
            return criteriaMgrE.FindAll(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList GetBillAddress(string partyCode)
        {
            return GetBillAddress(partyCode, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList GetBillAddress(string partyCode, bool includeInactive)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(BillAddress)).CreateAlias("Party", "P").Add(Expression.Eq("P.Code", partyCode));
            if (!includeInactive)
            {
                criteria.Add(Expression.Eq("IsActive", true));
            }
            return criteriaMgrE.FindAll(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList GetBillAddress(Party party)
        {
            return GetBillAddress(party.Code, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList GetBillAddress(Party party, bool includeInactive)
        {
            return GetBillAddress(party.Code, includeInactive);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList GetAllShipAddress()
        {
            return GetAllShipAddress(false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList GetAllShipAddress(bool includeInactive)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(ShipAddress));
            if (!includeInactive)
            {
                criteria.Add(Expression.Eq("IsActive", true));
            }
            return criteriaMgrE.FindAll(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList GetShipAddress(string partyCode)
        {
            return GetShipAddress(partyCode, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList GetShipAddress(string partyCode, bool includeInactive)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(ShipAddress)).CreateAlias("Party", "P").Add(Expression.Eq("P.Code", partyCode));
            if (!includeInactive)
            {
                criteria.Add(Expression.Eq("IsActive", true));
            }
            return criteriaMgrE.FindAll(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList GetShipAddress(Party party)
        {
            return GetShipAddress(party.Code, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList GetShipAddress(Party party, bool includeInactive)
        {
            return GetShipAddress(party.Code, includeInactive);
        }

        [Transaction(TransactionMode.Requires)]
        public void DeleteAddressByParent(string code)
        {
            this.entityDao.DeleteAddressByParent(code);
        }

        #endregion Customized Methods
    }
}


#region Extend Class
namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class AddressMgrE : com.Sconit.Service.MasterData.Impl.AddressMgr, IAddressMgrE
    {
       
    }
}
#endregion

using com.Sconit.Service.Ext.MasterData;


using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
//TODO: Add other using statements here.
using NHibernate.Expression;
using com.Sconit.Entity.Exception;

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class PartyMgr : PartyBaseMgr, IPartyMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IRegionMgrE RegionMgrE { get; set; }
        public ISupplierMgrE SupplierMgrE { get; set; }
        public ICustomerMgrE CustomerMgrE { get; set; }
       

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public Party CheckAndLoadParty(string partyCode)
        {
            Party party = this.LoadParty(partyCode);
            if (party == null)
            {
                throw new BusinessErrorException("Party.Error.PartyCodeNotExist", partyCode);
            }

            return party;
        }

        public IList<Party> GetFromParty(string orderType, string userCode)
        {
            return GetFromParty(orderType, userCode, false);
        }

        public IList<Party> GetFromParty(string orderType, string userCode, bool includeInactive)
        {
            DetachedCriteria criteria = DetachedCriteria.For<Party>();

            if (orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT || orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING)
            {
                DetachedCriteria[] pfCrieteria = SecurityHelper.GetSupplierPermissionCriteria(userCode);
                criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("Code", pfCrieteria[0]),
                        Subqueries.PropertyIn("Code", pfCrieteria[1])
                ));
            }
            else if (orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION)
            {
                DetachedCriteria[] pfCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);
                criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("Code", pfCrieteria[0]),
                        Subqueries.PropertyIn("Code", pfCrieteria[1])
                ));
            }
            else if (orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION)
            {
                DetachedCriteria[] regionCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);
                criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("Code", regionCrieteria[0]),
                        Subqueries.PropertyIn("Code", regionCrieteria[1])
                ));
            }           
            else if (orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_TRANSFER)
            {
                DetachedCriteria[] pfCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);

                criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("Code", pfCrieteria[0]),
                        Subqueries.PropertyIn("Code", pfCrieteria[1])
                ));
            }
            else if (orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS)
            {
                DetachedCriteria[] pfCrieteria = SecurityHelper.GetCustomerPermissionCriteria(userCode);
                criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("Code", pfCrieteria[0]),
                        Subqueries.PropertyIn("Code", pfCrieteria[1])
                ));
            }

            if (!includeInactive)
            {
                criteria.Add(Expression.Eq("IsActive", true));
            }

            return this.criteriaMgrE.FindAll<Party>(criteria);
        }

        public IList<Party> GetOrderFromParty(string orderType, string userCode)
        {
            return GetOrderFromParty(orderType, userCode, false);
        }

        public IList<Party> GetOrderFromParty(string orderType, string userCode, bool includeInactive)
        {
            DetachedCriteria criteria = DetachedCriteria.For<Party>();

            if (orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT || orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING)
            {
                DetachedCriteria[] pfCrieteria = SecurityHelper.GetPartyPermissionCriteria(userCode);
                criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("Code", pfCrieteria[0]),
                        Subqueries.PropertyIn("Code", pfCrieteria[1])
                ));
            }
            else if (orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION)
            {
                DetachedCriteria[] pfCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);
                criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("Code", pfCrieteria[0]),
                        Subqueries.PropertyIn("Code", pfCrieteria[1])
                ));
            }
            else if (orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION)
            {
                DetachedCriteria[] regionCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);
                criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("Code", regionCrieteria[0]),
                        Subqueries.PropertyIn("Code", regionCrieteria[1])
                ));
            }            
            else if (orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_TRANSFER)
            {
                DetachedCriteria[] pfCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);
                criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("Code", pfCrieteria[0]),
                        Subqueries.PropertyIn("Code", pfCrieteria[1])
                ));
            }
            else if (orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS)
            {
                DetachedCriteria[] pfCrieteria = SecurityHelper.GetCustomerPermissionCriteria(userCode);
                criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("Code", pfCrieteria[0]),
                        Subqueries.PropertyIn("Code", pfCrieteria[1])
                ));
            }

            if (!includeInactive)
            {
                criteria.Add(Expression.Eq("IsActive", true));
            }

            return this.criteriaMgrE.FindAll<Party>(criteria);
        }

        public IList<Party> GetToParty(string orderType, string userCode)
        {
            return GetToParty(orderType, userCode, false);
        }

        public IList<Party> GetToParty(string orderType, string userCode, bool includeInactive)
        {
            DetachedCriteria criteria = DetachedCriteria.For<Party>();

            if (orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT || orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING)
            {
                DetachedCriteria[] pfCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);
                criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("Code", pfCrieteria[0]),
                        Subqueries.PropertyIn("Code", pfCrieteria[1])
                ));
            }
            else if (orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION)
            {
                DetachedCriteria[] pfCrieteria = SecurityHelper.GetCustomerPermissionCriteria(userCode);
                criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("Code", pfCrieteria[0]),
                        Subqueries.PropertyIn("Code", pfCrieteria[1])
                ));
            }
            else if (orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION)
            {
                DetachedCriteria[] regionCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);
                criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("Code", regionCrieteria[0]),
                        Subqueries.PropertyIn("Code", regionCrieteria[1])
                ));
            }
            else if (orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_INSPECTION)
            {
                DetachedCriteria[] regionCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);
                criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("Code", regionCrieteria[0]),
                        Subqueries.PropertyIn("Code", regionCrieteria[1])
                ));
            }
            else if (orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_TRANSFER)
            {
                DetachedCriteria[] regionCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);
                criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("Code", regionCrieteria[0]),
                        Subqueries.PropertyIn("Code", regionCrieteria[1])
                ));
            }
            else if (orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS)
            {
                DetachedCriteria[] pfCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);
                criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("Code", pfCrieteria[0]),
                        Subqueries.PropertyIn("Code", pfCrieteria[1])
                ));
            }

            if (!includeInactive)
            {
                criteria.Add(Expression.Eq("IsActive", true));
            }

            return this.criteriaMgrE.FindAll<Party>(criteria);
        }

        public IList<Party> GetOrderToParty(string orderType, string userCode)
        {
            return GetOrderToParty(orderType, userCode, false);
        }

        public IList<Party> GetOrderToParty(string orderType, string userCode, bool includeInactive)
        {
            DetachedCriteria criteria = DetachedCriteria.For<Party>();

            if (orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT || orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING)
            {
                DetachedCriteria[] pfCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);
                criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("Code", pfCrieteria[0]),
                        Subqueries.PropertyIn("Code", pfCrieteria[1])
                ));
            }
            else if (orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION)
            {
                DetachedCriteria[] pfCrieteria = SecurityHelper.GetPartyPermissionCriteria(userCode, new string[] { BusinessConstants.CODE_MASTER_PARTY_TYPE_VALUE_CUSTOMER, BusinessConstants.CODE_MASTER_PARTY_TYPE_VALUE_REGION });
                criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("Code", pfCrieteria[0]),
                        Subqueries.PropertyIn("Code", pfCrieteria[1])
                ));
            }
            else if (orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION)
            {
                DetachedCriteria[] regionCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);
                criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("Code", regionCrieteria[0]),
                        Subqueries.PropertyIn("Code", regionCrieteria[1])
                ));
            }
            else if (orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_TRANSFER)
            {
                DetachedCriteria[] regionCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);
                criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("Code", regionCrieteria[0]),
                        Subqueries.PropertyIn("Code", regionCrieteria[1])
                ));
            }
            else if (orderType == BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS)
            {
                DetachedCriteria[] pfCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);
                criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("Code", pfCrieteria[0]),
                        Subqueries.PropertyIn("Code", pfCrieteria[1])
                ));
            }

            if (!includeInactive)
            {
                criteria.Add(Expression.Eq("IsActive", true));
            }

            return this.criteriaMgrE.FindAll<Party>(criteria);
        }

        public IList<Party> GetAllParty(string type)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Party));
            criteria.Add(Expression.Eq("IsActive", true));
            IList<Party> listParty = this.criteriaMgrE.FindAll<Party>(criteria);
            IList<Party> listParty0 = new List<Party>();
            foreach (Party party in listParty)
            {
                if (party.Type.Trim().ToLower() == type.Trim().ToLower())
                {
                    listParty0.Add(party);
                }
            }

            return listParty0;
        }

        public IList<Party> GetAllParty(string userCode, string type)
        {
            IList<Party> listParty = new List<Party>();
            if (type.Trim().ToLower() == BusinessConstants.PARTY_TYPE_SUPPLIER.Trim().ToLower()
                || type == null || type.Trim().ToLower() == string.Empty)
            {
                IList<Supplier> listSupplier = SupplierMgrE.GetSupplier(userCode);
                foreach (Supplier supplier in listSupplier)
                {
                    listParty.Add((Party)supplier);
                }
            }
            else if (type.Trim().ToLower() == BusinessConstants.PARTY_TYPE_CUSTOMER.Trim().ToLower()
                || type == null || type.Trim().ToLower() == string.Empty)
            {
                IList<Customer> listCustomer = CustomerMgrE.GetCustomer(userCode);
                foreach (Customer customer in listCustomer)
                {
                    listParty.Add((Party)customer);
                }
            }
            else
            {
                listParty = null;
            }

            return listParty;
        }

        public string GetDefaultInspectLocation(string partyCode)
        {
            return this.GetDefaultInspectLocation(partyCode, null);
        }

        public string GetDefaultInspectLocation(string partyCode, string defaultInspectLocation)
        {
            Party party = this.CheckAndLoadParty(partyCode);
            if (party.GetType() == typeof(Region))
            {
                return this.RegionMgrE.GetDefaultInspectLocation(partyCode, defaultInspectLocation);
            }
            else
            {
                return null;
            }
        }

        public string GetDefaultRejectLocation(string partyCode)
        {
            return this.GetDefaultRejectLocation(partyCode, null);
        }

        public string GetDefaultRejectLocation(string partyCode, string defaultRejectLocation)
        {
            Party party = this.CheckAndLoadParty(partyCode);
            if (party.GetType() == typeof(Region))
            {
                return this.RegionMgrE.GetDefaultRejectLocation(partyCode, defaultRejectLocation);
            }
            else
            {
                return null;
            }
        }
        #endregion Customized Methods
    }
}


#region Extend Class



namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class PartyMgrE : com.Sconit.Service.MasterData.Impl.PartyMgr, IPartyMgrE
    {
        
    }
}
#endregion

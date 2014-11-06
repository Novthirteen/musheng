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
using com.Sconit.Entity.Dss;

namespace com.Sconit.Service.Dss.Impl
{
    public class CustomerInboundMgr : AbstractInboundMgr
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger("Log.DssInbound");

        
        public ICustomerMgrE customerMgrE { get; set; }
        public IPartyMgrE partyMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public IShipAddressMgrE shipAddressMgrE { get; set; }
        public IBillAddressMgrE billAddressMgrE { get; set; }

        private string[] Customer2CustomerFields = new string[] 
            { 
                "Code",
                "Name"
            };

        private string[] ShipAddress2ShipAddressFields = new string[] 
            { 
                "Code",
                "Address",
                "ContactPersonName",
                "TelephoneNumber",
                "Fax",
                "MobilePhone",
                "PostalCode"
            };

        private string[] BillAddress2BillAddressFields = new string[] 
            { 
                "Code",
                "Address",
                "ContactPersonName",
                "TelephoneNumber",
                "Fax",
                "MobilePhone",
                "PostalCode"
            };


        public CustomerInboundMgr(
            IDssImportHistoryMgrE dssImportHistoryMgrE)
            : base(dssImportHistoryMgrE)
        {

        }

        protected override object DeserializeForDelete(DssImportHistory dssImportHistory)
        {
            return this.Deserialize(dssImportHistory);
        }

        protected override object DeserializeForCreate(DssImportHistory dssImportHistory)
        {
            return this.Deserialize(dssImportHistory);
        }

        private object Deserialize(DssImportHistory dssImportHistory)
        {
            Customer customer = new Customer();
            customer.Code = dssImportHistory[1].ToUpper();
            customer.Name = dssImportHistory[2];

            ShipAddress shipAddress = new ShipAddress();
            shipAddress.Code = customer.Code.ToUpper() + "_S";
            shipAddress.Address = dssImportHistory[3];
            shipAddress.ContactPersonName = dssImportHistory[12];
            shipAddress.TelephoneNumber = dssImportHistory[13] + dssImportHistory[14];
            shipAddress.Fax = dssImportHistory[15];
            shipAddress.MobilePhone = dssImportHistory[17] + dssImportHistory[18];
            shipAddress.PostalCode = dssImportHistory[8];
            shipAddress.Party = customer;

            BillAddress billAddress = new BillAddress();
            billAddress.Code = customer.Code.ToUpper() + "_B";
            billAddress.Address = dssImportHistory[3];
            billAddress.ContactPersonName = dssImportHistory[12];
            billAddress.TelephoneNumber = dssImportHistory[13] + dssImportHistory[14];
            billAddress.Fax = dssImportHistory[15];
            billAddress.MobilePhone = dssImportHistory[17] + dssImportHistory[18];
            billAddress.PostalCode = dssImportHistory[8];
            billAddress.Party = customer;

            IList<object> list = new List<object>();
            list.Add(customer);
            list.Add(shipAddress);
            list.Add(billAddress);
            return list;
        }

        protected override void CreateOrUpdateObject(object obj)
        {
            IList<object> list = (IList<object>)obj;
            Customer customer = (Customer)list[0];
            ShipAddress shipAddress = (ShipAddress)list[1];
            BillAddress billAddress = (BillAddress)list[2];

            Party newParty = this.partyMgrE.LoadParty(customer.Code);

            if (newParty == null)
            {

                customer.IsActive = true;
                //customer.LastModifyUser = this.userMgrE.GetMonitorUser();
                //customer.LastModifyDate = DateTime.Now;
                this.customerMgrE.CreateCustomer(customer);
            }
            else
            {
                if (newParty.Type.Equals(BusinessConstants.PARTY_TYPE_CUSTOMER))
                {
                    Customer newCustomer = this.customerMgrE.LoadCustomer(customer.Code);
                    CloneHelper.CopyProperty(customer, newCustomer, this.Customer2CustomerFields);
                    //newCustomer.LastModifyUser = this.userMgrE.GetMonitorUser();
                    //newCustomer.LastModifyDate = DateTime.Now;
                    this.customerMgrE.UpdateCustomer(newCustomer);
                }
                else
                {
                    throw new BusinessErrorException("Inbound.Customer.Error.Customer.Numbers.Already.Exist.But.For.The.Type.Of.Customers", customer.Code);
                }
            }


            BillAddress newBillAddress = this.billAddressMgrE.LoadBillAddress(billAddress.Code);
            if (newBillAddress == null)
            {

                billAddress.IsActive = true;
                //billAddress.LastModifyUser = this.userMgrE.GetMonitorUser();
                // billAddress.LastModifyDate = DateTime.Now;
                this.billAddressMgrE.CreateBillAddress(billAddress);
            }
            else
            {
                CloneHelper.CopyProperty(billAddress, newBillAddress, this.BillAddress2BillAddressFields);
                //newBillAddress.LastModifyUser = this.userMgrE.GetMonitorUser();
                //newBillAddress.LastModifyDate = DateTime.Now;
                this.billAddressMgrE.UpdateBillAddress(newBillAddress);
            }

            ShipAddress newShipAddress = this.shipAddressMgrE.LoadShipAddress(shipAddress.Code);
            if (newShipAddress == null)
            {
                shipAddress.IsActive = true;
                //shipAddress.LastModifyUser = this.userMgrE.GetMonitorUser();
                //shipAddress.LastModifyDate = DateTime.Now;
                this.shipAddressMgrE.CreateShipAddress(shipAddress);
            }
            else
            {
                CloneHelper.CopyProperty(shipAddress, newShipAddress, this.ShipAddress2ShipAddressFields);
                // newShipAddress.LastModifyUser = this.userMgrE.GetMonitorUser();
                // newShipAddress.LastModifyDate = DateTime.Now;
                this.shipAddressMgrE.UpdateShipAddress(newShipAddress);
            }

        }

        protected override void DeleteObject(object obj)
        {
            IList<object> list = (IList<object>)obj;
            Customer customer = (Customer)list[0];
            ShipAddress shipAddress = (ShipAddress)list[1];
            BillAddress billAddress = (BillAddress)list[2];

            customer.IsActive = false;
            this.customerMgrE.UpdateCustomer(customer);

            shipAddress.IsActive = false;
            this.shipAddressMgrE.UpdateShipAddress(shipAddress);

            billAddress.IsActive = false;
            this.billAddressMgrE.UpdateBillAddress(billAddress);
        }
    }
}




#region Extend Class

namespace com.Sconit.Service.Ext.Dss.Impl
{
    public partial class CustomerInboundMgrE : com.Sconit.Service.Dss.Impl.CustomerInboundMgr, IInboundMgrE
    {
        public CustomerInboundMgrE(
            IDssImportHistoryMgrE dssImportHistoryMgrE)
            : base(dssImportHistoryMgrE)
        {

        }
    }
}

#endregion

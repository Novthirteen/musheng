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
    public class SupplierInboundMgr : AbstractInboundMgr
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger("Log.DssInbound");
        
        
        public ISupplierMgrE supplierMgrE { get; set; }
        public IPartyMgrE partyMgrE { get; set; }
        public IUserMgrE userMgrE { get; set; }
        public IShipAddressMgrE shipAddressMgrE { get; set; }
        public IBillAddressMgrE billAddressMgrE { get; set; }

        private string[] Supplier2SupplierFields = new string[] 
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


        public SupplierInboundMgr(
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

            Supplier supplier = new Supplier();
            supplier.Code = dssImportHistory[1].ToUpper();
            supplier.Name = dssImportHistory[3];

            ShipAddress shipAddress = new ShipAddress();
            shipAddress.Code = supplier.Code.ToUpper() + "_S";
            shipAddress.Address = dssImportHistory[4];
            shipAddress.ContactPersonName = dssImportHistory[13];
            shipAddress.TelephoneNumber = dssImportHistory[14] + dssImportHistory[15];
            shipAddress.Fax = dssImportHistory[16];
            shipAddress.MobilePhone = dssImportHistory[18] + dssImportHistory[19];
            shipAddress.PostalCode = dssImportHistory[9];
            shipAddress.Party = supplier;

            BillAddress billAddress = new BillAddress();
            billAddress.Code = supplier.Code.ToUpper() + "_B";
            billAddress.Address = dssImportHistory[4];
            billAddress.ContactPersonName = dssImportHistory[13];
            billAddress.TelephoneNumber = dssImportHistory[14] + dssImportHistory[15];
            billAddress.Fax = dssImportHistory[16];
            billAddress.MobilePhone = dssImportHistory[18] + dssImportHistory[19];
            billAddress.PostalCode = dssImportHistory[9];
            billAddress.Party = supplier;

            IList<object> list = new List<object>();
            list.Add(supplier);
            list.Add(shipAddress);
            list.Add(billAddress);
            return list;
        }

        protected override void CreateOrUpdateObject(object obj)
        {
            IList<object> list = (IList<object>)obj;
            Supplier supplier = (Supplier)list[0];
            ShipAddress shipAddress = (ShipAddress)list[1];
            BillAddress billAddress = (BillAddress)list[2];

            Party newParty = this.partyMgrE.LoadParty(supplier.Code);

            if (newParty == null)
            {
                supplier.IsActive = true;
                this.supplierMgrE.CreateSupplier(supplier);
            }
            else
            {
                if (newParty.Type.Equals(BusinessConstants.PARTY_TYPE_SUPPLIER))
                {
                    Supplier newSupplier = this.supplierMgrE.LoadSupplier(supplier.Code);
                    CloneHelper.CopyProperty(supplier, newSupplier, this.Supplier2SupplierFields);
                    this.supplierMgrE.UpdateSupplier(newSupplier);
                }
                else
                {
                    throw new BusinessErrorException("Inbound.Customer.Error.Supplier.Numbers.Already.Exist.But.For.The.Type.Of.Suppliers", supplier.Code);
                }
            }


            BillAddress newBillAddress = this.billAddressMgrE.LoadBillAddress(billAddress.Code);
            if (newBillAddress == null)
            {

                billAddress.IsActive = true;
                this.billAddressMgrE.CreateBillAddress(billAddress);
            }
            else
            {
                CloneHelper.CopyProperty(billAddress, newBillAddress, this.BillAddress2BillAddressFields);
                this.billAddressMgrE.UpdateBillAddress(newBillAddress);
            }

            ShipAddress newShipAddress = this.shipAddressMgrE.LoadShipAddress(shipAddress.Code);
            if (newShipAddress == null)
            {
                shipAddress.IsActive = true;
                this.shipAddressMgrE.CreateShipAddress(shipAddress);
            }
            else
            {
                CloneHelper.CopyProperty(shipAddress, newShipAddress, this.ShipAddress2ShipAddressFields);
                this.shipAddressMgrE.UpdateShipAddress(newShipAddress);
            }

        }

        protected override void DeleteObject(object obj)
        {
            IList<object> list = (IList<object>)obj;
            Supplier supplier = (Supplier)list[0];
            ShipAddress shipAddress = (ShipAddress)list[1];
            BillAddress billAddress = (BillAddress)list[2];

            supplier.IsActive = false;
            this.supplierMgrE.UpdateSupplier(supplier);

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
    public partial class SupplierInboundMgrE : com.Sconit.Service.Dss.Impl.SupplierInboundMgr, IInboundMgrE
    {
        public SupplierInboundMgrE(
            IDssImportHistoryMgrE dssImportHistoryMgrE)
            : base(dssImportHistoryMgrE)
        {
        }
    }
}

#endregion

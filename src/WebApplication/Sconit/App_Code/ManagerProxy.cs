using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Entity.MasterData;
using com.Sconit.Utility;
using com.Sconit.Persistence;
using NHibernate;
using Castle.Facilities.NHibernateIntegration;
using Castle.Services.Transaction;
using com.Sconit.Service.Ext.Procurement;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Entity.Procurement;
using com.Sconit.Entity.Distribution;
using com.Sconit.Entity.Batch;
using com.Sconit.Service.Ext.Batch;
using com.Sconit.Service.Ext.Cost;
using com.Sconit.Entity.Cost;
using com.Sconit.Entity.Customize;
using com.Sconit.Service.Ext.Customize;
using com.Sconit.Service.Ext.MRP;

/// <summary>
/// Summary description for ManagerProxy
/// </summary>
namespace com.Sconit.Web
{
    public class CriteriaMgrProxy
    {
        private ICriteriaMgrE CriteriaMgr
        {
            get
            {
                return ServiceLocator.GetService<ICriteriaMgrE>("CriteriaMgr.service");
            }
        }

        public CriteriaMgrProxy()
        {
        }

        public IList FindAll(DetachedCriteria selectCriteria, int firstRow, int maxRows)
        {
            return CriteriaMgr.FindAll(selectCriteria, firstRow, maxRows);
        }

        public int FindCount(DetachedCriteria selectCriteria)
        {
            IList list = CriteriaMgr.FindAll(selectCriteria);
            if (list != null && list.Count > 0)
            {
                if (list[0] is int)
                {
                    return int.Parse(list[0].ToString());
                }
                else if (list[0] is object[])
                {
                    return int.Parse(((object[])list[0])[0].ToString());
                }
                //由于性能问题,此后禁用该方法。
                //else if (list[0] is object)
                //{
                //    return list.Count;
                //}
                else
                {
                    throw new Exception("unknow result type");
                }
            }
            else
            {
                return 0;
            }
        }
    }

    public class CodeMasterMgrProxy
    {
        private ICodeMasterMgrE CodeMasterMgr
        {
            get
            {
                return ServiceLocator.GetService<ICodeMasterMgrE>("CodeMasterMgr.service");
            }
        }

        public CodeMasterMgrProxy()
        {
        }

        public IList<CodeMaster> GetCachedCodeMaster(string code)
        {
            return CodeMasterMgr.GetCachedCodeMaster(code);
        }

        public CodeMaster GetCachedCodeMaster(string code, string value)
        {
            return CodeMasterMgr.GetCachedCodeMaster(code, value);
        }

    }

    public class UserMgrProxy
    {
        private IUserMgrE UserMgr
        {
            get
            {
                return ServiceLocator.GetService<IUserMgrE>("UserMgr.service");
            }
        }

        private ICriteriaMgrE CriteriaMgr
        {
            get
            {
                return ServiceLocator.GetService<ICriteriaMgrE>("CriteriaMgr.service");
            }
        }

        public UserMgrProxy()
        {
        }

        public void CreateUser(User user)
        {
            UserMgr.CreateUser(user);
        }

        public List<User> FindUser(DetachedCriteria selectCriteria, int firstRow, int maxRows)
        {
            IList targetList = this.CriteriaMgr.FindAll(selectCriteria);
            List<User> resultList = new List<User>();
            return resultList.GetRange(firstRow, (firstRow + maxRows) > resultList.Count ? (resultList.Count - firstRow) : maxRows);
        }

        public int FindUserCount(DetachedCriteria selectCriteria)
        {
            IList list = CriteriaMgr.FindAll(selectCriteria);
            return list.Count;
        }

        public User LoadUser(string code)
        {
            return UserMgr.LoadUser(code);
        }

        public void UpdateUser(User user)
        {
            UserMgr.UpdateUser(user);
        }

        public void DeleteUser(User user)
        {
            UserMgr.DeleteUser(user);
        }
        public IList<Permission> GetAllPermissions(string code)
        {
            return UserMgr.GetAllPermissions(code);
        }
    }

    public class RegionMgrProxy
    {
        private IRegionMgrE RegionMgr
        {
            get
            {
                return ServiceLocator.GetService<IRegionMgrE>("RegionMgr.service");
            }
        }

        public RegionMgrProxy()
        {
        }

        public void CreateRegion(Region region)
        {
            RegionMgr.CreateRegion(region);
        }

        public Region LoadRegion(string code)
        {
            return RegionMgr.LoadRegion(code);
        }

        public void UpdateRegion(Region region)
        {
            RegionMgr.UpdateRegion(region);
        }

        public void DeleteRegion(Region region)
        {
            RegionMgr.DeleteRegion(region);
        }
    }

    public class RoleMgrProxy
    {
        private IRoleMgrE RoleMgr
        {
            get
            {
                return ServiceLocator.GetService<IRoleMgrE>("RoleMgr.service");
            }
        }

        public RoleMgrProxy()
        {
        }

        public void CreateRole(Role role)
        {
            RoleMgr.CreateRole(role);
        }

        public Role LoadRole(string code)
        {
            return RoleMgr.LoadRole(code);
        }

        public void UpdateRole(Role role)
        {
            RoleMgr.UpdateRole(role);
        }

        public void DeleteRole(Role role)
        {
            RoleMgr.DeleteRole(role);
        }
    }

    public class SupplierMgrProxy
    {
        private ISupplierMgrE SupplierMgr
        {
            get
            {
                return ServiceLocator.GetService<ISupplierMgrE>("SupplierMgr.service");
            }
        }

        public SupplierMgrProxy()
        {
        }

        public void CreateSupplier(Supplier supplier)
        {
            SupplierMgr.CreateSupplier(supplier);
        }

        public Supplier LoadSupplier(string code)
        {
            return SupplierMgr.LoadSupplier(code);
        }

        public void UpdateSupplier(Supplier supplier)
        {
            SupplierMgr.UpdateSupplier(supplier);
        }

        public void DeleteSupplier(Supplier supplier)
        {
            SupplierMgr.DeleteSupplier(supplier);
        }
    }

    public class CustomerMgrProxy
    {
        private ICustomerMgrE CustomerMgr
        {
            get
            {
                return ServiceLocator.GetService<ICustomerMgrE>("CustomerMgr.service");
            }
        }

        public CustomerMgrProxy()
        {
        }

        public void CreateCustomer(Customer customer)
        {
            CustomerMgr.CreateCustomer(customer);
        }

        public Customer LoadCustomer(string code)
        {
            return CustomerMgr.LoadCustomer(code);
        }

        public void UpdateCustomer(Customer customer)
        {
            CustomerMgr.UpdateCustomer(customer);
        }

        public void DeleteCustomer(Customer customer)
        {
            CustomerMgr.DeleteCustomer(customer);
        }
    }

    public class WorkCenterMgrProxy
    {
        private IWorkCenterMgrE WorkCenterMgr
        {
            get
            {
                return ServiceLocator.GetService<IWorkCenterMgrE>("WorkCenterMgr.service");
            }
        }

        public WorkCenterMgrProxy()
        {
        }

        public void CreateWorkCenter(WorkCenter workCenter)
        {
            WorkCenterMgr.CreateWorkCenter(workCenter);
        }

        public WorkCenter LoadWorkCenter(string code)
        {
            return WorkCenterMgr.LoadWorkCenter(code);
        }

        public void UpdateWorkCenter(WorkCenter workCenter)
        {
            WorkCenterMgr.UpdateWorkCenter(workCenter);
        }

        public void DeleteWorkCenter(WorkCenter workCenter)
        {
            WorkCenterMgr.DeleteWorkCenter(workCenter);
        }
    }

    public class EntityPreferenceMgrProxy
    {
        private IEntityPreferenceMgrE EntityPreferenceMgr
        {
            get
            {
                return ServiceLocator.GetService<IEntityPreferenceMgrE>("EntityPreferenceMgr.service");
            }
        }

        public EntityPreferenceMgrProxy()
        {
        }

        public void UpdateEntityPreference()
        { }

        public IList<EntityPreference> LoadEntityPreference()
        {
            return EntityPreferenceMgr.GetAllEntityPreferenceOrderBySeq();
        }

        public void UpdateEntityPreference(EntityPreference entityPreference)
        {
            EntityPreferenceMgr.UpdateEntityPreference(entityPreference);
        }

    }

    public class WorkdayMgrProxy
    {
        private IWorkdayMgrE WorkdayMgr
        {
            get
            {
                return ServiceLocator.GetService<IWorkdayMgrE>("WorkdayMgr.service");
            }
        }

        public WorkdayMgrProxy()
        {
        }

        public void CreateWorkday(Workday workday)
        {
            WorkdayMgr.CreateWorkday(workday);
        }

        public Workday LoadWorkday(int ID)
        {
            return WorkdayMgr.LoadWorkday(ID);
        }

        public void UpdateWorkday(Workday workday)
        {
            WorkdayMgr.UpdateWorkday(workday);
        }

        public void DeleteWorkday(Workday workday)
        {
            WorkdayMgr.DeleteWorkday(workday.Id, true);
        }
    }

    public class UserRoleMgrProxy
    {
        private IUserRoleMgrE UserRoleMgr
        {
            get
            {
                return ServiceLocator.GetService<IUserRoleMgrE>("UserRoleMgr.service");
            }
        }

        public UserRoleMgrProxy()
        {
        }

        public IList<Role> GetRolesNotInUser(string code)
        {
            return UserRoleMgr.GetRolesNotInUser(code);
        }

        public IList<Role> GetRolesByUserCode(string code)
        {
            return UserRoleMgr.GetRolesByUserCode(code);
        }

        public IList<User> GetUsersNotInRole(string code)
        {
            return UserRoleMgr.GetUsersNotInRole(code);
        }

        public IList<User> GetUsersByRoleCode(string code)
        {
            return UserRoleMgr.GetUsersByRoleCode(code);
        }
    }

    public class ShiftMgrProxy
    {
        private IShiftMgrE ShiftMgr
        {
            get
            {
                return ServiceLocator.GetService<IShiftMgrE>("ShiftMgr.service");
            }
        }

        public ShiftMgrProxy()
        {
        }

        public void CreateShift(Shift shift)
        {
            ShiftMgr.CreateShift(shift);
        }

        public Shift LoadShift(String code)
        {
            return ShiftMgr.LoadShift(code);
        }

        public void UpdateShift(Shift shift)
        {
            ShiftMgr.UpdateShift(shift);
        }

        public void DeleteShift(Shift shift)
        {
            ShiftMgr.DeleteShift(shift);
        }
    }

    public class ShiftDetailMgrProxy
    {
        private IShiftDetailMgrE ShiftDetailMgr
        {
            get
            {
                return ServiceLocator.GetService<IShiftDetailMgrE>("ShiftDetailMgr.service");
            }
        }

        public ShiftDetailMgrProxy()
        {
        }

        public void CreateShiftDetail(ShiftDetail shiftDetail)
        {
            ShiftDetailMgr.CreateShiftDetail(shiftDetail);
        }

        public ShiftDetail LoadShiftDetail(int Id)
        {
            return ShiftDetailMgr.LoadShiftDetail(Id);
        }

        public void UpdateShiftDetail(ShiftDetail shiftDetail)
        {
            ShiftDetailMgr.UpdateShiftDetail(shiftDetail);
        }

        public void DeleteShiftDetail(ShiftDetail shiftDetail)
        {
            ShiftDetailMgr.DeleteShiftDetail(shiftDetail);
        }
    }

    public class SpecialTimeMgrProxy
    {
        private ISpecialTimeMgrE SpecialTimeMgr
        {
            get
            {
                return ServiceLocator.GetService<ISpecialTimeMgrE>("SpecialTimeMgr.service");
            }
        }

        public SpecialTimeMgrProxy()
        {
        }

        public void CreateSpecialTime(SpecialTime specialTime)
        {
            SpecialTimeMgr.CreateSpecialTime(specialTime);
        }

        public SpecialTime LoadSpecialTime(int ID)
        {
            return SpecialTimeMgr.LoadSpecialTime(ID);
        }

        public void UpdateSpecialTime(SpecialTime specialTime)
        {
            SpecialTimeMgr.UpdateSpecialTime(specialTime);
        }

        public void DeleteSpecialTime(SpecialTime specialTime)
        {
            SpecialTimeMgr.DeleteSpecialTime(specialTime);
        }
    }

    public class UserPermissionMgrProxy
    {
        private IUserPermissionMgrE UserPermissionMgr
        {
            get
            {
                return ServiceLocator.GetService<IUserPermissionMgrE>("UserPermissionMgr.service");
            }
        }

        public UserPermissionMgrProxy()
        {
        }

        public IList<Permission> GetPermissionsNotInUser(string code, string categoryCode)
        {
            if (code == string.Empty || code == null) return new List<Permission>();
            return UserPermissionMgr.GetPermissionsNotInUser(code, categoryCode);
        }

        public IList<Permission> GetPermissionsByUserCode(string code, string categoryCode)
        {
            if (code == string.Empty || code == null) return new List<Permission>();
            return UserPermissionMgr.GetPermissionsByUserCode(code, categoryCode);
        }
    }

    public class RolePermissionMgrProxy
    {
        private IRolePermissionMgrE RolePermissionMgr
        {
            get
            {
                return ServiceLocator.GetService<IRolePermissionMgrE>("RolePermissionMgr.service");
            }
        }

        public RolePermissionMgrProxy()
        {
        }

        public IList<Permission> GetPermissionsNotInRole(string code, string categoryCode)
        {
            if (code == string.Empty || code == null) return new List<Permission>();
            return RolePermissionMgr.GetPermissionsNotInRole(code, categoryCode);
        }

        public IList<Permission> GetPermissionsByRoleCode(string code, string categoryCode)
        {
            if (code == string.Empty || code == null) return new List<Permission>();
            return RolePermissionMgr.GetPermissionsByRoleCode(code, categoryCode);
        }
    }


    public class ItemMgrProxy
    {
        private IItemMgrE ItemMgr
        {
            get
            {
                return ServiceLocator.GetService<IItemMgrE>("ItemMgr.service");
            }
        }

        public ItemMgrProxy()
        {
        }

        public void CreateItem(Item item)
        {
            ItemMgr.CreateItem(item);
        }

        public Item LoadItem(string code)
        {
            return ItemMgr.LoadItem(code);
        }

        public void UpdateItem(Item item)
        {
            ItemMgr.UpdateItem(item);
        }

        public void DeleteItem(Item item)
        {
            ItemMgr.DeleteItem(item);
        }
    }


    public class ItemCategoryMgrProxy
    {
        private IItemCategoryMgrE ItemCategoryMgr
        {
            get
            {
                return ServiceLocator.GetService<IItemCategoryMgrE>("ItemCategoryMgr.service");
            }
        }

        public ItemCategoryMgrProxy()
        {
        }

        public void CreateItemCategory(ItemCategory itemCateogry)
        {
            ItemCategoryMgr.CreateItemCategory(itemCateogry);
        }

        public ItemCategory LoadItemCategory(string code)
        {
            return ItemCategoryMgr.LoadItemCategory(code);
        }

        public IList GetItemCategory(string code, string desc1)
        {
            return ItemCategoryMgr.GetItemCategory(code, desc1);
        }

        public void UpdateItemCategory(ItemCategory itemCategory)
        {
            ItemCategoryMgr.UpdateItemCategory(itemCategory);
        }

        public void DeleteItemCategory(ItemCategory itemCategory)
        {
            ItemCategoryMgr.DeleteItemCategory(itemCategory);
        }
    }

    public class ItemDisConMgrProxy
    {
        private IItemDiscontinueMgrE ItemDisConMgr
        {
            get
            {
                return ServiceLocator.GetService<IItemDiscontinueMgrE>("ItemDiscontinueMgr.service");
            }
        }

        public ItemDisConMgrProxy()
        {
        }

        public void CreateItemDiscontinue(ItemDiscontinue itemDisCon)
        {
            ItemDisConMgr.CreateItemDiscontinue(itemDisCon);
        }

        public ItemDiscontinue LoadItemDiscontinue(Int32 id)
        {
            return ItemDisConMgr.LoadItemDiscontinue(id);
        }

        public void UpdateItemDiscontinue(ItemDiscontinue itemDisCon)
        {
            ItemDisConMgr.UpdateItemDiscontinue(itemDisCon);
        }

        public void DeleteItemDisCon(ItemDiscontinue itemDisCon)
        {
            ItemDisConMgr.DeleteItemDiscontinue(itemDisCon);
        }
    }


    public class ItemBrandMgrProxy
    {
        private IItemBrandMgrE ItemBrandMgr
        {
            get
            {
                return ServiceLocator.GetService<IItemBrandMgrE>("ItemBrandMgr.service");
            }
        }

        public ItemBrandMgrProxy()
        {
        }

        public void CreateItemBrand(ItemBrand itemBrand)
        {
            ItemBrandMgr.CreateItemBrand(itemBrand);
        }

        public ItemBrand LoadItemBrand(string code)
        {
            return ItemBrandMgr.LoadItemBrand(code);
        }

        public void UpdateItemBrand(ItemBrand itemBrand)
        {
            ItemBrandMgr.UpdateItemBrand(itemBrand);
        }

        public void DeleteItemBrand(ItemBrand itemBrand)
        {
            ItemBrandMgr.DeleteItemBrand(itemBrand);
        }

        public IList<ItemBrand> GetItemBrandIncludeEmpty()
        {
            return ItemBrandMgr.GetItemBrandIncludeEmpty();
        }
    }

    public class AddressMgrProxy
    {
        private IAddressMgrE AddressMgr
        {
            get
            {
                return ServiceLocator.GetService<IAddressMgrE>("AddressMgr.service");
            }
        }

        public AddressMgrProxy()
        {
        }

        public void CreateAddress(Address address)
        {
            AddressMgr.CreateAddress(address);
        }

        public Address LoadAddress(string code)
        {
            return AddressMgr.LoadAddress(code);
        }

        public void UpdateAddress(Address address)
        {
            AddressMgr.UpdateAddress(address);
        }

        public void DeleteAddress(Address address)
        {
            AddressMgr.DeleteAddress(address);
        }
    }

    public class BillAddressMgrProxy
    {
        private IBillAddressMgrE BillAddressMgr
        {
            get
            {
                return ServiceLocator.GetService<IBillAddressMgrE>("BillAddressMgr.service");
            }
        }

        public BillAddressMgrProxy()
        {
        }

        public void CreateBillAddress(BillAddress address)
        {
            BillAddressMgr.CreateBillAddress(address);
        }

        public BillAddress LoadBillAddress(string code)
        {
            return BillAddressMgr.LoadBillAddress(code);
        }

        public void UpdateBillAddress(BillAddress address)
        {
            BillAddressMgr.UpdateBillAddress(address);
        }

        public void DeleteBillAddress(BillAddress address)
        {
            BillAddressMgr.DeleteBillAddress(address);
        }
    }

    public class ShipAddressMgrProxy
    {
        private IShipAddressMgrE ShipAddressMgr
        {
            get
            {
                return ServiceLocator.GetService<IShipAddressMgrE>("ShipAddressMgr.service");
            }
        }

        public ShipAddressMgrProxy()
        {
        }

        public void CreateShipAddress(ShipAddress address)
        {
            ShipAddressMgr.CreateShipAddress(address);
        }

        public ShipAddress LoadShipAddress(string code)
        {
            return ShipAddressMgr.LoadShipAddress(code);
        }

        public void UpdateShipAddress(ShipAddress address)
        {
            ShipAddressMgr.UpdateShipAddress(address);
        }

        public void DeleteShipAddress(ShipAddress address)
        {
            ShipAddressMgr.DeleteShipAddress(address);
        }
    }

    public class BomMgrProxy
    {
        private IBomMgrE BomMgr
        {
            get
            {
                return ServiceLocator.GetService<IBomMgrE>("BomMgr.service");
            }
        }

        public BomMgrProxy()
        {
        }

        public void CreateBom(Bom bom)
        {
            BomMgr.CreateBom(bom);
        }

        public Bom LoadBom(string code)
        {
            return BomMgr.LoadBom(code);
        }

        public void UpdateBom(Bom bom)
        {
            BomMgr.UpdateBom(bom);
        }

        public void DeleteBom(Bom bom)
        {
            BomMgr.DeleteBom(bom);
        }
    }

    public class BomDetailMgrProxy
    {
        private IBomDetailMgrE BomDetailMgr
        {
            get
            {
                return ServiceLocator.GetService<IBomDetailMgrE>("BomDetailMgr.service");
            }
        }

        public BomDetailMgrProxy()
        {
        }

        public void CreateBomDetail(BomDetail bomdetail)
        {
            BomDetailMgr.CreateBomDetail(bomdetail);
        }

        public BomDetail LoadBomDetail(int ID)
        {
            return BomDetailMgr.LoadBomDetail(ID);
        }

        public void UpdateBomDetail(BomDetail bomdetail)
        {
            BomDetailMgr.UpdateBomDetail(bomdetail);
        }

        public void DeleteBomDetail(BomDetail bomdetail)
        {
            BomDetailMgr.DeleteBomDetail(bomdetail);
        }
    }

    public class UomMgrProxy
    {
        private IUomMgrE UomMgr
        {
            get
            {
                return ServiceLocator.GetService<IUomMgrE>("UomMgr.service");
            }
        }

        public UomMgrProxy()
        {
        }

        public IList<Uom> GetAllUom()
        {
            return UomMgr.GetAllUom();
        }

        public void CreateUom(Uom location)
        {
            UomMgr.CreateUom(location);
        }

        public IList LoadUom(string code, string name, string desc)
        {
            return UomMgr.GetUom(code, name, desc);
        }

        public void UpdateUom(Uom location)
        {
            UomMgr.UpdateUom(location);
        }

        public void DeleteUom(Uom location)
        {
            UomMgr.DeleteUom(location);
        }
    }

    public class WorkdayShiftMgrProxy
    {
        private IWorkdayShiftMgrE WorkdayShiftMgr
        {
            get
            {
                return ServiceLocator.GetService<IWorkdayShiftMgrE>("WorkdayShiftMgr.service");
            }
        }

        public WorkdayShiftMgrProxy()
        {
        }

        public IList<Shift> GetShiftsNotInWorkday(int Id)
        {
            return WorkdayShiftMgr.GetShiftsNotInWorkday(Id);


        }

        public IList<Shift> GetShiftsByWorkdayId(int Id)
        {
            return WorkdayShiftMgr.GetShiftsByWorkdayId(Id);
        }
    }

    public class FlowMgrProxy
    {
        private IFlowMgrE FlowMgr
        {
            get
            {
                return ServiceLocator.GetService<IFlowMgrE>("FlowMgr.service");
            }
        }

        public Flow LoadFlow(string code)
        {
            if (code == null) return null;
            return FlowMgr.LoadFlow(code);
        }

        public void CreateFlow(Flow entity)
        {
            FlowMgr.CreateFlow(entity);
        }

        public void UpdateFlow(Flow entity)
        {
            FlowMgr.UpdateFlow(entity);
        }

        public void DeleteFlow(Flow entity)
        {
            FlowMgr.DeleteFlow(entity);
        }

    }

    public class LocationMgrProxy
    {
        private ILocationMgrE LocationMgr
        {
            get
            {
                return ServiceLocator.GetService<ILocationMgrE>("LocationMgr.service");
            }
        }

        public LocationMgrProxy()
        {
        }

        public void CreateLocation(Location location)
        {
            LocationMgr.CreateLocation(location);
        }

        public Location LoadLocation(string code)
        {
            return LocationMgr.LoadLocation(code);
        }

        public void UpdateLocation(Location location)
        {
            LocationMgr.UpdateLocation(location);
        }

        public void DeleteLocation(Location location)
        {
            LocationMgr.DeleteLocation(location);
        }
    }

    public class ItemPointMgrProxy
    {
        private IOrderProductionPlanMgrE ItemPointMgr
        {
            get
            {
                return ServiceLocator.GetService<IOrderProductionPlanMgrE>("OrderProductionPlanMgr.service");
            }
        }
        public ItemPointMgrProxy()
        {
        }

        public IList<ItemPoint> LoadItemPoint(string Item)
        {
            return ItemPointMgr.GetItemPoint(Item);
        }

        public void UpdateItemPoint(ItemPoint ItemPoint)
        {
            ItemPointMgr.UpdateItemPoint(ItemPoint);
        }

        public void DeleteItemPoint(ItemPoint ItemPoint)
        {
            ItemPointMgr.DeleteItemPoint(ItemPoint);
        }
    }

    public class CurrencyMgrProxy
    {
        private ICurrencyMgrE CurrencyMgr
        {
            get
            {
                return ServiceLocator.GetService<ICurrencyMgrE>("CurrencyMgr.service");
            }
        }

        public CurrencyMgrProxy()
        {
        }

        public void CreateCurrency(Currency currency)
        {
            CurrencyMgr.CreateCurrency(currency);
        }

        public IList LoadCurrency(string code, string name)
        {
            return CurrencyMgr.GetCurrency(code, name);
        }

        public void UpdateCurrency(Currency currency)
        {
            CurrencyMgr.UpdateCurrency(currency);
        }

        public void DeleteCurrency(Currency currency)
        {
            CurrencyMgr.DeleteCurrency(currency);
        }
    }

    public class CurrencyExchangeMgrProxy
    {
        private ICurrencyExchangeMgrE CurrencyExchangeMgr
        {
            get
            {
                return ServiceLocator.GetService<ICurrencyExchangeMgrE>("CurrencyExchangeMgr.service");
            }
        }

        public CurrencyExchangeMgrProxy()
        {
        }

        public void CreateCurrencyExchange(CurrencyExchange currencyExchange)
        {
            CurrencyExchangeMgr.CreateCurrencyExchange(currencyExchange);
        }

        public IList GetCurrencyExchange(string baseCurrency, string exchangeCurrency)
        {
            return CurrencyExchangeMgr.GetCurrencyExchange(baseCurrency, exchangeCurrency);
        }

        public void UpdateCurrencyExchange(CurrencyExchange currencyExchange)
        {
            CurrencyExchangeMgr.UpdateCurrencyExchange(currencyExchange);
        }

        public void DeleteCurrencyExchange(CurrencyExchange currencyExchange)
        {
            CurrencyExchangeMgr.DeleteCurrencyExchange(currencyExchange);
        }
    }

    public class UomConversionMgrProxy
    {
        private IUomConversionMgrE UomConversionMgr
        {
            get
            {
                return ServiceLocator.GetService<IUomConversionMgrE>("UomConversionMgr.service");
            }
        }

        public UomConversionMgrProxy()
        {
        }

        public void CreateUomConversion(UomConversion uomConversion)
        {
            UomConversionMgr.CreateUomConversion(uomConversion);
        }

        public IList LoadUomConversion(string itemCode)
        {
            return UomConversionMgr.GetUomConversion(itemCode, null, null);
        }

        public void UpdateUomConversion(UomConversion uomConversion)
        {
            UomConversionMgr.UpdateUomConversion(uomConversion);
        }

        public void DeleteUomConversion(UomConversion uomConversion)
        {
            UomConversionMgr.DeleteUomConversion(uomConversion);
        }
    }

    public class PriceListMgrProxy
    {
        private IPriceListMgrE PriceListMgr
        {
            get
            {
                return ServiceLocator.GetService<IPriceListMgrE>("PriceListMgr.service");
            }
        }

        private IPurchasePriceListMgrE PurchasePriceListMgr
        {
            get
            {
                return ServiceLocator.GetService<IPurchasePriceListMgrE>("PurchasePriceListMgr.service");
            }
        }

        private ISalesPriceListMgrE SalesPriceListMgr
        {
            get
            {
                return ServiceLocator.GetService<ISalesPriceListMgrE>("SalesPriceListMgr.service");
            }
        }

        public PriceListMgrProxy()
        {
        }

        public void CreatePriceList(PriceList priceList)
        {
            PriceListMgr.CreatePriceList(priceList);
        }
        public void CreatePurchasePriceList(PurchasePriceList purchasePriceList)
        {
            PurchasePriceListMgr.CreatePurchasePriceList(purchasePriceList);
        }
        public void CreateSalesPriceList(SalesPriceList salesPriceList)
        {
            SalesPriceListMgr.CreateSalesPriceList(salesPriceList);
        }

        public PriceList LoadPriceList(string code)
        {
            return PriceListMgr.LoadPriceList(code);
        }
        public PurchasePriceList LoadPurchasePriceList(string code)
        {
            return PurchasePriceListMgr.LoadPurchasePriceList(code);
        }
        public SalesPriceList LoadSalesPriceList(string code)
        {
            return SalesPriceListMgr.LoadSalesPriceList(code);
        }

        public void UpdatePriceList(PriceList priceList)
        {
            PriceListMgr.UpdatePriceList(priceList);
        }
        public void UpdatePurchasePriceList(PurchasePriceList purchasePriceList)
        {
            PurchasePriceListMgr.UpdatePurchasePriceList(purchasePriceList);
        }
        public void UpdateSalesPriceList(SalesPriceList salesPriceList)
        {
            SalesPriceListMgr.UpdateSalesPriceList(salesPriceList);
        }

        public void DeletePriceList(PriceList priceList)
        {
            PriceListMgr.DeletePriceList(priceList);
        }
        public void DeletePurchasePriceList(PurchasePriceList purchasePriceList)
        {
            PurchasePriceListMgr.DeletePurchasePriceList(purchasePriceList);
        }
        public void DeleteSalesPriceList(SalesPriceList salesPriceList)
        {
            SalesPriceListMgr.DeleteSalesPriceList(salesPriceList);
        }

        private ICustomerGoodsPriceListMgrE CustomerGoodsPriceListMgr
        {
            get
            {
                return ServiceLocator.GetService<ICustomerGoodsPriceListMgrE>("CustomerGoodsPriceListMgr.service");
            }
        }

        public CustomerGoodsPriceList LoadCustomerGoodsPriceList(String Code)
        {
            if (Code == null) return null;
            return CustomerGoodsPriceListMgr.LoadCustomerGoodsPriceList(Code);
        }

        public void CreateCustomerGoodsPriceList(CustomerGoodsPriceList entity)
        {
            CustomerGoodsPriceListMgr.CreateCustomerGoodsPriceList(entity);
        }

        public void UpdateCustomerGoodsPriceList(CustomerGoodsPriceList entity)
        {
            CustomerGoodsPriceListMgr.UpdateCustomerGoodsPriceList(entity);
        }

        public void DeleteCustomerGoodsPriceList(CustomerGoodsPriceList entity)
        {
            CustomerGoodsPriceListMgr.DeleteCustomerGoodsPriceList(entity);
        }
    }

    public class PriceListDetailMgrProxy
    {
        private IPriceListDetailMgrE PriceListDetailMgr
        {
            get
            {
                return ServiceLocator.GetService<IPriceListDetailMgrE>("PriceListDetailMgr.service");
            }
        }

        public PriceListDetailMgrProxy()
        {
        }

        public void CreatePriceListDetail(PriceListDetail priceListDetail)
        {
            PriceListDetailMgr.CreatePriceListDetail(priceListDetail);
        }

        public PriceListDetail LoadPriceListDetail(int ID)
        {
            return PriceListDetailMgr.LoadPriceListDetail(ID);
        }

        public void UpdatePriceListDetail(PriceListDetail priceListDetail)
        {
            PriceListDetailMgr.UpdatePriceListDetail(priceListDetail);
        }

        public void DeletePriceListDetail(PriceListDetail priceListDetail)
        {
            PriceListDetailMgr.DeletePriceListDetail(priceListDetail);
        }
    }

    public class FlowDetailMgrProxy
    {
        private IFlowDetailMgrE FlowDetailMgr
        {
            get
            {
                return ServiceLocator.GetService<IFlowDetailMgrE>("FlowDetailMgr.service");
            }
        }

        public FlowDetailMgrProxy()
        {
        }

        public void CreateFlowDetail(FlowDetail itemFlowDetail)
        {
            FlowDetailMgr.CreateFlowDetail(itemFlowDetail);
        }

        public FlowDetail FindFlowDetail(Int32 id)
        {
            return FlowDetailMgr.LoadFlowDetail(id);
        }

        public void UpdateFlowDetail(FlowDetail itemFlowDetail)
        {
            FlowDetailMgr.UpdateFlowDetail(itemFlowDetail);
        }

        public void DeleteFlowDetail(FlowDetail itemFlowDetail)
        {
            FlowDetailMgr.DeleteFlowDetail(itemFlowDetail);
        }
    }


    public class ProdutLineFeedSeqenceMgrProxy
    {
        private IProdutLineFeedSeqenceMgrE ProdutLineFeedSeqenceMgr
        {
            get
            {
                return ServiceLocator.GetService<IProdutLineFeedSeqenceMgrE>("ProdutLineFeedSeqenceMgr.service");
            }
        }

        public ProdutLineFeedSeqenceMgrProxy()
        {
        }

        public void CreateProdutLineFeedSeqence(ProdutLineFeedSeqence produtLineFeedSeqence)
        {
            ProdutLineFeedSeqenceMgr.CreateProdutLineFeedSeqence(produtLineFeedSeqence);
        }

        public ProdutLineFeedSeqence LoadProdutLineFeedSeqence(Int32 id)
        {
            return ProdutLineFeedSeqenceMgr.LoadProdutLineFeedSeqence(id);
        }

        public void UpdateProdutLineFeedSeqence(ProdutLineFeedSeqence produtLineFeedSeqence)
        {
            ProdutLineFeedSeqenceMgr.UpdateProdutLineFeedSeqence(produtLineFeedSeqence);
        }

        public void DeleteProdutLineFeedSeqence(ProdutLineFeedSeqence produtLineFeedSeqence)
        {
            ProdutLineFeedSeqenceMgr.DeleteProdutLineFeedSeqence(produtLineFeedSeqence);
        }
    }

    public class ProductLineFacilityMgrProxy
    {
        private IProductLineFacilityMgrE ProductLineFacilityMgr
        {
            get
            {
                return ServiceLocator.GetService<IProductLineFacilityMgrE>("ProductLineFacilityMgr.service");
            }
        }

        public ProductLineFacilityMgrProxy()
        {
        }

        public void CreateProductLineFacility(ProductLineFacility productLineFacility)
        {
            ProductLineFacilityMgr.CreateProductLineFacility(productLineFacility);
        }

        public ProductLineFacility FindProductLineFacility(int id)
        {
            return ProductLineFacilityMgr.LoadProductLineFacility(id);
        }

        public void UpdateProductLineFacility(ProductLineFacility productLineFacility)
        {
            ProductLineFacilityMgr.UpdateProductLineFacility(productLineFacility);
        }

        public void DeleteProductLineFacility(ProductLineFacility productLineFacility)
        {
            ProductLineFacilityMgr.DeleteProductLineFacility(productLineFacility);
        }
    }

    public class FlowBindingMgrProxy
    {
        private IFlowBindingMgrE FlowBindingMgr
        {
            get
            {
                return ServiceLocator.GetService<IFlowBindingMgrE>("FlowBindingMgr.service");
            }
        }

        public FlowBinding LoadFlowBinding(int id)
        {
            return FlowBindingMgr.LoadFlowBinding(id);
        }

        public void CreateFlowBinding(FlowBinding entity)
        {
            FlowBindingMgr.CreateFlowBinding(entity);
        }

        public void UpdateFlowBinding(FlowBinding entity)
        {
            FlowBindingMgr.UpdateFlowBinding(entity);
        }

        public void DeleteFlowBinding(FlowBinding entity)
        {
            FlowBindingMgr.DeleteFlowBinding(entity);
        }

    }

    public class OrderMgrProxy : System.Web.UI.UserControl
    {
        private IOrderHeadMgrE OrderHeadMgr
        {
            get
            {
                return ServiceLocator.GetService<IOrderHeadMgrE>("OrderHeadMgr.service");
            }
        }

        public OrderHead LoadOrderHead(string orderNo)
        {
            return OrderHeadMgr.LoadOrderHead(orderNo);
        }
    }


    public class ItemReferenceMgrProxy
    {
        private IItemReferenceMgrE ItemReferenceMgr
        {
            get
            {
                return ServiceLocator.GetService<IItemReferenceMgrE>("ItemReferenceMgr.service");
            }
        }

        public ItemReferenceMgrProxy()
        {
        }

        public void CreateItemReference(ItemReference itemRef)
        {
            ItemReferenceMgr.CreateItemReference(itemRef);
        }

        public IList<ItemReference> LoadItemReference(string itemCode)
        {
            return ItemReferenceMgr.GetItemReference(itemCode);
        }

        public ItemReference LoadItemReference(string itemCode, string partyCode, string referenceCode)
        {
            return ItemReferenceMgr.LoadItemReference(itemCode, partyCode, referenceCode);
        }

        public void UpdateItemReference(ItemReference itemRef)
        {
            ItemReferenceMgr.UpdateItemReference(itemRef);
        }

        public void DeleteItemReference(ItemReference itemRef)
        {
            ItemReferenceMgr.DeleteItemReference(itemRef);
        }
    }


    public class InProcessLocationMgrProxy
    {
        private IInProcessLocationMgrE InProcessLocationMgr
        {
            get
            {
                return ServiceLocator.GetService<IInProcessLocationMgrE>("InProcessLocationMgr.service");
            }
        }

        private ICriteriaMgrE CriteriaMgr
        {
            get
            {
                return ServiceLocator.GetService<ICriteriaMgrE>("CriteriaMgr.service");
            }
        }

        public InProcessLocationMgrProxy()
        {
        }

        public void CreateInProcessLocation(InProcessLocation entity)
        {
            InProcessLocationMgr.CreateInProcessLocation(entity);
        }

        public InProcessLocation LoadInProcessLocation(string code)
        {
            if (code != null && code != string.Empty)
            {
                return InProcessLocationMgr.LoadInProcessLocation(code);
            }
            else
                return null;
        }

        public void UpdateItemReference(InProcessLocation entity)
        {
            InProcessLocationMgr.UpdateInProcessLocation(entity);
        }

        public void DeleteItemReference(InProcessLocation entity)
        {
            InProcessLocationMgr.DeleteInProcessLocation(entity);
        }
    }

    public class RoutingMgrProxy
    {
        private IRoutingMgrE RoutingMgr
        {
            get
            {
                return ServiceLocator.GetService<IRoutingMgrE>("RoutingMgr.service");
            }
        }

        public RoutingMgrProxy()
        {
        }

        public void CreateRouting(Routing routing)
        {
            RoutingMgr.CreateRouting(routing);
        }

        public Routing LoadRouting(string code)
        {
            return RoutingMgr.LoadRouting(code);
        }

        public void UpdateRouting(Routing routing)
        {
            RoutingMgr.UpdateRouting(routing);
        }

        public void DeleteRouting(Routing routing)
        {
            RoutingMgr.DeleteRouting(routing);
        }
    }

    public class RoutingDetailMgrProxy
    {
        private IRoutingDetailMgrE RoutingDetailMgr
        {
            get
            {
                return ServiceLocator.GetService<IRoutingDetailMgrE>("RoutingDetailMgr.service");
            }
        }

        public RoutingDetailMgrProxy()
        {
        }

        public void CreateRoutingDetail(RoutingDetail routingDetail)
        {
            RoutingDetailMgr.CreateRoutingDetail(routingDetail);
        }

        public RoutingDetail LoadRoutingDetail(int ID)
        {
            return RoutingDetailMgr.LoadRoutingDetail(ID);
        }

        public void UpdateRoutingDetail(RoutingDetail routingDetail)
        {
            RoutingDetailMgr.UpdateRoutingDetail(routingDetail);
        }

        public void DeleteRoutingDetail(RoutingDetail routingDetail)
        {
            RoutingDetailMgr.DeleteRoutingDetail(routingDetail);
        }
    }

    public class EmployeeMgrProxy
    {
        private IEmployeeMgrE EmployeeMgr
        {
            get
            {
                return ServiceLocator.GetService<IEmployeeMgrE>("EmployeeMgr.service");
            }
        }

        public EmployeeMgrProxy()
        {
        }

        public void CreateEmployee(Employee employee)
        {
            EmployeeMgr.CreateEmployee(employee);
        }

        public Employee LoadEmployee(string code)
        {
            return EmployeeMgr.LoadEmployee(code);
        }

        public void UpdateEmployee(Employee employee)
        {
            EmployeeMgr.UpdateEmployee(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            EmployeeMgr.DeleteEmployee(employee);
        }
    }

    public class TaxRateMgrProxy
    {
        private ITaxRateMgrE TaxRateMgr
        {
            get
            {
                return ServiceLocator.GetService<ITaxRateMgrE>("TaxRateMgr.service");
            }
        }

        public TaxRateMgrProxy()
        {
        }

        public void CreateTaxRate(TaxRate taxRate)
        {
            TaxRateMgr.CreateTaxRate(taxRate);
        }

        public TaxRate LoadTaxRate(string code)
        {
            return TaxRateMgr.LoadTaxRate(code);
        }

        public void UpdateTaxRate(TaxRate taxRate)
        {
            TaxRateMgr.UpdateTaxRate(taxRate);
        }

        public void DeleteTaxRate(TaxRate taxRate)
        {
            TaxRateMgr.DeleteTaxRate(taxRate);
        }
    }

    public class ClientMgrProxy
    {
        private IClientMgrE ClientMgr
        {
            get
            {
                return ServiceLocator.GetService<IClientMgrE>("ClientMgr.service");
            }
        }

        public ClientMgrProxy()
        {
        }

        public void CreateClient(Client client)
        {
            ClientMgr.CreateClient(client);
        }

        public Client LoadClient(string ClientId)
        {
            return ClientMgr.LoadClient(ClientId);
        }

        public void UpdateClient(Client client)
        {
            ClientMgr.UpdateClient(client);
        }

        public void DeleteClient(Client client)
        {
            ClientMgr.DeleteClient(client);
        }
    }

    public class BillMgrProxy
    {
        private IBillMgrE BillMgr
        {
            get
            {
                return ServiceLocator.GetService<IBillMgrE>("BillMgr.service");
            }
        }

        public BillMgrProxy()
        {
        }

        public Bill LoadBill(string billNo, bool includeDetail)
        {
            return BillMgr.LoadBill(billNo, includeDetail);
        }
    }

    public class ReceiptMgrProxy
    {
        private IReceiptMgrE ReceiptMgr
        {
            get
            {
                return ServiceLocator.GetService<IReceiptMgrE>("ReceiptMgr.service");
            }
        }

        private ICriteriaMgrE CriteriaMgr
        {
            get
            {
                return ServiceLocator.GetService<ICriteriaMgrE>("CriteriaMgr.service");
            }
        }

        public ReceiptMgrProxy()
        {
        }

        public void CreateReceipt(Receipt entity)
        {
            ReceiptMgr.CreateReceipt(entity);
        }

        public Receipt LoadReceipt(string code)
        {
            if (code != null && code != string.Empty)
            {
                return ReceiptMgr.LoadReceipt(code);
            }
            else
                return null;
        }
    }

    public class HuMgrProxy
    {
        private IHuMgrE HuMgr
        {
            get
            {
                return ServiceLocator.GetService<IHuMgrE>("HuMgr.service");
            }
        }

        public HuMgrProxy()
        {
        }

        public void CreateHu(Hu hu)
        {
            HuMgr.CreateHu(hu);
        }

        public Hu LoadHu(string code)
        {
            return HuMgr.LoadHu(code);
        }

        public void UpdateHu(Hu hu)
        {
            HuMgr.UpdateHu(hu);
        }

        public void DeleteHu(Hu hu)
        {
            HuMgr.DeleteHu(hu);
        }
    }
    public class StorageAreaMgrProxy
    {
        private IStorageAreaMgrE TheStorageAreaMgr
        {
            get
            {
                return ServiceLocator.GetService<IStorageAreaMgrE>("StorageAreaMgr.service");
            }
        }

        public StorageAreaMgrProxy()
        {
        }

        public void CreateStorageArea(StorageArea storageArea)
        {
            TheStorageAreaMgr.CreateStorageArea(storageArea);
        }

        public StorageArea LoadStorageArea(string code)
        {
            return TheStorageAreaMgr.LoadStorageArea(code);
        }

        public void UpdateStorageArea(StorageArea storageArea)
        {
            TheStorageAreaMgr.UpdateStorageArea(storageArea);
        }

        public void DeleteStorageArea(StorageArea storageArea)
        {
            TheStorageAreaMgr.DeleteStorageArea(storageArea);
        }
    }

    public class StorageBinMgrProxy
    {
        private IStorageBinMgrE TheStorageBinMgr
        {
            get
            {
                return ServiceLocator.GetService<IStorageBinMgrE>("StorageBinMgr.service");
            }
        }

        public StorageBinMgrProxy()
        {
        }

        public void CreateStorageBin(StorageBin storageBin)
        {
            TheStorageBinMgr.CreateStorageBin(storageBin);
        }

        public StorageBin LoadStorageBin(string code)
        {
            return TheStorageBinMgr.LoadStorageBin(code);
        }

        public void UpdateStorageBin(StorageBin storageBin)
        {
            TheStorageBinMgr.UpdateStorageBin(storageBin);
        }

        public void DeleteStorageBin(StorageBin storageBin)
        {
            TheStorageBinMgr.DeleteStorageBin(storageBin);
        }
    }

    public class BatchTriggerMgrProxy
    {
        private IBatchTriggerMgrE BatchTriggerMgr
        {
            get
            {
                return ServiceLocator.GetService<IBatchTriggerMgrE>("BatchTriggerMgr.service");
            }
        }

        public BatchTriggerMgrProxy()
        {
        }

        public IList<BatchTrigger> GetActiveTrigger()
        {
            return BatchTriggerMgr.GetActiveTrigger();
        }

        public void UpdateBatchTrigger(BatchTrigger entity)
        {
            BatchTriggerMgr.UpdateBatchTrigger(entity);
        }

    }

    public class CostGroupMgrProxy
    {
        private ICostGroupMgrE CostGroupMgr
        {
            get
            {
                return ServiceLocator.GetService<ICostGroupMgrE>("CostGroupMgr.service");
            }
        }

        public CostGroupMgrProxy()
        {
        }

        public void CreateCostGroup(CostGroup costGroup)
        {
            CostGroupMgr.CreateCostGroup(costGroup);
        }

        public CostGroup LoadCostGroup(string code)
        {
            return CostGroupMgr.LoadCostGroup(code);
        }

        public void UpdateCostGroup(CostGroup costGroup)
        {
            CostGroupMgr.UpdateCostGroup(costGroup);
        }

        public void DeleteCostGroup(CostGroup costGroup)
        {
            CostGroupMgr.DeleteCostGroup(costGroup);
        }
    }

    public class CostCenterMgrProxy
    {
        private ICostCenterMgrE CostCenterMgr
        {
            get
            {
                return ServiceLocator.GetService<ICostCenterMgrE>("CostCenterMgr.service");
            }
        }

        public CostCenterMgrProxy()
        {
        }

        public void CreateCostCenter(CostCenter costCenter)
        {
            CostCenterMgr.CreateCostCenter(costCenter);
        }

        public CostCenter LoadCostCenter(string code)
        {
            return CostCenterMgr.LoadCostCenter(code);
        }

        public void UpdateCostCenter(CostCenter costCenter)
        {
            CostCenterMgr.UpdateCostCenter(costCenter);
        }

        public void DeleteCostCenter(CostCenter costCenter)
        {
            CostCenterMgr.DeleteCostCenter(costCenter);
        }
    }

    public class CostElementMgrProxy
    {
        private ICostElementMgrE CostElementMgr
        {
            get
            {
                return ServiceLocator.GetService<ICostElementMgrE>("CostElementMgr.service");
            }
        }

        public CostElementMgrProxy()
        {
        }

        public void CreateCostElement(CostElement costElement)
        {
            CostElementMgr.CreateCostElement(costElement);
        }

        public CostElement LoadCostElement(string code)
        {
            return CostElementMgr.LoadCostElement(code);
        }

        public void UpdateCostElement(CostElement costElement)
        {
            CostElementMgr.UpdateCostElement(costElement);
        }

        public void DeleteCostElement(CostElement costElement)
        {
            CostElementMgr.DeleteCostElement(costElement);
        }
    }

    public class StandardCostMgrProxy
    {
        private IStandardCostMgrE StandardCostMgr
        {
            get
            {
                return ServiceLocator.GetService<IStandardCostMgrE>("StandardCostMgr.service");
            }
        }

        public StandardCostMgrProxy()
        {
        }

        public void CreateStandardCost(StandardCost standardCost)
        {
            StandardCostMgr.CreateStandardCost(standardCost);
        }

        public StandardCost LoadStandardCost(int id)
        {
            return StandardCostMgr.LoadStandardCost(id);
        }

        public void UpdateStandardCost(StandardCost standardCost)
        {
            StandardCostMgr.UpdateStandardCost(standardCost);
        }

        public void DeleteStandardCost(StandardCost standardCost)
        {
            StandardCostMgr.DeleteStandardCost(standardCost);
        }
    }
    public class FinanceCalendarMgrProxy
    {
        private IFinanceCalendarMgrE FinanceCalendarMgr
        {
            get
            {
                return ServiceLocator.GetService<IFinanceCalendarMgrE>("FinanceCalendarMgr.service");
            }
        }

        public FinanceCalendarMgrProxy()
        {
        }

        public void CreateFinanceCalendar(FinanceCalendar financeCalendar)
        {
            FinanceCalendarMgr.CreateFinanceCalendar(financeCalendar);
        }

        public FinanceCalendar LoadFinanceCalendar(int id)
        {
            return FinanceCalendarMgr.LoadFinanceCalendar(id);
        }

        public void UpdateFinanceCalendar(FinanceCalendar financeCalendar)
        {
            FinanceCalendarMgr.UpdateFinanceCalendar(financeCalendar);
        }

        public void DeleteFinanceCalendar(FinanceCalendar financeCalendar)
        {
            FinanceCalendarMgr.DeleteFinanceCalendar(financeCalendar);
        }
    }
    public class ExpenseElementMgrProxy
    {
        private IExpenseElementMgrE ExpenseElementMgr
        {
            get
            {
                return ServiceLocator.GetService<IExpenseElementMgrE>("ExpenseElementMgr.service");
            }
        }

        public ExpenseElementMgrProxy()
        {
        }

        public void CreateExpenseElement(ExpenseElement expenseElement)
        {
            ExpenseElementMgr.CreateExpenseElement(expenseElement);
        }

        public ExpenseElement LoadExpenseElement(string code)
        {
            return ExpenseElementMgr.LoadExpenseElement(code);
        }

        public void UpdateExpenseElement(ExpenseElement expenseElement)
        {
            ExpenseElementMgr.UpdateExpenseElement(expenseElement);
        }

        public void DeleteExpenseElement(ExpenseElement expenseElement)
        {
            ExpenseElementMgr.DeleteExpenseElement(expenseElement);
        }
    }

    public class CostAllocateMethodMgrProxy
    {
        private ICostAllocateMethodMgrE CostAllocateMethodMgr
        {
            get
            {
                return ServiceLocator.GetService<ICostAllocateMethodMgrE>("CostAllocateMethodMgr.service");
            }
        }

        public CostAllocateMethodMgrProxy()
        {
        }

        public void CreateCostAllocateMethod(CostAllocateMethod costAllocateMethod)
        {
            CostAllocateMethodMgr.CreateCostAllocateMethod(costAllocateMethod);
        }

        public CostAllocateMethod LoadCostAllocateMethod(int id)
        {
            return CostAllocateMethodMgr.LoadCostAllocateMethod(id);
        }

        public void UpdateCostAllocateMethod(CostAllocateMethod costAllocateMethod)
        {
            CostAllocateMethodMgr.UpdateCostAllocateMethod(costAllocateMethod);
        }

        public void DeleteCostAllocateMethod(CostAllocateMethod costAllocateMethod)
        {
            CostAllocateMethodMgr.DeleteCostAllocateMethod(costAllocateMethod);
        }
    }

    public class ItemTypeMgrProxy
    {
        private IItemTypeMgrE ItemTypeMgr
        {
            get
            {
                return ServiceLocator.GetService<IItemTypeMgrE>("ItemTypeMgr.service");
            }
        }

        public ItemTypeMgrProxy()
        {
        }

        public void CreateItemType(ItemType itemType)
        {
            ItemTypeMgr.CreateItemType(itemType);
        }

        public ItemType LoadItemType(string code)
        {
            return ItemTypeMgr.LoadItemType(code);
        }

        public void UpdateItemType(ItemType itemType)
        {
            ItemTypeMgr.UpdateItemType(itemType);
        }

        public void DeleteItemType(ItemType itemType)
        {
            ItemTypeMgr.DeleteItemType(itemType);
        }

        public IList<ItemType> GetItemTypeIncludeEmpty(int level)
        {
            return ItemTypeMgr.GetItemType(level, true);
        }
    }

    public class LedSortLevelMgrProxy
    {
        private ILedSortLevelMgrE LedSortLevelMgr
        {
            get
            {
                return ServiceLocator.GetService<ILedSortLevelMgrE>("LedSortLevelMgr.service");
            }
        }

        public LedSortLevelMgrProxy()
        {
        }

        public void CreateLedSortLevel(LedSortLevel ledSortLevel)
        {
            LedSortLevelMgr.CreateLedSortLevel(ledSortLevel);
        }

        public LedSortLevel LoadLedSortLevel(int id)
        {
            return LedSortLevelMgr.LoadLedSortLevel(id);
        }

        public void UpdateLedSortLevel(LedSortLevel ledSortLevel)
        {
            LedSortLevelMgr.UpdateLedSortLevel(ledSortLevel);
        }

        public void DeleteLedSortLevel(LedSortLevel ledSortLevel)
        {
            LedSortLevelMgr.DeleteLedSortLevel(ledSortLevel);
        }
    }

    public class LedColorLevelMgrProxy
    {
        private ILedColorLevelMgrE LedColorLevelMgr
        {
            get
            {
                return ServiceLocator.GetService<ILedColorLevelMgrE>("LedColorLevelMgr.service");
            }
        }

        public LedColorLevelMgrProxy()
        {
        }

        public void CreateLedColorLevel(LedColorLevel ledColorLevel)
        {
            LedColorLevelMgr.CreateLedColorLevel(ledColorLevel);
        }

        public LedColorLevel LoadLedColorLevel(int id)
        {
            return LedColorLevelMgr.LoadLedColorLevel(id);
        }

        public void UpdateLedColorLevel(LedColorLevel ledColorLevel)
        {
            LedColorLevelMgr.UpdateLedColorLevel(ledColorLevel);
        }

        public void DeleteLedColorLevel(LedColorLevel ledColorLevel)
        {
            LedColorLevelMgr.DeleteLedColorLevel(ledColorLevel);
        }
    }

    public class RawIOBMgrProxy
    {
        public RawIOBMgrProxy()
        {
        }

        private IRawIOBMgrE RawIOBMgr
        {
            get
            {
                return ServiceLocator.GetService<IRawIOBMgrE>("RawIOBMgr.service");
            }
        }

        private IBalanceMgrE BalanceMgr
        {
            get
            {
                return ServiceLocator.GetService<IBalanceMgrE>("BalanceMgr.service");
            }
        }

        public RawIOB LoadRawIOB(Int32 Id)
        {
            if (Id == null) return null;
            return RawIOBMgr.LoadRawIOB(Id);
        }

        public void CreateRawIOB(RawIOB entity)
        {
            //RawIOBMgr.CreateRawIOB(entity);
        }

        public void UpdateRawIOB(RawIOB entity)
        {
            //RawIOBMgr.UpdateRawIOB(entity);
            BalanceMgr.UpdateRawIOB(entity);
        }

        public void DeleteRawIOB(RawIOB entity)
        {
            //RawIOBMgr.DeleteRawIOB(entity);
        }
    }
}
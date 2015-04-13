using System;
using System.Linq;
using System.Web.Services;
using System.Web.Services.Protocols;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.Business;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Procurement;
using com.Sconit.Service.Ext.Production;
using com.Sconit.Utility;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.View;
using com.Sconit.Service;
using System.Collections.Generic;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for BaseWS
/// </summary>
public class BaseWS : System.Web.Services.WebService
{
    public BaseWS()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    protected T GetService<T>(string serviceName)
    {
        return ServiceLocator.GetService<T>(serviceName);
    }

    public static string JsonSerializer<T>(List<T> list)
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();

        //传一个对象过去 
        String json = jss.Serialize(list);
        return json.Replace("null", "''");
    }

    #region Services
    protected IUserMgrE TheUserMgr { get { return GetService<IUserMgrE>("UserMgr.service"); } }
    protected IPermissionMgrE ThePermissionMgr { get { return GetService<IPermissionMgrE>("PermissionMgr.service"); } }
    protected IFavoritesMgrE TheFavoritesMgr { get { return GetService<IFavoritesMgrE>("FavoritesMgr.service"); } }
    protected IEntityPreferenceMgrE TheEntityPreferenceMgr { get { return GetService<IEntityPreferenceMgrE>("EntityPreferenceMgr.service"); } }
    protected IUserPreferenceMgrE TheUserPreferenceMgr { get { return GetService<IUserPreferenceMgrE>("UserPreferenceMgr.service"); } }
    protected ILanguageMgrE TheLanguageMgr { get { return GetService<ILanguageMgrE>("LanguageMgr.service"); } }
    protected ILocationMgrE TheLocationMgr { get { return GetService<ILocationMgrE>("LocationMgr.service"); } }
    protected IItemMgrE TheItemMgr { get { return GetService<IItemMgrE>("ItemMgr.service"); } }
    protected IItemReferenceMgrE TheItemReferenceMgr { get { return GetService<IItemReferenceMgrE>("ItemReferenceMgr.service"); } }
    protected IFlowMgrE TheFlowMgr { get { return GetService<IFlowMgrE>("FlowMgr.service"); } }
    protected IFlowDetailMgrE TheFlowDetailMgr { get { return GetService<IFlowDetailMgrE>("FlowDetailMgr.service"); } }
    protected IPriceListDetailMgrE ThePriceListDetailMgr { get { return GetService<IPriceListDetailMgrE>("PriceListDetailMgr.service"); } }
    protected ILocationDetailMgrE TheLocationDetailMgr { get { return GetService<ILocationDetailMgrE>("LocationDetailMgr.service"); } }
    protected IEmployeeMgrE TheEmployeeMgr { get { return GetService<IEmployeeMgrE>("EmployeeMgr.service"); } }
    protected IResolverMgrE TheResolverMgr { get { return GetService<IResolverMgrE>("ResolverMgr.service"); } }
    protected ICriteriaMgrE TheCriteriaMgr { get { return GetService<ICriteriaMgrE>("CriteriaMgr.service"); } }
    protected ISupllierLocationViewMgrE TheSupllierLocationViewMgr { get { return GetService<ISupllierLocationViewMgrE>("SupllierLocationViewMgr.service"); } }
    //protected ICodeMasterMgrE TheCodeMasterMgr { get { return GetService<ICodeMasterMgrE>("CodeMasterMgr.service"); } }
    //protected IUserRoleMgrE TheUserRoleMgr { get { return GetService<IUserRoleMgrE>("UserRoleMgr.service"); } }
    //protected IFileUploadMgrE TheFileUploadMgr { get { return GetService<IFileUploadMgrE>("FileUploadMgr.service"); } }
    //protected IRegionMgrE TheRegionMgr { get { return GetService<IRegionMgrE>("RegionMgr.service"); } }
    //protected ISupplierMgrE TheSupplierMgr { get { return GetService<ISupplierMgrE>("SupplierMgr.service"); } }
    //protected ICustomerMgrE TheCustomerMgr { get { return GetService<ICustomerMgrE>("CustomerMgr.service"); } }
    //protected IWorkCenterMgrE TheWorkCenterMgr { get { return GetService<IWorkCenterMgrE>("WorkCenterMgr.service"); } }
    //protected IRoleMgrE TheRoleMgr { get { return GetService<IRoleMgrE>("RoleMgr.service"); } }
    //protected IWorkdayMgrE TheWorkdayMgr { get { return GetService<IWorkdayMgrE>("WorkdayMgr.service"); } }
    //protected IShiftMgrE TheShiftMgr { get { return GetService<IShiftMgrE>("ShiftMgr.service"); } }
    //protected ISpecialTimeMgrE TheSpecialTimeMgr { get { return GetService<ISpecialTimeMgrE>("SpecialTimeMgr.service"); } }
    //protected IWorkCalendarMgrE TheWorkCalendarMgr { get { return GetService<IWorkCalendarMgrE>("WorkCalendarMgr.service"); } }
    //protected IPartyMgrE ThePartyMgr { get { return GetService<IPartyMgrE>("PartyMgr.service"); } }
    //protected IBomMgrE TheBomMgr { get { return GetService<IBomMgrE>("BomMgr.service"); } }
    //protected IBomDetailMgrE TheBomDetailMgr { get { return GetService<IBomDetailMgrE>("BomDetailMgr.service"); } }
    //protected ILocationTransactionMgrE TheLocationTransactionMgr { get { return GetService<ILocationTransactionMgrE>("LocationTransactionMgr.service"); } }
    //protected IUserPermissionMgrE TheUserPermissionMgr { get { return GetService<IUserPermissionMgrE>("UserPermissionMgr.service"); } }
    //protected IItemKitMgrE TheItemKitMgr { get { return GetService<IItemKitMgrE>("ItemKitMgr.service"); } }
    //protected IUomMgrE TheUomMgr { get { return GetService<IUomMgrE>("UomMgr.service"); } }
    //protected IUomConversionMgrE TheUomConversionMgr { get { return GetService<IUomConversionMgrE>("UomConversionMgr.service"); } }
    //protected IRoutingMgrE TheRoutingMgr { get { return GetService<IRoutingMgrE>("RoutingMgr.service"); } }
    //protected IRoutingDetailMgrE TheRoutingDetailMgr { get { return GetService<IRoutingDetailMgrE>("RoutingDetailMgr.service"); } }
    //protected IRolePermissionMgrE TheRolePermissionMgr { get { return GetService<IRolePermissionMgrE>("RolePermissionMgr.service"); } }
    //protected IPermissionCategoryMgrE ThePermissionCategoryMgr { get { return GetService<IPermissionCategoryMgrE>("PermissionCategoryMgr.service"); } }
    //protected IAddressMgrE TheAddressMgr { get { return GetService<IAddressMgrE>("AddressMgr.service"); } }
    //protected IBillAddressMgrE TheBillAddressMgr { get { return GetService<IBillAddressMgrE>("BillAddressMgr.service"); } }
    //protected IShipAddressMgrE TheShipAddressMgr { get { return GetService<IShipAddressMgrE>("ShipAddressMgr.service"); } }
    //protected IFlowBindingMgrE TheFlowBindingMgr { get { return GetService<IFlowBindingMgrE>("FlowBindingMgr.service"); } }
    protected IOrderMgrE TheOrderMgr { get { return GetService<IOrderMgrE>("OrderMgr.service"); } }
    protected IOrderHeadMgrE TheOrderHeadMgr { get { return GetService<IOrderHeadMgrE>("OrderHeadMgr.service"); } }
    protected IOrderDetailMgrE TheOrderDetailMgr { get { return GetService<IOrderDetailMgrE>("OrderDetailMgr.service"); } }
    protected IOrderLocationTransactionMgrE TheOrderLocationTransactionMgr { get { return GetService<IOrderLocationTransactionMgrE>("OrderLocationTransactionMgr.service"); } }
    //protected IOrderOperationMgrE TheOrderOperationMgr { get { return GetService<IOrderOperationMgrE>("OrderOperationMgr.service"); } }
    //protected IOrderBindingMgrE TheOrderBindingMgr { get { return GetService<IOrderBindingMgrE>("OrderBindingMgr.service"); } }
    //protected INumberControlMgrE TheNumberControlMgr { get { return GetService<INumberControlMgrE>("NumberControlMgr.service"); } }
    //protected IWorkdayShiftMgrE TheWorkdayShiftMgr { get { return GetService<IWorkdayShiftMgrE>("WorkdayShiftMgr.service"); } }
    //protected ICurrencyMgrE TheCurrencyMgr { get { return GetService<ICurrencyMgrE>("CurrencyMgr.service"); } }
    //protected IPriceListMgrE ThePriceListMgr { get { return GetService<IPriceListMgrE>("PriceListMgr.service"); } }
    //protected ILocationLotDetailMgrE TheLocationLotDetailMgr { get { return GetService<ILocationLotDetailMgrE>("LocationLotDetailMgr.service"); } }
    //protected IReceiptMgrE TheReceiptMgr { get { return GetService<IReceiptMgrE>("ReceiptMgr.service"); } }
    //protected IReceiptDetailMgrE TheReceiptDetailMgr { get { return GetService<IReceiptDetailMgrE>("ReceiptDetailMgr.service"); } }
    //protected IReceiptInProcessLocationMgrE TheReceiptInProcessLocationMgr { get { return GetService<IReceiptInProcessLocationMgrE>("ReceiptInProcessLocationMgr.service"); } }
    //protected INamedQueryMgrE TheNamedQueryMgr { get { return GetService<INamedQueryMgrE>("NamedQueryMgr.service"); } }
    //protected IHuMgrE TheHuMgr { get { return GetService<IHuMgrE>("HuMgr.service"); } }
    //protected IHuOddMgrE TheHuOddMgr { get { return GetService<IHuOddMgrE>("HuOddMgr.service"); } }
    //protected IMiscOrderMgrE TheMiscOrderMgr { get { return GetService<IMiscOrderMgrE>("MiscOrderMgr.service"); } }
    //protected IMiscOrderDetailMgrE TheMiscOrderDetailMgr { get { return GetService<IMiscOrderDetailMgrE>("MiscOrderDetailMgr.service"); } }
    //protected ICycleCountMgrE TheCycleCountMgr { get { return GetService<ICycleCountMgrE>("CycleCountMgr.service"); } }
    //protected ICycleCountDetailMgrE TheCycleCountDetailMgr { get { return GetService<ICycleCountDetailMgrE>("CycleCountDetailMgr.service"); } }
    //protected IPlannedBillMgrE ThePlannedBillMgr { get { return GetService<IPlannedBillMgrE>("PlannedBillMgr.service"); } }
    //protected IBillTransactionMgrE TheBillTransactionMgr { get { return GetService<IBillTransactionMgrE>("BillTransactionMgr.service"); } }
    //protected IPurchasePriceListMgrE ThePurchasePriceListMgr { get { return GetService<IPurchasePriceListMgrE>("PurchasePriceListMgr.service"); } }
    //protected ILeanEngineMgrE TheLeanEngineMgr { get { return GetService<ILeanEngineMgrE>("LeanEngineMgr.service"); } }
    //protected ISupplyChainMgrE TheSupplyChainMgr { get { return GetService<ISupplyChainMgrE>("SupplyChainMgr.service"); } }
    //protected IAutoOrderTrackMgrE TheAutoOrderTrackMgr { get { return GetService<IAutoOrderTrackMgrE>("AutoOrderTrackMgr.service"); } }
    //protected IItemFlowPlanMgrE TheItemFlowPlanMgr { get { return GetService<IItemFlowPlanMgrE>("ItemFlowPlanMgr.service"); } }
    //protected IItemFlowPlanDetailMgrE TheItemFlowPlanDetailMgr { get { return GetService<IItemFlowPlanDetailMgrE>("ItemFlowPlanDetailMgr.service"); } }
    //protected IItemFlowPlanTrackMgrE TheItemFlowPlanTrackMgr { get { return GetService<IItemFlowPlanTrackMgrE>("ItemFlowPlanTrackMgr.service"); } }
    //protected IActingBillMgrE TheActingBillMgr { get { return GetService<IActingBillMgrE>("ActingBillMgr.service"); } }
    //protected IBillMgrE TheBillMgr { get { return GetService<IBillMgrE>("BillMgr.service"); } }
    //protected IBillDetailMgrE TheBillDetailMgr { get { return GetService<IBillDetailMgrE>("BillDetailMgr.service"); } }
    //protected IShiftPlanScheduleMgrE TheShiftPlanScheduleMgr { get { return GetService<IShiftPlanScheduleMgrE>("ShiftPlanScheduleMgr.service"); } }
    //protected ISalesPriceListMgrE TheSalesPriceListMgr { get { return GetService<ISalesPriceListMgrE>("SalesPriceListMgr.service"); } }
    protected IInProcessLocationMgrE TheInProcessLocationMgr { get { return GetService<IInProcessLocationMgrE>("InProcessLocationMgr.service"); } }
    //protected IInProcessLocationDetailMgrE TheInProcessLocationDetailMgr { get { return GetService<IInProcessLocationDetailMgrE>("InProcessLocationDetailMgr.service"); } }
    //protected IInProcessLocationTrackMgrE TheInProcessLocationTrackMgr { get { return GetService<IInProcessLocationTrackMgrE>("InProcessLocationTrackMgr.service"); } }
    //protected IMaterialFlushBackMgrE TheMaterialFlushBackMgr { get { return GetService<IMaterialFlushBackMgrE>("MaterialFlushBackMgr.service"); } }
    //protected IWorkingHoursMgrE TheWorkingHoursMgr { get { return GetService<IWorkingHoursMgrE>("WorkingHoursMgr.service"); } }
    //protected IClientMonitorMgrE TheClientMonitorMgr { get { return GetService<IClientMonitorMgrE>("ClientMonitorMgr.service"); } }
    //protected IClientLogMgrE TheClientLogMgr { get { return GetService<IClientLogMgrE>("ClientLogMgr.service"); } }
    //protected IClientOrderHeadMgrE TheClientOrderHeadMgr { get { return GetService<IClientOrderHeadMgrE>("ClientOrderHeadMgr.service"); } }
    //protected IClientOrderDetailMgrE TheClientOrderDetailMgr { get { return GetService<IClientOrderDetailMgrE>("ClientOrderDetailMgr.service"); } }
    //protected IClientWorkingHoursMgrE TheClientWorkingHoursMgr { get { return GetService<IClientWorkingHoursMgrE>("ClientWorkingHoursMgr.service"); } }
    //protected IClientMgrE TheClientMgr { get { return GetService<IClientMgrE>("ClientMgr.service"); } }
    //protected IPickListMgrE ThePickListMgr { get { return GetService<IPickListMgrE>("PickListMgr.service"); } }
    //protected IPickListDetailMgrE ThePickListDetailMgr { get { return GetService<IPickListDetailMgrE>("PickListDetailMgr.service"); } }
    //protected IPickListResultMgrE ThePickListResultMgr { get { return GetService<IPickListResultMgrE>("PickListResultMgr.service"); } }
    //protected IStorageBinMgrE TheStorageBinMgr { get { return GetService<IStorageBinMgrE>("StorageBinMgr.service"); } }
    //protected IStorageAreaMgrE TheStorageAreaMgr { get { return GetService<IStorageAreaMgrE>("StorageAreaMgr.service"); } }
    //protected IReportMgrE TheReportMgr { get { return GetService<IReportMgrE>("ReportMgr.service"); } }
    //protected IScanBarcodeMgrE TheScanBarcodeMgr { get { return GetService<IScanBarcodeMgrE>("ScanBarcodeMgr.service"); } }
    protected IMenuViewMgrE TheMenuViewMgr = ServiceLocator.GetService<IMenuViewMgrE>("MenuViewMgr.service");
    protected IGenericMgr TheGenericMgr { get { return GetService<IGenericMgr>("GenericMgr.service"); } }
    #endregion

    #region 私有
    protected string RenderingLanguage(string content, string userCode, params string[] parameters)
    {
        try
        {
            content = ProcessMessage(content, parameters);
            if (userCode != null && userCode.Trim() != string.Empty)
            {
                User user = TheUserMgr.LoadUser(userCode, true, false);

                if (user != null && user.UserLanguage != null && user.UserLanguage != string.Empty)
                {
                    content = TheLanguageMgr.ProcessLanguage(content, user.UserLanguage);
                }
                else
                {
                    EntityPreference defaultLanguage = TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_DEFAULT_LANGUAGE);
                    content = TheLanguageMgr.ProcessLanguage(content, defaultLanguage.Value);
                }
            }
            else
            {
                EntityPreference defaultLanguage = TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_DEFAULT_LANGUAGE);
                content = TheLanguageMgr.ProcessLanguage(content, defaultLanguage.Value);
            }
        }
        catch (Exception ex)
        {
            return content;
        }
        return content;
    }

    private string ProcessMessage(string message, string[] paramters)
    {
        string messageParams = string.Empty;
        if (paramters != null && paramters.Length > 0)
        {
            //处理Message参数
            foreach (string para in paramters)
            {
                messageParams += "," + para;
            }
        }
        message = "${" + message + messageParams + "}";

        return message;
    }
    #endregion
}

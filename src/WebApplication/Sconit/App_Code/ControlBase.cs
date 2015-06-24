using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.Sconit.Service.Ext.MasterData;
using System.Collections.Generic;
using com.Sconit.Entity.MasterData;
using com.Sconit.Utility;
using System.IO;
using System.Text;
using System.ServiceModel.Channels;
using com.Sconit.Entity;
using com.Sconit.Service.Ext.Procurement;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Service.Ext.Production;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.Batch;
using com.Sconit.Service.Ext.View;
using com.Sconit.Service.Ext.Business;
using com.Sconit.Service.Ext.Report;
using com.Sconit.Service.Ext.Cost;
using com.Sconit.Service.Ext.Hql;
using com.Sconit.Service.Ext.Customize;
using com.Sconit.Service.Ext.MRP;
using com.Sconit.Service.Ext;
using com.Sconit.Service;

/// <summary>
/// Summary description for ControlBase
/// </summary>
namespace com.Sconit.Web
{
    public abstract class ControlBase : System.Web.UI.UserControl
    {

        #region 变量
        protected User CurrentUser
        {
            get
            {
                return (new SessionHelper(Page)).CurrentUser;
            }
        }

        private IEntityPreferenceMgrE EntityPreferenceMgr
        {
            get
            {
                return GetService<IEntityPreferenceMgrE>("EntityPreferenceMgr.service");
            }
        }

        private ILanguageMgrE LanguageMgr
        {
            get
            {
                return GetService<ILanguageMgrE>("LanguageMgr.service");
            }
        }

        private IDictionary<string, System.Web.UI.Control> _findControlHelperCache = new Dictionary<string, System.Web.UI.Control>();
        #endregion

        #region 构造函数
        public ControlBase()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion

        #region 页面事件
        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter mywriter = new HtmlTextWriter(sw);
            base.Render(mywriter);
            string content = sb.ToString();
            if (CurrentUser != null && CurrentUser.UserLanguage != null && CurrentUser.UserLanguage != string.Empty)
            {
                content = LanguageMgr.ProcessLanguage(content, CurrentUser.UserLanguage);
            }
            else
            {
                EntityPreference defaultLanguage = EntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_DEFAULT_LANGUAGE);
                content = LanguageMgr.ProcessLanguage(content, defaultLanguage.Value);
            }
            writer.Write(content);
        }

        /*
         * 用于反射调用,参见GridView
         * 
         */
        public string Render(String content)
        {

            if (CurrentUser != null && CurrentUser.UserLanguage != null && CurrentUser.UserLanguage != string.Empty)
            {
                content = TheLanguageMgr.ProcessLanguage(content, CurrentUser.UserLanguage);
            }
            else
            {
                EntityPreference defaultLanguage = TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_DEFAULT_LANGUAGE);
                content = TheLanguageMgr.ProcessLanguage(content, defaultLanguage.Value);
            }
            return content;

        }


        public HttpResponse GetResponse()
        {
            return Response;
        }
        #endregion

        #region 方法
        protected T GetService<T>(string serviceName)
        {
            return ServiceLocator.GetService<T>(serviceName);
        }

        /// <summary>
        /// This helper automates locating a control by ID.
        /// 
        /// It calls FindControl on the NamingContainer, then the Page.  If that fails,
        /// it fires the resolve event.
        /// </summary>
        /// <param name="id">The ID of the control to find</param>
        /// <param name="props">The TargetProperties class associated with that control</param>
        /// <returns></returns>
        protected System.Web.UI.Control FindControlHelper(string id)
        {
            System.Web.UI.Control c = null;
            if (_findControlHelperCache.ContainsKey(id))
            {
                c = _findControlHelperCache[id];
            }
            else
            {
                c = base.FindControl(id);  // Use "base." to avoid calling self in an infinite loop
                System.Web.UI.Control nc = NamingContainer;
                while ((null == c) && (null != nc))
                {
                    c = nc.FindControl(id);
                    nc = nc.NamingContainer;
                }
                //if (null == c)
                //{
                //    // Note: props MAY be null, but we're firing the event anyway to let the user
                //    // do the best they can
                //    ResolveControlEventArgs args = new ResolveControlEventArgs(id);

                //    OnResolveControlID(args);
                //    c = args.Control;

                //}
                if (null != c)
                {
                    _findControlHelperCache[id] = c;
                }
            }
            return c;
        }

        public override System.Web.UI.Control FindControl(string id)
        {
            // Use FindControlHelper so that more complete searching and OnResolveControlID will be used
            return FindControlHelper(id);
        }
        #endregion

        #region Services
        protected ICriteriaMgrE TheCriteriaMgr { get { return GetService<ICriteriaMgrE>("CriteriaMgr.service"); } }
        protected IHqlMgrE TheHqlMgr { get { return GetService<IHqlMgrE>("HqlMgr.service"); } }
        protected IUserMgrE TheUserMgr { get { return GetService<IUserMgrE>("UserMgr.service"); } }
        protected IUserRoleMgrE TheUserRoleMgr { get { return GetService<IUserRoleMgrE>("UserRoleMgr.service"); } }
        protected IFileUploadMgrE TheFileUploadMgr { get { return GetService<IFileUploadMgrE>("FileUploadMgr.service"); } }
        protected IPermissionMgrE ThePermissionMgr { get { return GetService<IPermissionMgrE>("PermissionMgr.service"); } }
        protected ICodeMasterMgrE TheCodeMasterMgr { get { return GetService<ICodeMasterMgrE>("CodeMasterMgr.service"); } }
        protected IFavoritesMgrE TheFavoritesMgr { get { return GetService<IFavoritesMgrE>("FavoritesMgr.service"); } }
        protected IEntityPreferenceMgrE TheEntityPreferenceMgr { get { return GetService<IEntityPreferenceMgrE>("EntityPreferenceMgr.service"); } }
        protected IUserPreferenceMgrE TheUserPreferenceMgr { get { return GetService<IUserPreferenceMgrE>("UserPreferenceMgr.service"); } }
        protected ILanguageMgrE TheLanguageMgr { get { return GetService<ILanguageMgrE>("LanguageMgr.service"); } }
        protected IRegionMgrE TheRegionMgr { get { return GetService<IRegionMgrE>("RegionMgr.service"); } }
        protected ISupplierMgrE TheSupplierMgr { get { return GetService<ISupplierMgrE>("SupplierMgr.service"); } }
        protected ICustomerMgrE TheCustomerMgr { get { return GetService<ICustomerMgrE>("CustomerMgr.service"); } }
        protected IWorkCenterMgrE TheWorkCenterMgr { get { return GetService<IWorkCenterMgrE>("WorkCenterMgr.service"); } }
        protected IRoleMgrE TheRoleMgr { get { return GetService<IRoleMgrE>("RoleMgr.service"); } }
        protected IWorkdayMgrE TheWorkdayMgr { get { return GetService<IWorkdayMgrE>("WorkdayMgr.service"); } }
        protected IShiftMgrE TheShiftMgr { get { return GetService<IShiftMgrE>("ShiftMgr.service"); } }
        protected IShiftDetailMgrE TheShiftDetailMgr { get { return GetService<IShiftDetailMgrE>("ShiftDetailMgr.service"); } }
        protected ISpecialTimeMgrE TheSpecialTimeMgr { get { return GetService<ISpecialTimeMgrE>("SpecialTimeMgr.service"); } }
        protected IWorkCalendarMgrE TheWorkCalendarMgr { get { return GetService<IWorkCalendarMgrE>("WorkCalendarMgr.service"); } }
        protected IPartyMgrE ThePartyMgr { get { return GetService<IPartyMgrE>("PartyMgr.service"); } }        
        protected IBomMgrE TheBomMgr { get { return GetService<IBomMgrE>("BomMgr.service"); } }
        protected IBomDetailMgrE TheBomDetailMgr { get { return GetService<IBomDetailMgrE>("BomDetailMgr.service"); } }
        protected ILocationMgrE TheLocationMgr { get { return GetService<ILocationMgrE>("LocationMgr.service"); } }
        protected ILocationTransactionMgrE TheLocationTransactionMgr { get { return GetService<ILocationTransactionMgrE>("LocationTransactionMgr.service"); } }
        protected IUserPermissionMgrE TheUserPermissionMgr { get { return GetService<IUserPermissionMgrE>("UserPermissionMgr.service"); } }
        protected IItemMgrE TheItemMgr { get { return GetService<IItemMgrE>("ItemMgr.service"); } }
        protected IItemCategoryMgrE TheItemCategoryMgr { get { return GetService<IItemCategoryMgrE>("ItemCategoryMgr.service"); } }
        protected IItemDiscontinueMgrE TheItemDiscontinueMgr { get { return GetService<IItemDiscontinueMgrE>("ItemDiscontinueMgr.service"); } }
        protected IItemBrandMgrE TheItemBrandMgr { get { return GetService<IItemBrandMgrE>("ItemBrandMgr.service"); } }
        protected IItemReferenceMgrE TheItemReferenceMgr { get { return GetService<IItemReferenceMgrE>("ItemReferenceMgr.service"); } }
        protected IItemKitMgrE TheItemKitMgr { get { return GetService<IItemKitMgrE>("ItemKitMgr.service"); } }
        protected IUomMgrE TheUomMgr { get { return GetService<IUomMgrE>("UomMgr.service"); } }
        protected IUomConversionMgrE TheUomConversionMgr { get { return GetService<IUomConversionMgrE>("UomConversionMgr.service"); } }
        protected IRoutingMgrE TheRoutingMgr { get { return GetService<IRoutingMgrE>("RoutingMgr.service"); } }
        protected IRoutingDetailMgrE TheRoutingDetailMgr { get { return GetService<IRoutingDetailMgrE>("RoutingDetailMgr.service"); } }
        protected IRolePermissionMgrE TheRolePermissionMgr { get { return GetService<IRolePermissionMgrE>("RolePermissionMgr.service"); } }
        protected IPermissionCategoryMgrE ThePermissionCategoryMgr { get { return GetService<IPermissionCategoryMgrE>("PermissionCategoryMgr.service"); } }
        protected IAddressMgrE TheAddressMgr { get { return GetService<IAddressMgrE>("AddressMgr.service"); } }
        protected IBillAddressMgrE TheBillAddressMgr { get { return GetService<IBillAddressMgrE>("BillAddressMgr.service"); } }
        protected IShipAddressMgrE TheShipAddressMgr { get { return GetService<IShipAddressMgrE>("ShipAddressMgr.service"); } }
        protected IFlowMgrE TheFlowMgr { get { return GetService<IFlowMgrE>("FlowMgr.service"); } }
        protected IProductLineFacilityMgrE TheProductLineFacilityMgr { get { return GetService<IProductLineFacilityMgrE>("ProductLineFacilityMgr.service"); } }
        protected IProdutLineFeedSeqenceMgrE TheProdutLineFeedSeqenceMgr { get { return GetService<IProdutLineFeedSeqenceMgrE>("ProdutLineFeedSeqenceMgr.service"); } }
        protected IFlowDetailMgrE TheFlowDetailMgr { get { return GetService<IFlowDetailMgrE>("FlowDetailMgr.service"); } }
        protected IFlowBindingMgrE TheFlowBindingMgr { get { return GetService<IFlowBindingMgrE>("FlowBindingMgr.service"); } }
        protected IOrderMgrE TheOrderMgr { get { return GetService<IOrderMgrE>("OrderMgr.service"); } }
        protected IOrderTracerMgrE TheOrderTracerMgr { get { return GetService<IOrderTracerMgrE>("OrderTracerMgr.service"); } }
        protected IOrderHeadMgrE TheOrderHeadMgr { get { return GetService<IOrderHeadMgrE>("OrderHeadMgr.service"); } }
        protected IOrderDetailMgrE TheOrderDetailMgr { get { return GetService<IOrderDetailMgrE>("OrderDetailMgr.service"); } }
        protected IOrderLocationTransactionMgrE TheOrderLocationTransactionMgr { get { return GetService<IOrderLocationTransactionMgrE>("OrderLocationTransactionMgr.service"); } }
        protected IOrderOperationMgrE TheOrderOperationMgr { get { return GetService<IOrderOperationMgrE>("OrderOperationMgr.service"); } }
        protected IOrderBindingMgrE TheOrderBindingMgr { get { return GetService<IOrderBindingMgrE>("OrderBindingMgr.service"); } }
        protected IPriceListDetailMgrE ThePriceListDetailMgr { get { return GetService<IPriceListDetailMgrE>("PriceListDetailMgr.service"); } }
        protected INumberControlMgrE TheNumberControlMgr { get { return GetService<INumberControlMgrE>("NumberControlMgr.service"); } }
        protected IWorkdayShiftMgrE TheWorkdayShiftMgr { get { return GetService<IWorkdayShiftMgrE>("WorkdayShiftMgr.service"); } }
        protected ICurrencyMgrE TheCurrencyMgr { get { return GetService<ICurrencyMgrE>("CurrencyMgr.service"); } }
        protected IPriceListMgrE ThePriceListMgr { get { return GetService<IPriceListMgrE>("PriceListMgr.service"); } }
        protected ILocationDetailMgrE TheLocationDetailMgr { get { return GetService<ILocationDetailMgrE>("LocationDetailMgr.service"); } }
        protected ILocationLotDetailMgrE TheLocationLotDetailMgr { get { return GetService<ILocationLotDetailMgrE>("LocationLotDetailMgr.service"); } }
        protected IReceiptMgrE TheReceiptMgr { get { return GetService<IReceiptMgrE>("ReceiptMgr.service"); } }
        protected IReceiptDetailMgrE TheReceiptDetailMgr { get { return GetService<IReceiptDetailMgrE>("ReceiptDetailMgr.service"); } }
        protected IReceiptInProcessLocationMgrE TheReceiptInProcessLocationMgr { get { return GetService<IReceiptInProcessLocationMgrE>("ReceiptInProcessLocationMgr.service"); } }
        protected INamedQueryMgrE TheNamedQueryMgr { get { return GetService<INamedQueryMgrE>("NamedQueryMgr.service"); } }
        protected IHuMgrE TheHuMgr { get { return GetService<IHuMgrE>("HuMgr.service"); } }
        protected IHuOddMgrE TheHuOddMgr { get { return GetService<IHuOddMgrE>("HuOddMgr.service"); } }
        protected IMiscOrderMgrE TheMiscOrderMgr { get { return GetService<IMiscOrderMgrE>("MiscOrderMgr.service"); } }
        protected IMiscOrderDetailMgrE TheMiscOrderDetailMgr { get { return GetService<IMiscOrderDetailMgrE>("MiscOrderDetailMgr.service"); } }
        protected ICycleCountMgrE TheCycleCountMgr { get { return GetService<ICycleCountMgrE>("CycleCountMgr.service"); } }
        protected ICycleCountDetailMgrE TheCycleCountDetailMgr { get { return GetService<ICycleCountDetailMgrE>("CycleCountDetailMgr.service"); } }
        protected ICycleCountResultMgrE TheCycleCountResultMgr { get { return GetService<ICycleCountResultMgrE>("CycleCountResultMgr.service"); } }
        protected IPlannedBillMgrE ThePlannedBillMgr { get { return GetService<IPlannedBillMgrE>("PlannedBillMgr.service"); } }
        protected IBillTransactionMgrE TheBillTransactionMgr { get { return GetService<IBillTransactionMgrE>("BillTransactionMgr.service"); } }
        protected IPurchasePriceListMgrE ThePurchasePriceListMgr { get { return GetService<IPurchasePriceListMgrE>("PurchasePriceListMgr.service"); } }
        protected ILeanEngineMgrE TheLeanEngineMgr { get { return GetService<ILeanEngineMgrE>("LeanEngineMgr.service"); } }
        protected ISupplyChainMgrE TheSupplyChainMgr { get { return GetService<ISupplyChainMgrE>("SupplyChainMgr.service"); } }
        protected IAutoOrderTrackMgrE TheAutoOrderTrackMgr { get { return GetService<IAutoOrderTrackMgrE>("AutoOrderTrackMgr.service"); } }
        protected IItemFlowPlanMgrE TheItemFlowPlanMgr { get { return GetService<IItemFlowPlanMgrE>("ItemFlowPlanMgr.service"); } }
        protected IItemFlowPlanDetailMgrE TheItemFlowPlanDetailMgr { get { return GetService<IItemFlowPlanDetailMgrE>("ItemFlowPlanDetailMgr.service"); } }
        protected IItemFlowPlanTrackMgrE TheItemFlowPlanTrackMgr { get { return GetService<IItemFlowPlanTrackMgrE>("ItemFlowPlanTrackMgr.service"); } }
        protected IActingBillMgrE TheActingBillMgr { get { return GetService<IActingBillMgrE>("ActingBillMgr.service"); } }
        protected IBillMgrE TheBillMgr { get { return GetService<IBillMgrE>("BillMgr.service"); } }
        protected IBillDetailMgrE TheBillDetailMgr { get { return GetService<IBillDetailMgrE>("BillDetailMgr.service"); } }
        protected IShiftPlanScheduleMgrE TheShiftPlanScheduleMgr { get { return GetService<IShiftPlanScheduleMgrE>("ShiftPlanScheduleMgr.service"); } }
        protected ISalesPriceListMgrE TheSalesPriceListMgr { get { return GetService<ISalesPriceListMgrE>("SalesPriceListMgr.service"); } }
        protected IInProcessLocationMgrE TheInProcessLocationMgr { get { return GetService<IInProcessLocationMgrE>("InProcessLocationMgr.service"); } }
        protected IInProcessLocationDetailMgrE TheInProcessLocationDetailMgr { get { return GetService<IInProcessLocationDetailMgrE>("InProcessLocationDetailMgr.service"); } }
        protected IInProcessLocationTrackMgrE TheInProcessLocationTrackMgr { get { return GetService<IInProcessLocationTrackMgrE>("InProcessLocationTrackMgr.service"); } }
        protected IMaterialFlushBackMgrE TheMaterialFlushBackMgr { get { return GetService<IMaterialFlushBackMgrE>("MaterialFlushBackMgr.service"); } }
        protected IEmployeeMgrE TheEmployeeMgr { get { return GetService<IEmployeeMgrE>("EmployeeMgr.service"); } }
        protected IWorkingHoursMgrE TheWorkingHoursMgr { get { return GetService<IWorkingHoursMgrE>("WorkingHoursMgr.service"); } }
        protected IClientMonitorMgrE TheClientMonitorMgr { get { return GetService<IClientMonitorMgrE>("ClientMonitorMgr.service"); } }
        protected IClientLogMgrE TheClientLogMgr { get { return GetService<IClientLogMgrE>("ClientLogMgr.service"); } }
        protected IClientOrderHeadMgrE TheClientOrderHeadMgr { get { return GetService<IClientOrderHeadMgrE>("ClientOrderHeadMgr.service"); } }
        protected IClientOrderDetailMgrE TheClientOrderDetailMgr { get { return GetService<IClientOrderDetailMgrE>("ClientOrderDetailMgr.service"); } }
        protected IClientWorkingHoursMgrE TheClientWorkingHoursMgr { get { return GetService<IClientWorkingHoursMgrE>("ClientWorkingHoursMgr.service"); } }
        protected IClientMgrE TheClientMgr { get { return GetService<IClientMgrE>("ClientMgr.service"); } }
        protected IPickListMgrE ThePickListMgr { get { return GetService<IPickListMgrE>("PickListMgr.service"); } }
        protected IPickListDetailMgrE ThePickListDetailMgr { get { return GetService<IPickListDetailMgrE>("PickListDetailMgr.service"); } }
        protected IPickListResultMgrE ThePickListResultMgr { get { return GetService<IPickListResultMgrE>("PickListResultMgr.service"); } }
        protected IStorageBinMgrE TheStorageBinMgr { get { return GetService<IStorageBinMgrE>("StorageBinMgr.service"); } }
        protected IStorageAreaMgrE TheStorageAreaMgr { get { return GetService<IStorageAreaMgrE>("StorageAreaMgr.service"); } }
        protected IReportMgrE TheReportMgr { get { return GetService<IReportMgrE>("ReportMgr.service"); } }
        protected IImportMgrE TheImportMgr { get { return GetService<IImportMgrE>("ImportMgr.service"); } }
        protected IRepackMgrE TheRepackMgr { get { return GetService<IRepackMgrE>("RepackMgr.service"); } }
        protected IRepackDetailMgrE TheRepackDetailMgr { get { return GetService<IRepackDetailMgrE>("RepackDetailMgr.service"); } }
        protected IScanBarcodeMgrE TheScanBarcodeMgr { get { return GetService<IScanBarcodeMgrE>("ScanBarcodeMgr.service"); } }
        protected IBatchTriggerMgrE TheBatchTriggerMgr { get { return GetService<IBatchTriggerMgrE>("BatchTriggerMgr.service"); } }
        protected IInspectOrderMgrE TheInspectOrderMgr { get { return GetService<IInspectOrderMgrE>("InspectOrderMgr.service"); } }
        protected IInspectOrderDetailMgrE TheInspectOrderDetailMgr { get { return GetService<IInspectOrderDetailMgrE>("InspectOrderDetailMgr.service"); } }
        protected IProductLineInProcessLocationDetailMgrE TheProductLineInProcessLocationDetailMgr { get { return GetService<IProductLineInProcessLocationDetailMgrE>("ProductLineInProcessLocationDetailMgr.service"); } }
        protected IOrderDetailViewMgrE TheOrderDetailViewMgr { get { return GetService<IOrderDetailViewMgrE>("OrderDetailViewMgr.service"); } }
        protected IOrderLocTransViewMgrE TheOrderLocTransViewMgr { get { return GetService<IOrderLocTransViewMgrE>("OrderLocTransViewMgr.service"); } }
        protected IPlannedBillViewMgrE ThePlannedBillViewMgr { get { return GetService<IPlannedBillViewMgrE>("PlannedBillViewMgr.service"); } }
        protected IBillAgingViewMgrE TheBillAgingViewMgr { get { return GetService<IBillAgingViewMgrE>("BillAgingViewMgr.service"); } }
        protected IResolverMgrE TheResolverMgr { get { return GetService<IResolverMgrE>("ResolverMgr.service"); } }
        protected ISubjectListMgrE TheSubjectListMgr { get { return GetService<ISubjectListMgrE>("SubjectListMgr.service"); } }
        protected IKPOrderMgrE TheKPOrderMgr { get { return GetService<IKPOrderMgrE>("KPOrderMgr.service"); } }
        protected IKPItemMgrE TheKPItemMgr { get { return GetService<IKPItemMgrE>("KPItemMgr.service"); } }
        protected ITaxRateMgrE TheTaxRateMgr { get { return GetService<ITaxRateMgrE>("TaxRateMgr.service"); } }
        protected ICurrencyExchangeMgrE TheCurrencyExchangeMgr { get { return GetService<ICurrencyExchangeMgrE>("CurrencyExchangeMgr.service"); } }
        protected ICostCenterMgrE TheCostCenterMgr { get { return ServiceLocator.GetService<ICostCenterMgrE>("CostCenterMgr.service"); } }
        protected ICostGroupMgrE TheCostGroupMgr { get { return ServiceLocator.GetService<ICostGroupMgrE>("CostGroupMgr.service"); } }
        protected ICostElementMgrE TheCostElementMgr { get { return ServiceLocator.GetService<ICostElementMgrE>("CostElementMgr.service"); } }
        protected IStandardCostMgrE TheStandardCostMgr { get { return ServiceLocator.GetService<IStandardCostMgrE>("StandardCostMgr.service"); } }
        protected IFinanceCalendarMgrE TheFinanceCalendarMgr { get { return ServiceLocator.GetService<IFinanceCalendarMgrE>("FinanceCalendarMgr.service"); } }
        protected ICostMgrE TheCostMgr { get { return ServiceLocator.GetService<ICostMgrE>("CostMgr.service"); } }
        protected ICostTransactionMgrE TheCostTransactionMgr { get { return ServiceLocator.GetService<ICostTransactionMgrE>("CostTransactionMgr.service"); } }
        protected IExpenseElementMgrE TheExpenseElementMgr { get { return ServiceLocator.GetService<IExpenseElementMgrE>("ExpenseElementMgr.service"); } }
        protected IInventoryBalanceMgrE TheInventoryBalanceMgr { get { return ServiceLocator.GetService<IInventoryBalanceMgrE>("InventoryBalanceMgr.service"); } }
        protected ICostAllocateTransactionMgrE TheCostAllocateTransactionMgr { get { return ServiceLocator.GetService<ICostAllocateTransactionMgrE>("CostAllocateTransactionMgr.service"); } }
        protected ICostAllocateMethodMgrE TheCostAllocateMethodMgr { get { return ServiceLocator.GetService<ICostAllocateMethodMgrE>("CostAllocateMethodMgr.service"); } }
        protected ILedSortLevelMgrE TheLedSortLevelMgr { get { return ServiceLocator.GetService<ILedSortLevelMgrE>("LedSortLevelMgr.service"); } }
        protected ILedColorLevelMgrE TheLedColorLevelMgr { get { return ServiceLocator.GetService<ILedColorLevelMgrE>("LedColorLevelMgr.service"); } }
        protected IItemTypeMgrE TheItemTypeMgr { get { return ServiceLocator.GetService<IItemTypeMgrE>("ItemTypeMgr.service"); } }
        
        #region MRP
        protected ICustomerScheduleDetailMgrE TheCustomerScheduleDetailMgr { get { return ServiceLocator.GetService<ICustomerScheduleDetailMgrE>("CustomerScheduleDetailMgr.service"); } }
        protected ICustomerScheduleMgrE TheCustomerScheduleMgr { get { return ServiceLocator.GetService<ICustomerScheduleMgrE>("CustomerScheduleMgr.service"); } }
        protected IMrpMgrE TheMrpMgr { get { return ServiceLocator.GetService<IMrpMgrE>("MrpMgr.service"); } }
        protected IMrpShipPlanMgrE TheMrpShipPlanMgr { get { return ServiceLocator.GetService<IMrpShipPlanMgrE>("MrpShipPlanMgr.service"); } }
        protected IMrpShipPlanViewMgrE TheMrpShipPlanViewMgr { get { return ServiceLocator.GetService<IMrpShipPlanViewMgrE>("MrpShipPlanViewMgr.service"); } }
        #endregion


        protected ISqlHelperMgrE TheSqlHelperMgr { get { return GetService<ISqlHelperMgrE>("SqlHelperMgr.service"); } }
        protected ISqlReportMgrE TheSqlReportMgr { get { return GetService<ISqlReportMgrE>("SqlReportMgr.service"); } }

        protected IBalanceMgrE TheBalanceMgr { get { return GetService<IBalanceMgrE>("BalanceMgr.service"); } }
        protected IRawIOBMgrE TheRawIOBMgr { get { return GetService<IRawIOBMgrE>("RawIOBMgr.service"); } }

        protected IGenericMgr TheGenericMgr { get { return GetService<IGenericMgr>("GenericMgr.service"); } }

        protected IOrderProductionPlanMgrE TheOrderProductionPlanMgr { get { return ServiceLocator.GetService<IOrderProductionPlanMgrE>("OrderProductionPlanMgr.service"); } }

        #endregion
    }
}

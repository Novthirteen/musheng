using System;
using System.IO;
using System.Web.UI;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Service.Ext.Batch;
using com.Sconit.Service.Ext.Business;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.Distribution;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Service.Ext.Procurement;
using com.Sconit.Service.Ext.Production;
using com.Sconit.Service.Ext.Report;
using com.Sconit.Service.Ext.View;
using com.Sconit.Service.Ext.Cost;
using com.Sconit.Utility;
using System.Text;
using com.Sconit.Service.Ext.Quote;


/// <summary>
/// Summary description for PageBase
/// </summary>

namespace com.Sconit.Web
{
    public class PageBase : System.Web.UI.Page
    {
        public string Permission { get; set; }

        public PageBase()
        {
            this.Page.Culture = "auto";
            this.Page.UICulture = "auto";
        }

        #region override
        protected override void OnPreInit(EventArgs e)
        {

            if (Request.Cookies["ThemePage"] == null)
            {
                this.Page.Theme = "Default";
            }
            else
            {
                this.Page.Theme = Request.Cookies["ThemePage"].Value;
            }
            base.OnPreInit(e);
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            #region 权限认证
            if (this.Permission == BusinessConstants.PERMISSION_NOTNEED_CHECK_PERMISSION || this.CurrentUser.Code.ToLower() == "su")
            {
                base.OnLoadComplete(e);
            }
            else
            {
                if (!this.CurrentUser.HasPermission(this.Permission))
                {
                    Response.Redirect("~/NoPermission.aspx");
                }
            }
            base.OnLoadComplete(e);
            #endregion
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Permission != BusinessConstants.PERMISSION_NOTNEED_CHECK_PERMISSION)
            {
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                HtmlTextWriter mywriter = new HtmlTextWriter(sw);
                base.Render(mywriter);
                string content = sb.ToString();
                content = TheLanguageMgr.ProcessLanguage(content, this.CurrentUser.UserLanguage);
                writer.Write(content);
                if (this.Permission != null)
                {
                    this.Title = TheLanguageMgr.ProcessLanguage(this.Permission, this.CurrentUser.UserLanguage);
                }
            }
            else
            {
                base.Render(writer);
            }
        }

        protected override void SavePageStateToPersistenceMedium(object state)
        {
            Pair pair;
            PageStatePersister persister = this.PageStatePersister;
            object viewState;
            if (state is Pair)
            {
                pair = (Pair)state;
                persister.ControlState = pair.First;
                viewState = pair.Second;
            }
            else
            {
                viewState = state;
            }

            LosFormatter formatter = new LosFormatter();
            StringWriter writer = new StringWriter();
            formatter.Serialize(writer, viewState);
            string viewStateStr = writer.ToString();
            byte[] data = Convert.FromBase64String(viewStateStr);
            byte[] compressedData = ViewStateHelper.Compress(data);
            string str = Convert.ToBase64String(compressedData);

            persister.ViewState = str;
            persister.Save();
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            PageStatePersister persister = this.PageStatePersister;
            persister.Load();

            string viewState = persister.ViewState.ToString();
            byte[] data = Convert.FromBase64String(viewState);
            byte[] uncompressedData = ViewStateHelper.Decompress(data);
            string str = Convert.ToBase64String(uncompressedData);
            LosFormatter formatter = new LosFormatter();
            return new Pair(persister.ControlState, formatter.Deserialize(str));
        }

        #endregion


        protected User CurrentUser
        {
            get
            {
                User user = (new SessionHelper(this.Page)).CurrentUser;
                if (user == null || user.UserLanguage == null || user.UserLanguage == string.Empty)
                {
                    user.UserLanguage = TheEntityPreferenceMgr.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_DEFAULT_LANGUAGE).Value;
                    UserPreference usrpf = new UserPreference();
                    usrpf.User = user;
                    usrpf.Code = BusinessConstants.ENTITY_PREFERENCE_CODE_DEFAULT_LANGUAGE;
                    usrpf.Value = user.UserLanguage;
                    TheUserPreferenceMgr.CreateUserPreference(usrpf);
                }
                return user;
            }
        }

        #region Services
        protected T GetService<T>(string serviceName) { return ServiceLocator.GetService<T>(serviceName); }
        protected ICriteriaMgrE TheCriteriaMgr { get { return GetService<ICriteriaMgrE>("CriteriaMgr.service"); } }
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
        protected IFlowDetailMgrE TheFlowDetailMgr { get { return GetService<IFlowDetailMgrE>("FlowDetailMgr.service"); } }
        protected IFlowBindingMgrE TheFlowBindingMgr { get { return GetService<IFlowBindingMgrE>("FlowBindingMgr.service"); } }
        protected IOrderMgrE TheOrderMgr { get { return GetService<IOrderMgrE>("OrderMgr.service"); } }
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
        protected IMenuViewMgrE TheMenuViewMgr { get { return ServiceLocator.GetService<IMenuViewMgrE>("MenuViewMgr.service"); } }
        protected ICostCenterMgrE TheCostCenterMgr { get { return ServiceLocator.GetService<ICostCenterMgrE>("CostCenterMgr.service"); } }
        protected ICostGroupMgrE TheCostGroupMgr { get { return ServiceLocator.GetService<ICostGroupMgrE>("CostGroupMgr.service"); } }
        protected ICostElementMgrE TheCostElementMgr { get { return ServiceLocator.GetService<ICostElementMgrE>("CostElementMgr.service"); } }
        protected IStandardCostMgrE TheStandardCostMgr { get { return ServiceLocator.GetService<IStandardCostMgrE>("StandardCostMgr.service"); } }
        protected IFinanceCalendarMgrE TheFinanceCalendarMgr { get { return ServiceLocator.GetService<IFinanceCalendarMgrE>("FinanceCalendarMgr.service"); } }
        protected IToolingMgrE TheToolingMgr { get { return GetService<IToolingMgrE>("ToolingMgr.service"); } }
        #endregion
    }
}

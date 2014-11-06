using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity.View;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.Customize;
using com.Sconit.Service.Ext.MasterData;
using com.Sconit.Utility;
using NHibernate.Expression;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class FlowMgr : FlowBaseMgr, IFlowMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IFlowDetailMgrE flowDetailMgrE { get; set; }
        public IBomMgrE bomMgrE { get; set; }
        public IBomDetailMgrE bomDetailMgrE { get; set; }
        public IProductLineFacilityMgrE productLineFacilityMgrE { get; set; }
        public IUserMgrE userMgr { get; set; }

        [Transaction(TransactionMode.Requires)]
        public override void DeleteFlow(string flowCode)
        {
            Flow flow = this.LoadFlow(flowCode, true, false, true);
            productLineFacilityMgrE.DeleteProductLineFacility(flow.Facilitys);
            flowDetailMgrE.DeleteFlowDetail(flow.FlowDetails);
            base.DeleteFlow(flowCode);
        }

        #region Customized Methods

        [Transaction(TransactionMode.Unspecified)]
        public Flow LoadFlow(string code, bool includeFlowDetail)
        {
            return LoadFlow(code, includeFlowDetail, false, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public Flow LoadFlow(string code, bool includeFlowDetail, bool includeRefDetail)
        {
            return LoadFlow(code, includeFlowDetail, includeRefDetail, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public Flow LoadFlow(string code, bool includeFlowDetail, bool includeRefDetail, bool includeFacility)
        {
            Flow flow = this.LoadFlow(code);
            if (includeFlowDetail && flow != null && flow.FlowDetails != null && flow.FlowDetails.Count > 0)
            {
            }

            if (includeRefDetail && flow != null && flow.ReferenceFlow != null && flow.ReferenceFlow.Trim() != string.Empty)
            {
                flow.FlowDetails = flowDetailMgrE.GetFlowDetail(flow.ReferenceFlow, includeRefDetail);
            }

            if (includeFacility && flow != null && flow.Facilitys != null && flow.Facilitys.Count > 0)
            {
            }
            if (flow != null)
            {
                flow.FlowDetails = flow.FlowDetails.OrderBy(f => f.Sequence).ThenBy(f => f.Item.Code).ToList();
            }
            return flow;
        }

        [Transaction(TransactionMode.Unspecified)]
        public Flow CheckAndLoadFlow(string code)
        {
            return CheckAndLoadFlow(code, false, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public Flow CheckAndLoadFlow(string code, bool includeFlowDetail)
        {
            return CheckAndLoadFlow(code, includeFlowDetail, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public Flow CheckAndLoadFlow(string code, bool includeFlowDetail, bool includeRefDetail)
        {
            Flow flow = this.LoadFlow(code, true);

            if (flow == null)
            {
                throw new BusinessErrorException("Flow.Error.FlowCodeNotExist", code);
            }

            if (includeFlowDetail && flow != null && flow.FlowDetails != null && flow.FlowDetails.Count > 0)
            {
            }

            if (includeRefDetail && flow != null && flow.ReferenceFlow != null && flow.ReferenceFlow.Trim() != string.Empty)
            {
                flow.FlowDetails = flowDetailMgrE.GetFlowDetail(flow.ReferenceFlow, includeRefDetail);
            }

            return flow;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Flow> GetProcurementFlow(string userCode)
        {
            return GetFlow(BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT, userCode, BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT, BusinessConstants.PARTY_AUTHRIZE_OPTION_TO, null);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Flow> GetProcurementFlow(string userCode, string partyAuthrizeOpt)
        {
            return GetFlow(BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT, userCode, BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT, partyAuthrizeOpt, null);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Flow> GetProcurementFlowByItem(string userCode, string itemCode)
        {
            return GetFlow(BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT, userCode, BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT, BusinessConstants.PARTY_AUTHRIZE_OPTION_TO, itemCode);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Flow> GetDistributionFlow(string userCode)
        {
            return GetFlow(BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_DISTRIBUTION, userCode, BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION, BusinessConstants.PARTY_AUTHRIZE_OPTION_FROM, null);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Flow> GetProductionFlow(string userCode)
        {
            return GetFlow(BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION, userCode, BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION, BusinessConstants.PARTY_AUTHRIZE_OPTION_BOTH, null);
        }


        [Transaction(TransactionMode.Unspecified)]
        public IList<Flow> GetTransferFlow(string userCode, string partyAuthrizeOpt)
        {
            return GetFlow(BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_TRANSFER, userCode, BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_TRANSFER, partyAuthrizeOpt, null);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Flow> GetTransferFlow(string userCode)
        {
            return GetFlow(BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_TRANSFER, userCode, BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_TRANSFER, BusinessConstants.PARTY_AUTHRIZE_OPTION_BOTH, null);
        }


        [Transaction(TransactionMode.Unspecified)]
        public IList<Flow> GetCustomerGoodsFlow(string userCode)
        {
            return GetFlow(BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS, userCode, BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_CUSTOMERGOODS, BusinessConstants.PARTY_AUTHRIZE_OPTION_TO, null);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Flow> GetCustomerGoodsFlow(string userCode, string partyAuthrizeOpt)
        {
            return GetFlow(BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS, userCode, BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_CUSTOMERGOODS, partyAuthrizeOpt, null);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Flow> GetSubconctractingFlow(string userCode)
        {
            return GetFlow(BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING, userCode, BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_SUBCONCTRACTING, BusinessConstants.PARTY_AUTHRIZE_OPTION_TO, null);
        }


        public IList<Flow> GetFlow(string flowType, string userCode, string specifiedType, string partyAuthrizeOpt, string itemCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For<Flow>();
            criteria.CreateAlias("PartyTo", "pt");
            criteria.CreateAlias("PartyFrom", "pf");
            criteria.Add(Expression.Eq("Type", flowType));

            if (itemCode != null)
            {
                criteria.CreateAlias("FlowDetails", "fds");
                criteria.CreateAlias("fds.Item", "fdsi");
                criteria.Add(Expression.Eq("fdsi.Code", itemCode));
            }

            if (specifiedType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PROCUREMENT)
            {
                //供货路线
                if (partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_FROM
                   || partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_BOTH)
                {
                    DetachedCriteria[] pfCrieteria = SecurityHelper.GetSupplierPermissionCriteria(userCode);

                    criteria.Add(
                          Expression.Or(
                              Subqueries.PropertyIn("pf.Code", pfCrieteria[0]),
                              Subqueries.PropertyIn("pf.Code", pfCrieteria[1])
                    ));
                }

                if (partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_TO
                   || partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_BOTH)
                {
                    DetachedCriteria[] ptCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);

                    criteria.Add(
                           Expression.Or(
                               Subqueries.PropertyIn("pt.Code", ptCrieteria[0]),
                               Subqueries.PropertyIn("pt.Code", ptCrieteria[1])
                    ));
                }
            }
            else if (specifiedType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_DISTRIBUTION)
            {
                //发货路线
                if (partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_FROM
                   || partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_BOTH)
                {
                    DetachedCriteria[] pfCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);

                    criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("pf.Code", pfCrieteria[0]),
                        Subqueries.PropertyIn("pf.Code", pfCrieteria[1])
                    ));
                }

                if (partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_TO
                   || partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_BOTH)
                {
                    DetachedCriteria[] ptCrieteria = SecurityHelper.GetCustomerPermissionCriteria(userCode);

                    criteria.Add(
                       Expression.Or(
                           Subqueries.PropertyIn("pt.Code", ptCrieteria[0]),
                           Subqueries.PropertyIn("pt.Code", ptCrieteria[1])
                   ));
                }
            }
            else if (specifiedType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION)
            {
                //生产
                DetachedCriteria[] regionCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);

                if (partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_FROM
                   || partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_BOTH)
                {
                    criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("pf.Code", regionCrieteria[0]),
                        Subqueries.PropertyIn("pf.Code", regionCrieteria[1])
                    ));
                }

                if (partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_TO
                   || partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_BOTH)
                {
                    criteria.Add(
                       Expression.Or(
                           Subqueries.PropertyIn("pt.Code", regionCrieteria[0]),
                           Subqueries.PropertyIn("pt.Code", regionCrieteria[1])
                   ));
                }
            }
            else if (specifiedType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_TRANSFER)
            {
                //移库路线
                DetachedCriteria[] rpCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);

                if (partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_FROM
                   || partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_BOTH)
                {
                    criteria.Add(
                        Expression.Or(
                            Subqueries.PropertyIn("pf.Code", rpCrieteria[0]),
                            Subqueries.PropertyIn("pf.Code", rpCrieteria[1])
                    ));
                }

                if (partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_TO
                   || partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_BOTH)
                {
                    criteria.Add(
                        Expression.Or(
                            Subqueries.PropertyIn("pt.Code", rpCrieteria[0]),
                            Subqueries.PropertyIn("pt.Code", rpCrieteria[1])
                    ));
                }
            }
            else if (specifiedType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_CUSTOMERGOODS)
            {
                //客供品路线
                if (partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_FROM
                   || partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_BOTH)
                {
                    DetachedCriteria[] pfCrieteria = SecurityHelper.GetCustomerPermissionCriteria(userCode);

                    criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("pf.Code", pfCrieteria[0]),
                        Subqueries.PropertyIn("pf.Code", pfCrieteria[1])
                    ));
                }

                if (partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_TO
                   || partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_BOTH)
                {
                    DetachedCriteria[] ptCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);

                    criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("pt.Code", ptCrieteria[0]),
                        Subqueries.PropertyIn("pt.Code", ptCrieteria[1])
                    ));
                }
            }
            else if (specifiedType == BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_SUBCONCTRACTING)
            {
                //委外加工路线
                if (partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_FROM
                   || partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_BOTH)
                {
                    DetachedCriteria[] pfCrieteria = SecurityHelper.GetSupplierPermissionCriteria(userCode);

                    criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("pf.Code", pfCrieteria[0]),
                        Subqueries.PropertyIn("pf.Code", pfCrieteria[1])
                    ));
                }

                if (partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_TO
                   || partyAuthrizeOpt == BusinessConstants.PARTY_AUTHRIZE_OPTION_BOTH)
                {
                    DetachedCriteria[] ptCrieteria = SecurityHelper.GetRegionPermissionCriteria(userCode);

                    criteria.Add(
                    Expression.Or(
                        Subqueries.PropertyIn("pt.Code", ptCrieteria[0]),
                        Subqueries.PropertyIn("pt.Code", ptCrieteria[1])
                    ));
                }
            }

            return criteriaMgrE.FindAll<Flow>(criteria).Distinct().ToList();
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Flow> GetAllFlow(string userCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For<Flow>();
            criteria.CreateAlias("PartyTo", "pt");
            criteria.CreateAlias("PartyFrom", "pf");

            criteria.Add(Expression.Eq("IsActive", true));

            DetachedCriteria[] pfCrieteria = SecurityHelper.GetPartyPermissionCriteria(userCode,
                BusinessConstants.CODE_MASTER_PARTY_TYPE_VALUE_REGION, BusinessConstants.CODE_MASTER_PARTY_TYPE_VALUE_SUPPLIER);

            DetachedCriteria[] ptCrieteria = SecurityHelper.GetPartyPermissionCriteria(userCode,
                BusinessConstants.CODE_MASTER_PARTY_TYPE_VALUE_REGION, BusinessConstants.CODE_MASTER_PARTY_TYPE_VALUE_CUSTOMER);

            criteria.Add(
                Expression.Or(
                    Subqueries.PropertyIn("pf.Code", pfCrieteria[0]),
                    Subqueries.PropertyIn("pf.Code", pfCrieteria[1])
            ));

            criteria.Add(
                Expression.Or(
                    Subqueries.PropertyIn("pt.Code", ptCrieteria[0]),
                    Subqueries.PropertyIn("pt.Code", ptCrieteria[1])
            ));

            return criteriaMgrE.FindAll<Flow>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<string> FindWinTime(Flow flow, DateTime date)
        {
            if (date != null)
            {
                switch (date.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        return ParseWinTime(flow.WinTime1);

                    case DayOfWeek.Tuesday:
                        return ParseWinTime(flow.WinTime2);

                    case DayOfWeek.Wednesday:
                        return ParseWinTime(flow.WinTime3);

                    case DayOfWeek.Thursday:
                        return ParseWinTime(flow.WinTime4);

                    case DayOfWeek.Friday:
                        return ParseWinTime(flow.WinTime5);

                    case DayOfWeek.Saturday:
                        return ParseWinTime(flow.WinTime6);

                    case DayOfWeek.Sunday:
                        return ParseWinTime(flow.WinTime7);
                }
            }

            return null;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<string> FindWinTime(string flowCode, DateTime date)
        {
            Flow flow = entityDao.LoadFlow(flowCode);
            if (flow == null)
            {
                return null;
            }
            return FindWinTime(flow, date);
        }

        [Transaction(TransactionMode.Unspecified)]
        public Flow LoadFlow(string flowCode, string userCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For<Flow>();
            criteria.Add(Expression.Eq("Code", flowCode));
            criteria.CreateAlias("PartyTo", "pt");
            criteria.CreateAlias("PartyFrom", "pf");

            DetachedCriteria[] pfCrieteria = SecurityHelper.GetPartyPermissionCriteria(userCode,
                BusinessConstants.CODE_MASTER_PARTY_TYPE_VALUE_REGION, BusinessConstants.CODE_MASTER_PARTY_TYPE_VALUE_SUPPLIER);

            DetachedCriteria[] ptCrieteria = SecurityHelper.GetPartyPermissionCriteria(userCode,
                BusinessConstants.CODE_MASTER_PARTY_TYPE_VALUE_REGION, BusinessConstants.CODE_MASTER_PARTY_TYPE_VALUE_CUSTOMER);

            criteria.Add(
                Expression.Or(
                    Subqueries.PropertyIn("pf.Code", pfCrieteria[0]),
                    Subqueries.PropertyIn("pf.Code", pfCrieteria[1])
            ));

            criteria.Add(
                Expression.Or(
                    Subqueries.PropertyIn("pt.Code", ptCrieteria[0]),
                    Subqueries.PropertyIn("pt.Code", ptCrieteria[1])
            ));

            IList<Flow> listFlow = criteriaMgrE.FindAll<Flow>(criteria);
            if (listFlow.Count > 0)
            {
                return listFlow[0];
            }
            return null;
        }

        public IList<string> ParseWinTime(string winTime)
        {
            if (winTime == null || winTime.Trim() == string.Empty)
            {
                return null;
            }

            IList<string> result = new List<string>();
            string[] speratedWinTime = winTime.Split('|');

            foreach (string wt in speratedWinTime)
            {
                result.Add(wt.Trim());
            }

            return result;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Flow> GetAllFlow(DateTime lastModifyDate, int firstRow, int maxRows)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Flow));
            criteria.Add(Expression.Gt("LastModifyDate", lastModifyDate));
            criteria.AddOrder(Order.Asc("LastModifyDate"));

            IList<Flow> flowList = criteriaMgrE.FindAll<Flow>(criteria, firstRow, maxRows);
            if (flowList.Count > 0)
            {
                return flowList;
            }
            return null;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Flow> GetFlowList(string userCode, bool includeProcurement, bool includeDistribution, bool includeTransfer, bool includeProduction, bool includeCustomerGoods, bool includeSubconctracting, string partyAuthrizeOpt)
        {
            List<Flow> flowList = new List<Flow>();
            if (includeProcurement)
            {
                IList<Flow> procurementList = GetProcurementFlow(userCode, partyAuthrizeOpt);
                flowList.AddRange(procurementList);
            }
            if (includeDistribution)
            {
                IList<Flow> distributionList = GetDistributionFlow(userCode);
                flowList.AddRange(distributionList);
            }
            if (includeTransfer)
            {
                IList<Flow> transferList = GetTransferFlow(userCode, partyAuthrizeOpt);
                flowList.AddRange(transferList);
            }
            if (includeProduction)
            {
                IList<Flow> productionList = GetProductionFlow(userCode);
                flowList.AddRange(productionList);
            }
            if (includeCustomerGoods)
            {
                IList<Flow> customerGoodsList = GetCustomerGoodsFlow(userCode, partyAuthrizeOpt);
                flowList.AddRange(customerGoodsList);
            }
            if (includeSubconctracting)
            {
                IList<Flow> subconctractingGoodsList = GetSubconctractingFlow(userCode);
                flowList.AddRange(subconctractingGoodsList);
            }
            flowList = flowList.Where(f => f.IsActive == true).ToList();
            return flowList;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<BomDetail> GetBatchFeedBomDetail(Flow flow)
        {
            return GetBatchFeedBomDetail(flow.Code);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<BomDetail> GetBatchFeedBomDetail(string flowCode)
        {
            Flow flow = this.LoadFlow(flowCode, true);

            if (flow != null && flow.FlowDetails != null && flow.FlowDetails.Count > 0)
            {
                IList<BomDetail> batchFeedBomDetailList = new List<BomDetail>();
                foreach (FlowDetail flowDetail in flow.FlowDetails)
                {
                    //先获取flowdetail上的bom，如果为null再以flowdetail的Item对象去找
                    string bomCode = flowDetail.Bom != null ? flowDetail.Bom.Code : this.bomMgrE.FindBomCode(flowDetail.Item);
                    IList<BomDetail> bomDetailList = this.bomDetailMgrE.GetFlatBomDetail(bomCode, DateTime.Now);

                    if (bomDetailList != null && bomDetailList.Count > 0)
                    {
                        foreach (BomDetail bomDetail in bomDetailList)
                        {
                            if (bomDetail.BackFlushMethod == BusinessConstants.CODE_MASTER_BACKFLUSH_METHOD_VALUE_BATCH_FEED
                                || bomDetail.BackFlushMethod == BusinessConstants.CODE_MASTER_BACKFLUSH_METHOD_VALUE_BATCH_FEED_GR)
                            {
                                bomDetail.DefaultLocation = flowDetail.DefaultLocationFrom;
                                batchFeedBomDetailList.Add(bomDetail);
                            }
                        }
                    }
                }

                return batchFeedBomDetailList;
            }
            return null;
        }


        [Transaction(TransactionMode.Unspecified)]
        public FlowView CheckAndLoadFlowView(string flowCode, string userCode, string locationFromCode, string locationToCode, Hu hu, List<string> flowTypes)
        {
            Item item = hu.Item;
            item.Uom = hu.Uom;
            item.UnitCount = hu.UnitCount;
            return CheckAndLoadFlowView(flowCode, userCode, locationFromCode, locationToCode, item, flowTypes);

        }

        [Transaction(TransactionMode.Unspecified)]
        public FlowView CheckAndLoadFlowView(string flowCode, string userCode, string locationFromCode, string locationToCode, Item item, List<string> flowTypes)
        {
            //按物料号,单位,单包装
            FlowView flowView = LoadFlowView(flowCode, userCode, locationFromCode, locationToCode, item, flowTypes);

            //如果允许新建明细
            if (flowView == null)
            {
                IList<Flow> flowList = this.GetFlows(flowCode, locationFromCode, locationToCode, true, userCode, flowTypes);
                if (flowList != null && flowList.Count > 0)
                {
                    flowView = new FlowView();
                    flowView.Flow = flowList[0];
                    FlowDetail flowDetail = new FlowDetail();
                    flowDetail.Item = item;
                    flowDetail.Uom = item.Uom;
                    flowDetail.UnitCount = item.UnitCount;
                    flowView.LocationFrom = flowView.Flow.LocationFrom;
                    flowView.LocationTo = flowView.Flow.LocationTo;
                }
            }
            if (flowView != null)
            {
                return flowView;
            }
            else
            {
                throw new BusinessErrorException("Flow.Error.NotFoundMacthFlow", flowCode);
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public FlowView LoadFlowView(string flowCode, Item item)
        {
            //按物料号,单位,单包装
            FlowView flowView = LoadFlowView(flowCode, null, null, null, item, null);

            //如果允许新建明细
            if (flowView == null)
            {
                IList<Flow> flowList = this.GetFlows(flowCode, null, null, true, null, null);
                if (flowList != null && flowList.Count > 0)
                {
                    flowView = new FlowView();
                    flowView.Flow = flowList[0];
                    FlowDetail flowDetail = new FlowDetail();
                    flowDetail.Item = item;
                    flowDetail.Uom = item.Uom;
                    flowDetail.UnitCount = item.UnitCount;
                    flowView.LocationFrom = flowView.Flow.LocationFrom;
                    flowView.LocationTo = flowView.Flow.LocationTo;
                }
            }
            return flowView;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<String> GetProductionFlowCode(string itemCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For<FlowDetail>();

            criteria.CreateAlias("Flow", "f");
            criteria.CreateAlias("Item", "i");

            criteria.Add(Expression.Eq("i.Code", itemCode));
            criteria.Add(Expression.Eq("f.Type", BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PRODUCTION));
            criteria.Add(Expression.Eq("f.IsActive", true));

            criteria.SetProjection(Projections.ProjectionList().Add(Projections.GroupProperty("f.Code")));

            return this.criteriaMgrE.FindAll<string>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Flow> GetFlowListForMushengRequire(string userCode)
        {
            IList<Flow> purFlow = new List<Flow>();
            IList<Flow> saleFlow = new List<Flow>();

            User user = this.userMgr.LoadUser(userCode, false, true);

            DetachedCriteria criteria = null;

            List<Permission> suppliers = user.OrganizationPermission.Where(p => (p.Category.Code == BusinessConstants.CODE_MASTER_PERMISSION_CATEGORY_TYPE_VALUE_SUPPLIER)).ToList();
            List<Permission> customers = user.OrganizationPermission.Where(p => (p.Category.Code == BusinessConstants.CODE_MASTER_PERMISSION_CATEGORY_TYPE_VALUE_CUSTOMER)).ToList();

            if (suppliers != null && suppliers.Count() > 0)
            {
                criteria = DetachedCriteria.For<Flow>();
                criteria.CreateAlias("PartyFrom", "pf");

                criteria.Add(Expression.In("pf.Code", suppliers.Select(p => p.Code).ToList()));

                List<string> flowTypes = new List<string>() 
                    {BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT
                        ,BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING};

                criteria.Add(Expression.In("Type", flowTypes));

                criteria.Add(Expression.Eq("IsActive", true));

                purFlow = this.criteriaMgrE.FindAll<Flow>(criteria);
            }

            if (customers != null && customers.Count() > 0)
            {
                criteria = DetachedCriteria.For<Flow>();
                criteria.CreateAlias("PartyFrom", "pf");

                criteria.Add(Expression.In("pf.Code", customers.Select(p => p.Code).ToList()));

                criteria.Add(Expression.Eq("Type", BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS));

                criteria.Add(Expression.Eq("IsActive", true));

                saleFlow = this.criteriaMgrE.FindAll<Flow>(criteria);
            }

            return purFlow.Concat(saleFlow).OrderBy(p => p.Code).ToList();

        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Flow> GetFlowListForMushengRequirePurOnly(string userCode)
        {
            IList<Flow> purFlow = new List<Flow>();

            User user = this.userMgr.LoadUser(userCode, false, true);

            DetachedCriteria criteria = null;

            List<Permission> suppliers = user.OrganizationPermission.Where(p => (p.Category.Code == BusinessConstants.CODE_MASTER_PERMISSION_CATEGORY_TYPE_VALUE_SUPPLIER)).ToList();

            if (suppliers != null && suppliers.Count() > 0)
            {
                criteria = DetachedCriteria.For<Flow>();
                criteria.CreateAlias("PartyFrom", "pf");

                criteria.Add(Expression.In("pf.Code", suppliers.Select(p => p.Code).ToList()));

                List<string> flowTypes = new List<string>() 
                    {BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_PROCUREMENT
                        ,BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_SUBCONCTRACTING};

                criteria.Add(Expression.In("Type", flowTypes));

                criteria.Add(Expression.Eq("IsActive", true));

                purFlow = this.criteriaMgrE.FindAll<Flow>(criteria);
            }

            return purFlow;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Flow> GetFlowListForMushengRequireCustOnly(string userCode)
        {
            IList<Flow> saleFlow = new List<Flow>();

            User user = this.userMgr.LoadUser(userCode, false, true);

            DetachedCriteria criteria = null;

            List<Permission> customers = user.OrganizationPermission.Where(p => (p.Category.Code == BusinessConstants.CODE_MASTER_PERMISSION_CATEGORY_TYPE_VALUE_CUSTOMER)).ToList();

            if (customers != null && customers.Count() > 0)
            {
                criteria = DetachedCriteria.For<Flow>();
                criteria.CreateAlias("PartyFrom", "pf");

                criteria.Add(Expression.In("pf.Code", customers.Select(p => p.Code).ToList()));

                criteria.Add(Expression.Eq("Type", BusinessConstants.CODE_MASTER_FLOW_TYPE_VALUE_CUSTOMERGOODS));

                criteria.Add(Expression.Eq("IsActive", true));

                saleFlow = this.criteriaMgrE.FindAll<Flow>(criteria);
            }

            return saleFlow;
        }
        #endregion Customized Methods

        #region Private Method

        [Transaction(TransactionMode.Unspecified)]
        public FlowView LoadFlowView(string flowCode, string userCode, string locationFromCode, string locationToCode, Item item, List<string> flowTypes)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(FlowView));
            criteria.CreateAlias("Flow", "f");
            criteria.CreateAlias("FlowDetail", "fd");
            if (flowTypes != null && flowTypes.Count > 0)
            {
                if (flowTypes.Count == 1)
                {
                    criteria.Add(Expression.Eq("f.Type", flowTypes[0]));
                }
                else
                {
                    criteria.Add(Expression.In("f.Type", flowTypes));
                }
            }
            //权限待处理
            //if (userCode != null && userCode.Trim() != string.Empty)
            //{
            //    SecurityHelper.SetPartySearchCriteria(criteria, "PartyFromCode", userCode);
            //    SecurityHelper.SetPartySearchCriteria(criteria, "PartyToCode", userCode);
            //}
            if (locationFromCode != null && locationFromCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("LocationFrom.Code", locationFromCode));
            }
            if (locationToCode != null && locationToCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("LocationTo.Code", locationToCode));
            }
            if (flowCode != null && flowCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("f.Code", flowCode));
            }
            //if (item.Uom != null && item.Uom.Code.Trim() != string.Empty)
            //{
            //    criteria.Add(Expression.Eq("fd.Uom.Code", item.Uom.Code));
            //}
            criteria.Add(Expression.Eq("fd.Item.Code", item.Code));
            //if (!allowCreateDetail)
            //{
            //    criteria.Add(Expression.Eq("ItemCode", hu.Item.Code));
            //    criteria.Add(Expression.Eq("UomCode", hu.Uom.Code));
            //    if (isMatchUnitCount)
            //    {
            //        criteria.Add(Expression.Eq("UnitCount", hu.UnitCount));
            //    }
            //}
            //criteria.Add(Expression.Eq("AllowCreateDetail", allowCreateDetail));

            IList<FlowView> list = criteriaMgrE.FindAll<FlowView>(criteria);

            if (list != null && list.Count > 0)
            {
                //按物料号,单位,单包装严格匹配
                var query = list.Where(f => f.FlowDetail.Uom.Code == item.Uom.Code && f.FlowDetail.UnitCount == item.UnitCount);
                if (query.Count() == 1)
                {
                    return query.ToList()[0];
                }
                else
                {
                    query = list.Where(f => f.FlowDetail.UnitCount == item.UnitCount);
                    if (query.Count() > 0)
                    {
                        //按物料号,单包装严格匹配
                        return query.ToList()[0];
                    }
                    query = list.Where(f => f.FlowDetail.Uom.Code == item.Uom.Code);
                    if (query.Count() > 0)
                    {
                        //按物料号,单位严格匹配
                        return query.ToList()[0];
                    }
                }
                //按物料号匹配
                return list[0];
            }
            else
            {
                return null;
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Flow> GetFlows(string locationFromCode, string locationToCode, bool allowCreateDetail, string userCode, List<string> flowTypes)
        {
            return this.GetFlows(null, locationFromCode, locationToCode, allowCreateDetail, userCode, flowTypes);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Flow> GetFlows(string flowCode, string locationFromCode, string locationToCode, bool allowCreateDetail, string userCode, List<string> flowTypes)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Flow));
            if (flowTypes != null && flowTypes.Count > 0)
            {
                if (flowTypes.Count == 1)
                {
                    criteria.Add(Expression.Eq("Type", flowTypes[0]));
                }
                else
                {
                    criteria.Add(Expression.In("Type", flowTypes));
                }
            }
            //权限待处理
            //if (userCode != null && userCode.Trim() != string.Empty)
            //{
            //    SecurityHelper.SetPartySearchCriteria(criteria, "PartyFrom.Code", userCode);
            //    SecurityHelper.SetPartySearchCriteria(criteria, "PartyTo.Code", userCode);
            //}
            if (flowCode != null && flowCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Code", flowCode));
            }
            if (locationFromCode != null && locationFromCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("LocationFrom.Code", locationFromCode));
            }
            if (locationToCode != null && locationToCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("LocationTo.Code", locationToCode));
            }
            criteria.Add(Expression.Eq("AllowCreateDetail", allowCreateDetail));

            return criteriaMgrE.FindAll<Flow>(criteria);
        }


        public IList<Item> GetFlowItems(string flowCode, string items)
        {
            if (flowCode == null || flowCode.Trim() == string.Empty)
            {
                return null;
            }

            string[] itemArray = items.Split('|');

            return GetFlowViews(flowCode, null).Select(f => f.FlowDetail.Item).Where(i => !itemArray.Contains(i.Code)).ToList();
        }

        private IList<FlowView> GetFlowViews(string flowCode, string flowType)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(FlowView));
            criteria.Add(Expression.Eq("Flow.Code", flowCode));
            if (flowType != null && flowType.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Flow.Type", flowType));
            }
            IList<FlowView> flowViews = criteriaMgrE.FindAll<FlowView>(criteria);

            return flowViews;
        }
        #endregion

        [Transaction(TransactionMode.Unspecified)]
        public Flow LoadFlow(string code, User user, bool includeFlowDetail, bool includeRefDetail)
        {
            Flow flow = this.LoadFlow(code);

            if (flow == null)
            {
                return null;
            }

            if (user != null)
            {
                SecurityHelper.CheckPermission(flow.Type, flow.PartyFrom.Code, flow.PartyTo.Code, user);
            }
            if (includeFlowDetail && flow != null && flow.FlowDetails != null && flow.FlowDetails.Count > 0)
            {
            }

            if (includeRefDetail && flow != null && flow.ReferenceFlow != null && flow.ReferenceFlow.Trim() != string.Empty)
            {
                flow.FlowDetails = flowDetailMgrE.GetFlowDetail(flow.ReferenceFlow, includeRefDetail);
            }

            if (flow != null)
            {
                flow.FlowDetails = flow.FlowDetails.OrderBy(f => f.Sequence).ThenBy(f => f.Item.Code).ToList();
            }
            return flow;
        }

        [Transaction(TransactionMode.Unspecified)]
        public Flow CheckAndLoadFlow(string code, User user)
        {
            return CheckAndLoadFlow(code, user, false, false);
        }

        [Transaction(TransactionMode.Unspecified)]
        public Flow CheckAndLoadFlow(string code, User user, bool includeFlowDetail, bool includeRefDetail)
        {
            Flow flow = this.LoadFlow(code, true);

            if (flow == null)
            {
                throw new BusinessErrorException("Flow.Error.FlowCodeNotExist", code);
            }

            if (user != null)
            {
                SecurityHelper.CheckPermission(flow.Type, flow.PartyFrom.Code, flow.PartyTo.Code, user);
            }

            if (includeFlowDetail && flow != null && flow.FlowDetails != null && flow.FlowDetails.Count > 0)
            {
            }

            if (includeRefDetail && flow != null && flow.ReferenceFlow != null && flow.ReferenceFlow.Trim() != string.Empty)
            {
                flow.FlowDetails = flowDetailMgrE.GetFlowDetail(flow.ReferenceFlow, includeRefDetail);
            }

            return flow;
        }

        public IList<string> GetFlowItem(string flowCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For<Flow>();
            criteria.Add(Expression.Eq("Code", flowCode));

            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.GroupProperty("ReferenceFlow").As("ReferenceFlow"))
                .Add(Projections.GroupProperty("IsActive").As("IsActive")));

            var referenceFlows = this.criteriaMgrE.FindAll<object[]>(criteria);
            List<string> flowItem = new List<string>();
            if ((bool)(referenceFlows[0][1]) == false)
            {
                return flowItem.ToList();
            }

            DateTime dateTimeNow = DateTime.Now;
            criteria = DetachedCriteria.For<FlowDetail>();
            
            if (referenceFlows != null && referenceFlows.Count() > 0)
            {
                criteria.Add(Expression.Or(Expression.Eq("Flow.Code", flowCode), Expression.Eq("Flow.Code", referenceFlows[0][0])));
                criteria.Add(Expression.Or(Expression.Le("StartDate", dateTimeNow), Expression.IsNull("StartDate")));
                criteria.Add(Expression.Or(Expression.Ge("EndDate", dateTimeNow), Expression.IsNull("EndDate")));
                criteria.SetProjection(Projections.ProjectionList().Add(
                    Projections.Distinct(Projections.GroupProperty("Item.Code"))));
            }
            else
            {
                criteria.Add(Expression.Eq("Flow.Code", flowCode));
                criteria.Add(Expression.Or(Expression.Le("StartDate", dateTimeNow), Expression.IsNull("StartDate")));
                criteria.Add(Expression.Or(Expression.Ge("EndDate", dateTimeNow), Expression.IsNull("EndDate")));
                criteria.SetProjection(Projections.ProjectionList().Add(
                    Projections.Distinct(Projections.GroupProperty("Item.Code"))));
            }

            return this.criteriaMgrE.FindAll<string>(criteria);
        }
    }
}

class PermissionComparer : IEqualityComparer<Permission>
{
    public bool Equals(Permission x, Permission y)
    {
        return x.Code == y.Code;
    }

    public int GetHashCode(Permission obj)
    {
        string hCode = obj.Code;
        return hCode.GetHashCode();
    }
}


#region Extend Class






namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class FlowMgrE : com.Sconit.Service.MasterData.Impl.FlowMgr, IFlowMgrE
    {

    }
}
#endregion
